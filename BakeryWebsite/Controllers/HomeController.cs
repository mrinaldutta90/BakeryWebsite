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
                int totalItems = product.OrderQuantity; 
                //Orders the Pricelist based on Descending Quantities, so as to use the least packs
                product.RateSlabs = product.RateSlabs.OrderByDescending(i => i.Quantity).ToList();
                while (totalItems > 0)
                  {
                    int startIndex = 1;
                    foreach (RateSlab rate in product.RateSlabs)
                    {
                        rate.Index = startIndex;
                        if (rate.Quantity != 0)
                            rate.MaxOccurance = totalItems / rate.Quantity;// Maximum number of times this particular slab can be assigned for the total count of products
                        else
                        {
                            rate.MaxOccurance = 0;
                            rate.DummySlab = true;
                        }
                        startIndex++;
                    }

                    RateSlab rateslab1 = new RateSlab();
                    RateSlab rateslab2 = new RateSlab();
                    RateSlab rateslab3 = new RateSlab();

                    rateslab1 = product.RateSlabs.Find(a => a.Index == 1);
                    rateslab2 = product.RateSlabs.Find(a => a.Index == 2);
                    rateslab3 = product.RateSlabs.Find(a => a.Index == 3);
                    product.RateSlabCombinations = new List<RateSlabCombination>();

                    for (int i = 0; i <= rateslab1.MaxOccurance; i++)
                    {
                        for (int j = 0; j <= rateslab2.MaxOccurance; j++)
                        {
                            for (int k = 0; k <= rateslab3.MaxOccurance; k++)
                            {
                                if ((rateslab1.Quantity * i + rateslab2.Quantity * j + rateslab3.Quantity * k) == totalItems)
                                {
                                    rateslab1.Packs = i;
                                    rateslab2.Packs = j;
                                    rateslab3.Packs = k;

                                    RateSlab rateSlabForCombination1 = new RateSlab(rateslab1.Index, rateslab1.Quantity, rateslab1.Price, i, rateslab1.MaxOccurance, rateslab1.DummySlab);
                                    RateSlab rateSlabForCombination2 = new RateSlab(rateslab2.Index, rateslab2.Quantity, rateslab2.Price, j, rateslab2.MaxOccurance, rateslab2.DummySlab);
                                    RateSlab rateSlabForCombination3 = new RateSlab(rateslab3.Index, rateslab3.Quantity, rateslab3.Price, k, rateslab3.MaxOccurance, rateslab3.DummySlab);



                                    RateSlabCombination rateSlabComination = new RateSlabCombination();
                                    rateSlabComination.RateSlabs = new List<RateSlab>();
                                    rateSlabComination.RateSlabs.Add(rateSlabForCombination1);
                                    rateSlabComination.RateSlabs.Add(rateSlabForCombination2);

                                    if (!rateslab3.DummySlab)
                                        rateSlabComination.RateSlabs.Add(rateSlabForCombination3);
                                    rateSlabComination.TotalPacks = rateSlabComination.RateSlabs.Sum(l => l.Packs);
                                    product.RateSlabCombinations.Add(rateSlabComination);

                                }
                            }
                        }
                    }

                    product.RateSlabs.RemoveAll(l => l.DummySlab = true);

                    product.RateSlabs = product.RateSlabCombinations.OrderBy(l => l.TotalPacks).FirstOrDefault().RateSlabs;
                    int itemsCalculated = 0;
                    foreach (RateSlab slab in product.RateSlabs)
                    {
                        itemsCalculated += slab.Packs * slab.Quantity;
                        slab.TotalPrice = slab.Packs * slab.Price;
                    }


                    //Calculate the Remaining Quantities for a particular product order
                    if (itemsCalculated == totalItems)
                        break;
                    else totalItems--;

                }
                product.RemainingQuantity = product.OrderQuantity - totalItems;
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