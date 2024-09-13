import { ref } from "vue";
import { createRouter, createWebHistory } from "vue-router";
import LoginView from "@/views/LoginView.vue";
import PublicatiesView from "../views/beheer/PublicatiesView.vue";
import getUser, { type User } from "@/stores/user";

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: "/",
      redirect: { name: "publicaties" }
    },
    {
      path: "/login",
      name: "login",
      component: LoginView,
      meta: {
        title: "Login"
      }
    },
    {
      path: "/beheer/publicaties",
      name: "publicaties",
      component: PublicatiesView,
      meta: {
        title: "Publicaties",
        requiresAuth: true
      }
    },
    {
      path: "/beheer/gebruikersgroepen",
      name: "gebruikersgroepen",
      component: () => import("../views/beheer/GebruikersGroepen.vue"),
      meta: {
        title: "Gebruikersgroepen",
        requiresAuth: true
      }
    },
    {
      path: "/beheer/gebruikersgroep/:id",
      name: "gebruikersgroep",
      component: () => import("../views/beheer/GebruikersGroep.vue"),
      props: true,
      meta: {
        title: "Gebruikersgroep",
        requiresAuth: true
      }
    }
  ]
});

router.beforeEach(async (to) => {
  document.title = `${to.meta?.title || ""} | ${import.meta.env.VITE_APP_TITLE}`;

  const requiresAuth = to.matched.some((route) => route.meta.requiresAuth);

  const user = await getUser(false);
  if (requiresAuth && !user?.isLoggedIn) {
    return { name: "login" };
  }
});

export default router;
