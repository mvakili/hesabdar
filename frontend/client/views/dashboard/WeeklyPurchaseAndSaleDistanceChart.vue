<template>
  <article class="tile is-child box">
    <h4 class="title">فاصله خرید و فروش هفتگی</h4>
    <div class="content">
        <chart :type="'line'" :data="data" :options="options" ref="chart"></chart>
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
      series: ['خرید', 'فروش'],
      datas: [[], []],
      labels: [],
      backgroundColors: [
        'rgba(240, 230, 30, 1)',
        'rgba(120, 120, 10, 1)'
      ]
    }
  },
  methods: {
    loadAsyncData () {
      Statistics.getWeeklyPurchaseAndSalePrice().then(response => {
        response.forEach(element => {
          this.datas[0].push(element.purchasesAmount)
          this.datas[1].push(element.salesAmount)
          this.labels.push(Vue.moment(element.date).locale('fa').format('dddd'))
        })
        this.options = this.options
      })
    }
  },
  computed: {
    data () {
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
        labels: this.labels.map((v, i) => {
          return v
        })
      }
    }
  },
  watch: {
    data: function () {
      this.$nextTick(() => this.$refs['chart'].resetChart())
    }
  },
  mounted: function () {
    this.loadAsyncData()
  }
}
</script>
