// Short Order Cook Simple UI Example
// Author: Jim Cowart
// License: MIT (http://www.opensource.org/licenses/mit-license)
// Version 1.0

window.so===void 0&&(window.so={});
(function(a,b){a.so.repository=new function(){a.amplify.request.define("submitOrder","ajax",{url:"/order",dataType:"json",type:"PUT",contentType:"application/json"});a.amplify.request.define("getMenuItems",function(b){b.success([new a.so.MenuItem({itemId:1,description:"Burger",imageSource:"./img/burger.png"}),new a.so.MenuItem({itemId:2,description:"Fries",imageSource:"./img/fries.png"}),new a.so.MenuItem({itemId:3,description:"Drink",imageSource:"./img/drink.png"}),new a.so.MenuItem({itemId:4,description:"Shake",
imageSource:"./img/shake.png"})])});a.amplify.request.define("getOrderStatus","ajax",{url:"/order/{orderNumber}",dataType:"json",type:"GET",contentType:"application/json"});a.amplify.request.define("getNewOrderId","ajax",{url:"/uniqueid",dataType:"json",type:"GET",contentType:"application/json"});this.getUniqueId=function(b){a.amplify.request("getNewOrderId",function(a){b(a)})};this.getMenuList=function(b){a.amplify.request("getMenuItems",b)};this.submitOrder=function(b){var c=function(b){for(var a=
0,c=[];a<b.length;a++)c.push({Description:b[a].description,ItemId:b[a].itemId,Qty:b[a].qty()});return c}(b.menuItems()),c={Id:b.orderNumber(),CustomerName:b.customerName(),Items:c},c=a.JSON.stringify(c);a.amplify.request("submitOrder",c,function(){a.so.viewModel.updateOrderStatus(b.orderNumber(),"Order Received")})};this.getOrderStatus=function(d){for(var c=0;c<d.length;c++){var e={a:d[c]};a.amplify.request("getOrderStatus",e,function(c){c!==b&&a.so.viewModel.updateOrderStatus(e.orderNumber,c.Status)})}}}})(window);
(function(a){a.so.MenuItem=function(a){a=a||{};this.itemId=a.itemId||"";this.description=a.description||"";this.imageSource=a.imageSource||""}})(window);(function(a){a.so.OrderedItem=function(a){a=a||{};this.itemId=a.itemId||"";this.qty=a.qty||0}})(window);
(function(a){a.so.Order=function(b){b=b||{};this.orderNumber=new a.ko.observable(b.orderNumber);this.acquireNewOrderNumber=function(){a.so.repository.getUniqueId(this.orderNumber)};this.customerName=new a.ko.observable(b.customerName||"");this.menuItems=new a.ko.observableArray(b.menuItems||[]);this.status=new a.ko.observable(b.status||"Pending");this.addOrderedItem=function(b){for(var c=0;c<this.menuItems().length;c++)if(this.menuItems()[c].itemId===b.itemId){this.menuItems()[c].qty(this.menuItems()[c].qty()+
1);return}b=$.extend({qty:new a.ko.observable(1)},b);b.qtyClass=new a.ko.dependentObservable(function(){var a="";switch(this.qty()){case 1:a="lightQty";break;case 2:a="dimQty";break;case 3:a="darkQty";break;default:a="darkestQty"}return a},b);this.menuItems.push(b)};this.removeOrderedItem=function(a){for(var b=0;b<this.menuItems().length;b++)if(this.menuItems()[b].itemId===a){this.menuItems.remove(this.menuItems()[b]);break}};this.acquireNewOrderNumber()}})(window);
(function(a){a.so.ViewModel=function(){this.orderToPlace=new a.so.Order;this.orders=new a.ko.observableArray([]);this.validationMessages=new a.ko.observableArray([{msg:"You haven't placed an order yet..."}]);this.menu=new a.ko.observableArray([]);this.placeOrder=function(){var b=!0;this.validationMessages([]);if(!this.orderToPlace.customerName()||this.orderToPlace.customerName().length===0)b=!1,this.validationMessages.push({msg:"Hey - if you don't give us your name, we can't give you your order!"});
this.orderToPlace.menuItems().length===0&&(b=!1,this.validationMessages.push({msg:"Look, it's great if you're on a diet, but you can't place an order without actually ordering something."}));b&&(b=new a.so.Order({orderNumber:this.orderToPlace.orderNumber(),customerName:this.orderToPlace.customerName(),menuItems:this.orderToPlace.menuItems()}),this.orders.push(b),a.so.repository.submitOrder(b),this.clearOrder(),this.orderToPlace.acquireNewOrderNumber())};this.clearOrder=function(){this.orderToPlace.customerName("");
this.orderToPlace.menuItems([])};this.cancelOrder=function(){};this.loadMenu=function(){var b=this.menu;a.so.repository.getMenuList(function(a){for(var c=0;c<a.length;c++)b.push(a[c])})};this.updateOrderStatus=function(a,d){for(var c=0;c<this.orders().length;c++)if(this.orders()[c].orderNumber()===a){this.orders()[c].status(d);break}};this.startPolling=function(){setInterval(function(){for(var b=[],d=0;d<a.so.viewModel.orders().length;d++)b.push(a.so.viewModel.orders()[d].orderNumber());a.so.repository.getOrderStatus(b)},
5E3)};this.startPolling()}})(window);$(function(){window.ko.externaljQueryTemplateEngine.setOptions({templateUrl:"template",templateSuffix:".html"});window.so.viewModel=new window.so.ViewModel;window.ko.applyBindings(window.so.viewModel);window.so.viewModel.loadMenu()});
