import { defineStore } from 'pinia'
import { ref, watch } from 'vue'
import { MOCK_AUTH_USER } from '@/constants/mockUser'
import { ApiError } from '@/services/apiClient'
import {
  createExercise as createExerciseRequest,
  createRoutine as createRoutineRequest,
  createWorkout,
  deleteRoutine as deleteRoutineRequest,
  getExercises,
  getRoutines,
  getWorkouts,
} from '@/services/workoutService'
import type {
  ExerciseListItemDto,
  RoutineListItemDto,
  WorkoutListItemDto,
} from '@/types/workoutApi.types'

// ─── Interfaces ────────────────────────────────────────────────────────────────

export interface Exercise {
  id: string
  name: string
  muscleGroup: string
}

export interface CompletedSet {
  setNumber: number
  kg: number
  reps: number
  isPR?: boolean
}

export interface CompletedExercise {
  id: string
  name: string
  muscleGroup: string
  hasPR?: boolean
  sets: CompletedSet[]
}

export interface Routine {
  id: string
  name: string
  exerciseCount: number
  targetAreas: string[]
  lastPerformed?: string
  exercises: Exercise[]
}

export interface SetItem {
  setNumber: number
  kg: number | null
  reps: number | null
  completed: boolean
  previousKg?: number
  previousReps?: number
}

export interface ActiveSessionExercise {
  id: string
  name: string
  muscleGroup: string
  sets: SetItem[]
}

export interface ActiveSession {
  routineId: string
  routineName: string
  startTime: Date
  currentExerciseIndex: number
  exercises: ActiveSessionExercise[]
}

export interface HistoricalSession {
  id: string
  routineName: string
  date: Date
  time: string
  durationMin: number
  totalVolumeKg: number
  totalSets: number
  exercises: CompletedExercise[]
}

export interface PersonalRecord {
  weight: number
  reps: number
}

export interface CreateRoutinePayload {
  name: string
  exerciseIds: string[]
}

export interface CreateExercisePayload {
  name: string
  muscleGroup: string
}

type StoredHistoricalSession = Omit<HistoricalSession, 'date'> & {
  date: string
}

type StoredActiveSession = Omit<ActiveSession, 'startTime'> & {
  startTime: string
}

type LegacyStoredActiveSession = {
  routineId: string
  routineName: string
  startTime: string
  currentExerciseIndex: number
  sets?: SetItem[]
  completedExercises?: CompletedExercise[]
}

// ─── Store ─────────────────────────────────────────────────────────────────────

