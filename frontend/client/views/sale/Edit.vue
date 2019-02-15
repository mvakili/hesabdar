<template>
  <div>
    <div class="tile is-ancestor">
      <div class="tile is-parent">
        <article class="tile is-child box">
          <h1 class="title">ویرایش طرف‌حساب</h1>
          <h2 class="subtitle"></h2>
          <form v-on:submit.prevent="edit">
            <div class="columns">
              <div class="column">
                <label class="label">نام طرف‌حساب</label>
                <p class="control has-icon has-icon-right">
                  <input class="input" v-model="dealer.name" type="text" placeholder="نام طرف‌حساب" value="" autofocus>
                </p>
              </div>
            </div>
            <p class="control">
              <input type="submit" class="button is-primary" value="تایید" />
            </p>
          </form>
        </article>
      </div>
    </div>
  </div>
</template>

<script>
import Dealer from './../../services/dealer'

export default {
  props: {
    id: Number
  },
  data () {
    return {
      dealer: {
      }
    }
  },
  methods: {
    edit () {
      Dealer.edit(this.id, this.dealer).then(res => {
        this.$emit('onSuccess', this.dealer)
      }).catch(err => {
        this.$emit('onFail', err)
      })
    }
  },
  mounted () {
    Dealer.get(this.id).then(res => {
      this.dealer = res
    })
  }
}
</script>

<style lang="scss" scoped>
@import '~bulma/sass/utilities/mixins';
.left-notification {
  left: 0px;
}
.button {
  margin: 5px 0 0;
}
.control .button {
  margin: inherit;
}
.control.has-addons {
  @include mobile() {
    input {
      width: 100%;
    }
    input.is-expanded {
      flex-shrink: 1;
    }
  }
}
</style>
