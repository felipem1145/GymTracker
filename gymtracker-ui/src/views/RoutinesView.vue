<template>
  <div class="min-h-screen bg-[#09090b] text-[#fafafa] pb-24">
    <!-- Header -->
    <header class="sticky top-0 z-20 bg-[#09090b]/95 backdrop-blur-sm border-b border-[#27272a] px-4 pt-6 pb-4">
      <div class="flex items-center justify-between mb-4">
        <h1 class="text-2xl font-bold">Routines & Exercises</h1>
        <button class="p-2 rounded-xl bg-[#18181b] hover:bg-[#27272a] transition-colors">
          <Settings class="w-5 h-5 text-[#a1a1aa]" />
        </button>
      </div>

      <!-- Tab Switcher -->
      <TabSwitcher v-model="activeTab" :tabs="tabs" />
    </header>

    <!-- Content -->
    <main class="px-4 pt-4">
      <!-- My Routines Tab -->
      <div v-if="activeTab === 'routines'">
        <button
          @click="openCreateRoutine"
          class="w-full mb-4 py-4 px-6 bg-[#22c55e] hover:bg-[#16a34a] text-[#09090b] font-semibold text-lg rounded-2xl flex items-center justify-center gap-3 transition-all active:scale-[0.98] shadow-lg shadow-[#22c55e]/20"
        >
          <Plus class="w-6 h-6" />
          Create New Routine
        </button>

        <RoutinesList />
      </div>

      <!-- Exercises Tab -->
      <div v-else-if="activeTab === 'exercises'">
        <button
          @click="openCreateExerciseModal"
          class="w-full mb-4 py-4 px-6 bg-[#18181b] hover:bg-[#27272a] border border-[#27272a] text-[#fafafa] font-semibold text-lg rounded-2xl flex items-center justify-center gap-3 transition-all active:scale-[0.98]"
        >
          <Plus class="w-6 h-6 text-[#22c55e]" />
          Add Custom Exercise
        </button>

        <ExercisesView />
      </div>
    </main>

    <div
      v-if="showCreateExerciseModal"
      class="fixed inset-0 z-50 bg-black/70 backdrop-blur-sm flex items-end sm:items-center justify-center px-4"
    >
      <div class="w-full max-w-md rounded-2xl border border-[#27272a] bg-[#09090b] p-4 sm:p-5">
        <div class="flex items-center justify-between mb-4">
          <h2 class="text-lg font-semibold">Add Custom Exercise</h2>
          <button
            type="button"
            class="rounded-lg p-2 hover:bg-[#18181b]"
            @click="closeCreateExerciseModal"
            aria-label="Close"
          >
            <X class="w-4 h-4 text-[#a1a1aa]" />
          </button>
        </div>

        <div class="space-y-4">
          <div>
            <label for="exercise-name" class="text-xs uppercase tracking-wide text-[#a1a1aa]">Exercise Name</label>
            <input
              id="exercise-name"
              v-model="newExerciseName"
              type="text"
              placeholder="e.g. Flat Dumbbell Press"
              class="mt-2 w-full rounded-xl border border-[#27272a] bg-[#18181b] px-4 py-3 text-[#fafafa] placeholder:text-[#71717a] focus:outline-none focus:border-[#22c55e] focus:ring-1 focus:ring-[#22c55e]"
            />
          </div>

          <div>
            <label for="exercise-group" class="text-xs uppercase tracking-wide text-[#a1a1aa]">Muscle Group</label>
            <select
              id="exercise-group"
              v-model="newExerciseMuscleGroup"
              class="mt-2 w-full rounded-xl border border-[#27272a] bg-[#18181b] px-4 py-3 text-[#fafafa] focus:outline-none focus:border-[#22c55e] focus:ring-1 focus:ring-[#22c55e]"
            >
              <option disabled value="">Select a category</option>
              <option v-for="group in muscleGroupOptions" :key="group" :value="group">{{ group }}</option>
            </select>
          </div>

          <p v-if="createExerciseError" class="text-sm text-red-300">{{ createExerciseError }}</p>

          <div class="grid grid-cols-2 gap-3 pt-1">
            <button
              type="button"
              class="rounded-xl border border-[#27272a] bg-[#18181b] px-4 py-3 text-sm font-medium hover:bg-[#27272a]"
              @click="closeCreateExerciseModal"
            >
              Cancel
            </button>
            <button
              type="button"
              :disabled="workoutStore.isLoading"
              class="rounded-xl px-4 py-3 text-sm font-semibold"
              :class="workoutStore.isLoading
                ? 'bg-[#3f3f46] text-[#a1a1aa] cursor-not-allowed'
                : 'bg-[#22c55e] text-[#09090b] hover:bg-[#16a34a]'"
              @click="submitCreateExercise"
            >
              {{ workoutStore.isLoading ? 'Saving...' : 'Save Exercise' }}
            </button>
          </div>
        </div>
      </div>
    </div>

    <!-- Bottom Navigation -->
    <BottomNav />
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { Settings, Plus, X } from '@lucide/vue'
import TabSwitcher from '@/components/TabSwitcher.vue'
import RoutinesList from '@/components/RoutinesList.vue'
import ExercisesView from '@/views/ExercisesView.vue'
import BottomNav from '@/components/BottomNav.vue'
import { useWorkoutStore } from '@/stores/workout'

const router = useRouter()
const workoutStore = useWorkoutStore()

interface Tab {
  id: string
  label: string
  icon: string
}

const activeTab = ref('routines')

const tabs: Tab[] = [
  { id: 'routines', label: 'My Routines', icon: 'folder' },
  { id: 'exercises', label: 'Exercises', icon: 'dumbbell' },
]

const muscleGroupOptions = ['Chest', 'Back', 'Legs', 'Shoulders', 'Arms', 'Core']
const showCreateExerciseModal = ref(false)
const newExerciseName = ref('')
const newExerciseMuscleGroup = ref('')
const createExerciseError = ref<string | null>(null)

function openCreateRoutine(): void {
  router.push('/routines/create')
}

function openCreateExerciseModal(): void {
  createExerciseError.value = null
  showCreateExerciseModal.value = true
}

function closeCreateExerciseModal(): void {
  showCreateExerciseModal.value = false
  newExerciseName.value = ''
  newExerciseMuscleGroup.value = ''
  createExerciseError.value = null
}

async function submitCreateExercise(): Promise<void> {
  createExerciseError.value = null

  if (!newExerciseName.value.trim()) {
    createExerciseError.value = 'Exercise name is required.'
    return
  }

  if (!newExerciseMuscleGroup.value) {
    createExerciseError.value = 'Select a muscle group.'
    return
  }

  const createdExerciseId = await workoutStore.createExercise({
    name: newExerciseName.value,
    muscleGroup: newExerciseMuscleGroup.value,
  })

  if (!createdExerciseId) {
    createExerciseError.value = workoutStore.errorMessage ?? 'Could not save exercise.'
    return
  }

  closeCreateExerciseModal()
}
</script>
