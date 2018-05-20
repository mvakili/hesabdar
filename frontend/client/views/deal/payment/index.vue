<template>
  <div>
    <section>
      <div class="control is-horizontal">
        <p class="control">
          <label class="radio">
            <input type="radio"  v-model="deal.dealPayment.method" value="Cash" name="paymentType"> &nbsp;
            نقدی
          </label>
          <label class="radio">
            <input type="radio"  v-model="deal.dealPayment.method" value="Cheque" name="paymentType"> &nbsp;
            چک
          </label>
        </p>
        <p class="control">
          <label class="radio">
            <vb-switch v-model="deal.dealPayment.payed" type="primary" ></vb-switch> &nbsp;
            <span v-if="deal.dealPayment.payed">
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
            <date-picker type="date" :auto-submit="true" :editable="true" format="YYYY-MM-DD HH:mm" display-format="jYYYY/jMM/jDD" v-model="deal.dealPayment.dueDate"></date-picker>
          </div>
         </div>
      </div>
      <p>&nbsp;</p>
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
            <date-picker type="datetime" :auto-submit="true" :editable="true" format="YYYY-MM-DD HH:mm" display-format="HH:mm jYYYY/jMM/jDD" v-model="deal.dealPayment.payDate"></date-picker>
          </div>
         </div>
      </div>
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
      deal: Object
    },
    methods: {
      loadAsyncData () {
        Payment.get(
          this.paymentId
        ).then(response => {
          this.deal.dealPayment = response
        })
      }
    },
    mounted () {
      this.loadAsyncData()
    }
  }
</script>
