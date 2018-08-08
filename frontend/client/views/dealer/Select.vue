<template>
  <div>
  <p class="control  is-expanded" :class="{'has-addons': isNew}" style="direction:ltr">
    <button class="button is-warning" v-if="isNew" @click="newDealerModalVisible = true"><i class="fa fa-plus"></i></button>    
  <autocomplete style="direction:rtl" class="is-expanded" ref="autoComplete"
    rounded
    v-model="modelName"
    :data="data"
    placeholder="طرف حساب"
    :keep-first="true"
    field="name"
    @focus="focused($event.target)"
    :disabled="disabled"
    :open-on-focus="true"
    @select="option => selected = option || {}">
    <template slot="empty">
      </template>     
  </autocomplete>
  </p>
  <modal :visible="newDealerModalVisible" @close="newDealerModalVisible = false">
    <div class="content has-text-centered">
      <new :name="modelName" @onSuccess="added"></new>
    </div>
  </modal>
  </div>
  
</template>

<script>
import { Modal } from 'vue-bulma-modal'
import Dealer from './../../services/dealer'
import New from './New'
import Autocomplete from './../../templates/autocomplete'

export default {
  components: {
    Modal,
    New,
    Autocomplete
  },
  props: ['value', 'disabled', 'id'],
  data () {
    return {
      data: [],
      name: '',
      loadData: true,
      selected: {},
      priceHistoryModalVisible: false,
      newDealerModalVisible: false
    }
  },
  methods: {
    focused: function (target) {
      this.loadAsyncData('')
      target.select()
    },
    loadAsyncData: function (name) {
      if (this.loadData) {
        Dealer.suggest(name || '').then(response => {
          this.data.splice(0, this.data.length)
          for (let index = 0; index < response.length; index++) {
            const element = response[index]
            this.data.push(element)
          }
        })
      }
      this.loadData = true
    },
    focus: function () {
      this.$refs.autoComplete.focus()
    },
    added: function (dealer) {
      this.modelName = dealer.name
      this.selected = dealer
      this.newDealerModalVisible = false
    }
  },
  watch: {
    name: function (val) {
      this.loadAsyncData(val)
    },
    selected: function (val) {
      if (val.id) {
        this.$emit('input', val)
        this.$emit('update:id', val.id)
        this.$emit('changed', val)
      }
    },
    value: function (val) {
      if (this.value) {
        this.selected = this.value
        this.loadData = false
        this.modelName = this.value.name
      }
    }
  },
  computed: {
    isNew: function () {
      return (this.modelName.length > 0 && this.modelName !== (this.selected ? this.selected.name : ''))
    },
    modelName: {
      get: function () {
        return this.name
      },
      set: function (val) {
        this.name = val || ''
      }
    }
  },
  mounted: function () {
    if (this.value) {
      this.selected = this.value
      this.modelName = (this.value ? this.value.name : '')
      if (this.value.id) {
        this.loadData = false
      }
    }
  }
}
</script>

<style>
.autocomplete {
  width: 100%;
}
.autocomplete input {
  width: 100% !important;
}
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
.is-hovered {
  background-color: lightblue;
}
.dropdown-menu {
  width: 100%;
  position: absolute;
  z-index: 1;
  background-color: white;
}
.modal-content {
  overflow: auto;
}
</style>
