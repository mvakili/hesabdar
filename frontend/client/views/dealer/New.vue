<template>
  <div>
    <div class="tile is-ancestor">


      <div class="tile is-parent">
        <article class="tile is-child box">
          <h1 class="title">ثبت طرف حساب جدید</h1>
          <h2 class="subtitle"></h2>
          <form v-on:submit.prevent="add">
            <div class="columns">
              <div class="column">
                <label class="label">نام طرف حساب</label>
                <p class="control has-icon has-icon-right">
                  <input class="input" v-model="dealer.name" type="text" ref="nameInput" placeholder="نام طرف حساب" value="" autofocus>
                </p>
              </div>
            </div>
            <div class="columns">
              <div class="column">
                <label class="label">تلفن</label>
                <p class="control has-icon has-icon-right">
                  <input class="input" v-model="dealer.phoneNumber" type="text" ref="phoneNumberInput" placeholder="تلفن" value="" >
                </p>
              </div>
            </div>
            <div class="columns">
              <div class="column">
                <label class="label">آدرس</label>
                <p class="control has-icon has-icon-right">
                  <input class="input" v-model="dealer.address" type="text" ref="addressInput" placeholder="آدرس" value="" >
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
    name: {
      type: String,
      default: ''
    }
  },
  data () {
    return {
      dealer: {
        name: '',
        address: '',
        phoneNumber: ''
      }
    }
  },
  methods: {
    add () {
      Dealer.add(this.dealer).then(res => {
        this.$emit('onSuccess', res)
      }).catch(err => {
        this.$emit('onFail', err)
      })
    }
  },
  mounted () {
    this.dealer.name = this.name
    this.$refs.nameInput.focus()
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
