<template>
  <div class="space-y-3">
    <div
      v-for="routine in routines"
      :key="routine.id"
      class="bg-[#18181b] rounded-2xl p-4 border border-[#27272a] hover:border-[#3f3f46] transition-all cursor-pointer active:scale-[0.99] group"
    >
      <div class="flex items-start justify-between">
        <div class="flex-1 min-w-0">
          <!-- Routine Name -->
          <div class="flex items-center gap-3 mb-2">
            <div class="w-10 h-10 rounded-xl bg-[#22c55e]/10 flex items-center justify-center flex-shrink-0">
              <Dumbbell class="w-5 h-5 text-[#22c55e]" />
            </div>
            <div class="min-w-0">
              <h3 class="text-lg font-semibold text-[#fafafa] truncate">{{ routine.name }}</h3>
              <p class="text-xs text-[#71717a]">{{ routine.lastPerformed }}</p>
            </div>
          </div>

          <!-- Target Areas -->
          <p class="text-sm text-[#a1a1aa] mb-3 line-clamp-1">
            {{ routine.targetAreas.join(' • ') }}
          </p>

          <!-- Exercise Count Badge -->
          <div class="flex items-center gap-2">
            <span class="inline-flex items-center gap-1.5 px-3 py-1 bg-[#27272a] rounded-full text-xs font-medium text-[#a1a1aa]">
              <Layers class="w-3.5 h-3.5" />
              {{ routine.exerciseCount }} exercises
            </span>
          </div>
        </div>

        <!-- Play Button -->
        <button
          class="w-12 h-12 rounded-xl bg-[#22c55e] hover:bg-[#16a34a] flex items-center justify-center flex-shrink-0 ml-4 transition-all shadow-lg shadow-[#22c55e]/20 group-hover:shadow-[#22c55e]/30"
          @click.stop="router.push('/workout')"
        >
          <Play class="w-5 h-5 text-[#09090b] ml-0.5" fill="currentColor" />
        </button>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { useRouter } from 'vue-router'
import { Dumbbell, Layers, Play } from '@lucide/vue'

const router = useRouter()

interface Routine {
  id: number
  name: string
  targetAreas: string[]
  exerciseCount: number
  lastPerformed?: string
}

defineProps<{
  routines: Routine[]
}>()
</script>
