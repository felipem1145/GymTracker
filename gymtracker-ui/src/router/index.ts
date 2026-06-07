import { createRouter, createWebHistory } from 'vue-router'
import DashboardView from '@/views/DashboardView.vue'
import { useAuthStore } from '@/stores/auth'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'dashboard',
      component: DashboardView,
      meta: { requiresAuth: true },
    },
    {
      path: '/routines',
      name: 'routines',
      component: () => import('@/views/RoutinesView.vue'),
      meta: { requiresAuth: true },
    },
    {
      path: '/routines/create',
      name: 'create-routine',
      component: () => import('@/views/CreateRoutineView.vue'),
      meta: { requiresAuth: true },
    },
    {
      path: '/workout',
      name: 'workout',
      component: () => import('@/views/WorkoutView.vue'),
      meta: { requiresAuth: true },
    },
    {
      path: '/workout-history/:id',
      name: 'workout-detail',
      component: () => import('@/views/WorkoutDetailView.vue'),
      meta: { requiresAuth: true },
    },
    {
      path: '/login',
      name: 'login',
      component: () => import('@/views/LoginView.vue'),
      meta: { requiresAuth: false },
    },
    {
      path: '/signup',
      name: 'signup',
      component: () => import('@/views/SignUpView.vue'),
      meta: { requiresAuth: false },
    },
  ],
})

router.beforeEach((to, from, next) => {
  const authStore = useAuthStore()
  const requiresAuth = to.matched.some((record) => record.meta.requiresAuth)
  const isAuthPage = to.path === '/login' || to.path === '/signup'

  if (requiresAuth && !authStore.isAuthenticated) {
    next('/login')
    return
  }

  if (isAuthPage && authStore.isAuthenticated) {
    next('/')
    return
  }

  next()
})

export default router
