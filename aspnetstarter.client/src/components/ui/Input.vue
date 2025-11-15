<script lang="ts" setup>
import {computed} from 'vue'
import {cva} from 'class-variance-authority'
import {cn} from '@/lib/utils'

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

interface InputProps {
  modelValue?: string | number
  type?: string
  variant?: NonNullable<Parameters<typeof inputVariants>[0]>['variant']
  style?: NonNullable<Parameters<typeof inputVariants>[0]>['style']
  size?: NonNullable<Parameters<typeof inputVariants>[0]>['size']
  disabled?: boolean
  placeholder?: string
  class?: string
}

const props = defineProps<InputProps>()

const emit = defineEmits(['update:modelValue'])

const classes = computed(() => cn(inputVariants({
  variant: props.variant,
  style: props.style,
  size: props.size,
}), props.class))

function onInput(e: Event) {
  const target = e.target as HTMLInputElement
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
