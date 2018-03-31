<template>
  <list @load-data="loadAsyncData">
    <template slot="nav">
      <a class="button has-text-success" v-on:click="newModalVisible = true"> جدید &nbsp; &nbsp;
        <span class="icon has-text-success">
          <i class="fa fa-plus"></i>
        </span>
      </a>
    </template>

    <template slot="table-template" slot-scope="props">
      <b-table-column field="id" label="#" width="100" sortable numeric>
          {{ props.row.id }}
      </b-table-column>

      <b-table-column field="name" label="نام" sortable>
          {{ props.row.name }}
      </b-table-column>
      <b-table-column  label="عملیات">
        <a class="button is-info">
        <span class="icon has-text-success">
            <i class="fa fa-bar-chart"></i>
          </span>
        </a>
        <a class="button is-warning">
          <span class="icon has-text-success">
            <i class="fa fa-pencil"></i>
          </span>
        </a>
        </a>
        <a class="button is-danger">
          <span class="icon has-text-success">
            <i class="fa fa-close"></i>
          </span>
        </a>
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
        newModalVisible: false
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

          console.log(response)
          table.loading = false
        }).catch(err => {
          console.log(err)
          table.loading = false
        })
      }
    }
  }
</script>
