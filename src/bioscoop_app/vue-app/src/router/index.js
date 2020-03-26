import Vue from 'vue'
import Router from 'vue-router'
import Home from "../components/Home";
import Admin from "../components/Admin";
import AddMovie from "../components/movie/AddMovie";


Vue.use(Router)

export default new Router({
  mode: 'history',
  routes: [
    {
      path: '/',
      name: 'Home',
      component: Home
    },
    {
      path: '/admin',
      name: 'Admin',
      component: Admin
    },
    {
      path: '/admin/movie/add',
      name: 'Movie',
      component: AddMovie
    }
  ]
})
