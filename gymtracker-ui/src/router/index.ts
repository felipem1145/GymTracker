import { createRouter, createWebHistory } from 'vue-router'
import DashboardView from '@/views/DashboardView.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'dashboard',
      component: DashboardView,
    },
    {
      path: '/routines',
      name: 'routines',
      component: () => import('@/views/RoutinesView.vue'),
    },
    {
      path: '/routines/create',
      name: 'create-routine',
      component: () => import('@/views/CreateRoutineView.vue'),
    },
    {
      path: '/workout',
      name: 'workout',
      component: () => import('@/views/WorkoutView.vue'),
    },
    {
      path: '/workout-history/:id',
      name: 'workout-detail',
      component: () => import('@/views/WorkoutDetailView.vue'),
    },
    {
      path: '/login',
      name: 'login',
      component: () => import('@/views/LoginView.vue'),
    },
  ],
})

export default router
