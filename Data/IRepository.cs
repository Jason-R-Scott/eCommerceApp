using System.Collections;
using System.Collections.Generic;
using ECommerceApp.Models;

namespace ECommerceApp.Data
{
    public interface IRepository
    {
        IEnumerable<Product> AllProducts();

        Product GetProductById(int id);

        IEnumerable<OrderDetail> OrderDetailsByOrderId(int id);

        IEnumerable<Order> OrdersByUser(int userId);
    }
}