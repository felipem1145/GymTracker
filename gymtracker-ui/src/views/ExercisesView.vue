<template>
  <section>
    <div class="mb-4 rounded-xl border border-[#27272a] bg-[#18181b] p-4" v-if="workoutStore.errorMessage">
      <p class="text-sm text-red-300">{{ workoutStore.errorMessage }}</p>
    </div>

    <div v-if="workoutStore.isLoading && workoutStore.exercises.length === 0" class="py-8 text-center text-sm text-[#a1a1aa]">
      Loading exercises...
    </div>

    <ExercisesList
      v-else
      :exercises="filteredExercises"
      v-model:search="searchQuery"
    />
  </section>
</template>

<script setup lang="ts">
import { computed, onMounted, ref } from 'vue'
import ExercisesList from '@/components/ExercisesList.vue'
import { useWorkoutStore } from '@/stores/workout'

const workoutStore = useWorkoutStore()
const searchQuery = ref('')

const filteredExercises = computed(() => {
  if (!searchQuery.value.trim()) {
    return workoutStore.exercises
  }

  const query = searchQuery.value.toLowerCase()
  return workoutStore.exercises.filter(
    (exercise) =>
      exercise.name.toLowerCase().includes(query) ||
      exercise.muscleGroup.toLowerCase().includes(query),
  )
})

onMounted(() => {
  if (workoutStore.exercises.length === 0) {
    void workoutStore.loadExercises()
  }
})
</script>
