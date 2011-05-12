
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
            // forcing evaluation of observables before creating command
            var items = getEvaluatedMenuItems(order['menuItems']()),
                ordNum = order['orderNumber'](),
                custName = order['customerName']();
            var createOrderCommand = {
                                        "Id": ordNum,
                                        "CustomerName": custName,
                                        "Items": items
                                     };
            var data = global['JSON']['stringify'](createOrderCommand);
            global['amplify']['request']("submitOrder", data, function() { global['so']['viewModel']['updateOrderStatus'](order['orderNumber'](), "Order Transmitted"); });
        };

        this['getOrderStatus'] = function(orderNumbers) {
            var i = 0;
            for(i; i < orderNumbers.length; i++) {
                var reqData = {orderNumber: orderNumbers[i]};
                global['amplify']['request']("getOrderStatus", reqData, function(data) {
                    if(data !== undefined) {
                        var statusMsg = "You order rank is: " + data.Rank;
                        global['so']['viewModel']['updateOrderStatus'](reqData['orderNumber'], statusMsg);
                    }
                });
            }
        };
    };
    
    global['so']['repository'] = new Repository();
})(window);
