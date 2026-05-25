<template>
  <div class="bg-card rounded-xl p-5 mb-6 border border-border">
    <!-- Exercise Name & Muscle Group -->
    <div class="flex items-start justify-between mb-5">
      <div>
        <h2 class="text-2xl font-bold text-foreground">{{ exercise.name }}</h2>
        <p class="text-sm text-muted-foreground mt-1">Target: {{ exercise.muscleGroup }}</p>
      </div>
      <span class="px-3 py-1 bg-primary/20 text-primary text-xs font-semibold rounded-full">
        {{ exercise.muscleGroup }}
      </span>
    </div>

    <!-- Last Time & PR Card -->
    <div
      v-if="lastPerformance.length > 0 || personalRecord"
      class="bg-secondary/50 rounded-lg p-4 border border-border/50"
    >
      <div class="mb-3 flex items-center justify-between gap-2">
        <p class="text-xs text-muted-foreground font-medium uppercase tracking-wider">Last Time:</p>
        <span class="inline-flex items-center gap-1 rounded-full border border-amber-300/50 bg-amber-500/10 px-2.5 py-1 text-xs font-semibold text-amber-700">
          <Trophy class="h-3.5 w-3.5" />
          PR: {{ personalRecord ? `${personalRecord.weight} kg x ${personalRecord.reps}` : '--' }}
        </span>
      </div>

      <div v-if="lastPerformance.length > 0" class="flex flex-wrap gap-4">
        <div v-for="(perf, idx) in lastPerformance" :key="idx" class="flex-1 min-w-max">
          <p class="text-xs text-muted-foreground">S{{ perf.set }}</p>
          <p class="text-sm font-semibold text-foreground">
            {{ perf.kg }}kg × {{ perf.reps }}
          </p>
        </div>
      </div>

      <p v-else class="text-sm text-muted-foreground">No previous records for this exercise yet.</p>
    </div>
  </div>
</template>

<script setup lang="ts">
import { Trophy } from '@lucide/vue'

interface Exercise {
  id: string
  name: string
  muscleGroup: string
  icon?: string
}

interface PerformanceData {
  set: number
  kg?: number
  reps?: number
}

interface PersonalRecord {
  weight: number
  reps: number
}

defineProps<{
  exercise: Exercise
  lastPerformance: PerformanceData[]
  personalRecord?: PersonalRecord | null
}>()
</script>
