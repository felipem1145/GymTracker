import { apiClient } from '@/services/apiClient'
import type {
  CreateExerciseCommandDto,
  CreateRoutineCommandDto,
  CreateWorkoutCommandDto,
  ExerciseDetailDto,
  ExerciseListItemDto,
  RoutineDetailDto,
  RoutineListItemDto,
  UpdateRoutineCommandDto,
  UpdateWorkoutCommandDto,
  UserDetailDto,
  UserListItemDto,
  WorkoutDetailDto,
  WorkoutListItemDto,
} from '@/types/workoutApi.types'

/** Mapea a: WorkoutsController.Get */
export function getWorkouts(): Promise<WorkoutListItemDto[]> {
  return apiClient.get<WorkoutListItemDto[]>('/workouts')
}

/** Mapea a: WorkoutsController.GetById */
export function getWorkoutById(id: string): Promise<WorkoutDetailDto> {
  return apiClient.get<WorkoutDetailDto>(`/workouts/${id}`)
}

/** Mapea a: WorkoutsController.Post */
export function createWorkout(command: CreateWorkoutCommandDto): Promise<string> {
  return apiClient.post<string, CreateWorkoutCommandDto>('/workouts', command)
}

/** Mapea a: WorkoutsController.Put */
export function updateWorkout(id: string, command: UpdateWorkoutCommandDto): Promise<void> {
  return apiClient.put<void, UpdateWorkoutCommandDto>(`/workouts/${id}`, command)
}

/** Mapea a: WorkoutsController.Delete */
export function deleteWorkout(id: string): Promise<void> {
  return apiClient.delete<void>(`/workouts/${id}`)
}

/** Mapea a: RoutinesController.Get */
export function getRoutines(): Promise<RoutineListItemDto[]> {
  return apiClient.get<RoutineListItemDto[]>('/routines')
}

/** Mapea a: RoutinesController.GetById */
export function getRoutineById(id: string): Promise<RoutineDetailDto> {
  return apiClient.get<RoutineDetailDto>(`/routines/${id}`)
}

/** Mapea a: RoutinesController.Post */
export function createRoutine(command: CreateRoutineCommandDto): Promise<string> {
  return apiClient.post<string, CreateRoutineCommandDto>('/routines', command)
}

/** Mapea a: RoutinesController.Put */
export function updateRoutine(id: string, command: UpdateRoutineCommandDto): Promise<void> {
  return apiClient.put<void, UpdateRoutineCommandDto>(`/routines/${id}`, command)
}

/** Mapea a: RoutinesController.Delete */
export function deleteRoutine(id: string): Promise<void> {
  return apiClient.delete<void>(`/routines/${id}`)
}

/** Mapea a: ExercisesController.Get */
export function getExercises(): Promise<ExerciseListItemDto[]> {
  return apiClient.get<ExerciseListItemDto[]>('/exercises')
}

/** Mapea a: ExercisesController.GetById */
export function getExerciseById(id: string): Promise<ExerciseDetailDto> {
  return apiClient.get<ExerciseDetailDto>(`/exercises/${id}`)
}

/** Mapea a: ExercisesController.Post */
export function createExercise(command: CreateExerciseCommandDto): Promise<string> {
  return apiClient.post<string, CreateExerciseCommandDto>('/exercises', command)
}

/** Mapea a: UsersController.Get */
export function getUsers(): Promise<UserListItemDto[]> {
  return apiClient.get<UserListItemDto[]>('/users')
}

/** Mapea a: UsersController.GetById */
export function getUserById(id: string): Promise<UserDetailDto> {
  return apiClient.get<UserDetailDto>(`/users/${id}`)
}
