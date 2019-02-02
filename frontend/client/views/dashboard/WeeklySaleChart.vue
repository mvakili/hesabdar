<template>
  <article class="tile is-child box">
    <h4 class="title">مقایسه فروش هفتگی</h4>
    <div class="content">
        <chart :type="'line'" :data="weekly_sale_chart_data" :options="options"></chart>
        
    </div>
  </article>
</template>
<script>
import Vue from 'vue'
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
      series: ['هفته قبل', 'هفته جاری'],
      datas: [[], []],
      backgroundColors: [
        'rgba(120, 200, 219, 1)',
        'rgba(151, 100, 118, 1)'
      ],
      labels: []
    }
  },
  methods: {
    loadAsyncData () {
      Statistics.getTwoWeeksSalesPrice().then(response => {
        response.slice(0, 7).forEach(element => {
          this.datas[0].push(element.amount)
          this.labels.push(Vue.moment(element.date).locale('fa').format('dddd'))
        })
        response.slice(7, 14).forEach(element => {
          this.datas[1].push(element.amount)
        })
      })
    }
  },
  computed: {
    weekly_sale_chart_data () {
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
        labels: this.labels
      }
    }
  },
  mounted: function () {
    this.loadAsyncData()
  }
}
</script>
