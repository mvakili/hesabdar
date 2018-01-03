<template>
  <div>
    <div class="tile is-ancestor">


      <div class="tile is-parent">
        <article class="tile is-child box">
          <h1 class="title">ثبت کالای جدید</h1>
          <h2 class="subtitle"></h2>
          <div class="columns">
            <div class="column">
              <label class="label">نام کالا</label>
              <p class="control has-icon has-icon-right">
                <input class="input" v-model="materialName" type="text" placeholder="نام کالا" value="">
                <!-- <validator></validator> -->
              </p>
            </div>
          </div>
          <p class="control">
            <a class="button is-primary" v-on:click="addMaterial()">تایید</a>
            <router-link class="button"
              v-if="$routerHistory.hasHistory()"
              :to="{ path: $routerHistory.previous().path }">
                بی خیال
            </router-link>
          </p>
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
        console.log(res)
        this.$router.go(-1)
      })
    }
  }
}
</script>

<style lang="scss" scoped>
@import '~bulma/sass/utilities/mixins';
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
