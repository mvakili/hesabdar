<template>
  <list @load-data="loadAsyncData"  :detailed="true">
    <template slot="nav">
      <a class="button is-warning" v-on:click="newModalVisible = true"> جدید &nbsp; &nbsp;
        <span class="icon">
          <i class="fa fa-plus"></i>
        </span>
      </a>
    </template>

    <template slot="table-detail" slot-scope="props">
      <deal-list :dealerId="props.row.id"></deal-list>
      <!-- <b-tabs type="is-boxed">
        <b-tab-item>
            <template slot="header">
                <span> خرید و فروش </span>
                <b-icon icon="list" />
            </template>
            <template>
              
            </template>
        </b-tab-item>
        <b-tab-item>
            <template slot="header">
                <span> دریافت و پرداخت </span>
                <b-icon icon="list" />
            </template>
            <template>
              
            </template>
        </b-tab-item>
      </b-tabs> -->
      
    </template>
    <template slot="table-template" slot-scope="props">
      <b-table-column field="id" label="#" width="100" sortable numeric>
          {{ props.row.id }}
      </b-table-column>

      <b-table-column field="name" label="نام" sortable>
          {{ props.row.name }}
      </b-table-column>
      <b-table-column field="phoneNumber" label="تلفن" sortable>
          {{ props.row.phoneNumber || ' ... ' }}
      </b-table-column>
      <b-table-column field="address" label="آدرس" sortable>
          {{ props.row.address || ' ... ' }}
      </b-table-column>
      <b-table-column field="balance" label="بدهی/طلب" sortable>
          {{ props.row.balance || 0 | abs |currency('', 0) }}
          {{ props.row.balance || 0 | balance }}
      </b-table-column>
      <b-table-column  label="" width="100">
        <b-dropdown :mobile-modal="false" v-model="isPublic" class="control" position="is-bottom-left">
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
            <!-- <b-dropdown-item :separator="true" /> -->
            <!-- <b-dropdown-item :value="false" disabled>
              <div class="media">
                <div class="media-content has-text-success">
                  <span>دریافت</span>
                </div>
              </div>
            </b-dropdown-item>
            <b-dropdown-item :value="false" disabled>
              <div class="media">
                <div class="media-content has-text-success">
                  <span>پرداخت</span>
                </div>
              </div>
            </b-dropdown-item>
            <b-dropdown-item :separator="true" />             -->
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
  import Dealer from './../../services/dealer'
  import New from './New'
  import DealList from './DealList'
  
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
      DealPayment,
      DealList
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
        Dealer.gets(
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
        })
      },
      openEditModal (id) {
        this.editId = id
        this.editModalVisible = true
      },
      edited (dealer) {
        this.editModalVisible = false
        this.loadAsyncData(this.table)
      },
      added (dealer) {
        this.newModalVisible = false
        this.loadAsyncData(this.table)
      },
      deleted () {
        this.deleteModalVisible = false
        this.loadAsyncData(this.table)
      }
    }
  }
</script>
