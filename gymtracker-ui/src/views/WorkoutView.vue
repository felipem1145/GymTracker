<template>
  <div v-if="activeSession" class="min-h-screen bg-background pb-8">
    <!-- Header -->
    <header class="sticky top-0 z-40 bg-background border-b border-border px-4 py-4">
      <div class="flex items-center justify-between">
        <div>
          <h1 class="text-xl font-bold text-foreground">{{ activeSession.routineName }}</h1>
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
      <!-- Exercise Tabs -->
      <div class="mb-6 overflow-x-auto">
        <div class="flex min-w-max gap-2 pb-1">
          <button
            v-for="(exercise, index) in sessionExercises"
            :key="exercise.id"
            @click="goToExercise(index)"
            class="flex items-center gap-2 rounded-full border px-4 py-2 text-sm font-medium transition-colors"
            :class="index === currentExerciseIndex
              ? 'border-primary bg-primary text-primary-foreground'
              : 'border-border bg-secondary/40 text-foreground hover:bg-secondary'"
          >
            <span class="text-xs opacity-80">{{ index + 1 }}</span>
            <span class="max-w-[120px] truncate">{{ exercise.name }}</span>
          </button>
        </div>
      </div>

      <!-- Current Exercise Card -->
      <ExerciseCard
        :exercise="currentExercise"
        :last-performance="lastPerformance"
        :personal-record="currentExercisePR"
      />

      <!-- Sets Table -->
      <div class="bg-card rounded-xl border border-border overflow-hidden mb-6">
        <div class="grid grid-cols-12 gap-2 bg-secondary/50 px-4 py-3 border-b border-border text-xs font-semibold text-muted-foreground uppercase tracking-wider">
          <div class="col-span-1">Set</div>
          <div class="col-span-2">Prev</div>
          <div class="col-span-3">kg</div>
          <div class="col-span-3">Reps</div>
          <div class="col-span-2 text-center">Done</div>
          <div class="col-span-1"></div>
        </div>

        <div v-if="currentExerciseSession" class="divide-y divide-border">
          <div
            v-for="(set, index) in currentExerciseSession.sets"
            :key="set.setNumber"
            class="grid grid-cols-12 gap-2 px-4 py-4 items-center bg-background hover:bg-secondary/30 transition-colors"
            :class="{ 'bg-green-500/10': set.completed }"
          >
            <div class="col-span-1">
              <span class="font-bold text-foreground text-sm">{{ set.setNumber }}</span>
            </div>

            <div class="col-span-2">
              <span class="text-xs text-muted-foreground">
                {{ set.previousKg !== undefined && set.previousReps !== undefined ? `${set.previousKg}x${set.previousReps}` : '—' }}
              </span>
            </div>

            <div class="col-span-3">
              <input
                v-model.number="set.kg"
                type="number"
                placeholder="0"
                class="w-full px-2 py-2 bg-background border border-border rounded text-foreground text-sm placeholder-muted-foreground focus:outline-none focus:ring-2 focus:ring-primary focus:border-transparent"
              />
            </div>

            <div class="col-span-3">
              <input
                v-model.number="set.reps"
                type="number"
                placeholder="0"
                class="w-full px-2 py-2 bg-background border border-border rounded text-foreground text-sm placeholder-muted-foreground focus:outline-none focus:ring-2 focus:ring-primary focus:border-transparent"
              />
            </div>

            <div class="col-span-2 flex justify-center">
              <button
                @click="toggleSetCompletion(index)"
                class="w-8 h-8 rounded-full border-2 flex items-center justify-center transition-all"
                :class="set.completed ? 'bg-green-500 border-green-500' : 'border-border hover:border-primary'"
              >
                <Check v-if="set.completed" class="w-4 h-4 text-white" />
              </button>
            </div>

            <div class="col-span-1 flex justify-center">
              <button
                @click="deleteSet(index)"
                class="w-7 h-7 flex items-center justify-center rounded text-muted-foreground hover:text-red-500 transition-colors"
              >
                <Trash2 class="w-4 h-4" />
              </button>
            </div>
          </div>
        </div>

        <button
          @click="addNewSet"
          class="w-full px-4 py-4 text-primary font-medium text-sm hover:bg-secondary/50 transition-colors border-t border-border flex items-center justify-center gap-2"
        >
          <Plus class="w-4 h-4" />
          Add Set
        </button>
      </div>

      <!-- Navigation Buttons -->
      <div class="flex gap-3 mt-8">
        <button
          @click="previousExercise"
          :disabled="currentExerciseIndex === 0"
          class="flex-1 px-4 py-3 bg-secondary hover:bg-muted text-foreground rounded-lg font-medium transition-colors"
          :class="{ 'opacity-50 cursor-not-allowed hover:bg-secondary': currentExerciseIndex === 0 }"
        >
          Previous
        </button>
        <button
          @click="nextExercise"
          :disabled="currentExerciseIndex >= sessionExercises.length - 1"
          class="flex-1 px-4 py-3 bg-primary hover:bg-opacity-90 text-primary-foreground rounded-lg font-medium transition-colors"
          :class="{ 'opacity-50 cursor-not-allowed hover:bg-primary': currentExerciseIndex >= sessionExercises.length - 1 }"
        >
          Next Exercise
        </button>
      </div>

      <!-- Finish Workout Button -->
      <button
        @click="openFinishDialog"
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

    <!-- Finish Workout Dialog -->
    <FinishWorkoutDialog
      :open="showFinishDialog"
      :is-processing="isFinishing"
      title="Finish Workout?"
      message="Your completed sets will be saved and this session will end."
      confirm-label="Yes, finish"
      cancel-label="Keep training"
      @confirm="confirmFinish"
      @cancel="showFinishDialog = false"
    />

    <ConfirmDialog
      :open="showEmptyWorkoutDialog"
      title="No Sets Logged"
      message="You haven't logged any sets in this workout. If you want to exit without saving, use 'Cancel Workout'."
      confirm-label="Got it"
      cancel-label="Keep training"
      @confirm="showEmptyWorkoutDialog = false"
      @cancel="showEmptyWorkoutDialog = false"
    />
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted, onUnmounted, watch } from 'vue'
import { useRouter } from 'vue-router'
import { Check, Flame, Plus, Trash2 } from '@lucide/vue'
import { storeToRefs } from 'pinia'
import { useWorkoutStore } from '@/stores/workout'
import ExerciseCard from '@/components/ExerciseCard.vue'
import ConfirmDialog from '@/components/ConfirmDialog.vue'
import FinishWorkoutDialog from '@/components/FinishWorkoutDialog.vue'

