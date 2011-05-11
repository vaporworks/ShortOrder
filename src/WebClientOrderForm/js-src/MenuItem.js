
(function(global, undefined) {
    global['so']['MenuItem'] = function(options) {
        var opt = options || {};
        this['itemId'] = opt['itemId'] || "";
        this['description'] = opt['description'] || "";
        this['imageSource'] = opt['imageSource'] || "";
    };
})(window);
