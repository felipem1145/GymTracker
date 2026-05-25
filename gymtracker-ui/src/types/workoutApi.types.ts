export interface WorkoutSetDto {
  exerciseId: string
  exerciseName: string
  setNumber: number
  weight: number
  reps: number
  rir: number | null
}

export interface WorkoutListItemDto {
  id: string
  userId: string
  routineId: string | null
  startedAt: string
  sets: WorkoutSetDto[]
}

export interface WorkoutDetailDto {
  id: string
  userId: string
  routineId: string | null
  startedAt: string
  sets: WorkoutSetDto[]
}

export interface CreateWorkoutSetItemDto {
  exerciseId: string
  weight: number
  reps: number
  rir: number | null
}

export interface CreateWorkoutCommandDto {
  userId: string
  routineId: string | null
  sets: CreateWorkoutSetItemDto[]
}

export interface UpdateWorkoutSetItemDto {
  exerciseId: string
  weight: number
  reps: number
  rir: number | null
}

export interface UpdateWorkoutCommandDto {
  userId: string
  routineId: string | null
  sets: UpdateWorkoutSetItemDto[]
}

export interface RoutineExerciseDto {
  exerciseId: string
  exerciseName: string
  sequenceOrder: number
}

export interface RoutineListItemDto {
  id: string
  userId: string
  name: string
  createdAt: string
  exercises: RoutineExerciseDto[]
}

export interface RoutineDetailDto {
  id: string
  userId: string
  name: string
  createdAt: string
  exercises: RoutineExerciseDto[]
}

export interface CreateRoutineCommandDto {
  userId: string
  name: string
  exerciseIds: string[]
}

export interface UpdateRoutineCommandDto {
  userId: string
  name: string
  exerciseIds: string[]
}

export interface ExerciseListItemDto {
  id: string
  name: string
  targetMuscle: string
}

export interface ExerciseDetailDto {
  id: string
  name: string
  targetMuscle: string
}

export interface CreateExerciseCommandDto {
  name: string
  targetMuscle: string
  userId: string
}

export interface UserListItemDto {
  id: string
  name: string
  email: string
  createdAt: string
}

export interface UserDetailDto {
  id: string
  name: string
  email: string
  createdAt: string
}
