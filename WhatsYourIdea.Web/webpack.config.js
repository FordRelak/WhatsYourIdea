const path = require('path');
const { CleanWebpackPlugin } = require('clean-webpack-plugin');
const webpack = require('webpack');
const copyWebpackPlugin = require('copy-webpack-plugin');

module.exports = {
    entry: {
        main: path.resolve(__dirname, './assets/index.js'),
        vendor: path.resolve(__dirname, './assets/vendor.js'),
        home: path.resolve(__dirname, './assets/home.js'),
        idea: path.resolve(__dirname, './assets/idea.js'),
        editor: path.resolve(__dirname, './assets/editor.js'),
    },
    module: {
        rules: [
            {
                test: /\.css$/i,
                use: [
                    "style-loader",
                    "css-loader"
                ],
            },
            {
                test: /\.s[ac]ss$/i,
                use: [
                    "style-loader",
                    "css-loader",
                    "sass-loader",
                ],
            },
            {
                test: /\.m?js$/,
                exclude: /(node_modules|bower_components)/,
                use: {
                    loader: 'babel-loader',
                    options: {
                        presets: ['@babel/preset-env']
                    }
                }
            },
            {
                test: /\.(woff|woff2|eot|ttf|otf)$/i,
                type: 'asset/resource',
            },
        ]
    },
    plugins: [
        new CleanWebpackPlugin(),
        new webpack.ProvidePlugin({
            $: 'jquery',
            jQuery: 'jquery',
            'window.jQuery': 'jquery'
        }),
        new copyWebpackPlugin({
            patterns: [
                { from: './node_modules/tinymce/plugins', to: './plugins' },
                { from: './node_modules/tinymce/themes', to: './themes' },
                { from: './node_modules/tinymce/skins', to: './skins' },
                { from: './node_modules/tinymce/models', to: './models' },
                { from: './node_modules/tinymce/icons', to: './icons' }
            ],
        }),
    ],
    devtool: 'inline-source-map',
    mode: 'development',
    output: {
        path: path.resolve(__dirname, './wwwroot/bundles'),
        filename: '[name].bundle.js',
    },
}