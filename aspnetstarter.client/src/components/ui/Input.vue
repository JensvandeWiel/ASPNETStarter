<script setup>
import {computed} from 'vue'
import {cva} from 'class-variance-authority'
import {cn} from '@/lib/utils.js'

const inputVariants = cva('input', {
  variants: {
    variant: {
      neutral: '',
      primary: 'input-primary',
      secondary: 'input-secondary',
      accent: 'input-accent',
      info: 'input-info',
      success: 'input-success',
      warning: 'input-warning',
      error: 'input-error',
    },
    style: {
      ghost: 'input-ghost',
    },
    size: {
      xs: 'input-xs',
      sm: 'input-sm',
      md: 'input-md',
      lg: 'input-lg',
      xl: 'input-xl',
    },
  },
  defaultVariants: {
    variant: 'neutral',
    size: 'md',
  },
})

/**
 * @typedef {Object} InputProps
 * @property {string|number} [modelValue]
 * @property {string} [type]
 * @property {string} [variant='neutral']
 * @property {string} [style]
 * @property {string} [size='md']
 * @property {boolean} [disabled]
 * @property {string} [placeholder]
 * @property {string} [class]
 */

const props = defineProps({
  modelValue: {type: [String, Number], default: null},
  type: {type: String, default: 'text'},
  variant: {type: String, default: 'neutral'},
  style: {type: String, default: null},
  size: {type: String, default: 'md'},
  disabled: {type: Boolean, default: false},
  placeholder: {type: String, default: null},
  class: {type: String, default: null},
})

const emit = defineEmits(['update:modelValue'])

const classes = computed(() => cn(inputVariants({
  variant: props.variant,
  style: props.style,
  size: props.size,
}), props.class))

function onInput(e) {
  const target = e.target
  emit('update:modelValue', target.value)
}

</script>

<template>
  <input
      :class="classes"
      :disabled="props.disabled"
      :placeholder="props.placeholder"
      :type="props.type ?? 'text'"
      :value="props.modelValue"
      v-bind="$attrs"
      @input="onInput"
  />
</template>

<style scoped>
/* Add custom styles if needed */
</style>
