<template>
  <list @load-data="loadAsyncData" :detailed="true" detailKey="id">
    <template slot="nav" >
      <a class="button is-warning" v-on:click="newModalVisible = true"> جدید &nbsp; &nbsp;
        <span class="icon">
          <i class="fa fa-plus"></i>
        </span>
      </a>
    </template>

    <template slot="table-detail" slot-scope="props">
      <div class="tile is-ancestor">
        <div class=" column is-three-quarters-mobile is-two-thirds-tablet is-half-desktop is-two-third-widescreen is-half-fullhd">
          <article class="tile is-child box">
            <h4 class="title">کالا</h4>
            <deal-item-list :deal="props.row" :dealId="props.row.id" ref="deal-item-list"></deal-item-list>
          </article>
        </div>
        <div class=" column ">
          <article class="tile is-child box">
            <h4 class="title">پرداخت</h4>
            <deal-payment :deal="props.row" :paymentId="props.row.dealPaymentId || null" ref="deal-payment"></deal-payment>
          </article>
          
              <div class="tile">
                <div class="column has-text-left">
                  <button class="button is-warning" @click="save(props.row, props.index)">
                    ذخیره
                  </button>
                </div>
              </div>


        </div>
      </div>
      
    </template>
    <template slot="table-template" slot-scope="props">
      <b-table-column field="id" label="#" width="100" sortable numeric>
          {{ props.row.id }}
      </b-table-column>

      <b-table-column field="buyer" label="خریدار" sortable>
        <dealer-select v-model="props.row.buyer" :id.sync="props.row.buyerId" :disabled="!table.openedDetailed.includes(props.row.id)"></dealer-select>
      </b-table-column>
      <b-table-column field="dealTime" label="زمان فروش" sortable>
        <date-picker type="date" :auto-submit="true" :editable="false" format="YYYY-MM-DD HH:mm" display-format="jYYYY/jMM/jDD" v-model="props.row.dealTime"></date-picker>
      </b-table-column>
      <b-table-column field="dealPrice" label="قیمت فروش" sortable>
          {{ props.row.dealPrice.amount || 0 | currency('', 0) }}
      </b-table-column>
      <b-table-column field="dealPaymentId" label="DealPaymentId" :visible="false">
        {{props.row.dealPaymentId}}          
      </b-table-column>
      <b-table-column  label="" width="100">
        <b-dropdown :mobile-modal="false" v-model="isPublic" position="is-bottom-left">
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
            <b-dropdown-item :value="true" class="" @click="openEditModal(props.row.id)">
              <div class="media">
                <div class="media-content has-text-success">
                  <span>ویرایش</span>
                </div>
              </div>
            </b-dropdown-item>
            <b-dropdown-item :separator="true" />
            
            <b-dropdown-item :value="false" disabled>
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

    <template slot="modals">
      <modal :visible="newModalVisible" @close="newModalVisible = false">
        <div class="content has-text-centered">
          <new @onSuccess="added"></new>
        </div>
      </modal>
     <modal :visible="editModalVisible" @close="editModalVisible = false">
        <div class="content has-text-centered">
          <edit :id="editId" @onSuccess="edited" ></edit>
        </div>
      </modal>
      <modal :visible="deleteModalVisible" @close="deleteModalVisible = false">
        <div class="content has-text-centered">
          <new></new>
        </div>
      </modal>
    </template>
  </list>
</template>

<script>
  import { CardModal, Modal } from 'vue-bulma-modal'
  import List from './../../templates/List'
  import Deal from './../../services/deal'
  import New from './New'
  import Edit from './Edit'
  import DealItemList from './../deal/dealItem/List'
  import DealPayment from './../deal/payment/index'
  import DealerSelect from './../dealer/Select'
  
  export default {
    components: {
      List,
      CardModal,
      New,
      Edit,
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
        Deal.getSales(
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
