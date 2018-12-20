const Vue = require('vue');

Vue.component('minecraft-server', require('./components/minecraft-server.vue.html').default);
Vue.component('minecraft-servers', require('./components/minecraft-servers.vue.html').default);
Vue.component('chat', require('./components/chat.vue.html').default);

new Vue({
    el: "#app"
})