<template>
    <div class="autocomplete control" :class="{'is-expanded': expanded}">
      <textarea-autosize
        @click.native.stop=""
        ref="input"
        v-model="newValue"
        :min-height="30"
        :important="false"
        @keyup.native.esc.prevent="isActive = false"
        @keydown.native.tab="tabPressed"
        @keydown.native.enter.prevent="enterPressed"
        @keydown.native.up.prevent="keyArrows('up')"
        @keydown.native.down.prevent="keyArrows('down')"
        v-bind="$attrs"
        class="input"
        @focus.native="focused"
        @blur.native="onBlur"
      ></textarea-autosize>
        <!-- <b-input
            v-model="newValue"
            ref="input"
            :size="size"
            :loading="loading"
            :rounded="rounded"
            :icon="icon"
            :icon-pack="iconPack"
            autocomplete="off"
            v-bind="$attrs"
            @focus="focused"
            @blur="onBlur"
            type="textarea"
            @keyup.native.esc.prevent="isActive = false"
            @keydown.native.tab="tabPressed"
            @keydown.native.enter.prevent="enterPressed"
            @keydown.native.up.prevent="keyArrows('up')"
            @keydown.native.down.prevent="keyArrows('down')"
        /> -->

        <transition name="fade">
            <div
                class="dropdown-menu"
                :class="{ 'is-opened-top': !isListInViewportVertically }"
                v-show="isActive && (data.length > 0 || hasEmptySlot)"
                ref="dropdown">
                <div class="dropdown-content">
                    <a
                        v-for="(option, index) in data"
                        :key="index"
                        class="dropdown-item"
                        :class="{ 'is-hovered': option === hovered }"
                        @click="setSelected(option)">

                        <slot
                            v-if="hasDefaultSlot"
                            :option="option"
                            :index="index"
                        />
                        <span v-else v-html="getValue(option)"/>
                    </a>
                    <div
                        v-if="data.length === 0"
                        class="dropdown-item is-disabled">
                        <slot name="empty"/>
                    </div>
                </div>
            </div>
        </transition>
    </div>
