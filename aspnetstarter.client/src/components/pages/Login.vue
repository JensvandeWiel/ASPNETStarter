<script lang="ts" setup>
import {useAuthStore} from "@/stores/auth-store.ts";
import {useRouter} from "vue-router";
import {onMounted, ref} from "vue";
import Button from "@/components/ui/Button.vue";
import Input from "@/components/ui/Input.vue";
import Label from "@/components/ui/Label.vue";

const authStore = useAuthStore()
const router = useRouter()
const email = ref("")
const password = ref("")
const loginError = ref("")
const doLogin = async () => {
  loginError.value = "";
  try {
    await authStore.login(email.value, password.value)
    await router.push("/")
  } catch (err: any) {
    loginError.value = err.message
    password.value = ""
  }
}

onMounted(() => {
  if (authStore.isAuthenticated) {
    router.push("/");
  }
});
</script>

<template>
  <div class="flex flex-col items-center justify-center min-h-screen">
    <div class="card w-96 bg-base-100 shadow-sm">
      <div class="card-body">
        <h2 class="card-title text-2xl mb-2">Login</h2>
        <p class="mb-4">Voer je inloggegevens in om verder te gaan.</p>
        <form @submit.prevent="doLogin">
          <div class="form-control mb-4">
            <Label text="Email" for="email">
              <Input
                id="email"
                v-model="email"
                type="email"
                placeholder="m@example.com"
                required
                class="w-full"
              />
            </Label>
          </div>
          <div class="form-control mb-4">
            <Label text="Wachtwoord" for="password">
              <Input
                id="password"
                v-model="password"
                type="password"
                required
                class="w-full"
                autocomplete="current-password"
              />
            </Label>
          </div>
          <p v-if="loginError" class="text-sm text-error mb-2">
            {{ loginError }}
          </p>
          <div class="card-actions mt-2">
            <Button type="submit" variant="primary" class="w-full">
              Inloggen
            </Button>
          </div>
        </form>
      </div>
    </div>
  </div>
</template>
