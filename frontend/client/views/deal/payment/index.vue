<template>
  <div>
    <section>
      <form>
        <div class="tile is-ancestor">
          <div class="tile is-parent is-12">
            <nav></nav>
          </div>
        </div>
        <div class="box">
          <div class="control is-horizontal">
            <p class="control">
              <label class="radio">
                <b-radio  v-model="deal.dealPayment.method" native-value="Cash" name="paymentType" />
                نقدی
              </label>
              &nbsp;&nbsp;&nbsp;
              <label class="radio">
                <b-radio  v-model="deal.dealPayment.method" native-value="Cheque" name="paymentType" />
                چک
              </label>
            </p>
            <p class="control">
              <label class="radio">
                <vb-switch v-model="deal.dealPayment.paid" type="primary" ></vb-switch> &nbsp;
                <span v-if="deal.dealPayment.paid">
                  پرداخت شده
                </span>
                <span v-else>
                  پرداخت نشده
                </span>
              </label>
            </p>
          </div>
          <p>&nbsp;</p>
          <div class="contol" v-if="deal.dealPayment.method == 'Cheque'">
            <div class="columns">
              <div  class="column">
                سررسید چک :
              </div>
              <div class="control column is-two-thirds">
                <date-picker type="date" :auto-submit="true" :editable="true" format="YYYY-MM-DD HH:mm" display-format="jYYYY/jMM/jDD" :placeholder="newPayment ? 'اکنون' : ''" v-model="deal.dealPayment.dueDate"></date-picker>
              </div>
            </div>
          </div>
          <p>&nbsp;</p>
          <template v-if="deal.dealPayment.method == 'Cheque' || (deal.dealPayment.method == 'Cash' && deal.dealPayment.paid)">
            <div class="control">
              <div class="columns">
                <div  class="column">
                  مقدار پرداخت : 
                </div>
                <div class="control column is-two-thirds">
                  <input class="input" placeholder="مقدار پرداخت" v-model="deal.dealPayment.amount">
                </div>
              </div>
            </div>
            <div class="contol">
              <div class="columns">
                <div  class="column">
                  تاریخ پرداخت: 
                </div>
                <div class="control column is-two-thirds">
                  <date-picker type="datetime" :auto-submit="true" :editable="true" format="YYYY-MM-DD HH:mm" display-format="HH:mm jYYYY/jMM/jDD" :placeholder="newPayment ? 'اکنون' : ''" v-model="deal.dealPayment.payDate"></date-picker>
                </div>
              </div>
            </div>
          </template>
        </div>
      </form>
    </section>
  </div>
</template>

<script>
  import VbSwitch from 'vue-bulma-switch'
  import Payment from './../../../services/payment'

  export default {
    components: {
      VbSwitch
    },
    props: {
      paymentId: {
        type: Number,
        default: null
      },
      deal: Object,
      newPayment: {
        type: Boolean,
        default: false
      }
    },
    methods: {
      loadAsyncData () {
        if (!this.newPayment) {
          Payment.get(
            this.paymentId
          ).then(response => {
            this.deal.dealPayment = response
          })
        }
      }
    },
    mounted () {
      this.loadAsyncData()
    }
  }
</script>
