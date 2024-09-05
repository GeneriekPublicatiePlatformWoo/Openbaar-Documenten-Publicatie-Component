import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: HomeView
    },
    {
      path: '/beheer/gebruikersgroepen',
      name: 'gebruikersgroepen',
      component: () => import('../views/beheer/GebruikersGroepen.vue')
    },
    {
      path: '/beheer/gebruikersgroep/:id',
      name: 'gebruikersgroep',
      component: () => import('../views/beheer/GebruikersGroep.vue'),
      props: true
    },
    {
      path: '/beheer/publicaties',
      name: 'publicaties',
      component: () => import('../views/beheer/PublicatiesView.vue'),
      props: true
    }
  ]
})

export default router
