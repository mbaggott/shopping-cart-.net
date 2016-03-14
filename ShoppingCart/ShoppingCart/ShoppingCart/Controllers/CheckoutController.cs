using ShoppingCart.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Diagnostics;

namespace ShoppingCart.Controllers
{
    public class CheckoutController : Controller
    {
        private ProductsEntities db = new ProductsEntities();

        // GET: Index
        public ActionResult Index()
        {
            if (Session["Username"] == null)
            {
                // User is not logged in - Redirect to Login page.
                return RedirectToAction("../Home/Login", new { checkout = true });
            }
            else if (Session["Username"] != null)
            {
                // User is logged in.
                using (ProductsEntities database = new ProductsEntities())
                {
                    // Find the member's details in the registration (Customers database).
                    var strItem = Convert.ToInt32(Session["UserId"]);
                    var match = from customer in database.Customers
                                where customer.UserId.Equals(strItem)
                                select customer;

                    // If the customer is in the 'Customers' database (Registered for postage).
                    foreach (Customer c in match)
                    {
                        if (c.UserId == strItem)
                        {
                            if (Session["cart"] != null && ((Dictionary<Product, int>)Session["cart"]).Count() > 0)
                            {
                                return View("Index");
                            }
                            else
                            {
                                return View("../Cart/Empty");
                            }
                        }
                    }

                    // Redirect to Registration page if a valid login is not found.
                    return View("RegisterCustomer");
                }
            }
            else if (Session["cart"] == null || ((Dictionary<Product, int>)Session["cart"]).Count() == 0)
            {
                // Shopping Cart is empty - Redirect to Cart Empty message.
                return RedirectToAction("../Cart");
            }

            return View();
        }

        // GET: Checkout
        public ActionResult Checkout()
        {
            // Open and automatically dispose of database.
            using (ProductsEntities database = new ProductsEntities())
            {

                // Convert session object to int for use with database.
                var convertInt = Convert.ToInt32(Session["UserId"]);

                // Get the cart dictionary from the session variable.
                Dictionary<Product, int> cart = (Dictionary<Product, int>)Session["cart"];

                // If no order for the session exists.
                if (Session["OrderId"] == null)
                {

                    // Get customers that match the session UserId.
                    var cust = from c in database.Customers
                               where c.UserId == convertInt
                               select c;

                    // Create a new Order for adding to the database.
                    Order order = new Order();

                    // Get the customer found in the above linq query.
                    Customer customer = cust.FirstOrDefault();

                    // Assign the order details.
                    order.CustId = customer.CustId;
                    order.OrderDate = DateTime.Now;
                    order.OrderStatus = "OPEN";

                    // Add the order to the database and save.
                    database.Orders.Add(order);
                    database.SaveChanges();



                    // Loop through the cart.
                    foreach (KeyValuePair<Product, int> entry in cart)
                    {
                        // Order the products, recording OrderId, ProductId, quantity and current price.
                        Order_Products ordProd = new Order_Products();
                        ordProd.OrderID = order.OrderId;
                        ordProd.ProductID = entry.Key.ProductID;
                        ordProd.Quantity = entry.Value;
                        ordProd.Price = entry.Key.Price;

                        // Add the product orders to the database and save the changes.
                        database.Order_Products.Add(ordProd);
                        database.SaveChanges();
                    }

                    // Set the session variable OrderId to indicate the order has been created and products added.
                    Session["OrderId"] = order.OrderId;

                }

                // If the order already exists.
                else
                {
                    // Convert the session order id to an int for use with the database.
                    var convertIntOrder = Convert.ToInt32(Session["OrderId"]);

                    // Find all the product orders for the session Order ID.
                    var orderProd = from o in database.Order_Products
                                    where o.OrderID == convertIntOrder
                                    select o;

                    // Remove the product orders from the database.
                    foreach (var op in orderProd)
                    {
                        database.Order_Products.Remove(op);
                    }


                    // Re-add the product orders from the cart to the database to reflect any new changes.
                    // Loop through the cart.
                    foreach (KeyValuePair<Product, int> entry in cart)
                    {
                        // Order the products, recording OrderId, ProductId, quantity and current price.
                        Order_Products ordProd = new Order_Products();
                        ordProd.OrderID = convertIntOrder;
                        ordProd.ProductID = entry.Key.ProductID;
                        ordProd.Quantity = entry.Value;
                        ordProd.Price = entry.Key.Price;

                        // Add the product orders to the database.
                        database.Order_Products.Add(ordProd);
                    }

                    // Save the database changes.
                    database.SaveChanges();
                }

                return View();
            }
        }


        // Controller method for customer registration.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegisterCustomer()
        {
            // Open the database.
            using (ProductsEntities database = new ProductsEntities())
            {
                // If the current state of the view model is valid.
                if (ModelState.IsValid)
                {
                    // Create a new customer and assign the details from the POST data.
                    Customer customer = new Customer();

                    customer.FirstName = Request["firstname"];
                    customer.LastName = Request["lastname"];
                    customer.Email = Request["email"];
                    customer.Address = Request["address"];
                    customer.Suburb = Request["suburb"];
                    customer.State = Request["state"];
                    customer.Postcode = Convert.ToInt32(Request["postcode"]);
                    customer.UserId = Convert.ToInt32(Session["UserId"]);

                    // Add the new customer details to the database and save the changes.
                    db.Customers.Add(customer);
                    db.SaveChanges();

                    // If the cart exists and has products in it return to the cart view.
                    if (Session["cart"] != null && ((Dictionary<Product, int>)Session["cart"]).Count() > 0)
                    {
                        return View("Index");
                    }

                    // Otherwise return to the cart is empty view.
                    else
                    {
                        return View("../Cart/Empty");
                    }
                }

                // If the view model state is not valid, return to the registration page.
                return View("RegisterCustomer");
            }
        }


        // Controller method for confirmation of the purchase.
        public ActionResult Summary()
        {
            // Open the database.
            using (ProductsEntities database = new ProductsEntities())
            {
                // Retrieve UserID and OrderID.
                var intUser = Convert.ToInt32(Session["UserId"]);
                var intOrd = Convert.ToInt32(Session["OrderId"]);

                // Retrieve Customer details from database.
                var cust = from c in database.Customers
                           where c.UserId == intUser
                           select c;

                // Retrieve Order details.
                var ord = from o in database.Orders
                          where o.OrderId == intOrd
                          select o;

                // Update Order status.
                Order order = ord.FirstOrDefault();
                try
                {
                    order.OrderStatus = "COMPLETE";
                    database.SaveChanges();
                }
                catch (System.NullReferenceException)
                {
                    return RedirectToAction("../Shop/Index");
                }

                // Retrieve list of Order Products.
                var prodOrd = from po in database.Order_Products
                              where po.OrderID == intOrd
                              select po;

                var modelCust = cust.Include("User").ToList();
                var modelOrd = ord.Include("Customer").ToList();
                var modelProdOrd = prodOrd.Include("Order").ToList();

                // Reset Session Variables for the Cart and Order.
                Session["OrderId"] = null;
                Session["cart"] = null;

                foreach (var ct in cust)
                {
                    Debug.WriteLine(ct.Address);
                }

                ViewBag.ModelCust = modelCust;
                ViewBag.ModelOrd = modelOrd;

                // Order Completed - Display Order Summary.
                return View(modelProdOrd);
            }
        }
    }
}