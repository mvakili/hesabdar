<template>
  <list @load-data="loadAsyncData">
    <template slot="nav">
      <a class="button is-warning" v-on:click="newModalVisible = true"> جدید &nbsp; &nbsp;
        <span class="icon">
          <i class="fa fa-plus"></i>
        </span>
      </a>
    </template>

    <template slot="table-detail" slot-scope="props">
      
    </template>
    <template slot="table-template" slot-scope="props">
      <b-table-column field="id" label="#" width="100" sortable numeric>
          {{ props.row.id }}
      </b-table-column>

      <b-table-column field="name" label="نام" sortable>
          {{ props.row.name }}
      </b-table-column>
      <b-table-column field="barcode" label="بارکد">
        <barcode v-if="props.row.barcode" :value="props.row.barcode"
          :options="{ height: 20,
                      width: 1.3,
                      fontSize: 12,
                      textMargin: 0,
                      font: 'tahoma',
                      margin: 0
                    }" />
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
            <b-dropdown-item :value="true" class="" @click="oprnPriceHistoryModal(props.row.id)">
              <div class="media">
                <div class="media-content has-text-success">
                  <span>آمار</span>
                </div>
              </div>
            </b-dropdown-item>
            <b-dropdown-item :separator="true" />
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

      <modal :visible="priceHistoryModalVisible" @close="priceHistoryModalVisible = false">
        <div class="modal-card-body">
          <price-history :materialId="priceHistoryId"></price-history>
        </div>
      </modal>
    </template>
  </list>
</template>

<script>
  import { CardModal, Modal } from 'vue-bulma-modal'
  import Barcode from '@xkeshi/vue-barcode'
  import List from './../../templates/List'
  import Material from './../../services/material'
  import New from './New'
  import Edit from './Edit'
  import PriceHistory from './../dealItem/PriceHistory'

  export default {
    components: {
      List,
      CardModal,
      New,
      Edit,
      Modal,
      Barcode,
      PriceHistory
    },
    data () {
      return {
        newModalVisible: false,
        deleteModalVisible: false,
        editModalVisible: false,
        priceHistoryModalVisible: false,
        priceHistoryId: null,
        isPublic: true,
        editId: null
      }
    },
    methods: {
      loadAsyncData (table) {
        table.loading = true
        Material.gets(
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
      oprnPriceHistoryModal (id) {
        this.priceHistoryId = id
        this.priceHistoryModalVisible = true
      },
      edited (material) {
        this.editModalVisible = false
        this.loadAsyncData(this.table)
      },
      added (material) {
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
