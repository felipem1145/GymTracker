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

    <main class="px-4 pt-6 space-y-4">

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

    <!-- Bottom Nav -->
    <BottomNav />
  </div>
</template>

<script setup lang="ts">
import { computed } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { ChevronLeft, Share2 } from '@lucide/vue'
import WorkoutSummaryHero from '@/components/WorkoutSummaryHero.vue'
import CompletedExerciseCard from '@/components/CompletedExerciseCard.vue'
import BottomNav from '@/components/BottomNav.vue'

const route = useRoute()
const router = useRouter()

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

interface WorkoutSession {
  id: string
  routineName: string
  date: string
  time: string
  durationMin: number
  totalVolumeKg: number
  totalSets: number
  exercises: CompletedExercise[]
}

const mockSessions: Record<string, WorkoutSession> = {
  '1': {
    id: '1',
    routineName: 'Push Day',
    date: 'May 22, 2026',
    time: '08:30 AM',
    durationMin: 62,
    totalVolumeKg: 5420,
    totalSets: 18,
    exercises: [
      {
        id: 'ex-1', name: 'Bench Press', muscleGroup: 'Chest', hasPR: true,
        sets: [
          { setNumber: 1, kg: 80, reps: 8 },
          { setNumber: 2, kg: 82.5, reps: 7 },
          { setNumber: 3, kg: 85, reps: 6, isPR: true },
        ],
      },
      {
        id: 'ex-2', name: 'Incline Dumbbell Flyes', muscleGroup: 'Chest', hasPR: false,
        sets: [
          { setNumber: 1, kg: 24, reps: 12 },
          { setNumber: 2, kg: 24, reps: 11 },
          { setNumber: 3, kg: 22, reps: 12 },
        ],
      },
      {
        id: 'ex-3', name: 'Overhead Press', muscleGroup: 'Shoulders', hasPR: false,
        sets: [
          { setNumber: 1, kg: 60, reps: 6 },
          { setNumber: 2, kg: 60, reps: 5 },
          { setNumber: 3, kg: 55, reps: 7 },
        ],
      },
      {
        id: 'ex-4', name: 'Lateral Raises', muscleGroup: 'Shoulders', hasPR: true,
        sets: [
          { setNumber: 1, kg: 14, reps: 15 },
          { setNumber: 2, kg: 14, reps: 14, isPR: true },
          { setNumber: 3, kg: 12, reps: 15 },
          { setNumber: 4, kg: 12, reps: 14 },
        ],
      },
      {
        id: 'ex-5', name: 'Tricep Pushdowns', muscleGroup: 'Arms', hasPR: false,
        sets: [
          { setNumber: 1, kg: 35, reps: 12 },
          { setNumber: 2, kg: 35, reps: 11 },
          { setNumber: 3, kg: 30, reps: 13 },
        ],
      },
      {
        id: 'ex-6', name: 'Cable Chest Flyes', muscleGroup: 'Chest', hasPR: false,
        sets: [
          { setNumber: 1, kg: 20, reps: 15 },
          { setNumber: 2, kg: 20, reps: 14 },
          { setNumber: 3, kg: 17.5, reps: 15 },
        ],
      },
    ],
  },
  '2': {
    id: '2',
    routineName: 'Leg Day',
    date: 'May 20, 2026',
    time: '07:00 AM',
    durationMin: 75,
    totalVolumeKg: 8750,
    totalSets: 20,
    exercises: [
      {
        id: 'ex-1', name: 'Squat', muscleGroup: 'Legs', hasPR: true,
        sets: [
          { setNumber: 1, kg: 100, reps: 6 },
          { setNumber: 2, kg: 105, reps: 5, isPR: true },
          { setNumber: 3, kg: 100, reps: 6 },
          { setNumber: 4, kg: 95, reps: 7 },
        ],
      },
      {
        id: 'ex-2', name: 'Romanian Deadlift', muscleGroup: 'Legs', hasPR: false,
        sets: [
          { setNumber: 1, kg: 80, reps: 8 },
          { setNumber: 2, kg: 80, reps: 8 },
          { setNumber: 3, kg: 75, reps: 10 },
        ],
      },
      {
        id: 'ex-3', name: 'Leg Press', muscleGroup: 'Legs', hasPR: false,
        sets: [
          { setNumber: 1, kg: 160, reps: 10 },
          { setNumber: 2, kg: 160, reps: 9 },
          { setNumber: 3, kg: 140, reps: 12 },
        ],
      },
      {
        id: 'ex-4', name: 'Leg Curl', muscleGroup: 'Legs', hasPR: false,
        sets: [
          { setNumber: 1, kg: 50, reps: 12 },
          { setNumber: 2, kg: 50, reps: 11 },
          { setNumber: 3, kg: 45, reps: 13 },
        ],
      },
      {
        id: 'ex-5', name: 'Calf Raises', muscleGroup: 'Legs', hasPR: false,
        sets: [
          { setNumber: 1, kg: 60, reps: 15 },
          { setNumber: 2, kg: 60, reps: 15 },
          { setNumber: 3, kg: 60, reps: 14 },
          { setNumber: 4, kg: 60, reps: 13 },
          { setNumber: 5, kg: 60, reps: 12 },
        ],
      },
    ],
  },
  '3': {
    id: '3',
    routineName: 'Pull Day',
    date: 'May 18, 2026',
    time: '09:00 AM',
    durationMin: 58,
    totalVolumeKg: 4890,
    totalSets: 21,
    exercises: [
      {
        id: 'ex-1', name: 'Pull-ups', muscleGroup: 'Back', hasPR: false,
        sets: [
          { setNumber: 1, kg: 0, reps: 10 },
          { setNumber: 2, kg: 0, reps: 9 },
          { setNumber: 3, kg: 0, reps: 8 },
        ],
      },
      {
        id: 'ex-2', name: 'Barbell Rows', muscleGroup: 'Back', hasPR: true,
        sets: [
          { setNumber: 1, kg: 80, reps: 8 },
          { setNumber: 2, kg: 85, reps: 7, isPR: true },
          { setNumber: 3, kg: 80, reps: 8 },
        ],
      },
      {
        id: 'ex-3', name: 'Lat Pulldown', muscleGroup: 'Back', hasPR: false,
        sets: [
          { setNumber: 1, kg: 65, reps: 10 },
          { setNumber: 2, kg: 65, reps: 10 },
          { setNumber: 3, kg: 60, reps: 11 },
        ],
      },
      {
        id: 'ex-4', name: 'Seated Cable Row', muscleGroup: 'Back', hasPR: false,
        sets: [
          { setNumber: 1, kg: 55, reps: 12 },
          { setNumber: 2, kg: 55, reps: 11 },
          { setNumber: 3, kg: 50, reps: 13 },
        ],
      },
      {
        id: 'ex-5', name: 'Bicep Curls', muscleGroup: 'Arms', hasPR: false,
        sets: [
          { setNumber: 1, kg: 20, reps: 12 },
          { setNumber: 2, kg: 20, reps: 11 },
          { setNumber: 3, kg: 18, reps: 13 },
        ],
      },
      {
        id: 'ex-6', name: 'Hammer Curls', muscleGroup: 'Arms', hasPR: false,
        sets: [
          { setNumber: 1, kg: 16, reps: 12 },
          { setNumber: 2, kg: 16, reps: 12 },
          { setNumber: 3, kg: 14, reps: 14 },
        ],
      },
      {
        id: 'ex-7', name: 'Face Pulls', muscleGroup: 'Shoulders', hasPR: false,
        sets: [
          { setNumber: 1, kg: 25, reps: 15 },
          { setNumber: 2, kg: 25, reps: 15 },
          { setNumber: 3, kg: 22, reps: 15 },
        ],
      },
    ],
  },
}

const session = computed<WorkoutSession>(() => {
  const id = String(route.params.id)
  return mockSessions[id] ?? mockSessions['1']!
})

const handleShare = () => {
  if (navigator.share) {
    navigator.share({
      title: `${session.value.routineName} — ${session.value.date}`,
      text: `Crushed ${session.value.routineName}! ${session.value.totalVolumeKg.toLocaleString()} kg lifted in ${session.value.durationMin} min. 💪`,
    })
  }
}
</script>
