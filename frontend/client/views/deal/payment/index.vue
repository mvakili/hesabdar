<template>
  <div>
    <section>
      <div class="control is-horizontal">
        <p class="control">
          <label class="radio">
            <input type="radio"  v-model="payment.method" value="Cash" name="paymentType"> &nbsp;
            نقدی
          </label>
          <label class="radio">
            <input type="radio"  v-model="payment.method" value="Cheque" name="paymentType"> &nbsp;
            چک
          </label>
        </p>
        <p class="control">
          <label class="radio">
            <vb-switch v-model="payment.payed" type="primary" ></vb-switch> &nbsp;
            <span v-if="payment.payed">
              پرداخت شده
            </span>
            <span v-else>
              پرداخت نشده
            </span>
          </label>
        </p>
      </div>
      <p>&nbsp;</p>
      <div class="contol" v-if="payment.method == 'Cheque'">
         <div class="columns">
          <div  class="column">
            سررسید چک :
          </div>
          <div class="control column is-two-thirds">
            <date-picker :editable="true" :auto-submit="true" input-format="YYYY-MM-DD" format="jYYYY/jMM/jDD" v-model="payment.dueDate"></date-picker>
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
            <cleave class="input"   placeholder="مقدار پرداخت" :value="payment.amount" :options="{ numeral: true, numeralThousandsGroupStyle: 'thousand' }"></cleave>
          </div>
        </div>
      </div>
      <div class="contol">
         <div class="columns">
          <div  class="column">
            تاریخ پرداخت: 
          </div>
          <div class="control column is-two-thirds">
            <date-picker type="datetime" :auto-submit="true" :editable="true" input-format="YYYY-MM-DD HH:mm" format="HH:mm jYYYY/jMM/jDD" v-model="payment.payDate"></date-picker>
          </div>
         </div>
      </div>
    </section>
  </div>
</template>

<script>
  import Cleave from 'vue-cleave'
  import VbSwitch from 'vue-bulma-switch'
  import Payment from './../../../services/payment'

  export default {
    components: {
      VbSwitch,
      Cleave
    },
    props: {
      paymentId: {
        type: Number,
        default: null
      }
    },
    data () {
      return {
        payment: {
        }
      }
    },
    methods: {
      loadAsyncData () {
        console.log(this.paymentId)
        Payment.get(
          this.paymentId
        ).then(response => {
          console.log(response)
          this.payment = response
        }).catch(err => {
          if (err.response.status !== 404) {
            console.log(err)
          }
        })
      }
    },
    mounted () {
      this.loadAsyncData()
    }
  }
</script>
