<template>
  <article class="tile is-child box">
    <h4 class="title">فاصله خرید و فروش هفتگی</h4>
    <div class="content">
        <chart :type="'line'" :data="weekly_buy_sell_distance_chart_data" :options="options"></chart>
    </div>
  </article>
</template>
<script>
import Chart from 'vue-bulma-chartjs'

export default {
  components: {
    Chart
  },
  data () {
    return {
      options: {
        tooltips: {
          mode: 'label'
        }
      },
      series: ['فروش', 'خرید'],
      datas: [
        [5, 9, 7, 8, 3, 9, 4],
        [2, 8, 15, 7, 3, 9, 7]
      ],
      backgroundColors: [
        'rgba(240, 230, 30, 1)',
        'rgba(120, 120, 10, 1)'
      ]
    }
  },
  computed: {
    weekly_buy_sell_distance_chart_data () {
      return {
        datasets: this.series.map((e, i) => {
          return {
            data: this.datas[i],
            label: this.series[i],
            borderColor: this.backgroundColors[i].replace(/1\)$/, '1)'),
            pointBackgroundColor: this.backgroundColors[i],
            backgroundColor: this.backgroundColors[i].replace(/1\)$/, '.5)')
          }
        }),
        labels: ['شنبه', 'یکشنبه', 'دو شنبه', 'سه شنبه', 'چارشنبه', 'پنج شنبه', 'جمعه']
      }
    }
  }
}
</script>
