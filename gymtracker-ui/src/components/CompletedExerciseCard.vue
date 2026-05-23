<template>
  <div class="bg-card border border-border rounded-2xl overflow-hidden">

    <!-- Exercise Header -->
    <div class="px-4 pt-4 pb-3 flex items-center justify-between gap-3">
      <div class="flex items-center gap-3 min-w-0">
        <div
          class="w-9 h-9 rounded-xl flex items-center justify-center shrink-0"
          :class="muscleBgClass"
        >
          <Dumbbell class="w-4 h-4" :class="muscleIconClass" />
        </div>
        <div class="min-w-0">
          <div class="flex items-center gap-2">
            <h3 class="font-semibold text-foreground text-[15px] leading-tight truncate">
              {{ exercise.name }}
            </h3>
            <!-- PR badge -->
            <span
              v-if="exercise.hasPR"
              class="inline-flex items-center gap-1 px-2 py-0.5 bg-amber-500/15 border border-amber-500/30 text-amber-400 text-[10px] font-bold rounded-full uppercase tracking-wide shrink-0"
            >
              <Trophy class="w-2.5 h-2.5" />
              PR
            </span>
          </div>
          <span
            class="mt-0.5 inline-block text-[11px] font-medium px-2 py-0.5 rounded-full"
            :class="musclePillClass"
          >
            {{ exercise.muscleGroup }}
          </span>
        </div>
      </div>
      <span class="text-xs text-muted-foreground shrink-0">
        {{ exercise.sets.length }} sets
      </span>
    </div>

    <!-- Divider -->
    <div class="h-px bg-border/60 mx-4" />

    <!-- Column headers -->
    <div class="px-4 pt-2.5 pb-1 grid grid-cols-[2.5rem_1fr_auto] gap-x-3 items-center">
      <span class="text-[10px] font-semibold text-muted-foreground uppercase tracking-wider">Set</span>
      <span class="text-[10px] font-semibold text-muted-foreground uppercase tracking-wider">Performance</span>
      <span class="text-[10px] font-semibold text-muted-foreground uppercase tracking-wider w-7 text-center">Done</span>
    </div>

    <!-- Set rows -->
    <div class="px-4 pb-4 space-y-1">
      <div
        v-for="set in exercise.sets"
        :key="set.setNumber"
        class="grid grid-cols-[2.5rem_1fr_auto] gap-x-3 items-center py-2 rounded-lg transition-colors"
        :class="set.isPR ? 'bg-amber-500/10' : 'hover:bg-secondary/50'"
      >
        <!-- Set number -->
        <div
          class="w-7 h-7 rounded-lg flex items-center justify-center text-xs font-bold"
          :class="set.isPR ? 'bg-amber-500/20 text-amber-400' : 'bg-secondary text-muted-foreground'"
        >
          {{ set.setNumber }}
        </div>

        <!-- Weight × Reps -->
        <div class="flex items-center gap-2">
          <span class="text-sm font-semibold text-foreground tabular-nums">
            {{ set.kg }} kg
          </span>
          <span class="text-muted-foreground text-xs">×</span>
          <span class="text-sm font-semibold text-foreground tabular-nums">
            {{ set.reps }} reps
          </span>
          <span
            v-if="set.isPR"
            class="inline-flex items-center gap-1 px-1.5 py-0.5 bg-amber-500/15 border border-amber-500/25 text-amber-400 text-[10px] font-bold rounded-md uppercase tracking-wide"
          >
            <Trophy class="w-2.5 h-2.5" />
            PR
          </span>
        </div>

        <!-- Done checkmark -->
        <div class="w-7 h-7 rounded-full bg-primary/15 flex items-center justify-center">
          <CheckCircle2 class="w-4 h-4 text-primary" />
        </div>
      </div>
    </div>

  </div>
</template>

<script setup lang="ts">
import { computed } from 'vue'
import { Dumbbell, CheckCircle2, Trophy } from '@lucide/vue'

interface CompletedSet {
  setNumber: number
  kg: number
  reps: number
  isPR?: boolean
}

interface CompletedExercise {
  id: string
  name: string
  muscleGroup: string
  hasPR?: boolean
  sets: CompletedSet[]
}

const props = defineProps<{
  exercise: CompletedExercise
}>()

const muscleColors: Record<string, { bg: string; icon: string; pill: string }> = {
  Chest:     { bg: 'bg-red-500/10',    icon: 'text-red-400',    pill: 'bg-red-500/15 text-red-400' },
  Back:      { bg: 'bg-blue-500/10',   icon: 'text-blue-400',   pill: 'bg-blue-500/15 text-blue-400' },
  Shoulders: { bg: 'bg-amber-500/10',  icon: 'text-amber-400',  pill: 'bg-amber-500/15 text-amber-400' },
  Legs:      { bg: 'bg-purple-500/10', icon: 'text-purple-400', pill: 'bg-purple-500/15 text-purple-400' },
  Arms:      { bg: 'bg-primary/10',    icon: 'text-primary',    pill: 'bg-primary/15 text-primary' },
  Core:      { bg: 'bg-pink-500/10',   icon: 'text-pink-400',   pill: 'bg-pink-500/15 text-pink-400' },
}

const colors = computed(() => muscleColors[props.exercise.muscleGroup] ?? muscleColors['Arms']!)
const muscleBgClass   = computed(() => colors.value.bg)
const muscleIconClass = computed(() => colors.value.icon)
const musclePillClass = computed(() => colors.value.pill)
</script>
