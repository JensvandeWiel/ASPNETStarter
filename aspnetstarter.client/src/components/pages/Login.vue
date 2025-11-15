<script setup>
import {useAuthStore} from "@/stores/auth-store.js";
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
  } catch (err) {
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
            <Label for="email" text="Email">
              <Input
                  id="email"
                  v-model="email"
                  class="w-full"
                  placeholder="m@example.com"
                  required
                  type="email"
              />
            </Label>
          </div>
          <div class="form-control mb-4">
            <Label for="password" text="Wachtwoord">
              <Input
                  id="password"
                  v-model="password"
                  autocomplete="current-password"
                  class="w-full"
                  required
                  type="password"
              />
            </Label>
          </div>
          <p v-if="loginError" class="text-sm text-error mb-2">
            {{ loginError }}
          </p>
          <div class="card-actions mt-2">
            <Button class="w-full" type="submit" variant="primary">
              Inloggen
            </Button>
          </div>
        </form>
      </div>
    </div>
  </div>
</template>
