using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerceApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using ECommerceApp.Helpers;
using ECommerceApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Stripe;
using System.Net.Mail;
using System.Net;

namespace ECommerceApp.Controllers
{
    

    public class CartController : Controller
    {

        private readonly DataContext _context;

        public CartController(DataContext context)
        {
            this._context = context;
        }


        [Route("cart")]
        public IActionResult Index()
        {
            int? userId = HttpContext.Session.GetInt32("ID");
            // var cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
            var cart = _context.Items.Include(p=> p.Product).Include(u=> u.User)
            .Where(u=>u.UserID == userId).ToList();
            System.Console.WriteLine("$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$");
            // foreach(var item in cart)
            // {
            //     System.Console.WriteLine(item.Product.ProductId);
            //     System.Console.WriteLine(item.Product.ProductName);
            //     System.Console.WriteLine(item.Product.Price);
            //     System.Console.WriteLine(item);
            //     // System.Console.WriteLine(item.User.FirstName);


            // }
            
            System.Console.WriteLine("$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$");
            ViewBag.cart = cart;
            ViewBag.total = (float)cart.Sum(item => item.Product.Price * item.Quantity);
            float before = cart.Sum(item => item.Product.Price * item.Quantity) * 100;
            int total = (int) before;
            HttpContext.Session.SetInt32("total", total);
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

        [HttpGet("addtocart/{id}")]
        public IActionResult Buy(int id)
        {

            int? userId = HttpContext.Session.GetInt32("ID");
           
                    var thisproduct = _context.Products.SingleOrDefault(p=>p.ProductId == id);
                    if(thisproduct.InitialQuantity > 0)
                    {
                        Item checkItem = _context.Items
                        .Include(i=>i.Product)
                        .Include(p=>p.User)
                        .SingleOrDefault(u=>u.UserID == userId && u.ProductId == id);
                        if(checkItem != null && checkItem.ProductId == id)
                        {
                        // if true - update quantity
                            checkItem.Quantity += 1;
                            _context.SaveChanges();
                            return RedirectToAction("Index");
                        }
                // check to see if product id already exists in cart
                    // if false create new new item
                        else
                        {
                            Item cart = new Item();
                            // cart.Add(new Item { Product = productModel.find(id), Quantity = 1 });
                            cart.ProductId = id;
                            cart.Product = _context.Products.SingleOrDefault(p=>p.ProductId == id);
                            cart.Quantity = 1;
                            cart.User = _context.Users.SingleOrDefault(u=>u.UserID == userId);
                            cart.UserID = (int)userId;

                            _context.Add(cart);
                            _context.SaveChanges();

                        

                            return RedirectToAction("Index");
                        }

                                   

                    }
                        TempData["QtyError"] = "This item is out of stock. Please check back soon."; 
                        return RedirectToAction("AllProducts", "Home");
        }

        [Route("remove/{id}")]
        public IActionResult Remove(int id)
        {
            // List<Item> cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
            var removedItem = _context.Items.SingleOrDefault(i=>i.ItemId == id);
            _context.Remove(removedItem);
            _context.SaveChanges();
            // int index = isExist(id);
            //removedItem.RemoveAt(index);
            //SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            return RedirectToAction("Index");
        }

        private int isExist(int id)
        {
            List<Item> cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
            for (int i = 0; i < cart.Count; i++)
            {
                if (cart[i].Product.ProductId.Equals(id))
                {
                    return i;
                }
            }
            return -1;
        }

        [HttpGet("payments")]
        public IActionResult PaymentPage()
        {
            List<Item> allitems = _context.Items
           .Where(i=>i.UserID == HttpContext.Session.GetInt32("ID"))
           .Include(p=> p.Product)
           .ToList();
            ViewBag.allitems = allitems;
            ViewBag.amount = HttpContext.Session.GetInt32("total");

            int? userId = HttpContext.Session.GetInt32("ID");
            var cart = _context.Items.Include(p=> p.Product).Include(u=> u.User)
            .Where(u=>u.UserID == userId).ToList();
            ViewBag.total = (float)cart.Sum(item => item.Product.Price * item.Quantity);
            return View();
        }

        [HttpGet("success")]
        public IActionResult Success()

        {
            return View("Charge");
        }

        [HttpPost("charge")]
        public IActionResult Charge(string stripeEmail, string stripeToken)
        {   

            var customers = new CustomerService();
            var charges = new ChargeService();

            var customer = customers.Create(new CustomerCreateOptions {
                Email = stripeEmail,
                SourceToken = stripeToken
            });
           
            StripeConfiguration.SetApiKey("sk_test_cXmDQf2AysFtVCS1M1sfcBRC");


            var charge = charges.Create(new ChargeCreateOptions {
                Amount = HttpContext.Session.GetInt32("total"),
                Description = "This is a test Charge",
                Currency = "usd",
                CustomerId = customer.Id
            });

            //TempData["ChargeAmount"] = (charge.Amount/100).ToString("N2");
            TempData["ChargeAmount"] = (charge.Amount/(double)100).ToString("N2");

            // System.Console.WriteLine("$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$");
            // System.Console.WriteLine(charge.Currency);
            // System.Console.WriteLine(charge.Description);
            // System.Console.WriteLine(charge.Amount);
            // System.Console.WriteLine("$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$");

           List<Item> allitems = _context.Items
           .Where(i=>i.UserID == HttpContext.Session.GetInt32("ID"))
           .Include(p=> p.Product)
           .ToList();
           foreach (var item in allitems)
           {
               System.Console.WriteLine("$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$");
               System.Console.WriteLine(item.Product.ProductName); 
               System.Console.WriteLine(item.Product.InitialQuantity);
               System.Console.WriteLine(item.Product.Price);

                    
               System.Console.WriteLine("$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$");
           }
           ECommerceApp.Models.Order newOrder = new ECommerceApp.Models.Order
           {
               OrderDate = DateTime.Now,
               UpdatedAt = DateTime.Now,
               UserID = (int)HttpContext.Session.GetInt32("ID"),
               OrderTotal = charge.Amount/ (float) 100, 
           };

           _context.Add(newOrder);
           _context.SaveChanges();


           
            foreach (var item in allitems)
            {
                ECommerceApp.Models.OrderDetail orderDetail = new OrderDetail
                {
                    OrderId = newOrder.OrderId,
                    ProductId = item.ProductId,
                    Price = item.Product.Price,
                    Quantity = item.Quantity,
                    ProductName = item.Product.ProductName,
                    SubTotal = item.Product.Price * item.Quantity,
                };

                _context.Add(orderDetail);

                var currentProduct =_context.Products.SingleOrDefault(p=>p.ProductId == item.ProductId);
                currentProduct.InitialQuantity -= item.Quantity;

                
                
                _context.SaveChanges();

            }

            foreach (var item in allitems)
            {
                _context.Items.Remove(item);
                _context.SaveChanges();
            }
        
            // The code below is for sending email to the customer who checked out successfully.
            var senderEmail = new MailAddress("demoemail536@gmail.com", "Tech Bazaar");
            var receiverEmail = new MailAddress(stripeEmail, HttpContext.Session.GetString("username"));

            var password = "test1234%";
            var subject = "Order Confirmation";
            var body = "Thanks for ordering with us. Your products should arrive soon. We hope you give us a chance to serve you again.";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(senderEmail.Address, password),
            };

            using(var mess = new MailMessage(senderEmail, receiverEmail)
                {
                    Subject = subject,
                    Body = body,   
                }
                )
                {
                    smtp.Send(mess);
                }


            return RedirectToAction("Success");
        }

    }   
}