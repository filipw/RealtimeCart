    var CartApp = CartApp || {};
    
    CartApp.Page = function() {
        var self = this;
        self.Cart = new CartApp.Cart();
        self.Books = ko.observableArray([]);
        self.UserId = "";
    };
    
    CartApp.Cart = function() {
        var self = this;
    
        self.items = ko.observableArray([]);
        self.add = function (item) {
            if (item.selectedQuantity() <= item.quantity && item.selectedQuantity() > 0) {
                self.items.remove(function (p) { return p.id === item.id; });
                self.items.push(new CartApp.CartItem(item));
            }
        };
    
        self.remove = function (item) {
            self.items.remove(function(p) { return p.id === item.id; });
        };
    
        self.checkOut = function () {
            var dataToSave = { CustomerName: viewModel.UserId, OrderTotal: self.grandTotal() };
            dataToSave.Books = [];
            $.each(self.items(), function (idx, item) {
                var dataItem = ko.toJS(item.product);
                dataToSave.Books.push(dataItem);
            });
    
            $.ajax({
                url: "/api/order",
                data: JSON.stringify(dataToSave),
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataType: "json"
            }).done(function() {
                self.items.removeAll();
                $.each(viewModel.Books(), function (idx,book) {
                    console.log(book);
                    book.selectedQuantity(0);
                });
                toastr.success("Your order has been submitted!");
            }).error(function(data) {
                toastr.error("Your order is invalid!");
            });
        };
    
        self.grandTotal = ko.computed(function() {
            var total = 0;
            $.each(self.items(), function () { total += this.subtotal(); });
            return total;
        });
    };
    
    CartApp.CartItem = function(book) {
    var self = this;

    self.id = book.id;
    self.product = book;
    self.quantity = ko.observable(book.selectedQuantity());
    self.subtotal = ko.computed(function() {
        return self.product ? self.product.price * parseInt("0" + self.quantity(), 10) : 0;
    });
};
    
    var viewModel;
    $(function () {
        viewModel = new CartApp.Page();
        hub = $.connection.cart;
    
        ko.applyBindings(viewModel);
    
        hub.client.updateProductCount = function (book) {
            var match = ko.utils.arrayFirst(viewModel.Books(), function (item) {
                return book.Id === item.id;
            });
            toastr.info("Product ("+ book.Id +") stock count changed!");
            match.quantity(book.Quantity);
        };
        
        hub.client.orderApproved = function (approval) {
            if (approval.Approved) {
                toastr.success("Your order " + approval.id + " has been approved!");
            } else {
                toastr.error("Your order " + approval.id + " has been rejected!");
            }
        };
    
        $.connection.hub.start().done(function() {
            viewModel.UserId = $.connection.hub.id;
        });
    
        $.get("/api/book", function (items) {
            $.each(items, function (idx, item) {
                viewModel.Books.push(new CartApp.Book(item));
            });
        }, "json");
    });