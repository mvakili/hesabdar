<template>
<div>
  <div class="tile is-ancestor">
    <div class="tile is-parent is-12">
      <nav>
        <a class="button has-text-success" v-on:click="newModalVisible = true">  جدید 
          &nbsp; &nbsp;<span class="icon has-text-success">
            <i class="fa fa-plus"></i>
          </span>
        </a>
      </nav>
    </div>
  </div>
  <div class="tile is-ancestor">
    <div class="tile is-parent is-12">
      <article class="tile is-child box">
        <section>
            <b-table
                :data="data"
                :loading="loading"
                :paginated="isPaginated"
                :per-page="perPage"
                :total="total"
                :current-page.sync="currentPage"
                :backend-sorting="true"
                :backend-pagination="true"
                @sort="onSort"
                @page-change="onPageChange"
                :mobile-cards="false">

                <template slot-scope="props">
                    <b-table-column field="id" label="#" width="100" sortable numeric>
                        {{ props.row.id }}
                    </b-table-column>

                    <b-table-column field="name" label="نام" sortable>
                        {{ props.row.name }}
                    </b-table-column>
                    <b-table-column  label="عملیات">
                      <a class="button is-info">آمار</a>
                      <a class="button is-warning">ویرایش</a>
                      <a class="button is-danger">حذف</a>
                    </b-table-column>
                </template>

            </b-table>
        </section>
      </article>
    </div>
  </div>
  <card-modal v-bind:visible="newModalVisible" @close="newModalVisible = false">
      <div class="content has-text-centered">
      <form action="">
                <div class="modal-card" style="width: auto">
                    <header class="modal-card-head">
                        <p class="modal-card-title">Login</p>
                    </header>
                    <section class="modal-card-body">
                        <b-field label="Email">
                            <b-input
                                type="email"
                                placeholder="Your email"
                                required>
                            </b-input>
                        </b-field>

                        <b-field label="Password">
                            <b-input
                                type="password"
  
                                password-reveal
                                placeholder="Your password"
                                required>
                            </b-input>
                        </b-field>

                        <b-checkbox>Remember me</b-checkbox>
                    </section>
                    <footer class="modal-card-foot">
                        <button class="button" type="button" @click="$parent.close()">Close</button>
                        <button class="button is-primary">Login</button>
                    </footer>
                </div>
            </form>
        </div>
    </card-modal>
</div>
</template>

<script>
import { CardModal } from 'vue-bulma-modal'
import Material from './../../services/material'

export default {
  components: {
    CardModal
  },
  data () {
    return {
      data: [],
      total: 0,
      isPaginated: true,
      currentPage: 1,
      perPage: 10,
      loading: false,
      sortField: '',
      sortOrder: 'desc',
      newModalVisible: false
    }
  },
  methods: {
    onPageChange (page) {
      this.currentPage = page

      this.loadAsyncData()
    },
    onSort (field, order) {
      this.sortField = field
      this.sortOrder = order
      this.loadAsyncData()
    },
    loadAsyncData () {
      this.loading = true
      Material.getMaterials(
        this.currentPage,
        this.perPage,
        this.sortField,
        this.sortOrder
      ).then(response => {
        this.data = response.queryable
        this.total = response.rowCount
        this.perPage = response.pageSize

        this.loading = false
      }).catch(err => {
        console.log(err)
        this.loading = false
      })
    }
  },
  mounted () {
    this.loadAsyncData()
  }
}
</script>

<style>
.pagination-previous {
  transform: rotate(180deg);
}
.pagination-next {
  transform: rotate(180deg);
}
.table th,
.table td {
  text-align: right !important;
}

.table th:last-child,
.table td:last-child
{
    text-align: left !important;
}
.pagination .fa {
  vertical-align: text-top !important;
}
</style>
