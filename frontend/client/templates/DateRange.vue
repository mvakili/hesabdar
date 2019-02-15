<template>
    <div class="columns">
      <div class="column">
        <date-picker  type="date" :auto-submit="true"  format="YYYY-MM-DD " placeholder="اکنون" display-format="jYYYY/jMM/jDD" v-model="from" >
          <div slot="label">
            <span>از</span>
          </div>
        </date-picker>
      </div>
      <div class="column">
        <date-picker  type="date" :auto-submit="true"  format="YYYY-MM-DD " placeholder="اکنون" display-format="jYYYY/jMM/jDD" v-model="to" >
          <div slot="label">
            <span>تا</span>
          </div>
        </date-picker>
      </div>
    </div>
</template>
<script>
export default {
  name: 'DateRange',
  props: {
    fromDate: {
      type: String,
      default: new Date().toString()
    },
    toDate: {
      type: String,
      default: new Date().toString()
    }
  },
  data () {
    return {
      from: new Date().toString(),
      to: new Date().toString()
    }
  },
  watch: {
    fromDate () {
      this.from = this.fromDate
    },
    toDate () {
      this.to = this.toDate
    },
    from () {
      this.$emit('update:fromDate', this.from)
    },
    to () {
      this.$emit('update:toDate', this.to)
    }
  },
  mounted: function () {
    this.from = this.fromDate
    this.to = this.toDate
  }
}
</script>
