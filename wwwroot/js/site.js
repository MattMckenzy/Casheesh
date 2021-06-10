GetElementWidth = (element) => { return element.getBoundingClientRect().width; };

ChangeElementHeight = (element, newHeight) => {
    element.style.height = newHeight.toString() + 'px';
};

UncheckElement = (element) => {
    element.checked = false;
};

ShowContainer = (element) => {
    element.style.opacity = "1";
    element.style.pointerEvents = "auto";
};

HideContainer = (element) => {
    element.style.opacity = "0";
    element.style.pointerEvents = "none";
};

PullAway = (element, newTop) => {
    element.classList.add("pull-away");
};

BringBack = (element) => {
    element.classList.remove("pull-away");
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