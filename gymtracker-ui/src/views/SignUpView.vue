<template>
  <div class="min-h-screen bg-[#09090b] flex flex-col items-center justify-center px-5 py-12">

    <!-- Branding zone -->
    <div class="flex flex-col items-center gap-4 mb-10">
      <div class="relative flex items-center justify-center w-16 h-16 rounded-2xl bg-[#22c55e]/10 border border-[#22c55e]/20">
        <div class="absolute inset-0 rounded-2xl bg-[#22c55e]/5 blur-lg"></div>
        <Dumbbell class="relative z-10 w-8 h-8 text-[#22c55e]" :stroke-width="2" />
      </div>
      <div class="text-center">
        <h1 class="text-3xl font-bold text-[#fafafa] tracking-tight">Create your account</h1>
        <p class="mt-1.5 text-sm text-[#a1a1aa] leading-relaxed">
          Sign up to track your progressive<br>overload and routines.
        </p>
      </div>
    </div>

    <!-- Card -->
    <div class="w-full max-w-sm flex flex-col gap-4">

      <!-- Full name field -->
      <div class="flex flex-col gap-1.5">
        <label for="name" class="text-xs font-medium text-[#a1a1aa] uppercase tracking-widest">Full Name</label>
        <div
          class="flex items-center gap-3 px-4 h-14 rounded-xl bg-[#18181b] border transition-colors duration-150"
          :class="focusedField === 'name' ? 'border-[#22c55e]' : 'border-[#27272a]'"
        >
          <User class="w-4 h-4 shrink-0 text-[#a1a1aa]" :stroke-width="2" />
          <input
            id="name"
            v-model="name"
            type="text"
            placeholder="John Doe"
            autocomplete="name"
            class="flex-1 bg-transparent text-[#fafafa] text-[15px] placeholder:text-[#52525b] outline-none"
            @focus="focusedField = 'name'"
            @blur="focusedField = null"
          />
        </div>
      </div>

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
            autocomplete="new-password"
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

      <!-- Confirm password field -->
      <div class="flex flex-col gap-1.5">
        <label for="confirm-password" class="text-xs font-medium text-[#a1a1aa] uppercase tracking-widest">Confirm Password</label>
        <div
          class="flex items-center gap-3 px-4 h-14 rounded-xl bg-[#18181b] border transition-colors duration-150"
          :class="focusedField === 'confirmPassword' ? 'border-[#22c55e]' : 'border-[#27272a]'"
        >
          <Lock class="w-4 h-4 shrink-0 text-[#a1a1aa]" :stroke-width="2" />
          <input
            id="confirm-password"
            v-model="confirmPassword"
            :type="showConfirmPassword ? 'text' : 'password'"
            placeholder="••••••••"
            autocomplete="new-password"
            class="flex-1 bg-transparent text-[#fafafa] text-[15px] placeholder:text-[#52525b] outline-none"
            @focus="focusedField = 'confirmPassword'"
            @blur="focusedField = null"
          />
          <button
            type="button"
            class="shrink-0 text-[#52525b] hover:text-[#a1a1aa] transition-colors duration-150 focus:outline-none"
            :aria-label="showConfirmPassword ? 'Hide password' : 'Show password'"
            @click="showConfirmPassword = !showConfirmPassword"
          >
            <Eye v-if="!showConfirmPassword" class="w-4 h-4" :stroke-width="2" />
            <EyeOff v-else class="w-4 h-4" :stroke-width="2" />
          </button>
        </div>
      </div>

      <!-- Sign up button -->
      <button
        type="button"
        class="mt-1 w-full h-14 rounded-xl bg-[#22c55e] text-[#0a0a0a] font-bold text-[15px] tracking-wide
               flex items-center justify-center gap-2
               hover:bg-[#16a34a] active:scale-[0.98]
               transition-all duration-150 focus:outline-none focus:ring-2 focus:ring-[#22c55e]/50"
        :class="{ 'opacity-60 cursor-not-allowed': isLoading }"
        :disabled="isLoading"
        @click="handleSignUp"
      >
        <Loader2 v-if="isLoading" class="w-5 h-5 animate-spin" :stroke-width="2.5" />
        <span v-else>Create Account</span>
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

    <!-- Sign in link -->
    <p class="mt-10 text-sm text-[#a1a1aa]">
      Already have an account?
      <RouterLink
        to="/login"
        class="text-[#22c55e] font-semibold hover:text-[#16a34a] transition-colors duration-150"
      >
        Sign in
      </RouterLink>
    </p>

  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { RouterLink, useRouter } from 'vue-router'
import { Dumbbell, User, Mail, Lock, Eye, EyeOff, Loader2, AlertCircle } from 'lucide-vue-next'
import { supabase } from '@/services/supabaseClient'
import { useAuthStore } from '@/stores/auth'

onMounted(() => {
  document.title = 'Sign Up — GymTracker'
})

const router = useRouter()
const authStore = useAuthStore()

const name = ref('')
const email = ref('')
const password = ref('')
const confirmPassword = ref('')
const showPassword = ref(false)
const showConfirmPassword = ref(false)
const isLoading = ref(false)
const errorMessage = ref('')
const focusedField = ref<'name' | 'email' | 'password' | 'confirmPassword' | null>(null)

async function handleSignUp() {
  errorMessage.value = ''

  if (!name.value || !email.value || !password.value || !confirmPassword.value) {
    errorMessage.value = 'Please complete all fields.'
    return
  }

  if (password.value !== confirmPassword.value) {
    errorMessage.value = 'Passwords do not match'
    return
  }

  isLoading.value = true

  try {
    const { data, error } = await supabase.auth.signUp({
      email: email.value,
      password: password.value,
      options: {
        data: {
          full_name: name.value,
        },
      },
    })

    if (error) {
      errorMessage.value = error.message
      return
    }

    authStore.setAuth({
      user: data.user ?? null,
      accessToken: data.session?.access_token ?? null,
    })

    await router.push('/')
  } catch (error) {
    errorMessage.value = error instanceof Error ? error.message : 'Unexpected error while signing up.'
  } finally {
    isLoading.value = false
  }
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