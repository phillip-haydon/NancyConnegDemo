using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Nancy;

namespace NancyConnegDemo.Modules
{
    public class HomeModule : NancyModule
    {
        public class Product
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public decimal Price { get; set; }
        }

        public static IList<Product> Products = new List<Product>
        {
            new Product {Id = 1, Name = "Surface", Price = 499},
            new Product {Id = 2, Name = "iPad", Price = 899},
            new Product {Id = 3, Name = "Nexus 10", Price = 599},
            new Product {Id = 4, Name = "Think Pad", Price = 499},
            new Product {Id = 5, Name = "Yoga", Price = 699},
        };

        public dynamic Model = new ExpandoObject();

        public HomeModule()
        {
            Model.Deleted = false;

            Get["/"] = _ =>
            {
                Model.Products = Products;

                return View["index", Model];
            };

            Get[@"/delete/{id}"] = _ =>
            {
                var id      = (int) _.id;
                var item    = Products.Single(x => x.Id == id);
                
                Products.Remove(item);

                Model.Products = Products;
                Model.Deleted = true;

                return Negotiate
                    .WithModel((object) Model)
                    .WithMediaRangeModel("application/json", new
                    {
                        Model.Deleted
                    })
                    .WithView("index");
            };
        }
    }
}