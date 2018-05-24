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
                :selected.sync="table.selected"
                :opened-detailed.sync="table.openedDetailed"
                @details-open="detailsOpen"
                @details-close="detailsClose"                
                @sort="onSort"
                :detailed="detailed"
                :detail-key="detailKey"
                @page-change="onPageChange"
                :row-class="rowClass"
                :mobile-cards="false">

                <template slot="detail" slot-scope="props">
                   <slot name="table-detail" :row="props.row" :index="props.index" >
                  </slot>
                </template>
                <template slot-scope="props">
                  <slot name="table-template" :row="props.row" :index="props.index" >
                  </slot>
                </template>
              <template slot="footer">
                  <slot name="table-footer" />
              </template>
              <template slot="empty">
                <section class="section">
                    <slot name="table-empty">
                      <div class="content has-text-grey has-text-centered">
                        <p>
                            <b-icon
                                icon="emoticon-sad"
                                size="is-large">
                            </b-icon>
                        </p>
                        <p>اطلاعاتی برای نمایش وجود ندارد</p>
                      </div>
                    </slot>
                </section>
              </template>
            </b-table>
            <div class="container is-fluid">
                <slot name="table-bottom" />
            </div>
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
    props: {
      detailed: {
        type: Boolean,
        default: false
      },
      detailKey: {
        type: String,
        default: ''
      },
      rowClass: {
        type: Function,
        default: () => ''
      },
      table: {
        type: Object,
        default: () => {
          return {
            data: [],
            total: 0,
            isPaginated: true,
            currentPage: 1,
            perPage: 10,
            loading: false,
            sortField: '',
            sortOrder: 'desc',
            selected: null
          }
        }
      }
    },
    methods: {
      onPageChange (page) {
        this.table.currentPage = page
        this.$emit('load-data', this.table)
      },
      detailsOpen (value) {
        this.$emit('detailsOpen', value)
      },
      detailsClose (value) {
        this.$emit('detailsClose', value)
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

.table>tr>th:last-child,
.table>tr>td:last-child,
.table>tfoot>tr>th:last-child
{
    text-align: left !important;
}

.table td .is-expanded>.fa-angle-right {
  transform: rotate(90deg);  
}
.table td .fa-angle-right {
  transform: rotate(180deg);  
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
.dropdown {
  left: 50px;
  margin-top: -6px;
}
.dropdown .dropdown-menu {
  z-index: 1;
  position: absolute;
  background-color: white;
}
.media-content {
  text-align: right !important;
}
</style>
