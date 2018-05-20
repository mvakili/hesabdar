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
        <div class="tile is-parent is-8 is-desktop ">
          <article class="tile is-child box">
            <h4 class="title">کالا</h4>
            <deal-item-list v-model="deal.items" :dealId="props.row.id" @price-changed="console.log(123)"></deal-item-list>
          </article>
        </div>
        <div class="tile is-parent is-desktop ">
          <article class="tile is-child box">
            <h4 class="title">پرداخت</h4>
            <deal-payment :paymentId="props.row.dealPaymentId"></deal-payment>            
          </article>
        </div>
      </div>
      
    </template>
    <template slot="table-template" slot-scope="props">
      <b-table-column field="id" label="#" width="100" sortable numeric>
          {{ props.row.id }}
      </b-table-column>

      <b-table-column field="seller.name" label="فروشنده" sortable>
          {{ props.row.seller.name }}
      </b-table-column>
      <b-table-column field="dealTime" label="زمان خرید" sortable>
          {{ props.row.dealTime | moment("HH:mm jYYYY/jMM/jD") }}
      </b-table-column>
            <b-table-column field="dealPrice" label="قیمت خرید" sortable>
          {{ props.row.dealPrice.amount || 0 }}
      </b-table-column>
      <b-table-column field="dealPaymentId" label="dealPaymentId" sortable :visible="false">
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
  
  export default {
    components: {
      List,
      CardModal,
      New,
      Edit,
      Modal,
      DealItemList,
      DealPayment
    },
    data () {
      return {
        newModalVisible: false,
        deleteModalVisible: false,
        editModalVisible: false,
        isPublic: true,
        editId: null,
        deal: {
          items: []
        }
      }
    },
    methods: {
      loadAsyncData (table) {
        table.loading = true
        Deal.getPurchases(
          table.currentPage,
          table.perPage,
          table.sortField,
          table.sortOrder
        ).then(response => {
          table.data = response.queryable
          table.total = response.rowCount
          table.perPage = response.pageSize
          table.loading = false
          this.table = table
        }).catch(err => {
          console.log(err)
          table.loading = false
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
      dealItemsPriceChanged (value) {
        console.log(value)
      }
    }
  }
</script>
