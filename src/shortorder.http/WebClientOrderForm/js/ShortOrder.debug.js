// Short Order Cook Simple UI Example
// Author: Jim Cowart
// License: MIT (http://www.opensource.org/licenses/mit-license)
// Version 1.0

(function(global, undefined){
    if(window['so'] === undefined) {
        global['so'] = {};
    }
})(window);

(function(global, undefined){
    var Repository = function() {
        global['amplify']['request']['define']( "submitOrder", "ajax", {
            "url": "/order",
            "dataType": "json",
            "type": "PUT",
            "contentType" : "application/json"
        });

        /*
        // Test request for submit order...
        ['amplify']['request']['define']( "submitOrder", function( settings ) {
            settings.success("Order Received");
        });
        */

        /*['amplify']['request']['define']("getMenuItems", "ajax", {
            url: "/menu",
            dataType: "json",
            type: "GET",
            contentType : "application/json"
        });*/
        global['amplify']['request']['define']( "getMenuItems", function( settings ) {
            settings['success']([
                new global['so']['MenuItem']({"itemId":1, "description": "Burger", "imageSource": "./img/burger.png"}),
                new global['so']['MenuItem']({"itemId":2, "description": "Fries", "imageSource": "./img/fries.png"}),
                new global['so']['MenuItem']({"itemId":3, "description": "Drink", "imageSource": "./img/drink.png"}),
                new global['so']['MenuItem']({"itemId":4, "description": "Shake", "imageSource": "./img/shake.png"})
            ]);
        });

        global['amplify']['request']['define']("getOrderStatus", "ajax", {
            url: "/order/{orderNumber}",
            dataType: "json",
            type: "GET",
            contentType : "application/json"
        });
        /*
        global['amplify']['request']['define']("getOrderStatus", function( settings ) {
            var testData = [];
            var i = 0;
            for(i; i < global['so']['viewModel']['orders']().length; i++) {
                testData.push({
                                "OrderNumber": global['so']['viewModel']['orders']()[i]['orderNumber'](),
                                "Status": "It's in the Queue"
                              });
            }
            settings['success'](testData);
        });
        */

        global['amplify']['request']['define']("getNewOrderId", "ajax", {
            "url": "/uniqueid",
            "dataType": "json",
            "type": "GET",
            "contentType" : "application/json"
        });

        /*
        // Test request for unique id
        ['amplify']['request']['define']("getNewOrderId", function( settings ) {
            settings.success(Math.floor(Math.random() * 1000) * Math.floor(Math.random() * 1000));
        });
        */

        this['getUniqueId'] = function(observable) {
            global['amplify']['request']("getNewOrderId", function(data) { observable(data); });
        };

        this['getMenuList'] = function(callback) {
            global['amplify']['request']("getMenuItems", callback);
        };

        this['submitOrder'] = function(order) {
            var getEvaluatedMenuItems = function(menuItems) {
                var i = 0, newArray = [];
                for(i;i<menuItems.length;i++)
                {
                    newArray.push({
                        "Description": menuItems[i]['description'],
                        "ItemId": menuItems[i]['itemId'],
                        "Qty": menuItems[i]['qty']()
                    });
                }
                return newArray;
            };
            var items = getEvaluatedMenuItems(order['menuItems']());
            var createOrderCommand = {
                                        "Id": order['orderNumber'](),
                                        "CustomerName": order['customerName'](),
                                        "Items": items
                                     };
            var data = global['JSON']['stringify'](createOrderCommand);
            global['amplify']['request']("submitOrder", data, function() { global['so']['viewModel']['updateOrderStatus'](order['orderNumber'](), "Order Received"); });
        };

        this['getOrderStatus'] = function(orderNumbers) {
            var i = 0;
            for(i; i < orderNumbers.length; i++) {
                var reqData = {orderNumber: orderNumbers[i]};
                global['amplify']['request']("getOrderStatus", reqData, function(data) {
                    if(data !== undefined) {
                        var statusMsg = "You order rank is: " + data.Rank;
                        global['so']['viewModel']['updateOrderStatus'](reqData['orderNumber'], data['Status']);
                    }
                });
            }
        };
    };
    
    global['so']['repository'] = new Repository();
})(window);

(function(global, undefined) {
    global['so']['MenuItem'] = function(options) {
        var opt = options || {};
        this['itemId'] = opt['itemId'] || "";
        this['description'] = opt['description'] || "";
        this['imageSource'] = opt['imageSource'] || "";
    };
})(window);

(function(global, undefined){
    global['so']['OrderedItem'] = function(options) {
        var opt = options || {};
        this['itemId'] = opt['itemId'] || "";
        this['qty'] = opt['qty'] || 0;
    };
})(window);

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

        this['acquireNewOrderNumber']();
    };
})(window);

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

        this['updateOrderStatus'] = function(orderNumber, status) {
            var i = 0;
            for(i; i < this['orders']().length; i++) {
                if(this['orders']()[i]['orderNumber']() === orderNumber) {
                    this['orders']()[i]['status'](status);
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
            }, 5000);
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
