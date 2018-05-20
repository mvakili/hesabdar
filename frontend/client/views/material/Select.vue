<template>
  <b-autocomplete ref="autoComplete"
    rounded
    v-model="name"
    :data="data"
    placeholder="کالا"
    :keep-first="true"
    field="name"
    @focus="loadAsyncData"
    :open-on-focus="true"
    @select="option => selected = option">
    <template slot="empty">No results found</template>
  </b-autocomplete>
</template>

<script>
import Material from './../../services/material'

export default {
  props: ['value'],
  data () {
    return {
      data: [],
      name: '',
      loadData: true,
      selected: {}
    }
  },
  methods: {
    loadAsyncData: function () {
      if (this.loadData) {
        Material.suggest(this.name || '').then(response => {
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
      this.loadAsyncData()
    },
    selected: function (val) {
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
.dropdown-content {
  box-shadow: 0 2px 3px rgba(10, 10, 10, 0.1), 0 0 0 1px rgba(10, 10, 10, 0.1);
  display: block;
}
.dropdown-menu {
  position: absolute;
  z-index: 1;
  background-color: white;
}
</style>
