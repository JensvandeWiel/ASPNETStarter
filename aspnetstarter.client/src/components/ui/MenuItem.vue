<script setup lang="ts">
import {defineProps} from 'vue';
import {cn} from "@/lib/utils.ts";
const props = defineProps<{ class?: string, end?: boolean }>();
</script>

<template>
  <li :class="cn(props.class, props.end ? 'mt-auto' : '')">
    <slot/>
  </li>
</template>

<style scoped>
</style>

