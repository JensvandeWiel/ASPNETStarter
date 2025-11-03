import './main.css'

import { createApp } from 'vue'
import { createPinia } from 'pinia'
import App from './App.vue'
import AppLayout from "@/components/layouts/AppLayout.vue";
import {createRouter, createWebHistory} from "vue-router";
import Login from "@/components/pages/Login.vue";
import {useAuthStore} from "@/stores/auth-store.ts";
import {useUserStore} from "@/stores/user-store.ts";
import WrongRolePage from "@/components/pages/WrongRolePage.vue";
import Home from "@/components/pages/Home.vue";

const routes = [
  {path: '/',
    component: AppLayout,
    children: [
      {path: '', component: Home, meta: {requiresAuth: true}, name: 'Thuis'},
    ]
  },
  {path: '/wrongrole', component: WrongRolePage},
  {path: '/login', component: Login, meta: {requiresAuth: false}, name: 'Inloggen'},
  {path: '/:pathMatch(.*)*', redirect: '/'},
]

const app = createApp(App)
const pinia = createPinia()
app.use(pinia)

const authStore = useAuthStore();

const router = createRouter({
  history: createWebHistory(),
  routes,
})
// Replace single auth check with ordered checks:
// 1) requiresAuth (redirect to /login if not authenticated)
// 2) requiresOneOfRoles (fetches user if needed and checks roles)
router.beforeEach(async (to, _, next) => {
  // 1) Auth check
  if (to.meta['requiresAuth'] && !authStore.isAuthenticated) {
    return next('/login');
  }

  // 2) Roles check (only if requiresAuth is true AND we're authenticated)
  const requiredRoles = to.meta['requiresOneOfRoles'] as string[] | undefined;
  if (to.meta['requiresAuth'] && requiredRoles && requiredRoles.length > 0) {
    const userStore = useUserStore();

    try {
      await userStore.getUser();
    } catch (e) {
      // If fetching user failed (e.g. token expired), redirect to login
      return next('/login');
    }

    const userRoles = userStore.currentUser?.roles || [];
    const hasRequiredRole = requiredRoles.some(r => userRoles.includes(r));

    if (!hasRequiredRole) {
      // If user doesn't have role and isn't already on wrongrole page, go there
      if (to.path !== '/wrongrole') {
        return next('/wrongrole');
      }
    }
  }

  return next();
})

// Set title based on route name
router.afterEach((to) => {
  const defaultTitle = 'RevaliNow';
  document.title = to.name ? `${String(to.name)} - ${defaultTitle}` : defaultTitle;
});

app.use(router)
app.mount('#app')

export { router };
