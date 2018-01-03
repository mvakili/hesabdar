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
                <input class="input" type="text" placeholder="نام کالا" value="">
                <!-- <validator></validator> -->
              </p>
            </div>
          </div>
          <p class="control">
            <router-link class="button is-primary"
              v-if="$routerHistory.hasHistory()"
              :to="{ path: $routerHistory.previous().path }">
                تایید
            </router-link>
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
import Cleave from 'vue-cleave'
import 'cleave.js/dist/addons/cleave-phone.cn'
export default {
  components: {
    Cleave
  },
  data () {
    return {
      demo: {
        value: '',
        rawValue: ''
      }
    }
  },
  methods: {
    onRawValueChanged (newVal) {
      this.demo.rawValue = newVal
    }
  },
  watch: {
    'demo.value' (newVal, oldVal) {
      console.log('value: new ->', newVal, 'old ->', oldVal)
    },
    'demo.rawValue' (newVal, oldVal) {
      console.log('rawValue: new ->', newVal, 'old ->', oldVal)
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
