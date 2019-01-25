using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using ECommerceApp.Models;
using ECommerceApp.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;

namespace ECommerceApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly DataContext _context;
        private readonly IRepository _repo;

        public HomeController(DataContext context, IRepository repo)
        {
            this._context = context;
            this._repo = repo;
        }


        // GET: /Home/
        [HttpGet]
        [Route("dashboard")]
        public IActionResult Dashboard()
        {
            if(HttpContext.Session.GetString("loggedin") == "true")
            {
                int? userId = HttpContext.Session.GetInt32("ID");
                String userName = HttpContext.Session.GetString("Name");
              
                var allproducts = _context.Products.ToList();
                float sum = 0;
                foreach(var i in allproducts)
                {
                    sum+=i.Price;
                }
                                 
                ViewBag.userName = userName;
                ViewBag.userId = userId;
                ViewBag.sum = sum;
                ViewBag.allproducts = allproducts;

                return View();
            }
            return RedirectToAction("Index", "Auth");
        }



        [HttpGet("allproducts")]
        public IActionResult AllProducts()
        {
            if(HttpContext.Session.GetString("loggedin") == "true")
            {
                int? userId = HttpContext.Session.GetInt32("ID");
                String userName = HttpContext.Session.GetString("Name");
                // var allproducts = _context.Products.ToList();
                var allproducts = _repo.AllProducts();
                
                float sum=0;
                
                foreach(var i in allproducts){
                    sum+=i.Price;
                    System.Console.WriteLine(i.Price);
                }

                ViewBag.userName = userName;
                ViewBag.userId = userId;
                ViewBag.allproducts = allproducts;
                ViewBag.sum = sum;

                return View();

                // Get most ordered products
                
            }
            return RedirectToAction("Index", "Auth");
        }

        [HttpGet("addproduct")]
        public IActionResult ProductForm()
        {
            return View();
        }

        [HttpGet("product/{ProductId}")]
        public IActionResult ProductInfo(int ProductId)
        {   
            // var currentproduct = _context.Products.SingleOrDefault(p=>p.ProductId == ProductId);
            var currentproduct = _repo.GetProductById(ProductId);
            ViewBag.currentproduct = currentproduct;
            return View();
        }



        [HttpPost("createproduct")]
        public IActionResult CreateProduct(Product pro)
        {
            if(ModelState.IsValid)
            {
                Product newProduct = new Product
                {
                    ProductName = pro.ProductName,
                    ProductDescription = pro.ProductDescription,
                    InitialQuantity = pro.InitialQuantity,
                    Price = pro.Price,
                    ImageUrl=pro.ImageUrl,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                };

                _context.Add(newProduct);
                _context.SaveChanges();
                return RedirectToAction("AllProducts");
            }
            return View("AllProducts");
            
        }



        [HttpGet("orders")]
        public IActionResult OrderPage()
        {

            if(HttpContext.Session.GetString("loggedin") == "true")
            {
                // var allorders = _context.Orders
                // .Include(o=>o.Creator)
                // .Where(u=> u.UserID == HttpContext.Session.GetInt32("ID"))
                // .OrderByDescending(x=>x.OrderDate)
                // .ToList();
                System.Console.WriteLine("#$$$$$$$$$$$$$$$$$$$$$$$$$");
                // System.Console.WriteLine();
                var allorders = _repo.OrdersByUser( (int) HttpContext.Session.GetInt32("ID"));
                ViewBag.allorders = allorders;
                return View();
            }
            return RedirectToAction("Index", "Auth");
        }

        [HttpGet("orderdetails/{OrderId}")]
        public IActionResult OrderDetails(int OrderId)
        {
            // var orderdetail = _context.OrderDetails
            // .Include(o=> o.productOrdered)
            // .Include(p=>p.order)
            // .Where(x=>x.OrderId == OrderId)
            // .ToList();

            var orderdetail = _repo.OrderDetailsByOrderId(OrderId);

            float total = orderdetail.Sum(s=>s.SubTotal);
            
            ViewBag.total = total;
            ViewBag.orderdetail = orderdetail;
            ViewBag.OrderId = OrderId;
            return View("OrderDetails");
        }



        // [HttpGet("SearchPage")]
        // public IActionResult SearchPage ()
        // {
        //     if(HttpContext.Session.GetString("loggedin") == "true")
        //     {
        //         System.Console.WriteLine("###############################");
        //         var allproducts = _context.Products.ToList();
        //         ViewBag.allproducts = allproducts;
        //         float sum=0;
        //         foreach(var i in allproducts){
        //             sum+=i.Price;
        //         }
        //         ViewBag.sum = sum;

        //         ViewBag.searchproduct = TempData["search"];
        //         return View("SearchPage");
        //         List<Product> searchproduct = _context.Products.Where(u=>u.ProductName.Contains(search)).ToList();
        //         System.Console.WriteLine("$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$");
        //         System.Console.WriteLine(searchproduct);
        //         System.Console.WriteLine(Request.Query["search"]);
        //     // ViewBag.searchproduct = searchproduct;
        //         TempData["search"] = searchproduct;
        //     }
        //     return RedirectToAction("Index", "Auth");
        // }

        [HttpGet("search")]
        public IActionResult Search(string search)
        {
            if(HttpContext.Session.GetString("loggedin") == "true")
            {
                // System.Console.WriteLine("###############################");
                // var allproducts = _context.Products.ToList();
                var allproducts = _repo.AllProducts();
                ViewBag.allproducts = allproducts;
                float sum=0;
                foreach(var i in allproducts){
                    sum+=i.Price;
                }
                ViewBag.sum = sum;

                List<Product> searchproduct = _context.Products
                .Where(u=>u.ProductName.Contains(search) || u.ProductDescription.Contains(search))
                .ToList();
                System.Console.WriteLine("$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$");
                System.Console.WriteLine(searchproduct);
                System.Console.WriteLine(Request.Query["search"]);
                System.Console.WriteLine(search);
                ViewBag.searchproduct = searchproduct;
                foreach(var i in searchproduct){
                    System.Console.WriteLine(i.ProductName);
                    System.Console.WriteLine(i.ProductDescription);
                }
                return View();
            }
            return RedirectToAction("Index", "Auth");
        }
    }
}
