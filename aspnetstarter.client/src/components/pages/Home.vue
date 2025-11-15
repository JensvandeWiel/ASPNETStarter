<script setup>
import {useUserStore} from "@/stores/user-store.js";
import {onMounted} from "vue";

const userStore = useUserStore();

onMounted(() => {
  if (!userStore.isLoaded) {
    userStore.getUser();
  }
})
</script>

<template>
  <div class="flex flex-col gap-4 h-full overflow-y-auto pb-4">
    <div class="card bg-base-100 h-32 flex-shrink-0">
      <div class="card-body">
        <h2 class="card-title">Welkom bij ASPNETStarter {{userStore.user?.userName}}</h2>
        <p>Gebruik het menu aan de linkerkant om te navigeren.</p>
      </div>
    </div>
  </div>
</template>

<style scoped>

</style>
