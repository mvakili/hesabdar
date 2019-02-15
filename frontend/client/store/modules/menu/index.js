import * as types from '../../mutation-types'
import lazyLoading from './lazyLoading'

// import uifeatures from './uifeatures'
// import components from './components'
// import tables from './tables'

// show: meta.label -> name
// name: component name
// meta.label: display label

const state = {
  items: [
    {
      name: 'خانه',
      path: '/dashboard',
      meta: {
        icon: 'fa-tachometer',
        link: 'dashboard/index.vue'
      },
      component: lazyLoading('dashboard', true)
    },
    {
      name: 'خرید',
      path: '/purchase',
      meta: {
        icon: 'fa-shopping-bag',
        link: 'purchase/index.vue'
      },
      component: lazyLoading('purchase', true)
    },
    {
      name: 'فروش',
      path: '/sale',
      meta: {
        icon: 'fa-shopping-cart',
        link: 'sale/index.vue'
      },
      component: lazyLoading('sale', true)
    },
    {
      name: 'طرف‌حساب',
      path: '/dealers',
      meta: {
        icon: 'fa-male ',
        link: 'dealer/index.vue'
      },
      component: lazyLoading('dealer', true)
    },
    {
      name: 'کالا',
      path: '/material',
      meta: {
        icon: 'fa-barcode ',
        link: 'material/index.vue'
      },
      component: lazyLoading('material', true)
    },
    {
      name: 'پرداخت',
      path: '/payments',
      meta: {
        icon: 'fa-dollar ',
        link: 'payment/index.vue'
      },
      component: lazyLoading('payment', true)
    }
  ]
}

const mutations = {
  [types.EXPAND_MENU] (state, menuItem) {
    if (menuItem.index > -1) {
      if (state.items[menuItem.index] && state.items[menuItem.index].meta) {
        state.items[menuItem.index].meta.expanded = menuItem.expanded
      }
    } else if (menuItem.item && 'expanded' in menuItem.item.meta) {
      menuItem.item.meta.expanded = menuItem.expanded
    }
  }
}

export default {
  state,
  mutations
}
