<template>
  <div class="min-h-screen bg-background pb-28">

    <!-- Top Navigation Header -->
    <header class="sticky top-0 z-40 bg-background/95 backdrop-blur-sm border-b border-border px-4 py-3">
      <div class="flex items-center justify-between">
        <button
          @click="router.back()"
          class="w-9 h-9 flex items-center justify-center rounded-xl bg-secondary hover:bg-muted transition-colors"
          aria-label="Go back"
        >
          <ChevronLeft class="w-5 h-5 text-foreground" />
        </button>

        <h1 class="text-base font-semibold text-foreground tracking-tight">Workout Summary</h1>

        <button
          @click="handleShare"
          class="w-9 h-9 flex items-center justify-center rounded-xl bg-secondary hover:bg-muted transition-colors"
          aria-label="Share workout"
        >
          <Share2 class="w-4 h-4 text-muted-foreground" />
        </button>
      </div>
    </header>

    <main v-if="session" class="px-4 pt-6 space-y-4">

      <!-- Hero Stats Card -->
      <WorkoutSummaryHero :session="session" />

      <!-- Section Label -->
      <div class="flex items-center gap-3 pt-2">
        <h2 class="text-xs font-semibold text-muted-foreground uppercase tracking-widest">Exercises</h2>
        <div class="flex-1 h-px bg-border" />
        <span class="text-xs text-muted-foreground">{{ session.exercises.length }} total</span>
      </div>

      <!-- Exercise Cards -->
      <CompletedExerciseCard
        v-for="exercise in session.exercises"
        :key="exercise.id"
        :exercise="exercise"
      />

    </main>

    <!-- Not found fallback -->
    <div v-else class="px-4 pt-20 flex flex-col items-center gap-3 text-center">
      <p class="text-muted-foreground text-sm">Workout session not found.</p>
      <button
        @click="router.back()"
        class="text-primary text-sm font-medium"
      >Go back</button>
    </div>

    <!-- Bottom Nav -->
    <BottomNav />
  </div>
</template>

<script setup lang="ts">
import { computed } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { ChevronLeft, Share2 } from '@lucide/vue'
import { useWorkoutStore } from '@/stores/workout'
import WorkoutSummaryHero from '@/components/WorkoutSummaryHero.vue'
import CompletedExerciseCard from '@/components/CompletedExerciseCard.vue'
import BottomNav from '@/components/BottomNav.vue'

const route = useRoute()
const router = useRouter()
const workoutStore = useWorkoutStore()

const sessionData = computed(() =>
  workoutStore.history.find((s) => s.id === String(route.params.id)),
)

// Flatten Date → formatted string so WorkoutSummaryHero receives date: string
const session = computed(() => {
  if (!sessionData.value) return null
  return {
    ...sessionData.value,
    date: sessionData.value.date.toLocaleDateString('en-US', {
      month: 'long',
      day: 'numeric',
      year: 'numeric',
    }),
  }
})

const handleShare = () => {
  if (!session.value || !navigator.share) return
  navigator.share({
    title: `${session.value.routineName} — ${session.value.date}`,
    text: `Crushed ${session.value.routineName}! ${session.value.totalVolumeKg.toLocaleString()} kg lifted in ${session.value.durationMin} min. 💪`,
  })
}
</script>

