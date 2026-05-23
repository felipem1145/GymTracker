<template>
  <div class="min-h-screen bg-[#09090b] flex flex-col items-center justify-center px-5 py-12">

    <!-- Branding zone -->
    <div class="flex flex-col items-center gap-4 mb-10">
      <div class="relative flex items-center justify-center w-16 h-16 rounded-2xl bg-[#22c55e]/10 border border-[#22c55e]/20">
        <div class="absolute inset-0 rounded-2xl bg-[#22c55e]/5 blur-lg"></div>
        <Dumbbell class="relative z-10 w-8 h-8 text-[#22c55e]" :stroke-width="2" />
      </div>
      <div class="text-center">
        <h1 class="text-3xl font-bold text-[#fafafa] tracking-tight">Let's Train</h1>
        <p class="mt-1.5 text-sm text-[#a1a1aa] leading-relaxed">
          Log in to track your progressive<br>overload and routines.
        </p>
      </div>
    </div>

    <!-- Card -->
    <div class="w-full max-w-sm flex flex-col gap-4">

      <!-- Email field -->
      <div class="flex flex-col gap-1.5">
        <label for="email" class="text-xs font-medium text-[#a1a1aa] uppercase tracking-widest">Email</label>
        <div
          class="flex items-center gap-3 px-4 h-14 rounded-xl bg-[#18181b] border transition-colors duration-150"
          :class="focusedField === 'email' ? 'border-[#22c55e]' : 'border-[#27272a]'"
        >
          <Mail class="w-4 h-4 shrink-0 text-[#a1a1aa]" :stroke-width="2" />
          <input
            id="email"
            v-model="email"
            type="email"
            placeholder="you@example.com"
            autocomplete="email"
            class="flex-1 bg-transparent text-[#fafafa] text-[15px] placeholder:text-[#52525b] outline-none"
            @focus="focusedField = 'email'"
            @blur="focusedField = null"
          />
        </div>
      </div>

      <!-- Password field -->
      <div class="flex flex-col gap-1.5">
        <label for="password" class="text-xs font-medium text-[#a1a1aa] uppercase tracking-widest">Password</label>
        <div
          class="flex items-center gap-3 px-4 h-14 rounded-xl bg-[#18181b] border transition-colors duration-150"
          :class="focusedField === 'password' ? 'border-[#22c55e]' : 'border-[#27272a]'"
        >
          <Lock class="w-4 h-4 shrink-0 text-[#a1a1aa]" :stroke-width="2" />
          <input
            id="password"
            v-model="password"
            :type="showPassword ? 'text' : 'password'"
            placeholder="••••••••"
            autocomplete="current-password"
            class="flex-1 bg-transparent text-[#fafafa] text-[15px] placeholder:text-[#52525b] outline-none"
            @focus="focusedField = 'password'"
            @blur="focusedField = null"
          />
          <button
            type="button"
            class="shrink-0 text-[#52525b] hover:text-[#a1a1aa] transition-colors duration-150 focus:outline-none"
            :aria-label="showPassword ? 'Hide password' : 'Show password'"
            @click="showPassword = !showPassword"
          >
            <Eye v-if="!showPassword" class="w-4 h-4" :stroke-width="2" />
            <EyeOff v-else class="w-4 h-4" :stroke-width="2" />
          </button>
        </div>
      </div>

      <!-- Forgot password -->
      <div class="flex justify-end -mt-1">
        <button
          type="button"
          class="text-xs text-[#a1a1aa] hover:text-[#22c55e] transition-colors duration-150 focus:outline-none"
        >
          Forgot password?
        </button>
      </div>

      <!-- Sign in button -->
      <button
        type="button"
        class="mt-1 w-full h-14 rounded-xl bg-[#22c55e] text-[#0a0a0a] font-bold text-[15px] tracking-wide
               flex items-center justify-center gap-2
               hover:bg-[#16a34a] active:scale-[0.98]
               transition-all duration-150 focus:outline-none focus:ring-2 focus:ring-[#22c55e]/50"
        :class="{ 'opacity-60 cursor-not-allowed': isLoading }"
        :disabled="isLoading"
        @click="handleLogin"
      >
        <Loader2 v-if="isLoading" class="w-5 h-5 animate-spin" :stroke-width="2.5" />
        <span v-else>Sign In</span>
      </button>

      <!-- Divider -->
      <div class="flex items-center gap-3 my-1">
        <div class="flex-1 h-px bg-[#27272a]"></div>
        <span class="text-xs text-[#52525b] font-medium">OR</span>
        <div class="flex-1 h-px bg-[#27272a]"></div>
      </div>

      <!-- Google button -->
      <button
        type="button"
        class="w-full h-14 rounded-xl bg-transparent border border-[#27272a] text-[#fafafa] font-semibold text-[15px]
               flex items-center justify-center gap-3
               hover:bg-[#18181b] hover:border-[#3f3f46] active:scale-[0.98]
               transition-all duration-150 focus:outline-none"
      >
        <!-- Google "G" SVG icon -->
        <svg class="w-5 h-5" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
          <path d="M22.56 12.25c0-.78-.07-1.53-.2-2.25H12v4.26h5.92c-.26 1.37-1.04 2.53-2.21 3.31v2.77h3.57c2.08-1.92 3.28-4.74 3.28-8.09z" fill="#4285F4"/>
          <path d="M12 23c2.97 0 5.46-.98 7.28-2.66l-3.57-2.77c-.98.66-2.23 1.06-3.71 1.06-2.86 0-5.29-1.93-6.16-4.53H2.18v2.84C3.99 20.53 7.7 23 12 23z" fill="#34A853"/>
          <path d="M5.84 14.09c-.22-.66-.35-1.36-.35-2.09s.13-1.43.35-2.09V7.07H2.18C1.43 8.55 1 10.22 1 12s.43 3.45 1.18 4.93l3.66-2.84z" fill="#FBBC05"/>
          <path d="M12 5.38c1.62 0 3.06.56 4.21 1.64l3.15-3.15C17.45 2.09 14.97 1 12 1 7.7 1 3.99 3.47 2.18 7.07l3.66 2.84c.87-2.6 3.3-4.53 6.16-4.53z" fill="#EA4335"/>
        </svg>
        Continue with Google
      </button>

      <!-- Error message -->
      <Transition name="slide-fade">
        <div
          v-if="errorMessage"
          class="flex items-center gap-2.5 px-4 py-3 rounded-xl bg-red-500/10 border border-red-500/20 text-red-400 text-sm"
        >
          <AlertCircle class="w-4 h-4 shrink-0" :stroke-width="2" />
          <span>{{ errorMessage }}</span>
        </div>
      </Transition>

    </div>

    <!-- Sign up link -->
    <p class="mt-10 text-sm text-[#a1a1aa]">
      Don&apos;t have an account?
      <RouterLink
        to="/login"
        class="text-[#22c55e] font-semibold hover:text-[#16a34a] transition-colors duration-150"
      >
        Sign up
      </RouterLink>
    </p>

  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { RouterLink } from 'vue-router'
import { Dumbbell, Mail, Lock, Eye, EyeOff, Loader2, AlertCircle } from '@lucide/vue'

onMounted(() => {
  document.title = 'Sign In — GymTracker'
})

const email = ref('')
const password = ref('')
const showPassword = ref(false)
const isLoading = ref(false)
const errorMessage = ref('')
const focusedField = ref<'email' | 'password' | null>(null)

async function handleLogin() {
  errorMessage.value = ''

  if (!email.value || !password.value) {
    errorMessage.value = 'Please enter your email and password.'
    return
  }

  isLoading.value = true

  // Stub: simulate network delay
  await new Promise((resolve) => setTimeout(resolve, 1200))

  console.log('[GymTracker] handleLogin —', { email: email.value })
  isLoading.value = false

  // Stub error for demo — remove when wiring real auth
  errorMessage.value = 'Invalid email or password. Please try again.'
}
</script>

<style scoped>
.slide-fade-enter-active,
.slide-fade-leave-active {
  transition: opacity 0.2s ease, transform 0.2s ease;
}
.slide-fade-enter-from,
.slide-fade-leave-to {
  opacity: 0;
  transform: translateY(-4px);
}
</style>
