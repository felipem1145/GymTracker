<template>
  <div class="mb-8">
    <!-- Primary CTA -->
    <button
      @click="$emit('startWorkout')"
      class="w-full bg-primary hover:bg-primary/90 text-primary-foreground font-semibold py-5 px-6 rounded-2xl flex items-center justify-center gap-3 transition-all duration-200 active:scale-[0.98] shadow-lg shadow-primary/20"
    >
      <Play class="w-6 h-6" fill="currentColor" />
      <span class="text-lg">Start Workout</span>
    </button>
  </div>
</template>

<script setup lang="ts">
import { Play } from '@lucide/vue'

defineEmits<{
  startWorkout: []
}>()
</script>
