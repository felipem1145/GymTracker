import type { Session, User } from '@supabase/supabase-js'
import { defineStore } from 'pinia'
import { computed, ref } from 'vue'
import { supabase } from '@/services/supabaseClient'

interface AuthState {
  user: User | null
  accessToken: string | null
}

export const useAuthStore = defineStore('auth', () => {
  const user = ref<User | null>(null)
  const accessToken = ref<string | null>(null)
  let isAuthListenerInitialized = false

  const isAuthenticated = computed(() => Boolean(user.value && accessToken.value))

  function setAuth(state: AuthState): void {
    user.value = state.user
    accessToken.value = state.accessToken
  }

  function clearAuth(): void {
    user.value = null
    accessToken.value = null
  }

  function syncFromSession(session: Session | null): void {
    if (!session?.user || !session.access_token) {
      clearAuth()
      return
    }

    setAuth({
      user: session.user,
      accessToken: session.access_token,
    })
  }

  async function initializeAuth(): Promise<void> {
    const { data, error } = await supabase.auth.getSession()

    if (error) {
      clearAuth()
    } else {
      syncFromSession(data.session)
    }

    if (isAuthListenerInitialized) {
      return
    }

    supabase.auth.onAuthStateChange((event, session) => {
      if (event === 'SIGNED_IN' || event === 'TOKEN_REFRESHED') {
        syncFromSession(session)
        return
      }

      if (event === 'SIGNED_OUT') {
        clearAuth()
      }
    })

    isAuthListenerInitialized = true
  }

  return {
    user,
    accessToken,
    isAuthenticated,
    setAuth,
    clearAuth,
    initializeAuth,
  }
})