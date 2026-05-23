<template>
  <div class="bg-card border border-border rounded-2xl overflow-hidden">

    <!-- Routine name + date block -->
    <div class="px-5 pt-5 pb-4">
      <div class="flex items-start justify-between gap-3">
        <div class="flex-1 min-w-0">
          <h2 class="text-3xl font-extrabold text-foreground tracking-tight leading-none">
            {{ session.routineName }}
          </h2>
          <p class="mt-2 text-sm text-muted-foreground flex items-center gap-1.5">
            <CalendarDays class="w-3.5 h-3.5 shrink-0" />
            {{ session.date }}
            <span class="text-border">•</span>
            {{ session.time }}
          </p>
        </div>
        <!-- Dumbbell icon accent -->
        <div class="w-12 h-12 rounded-xl bg-primary/10 flex items-center justify-center shrink-0">
          <Dumbbell class="w-6 h-6 text-primary" />
        </div>
      </div>
    </div>

    <!-- Divider -->
    <div class="h-px bg-border mx-5" />

    <!-- 3-column key metrics -->
    <div class="grid grid-cols-3 divide-x divide-border">

      <div class="flex flex-col items-center gap-1.5 py-4 px-2">
        <div class="w-8 h-8 rounded-lg bg-blue-500/10 flex items-center justify-center">
          <Clock class="w-4 h-4 text-blue-400" />
        </div>
        <span class="text-xl font-bold text-foreground tabular-nums leading-none">
          {{ session.durationMin }}
        </span>
        <span class="text-[11px] text-muted-foreground font-medium">min</span>
      </div>

      <div class="flex flex-col items-center gap-1.5 py-4 px-2">
        <div class="w-8 h-8 rounded-lg bg-primary/10 flex items-center justify-center">
          <Weight class="w-4 h-4 text-primary" />
        </div>
        <span class="text-xl font-bold text-foreground tabular-nums leading-none">
          {{ formatVolume(session.totalVolumeKg) }}
        </span>
        <span class="text-[11px] text-muted-foreground font-medium">kg lifted</span>
      </div>

      <div class="flex flex-col items-center gap-1.5 py-4 px-2">
        <div class="w-8 h-8 rounded-lg bg-violet-500/10 flex items-center justify-center">
          <CheckCircle2 class="w-4 h-4 text-violet-400" />
        </div>
        <span class="text-xl font-bold text-foreground tabular-nums leading-none">
          {{ session.totalSets }}
        </span>
        <span class="text-[11px] text-muted-foreground font-medium">sets done</span>
      </div>

    </div>
  </div>
</template>

<script setup lang="ts">
import { CalendarDays, Clock, Weight, CheckCircle2, Dumbbell } from '@lucide/vue'

interface WorkoutSession {
  routineName: string
  date: string
  time: string
  durationMin: number
  totalVolumeKg: number
  totalSets: number
}

defineProps<{
  session: WorkoutSession
}>()

const formatVolume = (kg: number) => {
  if (kg >= 1000) return (kg / 1000).toFixed(1).replace(/\.0$/, '') + 'k'
  return kg.toLocaleString()
}
</script>
