<script setup>

import MenuLink from "@/components/ui/MenuLink.vue";
import MenuItem from "@/components/ui/MenuItem.vue";
import {useAuthStore} from "@/stores/auth-store.js";
import {useRouter} from "vue-router";
import {LogOut} from 'lucide-vue-next';

const authStore = useAuthStore();
const router = useRouter();
</script>

<template>
  <aside class="w-1/6 m-4 bg-base-100 rounded-box flex flex-col">
    <ul class="menu menu-vertical gap-1 w-full h-full">
      <MenuLink class="text-base-content text-xl" isTitle label="ASPNETStarter" to="/"/>
      <MenuItem end>
        <button @click="authStore.logout(router)">
          <LogOut class="w-5 h-5"/>
          Log Uit
        </button>
      </MenuItem>
    </ul>
  </aside>
</template>

<style scoped>
</style>
