<template>
  <div>
    <dashboard-nav></dashboard-nav>
    <div class="tile is-ancestor">
      <div class="tile is-parent" >
       <weekly-sale-chart></weekly-sale-chart>
       <weekly-purchase-chart></weekly-purchase-chart>
      </div>
    </div>
    <div class="tile is-ancestor">
      <div class="tile is-parent" >
        <weekly-purchase-and-sale-distance-chart></weekly-purchase-and-sale-distance-chart>
      </div>
    </div>
  </div>
</template>

<script>
import Chart from 'vue-bulma-chartjs'
import DashboardNav from './DashboardNav'
import WeeklySaleChart from './WeeklySaleChart'
import WeeklyPurchaseChart from './WeeklyPurchaseChart'
import WeeklyPurchaseAndSaleDistanceChart from './WeeklyPurchaseAndSaleDistanceChart'

export default {
  components: {
    Chart,
    DashboardNav,
    WeeklySaleChart,
    WeeklyPurchaseChart,
    WeeklyPurchaseAndSaleDistanceChart
  }
}
</script>

<style lang="scss" scoped>
</style>
