var repository = {
    getUniqueId: function() {
        return Math.floor(Math.random() * 1000) * Math.floor(Math.random() * 1000);
    },
    
    getMenuList: function() {
        return [
            new MenuItem({itemId:1, description: "Burger", imageSource: "./img/burger.png"}),
            new MenuItem({itemId:2, description: "Fries", imageSource: "./img/fries.png"}),
            new MenuItem({itemId:3, description: "Drink", imageSource: "./img/drink.png"}),
            new MenuItem({itemId:4, description: "Shake", imageSource: "./img/shake.png"})
        ];
    },

    submitOrder: function() {
        //TODO: tie in amplify.js to submit ajax call
    },

    getOrderStatus: function(orderNumber) {
        //TODO: tie in amplify.js to make a request for all pending orders, etc.
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
    this.orderNumber = new ko.observable(opt.orderNumber || repository.getUniqueId());
    this.customerName = new ko.observable(opt.customerName || "");
    this.menuItems = new ko.observableArray(opt.menuItems || []);
    this.status = new ko.observable("Pending");
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
};

window.ViewModel = function() {
    this.orderToPlace = new Order();

    this.orders = new ko.observableArray([]);

    this.validationMessages = new ko.observableArray([{msg: "You haven't placed an order yet..."}]);

    this.menu = new ko.observableArray(repository.getMenuList());

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
            this.orders.push(new Order({
                                            orderNumber: this.orderToPlace.orderNumber(),
                                            customerName: this.orderToPlace.customerName(),
                                            menuItems: this.orderToPlace.menuItems()
                                            }));
            this.clearOrder();
        }
    };

    this.clearOrder = function() {
        this.orderToPlace.customerName("");
        this.orderToPlace.menuItems([]);
    };

    this.cancelOrder = function() {
        //TODO: implement this.....
    };
};

$(function(){
  ko.externaljQueryTemplateEngine.setOptions({
        templateUrl: "template",
        templateSuffix: ".html"
  });
  window.viewModel = new window.ViewModel();
  ko.applyBindings(window.viewModel);
});