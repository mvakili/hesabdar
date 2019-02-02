<template>
  <list @load-data="loadAsyncData" :detailed="true" detailKey="id">
    <template slot="table-detail" slot-scope="detailProps">
      <div class="tile is-ancestor">
        <div class=" column is-three-quarters-mobile is-two-thirds-tablet is-two-thirds-desktop is-two-third-widescreen is-half-fullhd">
          <article class="tile is-child box">
            <h4 class="title">کالا</h4>
            <deal-item-list :deal="detailProps.row" :saleOrPurchase="(detailProps.row.sellerId == dealerId) ? 'purchase' : 'sale'" :dealId="detailProps.row.id" ref="deal-item-list"></deal-item-list>
          </article>
        </div>
        <div class=" column ">
          <article class="tile is-child box">
            <h4 class="title">پرداخت</h4>
            <deal-payment :deal="detailProps.row" :paymentId="detailProps.row.dealPaymentId || null" ref="deal-payment"></deal-payment>
          </article>
        </div>
      </div>
      
    </template>
    <template slot="table-template" slot-scope="detailProps">
      <b-table-column field="id" label="#" width="100" sortable numeric>
          {{ detailProps.row.id }}
      </b-table-column>

      <b-table-column field="seller.id" label="فروش / خرید" sortable>
        <span v-if="detailProps.row.sellerId == dealerId">
          خرید
        </span>
        <span v-else>
          فروش
        </span>
      </b-table-column>
      <b-table-column field="dealTime" label="زمان خرید / فروش" sortable>
        <date-picker :class="{'disable-event': !table.openedDetailed.includes(detailProps.row.id)}" type="datetime" :auto-submit="true"  format="YYYY-MM-DD HH:mm" display-format="HH:mm jYYYY/jMM/jDD" v-model="detailProps.row.dealTime"></date-picker>
      </b-table-column>
      <b-table-column field="dealPrice.amount" label="قیمت خرید / فروش" sortable>
        <input class="input" v-if="table.openedDetailed.includes(detailProps.row.id)" type="text" placeholder="قیمت خرید" v-model="detailProps.row.dealPrice.amount" />        
        <span v-else>
          {{ detailProps.row.dealPrice.amount || 0 | currency('', 0) }}
        </span>
      </b-table-column>
      <b-table-column field="dealPayment.amount" label="پرداخت">
        <span>
          {{ (detailProps.row.dealPayment.paid ? detailProps.row.dealPayment.amount : 0) || 0 | currency('', 0) }}
        </span>
      </b-table-column>
      <b-table-column field="dealPaymentId" label="DealPaymentId" :visible="false">
        {{detailProps.row.dealPaymentId}}          
      </b-table-column>
      <b-table-column  label="" width="100">
        <button v-if="table.openedDetailed.includes(detailProps.row.id)" class="button is-warning" @click="save(detailProps.row, detailProps.index)">
          ذخیره
        </button>
        <b-dropdown v-else :mobile-modal="false" v-model="isPublic" class="control" position="is-bottom-left">
          <button class="button is-link" type="button" slot="trigger">
            <template v-if="isPublic">
              <b-icon icon="earth"></b-icon>
              <span>
                <i class="fa fa-ellipsis-v" aria-hidden="true"></i>
              </span>
            </template>
            <template v-else>
              <b-icon icon="account-multiple"></b-icon>
              <span>
                <i class="fa fa-ellipsis-v" aria-hidden="true"></i>
              </span>
            </template>
            <b-icon icon="menu-down"></b-icon>
          </button>
          <div class="box"> 
            <!-- <b-dropdown-item :value="false" disabled>
              <div class="media">
                <div class="media-content has-text-success">
                  <span>حذف</span>
                </div>
              </div>
            </b-dropdown-item> -->
          </div>
        </b-dropdown>
      </b-table-column>
    </template>
  </list>
            
</template>

<script>
  import { CardModal, Modal } from 'vue-bulma-modal'
  import List from './../../templates/List'
  import Deal from './../../services/deal'
  import DealItemList from './../deal/dealItem/List'
  import DealPayment from './../deal/payment/index'
  import DealerSelect from './../dealer/Select'
  
  export default {
    props: {
      dealerId: {
        type: Number,
        required: true
      }
    },
    components: {
      List,
      CardModal,
      Modal,
      DealItemList,
      DealPayment,
      DealerSelect
    },
    data () {
      return {
        newModalVisible: false,
        deleteModalVisible: false,
        editModalVisible: false,
        isPublic: true,
        editId: null
      }
    },
    methods: {
      loadAsyncData (table) {
        table.loading = true
        Deal.getDealsOfDealer(
          this.dealerId,
          table.currentPage,
          table.perPage,
          table.sortField,
          table.sortOrder
        ).then(response => {
          table.data = response.queryable
          table.total = response.rowCount
          table.perPage = response.pageSize
          table.loading = false
          table.openedDetailed = []
          this.table = table
        })
      },
      openEditModal (id) {
        this.editId = id
        this.editModalVisible = true
      },
      edited (deal) {
        this.editModalVisible = false
        this.loadAsyncData(this.table)
      },
      added (deal) {
        this.newModalVisible = false
        this.loadAsyncData(this.table)
      },
      deleted () {
        this.deleteModalVisible = false
        this.loadAsyncData(this.table)
      },
      save (row, index) {
        Deal.edit(row.id, row).then(response => {
          this.$openNotification('عملیات موفق', 'تغییرات ذخیره شد', 'success')
          this.updateRow(row, index)
        }).catch(() => {
          this.$openNotification('عملیات ناموفق', 'دوباره سعی کنید', 'danger')
          this.updateRow(row, index)
        })
      },
      updateRow (row, index) {
        Deal.get(row.id).then(response => {
          this.table.data.splice(index, 1)
          this.table.data.splice(index, 0, response)
          this.$refs['deal-item-list'].loadAsyncData()
          this.$refs['deal-payment'].loadAsyncData()
        })
      }
    }
  }
</script>
