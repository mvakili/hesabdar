<template>
  <div>
    <div class="tile is-ancestor">


      <div class="tile is-parent">
        <article class="tile is-child box">
          <h1 class="title">ثبت کالای جدید</h1>
          <h2 class="subtitle"></h2>
          <form v-on:submit.prevent="addMaterial">
            <div class="columns">
              <div class="column">
                <label class="label">نام کالا</label>
                <p class="control has-icon has-icon-right">
                  <input class="input" v-model="materialName" type="text" placeholder="نام کالا" value="" autofocus>
                  <!-- <validator></validator> -->
                </p>
              </div>
            </div>
            <p class="control">
              <input type="submit" class="button is-primary" value="تایید" />
              <router-link class="button"
                v-if="$routerHistory.hasHistory()"
                :to="{ path: $routerHistory.previous().path }">
                  بی خیال
              </router-link>
            </p>
          </form>
        </article>
      </div>
    </div>
  </div>
</template>

<script>
import Material from './../../services/material'

export default {
  data () {
    return {
      demo: {
        value: '',
        rawValue: ''
      },
      materialName: ''
    }
  },
  methods: {
    onRawValueChanged (newVal) {
      this.demo.rawValue = newVal
    },
    addMaterial () {
      Material.addMaterial(this.materialName).then(res => {
        this.$router.go(-1)
      })
    }
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
