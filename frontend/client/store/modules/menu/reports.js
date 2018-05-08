import lazyLoading from './lazyLoading'

export default {
  name: 'گزارشات',
  path: '/reports',
  redirect: 'reports/Materials',
  meta: {
    icon: 'fa-bar-chart-o',
    expanded: false,
    link: 'reports/index.vue'
  },
  component: lazyLoading('reports', true),

  children: [
    {
      name: 'کالا',
      path: 'materials',
      component: lazyLoading('reports/Materials'),
      meta: {
        link: 'reports/Materials.vue'
      }
    },
    {
      name: 'مالی',
      path: 'financial',
      component: lazyLoading('reports/Financial'),
      meta: {
        link: 'reports/Financial.vue'
      }
    }
  ]
}
