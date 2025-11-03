<script lang="ts" setup>
import { RouterView } from 'vue-router';
import { storeToRefs } from 'pinia';
import {NotificationTypeIcons, useNotifyStore} from "@/stores/notify-store.ts";
import { XIcon } from 'lucide-vue-next';
import {cn} from "@/lib/utils.ts";
const notifyStore = useNotifyStore();
const { notifications } = storeToRefs(notifyStore);
</script>

<template>
  <div class="toast toast-end toast-top absolute w-1/3 right-0 z-50">
    <div role="alert" v-for="notification in notifications" :class="cn('flex-row w-full alert', 'alert-' + notification.type, notification.soft ? 'alert-soft' : '')">
      <component :is="NotificationTypeIcons[notification.type]" />
      <span>{{ notification.message }}</span>
      <div class="ml-auto">
        <button @click="notifyStore.removeNotification(notification)" :class="cn('btn btn-square btn-ghost btn-sm')">
          <XIcon class="h-5 w-5" />
        </button>
      </div>
    </div>
  </div>
  <RouterView/>
</template>
