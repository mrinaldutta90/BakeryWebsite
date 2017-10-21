using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel;
using BakeryWebsite.Models;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using System.IO;
using System.Configuration;

namespace BakeryWebsite.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// Returns the PlaceOrder View
        /// </summary>
        [HttpGet]
        public ActionResult PlaceOrder()
        {
            return View();
        }

        /// <summary>
        /// Calculates the breakdown of a particular Order
        /// </summary>
        /// <param name="form">FormCollection</param>      
        /// <returns>ActionResult</returns>
        [HttpPost]
        public ActionResult PlaceOrder(FormCollection form)
        {
            #region Fetching the List of Products from the static JSON file
            //In an ideal world this would be maintained in a relational database
            var products = new List<Order>();
            using (StreamReader r = new StreamReader(ConfigurationManager.AppSettings["LocalJSONPath"]))
            {
                string json = r.ReadToEnd();
                products = JsonConvert.DeserializeObject<List<Order>>(json);

            }

            #endregion

            #region Declarations
            var observableProducts = new ObservableProducts();
            observableProducts.Orders = products;
            observableProducts.OutputString = "";
            #endregion


            foreach (Order product in observableProducts.Orders)
            {

                #region View processing for the input product Quantities
                product.OrderQuantity = Convert.ToInt32(form[product.ProductName.ToString() + " " + "Quantity"].ToString());

                if (product.ProductName == "Vegemite Scroll")
                    ViewBag.vegScrollQuantity = product.OrderQuantity;
                else if (product.ProductName == "Blueberry Muffin")
                    ViewBag.blueberryMuffinQuantity = product.OrderQuantity;
                else if (product.ProductName == "Croissant")
                    ViewBag.croissantQuantity = product.OrderQuantity;
                #endregion

                #region Breaks each order into slabs and calculate the slab prices and Remaining Quantities

                //Assigning the total items for this particular product
                int itemsLeft = product.OrderQuantity; 
                //Orders the Pricelist based on Descending Quantities, so as to use the least packs
                product.RateSlabs = product.RateSlabs.OrderByDescending(i => i.Quantity).ToList();

                foreach (RateSlab rate in product.RateSlabs)
                {
                    rate.Packs = itemsLeft / rate.Quantity; //Gets the number of packs for a particular rate slab
                    itemsLeft = itemsLeft % rate.Quantity; //Gets the number of packs remaining 
                    rate.TotalPrice = rate.Packs * rate.Price; // Calculate Slab Total Price

                    if (itemsLeft == 0) // No More Items left to assign
                        break;
                }

                //Calculate the Remaining Quantities for a particular product order
                if (itemsLeft > 0)
                    product.RemainingQuantity = itemsLeft;
                else
                    product.RemainingQuantity = 0;

                product.ProductTotalPrice = product.RateSlabs.Sum(i => i.TotalPrice);
                product.DeliveredQuantity = product.OrderQuantity - product.RemainingQuantity;

                #endregion

                #region Processing the output string
                //String Processing to format the output

                observableProducts.OutputString += product.DeliveredQuantity.ToString() + " " + product.ProductShortName + " " + product.ProductTotalPrice.ToString() + "\n";
                foreach (RateSlab rateslab in product.RateSlabs)
                {
                    if(rateslab.Packs >0)                    
                    observableProducts.OutputString += rateslab.Packs.ToString() + " x " + rateslab.Quantity.ToString() + " $" + rateslab.Price.ToString() + "\n";
                    
                }

                if (product.RemainingQuantity > 0)
                    observableProducts.OutputString += "Remaining Quantity " + product.RemainingQuantity.ToString() + "\n\n";
                else
                    observableProducts.OutputString += "\n\n";
                #endregion
            }

            return View(observableProducts);

        }

       
    }
}