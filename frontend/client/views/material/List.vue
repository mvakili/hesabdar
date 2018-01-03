<template>
  <div>
    <div class="tile is-ancestor">
      <div class="tile is-parent is-12">
        <nav>
          <router-link class="button has-text-success" to="material/New">جدید
             &nbsp; &nbsp;<span class="icon has-text-success">
              <i class="fa fa-plus"></i>
            </span>
          </router-link>
        </nav>
      </div>
    </div>
    <div class="tile is-ancestor">
      <div class="tile is-parent">
        <article class="tile is-child">
          <div class="block">
            <p class="control has-addons">
              <input class="input" type="text" placeholder="...">
              <a class="button is-primary" v-on:click="loadDataList()">جستجو</a>
            </p>
          </div>
        </article>
      </div>
    </div>
    <div class="tile is-ancestor">
      <div class="tile is-parent is-12">
        <article class="tile is-child box">
          <table class="table">
            <thead>
              <tr>
                <th>#</th>
                <th>نام کالا</th>              
              </tr>
            </thead>
            <tbody>
              <tr v-for="item in datalist">
                <td>{{item.id}}</td>
                <td>{{item.name}}</td>
              </tr>
            </tbody>
          </table>
        </article>
      </div>
    </div>
    <div class="tile is-ancestor">
      <div class="tile is-parent is-12">
        <nav class="pagination" role="navigation" aria-label="pagination">
          <a class="pagination-previous" title="This is the first page" disabled>صفحه قبل</a>
          <a class="pagination-next">صفحه بعد</a>
          <ul class="pagination-list">
            <li>
              <a class="pagination-link is-current" aria-label="Page 1" aria-current="page">1</a>
            </li>
            <li>
              <a class="pagination-link" aria-label="Goto page 2">2</a>
            </li>
            <li>
              <a class="pagination-link" aria-label="Goto page 3">3</a>
            </li>
          </ul>
        </nav>
      </div>
    </div>
    <card-modal v-bind:visible="newMaterialModalVisible" @close="newMaterialModalVisible = false">
      <div class="content has-text-centered">
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
      newMaterialModalVisible: false,
      datalist: [
      ]
    }
  },
  methods: {
    loadDataList () {
      Material.getMaterials().then(res => {
        this.datalist = res.data.data
      })
    }
  },
  created () {
    this.loadDataList()
  }
}
</script>

<style lang="scss" scoped>
.table-responsive {
  display: block;
  width: 100%;
  min-height: .01%;
  overflow-x: auto;
  text-align: right;
}

td, th {
  text-align: right;
}
.control.has-addons .button:last-child {
  border-radius: 3px 0px 0px 3px;
}
</style>
