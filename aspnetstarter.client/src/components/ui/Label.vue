<template>
  <label :class="labelClass" :for="props.for">
    <template v-if="floating">
      <slot/>
      <span>{{ text }}</span>
    </template>
    <template v-else>
      <span v-if="position === 'before'" class="label">{{ text }}</span>
      <slot/>
      <span v-if="position === 'after'" class="label">{{ text }}</span>
    </template>
  </label>
</template>

<script lang="ts" setup>
import {computed} from 'vue';

const props = defineProps({
  text: {type: String, default: ''},
  position: {type: String as () => 'before' | 'after', default: 'before'},
  floating: {type: Boolean, default: false},
  for: {type: String, default: undefined},
  class: {type: String, default: ''},
});

const labelClass = computed(() => {
  let base = props.floating ? 'floating-label' : '';
  return [base, props.class].filter(Boolean).join(' ');
});
</script>

