<template>
  <nav class="level is-mobile">
      <div class="level-item has-text-centered notification is-warning">
        <div>
          <p class="heading">تعداد فاکتور فروش امروز</p>
          <p class="title">{{statistics.todaySaleInvoicesCount | currency('', 0) || '-'}}</p>
          <p class="is-size-6">عدد</p>
        </div>
      </div>
      <div class="level-item has-text-centered notification is-warning">
        <div>
          <p class="heading">فروش امروز</p>
          <p class="title">{{statistics.todaySalesPrice | currency('', 0) || '-'}}</p>
          <p class="is-size-6">&nbsp;</p>
          
        </div>
      </div>
      <div class="level-item has-text-centered  notification is-primary">
        <div>
          <p class="heading">تعداد فاکتور خرید امروز</p>
          <p class="title">{{statistics.todayPurchaseInvoicesCount | currency('', 0) || '-'}}</p>
          <p class="is-size-6">عدد</p>
        </div>
      </div>
      <div class="level-item has-text-centered  notification is-primary">
        <div>
          <p class="heading">خرید امروز</p>
          <p class="title">{{statistics.todayPurchasePrice | currency('', 0) || '-'}}</p>
          <p class="is-size-6">&nbsp;</p>
        </div>
      </div>
    </nav>
</template>
<script>
import Statistics from './../../services/statistics'
export default {
  data () {
    return {
      statistics: {

      }
    }
  },
  methods: {
    loadAsyncData () {
      Statistics.getGeneral().then(response => {
        this.statistics = response
      })
    }
  },
  mounted: function () {
    this.loadAsyncData()
  }
}
</script>
<style scoped>

</style>