const router = useRouter()
const workoutStore = useWorkoutStore()
const { activeSession, routines } = storeToRefs(workoutStore)

const showCancelDialog = ref(false)
const showFinishDialog = ref(false)
const showEmptyWorkoutDialog = ref(false)
const isFinishing = ref(false)
const sessionDuration = ref('00:00:00')
let timerInterval: ReturnType<typeof setInterval> | null = null

const currentExerciseIndex = computed({
  get: () => activeSession.value?.currentExerciseIndex ?? 0,
  set: (index: number) => {
    if (!activeSession.value) return
    const maxIndex = Math.max(0, activeSession.value.exercises.length - 1)
    activeSession.value.currentExerciseIndex = Math.min(Math.max(index, 0), maxIndex)
  },
})

const sessionExercises = computed(() => activeSession.value?.exercises ?? [])

const currentExercise = computed(() => {
  const ex =
    sessionExercises.value[currentExerciseIndex.value] ??
    sessionExercises.value[0] ?? { id: '', name: '', muscleGroup: '' }
  return { ...ex, icon: 'dumbbell' }
})

const currentExerciseSession = computed(() =>
  sessionExercises.value[currentExerciseIndex.value],
)

const currentSets = computed(() =>
  currentExerciseSession.value?.sets ?? [],
)

const currentExercisePR = computed(() => {
  const exerciseId = currentExerciseSession.value?.id
  if (!exerciseId) return null

  return workoutStore.getPersonalRecord(exerciseId)
})

const lastPerformance = computed(() =>
  currentSets.value
    .filter((set) => set.previousKg !== undefined && set.previousReps !== undefined)
    .map((set) => ({ set: set.setNumber, kg: set.previousKg, reps: set.previousReps })),
)

