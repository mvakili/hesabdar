<template>
  <list :table="table">
    <template slot="table-detail" slot-scope="props">
    </template>
    <template slot="table-template" slot-scope="props">
      <b-table-column field="id" label="#"  numeric :visible="false">
          {{ props.row.id }}
      </b-table-column>

      <b-table-column field="material" label="کالا" width="200" >
          <material-select v-model="props.row.material" :id.sync="props.row.materialId"></material-select>
      </b-table-column>
      <b-table-column field="quantity" label="تعداد" >
          <input class="input" v-model="props.row.quantity" style="height:30px"/>
      </b-table-column>
      <b-table-column field="pricePerOne" label="فی" >
         <input class="input" v-model="props.row.pricePerOne" style="height:30px" />
      </b-table-column>
      <b-table-column field="pricePerOne*quantity" label="قیمت" style="height:30px">
          {{ props.row.pricePerOne * props.row.quantity | currency('', 0) }}
      </b-table-column>
      <b-table-column  label="" width="20">
        <b-dropdown class="control" position="is-bottom-left">
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
            <b-dropdown-item @click="remove(props.index)">
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
          <material-select ref="newMaterial" v-model="newRow.material"  @changed="newRowMaterialChanged"></material-select>
      </th>
      <th>
          <input class="input" ref="newQuantity" @keypress.enter="newRowQuantityUpdated" v-model="newRow.quantity" style="height:30px" />
      </th>
      <th>
        <p class="control has-addons is-expanded" style="direction:ltr">
          <tooltip :label="'بصورت پیشفرض قیمت آخرین '+ (saleOrPurchase == 'sale' ? 'فروش' : 'خرید') +' نمایش داده می شود'" type="primary">
            <button class="button is-primary" tabindex="-1">
              <span>
                ؟
              </span>
            </button>
          </tooltip>
            <input style="direction:rtl; height: 30px" class="input"  ref="newPricePerOne" @keypress.enter="newRowPricePerOneUpdated" v-model="newRow.pricePerOne"  />  
        </p>
      </th>
      <th>
        {{newRow.pricePerOne * newRow.quantity || 0 | currency('', 0) }}
      </th>
      <th width="50">
          <button  class="button is-primary is-fullwidth" v-on:click="add(newRow)" ><i class="fa fa-plus"></i></button>
      </th>
    </template>
    <template slot="table-bottom">
      <div class="control is-horizontal">
        <p class="control" style="direction: ltr" >
          جمع کل: {{ totalPrice | currency('', 0) }}
        </p>
      </div>
    </template>
    <template slot="table-empty">
      <div></div>
    </template>
  </list>
</template>

<script>
  import Tooltip from 'vue-bulma-tooltip'
  import { CardModal, Modal } from 'vue-bulma-modal'
  import List from './../../../templates/List'
  import DealItem from './../../../services/dealItem'
  import MaterialSelect from './../../material/Select'

  export default {
    components: {
      List,
      CardModal,
      Modal,
      MaterialSelect,
      Tooltip
    },
    props: {
      dealId: {
        type: Number,
        default: null
      },
      deal: Object,
      saleOrPurchase: {
        type: String,
        default: 'sale'
      }
    },
    data () {
      return {
        table: {
          data: []
        },
        newRow: {}
      }
    },
    methods: {
      loadAsyncData () {
        if (this.deal.id) {
          DealItem.getDealItemsOfDeal(
            this.dealId
          ).then(response => {
            this.table.data = response
            this.deal.items = this.table.data
          })
        } else {
          this.table.data = this.deal.items
        }
      },
      add: function (row) {
        try {
          row.quantity = Number(row.quantity)
          row.pricePerOne = Number(row.pricePerOne)

          if (!row.quantity) {
            this.$openNotification('خطا', 'تعداد وارد نشده', 'danger')
          } else if (!row.pricePerOne) {
            this.$openNotification('خطا', 'قیمت  وارد نشده', 'danger')
          } else if (!row.material || !row.material.id) {
            this.$openNotification('خطا', 'کالا انتخاب نشده است', 'danger')
          } else {
            this.table.data.push(row)
            this.initNewRow()
            this.$refs.newMaterial.focus()
          }
        } catch (error) {
          this.$openNotification('خطا', 'تعداد یا قیمت وارد شده اشتباه است', 'danger')
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
      },
      newRowMaterialChanged: function (value) {
        this.newRow.material = value
        this.$refs.newQuantity.select()
        this.calculateNewPrice(this.newRow.material)
      },
      calculateNewPrice (material) {
        if (material.id) {
          switch (this.saleOrPurchase) {
            case 'sale':
              DealItem.getLastSalePrice(material.id).then(response => {
                this.newRow.pricePerOne = response
              })
              break
            case 'purchase':
              DealItem.getLastPurchasePrice(material.id).then(response => {
                this.newRow.pricePerOne = response
              })
              break
            default:
              break
          }
        }
      },
      newRowQuantityUpdated: function () {
        this.$refs.newPricePerOne.select()
      },
      newRowPricePerOneUpdated: function () {
        this.add(this.newRow)
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
          this.$emit('totalPriceChanged', sum)
          return sum
        },
        set: function (val) {
        }
      },
      dealPrice: {
        get: function () {
          return String(this.deal.dealPrice.amount)
        },
        set: function (val) {
          this.deal.dealPrice.amount = Number(val)
        }
      }
    },
    watch: {
      // totalPrice: function (val, oldValue) {
      //   let dealPriceValue = Number(this.deal.dealPrice.amount)
      //   if (dealPriceValue === oldValue || dealPriceValue === 0) {
      //     this.deal.dealPrice.amount = val
      //   }
      // }
    },
    mounted: function () {
      this.loadAsyncData()
    },
    beforeMount: function () {
      this.initNewRow()
    }
  }
</script>
