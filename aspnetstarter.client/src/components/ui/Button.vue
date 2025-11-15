<script lang="ts" setup>
import {computed} from 'vue'
import {cva} from 'class-variance-authority'
import {cn} from '@/lib/utils'

const buttonVariants = cva('btn', {
  variants: {
    variant: {
      neutral: '',
      primary: 'btn-primary',
      secondary: 'btn-secondary',
      accent: 'btn-accent',
      info: 'btn-info',
      success: 'btn-success',
      warning: 'btn-warning',
      error: 'btn-error',
    },
    style: {
      outline: 'btn-outline',
      dash: 'btn-dash',
      soft: 'btn-soft',
      ghost: 'btn-ghost',
      link: 'btn-link',
    },
    size: {
      xs: 'btn-xs',
      sm: 'btn-sm',
      md: 'btn-md',
      lg: 'btn-lg',
      xl: 'btn-xl',
    },
  },
  defaultVariants: {
    variant: 'neutral',
    size: 'md',
  },
})

interface ButtonProps {
  variant?: NonNullable<Parameters<typeof buttonVariants>[0]>['variant']
  style?: NonNullable<Parameters<typeof buttonVariants>[0]>['style']
  size?: NonNullable<Parameters<typeof buttonVariants>[0]>['size']
  block?: boolean
  wide?: boolean
  square?: boolean
  circle?: boolean
  active?: boolean
  disabled?: boolean
  type?: 'button' | 'submit' | 'reset'
  class?: string
}

const props = defineProps<ButtonProps>()

const classes = computed(() => cn(
    buttonVariants({
      variant: props.variant,
      style: props.style,
      size: props.size,
    }),
    props.block && 'btn-block',
    props.wide && 'btn-wide',
    props.square && 'btn-square',
    props.circle && 'btn-circle',
    props.active && 'btn-active',
    props.disabled && 'btn-disabled',
    props.class
))
</script>

<template>
  <button
      :class="classes"
      :disabled="props.disabled"
      :type="props.type ?? 'button'"
      v-bind="$attrs"
  >
    <slot/>
  </button>
</template>
