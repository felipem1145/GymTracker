<template>
  <div class="grid grid-cols-3 gap-3 mb-6">
    <div class="bg-card rounded-xl p-4 border border-border">
      <div class="flex items-center gap-2 mb-2">
        <div class="w-8 h-8 rounded-lg bg-primary/10 flex items-center justify-center">
          <Activity class="w-4 h-4 text-primary" />
        </div>
      </div>
      <p class="text-2xl font-bold text-foreground">{{ stats.workouts }}</p>
      <p class="text-xs text-muted-foreground">Workouts</p>
    </div>

    <div class="bg-card rounded-xl p-4 border border-border">
      <div class="flex items-center gap-2 mb-2">
        <div class="w-8 h-8 rounded-lg bg-accent/10 flex items-center justify-center">
          <TrendingUp class="w-4 h-4 text-accent" />
        </div>
      </div>
      <p class="text-2xl font-bold text-foreground">{{ formattedWeight }}</p>
      <p class="text-xs text-muted-foreground">Total kg</p>
    </div>

    <div class="bg-card rounded-xl p-4 border border-border">
      <div class="flex items-center gap-2 mb-2">
        <div class="w-8 h-8 rounded-lg bg-secondary flex items-center justify-center">
          <Clock class="w-4 h-4 text-foreground" />
        </div>
      </div>
      <p class="text-2xl font-bold text-foreground">{{ formattedDuration }}</p>
      <p class="text-xs text-muted-foreground">Minutes</p>
    </div>
  </div>
</template>

<script setup lang="ts">
import { computed } from 'vue'
import { Activity, TrendingUp, Clock } from '@lucide/vue'

const props = defineProps<{
  stats: {
    workouts: number
    totalWeight: number
    duration: number
  }
}>()

const formattedWeight = computed(() => {
  if (props.stats.totalWeight >= 1000) {
    return (props.stats.totalWeight / 1000).toFixed(1) + 'k'
  }
  return props.stats.totalWeight.toString()
})

const formattedDuration = computed(() => props.stats.duration.toString())
</script>
