<script setup lang="ts">
import {cn} from "@/lib/utils.ts";
const props = defineProps<{ to: string; label: string; isTitle?: boolean, class?: string, end?: boolean }>();
</script>

<template>
  <li :class="props.end ? 'mt-auto' : ''">
    <RouterLink :to="props.to" active-class="menu-active" :exact-active-class="props.to === '/' ? 'menu-active' : ''" :class="cn(props.isTitle ? 'menu-title' : '', props.class)">
      {{ props.label }}
    </RouterLink>
  </li>
</template>

<style scoped>
</style>
