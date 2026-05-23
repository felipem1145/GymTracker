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
          class="w-full mb-4 py-4 px-6 bg-[#22c55e] hover:bg-[#16a34a] text-[#09090b] font-semibold text-lg rounded-2xl flex items-center justify-center gap-3 transition-all active:scale-[0.98] shadow-lg shadow-[#22c55e]/20"
        >
          <Plus class="w-6 h-6" />
          Create New Routine
        </button>

        <RoutinesList :routines="routines" />
      </div>

      <!-- Exercises Tab -->
      <div v-else-if="activeTab === 'exercises'">
        <button
          class="w-full mb-4 py-4 px-6 bg-[#18181b] hover:bg-[#27272a] border border-[#27272a] text-[#fafafa] font-semibold text-lg rounded-2xl flex items-center justify-center gap-3 transition-all active:scale-[0.98]"
        >
          <Plus class="w-6 h-6 text-[#22c55e]" />
          Add Custom Exercise
        </button>

        <ExercisesList
          :exercises="filteredExercises"
          v-model:search="searchQuery"
        />
      </div>
    </main>

    <!-- Bottom Navigation -->
    <BottomNav />
  </div>
</template>

<script setup lang="ts">
import { ref, computed } from 'vue'
import { Settings, Plus } from '@lucide/vue'
import TabSwitcher from '@/components/TabSwitcher.vue'
import RoutinesList from '@/components/RoutinesList.vue'
import ExercisesList from '@/components/ExercisesList.vue'
import BottomNav from '@/components/BottomNav.vue'

interface Tab {
  id: string
  label: string
  icon: string
}

interface Routine {
  id: number
  name: string
  targetAreas: string[]
  exerciseCount: number
  lastPerformed?: string
}

interface Exercise {
  id: number
  name: string
  muscleGroup: string
  muscleGroupColor: string
}

const activeTab = ref('routines')

const tabs: Tab[] = [
  { id: 'routines', label: 'My Routines', icon: 'folder' },
  { id: 'exercises', label: 'Exercises', icon: 'dumbbell' },
]

const routines = ref<Routine[]>([
  {
    id: 1,
    name: 'Push Day',
    targetAreas: ['Chest', 'Shoulders', 'Triceps'],
    exerciseCount: 6,
    lastPerformed: '2 days ago',
  },
  {
    id: 2,
    name: 'Pull Day',
    targetAreas: ['Back', 'Biceps', 'Rear Delts'],
    exerciseCount: 7,
    lastPerformed: 'Yesterday',
  },
  {
    id: 3,
    name: 'Leg Day',
    targetAreas: ['Quads', 'Hamstrings', 'Glutes', 'Calves'],
    exerciseCount: 8,
    lastPerformed: '4 days ago',
  },
  {
    id: 4,
    name: 'Upper Body',
    targetAreas: ['Chest', 'Back', 'Shoulders', 'Arms'],
    exerciseCount: 10,
    lastPerformed: '1 week ago',
  },
  {
    id: 5,
    name: 'Core & Abs',
    targetAreas: ['Abs', 'Obliques', 'Lower Back'],
    exerciseCount: 5,
    lastPerformed: '3 days ago',
  },
])

const exercises = ref<Exercise[]>([
  { id: 1, name: 'Bench Press', muscleGroup: 'Chest', muscleGroupColor: '#ef4444' },
  { id: 2, name: 'Incline Dumbbell Press', muscleGroup: 'Chest', muscleGroupColor: '#ef4444' },
  { id: 3, name: 'Cable Flyes', muscleGroup: 'Chest', muscleGroupColor: '#ef4444' },
  { id: 4, name: 'Squat', muscleGroup: 'Legs', muscleGroupColor: '#8b5cf6' },
  { id: 5, name: 'Romanian Deadlift', muscleGroup: 'Legs', muscleGroupColor: '#8b5cf6' },
  { id: 6, name: 'Leg Press', muscleGroup: 'Legs', muscleGroupColor: '#8b5cf6' },
  { id: 7, name: 'Overhead Press', muscleGroup: 'Shoulders', muscleGroupColor: '#f59e0b' },
  { id: 8, name: 'Lateral Raises', muscleGroup: 'Shoulders', muscleGroupColor: '#f59e0b' },
  { id: 9, name: 'Pull-ups', muscleGroup: 'Back', muscleGroupColor: '#3b82f6' },
  { id: 10, name: 'Barbell Rows', muscleGroup: 'Back', muscleGroupColor: '#3b82f6' },
  { id: 11, name: 'Lat Pulldown', muscleGroup: 'Back', muscleGroupColor: '#3b82f6' },
  { id: 12, name: 'Bicep Curls', muscleGroup: 'Arms', muscleGroupColor: '#22c55e' },
  { id: 13, name: 'Tricep Pushdowns', muscleGroup: 'Arms', muscleGroupColor: '#22c55e' },
  { id: 14, name: 'Hammer Curls', muscleGroup: 'Arms', muscleGroupColor: '#22c55e' },
  { id: 15, name: 'Plank', muscleGroup: 'Core', muscleGroupColor: '#ec4899' },
  { id: 16, name: 'Cable Crunches', muscleGroup: 'Core', muscleGroupColor: '#ec4899' },
])

const searchQuery = ref('')

const filteredExercises = computed(() => {
  if (!searchQuery.value.trim()) return exercises.value
  const query = searchQuery.value.toLowerCase()
  return exercises.value.filter(
    (ex) =>
      ex.name.toLowerCase().includes(query) || ex.muscleGroup.toLowerCase().includes(query),
  )
})
</script>
