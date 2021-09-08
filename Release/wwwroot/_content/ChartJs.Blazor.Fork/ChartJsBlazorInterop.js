"use strict";
class ChartJsInterop {
    constructor() {
        this.BlazorCharts = new Map();
    }
    setupChart(config) {
        if (!this.BlazorCharts.has(config.canvasId)) {
            this.wireUpCallbacks(config);
            let chart = new Chart(config.canvasId, config);
            this.BlazorCharts.set(config.canvasId, chart);
            return true;
        }
        else {
            return this.updateChart(config);
        }
    }
    updateChart(config) {
        if (!this.BlazorCharts.has(config.canvasId))
            throw `Could not find a chart with the given id. ${config.canvasId}`;
        let myChart = this.BlazorCharts.get(config.canvasId);
        this.mergeDatasets(myChart.config.data.datasets, config.data.datasets);
        this.mergeLabels(myChart.config.data, config.data);
        this.wireUpCallbacks(config);
        Chart.helpers.extend(myChart.config.options, config.options);
        myChart.update();
        return true;
    }
    mergeDatasets(oldDatasets, newDatasets) {
        for (let i = oldDatasets.length - 1; i >= 0; i--) {
            let sameDatasetInNewConfig = newDatasets.find(newD => newD.id === oldDatasets[i].id);
            if (sameDatasetInNewConfig === undefined) {
                oldDatasets.splice(i, 1);
            }
            else {
                Chart.helpers.extend(oldDatasets[i], sameDatasetInNewConfig);
            }
        }
        let currentIds = oldDatasets.map(dataset => dataset.id);
        newDatasets.filter(newDataset => !currentIds.includes(newDataset.id))
            .forEach(newDataset => oldDatasets.push(newDataset));
    }
    mergeLabels(oldChartData, newChartData) {
        const innerFunc = (oldLabels, newLabels) => {
            if (newLabels == null || newLabels.length === 0) {
                if (oldLabels) {
                    oldLabels.length = 0;
                }
                return oldLabels;
            }
            if (oldLabels == null) {
                return newLabels;
            }
            oldLabels.length = 0;
            for (var i = 0; i < newLabels.length; i++) {
                oldLabels.push(newLabels[i]);
            }
            return oldLabels;
        };
        oldChartData.labels = innerFunc(oldChartData.labels, newChartData.labels);
        oldChartData.xLabels = innerFunc(oldChartData.xLabels, newChartData.xLabels);
        oldChartData.yLabels = innerFunc(oldChartData.yLabels, newChartData.yLabels);
    }
    wireUpCallbacks(config) {
        this.wireUpOptionsOnClick(config);
        this.wireUpOptionsOnHover(config);
        this.wireUpLegendOnClick(config);
        this.wireUpLegendOnHover(config);
        this.wireUpLegendItemFilter(config);
        this.wireUpGenerateLabels(config);
        this.wireUpTickCallback(config);
    }
    wireUpOptionsOnClick(config) {
        let getDefaultFunc = type => {
            let defaults = Chart.defaults[type] || Chart.defaults.global;
            return (defaults === null || defaults === void 0 ? void 0 : defaults.onClick) || Chart.defaults.global.onClick;
        };
        if (!config.options)
            return;
        config.options.onClick = this.getMethodHandler(config.options.onClick, getDefaultFunc(config.type));
    }
    wireUpOptionsOnHover(config) {
        let getDefaultFunc = type => {
            let defaults = Chart.defaults[type] || Chart.defaults.global;
            return (defaults === null || defaults === void 0 ? void 0 : defaults.onHover) || Chart.defaults.global.onHover;
        };
        if (!config.options)
            return;
        config.options.onHover = this.getMethodHandler(config.options.onHover, getDefaultFunc(config.type));
    }
    wireUpLegendOnClick(config) {
        var _a;
        let getDefaultHandler = type => {
            var _a;
            let chartDefaults = Chart.defaults[type] || Chart.defaults.global;
            return ((_a = chartDefaults === null || chartDefaults === void 0 ? void 0 : chartDefaults.legend) === null || _a === void 0 ? void 0 : _a.onClick) || Chart.defaults.global.legend.onClick;
        };
        if (!((_a = config.options) === null || _a === void 0 ? void 0 : _a.legend))
            return;
        config.options.legend.onClick = this.getMethodHandler(config.options.legend.onClick, getDefaultHandler(config.type));
    }
    wireUpLegendOnHover(config) {
        var _a;
        let getDefaultFunc = type => {
            var _a;
            let chartDefaults = Chart.defaults[type] || Chart.defaults.global;
            return ((_a = chartDefaults === null || chartDefaults === void 0 ? void 0 : chartDefaults.legend) === null || _a === void 0 ? void 0 : _a.onHover) || Chart.defaults.global.legend.onHover;
        };
        if (!((_a = config.options) === null || _a === void 0 ? void 0 : _a.legend))
            return;
        config.options.legend.onHover = this.getMethodHandler(config.options.legend.onHover, getDefaultFunc(config.type));
    }
    wireUpLegendItemFilter(config) {
        var _a, _b;
        let getDefaultFunc = type => {
            var _a, _b;
            let chartDefaults = Chart.defaults[type] || Chart.defaults.global;
            return ((_b = (_a = chartDefaults === null || chartDefaults === void 0 ? void 0 : chartDefaults.legend) === null || _a === void 0 ? void 0 : _a.labels) === null || _b === void 0 ? void 0 : _b.filter) || Chart.defaults.global.legend.labels.filter;
        };
        if (!((_b = (_a = config.options) === null || _a === void 0 ? void 0 : _a.legend) === null || _b === void 0 ? void 0 : _b.labels))
            return;
        config.options.legend.labels.filter = this.getMethodHandler(config.options.legend.labels.filter, getDefaultFunc(config.type));
    }
    wireUpGenerateLabels(config) {
        var _a, _b;
        let getDefaultFunc = type => {
            var _a, _b;
            let chartDefaults = Chart.defaults[type] || Chart.defaults.global;
            return ((_b = (_a = chartDefaults === null || chartDefaults === void 0 ? void 0 : chartDefaults.legend) === null || _a === void 0 ? void 0 : _a.labels) === null || _b === void 0 ? void 0 : _b.generateLabels) || Chart.defaults.global.legend.labels.generateLabels;
        };
        if (!((_b = (_a = config.options) === null || _a === void 0 ? void 0 : _a.legend) === null || _b === void 0 ? void 0 : _b.labels))
            return;
        config.options.legend.labels.generateLabels = this.getMethodHandler(config.options.legend.labels.generateLabels, getDefaultFunc(config.type));
    }
    wireUpTickCallback(config) {
        var _a, _b, _c;
        const assignCallbacks = axes => {
            if (axes) {
                for (var i = 0; i < axes.length; i++) {
                    if (!axes[i].ticks)
                        continue;
                    axes[i].ticks.callback = this.getMethodHandler(axes[i].ticks.callback, undefined);
                    if (!axes[i].ticks.callback) {
                        delete axes[i].ticks.callback;
                    }
                }
            }
        };
        if ((_a = config.options) === null || _a === void 0 ? void 0 : _a.scales) {
            assignCallbacks(config.options.scales.xAxes);
            assignCallbacks(config.options.scales.yAxes);
        }
        if ((_c = (_b = config.options) === null || _b === void 0 ? void 0 : _b.scale) === null || _c === void 0 ? void 0 : _c.ticks) {
            config.options.scale.ticks.callback = this.getMethodHandler(config.options.scale.ticks.callback, undefined);
            if (!config.options.scale.ticks.callback) {
                delete config.options.scale.ticks.callback;
            }
        }
    }
    getMethodHandler(handler, defaultFunc) {
        if (handler == null) {
            return defaultFunc;
        }
        if (this.isDelegateHandler(handler)) {
            const stringifyArgs = (args) => {
                for (var i = 0; i < args.length; i++) {
                    if (handler.ignoredIndices.includes(i)) {
                        args[i] = '';
                    }
                    else {
                        args[i] = this.stringifyObjectIgnoreCircular(args[i]);
                    }
                }
                return args;
            };
            if (!handler.returnsValue) {
                return (...args) => handler.handlerReference.invokeMethodAsync(handler.methodName, stringifyArgs(args));
            }
            else {
                if (window.hasOwnProperty('MONO')) {
                    return (...args) => handler.handlerReference.invokeMethod(handler.methodName, stringifyArgs(args));
                }
                else {
                    console.warn('Using C# delegates that return values in chart.js callbacks is not supported on ' +
                        "server side blazor because the server side dispatcher doesn't support synchronous interop calls. Falling back to default value.");
                    return defaultFunc;
                }
            }
        }
        else {
            if (handler.methodName == null) {
                return defaultFunc;
            }
            const namespaceAndFunc = handler.methodName.split('.');
            if (namespaceAndFunc.length !== 2) {
                return defaultFunc;
            }
            const namespace = window[namespaceAndFunc[0]];
            if (namespace == null) {
                return defaultFunc;
            }
            const func = namespace[namespaceAndFunc[1]];
            if (typeof func === 'function') {
                return func;
            }
            else {
                return defaultFunc;
            }
        }
    }
    isDelegateHandler(handler) {
        return 'handlerReference' in handler;
    }
    stringifyObjectIgnoreCircular(object) {
        const seen = new WeakSet();
        const replacer = (_name, value) => {
            if (typeof value === 'object' &&
                value !== null &&
                !(value instanceof Boolean) &&
                !(value instanceof Date) &&
                !(value instanceof Number) &&
                !(value instanceof RegExp) &&
                !(value instanceof String)) {
                if (seen.has(value))
                    return undefined;
                seen.add(value);
            }
            return value;
        };
        return JSON.stringify(object, replacer);
    }
}
window[ChartJsInterop.name] = new ChartJsInterop();
