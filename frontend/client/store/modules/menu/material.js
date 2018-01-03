import lazyLoading from './lazyLoading'

export default {
  name: 'کالا',
  path: '/material',
  redirect: 'material/List',
  meta: {
    icon: 'fa-rocket',
    expanded: false,
    link: 'material/index.vue'
  },
  component: lazyLoading('material', true),

  children: [
    {
      name: 'لیست کالا ها',
      path: '',
      component: lazyLoading('material/List'),
      meta: {
        show: false,
        label: 'لیست',
        link: 'material/List.vue'
      }
    },
    {
      name: 'کالای جدید',
      path: 'new',
      component: lazyLoading('material/New'),
      meta: {
        show: false,
        link: 'material/New.vue'
      }
    }
  ]
}
