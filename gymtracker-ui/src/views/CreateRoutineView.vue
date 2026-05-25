<template>
  <div class="min-h-screen bg-[#09090b] text-[#fafafa] pb-24">
    <header class="sticky top-0 z-20 bg-[#09090b]/95 backdrop-blur-sm border-b border-[#27272a] px-4 py-4">
      <div class="flex items-center gap-3">
        <button
          @click="router.back()"
          class="h-9 w-9 rounded-xl bg-[#18181b] hover:bg-[#27272a] transition-colors flex items-center justify-center"
          aria-label="Volver"
        >
          <ChevronLeft class="w-5 h-5" />
        </button>
        <div>
          <h1 class="text-xl font-bold">Create New Routine</h1>
          <p class="text-xs text-[#71717a]">Pick exercises and save to backend</p>
        </div>
      </div>
    </header>

    <main class="px-4 pt-5 space-y-4">
      <div class="space-y-2">
        <label for="routine-name" class="text-sm text-[#a1a1aa]">Routine Name</label>
        <input
          id="routine-name"
          v-model="routineName"
          type="text"
          placeholder="e.g. Push Strength"
          class="w-full rounded-xl border border-[#27272a] bg-[#18181b] px-4 py-3 text-[#fafafa] placeholder:text-[#71717a] focus:outline-none focus:border-[#22c55e] focus:ring-1 focus:ring-[#22c55e]"
        />
      </div>

      <div class="rounded-xl border border-[#27272a] bg-[#18181b] p-4" v-if="formError || workoutStore.errorMessage">
        <p class="text-sm text-red-300">{{ formError || workoutStore.errorMessage }}</p>
      </div>

      <div v-if="workoutStore.isLoading && workoutStore.exercises.length === 0" class="py-8 text-center text-sm text-[#a1a1aa]">
        Loading exercises...
      </div>

      <div v-else>
        <div class="mb-3 flex items-center justify-between">
          <h2 class="text-sm font-semibold text-[#e4e4e7]">Available Exercises</h2>
          <span class="text-xs text-[#71717a]">{{ selectedExerciseIds.length }} selected</span>
        </div>

        <div class="relative mb-4">
          <Search class="absolute left-4 top-1/2 -translate-y-1/2 w-5 h-5 text-[#71717a]" />
          <input
            type="text"
            v-model="searchQuery"
            placeholder="Search exercises..."
            class="w-full bg-[#18181b] border border-[#27272a] rounded-xl py-3.5 pl-12 pr-4 text-[#fafafa] placeholder:text-[#71717a] focus:outline-none focus:border-[#22c55e] focus:ring-1 focus:ring-[#22c55e] transition-all"
          />
          <button
            v-if="searchQuery"
            @click="searchQuery = ''"
            class="absolute right-4 top-1/2 -translate-y-1/2 p-1 rounded-full hover:bg-[#27272a] transition-colors"
          >
            <X class="w-4 h-4 text-[#71717a]" />
          </button>
        </div>

        <div class="space-y-2 max-h-[52vh] overflow-auto pr-1">
          <button
            v-for="exercise in filteredExercises"
            :key="exercise.id"
            type="button"
            @click="toggleExerciseSelection(exercise.id)"
            class="w-full rounded-xl border p-3 text-left transition-all flex items-center justify-between"
            :class="selectedExerciseIds.includes(exercise.id)
              ? 'border-[#22c55e] bg-[#14532d]/30'
              : 'border-[#27272a] bg-[#18181b] hover:border-[#3f3f46]'"
          >
            <div>
              <p class="font-medium">{{ exercise.name }}</p>
              <p class="text-xs text-[#a1a1aa]">{{ exercise.muscleGroup }}</p>
            </div>
            <Check class="w-5 h-5" :class="selectedExerciseIds.includes(exercise.id) ? 'text-[#22c55e]' : 'text-[#52525b]'" />
          </button>
        </div>
      </div>

      <button
        type="button"
        :disabled="workoutStore.isLoading"
        @click="saveRoutine"
        class="w-full py-3.5 rounded-xl font-semibold transition-all"
        :class="workoutStore.isLoading
          ? 'bg-[#3f3f46] text-[#a1a1aa] cursor-not-allowed'
          : 'bg-[#22c55e] hover:bg-[#16a34a] text-[#09090b]'"
      >
        {{ workoutStore.isLoading ? 'Saving...' : 'Save Routine' }}
      </button>
    </main>
  </div>
</template>

<script setup lang="ts">
import { computed, onMounted, ref } from 'vue'
import { useRouter } from 'vue-router'
import { Check, ChevronLeft, Search, X } from '@lucide/vue'
import { useWorkoutStore } from '@/stores/workout'

const router = useRouter()
const workoutStore = useWorkoutStore()

const routineName = ref('')
const selectedExerciseIds = ref<string[]>([])
const formError = ref<string | null>(null)
const searchQuery = ref('')

const filteredExercises = computed(() => {
  const selectedIds = new Set(selectedExerciseIds.value)

  if (!searchQuery.value.trim()) {
    return [...workoutStore.exercises].sort((a, b) => {
      const aSelected = selectedIds.has(a.id)
      const bSelected = selectedIds.has(b.id)

      if (aSelected === bSelected) {
        return a.name.localeCompare(b.name)
      }

      return aSelected ? -1 : 1
    })
  }

  const query = searchQuery.value.toLowerCase()
  return workoutStore.exercises
    .filter(
      (exercise) =>
        exercise.name.toLowerCase().includes(query) ||
        exercise.muscleGroup.toLowerCase().includes(query),
    )
    .sort((a, b) => {
      const aSelected = selectedIds.has(a.id)
      const bSelected = selectedIds.has(b.id)

      if (aSelected === bSelected) {
        return a.name.localeCompare(b.name)
      }

      return aSelected ? -1 : 1
    })
})

function toggleExerciseSelection(exerciseId: string): void {
  if (selectedExerciseIds.value.includes(exerciseId)) {
    selectedExerciseIds.value = selectedExerciseIds.value.filter((id) => id !== exerciseId)
    return
  }

  selectedExerciseIds.value = [...selectedExerciseIds.value, exerciseId]
}

async function saveRoutine(): Promise<void> {
  formError.value = null

  if (!routineName.value.trim()) {
    formError.value = 'Routine name is required.'
    return
  }

  if (selectedExerciseIds.value.length === 0) {
    formError.value = 'Select at least one exercise.'
    return
  }

  const routineId = await workoutStore.createRoutine({
    name: routineName.value,
    exerciseIds: selectedExerciseIds.value,
  })

  if (!routineId) {
    return
  }

  routineName.value = ''
  selectedExerciseIds.value = []
  searchQuery.value = ''
  router.push('/routines')
}

onMounted(() => {
  if (workoutStore.exercises.length === 0) {
    void workoutStore.loadExercises()
  }
})
</script>
