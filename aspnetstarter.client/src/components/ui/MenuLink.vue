<script lang="ts" setup>
import {cn} from "@/lib/utils.ts";

const props = defineProps<{
  to: string;
  label: string;
  isTitle?: boolean,
  class?: string,
  end?: boolean
}>();
</script>

<template>
  <li :class="props.end ? 'mt-auto' : ''">
    <RouterLink :class="cn(props.isTitle ? 'menu-title' : '', props.class)" :exact-active-class="props.to === '/' ? 'menu-active' : ''"
                :to="props.to"
                active-class="menu-active">
      {{ props.label }}
    </RouterLink>
  </li>
</template>

<style scoped>
</style>
