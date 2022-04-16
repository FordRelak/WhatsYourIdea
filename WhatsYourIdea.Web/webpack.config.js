const path = require('path');
const { CleanWebpackPlugin } = require('clean-webpack-plugin');

module.exports = {
    entry: {
        main: path.resolve(__dirname, './assets/index.js'),
        vendor: path.resolve(__dirname, './assets/vendor.js'),
        home: path.resolve(__dirname, './assets/styles/pages/home/home.js'),
    },
    module: {
        rules: [
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
    ],
    devtool: 'inline-source-map',
    mode: 'development',
    output: {
        path: path.resolve(__dirname, './wwwroot/bundles'),
        filename: '[name].bundle.js',
    },
}