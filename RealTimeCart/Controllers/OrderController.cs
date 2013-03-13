using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using RealTimeCart.Models;

namespace RealTimeCart.Controllers
{
    public class OrderController : BaseController
    {
        private readonly IRepository<Book> _bookRepo;
        private readonly IRepository<Order> _orderRepo;

        public OrderController() : this(new MemoryRepository<Book>(), new MemoryRepository<Order>())
        {}

        public OrderController(IRepository<Book> bookRepo, IRepository<Order> orderRepo)
        {
            _bookRepo = bookRepo;
            _orderRepo = orderRepo;
        }

        public IEnumerable<Order> GetAll()
        {
            return _orderRepo.Items;
        }

        public void Put(int id, Approval approval)
        {
            if (!ModelState.IsValid) throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState));
            var order = _orderRepo.Get(id);
            if (order.Approved == approval.Approved) return;

            order.Approved = approval.Approved;
            _orderRepo.Update(order);

            ShoppingCartHub.Value.Clients.Client(order.CustomerName).orderApproved(order);

            if (!order.Approved)
            {
                if (!ValidateQuantities(order)) throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Order has illegal quantities!."));

                foreach (var book in order.Books)
                {   
                    var originalBook = _bookRepo.Get(book.Id);
                    originalBook.Quantity += book.SelectedQuantity;
                    _bookRepo.Update(originalBook);
                    ShoppingCartHub.Value.Clients.All.updateProductCount(originalBook);
                }
            }
        }

        public void Post(Order order)
        {
            if (!ModelState.IsValid) throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState));
            if (!ValidateQuantities(order)) throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Order has illegal quantities!."));

            foreach (var book in order.Books)
            {
                var originalBook = _bookRepo.Get(book.Id);
                originalBook.Quantity -= book.SelectedQuantity;
                _bookRepo.Update(originalBook);
                ShoppingCartHub.Value.Clients.All.updateProductCount(originalBook);
            }

            var added = _orderRepo.Add(order);
            AdminHub.Value.Clients.All.orderReceived(added);
        }

        private bool ValidateQuantities(Order order)
        {
            foreach (var book in order.Books)
            {
                var originalBook = _bookRepo.Get(book.Id);
                if (originalBook.Quantity < book.SelectedQuantity) return false;
            }

            return true;
        }
    }
}