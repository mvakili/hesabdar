<template>
  <div>
    <div class="tile is-ancestor">
      <div class="tile is-parent">
        <article class="tile is-child box">
          <h1 class="title">ویرایش کالا</h1>
          <h2 class="subtitle"></h2>
          <form v-on:submit.prevent="edit">
            <div class="columns">
              <div class="column">
                <label class="label">نام کالا</label>
                <p class="control has-icon has-icon-right">
                  <input class="input" v-model="material.name" type="text" placeholder="نام کالا" value="" autofocus>
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
                  <input class="input" v-model="material.barcode" type="text" placeholder="بارکد" value="" autofocus>
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
    id: Number
  },
  data () {
    return {
      material: {
      }
    }
  },
  methods: {
    edit () {
      Material.edit(this.id, this.material).then(res => {
        this.$emit('onSuccess', this.material)
      }).catch(err => {
        this.$emit('onFail', err)
      })
    }
  },
  mounted () {
    Material.get(this.id).then(res => {
      this.material = res
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
