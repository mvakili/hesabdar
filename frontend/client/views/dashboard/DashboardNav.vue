<template>
  <nav class="level is-mobile">
      <div class="level-item has-text-centered notification is-warning">
        <div>
          <p class="heading">فاکتور فروش امروز</p>
          <p class="title">{{sell_invoices_count}}</p>
          <p class="is-size-6">عدد</p>
        </div>
      </div>
      <div class="level-item has-text-centered notification is-warning">
        <div>
          <p class="heading">فروش امروز</p>
          <p class="title">{{today_sell}}</p>
          <p class="is-size-6">{{unit.name}}</p>
        </div>
      </div>
      <div class="level-item has-text-centered  notification is-primary">
        <div>
          <p class="heading">فاکتور خرید امروز</p>
          <p class="title">{{buy_invoices_count}}</p>
          <p class="is-size-6">عدد</p>
        </div>
      </div>
      <div class="level-item has-text-centered  notification is-primary">
        <div>
          <p class="heading">خرید امروز</p>
          <p class="title">{{today_buy}}</p>
          <p class="is-size-6">{{unit.name}}</p>
        </div>
      </div>
    </nav>
</template>
<script>
export default {
  data () {
    return {
      today_sell: 10028,
      today_buy: 17000,
      unit: {
        name: 'ریال',
        id: '1'
      },
      sell_invoices_count: 10,
      buy_invoices_count: 3
    }
  }
}
</script>
<style scoped>

</style>
