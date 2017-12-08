<template>
    <article class="tile is-child box">
        <h4 class="title">مقایسه فروش هفتگی</h4>
        <div class="content">
            <chart :type="'line'" :data="weekly_sell_chart_data" :options="options"></chart>
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
        [12, 9, 7, 8, 5, 5, 4],
        [2, 1, 3.5, 7, 3, 4, 2]
      ],
      backgroundColors: [
        'rgba(120, 200, 219, 1)',
        'rgba(151, 100, 118, 1)'
      ]
    }
  },
  computed: {
    weekly_sell_chart_data () {
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