export const useWorkoutStore = defineStore('workout', () => {
  // ── State ──────────────────────────────────────────────────────────────────

  const routines = ref<Routine[]>([])
  const exercises = ref<Exercise[]>([])
  const isLoading = ref(false)
  const errorMessage = ref<string | null>(null)
  const currentUserId = ref<string>(MOCK_AUTH_USER.id)

  // ── Parsers (restore Date objects lost during JSON serialization) ────────

  function parseHistoricalSession(raw: StoredHistoricalSession): HistoricalSession {
    return { ...raw, date: new Date(raw.date) }
  }

  function parseActiveSession(raw: StoredActiveSession | LegacyStoredActiveSession): ActiveSession {
    const startTime = new Date(raw.startTime)

    if ('exercises' in raw && Array.isArray(raw.exercises)) {
      return { ...raw, startTime }
    }

    return {
      routineId: raw.routineId,
      routineName: raw.routineName,
      startTime,
      currentExerciseIndex: raw.currentExerciseIndex ?? 0,
      exercises: [],
    }
  }

  const storedHistory = JSON.parse(localStorage.getItem('gym_history') ?? '[]') as StoredHistoricalSession[]
  const history = ref<HistoricalSession[]>(storedHistory.map(parseHistoricalSession))

  const storedSession = JSON.parse(localStorage.getItem('gym_active_session') ?? 'null') as
    | StoredActiveSession
    | LegacyStoredActiveSession
    | null
  const activeSession = ref<ActiveSession | null>(
    storedSession ? parseActiveSession(storedSession) : null,
  )

  // ── Helpers ────────────────────────────────────────────────────────────────

  function toErrorMessage(error: unknown): string {
    if (error instanceof ApiError) {
      return error.message
    }

    if (error instanceof Error) {
      return error.message
    }

    return 'Unexpected error while processing the request.'
  }

  function updateErrorState(error: unknown): void {
    errorMessage.value = toErrorMessage(error)
  }

  function clearErrorState(): void {
    errorMessage.value = null
  }

  function buildInitialSets(previousSets: CompletedSet[] = []): SetItem[] {
    const totalSets = Math.max(3, previousSets.length)

    return Array.from({ length: totalSets }, (_, index) => {
      const previousSet = previousSets[index]

      return {
        setNumber: index + 1,
        kg: null,
        reps: null,
        completed: false,
        previousKg: previousSet?.kg,
        previousReps: previousSet?.reps,
      }
    })
  }

  function getLastExercisePerformance(exerciseId: string): CompletedSet[] {
    const lastSession = history.value.find((session) =>
      session.exercises.some((exercise) => exercise.id === exerciseId),
    )

    const exerciseFromLastSession = lastSession?.exercises.find((exercise) => exercise.id === exerciseId)

    if (!exerciseFromLastSession) {
      return []
    }

    return [...exerciseFromLastSession.sets].sort((a, b) => a.setNumber - b.setNumber)
  }

  function buildInitialSetsForExercise(exerciseId: string): SetItem[] {
    return buildInitialSets(getLastExercisePerformance(exerciseId))
  }

  function getPersonalRecord(exerciseId: string): PersonalRecord | null {
    let bestRecord: PersonalRecord | null = null

    history.value.forEach((session) => {
      session.exercises
        .filter((exercise) => exercise.id === exerciseId)
        .forEach((exercise) => {
          exercise.sets.forEach((set) => {
            if (!bestRecord) {
              bestRecord = { weight: set.kg, reps: set.reps }
              return
            }

            if (set.kg > bestRecord.weight || (set.kg === bestRecord.weight && set.reps > bestRecord.reps)) {
              bestRecord = { weight: set.kg, reps: set.reps }
            }
          })
        })
    })

    return bestRecord
  }

  function formatRelativeDate(date: Date): string {
    const now = new Date()
    const diffMs = now.getTime() - date.getTime()
    const dayMs = 24 * 60 * 60 * 1000
    const diffDays = Math.floor(diffMs / dayMs)

    if (diffDays <= 0) return 'Today'
    if (diffDays === 1) return 'Yesterday'
    if (diffDays < 7) return `${diffDays} days ago`

    const diffWeeks = Math.floor(diffDays / 7)
    if (diffWeeks === 1) return '1 week ago'
    return `${diffWeeks} weeks ago`
  }

  function mapExerciseDto(dto: ExerciseListItemDto): Exercise {
    return {
      id: dto.id,
      name: dto.name,
      muscleGroup: dto.targetMuscle,
    }
  }

  function mapRoutineDto(dto: RoutineListItemDto, exercisesById: Map<string, ExerciseListItemDto>): Routine {
    const mappedExercises: Exercise[] = dto.exercises
      .sort((a, b) => a.sequenceOrder - b.sequenceOrder)
      .map((exercise) => ({
        id: exercise.exerciseId,
        name: exercise.exerciseName,
        muscleGroup: exercisesById.get(exercise.exerciseId)?.targetMuscle ?? 'Unknown',
      }))

    const targetAreas = [...new Set(mappedExercises.map((exercise) => exercise.muscleGroup))]

    return {
      id: dto.id,
      name: dto.name,
      exerciseCount: mappedExercises.length,
      targetAreas,
      exercises: mappedExercises,
      lastPerformed: undefined,
    }
  }

  function mapWorkoutDtoToHistory(
    dto: WorkoutListItemDto,
    routinesById: Map<string, Routine>,
    exercisesById: Map<string, ExerciseListItemDto>,
  ): HistoricalSession {
    const startedAt = new Date(dto.startedAt)
    const completedByExercise = new Map<string, CompletedExercise>()

    dto.sets.forEach((set) => {
      const existing = completedByExercise.get(set.exerciseId)

      if (existing) {
        existing.sets.push({
          setNumber: existing.sets.length + 1,
          kg: Number(set.weight),
          reps: set.reps,
        })
        return
      }

      completedByExercise.set(set.exerciseId, {
        id: set.exerciseId,
        name: set.exerciseName,
        muscleGroup: exercisesById.get(set.exerciseId)?.targetMuscle ?? 'Unknown',
        sets: [
          {
            setNumber: 1,
            kg: Number(set.weight),
            reps: set.reps,
          },
        ],
      })
    })

    const totalVolumeKg = dto.sets.reduce((total, set) => total + Number(set.weight) * set.reps, 0)
    const routineName = dto.routineId ? (routinesById.get(dto.routineId)?.name ?? 'Custom Workout') : 'Custom Workout'

    return {
      id: dto.id,
      routineName,
      date: startedAt,
      time: startedAt.toLocaleTimeString('en-US', {
        hour: '2-digit',
        minute: '2-digit',
        hour12: true,
      }),
      durationMin: 0,
      totalVolumeKg,
      totalSets: dto.sets.length,
      exercises: [...completedByExercise.values()],
    }
  }

  function applyLastPerformedMetadata(
    routineList: Routine[],
    workoutHistory: HistoricalSession[],
  ): Routine[] {
    const lastSessionByRoutineName = new Map<string, Date>()

    workoutHistory.forEach((session) => {
      const current = lastSessionByRoutineName.get(session.routineName)
      if (!current || session.date > current) {
        lastSessionByRoutineName.set(session.routineName, session.date)
      }
    })

    return routineList.map((routine) => {
      const lastSession = lastSessionByRoutineName.get(routine.name)

      return {
        ...routine,
        lastPerformed: lastSession ? formatRelativeDate(lastSession) : undefined,
      }
    })
  }

  async function ensureCurrentUserId(): Promise<string> {
    return currentUserId.value
  }

  // ── Actions ────────────────────────────────────────────────────────────────

  async function loadExercises(): Promise<void> {
    isLoading.value = true
    clearErrorState()

    try {
      const exercisesResponse = await getExercises()
      exercises.value = exercisesResponse.map(mapExerciseDto)
    } catch (error) {
      updateErrorState(error)
    } finally {
      isLoading.value = false
    }
  }

  async function loadRoutinesFromBackend(): Promise<void> {
    isLoading.value = true
    clearErrorState()

    try {
      const [routinesResponse, exercisesResponse] = await Promise.all([
        getRoutines(),
        getExercises(),
      ])

      exercises.value = exercisesResponse.map(mapExerciseDto)
      const exercisesById = new Map(exercisesResponse.map((exercise) => [exercise.id, exercise]))
      const mappedRoutines = routinesResponse.map((routine) => mapRoutineDto(routine, exercisesById))

      routines.value = applyLastPerformedMetadata(mappedRoutines, history.value)
    } catch (error) {
      updateErrorState(error)
    } finally {
      isLoading.value = false
    }
  }

  async function loadRemoteData(): Promise<void> {
    isLoading.value = true
    clearErrorState()

    try {
      const [routinesResponse, workoutsResponse, exercisesResponse] = await Promise.all([
        getRoutines(),
        getWorkouts(),
        getExercises(),
      ])

      exercises.value = exercisesResponse.map(mapExerciseDto)
      const exercisesById = new Map(exercisesResponse.map((exercise) => [exercise.id, exercise]))

      const mappedRoutines = routinesResponse.map((routine) => mapRoutineDto(routine, exercisesById))
      const routinesById = new Map(mappedRoutines.map((routine) => [routine.id, routine]))

      const mappedHistory = workoutsResponse.map((workout) =>
        mapWorkoutDtoToHistory(workout, routinesById, exercisesById),
      )

      history.value = mappedHistory
      routines.value = applyLastPerformedMetadata(mappedRoutines, mappedHistory)
    } catch (error) {
      updateErrorState(error)
    } finally {
      isLoading.value = false
    }
  }

  async function createRoutine(payload: CreateRoutinePayload): Promise<string | null> {
    isLoading.value = true
    clearErrorState()

    try {
      const userId = await ensureCurrentUserId()
      const name = payload.name.trim()

      if (!name) {
        throw new Error('Routine name is required.')
      }

      const routineId = await createRoutineRequest({
        userId,
        name,
        exerciseIds: payload.exerciseIds,
      })

      await loadRoutinesFromBackend()
      return routineId
    } catch (error) {
      updateErrorState(error)
      return null
    } finally {
      isLoading.value = false
    }
  }

  async function createExercise(payload: CreateExercisePayload): Promise<string | null> {
    isLoading.value = true
    clearErrorState()

    try {
      const userId = await ensureCurrentUserId()
      const name = payload.name.trim()
      const targetMuscle = payload.muscleGroup.trim()

      if (!name) {
        throw new Error('Exercise name is required.')
      }

      if (!targetMuscle) {
        throw new Error('Exercise category is required.')
      }

      const exerciseId = await createExerciseRequest({
        name,
        targetMuscle,
        userId,
      })

      await loadExercises()
      return exerciseId
    } catch (error) {
      updateErrorState(error)
      return null
    } finally {
      isLoading.value = false
    }
  }

  async function deleteRoutine(id: string): Promise<void> {
    isLoading.value = true
    clearErrorState()

    try {
      await deleteRoutineRequest(id)
      await loadRoutinesFromBackend()
    } catch (error) {
      updateErrorState(error)
    } finally {
      isLoading.value = false
    }
  }

  async function startWorkout(routineId: string): Promise<void> {
    isLoading.value = true
    clearErrorState()

    try {
      if (routines.value.length === 0) {
        await loadRemoteData()
      }

      const routine = routines.value.find((r) => r.id === routineId)
      if (!routine) {
        throw new Error('Routine not found.')
      }

      const sessionExercises: ActiveSessionExercise[] = routine.exercises.map((exercise) => ({
        id: exercise.id,
        name: exercise.name,
        muscleGroup: exercise.muscleGroup,
        sets: buildInitialSetsForExercise(exercise.id),
      }))

      activeSession.value = {
        routineId: routine.id,
        routineName: routine.name,
        startTime: new Date(),
        currentExerciseIndex: 0,
        exercises: sessionExercises,
      }
    } catch (error) {
      updateErrorState(error)
    } finally {
      isLoading.value = false
    }
  }

  async function cancelWorkout(): Promise<void> {
    isLoading.value = true
    clearErrorState()

    try {
      activeSession.value = null
    } catch (error) {
      updateErrorState(error)
    } finally {
      isLoading.value = false
    }
  }

  async function finishWorkout(durationMin: number, totalVolumeKg: number, totalSets: number): Promise<void> {
    if (!activeSession.value) return

    isLoading.value = true
    clearErrorState()

    try {
      const sessionSnapshot = activeSession.value
      const userId = await ensureCurrentUserId()

      const completedExercises: CompletedExercise[] = sessionSnapshot.exercises
        .map((exercise) => {
          const completedSets: CompletedSet[] = exercise.sets
            .filter((set) => set.completed && set.kg !== null && set.reps !== null)
            .map((set) => ({
              setNumber: set.setNumber,
              kg: Number(set.kg),
              reps: Number(set.reps),
              isPR: false,
            }))

          return {
            id: exercise.id,
            name: exercise.name,
            muscleGroup: exercise.muscleGroup,
            hasPR: false,
            sets: completedSets,
          }
        })
        .filter((exercise) => exercise.sets.length > 0)

      const payloadSets = completedExercises.flatMap((exercise) =>
        exercise.sets.map((set) => ({
          exerciseId: exercise.id,
          weight: set.kg,
          reps: set.reps,
          rir: null,
        })),
      )

      if (payloadSets.length === 0) {
        throw new Error('Complete at least one set before finishing the workout.')
      }

      const createdId = await createWorkout({
        userId,
        routineId: sessionSnapshot.routineId,
        sets: payloadSets,
      })

      const now = new Date()
      const timeStr = now.toLocaleTimeString('en-US', {
        hour: '2-digit',
        minute: '2-digit',
        hour12: true,
      })

      const newSession: HistoricalSession = {
        id: createdId,
        routineName: sessionSnapshot.routineName,
        date: now,
        time: timeStr,
        durationMin,
        totalVolumeKg,
        totalSets,
        exercises: completedExercises,
      }

      history.value.unshift(newSession)
      routines.value = applyLastPerformedMetadata(routines.value, history.value)
      activeSession.value = null
    } catch (error) {
      updateErrorState(error)
    } finally {
      isLoading.value = false
    }
  }

  // ── Persistence watches ────────────────────────────────────────────────────

  watch(
    history,
    (val) => {
      localStorage.setItem('gym_history', JSON.stringify(val))
    },
    { deep: true },
  )

  watch(
    activeSession,
    (val) => {
      if (val === null) {
        localStorage.removeItem('gym_active_session')
      } else {
        localStorage.setItem('gym_active_session', JSON.stringify(val))
      }
    },
    { deep: true },
  )

  localStorage.setItem('gym_user_id', currentUserId.value)

  void loadRemoteData()

  return {
    routines,
    exercises,
    activeSession,
    history,
    isLoading,
    errorMessage,
    loadExercises,
    loadRoutinesFromBackend,
    loadRemoteData,
    createRoutine,
    createExercise,
    deleteRoutine,
    buildInitialSetsForExercise,
    getPersonalRecord,
    startWorkout,
    cancelWorkout,
    finishWorkout,
  }
})
