// Short Order Cook Simple UI Example
// Author: Jim Cowart
// License: MIT (http://www.opensource.org/licenses/mit-license)
// Version 1.0

window.so===void 0&&(window.so={});
(function(b){b.so.repository=new function(){b.amplify.request.define("submitOrder","ajax",{url:"/order",dataType:"json",type:"PUT",contentType:"application/json"});b.amplify.request.define("getMenuItems",function(a){a.success([new b.so.MenuItem({itemId:1,description:"Burger",imageSource:"./img/burger.png"}),new b.so.MenuItem({itemId:2,description:"Fries",imageSource:"./img/fries.png"}),new b.so.MenuItem({itemId:3,description:"Drink",imageSource:"./img/drink.png"}),new b.so.MenuItem({itemId:4,description:"Shake",
imageSource:"./img/shake.png"})])});b.amplify.request.define("getOrderStatus",function(a){for(var d=[],c=0;c<b.so.viewModel.orders().length;c++)d.push({OrderNumber:b.so.viewModel.orders()[c].orderNumber(),Status:"It's in the Queue"});a.success(d)});b.amplify.request.define("getNewOrderId","ajax",{url:"/uniqueid",dataType:"json",type:"GET",contentType:"application/json"});this.getUniqueId=function(a){b.amplify.request("getNewOrderId",function(b){a(b)})};this.getMenuList=function(a){b.amplify.request("getMenuItems",
a)};this.submitOrder=function(a){var d=function(a){for(var b=0,d=[];b<a.length;b++)d.push({Description:a[b].description,ItemId:a[b].itemId,Qty:a[b].qty()});return d}(a.menuItems()),d={Id:a.orderNumber(),CustomerName:a.customerName(),Items:d},d=b.JSON.stringify(d);b.amplify.request("submitOrder",d,function(){b.so.viewModel.updateOrderStatus(a.orderNumber(),"Order Received")})};this.getOrderStatus=function(a){a=b.JSON.stringify({OrderNumbers:a});b.amplify.request("getOrderStatus",a,function(a){for(var c=
0;c<a.length;c++)b.so.viewModel.updateOrderStatus(a[c].OrderNumber,a[c].Status)})}}})(window);(function(b){b.so.MenuItem=function(a){a=a||{};this.itemId=a.itemId||"";this.description=a.description||"";this.imageSource=a.imageSource||""}})(window);(function(b){b.so.OrderedItem=function(a){a=a||{};this.itemId=a.itemId||"";this.qty=a.qty||0}})(window);
(function(b){b.so.Order=function(a){a=a||{};this.orderNumber=new b.ko.observable(a.orderNumber);this.acquireNewOrderNumber=function(){b.so.repository.getUniqueId(this.orderNumber)};this.customerName=new b.ko.observable(a.customerName||"");this.menuItems=new b.ko.observableArray(a.menuItems||[]);this.status=new b.ko.observable(a.status||"Pending");this.addOrderedItem=function(a){for(var c=0;c<this.menuItems().length;c++)if(this.menuItems()[c].itemId===a.itemId){this.menuItems()[c].qty(this.menuItems()[c].qty()+
1);return}a=$.extend({qty:new b.ko.observable(1)},a);a.qtyClass=new b.ko.dependentObservable(function(){var a="";switch(this.qty()){case 1:a="lightQty";break;case 2:a="dimQty";break;case 3:a="darkQty";break;default:a="darkestQty"}return a},a);this.menuItems.push(a)};this.removeOrderedItem=function(a){for(var b=0;b<this.menuItems().length;b++)if(this.menuItems()[b].itemId===a){this.menuItems.remove(this.menuItems()[b]);break}};this.acquireNewOrderNumber()}})(window);
(function(b){b.so.ViewModel=function(){this.orderToPlace=new b.so.Order;this.orders=new b.ko.observableArray([]);this.validationMessages=new b.ko.observableArray([{msg:"You haven't placed an order yet..."}]);this.menu=new b.ko.observableArray([]);this.placeOrder=function(){var a=!0;this.validationMessages([]);if(!this.orderToPlace.customerName()||this.orderToPlace.customerName().length===0)a=!1,this.validationMessages.push({msg:"Hey - if you don't give us your name, we can't give you your order!"});
this.orderToPlace.menuItems().length===0&&(a=!1,this.validationMessages.push({msg:"Look, it's great if you're on a diet, but you can't place an order without actually ordering something."}));a&&(a=new b.so.Order({orderNumber:this.orderToPlace.orderNumber(),customerName:this.orderToPlace.customerName(),menuItems:this.orderToPlace.menuItems()}),this.orders.push(a),b.so.repository.submitOrder(a),this.clearOrder(),this.orderToPlace.acquireNewOrderNumber())};this.clearOrder=function(){this.orderToPlace.customerName("");
this.orderToPlace.menuItems([])};this.cancelOrder=function(){};this.loadMenu=function(){var a=this.menu;b.so.repository.getMenuList(function(b){for(var c=0;c<b.length;c++)a.push(b[c])})};this.updateOrderStatus=function(a,b){for(var c=0;c<this.orders().length;c++)if(this.orders()[c].orderNumber()===a){this.orders()[c].status(b);break}};this.startPolling=function(){setInterval(function(){for(var a=[],d=0;d<b.so.viewModel.orders().length;d++)a.push(b.so.viewModel.orders()[d].orderNumber());b.so.repository.getOrderStatus(a)},
5E3)};this.startPolling()}})(window);$(function(){window.ko.externaljQueryTemplateEngine.setOptions({templateUrl:"template",templateSuffix:".html"});window.so.viewModel=new window.so.ViewModel;window.ko.applyBindings(window.so.viewModel);window.so.viewModel.loadMenu()});