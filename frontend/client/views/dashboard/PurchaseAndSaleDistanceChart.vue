<template>
  <article class="tile is-child box">
    <h4 class="title">فاصله خرید و فروش</h4>
    <div class="tile is-ancestor">
      <div class="tile is-parent" >
        <date-range :fromDate.sync="fromDate" :toDate.sync="toDate"></date-range>
      </div>
    </div>
    <div class="tile is-ancestor">
      <div class="tile is-parent" >
        <chart :type="'bar'" :data="data" :options="options" ref="chart"></chart>
      </div>
    </div>
  </article>
</template>
<script>
import Vue from 'vue'
import Statistics from './../../services/statistics'
import DateRange from './../../templates/DateRange'
import Chart from 'vue-bulma-chartjs'
export default {
  components: {
    Chart,
    DateRange
  },
  data () {
    return {
      options: {
        tooltips: {
          mode: 'label'
        }
      },
      fromDate: new Date(new Date().setDate(new Date().getDate() - 30)).toISOString(),
      toDate: new Date().toISOString(),
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
      Statistics.getPurchaseAndSalePrice(this.fromDate, this.toDate).then(response => {
        this.datas[0] = []
        this.datas[1] = []
        this.labels = []
        response.forEach(element => {
          this.datas[0].push(element.purchasesAmount)
          this.datas[1].push(element.salesAmount)
          this.labels.push(Vue.moment(element.date).locale('fa').format('jYYYY/jMM/jDD'))
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
    },
    fromDate: function () {
      this.loadAsyncData()
    },
    toDate: function () {
      this.loadAsyncData()
    }
  },
  mounted: function () {
    this.loadAsyncData()
  }
}
</script>
