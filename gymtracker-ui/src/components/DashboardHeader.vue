<template>
  <header class="sticky top-0 z-10 bg-background/80 backdrop-blur-lg border-b border-border">
    <div class="px-4 py-4">
      <div class="flex items-center justify-between">
        <!-- Greeting -->
        <div>
          <p class="text-muted-foreground text-sm">Welcome back,</p>
          <h1 class="text-xl font-bold text-foreground">{{ userName }}</h1>
        </div>

        <!-- Streak Badge -->
        <div class="flex items-center gap-2 bg-accent/10 px-3 py-2 rounded-lg">
          <Flame class="w-5 h-5 text-accent" />
          <span class="text-accent font-semibold">{{ streak }} day streak</span>
        </div>
      </div>
    </div>
  </header>
</template>

<script setup lang="ts">
import { Flame } from '@lucide/vue'

defineProps<{
  userName: string
  streak: number
}>()
</script>
