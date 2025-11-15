<script setup>
import {defineProps} from 'vue';
import {cn} from "@/lib/utils.js";

const props = defineProps({
  class: {type: String, default: null},
  end: {type: Boolean, default: false},
});
</script>

<template>
  <li :class="cn(props.class, props.end ? 'mt-auto' : '')">
    <slot/>
  </li>
</template>

<style scoped>
</style>

