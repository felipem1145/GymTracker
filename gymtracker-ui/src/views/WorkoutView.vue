<template>
  <div class="min-h-screen bg-background pb-8">
    <!-- Header -->
    <header class="sticky top-0 z-40 bg-background border-b border-border px-4 py-4">
      <div class="flex items-center justify-between">
        <div>
          <h1 class="text-xl font-bold text-foreground">{{ activePlan.name }}</h1>
          <div class="flex items-center gap-2 mt-1">
            <span class="text-sm text-muted-foreground">{{ sessionDuration }}</span>
            <Flame class="w-4 h-4 text-orange-500" />
          </div>
        </div>
        <button
          @click="endWorkout"
          class="px-4 py-2 bg-red-600 hover:bg-red-700 text-white rounded-lg font-medium transition-colors text-sm"
        >
          Cancel Workout
        </button>
      </div>
    </header>

    <!-- Main Content -->
    <main class="px-4 pt-6">
      <!-- Current Exercise Card -->
      <ExerciseCard
        :exercise="currentExercise"
        :last-performance="lastPerformance"
      />

      <!-- Sets Table -->
      <SetsList
        :sets="currentSets"
        @toggle-set="toggleSetCompletion"
        @update-set="updateSet"
        @add-set="addNewSet"
        @delete-set="deleteSet"
      />

      <!-- Navigation Buttons -->
      <div class="flex gap-3 mt-8">
        <button
          @click="previousExercise"
          class="flex-1 px-4 py-3 bg-secondary hover:bg-muted text-foreground rounded-lg font-medium transition-colors"
        >
          Previous
        </button>
        <button
          @click="nextExercise"
          class="flex-1 px-4 py-3 bg-primary hover:bg-opacity-90 text-primary-foreground rounded-lg font-medium transition-colors"
        >
          Next Exercise
        </button>
      </div>

      <!-- Finish Workout Button -->
      <button
        @click="finishWorkout"
        class="w-full mt-4 px-4 py-4 bg-gradient-to-r from-primary to-green-600 hover:opacity-90 text-primary-foreground rounded-lg font-bold text-lg transition-opacity"
      >
        Finish Workout
      </button>
    </main>

    <!-- Cancel Workout Dialog -->
    <ConfirmDialog
      :open="showCancelDialog"
      title="Cancel Workout?"
      message="You'll lose all progress from this session. This action cannot be undone."
      confirm-label="Yes, cancel"
      cancel-label="Keep training"
      @confirm="confirmCancel"
      @cancel="showCancelDialog = false"
    />
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted, onUnmounted } from 'vue'
import { useRouter } from 'vue-router'
import { Flame } from '@lucide/vue'
import ExerciseCard from '@/components/ExerciseCard.vue'
import SetsList from '@/components/SetsList.vue'
import ConfirmDialog from '@/components/ConfirmDialog.vue'

const router = useRouter()

const showCancelDialog = ref(false)

interface Set {
  setNumber: number
  previousKg?: number
  previousReps?: number
  kg: number | null
  reps: number | null
  completed: boolean
}

interface Exercise {
  id: string
  name: string
  muscleGroup: string
  icon: string
}

interface WorkoutPlan {
  id: string
  name: string
  exercises: Exercise[]
}

const activePlan = ref<WorkoutPlan>({
  id: 'push-day',
  name: 'Push Day - Session',
  exercises: [
    { id: '1', name: 'Bench Press', muscleGroup: 'Chest', icon: 'dumbbell' },
    { id: '2', name: 'Incline Dumbbell Press', muscleGroup: 'Chest', icon: 'dumbbell' },
    { id: '3', name: 'Overhead Press', muscleGroup: 'Shoulders', icon: 'arrow-up' },
    { id: '4', name: 'Lateral Raises', muscleGroup: 'Shoulders', icon: 'arrow-up' },
    { id: '5', name: 'Tricep Dips', muscleGroup: 'Arms', icon: 'dumbbell' },
  ],
})

const currentExerciseIndex = ref(0)
const currentSets = ref<Set[]>([
  { setNumber: 1, previousKg: 80, previousReps: 8, kg: 80, reps: null, completed: false },
  { setNumber: 2, previousKg: 80, previousReps: 7, kg: 80, reps: null, completed: false },
  { setNumber: 3, previousKg: 75, previousReps: 10, kg: 75, reps: null, completed: false },
])

const startTime = ref(new Date())
const sessionDuration = ref('00:00:00')
let timerInterval: ReturnType<typeof setInterval> | null = null

const currentExercise = computed<Exercise>(
  () => activePlan.value.exercises[currentExerciseIndex.value] ?? activePlan.value.exercises[0]!,
)

const lastPerformance = computed(() => [
  { set: 1, kg: currentSets.value[0]?.previousKg, reps: currentSets.value[0]?.previousReps },
  { set: 2, kg: currentSets.value[1]?.previousKg, reps: currentSets.value[1]?.previousReps },
  { set: 3, kg: currentSets.value[2]?.previousKg, reps: currentSets.value[2]?.previousReps },
])

onMounted(() => {
  timerInterval = setInterval(() => {
    const elapsed = Math.floor((new Date().getTime() - startTime.value.getTime()) / 1000)
    const hours = Math.floor(elapsed / 3600)
    const minutes = Math.floor((elapsed % 3600) / 60)
    const seconds = elapsed % 60
    sessionDuration.value = `${String(hours).padStart(2, '0')}:${String(minutes).padStart(2, '0')}:${String(seconds).padStart(2, '0')}`
  }, 1000)
})

onUnmounted(() => {
  if (timerInterval !== null) clearInterval(timerInterval)
})

const toggleSetCompletion = (setNumber: number) => {
  const set = currentSets.value.find((s) => s.setNumber === setNumber)
  if (set) set.completed = !set.completed
}

const updateSet = (setNumber: number, field: 'kg' | 'reps', value: number | null) => {
  const set = currentSets.value.find((s) => s.setNumber === setNumber)
  if (set) set[field] = value
}

const addNewSet = () => {
  const newSetNumber = currentSets.value.length + 1
  currentSets.value.push({
    setNumber: newSetNumber,
    kg: null,
    reps: null,
    completed: false,
  })
}

const nextExercise = () => {
  if (currentExerciseIndex.value < activePlan.value.exercises.length - 1) {
    currentExerciseIndex.value++
    currentSets.value = [
      { setNumber: 1, kg: null, reps: null, completed: false },
      { setNumber: 2, kg: null, reps: null, completed: false },
      { setNumber: 3, kg: null, reps: null, completed: false },
    ]
  }
}

const previousExercise = () => {
  if (currentExerciseIndex.value > 0) {
    currentExerciseIndex.value--
  }
}

const endWorkout = () => {
  showCancelDialog.value = true
}

const confirmCancel = () => {
  router.push('/')
}

const deleteSet = (setNumber: number) => {
  currentSets.value = currentSets.value
    .filter((s) => s.setNumber !== setNumber)
    .map((s, idx) => ({ ...s, setNumber: idx + 1 }))
}

const finishWorkout = () => {
  console.log('Finishing workout with sets:', currentSets.value)
}
</script>
