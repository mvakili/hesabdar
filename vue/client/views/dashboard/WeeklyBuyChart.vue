<template>
  <article class="tile is-child box">
    <h4 class="title">مقایسه خرید هفتگی</h4>
    <div class="content">
        <chart :type="'line'" :data="weekly_buy_chart_data" :options="options"></chart>
        
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
      series: ['هفته قبل', 'هفته فعلی'],
      datas: [
        [13, 9, 7, 12, 5, 9, 6],
        [5, 1, 8.5, 8, 9, 4, 7]
      ],
      backgroundColors: [
        'rgba(240, 200, 30, 1)',
        'rgba(120, 30, 180, 1)'
      ]
    }
  },
  computed: {
    weekly_buy_chart_data () {
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