function hydrateSessionExercisesFromRoutine(): void {
  if (!activeSession.value || activeSession.value.exercises.length > 0) {
    return
  }

  const routine = routines.value.find((r) => r.id === activeSession.value?.routineId)
  if (!routine) {
    return
  }

  activeSession.value.exercises = routine.exercises.map((exercise) => ({
    id: exercise.id,
    name: exercise.name,
    muscleGroup: exercise.muscleGroup,
    sets: workoutStore.buildInitialSetsForExercise(exercise.id),
  }))

  currentExerciseIndex.value = activeSession.value.currentExerciseIndex
}

watch([activeSession, routines], () => {
  hydrateSessionExercisesFromRoutine()
}, { immediate: true })

onMounted(() => {
  if (!workoutStore.activeSession) {
    router.push('/')
    return
  }

  hydrateSessionExercisesFromRoutine()

  timerInterval = setInterval(() => {
    const start = activeSession.value?.startTime
    if (!start) return
    const elapsed = Math.floor((new Date().getTime() - start.getTime()) / 1000)
    const hours = Math.floor(elapsed / 3600)
    const minutes = Math.floor((elapsed % 3600) / 60)
    const seconds = elapsed % 60
    sessionDuration.value = `${String(hours).padStart(2, '0')}:${String(minutes).padStart(2, '0')}:${String(seconds).padStart(2, '0')}`
  }, 1000)
})

onUnmounted(() => {
  if (timerInterval !== null) clearInterval(timerInterval)
})

const goToExercise = (index: number) => {
  currentExerciseIndex.value = index
}

const toggleSetCompletion = (setIndex: number) => {
  if (!activeSession.value) return
  const set = activeSession.value.exercises[currentExerciseIndex.value]?.sets[setIndex]
  if (set) {
    set.completed = !set.completed
  }
}

const addNewSet = () => {
  if (!activeSession.value) return

  const exerciseSets = activeSession.value.exercises[currentExerciseIndex.value]?.sets
  if (!exerciseSets) return

  const newSetNumber = exerciseSets.length + 1
  exerciseSets.push({ setNumber: newSetNumber, kg: null, reps: null, completed: false })
}

const deleteSet = (setIndex: number) => {
  if (!activeSession.value) return

  const exercise = activeSession.value.exercises[currentExerciseIndex.value]
  if (!exercise) return

  exercise.sets = exercise.sets
    .filter((_, index) => index !== setIndex)
    .map((s, idx) => ({ ...s, setNumber: idx + 1 }))
}

const nextExercise = () => {
  if (currentExerciseIndex.value < sessionExercises.value.length - 1) {
    currentExerciseIndex.value += 1
  }
}

const previousExercise = () => {
  if (currentExerciseIndex.value > 0) {
    currentExerciseIndex.value -= 1
  }
}

const endWorkout = () => {
  showCancelDialog.value = true
}

const openFinishDialog = () => {
  showFinishDialog.value = true
}

function saveCurrentExerciseSets(): void {
  // Kept intentionally as a no-op. Set data lives directly in activeSession.exercises.
}

const confirmCancel = async () => {
  await workoutStore.cancelWorkout()
  router.push('/')
}

const confirmFinish = async () => {
  if (isFinishing.value) {
    return
  }

  const hasCompletedSet = sessionExercises.value.some((exercise) =>
    exercise.sets.some((set) => set.completed),
  )

  if (!hasCompletedSet) {
    showFinishDialog.value = false
    showEmptyWorkoutDialog.value = true
    return
  }

  isFinishing.value = true
  await finishWorkout()
  isFinishing.value = false
}

const finishWorkout = async () => {
  if (!activeSession.value) return

  const allSets = sessionExercises.value.flatMap((exercise) =>
    exercise.sets
      .filter((set) => set.completed && set.kg !== null && set.reps !== null)
      .map((set) => ({ kg: Number(set.kg), reps: Number(set.reps) })),
  )

  const totalSets = allSets.length
  const totalVolumeKg = allSets.reduce((acc, s) => acc + s.kg * s.reps, 0)
  const durationMin = Math.max(
    1,
    Math.floor((new Date().getTime() - activeSession.value.startTime.getTime()) / 60000),
  )

  await workoutStore.finishWorkout(durationMin, totalVolumeKg, totalSets)

  if (!workoutStore.activeSession) {
    showFinishDialog.value = false
    router.push('/')
  }
}
</script>
