<template>
  <list :table="table">
    <template slot="table-detail" slot-scope="props">
    </template>
    <template slot="table-template" slot-scope="props">
      <b-table-column field="id" label="#" width="100"  numeric :visible="false">
          {{ props.row.id }}
      </b-table-column>

      <b-table-column field="material.name" label="کالا" >
          <material-select v-bind:value="props.row.material"></material-select>
      </b-table-column>
      <b-table-column field="quantity" label="تعداد" >
          <input class="input" v-model="props.row.quantity" />
      </b-table-column>
      <b-table-column field="pricePerOne" label="فی" >
         <input class="input" v-model="props.row.pricePerOne" />
      </b-table-column>
      <b-table-column field="pricePerOne*quantity" label="قیمت" >
          {{ props.row.pricePerOne * props.row.quantity }}
      </b-table-column>
      <b-table-column  label="" width="100">
        <b-dropdown position="is-bottom-left">
          <button class="button is-link" type="button" slot="trigger">
            <template>
              <b-icon icon="account-multiple"></b-icon>
              <span>
                <i class="fa fa-ellipsis-v" aria-hidden="true"></i>
              </span>
            </template>
            <b-icon icon="menu-down"></b-icon>
          </button>
          <div class="box"> 
            <b-dropdown-item v-on:click="remove(props.index)">
              <div class="media">
                <div class="media-content has-text-success">
                  <span>حذف</span>
                </div>
              </div>
            </b-dropdown-item>
          </div>
        </b-dropdown>
      </b-table-column>
    </template>
    <template slot="table-footer">
      <th>
          <material-select v-model="newRow.material"></material-select>
          {{newRow.material.name}}
      </th>
      <th>
          <input class="input" v-model="newRow.quantity" />
      </th>
      <th>
          <input class="input" v-model="newRow.pricePerOne" />
      </th>
      <th>
        {{newRow.pricePerOne * newRow.quantity || 0}}
      </th>
      <th width="100">
          <button  class="button is-primary is-fullwidth" v-on:click="add(newRow)" ><i class="fa fa-plus"></i></button>
      </th>
    </template>
    <template slot="table-bottom">
      <div class="control is-horizontal">
        <p class="control" >
          جمع کل: {{ totalPrice }}
        </p>
        <p class="control">
          <cleave class="input"  placeholder="قیمت فاکتور" :value="dealPrice.amount" v-model="dealPrice.amount" :options="{ numeral: true }"></cleave>
        </p>
      </div>
    </template>
    <template slot="table-empty">
      <div></div>
    </template>
  </list>
</template>

<script>
  import Cleave from 'vue-cleave'
  import { CardModal, Modal } from 'vue-bulma-modal'
  import List from './../../../templates/List'
  import DealItem from './../../../services/dealItem'
  import MaterialSelect from './../../material/Select'
  import Payment from './../../../services/payment'

  export default {
    components: {
      List,
      CardModal,
      Modal,
      Cleave,
      MaterialSelect
    },
    props: {
      dealId: {
        type: Number,
        default: null
      },
      value: []
    },
    data () {
      return {
        deal: {
        },
        dealPrice: {
          amount: '0'
        },
        table: {
          data: {
            get: function () {
              return this.value
            },
            set: function (val) {
              this.value = val
            }
          }
        },
        newRow: {}
      }
    },
    methods: {
      loadAsyncData () {
        DealItem.getDealItemsOfDeal(
          this.dealId
        ).then(response => {
          this.table.data = response
        })
        Payment.getPriceOfDeal(
          this.dealId
        ).then(response => {
          this.dealPrice = response
        })
      },
      add: function (row) {
        if (row.quantity && row.pricePerOne) {
          this.table.data.push(row)
          this.initNewRow()
        } else {
          this.$openNotification('خطا', 'قیمت یا تعداد وارد نشده', 'danger')
        }
      },
      remove: function (index) {
        delete this.table.data.splice(index, 1)
      },
      initNewRow: function () {
        this.newRow = {
          quantity: 1,
          pricePerOne: 0,
          material: {}
        }
      }
    },
    computed: {
      totalPrice: {
        get: function () {
          var sum = 0
          if (!(Array.isArray(this.table.data))) return 0
          this.table.data.forEach(element => {
            sum += element.pricePerOne * element.quantity
          })
          return sum
        },
        set: function (val) {
        }
      }
    },
    watch: {
      totalPrice: function (val, oldValue) {
        this.dealPrice.amount = val + ''
      },
      'dealPrice.amount': function (value, oldValue) {
        this.$emit('price-changed', this.dealPrice)
      }
    },
    mounted: function () {
      this.loadAsyncData()
    },
    beforeMount: function () {
      this.initNewRow()
    }
  }
</script>
