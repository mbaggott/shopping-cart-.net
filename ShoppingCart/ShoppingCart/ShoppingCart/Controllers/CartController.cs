using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShoppingCart.Models;
using PagedList;

namespace ShoppingCart.Controllers
{
    public class CartController : Controller
    {

        // GET: Index
        public ActionResult Index(int? page)
        {
            using (ProductsEntities database = new ProductsEntities())
            {
                // Set PagedList size.
                int pageSize = 4;
                int pageIndex = 1;

                // Default to Page 1 if no value is provided.
                pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;

                // Check whether the Shopping Cart is empty or not.
                if (Session["cart"] != null && ((Dictionary<Product, int>)Session["cart"]).Count() != 0)
                {
                    // Retrieve current contents of the Cart and save to the PagedList container.
                    IPagedList<Product> cartList = null;
                    cartList = ((Dictionary<Product, int>)Session["cart"]).Keys.ToPagedList(pageIndex, pageSize);
                    
                    // Show Shopping Cart.
                    return View(cartList);
                }
                else
                {
                    // Show Shopping Cart Empty message.
                    return View("Empty");
                }
            }
        }


        // GET: Reset
        public ActionResult Reset()
        { 
            // Reset the Shopping Cart. 
            ((Dictionary<Product, int>)Session["cart"]).Clear(); 

            // Show Shopping Cart Empty message.
            return View("Empty");
        }


        [HttpPost]
        public string AddToCart()
        {
            using (ProductsEntities database = new ProductsEntities())
            {
                // Get ID of product from Ajax POST.
                var id = Convert.ToInt32(Request["id"]);

                // Plus or minus button will aways be a 1, set quantity to reflect this.
                var quantity = 1;

                // Find the product in the database and assign it to the product variable.
                Product product = database.Products.Find(id);

                // Check if Cart exists.
                if (Session["cart"] == null)
                {
                    // Cart does not exist - Instantiate the Cart.
                    Session["cart"] = new Dictionary<Product, int>();
                }

                // Check whether Product is already in the Cart.
                if (((Dictionary<Product, int>)Session["cart"]).ContainsKey(product))
                {
                    // Product is already in Cart - Increase the Quantity.
                    ((Dictionary<Product, int>)Session["cart"])[product] += quantity;
                }
                else
                {
                    // Add Product to the Cart.
                    ((Dictionary<Product, int>)Session["cart"]).Add(product, quantity);
                }

                return ((Dictionary<Product, int>)Session["cart"])[product].ToString();
            }
        }


        [HttpPost]
        public string RemoveFromCart()
        {
            using (ProductsEntities database = new ProductsEntities())
            {
                // Get ID of product from ajax POST
                var id = Convert.ToInt32(Request["id"]);

                // Plus or minus button will aways be a 1, set quantity to reflect this
                var quantity = 1;

                // Find the product in the database and assign it to the product variable
                Product product = database.Products.Find(id);

                // Check if Cart exists.
                if (Session["cart"] != null)
                {
                    // Check Product is in the Cart.
                    if (((Dictionary<Product, int>)Session["cart"]).ContainsKey(product))
                    {
                        // Product is in Cart - Deduct Quantity.
                        ((Dictionary<Product, int>)Session["cart"])[product] -= quantity;

                        // If Quantity of Product is 0, Remove from Cart completely.
                        if (((Dictionary<Product, int>)Session["cart"])[product] == 0)
                        {
                            ((Dictionary<Product, int>)Session["cart"]).Remove(product);
                        }
                    }

                    // Check if Cart is now Empty.
                    if (((Dictionary<Product, int>)Session["cart"]).Count == 0)
                    {
                        // Cart is now empty after Product removal - Reset the cart.
                        Session["cart"] = null;
                    }
                }

                // If Cart is now Empty or Product is no longer in the Cart, return 0.
                if (Session["cart"] == null || 
                    ((Dictionary<Product, int>)Session["cart"]).Count == 0 ||
                    !((Dictionary<Product, int>)Session["cart"]).ContainsKey(product))
                {
                    return "0";
                }
                else
                {
                    return ((Dictionary<Product, int>)Session["cart"])[product].ToString();
                }
            }
        }


        // Controller method to use Ajax to update the quantity of a product in the cart without having to reload the page.
        [HttpPost]
        public JsonResult UpdateQTY()
        {
            using (ProductsEntities database = new ProductsEntities())
            {
                // Updates the QTY of product in cart from the "cart" session variable.
                // Occurs on page load.
                var quantity = 0;
                decimal subTotal = 0;
                decimal totalCost = 0;

                // Get the POSTED value of the product ID.
                var id = Convert.ToInt32(Request["qtyID"]);

                // Find the matching product.
                Product product = database.Products.Find(id);

                // If Cart is now Empty or Product is no longer in the Cart, return 0.
                if (Session["cart"] == null ||
                    ((Dictionary<Product, int>)Session["cart"]).Count == 0)
                {
                    quantity = 0;
                    subTotal = 0;
                    totalCost = 0;
                }
                else if (product != null)
                {
                    // Check if Product is currently in Cart.
                    if (((Dictionary<Product, int>)Session["cart"]).ContainsKey(product))
                    {
                        // Product is in Cart.
                        quantity = ((Dictionary<Product, int>)Session["cart"])[product];
                        subTotal = (product.Price) * (quantity);
                        totalCost = TotalCost();
                    }
                    else
                    {
                        // Product has been removed from Cart - Quantity and SubTotal are 0.
                        quantity = 0;
                        subTotal = 0;
                    }

                    // Calculate Total Cost of the Cart.
                    totalCost = TotalCost();
                }

                return Json(new { prodid = id.ToString(), quantity = quantity.ToString(), subtotal = subTotal.ToString(), totalcost = totalCost.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }


        // Calculate Total Cost of Cart contents.
        public decimal TotalCost()
        {
            decimal totalCost = 0;

            if (Session["cart"] != null)
            {
                // Loop through the Cart and add costs of each item.
                foreach (KeyValuePair<ShoppingCart.Models.Product, int> item in ((Dictionary<ShoppingCart.Models.Product, int>)Session["cart"]))
                {
                    totalCost += (item.Key.Price * item.Value);
                }
            }
            
            return totalCost;
        }
    }
}