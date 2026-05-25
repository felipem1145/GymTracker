<template>
  <Teleport to="body">
    <Transition name="overlay">
      <div
        v-if="open"
        class="fixed inset-0 z-50 bg-black/60 backdrop-blur-sm"
        @click="$emit('cancel')"
      />
    </Transition>

    <Transition name="sheet">
      <div
        v-if="open"
        class="fixed bottom-0 left-0 right-0 z-50 bg-card border-t border-border rounded-t-3xl px-6 pt-6 pb-10 safe-area-inset-bottom"
        @click.stop
      >
        <div class="w-10 h-1 bg-border rounded-full mx-auto mb-6" />

        <div class="w-14 h-14 rounded-2xl bg-green-500/10 flex items-center justify-center mx-auto mb-4">
          <CheckCircle2 class="w-7 h-7 text-green-500" />
        </div>

        <h2 class="text-xl font-bold text-foreground text-center mb-2">
          {{ title }}
        </h2>
        <p class="text-sm text-muted-foreground text-center mb-8 leading-relaxed">
          {{ message }}
        </p>

        <div class="flex flex-col gap-3">
          <button
            type="button"
            :disabled="isProcessing"
            @click.stop="$emit('confirm')"
            class="w-full py-4 bg-green-600 hover:bg-green-700 active:scale-[0.98] text-white font-semibold rounded-2xl transition-all disabled:opacity-60 disabled:cursor-not-allowed disabled:active:scale-100"
          >
            {{ isProcessing ? 'Finishing...' : confirmLabel }}
          </button>
          <button
            type="button"
            :disabled="isProcessing"
            @click.stop="$emit('cancel')"
            class="w-full py-4 bg-secondary hover:bg-secondary/80 active:scale-[0.98] text-secondary-foreground font-medium rounded-2xl transition-all disabled:opacity-60 disabled:cursor-not-allowed disabled:active:scale-100"
          >
            {{ cancelLabel }}
          </button>
        </div>
      </div>
    </Transition>
  </Teleport>
</template>

<script setup lang="ts">
import { CheckCircle2 } from '@lucide/vue'

defineProps<{
  open: boolean
  isProcessing?: boolean
  title: string
  message: string
  confirmLabel?: string
  cancelLabel?: string
}>()

defineEmits<{
  confirm: []
  cancel: []
}>()
</script>

<style scoped>
.overlay-enter-active,
.overlay-leave-active {
  transition: opacity 0.25s ease;
}
.overlay-enter-from,
.overlay-leave-to {
  opacity: 0;
}

.sheet-enter-active,
.sheet-leave-active {
  transition: transform 0.3s cubic-bezier(0.32, 0.72, 0, 1);
}
.sheet-enter-from,
.sheet-leave-to {
  transform: translateY(100%);
}

.safe-area-inset-bottom {
  padding-bottom: max(2.5rem, env(safe-area-inset-bottom));
}
</style>
