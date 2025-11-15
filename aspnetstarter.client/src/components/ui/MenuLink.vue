<script setup>
import {cn} from "@/lib/utils.js";

const props = defineProps({
  to: {type: String, required: true},
  label: {type: String, required: true},
  isTitle: {type: Boolean, default: false},
  class: {type: String, default: null},
  end: {type: Boolean, default: false},
});
</script>

<template>
  <li :class="props.end ? 'mt-auto' : ''">
    <RouterLink :class="cn(props.isTitle ? 'menu-title' : '', props.class)"
                :exact-active-class="props.to === '/' ? 'menu-active' : ''"
                :to="props.to"
                active-class="menu-active">
      {{ props.label }}
    </RouterLink>
  </li>
</template>

<style scoped>
</style>
