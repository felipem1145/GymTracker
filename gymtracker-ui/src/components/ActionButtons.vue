<template>
  <div class="mb-8">
    <!-- Primary CTA -->
    <button
      @click="$emit('startWorkout')"
      class="w-full bg-primary hover:bg-primary/90 text-primary-foreground font-semibold py-5 px-6 rounded-2xl flex items-center justify-center gap-3 transition-all duration-200 active:scale-[0.98] shadow-lg shadow-primary/20 mb-3"
    >
      <Play class="w-6 h-6" fill="currentColor" />
      <span class="text-lg">Start Workout</span>
    </button>

    <!-- Secondary CTA -->
    <button
      @click="$emit('chooseRoutine')"
      class="w-full bg-secondary hover:bg-secondary/80 text-secondary-foreground font-medium py-4 px-6 rounded-xl flex items-center justify-center gap-3 transition-all duration-200 active:scale-[0.98] border border-border"
    >
      <LayoutGrid class="w-5 h-5" />
      <span>Choose a Routine</span>
      <ChevronRight class="w-5 h-5 ml-auto text-muted-foreground" />
    </button>
  </div>
</template>

<script setup lang="ts">
import { Play, LayoutGrid, ChevronRight } from '@lucide/vue'

defineEmits<{
  startWorkout: []
  chooseRoutine: []
}>()
</script>
