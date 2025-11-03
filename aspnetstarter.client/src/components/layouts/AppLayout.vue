<script lang="ts" setup>
import Menu from "@/components/ui/Menu.vue";
</script>

<template>
  <div class="flex h-screen max-h-screen overflow-hidden bg-base-200">
    <Menu/>
    <main class="flex-1 overflow-y-auto m-4">
      <RouterView/>
    </main>
  </div>
</template>
