import Vue from 'vue'
import axios from 'axios'
import VueAxios from 'vue-axios'
import VueAuth from '@websanova/vue-auth'
import NProgress from 'vue-nprogress'
import { sync } from 'vuex-router-sync'
import App from './App.vue'
import router from './router'
import store from './store'
import * as filters from './filters'
import { TOGGLE_SIDEBAR } from 'vuex-store/mutation-types'
import Buefy from 'buefy'
import Notification from 'vue-bulma-notification'
import VuePersianDatetimePicker from 'vue-persian-datetime-picker'
import Chart from 'chart.js'
import Vue2Filters from 'vue2-filters'

Vue.use(Vue2Filters)
Chart.defaults.global.defaultFontFamily = 'IRANSansWeb'

const NotificationComponent = Vue.extend(Notification)

Vue.prototype.$openNotification = (title = '', message = '', type = 'success') => {
  var prop = {
    title: title,
    message: message,
    type: type,
    direction: 'Left',
    duration: 4500,
    container: '.notifications'
  }
  return new NotificationComponent({
    el: document.createElement('div'),
    propsData: prop
  })
}

Vue.router = router

Vue.component('date-picker', VuePersianDatetimePicker)
Vue.use(Buefy, {
  defaultIconPack: 'fa'
  // ...
})
Vue.use(VueAxios, axios)
Vue.use(VueAuth, {
  auth: {
    request: function (req, token) {
      this.options.http._setHeaders.call(this, req, {Authorization: 'Bearer ' + token})
    },
    response: function (res) {
      // Get Token from response body
      return res.data
    }
  },
  http: require('@websanova/vue-auth/drivers/http/axios.1.x.js'),
  router: require('@websanova/vue-auth/drivers/router/vue-router.2.x.js'),
  loginData: { url: 'http://localhost:6789/login', fetchUser: false },
  refreshData: { enabled: false }
})

Vue.use(require('vue-moment-jalaali'))
Vue.use(NProgress)
// Enable devtools
Vue.config.devtools = true

sync(store, router)

const nprogress = new NProgress({ parent: '.nprogress-container' })

const { state } = store
router.beforeEach((route, redirect, next) => {
  if (state.app.device.isMobile && state.app.sidebar.opened) {
    store.commit(TOGGLE_SIDEBAR, false)
  }
  next()
})

Object.keys(filters).forEach(key => {
  Vue.filter(key, filters[key])
})

const app = new Vue({
  router,
  store,
  nprogress,
  ...App
})

export { app, router, store }
