<template>
  <div class="min-h-screen bg-background pb-24">
    <!-- Header -->
    <DashboardHeader :user-name="userName" :streak="currentStreak" />

    <!-- Main Content -->
    <main class="px-4 pt-4">
      <!-- Quick Stats -->
      <QuickStats :stats="weeklyStats" />

      <!-- Action Buttons -->
      <ActionButtons
        @start-workout="handleStartWorkout"
        @choose-routine="handleChooseRoutine"
      />

      <!-- Workout History -->
      <WorkoutHistory
        :workouts="workoutHistory"
        @view-workout="handleViewWorkout"
      />
    </main>

    <!-- Bottom Navigation -->
    <BottomNav />
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import DashboardHeader from '@/components/DashboardHeader.vue'
import QuickStats from '@/components/QuickStats.vue'
import ActionButtons from '@/components/ActionButtons.vue'
import WorkoutHistory from '@/components/WorkoutHistory.vue'
import BottomNav from '@/components/BottomNav.vue'

const router = useRouter()

const userName = ref('Alex')
const currentStreak = ref(12)

const weeklyStats = ref({
  workouts: 5,
  totalWeight: 24500,
  duration: 285,
})

const workoutHistory = ref([
  {
    id: '1',
    name: 'Push Day',
    date: new Date(2026, 4, 22),
    dayOfWeek: 'Thursday',
    totalWeight: 5420,
    duration: 62,
    exercises: 6,
    icon: 'dumbbell',
  },
  {
    id: '2',
    name: 'Leg Day',
    date: new Date(2026, 4, 20),
    dayOfWeek: 'Tuesday',
    totalWeight: 8750,
    duration: 75,
    exercises: 5,
    icon: 'footprints',
  },
  {
    id: '3',
    name: 'Pull Day',
    date: new Date(2026, 4, 18),
    dayOfWeek: 'Sunday',
    totalWeight: 4890,
    duration: 58,
    exercises: 7,
    icon: 'arrow-up',
  },
  {
    id: '4',
    name: 'Upper Body',
    date: new Date(2026, 4, 16),
    dayOfWeek: 'Friday',
    totalWeight: 5280,
    duration: 65,
    exercises: 8,
    icon: 'dumbbell',
  },
  {
    id: '5',
    name: 'Lower Body',
    date: new Date(2026, 4, 14),
    dayOfWeek: 'Wednesday',
    totalWeight: 9120,
    duration: 70,
    exercises: 6,
    icon: 'footprints',
  },
])

const handleStartWorkout = () => {
  router.push('/workout')
}

const handleChooseRoutine = () => {
  router.push('/routines')
}

const handleViewWorkout = (workoutId: string) => {
  router.push({ name: 'workout-detail', params: { id: workoutId } })
}
</script>
