using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShoppingCart.Models;
using System.Data.Entity;

namespace ShoppingCart.Controllers
{
    public class ShopController : Controller
    {

        // Default Shop page controller method.
        public ActionResult Index(string productCategory, string searchString)
        {
            // Open the database.
            using (ProductsEntities database = new ProductsEntities())
            {
                // Create the category list for the dropdown list and send it to the view.
                var catList = new List<string>();
                var catQry = from d in database.Categories
                             orderby d.Title
                             select d.Title;

                // Select only distinct categories, so there will only be one of each category in the list.
                catList.AddRange(catQry.Distinct());

                // Send the category list to the view.
                ViewBag.productCategory = new SelectList(catList);

                // Select all the products.
                var products = from p in database.Products
                               select p;

                // Select products that match the search string.
                if (!String.IsNullOrEmpty(searchString))
                {
                    products = products.Where(p => p.Title.Contains(searchString));
                }

                // Convert the selected category string to an int usable in the category match query.
                int intCat = 0;

                var catsToInt = from c in database.Categories
                                select c;

                // Loop through all the categories and set the integer eauivalent of the category name.
                foreach (var catToInt in catsToInt)
                {
                    if (productCategory == catToInt.Title)
                    {
                        intCat = catToInt.CategoryID;
                    }
                }


                // Select the matched category integer.
                if (intCat != 0)
                {
                    products = products.Where(x => x.CategoryID == intCat);
                }

                // Convert the query to an IEnumerable object for eager loading.
                var converted = products.Include("Category").ToList();

                return View(converted);
            }
        }


        // Controller method to use Ajax to update the quantity of a product in the cart without having to reload the page.
        [HttpPost]
        public string UpdateQTY()
        {
            using (ProductsEntities database = new ProductsEntities())
            {
                // Updates the QTY of product in cart from the "cart" session variable.
                // Occurs on page load.

                // Get the POSTED value of the product ID.
                var id = Convert.ToInt32(Request["qtyID"]);

                // Find the matching product.
                Product product = database.Products.Find(id);

                // Check if the Cart exists.
                if (Session["cart"] != null)
                {
                    // Check if the cart contains the requested product.
                    if (((Dictionary<Product, int>)Session["cart"]).ContainsKey(product))
                    {
                        // Return a string representation of the product to the view 
                        // so the quantity of the product in the cart can be set.
                        return ((Dictionary<Product, int>)Session["cart"])[product].ToString();
                    }
                }

                // If there was no product found in the cart (zero quantity), return string value of zero.
                return "0";
            }
        }
    }

}