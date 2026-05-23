<template>
  <section>
    <div class="flex items-center justify-between mb-4">
      <h2 class="text-lg font-semibold text-foreground">Workout History</h2>
      <button class="text-sm text-primary font-medium flex items-center gap-1">
        View All
        <ChevronRight class="w-4 h-4" />
      </button>
    </div>

    <div class="space-y-3">
      <WorkoutCard
        v-for="workout in workouts"
        :key="workout.id"
        :workout="workout"
        @click="$emit('viewWorkout', workout.id)"
      />
    </div>
  </section>
</template>

<script setup lang="ts">
import { ChevronRight } from '@lucide/vue'
import WorkoutCard from '@/components/WorkoutCard.vue'

export interface Workout {
  id: string
  name: string
  date: Date
  dayOfWeek: string
  totalWeight: number
  duration: number
  exercises: number
  icon: string
}

defineProps<{
  workouts: Workout[]
}>()

defineEmits<{
  viewWorkout: [id: string]
}>()
</script>
