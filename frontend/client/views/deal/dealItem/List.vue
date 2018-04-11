<template>
  <list @load-data="loadAsyncData">
    <template slot="table-detail" slot-scope="props">
      
    </template>
    <template slot="table-template" slot-scope="props">
      <b-table-column field="id" label="#" width="100" sortable numeric>
          {{ props.row.id }}
      </b-table-column>

      <b-table-column field="name" label="جنس" sortable>
          {{ props.row.material.name }}
      </b-table-column>
      <b-table-column field="name" label="تعداد" sortable>
          {{ props.row.quantity }}
      </b-table-column>
      <b-table-column field="name" label="فی" sortable>
          {{ props.row.pricePerOne }}
      </b-table-column>
      <b-table-column field="name" label="قیمت" sortable>
          {{ props.row.pricePerOne * props.row.quantity }}
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
  import List from './../../../templates/List'
  import DealItem from './../../../services/dealItem'
  import Edit from './Edit'

  export default {
    components: {
      List,
      CardModal,
      Edit,
      Modal
    },
    props: {
      dealId: {
        type: Number,
        default: null
      }
    },
    data () {
      return {
        deleteModalVisible: false,
        editModalVisible: false,
        isPublic: true,
        editId: null
      }
    },
    methods: {
      loadAsyncData (table) {
        table.loading = true
        DealItem.gets(
          this.dealId
        ).then(response => {
          table.data = response
          table.isPaginated = false
          table.loading = false
          table.perPage = response.length
          this.table = table
          console.log(response)
        }).catch(err => {
          console.log(err)
          table.loading = false
        })
      },
      openEditModal (id) {
        this.editId = id
        this.editModalVisible = true
      },
      edited (material) {
        this.editModalVisible = false
        this.loadAsyncData(this.table)
      },
      added (material) {
        this.loadAsyncData(this.table)
      },
      deleted () {
        this.deleteModalVisible = false
        this.loadAsyncData(this.table)
      }
    }
  }
</script>
