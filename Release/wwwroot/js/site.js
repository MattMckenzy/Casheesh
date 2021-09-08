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

PullAway = (element) => {
    element.classList.add("pull-away");
};

BringBack = (element) => {
    element.classList.remove("pull-away");
};

CardContainerExpand = (element) => {
    element.classList.add("card-container-expanded");
};

CardContainerShrink = (element) => {
    element.classList.remove("card-container-expanded");
};


var chartCallbacks = {
    formatCurrencyString: function (value, index, values) {
        return Number(value).toLocaleString(Culture, { style: 'currency', currency: CurrencyCode });
    }
}