using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.SignalR;
using RealTimeCart.Models;

namespace RealTimeCart.Controllers
{
    public class AdminController : BaseController
    {
        private readonly IRepository<Book> _repository;

        public AdminController() : this(new MemoryRepository<Book>())
        {}

        public AdminController(IRepository<Book> repository)
        {
            _repository = repository;
        }

        public void Put(int id, Book book)
        {
            if (!ModelState.IsValid) throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState));

            book.Id = id;
            var newBook = _repository.Update(book);
            ShoppingCartHub.Value.Clients.All.updateProductCount(newBook);
        }        
    }
}