</template>
<script>
import FormElementMixin from './FormElementMixin'
import TextareaAutosize from './../TextareaAutosize'
export default {
  mixins: [FormElementMixin],
  components: {
    TextareaAutosize
  },
  inheritAttrs: false,
  props: {
    value: [Number, String],
    data: {
      type: Array,
      default: () => []
    },
    field: {
      type: String,
      default: 'value'
    },
    keepFirst: Boolean,
    clearOnSelect: Boolean,
    openOnFocus: {
      type: Boolean,
      default: true
    }
  },
  data () {
    return {
      selected: null,
      hovered: null,
      isActive: false,
      newValue: this.value,
      isListInViewportVertically: true,
      hasFocus: false,
      _isAutocomplete: true,
      _elementRef: 'input'
    }
  },
  computed: {
    /**
     * White-listed items to not close when clicked.
     * Add input, dropdown and all children.
     */
    whiteList () {
      const whiteList = []
      whiteList.push(this.$refs.input.$el.querySelector('textarea'))
      whiteList.push(this.$refs.dropdown)
      // Add all chidren from dropdown
      if (this.$refs.dropdown !== undefined) {
        const children = this.$refs.dropdown.querySelectorAll('*')
        for (const child of children) {
          whiteList.push(child)
        }
      }

      return whiteList
    },

    /**
     * Check if exists default slot
     */
    hasDefaultSlot () {
      return !!this.$scopedSlots.default
    },

    /**
     * Check if exists 'empty' slot
     */
    hasEmptySlot () {
      return !!this.$slots.empty
    }
  },
  watch: {
    /**
     * When dropdown is toggled, check the visibility to know when
     * to open upwards.
     */
    isActive (active) {
      if (active) {
        this.calcDropdownInViewportVertical()
      } else {
        this.$nextTick(() => this.setHovered(null))
        // Timeout to wait for the animation to finish before recalculating
        setTimeout(() => {
          this.calcDropdownInViewportVertical()
        }, 100)
      }
    },

    /**
     * When updating input's value
     *   1. Emit changes
     *   2. If value isn't the same as selected, set null
     *   3. Close dropdown if value is clear or else open it
     */
    newValue (value) {
      this.$emit('input', value)
      // Check if selected is invalid
      const currentValue = this.getValue(this.selected)
      if (currentValue && currentValue !== value) {
        this.setSelected(null, false)
      }
      // Close dropdown if input is clear or else open it
      if (this.hasFocus && (!this.openOnFocus || value)) {
        this.isActive = !!value
      }
    },

    /**
     * When v-model is changed:
     *   1. Update internal value.
     *   2. If it's invalid, validate again.
     */
    value (value) {
      this.newValue = value
      !this.isValid && this.$refs.input.checkHtml5Validity()
    },

    /**
     * Select first option if 'keep-first
     */
    data (value) {
      // Keep first option always pre-selected
      if (this.keepFirst) {
        this.selectFirstOption(value)
      }
    }
  },
  methods: {
    /**
     * Set which option is currently hovered.
     */
    setHovered (option) {
      if (option === undefined) return

      this.hovered = option
    },
    /**
      * Get value of an object property/path even if it's nested
      */
    getValueByPath (obj, path) {
      const value = path.split('.').reduce((o, i) => o[i], obj)
      return value
    },

    /**
     * Set which option is currently selected, update v-model,
     * update input value and close dropdown.
     */
    setSelected (option, closeDropdown = true) {
      if (option === undefined) return

      this.selected = option
      this.$emit('select', this.selected)
      if (this.selected !== null) {
        this.newValue = this.clearOnSelect ? '' : this.getValue(this.selected)
      }
      closeDropdown &&
        this.$nextTick(() => {
          this.isActive = false
        })
    },

    /**
     * Select first option
     */
    selectFirstOption (options) {
      this.$nextTick(() => {
        if (options.length) {
          // If has visible data or open on focus, keep updating the hovered
          if (
            this.openOnFocus ||
            (this.newValue !== '' && this.hovered !== options[0])
          ) {
            this.setHovered(options[0])
          }
        } else {
          this.setHovered(null)
        }
      })
    },

    /**
     * Enter key listener.
     * Select the hovered option.
     */
    enterPressed () {
      if (this.hovered === null) return
      this.setSelected(this.hovered)
    },

    /**
     * Tab key listener.
     * Select hovered option if it exists, close dropdown, then allow
     * native handling to move to next tabbable element.
     */
    tabPressed () {
      if (this.hovered === null) {
        console.log(3)
        this.isActive = false
        return
      }
      this.setSelected(this.hovered)
    },

    /**
     * Close dropdown if clicked outside.
     */
    clickedOutside (event) {
      if (this.$el !== event.target) {
        this.isActive = false
      }
    },

    /**
     * Return display text for the input.
     * If object, get value from path, or else just the value.
     * If hightlight, find the text with regex and make bold.
     */
    getValue (option) {
      if (!option) return

      const value =
        typeof option === 'object'
          ? this.getValueByPath(option, this.field)
          : option

      return value
    },

    /**
     * Calculate if the dropdown is vertically visible when activated,
     * otherwise it is openened upwards.
     */
    calcDropdownInViewportVertical () {
      this.$nextTick(() => {
        /**
         * this.$refs.dropdown may be undefined
         * when Autocomplete is conditional rendered
         */
        if (this.$refs.dropdown === undefined) return

        const rect = this.$refs.dropdown.getBoundingClientRect()

        this.isListInViewportVertically =
          rect.top >= 0 &&
          rect.bottom <=
            (window.innerHeight || document.documentElement.clientHeight)
      })
    },

    /**
     * Arrows keys listener.
     * If dropdown is active, set hovered option, or else just open.
     */
    keyArrows (direction) {
      const sum = direction === 'down' ? 1 : -1
      if (this.isActive) {
        let index = this.data.indexOf(this.hovered) + sum
        index = index > this.data.length - 1 ? this.data.length : index
        index = index < 0 ? 0 : index

        this.setHovered(this.data[index])

        const list = this.$refs.dropdown.querySelector('.dropdown-content')
        const element = list.querySelectorAll(
          '.dropdown-item:not(.is-disabled)'
        )[index]

        if (!element) return

        const visMin = list.scrollTop
        const visMax =
          list.scrollTop + list.clientHeight - element.clientHeight

        if (element.offsetTop < visMin) {
          list.scrollTop = element.offsetTop
        } else if (element.offsetTop >= visMax) {
          list.scrollTop =
            element.offsetTop - list.clientHeight + element.clientHeight
        }
      } else {
        this.isActive = true
      }
    },

    /**
     * Focus listener.
     * If value is the same as selected, select all text.
     */
    focused (event) {
      if (this.getValue(this.selected) === this.newValue) {
        this.$el.querySelector('textarea').select()
      }
      if (this.openOnFocus) {
        this.isActive = true
        if (this.keepFirst) {
          this.selectFirstOption(this.data)
        }
      }
      this.hasFocus = true
      this.$emit('focus', event)
    },

    /**
     * Blur listener.
     */
    onBlur (event) {
      this.hasFocus = false
      this.$emit('blur', event)
    }
  },
  created () {
    if (typeof window !== 'undefined') {
      document.addEventListener('click', this.clickedOutside)
      window.addEventListener('resize', this.calcDropdownInViewportVertical)
    }
  },
  beforeDestroy () {
    if (typeof window !== 'undefined') {
      document.removeEventListener('click', this.clickedOutside)
      window.removeEventListener('resize', this.calcDropdownInViewportVertical)
    }
  }
}
</script>

<style>
  .input {
    width:100% !important;
    direction:rtl;
    display:block;
    max-width:100%;
    line-height:1.5;
    padding-top: -5px;
    min-height: 15px !important;
    height:100%;
    resize: none;
	  overflow: hidden;
	  box-sizing: border-box;
  }
</style>
