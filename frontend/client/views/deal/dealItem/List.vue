<template>
  <list @load-data="loadAsyncData">
    <template slot="table-detail" slot-scope="props">
    </template>
    <template slot="table-template" slot-scope="props">
      <b-table-column field="id" label="#" width="100" sortable numeric :visible="false">
          {{ props.row.id }}
      </b-table-column>

      <b-table-column field="material.name" label="جنس" sortable>
          {{ props.row.material.name }}
      </b-table-column>
      <b-table-column field="quantity" label="تعداد" sortable>
          <input class="input" v-model="props.row.quantity" />
      </b-table-column>
      <b-table-column field="pricePerOne" label="فی" sortable>
         <input class="input" v-model="props.row.pricePerOne" />
      </b-table-column>
      <b-table-column field="pricePerOne*quantity" label="قیمت" sortable>
          {{ props.row.pricePerOne * props.row.quantity }}
      </b-table-column>
    </template>
    <template slot="table-footer">
      <div class="control is-horizontal">
        <p class="control" >
          جمع کل: {{ totalPrice }}
        </p>
        <p class="control">
          <cleave class="input"  placeholder="قیمت فاکتور" :value="deal.price" :options="{ numeral: true, numeralThousandsGroupStyle: 'thousand' }"></cleave>
        </p>
      </div>
    </template>
  </list>
</template>

<script>
  import Cleave from 'vue-cleave'
  import { CardModal, Modal } from 'vue-bulma-modal'
  import List from './../../../templates/List'
  import DealItem from './../../../services/dealItem'

  export default {
    components: {
      List,
      CardModal,
      Modal,
      Cleave
    },
    props: {
      dealId: {
        type: Number,
        default: null
      }
    },
    data () {
      return {
        deal: {
          price: '0'
        },
        table: null
      }
    },
    methods: {
      loadAsyncData (table) {
        table.loading = true
        DealItem.getDealItemsOfDeal(
          this.dealId,
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
      }
    },
    computed: {
      totalPrice: {
        get: function () {
          var sum = 0
          if (!(this.table)) return 0
          this.table.data.forEach(element => {
            sum += element.pricePerOne * element.quantity
          })
          return sum
        },
        set: function (val) {
        }
      }
    },
    watch: {
      totalPrice: function (val) {
        this.deal.price = val + ''
      }
    }
  }
</script>
