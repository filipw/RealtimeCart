    var CartApp = CartApp || {};
    
    CartApp.AdminPage = function() {
        var self = this;
        self.Orders = ko.observableArray([]);
        self.AdminTools = new CartApp.AdminTools();
    };
    
    CartApp.AdminTools = function() {
        var self = this;
    
        self.reject = function (item) {
            var dataToSave = { id: item.id, approved: false };
            approveInternal(dataToSave);
        };
        
        self.approve = function (item) {
            var dataToSave = { id: item.id, approved: true };
            approveInternal(dataToSave);
        };
        
        var approveInternal = function(data) {
            $.ajax({
                url: "/api/order/" + data.id,
                data: JSON.stringify(data),
                type: "PUT",
                contentType: "application/json; charset=utf-8",
                dataType: "json"
            }).done(function() {
                var match = ko.utils.arrayFirst(viewModel.Orders(), function(found) {
                    return data.id === found.id;
                });
                match.approved(data.approved);
            }).error(function (data) {
                toastr.error("Order is invalid!");
            });;
        };
    };
    
    var viewModel;
    $(function () {
        viewModel = new CartApp.AdminPage();
        hub = $.connection.admin;
    
        ko.applyBindings(viewModel);
    
        hub.client.orderReceived = function (order) {
            toastr.info("New order (" + order.Id + ") received!");
            viewModel.Orders.push(new CartApp.Order(order));
        };
    
        $.connection.hub.start();
    
        $.get("/api/order", function (items) {
            $.each(items, function (idx, item) {
                viewModel.Orders.push(new CartApp.Order(item));
            });
        }, "json");
    });