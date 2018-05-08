<template>
  <article class="tile is-child box">
    <h4 class="title">مقایسه خرید هفتگی</h4>
    <div class="content">
        <chart :type="'line'" :data="weekly_purchase_chart_data" :options="options"></chart>
        
    </div>
  </article>
</template>
<script>
import Statistics from './../../services/statistics'
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
      datas: [[], []],
      backgroundColors: [
        'rgba(240, 200, 30, 1)',
        'rgba(120, 30, 180, 1)'
      ]
    }
  },
  methods: {
    loadAsyncData () {
      Statistics.getTwoWeeksPurchasesPrice().then(response => {
        response.slice(0, 7).forEach(element => {
          this.datas[0].push(element.amount)
        })
        response.slice(7, 14).forEach(element => {
          this.datas[1].push(element.amount)
        })
      })
    }
  },
  computed: {
    weekly_purchase_chart_data () {
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
  },
  mounted: function () {
    this.loadAsyncData()
  }
}
</script>
