const path = require('path');
const VueLoader = require('vue-loader');
const bundleOutputDir = './wwwroot/dist';


module.exports = {
    mode: 'development',
    entry: {
        main: './ClientApp/app.js'
    },
    output: {
        path: path.join(__dirname, bundleOutputDir),
        filename: '[name].js',
        publicPath: 'dist/'
    },
    resolve: {
        alias: {
            vue: 'vue/dist/vue.js'
        }
    },
    module: {
        rules: [
            { test: /\.vue\.html$/, include: /ClientApp/, loader: 'vue-loader', options: { loaders: { js: 'awesome-typescript-loader?silent=true' } } }
        ]
    },
    plugins: [
        new VueLoader.VueLoaderPlugin()
    ]
};