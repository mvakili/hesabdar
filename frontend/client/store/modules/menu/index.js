import * as types from '../../mutation-types'
import lazyLoading from './lazyLoading'
import reports from './reports'
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
      path: '/buy',
      meta: {
        icon: 'fa-rocket',
        link: 'buy/index.vue'
      },
      component: lazyLoading('buy', true)
    },
    {
      name: 'فروش',
      path: '/sell',
      meta: {
        icon: 'fa-rocket',
        link: 'sell/index.vue'
      },
      component: lazyLoading('sell', true)
    },
    reports,
    // uifeatures,
    // components,
    // tables
    {
      name: 'خریدار - فروشنده',
      path: '/dealers',
      meta: {
        icon: 'fa-rocket',
        link: 'dealer/index.vue'
      },
      component: lazyLoading('dealer', true)
    },
    {
      name: 'اجناس',
      path: '/material',
      meta: {
        icon: 'fa-rocket',
        link: 'material/index.vue'
      },
      component: lazyLoading('material', true)
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
