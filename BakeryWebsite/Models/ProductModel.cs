using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BakeryWebsite.Models
{
    public class Order
    {
        public string ProductName { get; set; } //  Masterdata, Read from JSON
        public string ProductShortName { get; set; } //  Masterdata, Read from JSON
        public List<RateSlab> RateSlabs{ get;set; } //  Masterdata, Read from JSON
        public int OrderQuantity{  get; set;} // Input Quantity Of Order
        public int DeliveredQuantity { get; set; } // Quantity Of Delivery = OrderQuantity-RemainingQuantity
        public int RemainingQuantity { get; set; } // Remainning Quantity after assigining all packs
        public double ProductTotalPrice { get; set; } // Total Price for this Order 
        public List<RateSlabCombination> RateSlabCombinations { get; set; }
      
    }

    public class RateSlab
    {
        public int Index { get; set; }
        public int Quantity { get; set; } // Masterdata, Read from JSON
        public double Price { get; set; } //  Masterdata, Read from JSON
        public int Packs { get; set; } //Assigned Packs for this RateSlab
        public double TotalPrice { get; set; }// Total Price for this RateSlab
        public int MaxOccurance { get; set; }

        public bool DummySlab { get; set; } = false;

        public RateSlab( int index, int quantity, double price, int packs, int maxOccurance, bool dummySlab)
        {
            this.Index = index;
            this.Quantity = quantity;
            this.Price = price;
            this.Packs = packs;
            this.MaxOccurance = maxOccurance;
            this.DummySlab = dummySlab;
        }

        public RateSlab()
        {
        }
    }

    public class RateSlabCombination
    {
        public List<RateSlab> RateSlabs { get; set; }
        public int TotalPacks { get; set; }
    }

    public class ObservableProducts
    {
        public List<Order> Orders { get; set; }  
        public string OutputString { get; set; }
    }
}