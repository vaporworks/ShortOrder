
(function(global, undefined) {
    global['so']['Order'] = function(options) {
        var opt = options || {};

        this['orderNumber'] = new global['ko']['observable'](opt['orderNumber']);

        this['acquireNewOrderNumber'] = function() {
            global['so']['repository']['getUniqueId'](this['orderNumber']);
        };

        this['customerName'] = new global['ko']['observable'](opt['customerName'] || "");

        this['menuItems'] = new global['ko']['observableArray'](opt['menuItems'] || []);

        this['status'] = new global['ko']['observable'](opt['status'] || "Pending");

        this['addOrderedItem'] = function(menuItem) {
            var i = 0;
            for(i; i < this['menuItems']().length;i++) {
                if (this['menuItems']()[i]['itemId'] === menuItem['itemId']) {
                    this['menuItems']()[i]['qty'](this['menuItems']()[i]['qty']() + 1);
                    return;
                }
            }
            var orderedItem = $.extend({ "qty": new global['ko']['observable'](1) }, menuItem);
            orderedItem['qtyClass'] = new global['ko']['dependentObservable'](function() {
                var cls = "";
                switch(this['qty']()){
                    case 1:
                        cls = "lightQty";
                        break;
                    case 2:
                        cls = "dimQty";
                        break;
                    case 3:
                        cls = "darkQty";
                        break;
                    default:
                        cls = "darkestQty";
                        break;
                };
                return cls;
            }, orderedItem);
            this['menuItems'].push(orderedItem);
        };

        this['removeOrderedItem'] = function(itemId) {
            var i = 0;
            for(i; i < this['menuItems']().length;i++) {
                if (this['menuItems']()[i]['itemId'] === itemId) {
                    this['menuItems'].remove(this['menuItems']()[i]);
                    return;
                }
            }
        }

        if(!this['orderNumber']()) {
            this['acquireNewOrderNumber']();
        }
    };
})(window);
