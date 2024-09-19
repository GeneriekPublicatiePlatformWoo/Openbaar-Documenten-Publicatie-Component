import { createRouter, createWebHistory } from "vue-router";
import LoginView from "@/views/LoginView.vue";
import PublicatiesView from "../views/beheer/PublicatiesView.vue";
import PublicatieView from "../views/beheer/PublicatieView.vue";
import getUser from "@/stores/user";

const resetFocus = () => {
  document.body.setAttribute("tabindex", "-1");
  document.body.focus();
  document.body.removeAttribute("tabindex");
};

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  scrollBehavior() {
    resetFocus();

    return { top: 0 };
  },
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
      path: "/beheer/publicaties/overzicht",
      name: "publicaties",
      component: PublicatiesView,
      meta: {
        title: "Publicaties",
        requiresAuth: true
      }
    },
    {
      path: "/beheer/publicaties/:id?",
      name: "publicatie",
      component: PublicatieView,
      props: true,
      meta: {
        title: "Publicatie",
        requiresAuth: true
      }
    },
    {
      path: "/beheer/gebruikersgroepen/overzicht",
      name: "gebruikersgroepen",
      component: () => import("../views/beheer/GebruikersgroepenView.vue"),
      meta: {
        title: "Gebruikersgroepen",
        requiresAuth: true
      }
    },
    {
      path: "/beheer/gebruikersgroepen/:id",
      name: "gebruikersgroep",
      component: () => import("../views/beheer/GebruikersgroepView.vue"),
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
