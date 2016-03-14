using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShoppingCart.Models;
using System.Data.Entity;
using System.Diagnostics;
using System.Data.Entity.Validation;

namespace ShoppingCart.Controllers
{
    public class HomeController : Controller
    {

        // Login controller method.
        public ActionResult Login(bool checkout = false)
        {
            // Page header.
            ViewBag.Message = "Login";

            // If coming from the checkout, set the session variable checkout to true.
            if (checkout == true)
            {
                Session["checkout"] = "true";
            }

            return View();
        }

        // Login method that accests login POST data from the view.
        [HttpPost]
        [ActionName("Login")]
        public ActionResult LoginPost([Bind(Include = "Username, Password")]  User user)
        {
            // Open the database.
            using (ProductsEntities database = new ProductsEntities())
            {
                // Get user that matches the username and password details.
                User login = database.Users.FirstOrDefault(u => u.Username == user.Username &&
                                                                u.Password == user.Password);

                // If there is a user that matches.
                if (login != null)
                {
                    // Set the session details for username and password.
                    Session["Username"] = login.Username;
                    Session["UserId"] = login.UserId;

                    // If coming from the checkout page, redirect back to the checkout.
                    if (Session["checkout"] != null)
                    {
                        return RedirectToAction("../Checkout");
                    }

                    // If not coming from the checkout page, redirect to the shop.
                    else
                    {
                        return RedirectToAction("../Shop");
                    }
                }
            }

            return View(user);
        }

        // Logout controller method.
        public ActionResult Logout()
        {
            // Set the login session details to null.
            Session["Username"] = null;
            Session["UserId"] = null;

            // Redirect back to the shop.
            return RedirectToAction("../Shop");
        }


        // Register a user controller method.
        public ActionResult RegisterUser()
        {
            // Return the RegisterUser view.
            return View();
        }


        // Add a user to the database controller method.
        public ActionResult AddUser(string Username)
        {

            // Reset the session variables used in this method.
            Session["exists"] = "";
            Session["match"] = "";

            // Open the database.
            using (ProductsEntities database = new ProductsEntities())
            {
                // See if the requested username already exists.
                if (Request["username"] != null)
                {
                    // Convert the POST username to a string for use in Linq query.
                    var strItem = Request["username"].ToString();

                    // Find the requested username in the database.
                    var match = from user in database.Users
                                where user.Username == strItem
                                select user;

                    // Loop through the matched usernames to see if one does exist.
                    foreach (User u in match)
                    {
                        // Set the session variable for username already exists to true.
                        Session["exists"] = "true";

                        // Redirect back to the RegisterUser page.
                        return RedirectToAction("RegisterUser");
                    }
                }

                // If the username is available, continue.
                // Create a new user to add to the database.
                User newUser = new User();

                newUser.Username = Request["Username"];
                newUser.Password = Request["Password"];
                newUser.ConfirmPassword = Request["ConfirmPassword"];

                // Add the new user to the database and save the changes.
                database.Users.Add(newUser);
                database.SaveChanges();

                // Redirect to the Thank you for registering page.
                return RedirectToAction("ThankYou");

            }
        }


        // Controller method for thanks for registering view.
        public ActionResult ThankYou()
        {
            // Set the thank you message.
            ViewBag.Message = "Thank you for registering";

            return View();
        }


        // May not be required.
        public ActionResult Checkout()
        {
            ViewBag.Message = "Proceeding to checkout";
            return View();
        }
    }
}