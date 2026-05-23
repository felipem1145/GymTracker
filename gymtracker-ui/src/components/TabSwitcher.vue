<template>
  <div class="flex bg-[#18181b] rounded-xl p-1 gap-1">
    <button
      v-for="tab in tabs"
      :key="tab.id"
      @click="$emit('update:modelValue', tab.id)"
      :class="[
        'flex-1 py-3 px-4 rounded-lg font-medium text-sm transition-all flex items-center justify-center gap-2',
        modelValue === tab.id
          ? 'bg-[#27272a] text-[#fafafa] shadow-sm'
          : 'text-[#a1a1aa] hover:text-[#fafafa]'
      ]"
    >
      <component :is="getIcon(tab.icon)" class="w-4 h-4" />
      {{ tab.label }}
    </button>
  </div>
</template>

<script setup lang="ts">
import { FolderOpen, Dumbbell } from '@lucide/vue'

interface Tab {
  id: string
  label: string
  icon: string
}

defineProps<{
  modelValue: string
  tabs: Tab[]
}>()

defineEmits<{
  'update:modelValue': [value: string]
}>()

const getIcon = (icon: string) => {
  const icons: Record<string, typeof FolderOpen> = {
    folder: FolderOpen,
    dumbbell: Dumbbell,
  }
  return icons[icon] || FolderOpen
}
</script>
