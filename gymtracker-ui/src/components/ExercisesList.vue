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
            backgroundColor: exercise.muscleGroupColor + '20',
            color: exercise.muscleGroupColor,
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
  id: number
  name: string
  muscleGroup: string
  muscleGroupColor: string
}

defineProps<{
  exercises: Exercise[]
  search: string
}>()

defineEmits<{
  'update:search': [value: string]
}>()
</script>
