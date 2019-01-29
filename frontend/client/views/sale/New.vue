<template>
  <div>
      <list :table="newRowHeader">
        <template slot="table-template" slot-scope="props">

          <b-table-column field="buyer" label="خریدار" >
            <dealer-select v-model="props.row.buyer" :id.sync="props.row.buyerId" ></dealer-select>
          </b-table-column>
          <b-table-column field="dealTime" label="زمان فروش" >
            <date-picker  type="datetime" :auto-submit="true"  format="YYYY-MM-DD HH:mm" placeholder="اکنون" display-format="HH:mm jYYYY/jMM/jDD" v-model="props.row.dealTime"></date-picker>
          </b-table-column>
          <b-table-column field="dealPrice" label="قیمت فروش" >
              <input class="input" type="text" placeholder="قیمت فروش" v-model="props.row.dealPrice.amount" />        

          </b-table-column>
          <b-table-column field="dealPaymentId" label="DealPaymentId" :visible="false">
            {{props.row.dealPaymentId}}          
          </b-table-column>
          <b-table-column  label="" width="100">
            <button  class="button is-warning" @click="add">
              ذخیره
            </button>
          </b-table-column>
        </template>
        <template slot="table-bottom">
          <div class="tile is-ancestor">
            <div class=" column is-three-quarters-mobile is-two-thirds-tablet is-two-thirds-desktop is-two-third-widescreen is-half-fullhd">
              <article class="tile is-child box">
                <h4 class="title">کالا</h4>
                <deal-item-list :deal="newDeal" :dealId="null" saleOrPurchase="sale" ref="deal-item-list" @totalPriceChanged="totalPriceChanged"></deal-item-list>
              </article>
            </div>
            <div class=" column ">
              <article class="tile is-child box">
                <h4 class="title">دریافت</h4>
                <deal-payment :deal="newDeal" :paymentId="null" :newPayment="true" ref="deal-payment"></deal-payment>
              </article>
            </div>
          </div>
        </template>
      </list>
    </div>
</template>

<script>
import Deal from './../../services/deal'
import List from './../../templates/List'
import DealItemList from './../deal/dealItem/List'
import DealPayment from './../deal/payment/index'
import DealerSelect from './../dealer/Select'
export default {
  components: {
    DealItemList,
    DealPayment,
    DealerSelect,
    List
  },
  data () {
    return {
      newDeal: {}
    }
  },
  computed: {
    newRowHeader: function () {
      return {
        data: [this.newDeal]
      }
    }
  },
  methods: {
    add () {
      console.log(this.newDeal.items.length)
      console.log(this.newDeal.buyerId)
      if (!this.newDeal.buyerId) {
        this.$openNotification('عملیات ناموفق', 'خریدار انتخاب نشده است', 'danger')
      } else {
        Deal.addNewSale(this.newDeal).then(res => {
          this.$openNotification('عملیات موفق', 'تغییرات ذخیره شد', 'success')
          this.$emit('onSuccess', res.data)
          this.initNewDeal()
          this.$refs['deal-item-list'].deal = this.newDeal
          this.$refs['deal-item-list'].loadAsyncData()
          this.$refs['deal-payment'].deal = this.newDeal
          this.$refs['deal-payment'].paymentId = null
        }).catch(err => {
          this.$openNotification('عملیات ناموفق', 'دوباره سعی کنید', 'danger')
          this.$emit('onFail', err)
        })
      }
    },
    totalPriceChanged (value) {
      this.newDeal.dealPrice.amount = value
    },
    initNewDeal () {
      this.newDeal = {
        dealPayment: {
          method: 'Cash',
          paid: true,
          amount: 0
        },
        items: [],
        dealPrice: {
          amount: 0
        },
        buyerId: 0,
        buyer: {
        },
        buyeeId: 0,
        buyee: {
        }
      }
    }
  },
  watch: {
    'newDeal.dealPrice.amount': function (val) {
      this.newDeal.dealPayment.amount = val
    }
  },
  beforeMount: function () {
    this.initNewDeal()
  }
}
</script>

<style lang="scss" scoped>
@import '~bulma/sass/utilities/mixins';
.left-notification {
  left: 0px;
}
.control .button {
  margin: inherit;
}
.control.has-addons {
  @include mobile() {
    input {
      width: 100%;
    }
    input.is-expanded {
      flex-shrink: 1;
    }
  }
}
</style>
