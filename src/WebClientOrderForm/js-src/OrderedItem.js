
(function(global, undefined){
    global['so']['OrderedItem'] = function(options) {
        var opt = options || {};
        this['itemId'] = opt['itemId'] || "";
        this['qty'] = opt['qty'] || 0;
    };
})(window);
