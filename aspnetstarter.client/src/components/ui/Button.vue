<script setup>
import {computed} from 'vue'
import {cva} from 'class-variance-authority'
import {cn} from '@/lib/utils.js'

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

/**
 * @typedef {Object} ButtonProps
 * @property {string} [variant='neutral']
 * @property {string} [style]
 * @property {string} [size='md']
 * @property {boolean} [block]
 * @property {boolean} [wide]
 * @property {boolean} [square]
 * @property {boolean} [circle]
 * @property {boolean} [active]
 * @property {boolean} [disabled]
 * @property {string} [type='button']
 * @property {string} [class]
 */

const props = defineProps({
  variant: {type: String, default: 'neutral'},
  style: {type: String, default: null},
  size: {type: String, default: 'md'},
  block: {type: Boolean, default: false},
  wide: {type: Boolean, default: false},
  square: {type: Boolean, default: false},
  circle: {type: Boolean, default: false},
  active: {type: Boolean, default: false},
  disabled: {type: Boolean, default: false},
  type: {type: String, default: 'button'},
  class: {type: String, default: null},
})

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
