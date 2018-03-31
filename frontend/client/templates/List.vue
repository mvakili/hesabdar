<template>
<div>
  <div class="tile is-ancestor">
    <div class="tile is-parent is-12">
      <nav>
        <slot name="nav">
        </slot>
      </nav>
    </div>
  </div>
  <div class="tile is-ancestor">
    <div class="tile is-parent is-12">
      <article class="tile is-child box">
        <section>
            <b-table
                :data="table.data"
                :loading="table.loading"
                :paginated="table.isPaginated"
                :per-page="table.perPage"
                :total="table.total"
                :current-page.sync="table.currentPage"
                :backend-sorting="true"
                :backend-pagination="true"
                @sort="onSort"
                @page-change="onPageChange"
                :mobile-cards="false">

                <template slot-scope="props">
                  <slot name="table-template" :row="props.row" :index="props.index" >
                  </slot>
                </template>
            </b-table>
        </section>
      </article>
    </div>
  </div>
  <slot name="modals">
  </slot>
</div>
</template>

<script>

  export default {
    data () {
      return {
        table: {
          data: [],
          total: 0,
          isPaginated: true,
          currentPage: 1,
          perPage: 10,
          loading: false,
          sortField: '',
          sortOrder: 'desc'
        }
      }
    },
    methods: {
      onPageChange (page) {
        this.table.currentPage = page
        this.$emit('load-data', this.table)
      },
      onSort (field, order) {
        this.table.sortField = field
        this.table.sortOrder = order
        this.$emit('load-data', this.table)
      }
    },
    mounted () {
      this.$emit('load-data', this.table)
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

.is-desc {
  transform: rotate(180deg);
}

.pagination-list li:not(:first-child) {
  margin-right: 0.375rem;
  margin-left: 0rem;
  
}

.pagination-previous, .pagination-next {
  margin-right: 0.75rem;
  margin-left: 0rem;
}
.modal-content > .content {
  margin: 50px;
}
</style>
