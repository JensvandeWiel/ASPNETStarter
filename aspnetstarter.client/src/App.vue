<script setup>
import {RouterView} from 'vue-router';
import {storeToRefs} from 'pinia';
import {NotificationTypeIcons, useNotifyStore} from "@/stores/notify-store.js";
import {XIcon} from 'lucide-vue-next';
import {cn} from "@/lib/utils.js";

const notifyStore = useNotifyStore();
const {notifications} = storeToRefs(notifyStore);
</script>

<template>
  <div class="toast toast-end toast-top absolute w-1/3 right-0 z-50">
    <div v-for="notification in notifications" :key="notification.id"
         :class="cn('flex-row w-full alert', 'alert-' + notification.type, notification.soft ? 'alert-soft' : '')"
         role="alert">
      <component :is="NotificationTypeIcons[notification.type]"/>
      <span>{{ notification.message }}</span>
      <div class="ml-auto">
        <button :class="cn('btn btn-square btn-ghost btn-sm')"
                @click="notifyStore.removeNotification(notification)">
          <XIcon class="h-5 w-5"/>
        </button>
      </div>
    </div>
  </div>
  <RouterView/>
</template>
