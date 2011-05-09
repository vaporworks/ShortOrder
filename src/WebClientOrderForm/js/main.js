/*amplify.request.define( "submitOrder", "ajax", {
    url: "/order/{orderNumber}",
    dataType: "json",
    type: "PUT",
    contentType : "application/json"
});*/
amplify.request.define( "submitOrder", function( settings ) {
    settings.success("Order Received");
});

/*amplify.request.define("getMenuItems", "ajax", {
    url: "/menu",
    dataType: "json",
    type: "GET",
    contentType : "application/json"
});*/
amplify.request.define( "getMenuItems", function( settings ) {
	settings.success([
		new MenuItem({itemId:1, description: "Burger", imageSource: "./img/burger.png"}),
        new MenuItem({itemId:2, description: "Fries", imageSource: "./img/fries.png"}),
        new MenuItem({itemId:3, description: "Drink", imageSource: "./img/drink.png"}),
        new MenuItem({itemId:4, description: "Shake", imageSource: "./img/shake.png"})
	]);
});

/*amplify.request.define("getOrderStatus", "ajax", {
    url: "/orderStatus",
    dataType: "json",
    type: "POST",
    contentType : "application/json"
});*/
amplify.request.define("getOrderStatus", function( settings ) {
    var testData = [];
    var i = 0;
    for(i; i < window.viewModel.orders().length; i++) {
        testData.push({
                        OrderNumber: window.viewModel.orders()[i].orderNumber(),
                        Status: "It's in the Queue"
                      });
    }
    settings.success(testData);
});

/*amplify.request.define("getNewOrderId", "ajax", {
    url: "/uniqueid",
    dataType: "json",
    type: "GET",
    contentType : "application/json"
});*/

amplify.request.define("getNewOrderId", function( settings ) {
    settings.success(Math.floor(Math.random() * 1000) * Math.floor(Math.random() * 1000));
});

var repository = {
    getUniqueId: function(observable) {
        amplify.request("getNewOrderId", function(data) { observable(data); });
    },
    
    getMenuList: function(callback) {
        amplify.request("getMenuItems", callback);
    },

    submitOrder: function(order) {
        var createOrderCommand = {
                                    Id: order.orderNumber(),
                                    CustomerName: order.customerName(),
                                    ItemIds: order.menuItems()
                                 };
        var data = { request: JSON.stringify(createOrderCommand) };
        amplify.request("submitOrder", data, function() { window.viewModel.updateOrderStatus(order.orderNumber(), "Order Received"); });
    },

    getOrderStatus: function(orderNumbers) {
        var ords = { OrderNumbers: orderNumbers };
        var data = { request: JSON.stringify(ords) };
        amplify.request("getOrderStatus", data, function(data) {
            var x = 0;
            for(x; x < data.length; x ++) {
                window.viewModel.updateOrderStatus(data[x].OrderNumber, data[x].Status);
            }
        });
    }
};

var MenuItem = function(options) {
    var opt = options || {};
    this.itemId = opt.itemId || "";
    this.description = opt.description || "";
    this.imageSource = opt.imageSource || "";
};

var OrderedItem = function(options) {
    var opt = options || {};
    this.itemId = opt.itemId || "";
    this.qty = opt.qty || 0;
};

var Order = function(options) {
    var opt = options || {};

    this.orderNumber = new ko.observable(opt.orderNumber);

    this.acquireNewOrderNumber = function() {
        repository.getUniqueId(this.orderNumber);
    };

    this.customerName = new ko.observable(opt.customerName || "");

    this.menuItems = new ko.observableArray(opt.menuItems || []);

    this.status = new ko.observable(opt.status || "Pending");

    this.addOrderedItem = function(menuItem) {
        var i = 0;
        for(i; i < this.menuItems().length;i++) {
            if (this.menuItems()[i].itemId === menuItem.itemId) {
                this.menuItems()[i].qty(this.menuItems()[i].qty() + 1);
                return;
            }
        }
        var orderedItem = $.extend({ qty: new ko.observable(1) }, menuItem);
        orderedItem.qtyClass = new ko.dependentObservable(function() {
            var cls = "";
            switch(this.qty()){
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
        this.menuItems.push(orderedItem);
    };

    this.removeOrderedItem = function(itemId) {
        var i = 0;
        for(i; i < this.menuItems().length;i++) {
            if (this.menuItems()[i].itemId === itemId) {
                this.menuItems.remove(this.menuItems()[i]);
                return;
            }
        }
    }

    this.acquireNewOrderNumber();
};

window.ViewModel = function() {

    this.orderToPlace = new Order();

    this.orders = new ko.observableArray([]);

    this.validationMessages = new ko.observableArray([{msg: "You haven't placed an order yet..."}]);

    this.menu = new ko.observableArray([]);

    this.placeOrder = function() {
        var valid = true;
        this.validationMessages([]);
        if(!this.orderToPlace.customerName() || this.orderToPlace.customerName().length === 0) {
            valid = false;
            this.validationMessages.push({msg: "Hey - if you don't give us your name, we can't give you your order!"});
        }

        if(this.orderToPlace.menuItems().length === 0) {
            valid = false;
            this.validationMessages.push({msg:"Look, it's great if you're on a diet, but you can't place an order without actually ordering something."});
        }

        if(valid)  {
            var newOrder = new Order({
                                        orderNumber: this.orderToPlace.orderNumber(),
                                        customerName: this.orderToPlace.customerName(),
                                        menuItems: this.orderToPlace.menuItems()
                                     });
            this.orders.push(newOrder);
            repository.submitOrder(newOrder);
            this.clearOrder();
            this.orderToPlace.acquireNewOrderNumber();
        }
    };

    this.clearOrder = function() {
        this.orderToPlace.customerName("");
        this.orderToPlace.menuItems([]);
    };

    this.cancelOrder = function() {
        //TODO: implement this.....
    };

    this.loadMenu = function() {
        var menu = this.menu;
        repository.getMenuList(function(data) {
            var i = 0;
            for(i; i < data.length; i++) {
                menu.push(data[i]);
            }
        });
    };

    this.updateOrderStatus = function(orderNumber, status) {
        var i = 0;
        for(i; i < this.orders().length; i++) {
            if(this.orders()[i].orderNumber() === orderNumber) {
                this.orders()[i].status(status);
                break;
            }
        }
    };

    this.startPolling = function() {
        setInterval(function() {
            var ordNums = [];
            var i = 0;
            for(i; i < window.viewModel.orders().length; i++) {
                ordNums.push(window.viewModel.orders()[i].orderNumber());
            }
            repository.getOrderStatus(ordNums);
        }, 5000);
    };

    this.startPolling();
};

$(function(){
  ko.externaljQueryTemplateEngine.setOptions({
        templateUrl: "template",
        templateSuffix: ".html"
  });
  window.viewModel = new window.ViewModel();
  ko.applyBindings(window.viewModel);
  window.viewModel.loadMenu();
});