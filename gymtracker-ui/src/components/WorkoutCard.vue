<template>
  <button
    class="w-full bg-card hover:bg-card/80 border border-border rounded-xl p-4 flex items-center gap-4 transition-all duration-200 active:scale-[0.99] group text-left"
  >
    <!-- Icon -->
    <div class="w-12 h-12 rounded-xl bg-secondary flex items-center justify-center shrink-0">
      <component :is="iconComponent" class="w-6 h-6 text-foreground" />
    </div>

    <!-- Content -->
    <div class="flex-1 min-w-0">
      <div class="flex items-center justify-between mb-1">
        <h3 class="font-semibold text-foreground truncate">{{ workout.name }}</h3>
        <span class="text-xs text-muted-foreground">{{ formattedDate }}</span>
      </div>
      <div class="flex items-center gap-4 text-sm text-muted-foreground">
        <span class="flex items-center gap-1">
          <Weight class="w-3.5 h-3.5" />
          {{ formattedWeight }} kg
        </span>
        <span class="flex items-center gap-1">
          <Clock class="w-3.5 h-3.5" />
          {{ workout.duration }}m
        </span>
        <span class="flex items-center gap-1">
          <Layers class="w-3.5 h-3.5" />
          {{ workout.exercises }}
        </span>
      </div>
    </div>

    <!-- Chevron -->
    <ChevronRight class="w-5 h-5 text-muted-foreground group-hover:text-foreground transition-colors shrink-0" />
  </button>
</template>

<script setup lang="ts">
import { computed } from 'vue'
import { Dumbbell, Footprints, ArrowUp, Weight, Clock, Layers, ChevronRight } from '@lucide/vue'

interface Workout {
  id: string
  name: string
  date: Date
  dayOfWeek: string
  totalWeight: number
  duration: number
  exercises: number
  icon: string
}

const props = defineProps<{
  workout: Workout
}>()

const iconComponent = computed(() => {
  switch (props.workout.icon) {
    case 'dumbbell':
      return Dumbbell
    case 'footprints':
      return Footprints
    case 'arrow-up':
      return ArrowUp
    default:
      return Dumbbell
  }
})

const formattedDate = computed(() => {
  const now = new Date()
  const diffDays = Math.floor((now.getTime() - props.workout.date.getTime()) / (1000 * 60 * 60 * 24))

  if (diffDays === 0) return 'Today'
  if (diffDays === 1) return 'Yesterday'
  if (diffDays < 7) return props.workout.dayOfWeek

  return props.workout.date.toLocaleDateString('en-US', { month: 'short', day: 'numeric' })
})

const formattedWeight = computed(() => {
  return props.workout.totalWeight.toLocaleString()
})
</script>
