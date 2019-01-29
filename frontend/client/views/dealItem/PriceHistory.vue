<template>
  <b-tabs type="is-boxed">
    <b-tab-item>
      <template slot="header">
        <span>لیست</span>
        <b-icon icon="list"/>
      </template>
      <template>
        <list @load-data="loadAsyncData">
          <template slot="table-template" slot-scope="props">
            <b-table-column field="deal.buyerId" label="فروش / خرید" sortable>
              <span v-if="props.row.deal.buyerId == 1">خرید</span>
              <span v-else>فروش</span>
            </b-table-column>
            <b-table-column field="quantity" label="تعداد" sortable>{{props.row.quantity}}</b-table-column>
            <b-table-column
              field="pricePerOne"
              label="فی"
              sortable
            >{{props.row.pricePerOne | currency('', 0)}}</b-table-column>
            <b-table-column field="deal.dealer" label="فروشنده / خریدار">
              <span v-if="props.row.deal.buyerId == 1">{{props.row.deal.seller.name}}</span>
              <span v-else>{{props.row.deal.buyer.name}}</span>
            </b-table-column>
            <b-table-column
              field="deal.dealTime"
              label="زمان"
              sortable
            >{{props.row.deal.dealTime | moment('HH:mm jYYYY/jMM/jDD')}}</b-table-column>
          </template>
        </list>
      </template>
    </b-tab-item>
    <b-tab-item>
      <template slot="header">
        <span>نمودار</span>
        <b-icon icon="list"/>
      </template>
      <template>
        <material-amount-chart :materialId="materialId"></material-amount-chart>
      </template>
    </b-tab-item>
  </b-tabs>
</template>

<script>
  import List from './../../templates/List'
  import DealItem from './../../services/dealItem'
  import MaterialAmountChart from './MaterialAmountChart'
  
  export default {
    components: {
      List,
      MaterialAmountChart
    },
    props: {
      materialId: {
        type: Number,
        required: true
      }
    },
    data () {
      return {
        isPublic: true,
        editId: null
      }
    },
    methods: {
      loadAsyncData (table) {
        table.loading = true
        DealItem.getDealItemsOfMaterial(
          this.materialId,
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
      }
    }
  }
</script>
