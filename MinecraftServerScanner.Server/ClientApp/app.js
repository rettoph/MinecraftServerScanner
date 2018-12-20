const Vue = require('vue');

Vue.component('minecraft-server', require('./components/minecraft-server.vue.html').default);

new Vue({
    el: "#app"
})