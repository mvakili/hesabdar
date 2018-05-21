<template>
  <b-autocomplete ref="autoComplete"
    rounded
    v-model="name"
    :data="data"
    placeholder=""
    :keep-first="true"
    field="name"
    @focus="focused($event.target)"
    :disabled="disabled"
    :open-on-focus="true"
    @select="option => selected = option">
    <template slot="empty">نتیجه ای پیدا نشد</template>
  </b-autocomplete>
</template>

<script>
import Dealer from './../../services/dealer'

export default {
  props: ['value', 'disabled', 'id'],
  data () {
    return {
      data: [],
      name: '',
      loadData: true,
      selected: {}
    }
  },
  methods: {
    focused: function (target) {
      this.loadAsyncData('')
      console.log(target)
      console.log(this.$refs.autoComplete)
      target.select()
    },
    loadAsyncData: function (name) {
      if (this.loadData) {
        Dealer.suggest(name || '').then(response => {
          this.data = response
        })
      }
      this.loadData = true
    },
    focus: function () {
      this.$refs.autoComplete.focus()
    }
  },
  watch: {
    name: function (val) {
      this.loadAsyncData(val)
    },
    selected: function (val) {
      this.$emit('input', this.selected)
      this.$emit('update:id', this.selected.id)
      this.$emit('changed', this.selected)
    },
    value: function (val) {
      if (this.value) {
        this.selected = this.value
        this.loadData = false
        this.name = this.value.name
      }
    }
  },
  mounted: function () {
    if (this.value) {
      this.selected = this.value
      this.loadData = false
      this.name = this.value.name
    }
  }
}
</script>

<style>
.autocomplete>.control {
  margin-bottom: auto;
}
.dropdown-item {
  display: inherit;
}
.is-hovered {
  background-color: lightblue;
}
.dropdown-content {
  box-shadow: 0 2px 3px rgba(10, 10, 10, 0.1), 0 0 0 1px rgba(10, 10, 10, 0.1);
  display: block;
}
.dropdown-menu {
  width: 100%;  
  position: absolute;
  z-index: 1;
  background-color: white;
}
</style>
