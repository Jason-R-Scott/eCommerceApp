using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ECommerceApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApp.Data
{
    public class Repository : IRepository
    {
        private readonly DataContext _context;

        public Repository(DataContext context)
        {
            this._context = context;
        }

        public IEnumerable<Product> AllProducts()
        {
            var allproducts = _context.Products.ToList();
            return allproducts;
            // throw new System.NotImplementedException();
        }

        public Product GetProductById(int id)
        {
            var product = _context.Products.SingleOrDefault(p=>p.ProductId == id);
            return product;
        }

        public IEnumerable<OrderDetail> OrderDetailsByOrderId(int OrderId)
        {
            var orderdetail = _context.OrderDetails
            .Include(o=> o.productOrdered)
            .Include(p=>p.order)
            .Where(x=>x.OrderId == OrderId)
            .ToList();

            return orderdetail;
        }

        public IEnumerable<Order> OrdersByUser(int userId)
        {
            var allorders = _context.Orders
                .Include(o=>o.Creator)
                .Where(u=> u.UserID == userId) 
                .OrderByDescending(x=>x.OrderDate)
                .ToList();

            return allorders;
        }
        
    }
}