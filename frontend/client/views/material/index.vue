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
            <b-dropdown-item :value="true" class="">
              <div class="media">
                <div class="media-content has-text-success">
                  <span>ویرایش</span>
                </div>
              </div>
            </b-dropdown-item>
            <b-dropdown-item :separator="true" />
            
            <b-dropdown-item :value="false">
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
          <new-material></new-material>
        </div>
      </modal>
    </template>
  </list>
</template>

<script>
  import { CardModal, Modal } from 'vue-bulma-modal'
  import List from './../../templates/List'
  import Material from './../../services/material'
  import NewMaterial from './New'

  export default {
    components: {
      List,
      CardModal,
      NewMaterial,
      Modal
    },
    data () {
      return {
        newModalVisible: false,
        isPublic: true
      }
    },
    methods: {
      loadAsyncData (table) {
        table.loading = true
        Material.getMaterials(
          table.currentPage,
          table.perPage,
          table.sortField,
          table.sortOrder
        ).then(response => {
          table.data = response.queryable
          table.total = response.rowCount
          table.perPage = response.pageSize
          table.loading = false
        }).catch(err => {
          console.log(err)
          table.loading = false
        })
      }
    }
  }
</script>
