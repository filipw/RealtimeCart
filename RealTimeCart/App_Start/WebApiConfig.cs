using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using RealTimeCart.Models;

namespace RealTimeCart
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            MemoryRepository<Book>.Repo = new ConcurrentDictionary<int, Book>();
            MemoryRepository<Book>.Repo.TryAdd(1, new Book { Author = "Arkady Plotnitsky", Id = 1, Price = 10, Quantity = 10, Title = "Epistemology and Probability: Bohr, Heisenberg, Schrödinger, and the Nature of Quantum-Theoretical Thinking" });
            MemoryRepository<Book>.Repo.TryAdd(2, new Book { Author = "Brian Cox", Id = 2, Price = 15, Quantity = 100, Title = "The Quantum Universe: Everything that can happen does happen" });
            MemoryRepository<Book>.Repo.TryAdd(3, new Book { Author = "Jim Al-Khalili", Id = 3, Price = 20, Quantity = 1000, Title = "Quantum: A Guide For The Perplexed" });

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
