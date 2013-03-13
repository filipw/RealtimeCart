    var CartApp = CartApp || {};
    
    CartApp.Book = function (item) {
        var self = this;
        self.id = item.Id;
        self.title = item.Title;
        self.author = item.Author;
        self.quantity = ko.observable(item.Quantity);
        self.price = item.Price;
        self.selectedQuantity = ko.observable(item.SelectedQuantity);
    };
    
    CartApp.Order = function(item) {
        var self = this;
        self.id = item.Id;
        self.approved = ko.observable(item.Approved);
        self.customerName = item.CustomerName;
        self.orderTotal = item.OrderTotal;
        self.books = [];
        $.each(item.Books, function(idx, book) {
            self.books.push(new CartApp.Book(book));
        });
    };