<template>
  <div class="space-y-3">
    <div
      v-for="routine in workoutStore.routines"
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

        <div class="ml-4 flex flex-col gap-2">
          <button
            class="w-12 h-10 rounded-xl border border-[#3f3f46] bg-[#18181b] hover:bg-[#27272a] flex items-center justify-center flex-shrink-0 transition-all"
            @click.stop="handleDeleteRoutine(routine.id, routine.name)"
          >
            <Trash2 class="w-4 h-4 text-red-300" />
          </button>

          <!-- Play Button -->
          <button
            class="w-12 h-12 rounded-xl bg-[#22c55e] hover:bg-[#16a34a] flex items-center justify-center flex-shrink-0 transition-all shadow-lg shadow-[#22c55e]/20 group-hover:shadow-[#22c55e]/30"
            @click.stop="handlePlay(routine.id)"
          >
            <Play class="w-5 h-5 text-[#09090b] ml-0.5" fill="currentColor" />
          </button>
        </div>
      </div>
    </div>

    <ConfirmDialog
      :open="showDeleteDialog"
      title="Delete Routine?"
      :message="deleteDialogMessage"
      confirm-label="Yes, delete"
      cancel-label="Keep routine"
      @confirm="confirmDeleteRoutine"
      @cancel="cancelDeleteRoutine"
    />
  </div>
</template>

<script setup lang="ts">
import { computed, ref } from 'vue'
import { useRouter } from 'vue-router'
import { Dumbbell, Layers, Play, Trash2 } from '@lucide/vue'
import ConfirmDialog from '@/components/ConfirmDialog.vue'
import { useWorkoutStore } from '@/stores/workout'

const router = useRouter()
const workoutStore = useWorkoutStore()
const showDeleteDialog = ref(false)
const routineToDelete = ref<{ id: string; name: string } | null>(null)

const deleteDialogMessage = computed(() => {
  if (!routineToDelete.value) {
    return 'This action cannot be undone.'
  }

  return `Are you sure you want to delete the routine "${routineToDelete.value.name}"? This action cannot be undone.`
})

async function handlePlay(routineId: string): Promise<void> {
  await workoutStore.startWorkout(routineId)

  if (workoutStore.activeSession) {
    router.push('/workout')
  }
}

function handleDeleteRoutine(id: string, name: string): void {
  routineToDelete.value = { id, name }
  showDeleteDialog.value = true
}

function cancelDeleteRoutine(): void {
  showDeleteDialog.value = false
  routineToDelete.value = null
}

async function confirmDeleteRoutine(): Promise<void> {
  if (!routineToDelete.value) {
    return
  }

  const { id } = routineToDelete.value
  cancelDeleteRoutine()

  await workoutStore.deleteRoutine(id)
}
</script>
