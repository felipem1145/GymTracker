<template>
  <div>
    <!-- Search Bar -->
    <div class="relative mb-4">
      <Search class="absolute left-4 top-1/2 -translate-y-1/2 w-5 h-5 text-[#71717a]" />
      <input
        type="text"
        :value="search"
        @input="$emit('update:search', ($event.target as HTMLInputElement).value)"
        placeholder="Search exercises..."
        class="w-full bg-[#18181b] border border-[#27272a] rounded-xl py-3.5 pl-12 pr-4 text-[#fafafa] placeholder:text-[#71717a] focus:outline-none focus:border-[#22c55e] focus:ring-1 focus:ring-[#22c55e] transition-all"
      />
      <button
        v-if="search"
        @click="$emit('update:search', '')"
        class="absolute right-4 top-1/2 -translate-y-1/2 p-1 rounded-full hover:bg-[#27272a] transition-colors"
      >
        <X class="w-4 h-4 text-[#71717a]" />
      </button>
    </div>

    <!-- Results Count -->
    <p class="text-sm text-[#71717a] mb-3">
      {{ exercises.length }} exercise{{ exercises.length !== 1 ? 's' : '' }} found
    </p>

    <!-- Exercise List -->
    <div class="space-y-2">
      <div
        v-for="exercise in exercises"
        :key="exercise.id"
        class="bg-[#18181b] rounded-xl p-4 border border-[#27272a] hover:border-[#3f3f46] transition-all cursor-pointer active:scale-[0.99] flex items-center justify-between"
      >
        <div class="flex items-center gap-3 min-w-0">
          <div class="w-10 h-10 rounded-xl bg-[#27272a] flex items-center justify-center flex-shrink-0">
            <Target class="w-5 h-5 text-[#a1a1aa]" />
          </div>
          <span class="font-medium text-[#fafafa] truncate">{{ exercise.name }}</span>
        </div>

        <!-- Muscle Group Badge -->
        <span
          class="flex-shrink-0 ml-3 px-3 py-1 rounded-full text-xs font-semibold"
          :style="{
            backgroundColor: getMuscleGroupColor(exercise.muscleGroup) + '20',
            color: getMuscleGroupColor(exercise.muscleGroup),
          }"
        >
          {{ exercise.muscleGroup }}
        </span>
      </div>
    </div>

    <!-- Empty State -->
    <div v-if="exercises.length === 0" class="text-center py-12">
      <div class="w-16 h-16 rounded-2xl bg-[#18181b] flex items-center justify-center mx-auto mb-4">
        <SearchX class="w-8 h-8 text-[#71717a]" />
      </div>
      <p class="text-[#a1a1aa] font-medium">No exercises found</p>
      <p class="text-sm text-[#71717a] mt-1">Try a different search term</p>
    </div>
  </div>
</template>

<script setup lang="ts">
import { Search, X, Target, SearchX } from '@lucide/vue'

interface Exercise {
  id: string
  name: string
  muscleGroup: string
}

function getMuscleGroupColor(muscleGroup: string): string {
  const normalized = muscleGroup.trim().toLowerCase()

  const colorMap: Record<string, string> = {
    chest: '#ef4444',
    back: '#3b82f6',
    shoulders: '#f59e0b',
    arms: '#22c55e',
    biceps: '#22c55e',
    triceps: '#22c55e',
    legs: '#8b5cf6',
    quads: '#8b5cf6',
    hamstrings: '#8b5cf6',
    glutes: '#8b5cf6',
    calves: '#8b5cf6',
    core: '#ec4899',
  }

  return colorMap[normalized] ?? '#a1a1aa'
}

defineProps<{
  exercises: Exercise[]
  search: string
}>()

defineEmits<{
  'update:search': [value: string]
}>()
</script>
