<template>
  <div>
    <div class="tile is-ancestor">
      <div class="tile is-parent is-12">
        <nav>
          <a class="button has-text-success" v-on:click="newBuyModalVisible = true">  جدید 
             &nbsp; &nbsp;<span class="icon has-text-success">
              <i class="fa fa-plus"></i>
            </span>
          </a>
        </nav>
      </div>
    </div>
    <div class="tile is-ancestor">
      <div class="tile is-parent">
        <article class="tile is-child">
          <div class="block">
            <p class="control has-addons">
              <input class="input" type="text" placeholder="...">
              <a class="button is-primary">جستجو</a>
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
                <th>تامین کننده</th>
                <th>تاریخ</th>
                <th>قیمت</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="item in buyingData">
                <td>{{item.id}}</td>
                <td>{{item.provider}}</td>
                <td >
                  {{item.date | moment("HH:mm jYYYY/jM/jD")}}
                </td>
                <td>
                  {{item.price}}
                </td>
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
    <card-modal v-bind:visible="newBuyModalVisible" @close="newBuyModalVisible = false">
      <div class="content has-text-centered">
      </div>
    </card-modal>
  </div>
</template>
<script>
import { CardModal } from 'vue-bulma-modal'

export default {
  components: {
    CardModal
  },
  data () {
    return {
      newBuyModalVisible: false,
      buyingData: [
        {
          id: 1,
          provider: 'تامین کننده 1',
          date: '2017-05-12 16:55',
          price: '32000'
        },
        {
          id: 2,
          provider: 'تامین کننده 2',
          date: '2017-05-12 16:55',
          price: '32000'
        },
        {
          id: 3,
          provider: 'تامین کننده 3',
          date: '2017-05-12 16:55',
          price: '32000'
        },
        {
          id: 3,
          provider: 'تامین کننده 4',
          date: '2017-05-12 16:55',
          price: '32000'
        }
      ]
    }
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
