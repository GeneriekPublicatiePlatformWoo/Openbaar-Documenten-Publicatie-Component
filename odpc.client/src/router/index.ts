import { ref } from "vue";
import { createRouter, createWebHistory } from "vue-router";
import HomeView from "../views/HomeView.vue";
import LoginView from "@/views/LoginView.vue";
import getUser, { type User } from "@/stores/user";

const user = ref<User | null>(await getUser());

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: "/",
      name: "home",
      component: HomeView
    },
    {
      path: "/login",
      name: "login",
      component: LoginView
    },
    {
      path: "/beheer/publicaties",
      name: "publicaties",
      component: () => import("../views/beheer/PublicatiesView.vue"),
      props: true,
      meta: {
        requiresAuth: true
      }
    },
    {
      path: "/beheer/gebruikersgroepen",
      name: "gebruikersgroepen",
      component: () => import("../views/beheer/GebruikersGroepen.vue")
    },
    {
      path: "/beheer/gebruikersgroep/:id",
      name: "gebruikersgroep",
      component: () => import("../views/beheer/GebruikersGroep.vue"),
      props: true
    }
  ]
});

router.beforeEach(async (to) => {
  const requiresAuth = to.matched.some((route) => route.meta.requiresAuth);

  if (requiresAuth && !user.value?.isLoggedIn) {
    return { name: "login" };
  }
});

export default router;
