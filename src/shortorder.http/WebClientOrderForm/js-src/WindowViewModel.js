
(function(global, undefined) {
    global['so']['ViewModel'] = function() {

        this['orderToPlace'] = new global['so']['Order']();

        this['orders'] = new global['ko']['observableArray']([]);

        this['validationMessages'] = new global['ko']['observableArray']([{"msg": "You haven't placed an order yet..."}]);

        this['menu'] = new global['ko']['observableArray']([]);

        this['placeOrder'] = function() {
            var valid = true;
            this['validationMessages']([]);
            if(!this['orderToPlace']['customerName']() || this['orderToPlace']['customerName']().length === 0) {
                valid = false;
                this['validationMessages'].push({"msg": "Hey - if you don't give us your name, we can't give you your order!"});
            }

            if(this['orderToPlace']['menuItems']().length === 0) {
                valid = false;
                this['validationMessages'].push({"msg":"Look, it's great if you're on a diet, but you can't place an order without actually ordering something."});
            }

            if(valid)  {
                var newOrder = new global['so']['Order']({
                                            "orderNumber": this['orderToPlace']['orderNumber'](),
                                            "customerName": this['orderToPlace']['customerName'](),
                                            "menuItems": this['orderToPlace']['menuItems']()
                                         });
                this['orders'].push(newOrder);
                global['so']['repository']['submitOrder'](newOrder);
                this['clearOrder']();
                this['orderToPlace']['acquireNewOrderNumber']();
            }
        };

        this['clearOrder'] = function() {
            this['orderToPlace']['customerName']("");
            this['orderToPlace']['menuItems']([]);
        };

        this['cancelOrder'] = function() {
            //TODO: implement this.....
        };

        this['loadMenu'] = function() {
            var menu = this['menu'];
            global['so']['repository']['getMenuList'](function(data) {
                var i = 0;
                for(i; i < data.length; i++) {
                    menu.push(data[i]);
                }
            });
        };

        this['updateOrderStatus'] = function(orderNumber, data) {
            var i = 0;
            for(i; i < this['orders']().length; i++) {
                if(this['orders']()[i]['orderNumber']() === orderNumber) {
                    if(!data.Complete) {
                        this['orders']()[i]['status'](data.statusMsg);
                    }
                    else {
                        this['orders'][i].remove(this['orders']()[i]);
                    }
                    break;
                }
            }
        };

        this['startPolling'] = function() {
            setInterval(function() {
                var ordNums = [];
                var i = 0;
                for(i; i < global['so']['viewModel']['orders']().length; i++) {
                    ordNums.push(global['so']['viewModel']['orders']()[i]['orderNumber']());
                }
                global['so']['repository']['getOrderStatus'](ordNums);
            }, 10000);
        };

        this['startPolling']();
    };
})(window);

$(function(){
  window['ko']['externaljQueryTemplateEngine']['setOptions']({
        "templateUrl": "template",
        "templateSuffix": ".html"
  });
  window['so']['viewModel'] = new window['so']['ViewModel']();
  window['ko']['applyBindings'](window['so']['viewModel']);
  window['so']['viewModel']['loadMenu']();
});
