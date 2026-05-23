<template>
  <div class="bg-card rounded-xl border border-border overflow-hidden mb-6">
    <!-- Table Header -->
    <div class="grid grid-cols-12 gap-2 bg-secondary/50 px-4 py-3 border-b border-border text-xs font-semibold text-muted-foreground uppercase tracking-wider">
      <div class="col-span-1">Set</div>
      <div class="col-span-2">Prev</div>
      <div class="col-span-3">kg</div>
      <div class="col-span-3">Reps</div>
      <div class="col-span-2 text-center">Done</div>
      <div class="col-span-1"></div>
    </div>

    <!-- Table Rows -->
    <div class="divide-y divide-border">
      <div
        v-for="set in sets"
        :key="set.setNumber"
        class="grid grid-cols-12 gap-2 px-4 py-4 items-center bg-background hover:bg-secondary/30 transition-colors"
        :class="{ 'bg-green-500/10': set.completed }"
      >
        <!-- Set Number -->
        <div class="col-span-1">
          <span class="font-bold text-foreground text-sm">{{ set.setNumber }}</span>
        </div>

        <!-- Previous -->
        <div class="col-span-2">
          <span class="text-xs text-muted-foreground">
            {{ set.previousKg ? `${set.previousKg}×${set.previousReps}` : '—' }}
          </span>
        </div>

        <!-- kg Input -->
        <div class="col-span-3">
          <input
            :value="set.kg ?? ''"
            @input="(e) => updateSetField(set.setNumber, 'kg', (e.target as HTMLInputElement).value ? parseInt((e.target as HTMLInputElement).value) : null)"
            type="number"
            placeholder="0"
            class="w-full px-2 py-2 bg-background border border-border rounded text-foreground text-sm placeholder-muted-foreground focus:outline-none focus:ring-2 focus:ring-primary focus:border-transparent"
          />
        </div>

        <!-- Reps Input -->
        <div class="col-span-3">
          <input
            :value="set.reps ?? ''"
            @input="(e) => updateSetField(set.setNumber, 'reps', (e.target as HTMLInputElement).value ? parseInt((e.target as HTMLInputElement).value) : null)"
            type="number"
            placeholder="0"
            class="w-full px-2 py-2 bg-background border border-border rounded text-foreground text-sm placeholder-muted-foreground focus:outline-none focus:ring-2 focus:ring-primary focus:border-transparent"
          />
        </div>

        <!-- Checkmark Button -->
        <div class="col-span-2 flex justify-center">
          <button
            @click="toggleCompletion(set.setNumber)"
            class="w-8 h-8 rounded-full border-2 flex items-center justify-center transition-all"
            :class="set.completed ? 'bg-green-500 border-green-500' : 'border-border hover:border-primary'"
          >
            <Check v-if="set.completed" class="w-4 h-4 text-white" />
          </button>
        </div>

        <!-- Delete Button -->
        <div class="col-span-1 flex justify-center">
          <button
            @click="emit('delete-set', set.setNumber)"
            class="w-7 h-7 flex items-center justify-center rounded text-muted-foreground hover:text-red-500 transition-colors"
          >
            <Trash2 class="w-4 h-4" />
          </button>
        </div>
      </div>
    </div>

    <!-- Add Set Button -->
    <button
      @click="addSet"
      class="w-full px-4 py-4 text-primary font-medium text-sm hover:bg-secondary/50 transition-colors border-t border-border flex items-center justify-center gap-2"
    >
      <Plus class="w-4 h-4" />
      Add Set
    </button>
  </div>
</template>

<script setup lang="ts">
import { Check, Plus, Trash2 } from '@lucide/vue'

interface Set {
  setNumber: number
  previousKg?: number
  previousReps?: number
  kg: number | null
  reps: number | null
  completed: boolean
}

defineProps<{
  sets: Set[]
}>()

const emit = defineEmits<{
  'toggle-set': [setNumber: number]
  'update-set': [setNumber: number, field: 'kg' | 'reps', value: number | null]
  'add-set': []
  'delete-set': [setNumber: number]
}>()

const toggleCompletion = (setNumber: number) => {
  emit('toggle-set', setNumber)
}

const updateSetField = (setNumber: number, field: 'kg' | 'reps', value: number | null) => {
  emit('update-set', setNumber, field, value)
}

const addSet = () => {
  emit('add-set')
}
</script>
