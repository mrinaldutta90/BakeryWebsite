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
using static System.Net.Mime.MediaTypeNames;

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
        /// HTTP Post for Placing orders
        /// </summary>
        /// <param name="form">FormCollection</param>      
        /// <returns>ActionResult</returns>
        [HttpPost]
        public ActionResult PlaceOrder(FormCollection form)
        {
            List<Order> products = FetchOrderFromJSON();

            #region Declarations
            var observableProducts = new ObservableProducts();
            observableProducts.Orders = products;
            observableProducts.OutputString = "";
            #endregion


            foreach (Order product in observableProducts.Orders)
            {

                #region View processing for the input product Quantities
                //If blank replacing by 0           

                product.OrderQuantity = Convert.ToInt32( (form[product.ProductName.ToString() + " " + "Quantity"] == "" ?  0.ToString() : form[product.ProductName.ToString() + " " + "Quantity"]).ToString() );

                if (product.ProductName == "Vegemite Scroll")
                    ViewBag.vegScrollQuantity = product.OrderQuantity;
                else if (product.ProductName == "Blueberry Muffin")
                    ViewBag.blueberryMuffinQuantity = product.OrderQuantity;
                else if (product.ProductName == "Croissant")
                    ViewBag.croissantQuantity = product.OrderQuantity;
                #endregion

                ProcessEachOrder(product);  

                #region Processing the output string
                //String Processing to format the output

                observableProducts.OutputString += product.DeliveredQuantity.ToString() + " " + product.ProductShortName + " $" + product.ProductTotalPrice.ToString() + "\n";
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

        /// <summary>
        /// Processes Each Type of order
        /// </summary      
        /// <param name="order">Order</param>      
        /// <returns>ActionResult</returns>
        public void ProcessEachOrder(Order order)
        {
            #region Breaks each order into slabs and calculate the slab prices and Remaining Quantities

            bool isAtleastOneCombinationFound = false;
            //Assigning the total items for this particular product
            int totalItems = order.OrderQuantity;
            //Orders the Pricelist based on Descending Quantities, so as to use the least packs
            order.RateSlabs = order.RateSlabs.OrderByDescending(i => i.Quantity).ToList();
            while (totalItems >= Convert.ToInt32(order.RateSlabs.FindAll(j => j.Quantity != 0).Last().Quantity))
            {
                int startIndex = 1;
                foreach (RateSlab rate in order.RateSlabs)
                {
                    rate.Index = startIndex;
                    //handling for the Dummy Slab introduced for Vegemite Scroll
                    if (rate.Quantity != 0)
                        // Maximum number of times this particular slab can be assigned for the total count of products
                        rate.MaxOccurance = totalItems / rate.Quantity;
                    else
                    {
                        rate.MaxOccurance = 0;
                        rate.DummySlab = true;
                    }
                    startIndex++;
                }

                // Instantiated the Slab classes for the three slabs required to find out the combinations 
                RateSlab rateSlab1 = new RateSlab();
                RateSlab rateSlab2 = new RateSlab();
                RateSlab rateSlab3 = new RateSlab();

                // Assign Indexes to the objects
                rateSlab1 = order.RateSlabs.Find(a => a.Index == 1);
                rateSlab2 = order.RateSlabs.Find(a => a.Index == 2);
                rateSlab3 = order.RateSlabs.Find(a => a.Index == 3);

                // Instantiated a list of Slab combination to be used within the loop to add combinations which match the total count
                order.RateSlabCombinations = new List<RateSlabCombination>();

                //Looping through three layers of Slabs, to find out the combinations for which the number of Sum(pack*Quantity) matches the totalItems
                // Checking for all combinations i=0 to Max, j=0 to Max, k=0 to Max. Total loop execution (i*1)*(j+1)*(k+1)
                for (int i = 0; i <= rateSlab1.MaxOccurance; i++)
                {
                    for (int j = 0; j <= rateSlab2.MaxOccurance; j++)
                    {
                        for (int k = 0; k <= rateSlab3.MaxOccurance; k++)
                        {
                            if ((rateSlab1.Quantity * i + rateSlab2.Quantity * j + rateSlab3.Quantity * k) == totalItems)
                            {
                                // Hurray! We have found a combination to match the totalItems                               

                                //Constructing three new rateSlabs to be used in the rateSlabComination object
                                RateSlab rateSlabForCombination1 = new RateSlab(rateSlab1.Index, rateSlab1.Quantity, rateSlab1.Price, i, rateSlab1.MaxOccurance, rateSlab1.DummySlab);
                                RateSlab rateSlabForCombination2 = new RateSlab(rateSlab2.Index, rateSlab2.Quantity, rateSlab2.Price, j, rateSlab2.MaxOccurance, rateSlab2.DummySlab);
                                RateSlab rateSlabForCombination3 = new RateSlab(rateSlab3.Index, rateSlab3.Quantity, rateSlab3.Price, k, rateSlab3.MaxOccurance, rateSlab3.DummySlab);


                                // Instantiated the RateSlabCombination object to add the a Slab Combination matching the criteria
                                RateSlabCombination rateSlabComination = new RateSlabCombination();
                                rateSlabComination.RateSlabs = new List<RateSlab>();
                                rateSlabComination.RateSlabs.Add(rateSlabForCombination1);
                                rateSlabComination.RateSlabs.Add(rateSlabForCombination2);

                                //Do not add for a Dummy slab. It's only there to make life easier!
                                if (!rateSlab3.DummySlab)
                                    rateSlabComination.RateSlabs.Add(rateSlabForCombination3);
                                //Calculate the Total packs for the Slab combination, to be used later to get the one with the lowest TotalPacks
                                rateSlabComination.TotalPacks = rateSlabComination.RateSlabs.Sum(l => l.Packs);
                                order.RateSlabCombinations.Add(rateSlabComination);

                            }
                        }
                    }
                }

                

                int itemsCalculated = 0;
                if (order.RateSlabCombinations.Count() != 0) // If there is any rateSlabsCominations found then do this
                {
                    //If there is more than one rate slab combination, Fetch the Rate slab combination which has the least Total packs 
                    order.RateSlabs = order.RateSlabCombinations.OrderBy(l => l.TotalPacks).FirstOrDefault().RateSlabs;

                    // Calculate the Total Price for a product TotalPrice = Packs* Price of Pack
                    //Also Calculate the number of items accounted for
                    foreach (RateSlab slab in order.RateSlabs)
                    {
                        itemsCalculated += slab.Packs * slab.Quantity;
                        slab.TotalPrice = slab.Packs * slab.Price;
                    }
                    isAtleastOneCombinationFound = true;
                    break;// break if akdtleast one combination found
                }

                else totalItems--; //If no combination found for the amount entered, decrement the total items by one and try again

            }
            //Set the remaining Quantity as a difference of OrderedQuantity and the closest Number of Items for which a combination was found
            if (isAtleastOneCombinationFound) //If No Slab Combinations found that means the original Quantity is remaining
                order.RemainingQuantity = order.OrderQuantity - totalItems;
            else
                order.RemainingQuantity = order.OrderQuantity;
            //Calculate the sum of a particular product
            order.ProductTotalPrice = Math.Round( order.RateSlabs.Sum(i => i.TotalPrice),2);
            
            order.DeliveredQuantity = totalItems;           

            #endregion

        }

        public List<Order> FetchOrderFromJSON()
        {
            #region Fetching the List of Products from the static JSON file
            //In an ideal world this would be maintained in a relational database
            var products = new List<Order>();
            using (StreamReader r = new StreamReader(AppDomain.CurrentDomain.BaseDirectory +"\\SourceJSON.json" ))
            {
                string json = r.ReadToEnd();
                products = JsonConvert.DeserializeObject<List<Order>>(json);

            }

            return products;

            #endregion
        }
    }
}