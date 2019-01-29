<template>
  <div id="app" class="is-unselectable">
    <nprogress-container></nprogress-container>
    <navbar :show="true"></navbar>
    <sidebar :show="sidebar.opened && !sidebar.hidden"></sidebar>
    <app-main></app-main>
  </div>
</template>

<script>
import NprogressContainer from 'vue-nprogress/src/NprogressContainer'
import { Navbar, Sidebar, AppMain, FooterBar } from 'components/layout/'
import { mapGetters, mapActions } from 'vuex'

export default {
  components: {
    Navbar,
    Sidebar,
    AppMain,
    FooterBar,
    NprogressContainer
  },

  beforeMount () {
    const { body } = document
    const WIDTH = 757
    const RATIO = 3

    const handler = () => {
      if (!document.hidden) {
        let rect = body.getBoundingClientRect()
        let isMobile = rect.width - RATIO < WIDTH
        this.toggleDevice(isMobile ? 'mobile' : 'other')
        this.toggleSidebar({
          opened: !isMobile
        })
      }
    }

    document.addEventListener('visibilitychange', handler)
    window.addEventListener('DOMContentLoaded', handler)
    window.addEventListener('resize', handler)
  },
  computed: mapGetters({
    sidebar: 'sidebar'
  }),

  methods: mapActions([
    'toggleDevice',
    'toggleSidebar'
  ])
}
</script>

<style lang="scss">
@import '~animate.css';
$iransans-font-path: 'assets/fonts';
@import '~iransans-fontface/css/iransans/sass/iransans-fontface-regular';
.animated {
  animation-duration: .377s;
}

@import '~bulma';

$fa-font-path: "assets/fonts" !default;
@import '~font-awesome/scss/font-awesome';

html {
  background-color: whitesmoke;
}

.nprogress-container {
  position: fixed !important;
  width: 100%;
  height: 50px;
  z-index: 2048;
  pointer-events: none;

  #nprogress {
    $color: #48e79a;

    .bar {
      background: $color;
    }
    .peg {
      box-shadow: 0 0 10px $color, 0 0 5px $color;
    }

    .spinner-icon {
      border-top-color: $color;
      border-left-color: $color;
    }
  }
}

.p-datetime-picker-input-group input {
  display: table !important;
}
.control {
  text-align: right;
}
.dropdown {
  left: auto !important;
}
.notifications {
  right: auto !important;
  top: auto !important;
  
  left: 0px !important;
  bottom: 50px !important;
}
body, button, input, select, textarea {
  font-family: "IRANSansWeb";
}
.modal-content {
  overflow: visible !important;
}
.disable-event {
  pointer-events: none;
  background-color: whitesmoke;
}
.disable-event input, .disable-event span {
  background-color: whitesmoke;
  color: #7a7a7a;
}
tr.is-empty>td>section.section {
  padding: 0px;
}
.p-datetime-picker-container {
  position: sticky !important;
}
.p-datetime-picker-time .p-datetime-picker-time-h, .p-datetime-picker-time .p-datetime-picker-time-m {
  font-family: inherit !important;
}
.vpd-weekday {
  float:  left !important;
}
.vpd-day {
  float:  left !important;
}
</style>
