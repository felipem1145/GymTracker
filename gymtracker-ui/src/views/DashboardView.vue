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
import { ref, computed, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { MOCK_AUTH_USER } from '@/constants/mockUser'
import { useWorkoutStore } from '@/stores/workout'
import DashboardHeader from '@/components/DashboardHeader.vue'
import QuickStats from '@/components/QuickStats.vue'
import ActionButtons from '@/components/ActionButtons.vue'
import WorkoutHistory from '@/components/WorkoutHistory.vue'
import BottomNav from '@/components/BottomNav.vue'

const router = useRouter()
const workoutStore = useWorkoutStore()

onMounted(() => {
  if (workoutStore.activeSession) {
    router.push('/workout')
  }
})

const userName = ref(MOCK_AUTH_USER.name)
const currentStreak = ref(12)

const weeklyStats = ref({
  workouts: 5,
  totalWeight: 24500,
  duration: 285,
})

const workoutHistory = computed(() =>
  workoutStore.history.map((session) => {
    const routine = workoutStore.routines.find((r) => r.name === session.routineName)
    return {
      id: session.id,
      name: session.routineName,
      date: session.date,
      dayOfWeek: session.date.toLocaleDateString('en-US', { weekday: 'long' }),
      totalWeight: session.totalVolumeKg,
      duration: session.durationMin,
      exercises: routine?.exerciseCount ?? session.exercises.length,
      icon: 'dumbbell',
    }
  }),
)

const handleStartWorkout = () => {
  router.push('/routines')
}

const handleViewWorkout = (workoutId: string) => {
  router.push({ name: 'workout-detail', params: { id: workoutId } })
}
</script>
