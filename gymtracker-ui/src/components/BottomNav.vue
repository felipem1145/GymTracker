<template>
  <nav class="fixed bottom-0 left-0 right-0 bg-background/95 backdrop-blur-lg border-t border-border safe-area-inset-bottom">
    <div class="flex items-center justify-around py-2 px-4">
      <RouterLink
        v-for="item in navItems"
        :key="item.id"
        :to="item.to"
        custom
        v-slot="{ navigate, isActive }"
      >
        <button
          @click="navigate"
          :class="[
            'flex flex-col items-center gap-1 py-2 px-4 rounded-xl transition-all duration-200',
            isActive ? 'text-primary' : 'text-muted-foreground hover:text-foreground',
          ]"
        >
          <component
            :is="item.icon"
            :class="['w-6 h-6', isActive ? 'text-primary' : '']"
            :fill="isActive ? 'currentColor' : 'none'"
          />
          <span class="text-xs font-medium">{{ item.label }}</span>
        </button>
      </RouterLink>
    </div>
  </nav>
</template>

<script setup lang="ts">
import { RouterLink } from 'vue-router'
import { Home, Dumbbell, BarChart3, User } from '@lucide/vue'

const navItems = [
  { id: 'home', label: 'Home', icon: Home, to: '/' },
  { id: 'routines', label: 'Routines', icon: Dumbbell, to: '/routines' },
  { id: 'progress', label: 'Progress', icon: BarChart3, to: '/progress' },
  { id: 'profile', label: 'Profile', icon: User, to: '/profile' },
]
</script>

<style scoped>
.safe-area-inset-bottom {
  padding-bottom: max(0.5rem, env(safe-area-inset-bottom));
}
</style>
