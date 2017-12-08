<template>
  <div>
    <dashboard-nav></dashboard-nav>
    <div class="tile is-ancestor">
      <div class="tile is-parent" >
       <weekly-sell-chart></weekly-sell-chart>
       <weekly-buy-chart></weekly-buy-chart>
      </div>
    </div>
    <div class="tile is-ancestor">
      <div class="tile is-parent" >
        <weekly-buy-sell-distance-chart></weekly-buy-sell-distance-chart>
      </div>
    </div>
  </div>
</template>

<script>
import Chart from 'vue-bulma-chartjs'
import DashboardNav from './DashboardNav'
import WeeklySellChart from './WeeklySellChart'
import WeeklyBuyChart from './WeeklyBuyChart'
import WeeklyBuySellDistanceChart from './WeeklyBuySellDistanceChart'

export default {
  components: {
    Chart,
    DashboardNav,
    WeeklySellChart,
    WeeklyBuyChart,
    WeeklyBuySellDistanceChart
  }
}
</script>

<style lang="scss" scoped>
</style>
