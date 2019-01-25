<template>
  <div>
    <div class="tile is-ancestor">
      <div class="tile is-parent">
        <article class="tile is-child box">
          <h1 class="title">ثبت کالا جدید</h1>
          <h2 class="subtitle"></h2>
          <form v-on:submit.prevent="add">
            <div class="columns">
              <div class="column">
                <label class="label">نام کالا</label>
                <p class="control has-icon has-icon-right">
                  <input class="input" v-model="material.name" type="text" ref="nameInput" placeholder="نام کالا" value="material.name" autofocus>
                </p>
              </div>
            </div>
            <div class="columns">
              <div class="column">
                <label class="label">بارکد</label>
                <p class="control has-icon has-icon-right">
                   <barcode v-if="material.barcode" :value="material.barcode"
                      :options="{ height: 20,
                        width: 1.3,
                        displayValue: false,
                        margin: 0
                    }" />
                  <input class="input" v-model="material.barcode" type="text"  placeholder="بارکد" value="" autofocus>
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
import Material from './../../services/material'
import Barcode from '@xkeshi/vue-barcode'

export default {
  components: {
    Barcode
  },
  props: {
    name: {
      type: String,
      default: ''
    }
  },
  data () {
    return {
      material: {
        name: '',
        barcode: ''
      }
    }
  },
  methods: {
    add () {
      Material.add(this.material).then(res => {
        this.$emit('onSuccess', res)
      }).catch(err => {
        this.$emit('onFail', err)
      })
    }
  },
  mounted () {
    this.material.name = this.name
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
