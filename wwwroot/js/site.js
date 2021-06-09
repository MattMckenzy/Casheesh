GetElementWidth = (element) => { return element.getBoundingClientRect().width; };

ChangeElementHeight = (element, newHeight) => {
    element.style.height = newHeight.toString() + 'px';
};

UncheckElement = (element) => {
    element.checked = false;
};

var currencyFormatter = new Intl.NumberFormat("en-CA", {
    style: 'currency',
    currency: 'CAD',
    minimumFractionDigits: 2, 
    maximumFractionDigits: 2
});

var chartCallbacks = {
    formatCurrencyString: function (value, index, values) {
        return currencyFormatter.format(Number(value));
    }
}