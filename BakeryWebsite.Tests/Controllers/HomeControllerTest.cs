using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BakeryWebsite;
using BakeryWebsite.Controllers;
using BakeryWebsite.Models;

namespace BakeryWebsite.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Test_VegemiteScroll_2()
        {
            // Arrange
            HomeController controller = new HomeController();

            var orders = new List<Order>();
            orders = controller.FetchOrderFromJSON();
            var rateSlabs = new List<RateSlab>();
            rateSlabs = orders.Find(l => l.ProductName == "Vegemite Scroll").RateSlabs;
            Order order = new Order("Vegemite Scroll", rateSlabs, 2);
            controller.ProcessEachOrder(order);

            Assert.AreEqual(order.RemainingQuantity, 2);
            Assert.AreEqual(order.ProductTotalPrice, 0);            
        }

        [TestMethod]
        public void Test_VegemiteScroll_3()
        {
            // Arrange
            HomeController controller = new HomeController();

            var orders = new List<Order>();
            orders = controller.FetchOrderFromJSON();
            var rateSlabs = new List<RateSlab>();
            rateSlabs = orders.Find(l => l.ProductName == "Vegemite Scroll").RateSlabs;
            Order order = new Order("Vegemite Scroll", rateSlabs, 3);
            controller.ProcessEachOrder(order);

            Assert.AreEqual(order.RemainingQuantity, 0);
            Assert.AreEqual(order.ProductTotalPrice, 6.99);

        }


        [TestMethod]
        public void Test_VegemiteScroll_4()
        {
            // Arrange
            HomeController controller = new HomeController();

            var orders = new List<Order>();
            orders = controller.FetchOrderFromJSON();
            var rateSlabs = new List<RateSlab>();
            rateSlabs = orders.Find(l => l.ProductName == "Vegemite Scroll").RateSlabs;
            Order order = new Order("Vegemite Scroll", rateSlabs, 4);
            controller.ProcessEachOrder(order);

            Assert.AreEqual(order.RemainingQuantity, 1);
            Assert.AreEqual(order.ProductTotalPrice, 6.99);

        }

        [TestMethod]
        public void Test_VegemiteScroll_5()
        {
            // Arrange
            HomeController controller = new HomeController();

            var orders = new List<Order>();
            orders = controller.FetchOrderFromJSON();
            var rateSlabs = new List<RateSlab>();
            rateSlabs = orders.Find(l => l.ProductName == "Vegemite Scroll").RateSlabs;
            Order order = new Order("Vegemite Scroll", rateSlabs, 5);
            controller.ProcessEachOrder(order);

            Assert.AreEqual(order.RemainingQuantity, 0);
            Assert.AreEqual(order.ProductTotalPrice, 8.99);

        }

        [TestMethod]
        public void Test_VegemiteScroll_6()
        {
            // Arrange
            HomeController controller = new HomeController();

            var orders = new List<Order>();
            orders = controller.FetchOrderFromJSON();
            var rateSlabs = new List<RateSlab>();
            rateSlabs = orders.Find(l => l.ProductName == "Vegemite Scroll").RateSlabs;
            Order order = new Order("Vegemite Scroll", rateSlabs, 6);
            controller.ProcessEachOrder(order);

            Assert.AreEqual(order.RemainingQuantity, 0);
            Assert.AreEqual(order.ProductTotalPrice, 13.98);

        }

        [TestMethod]
        public void Test_VegemiteScroll_10()
        {
            // Arrange
            HomeController controller = new HomeController();

            var orders = new List<Order>();
            orders = controller.FetchOrderFromJSON();
            var rateSlabs = new List<RateSlab>();
            rateSlabs = orders.Find(l => l.ProductName == "Vegemite Scroll").RateSlabs;
            Order order = new Order("Vegemite Scroll", rateSlabs, 10);
            controller.ProcessEachOrder(order);

            Assert.AreEqual(order.RemainingQuantity, 0);
            Assert.AreEqual(order.ProductTotalPrice, 17.98);

        }


        [TestMethod]
        public void Test_BlueBerryMuffin_4()
        {
            // Arrange
            HomeController controller = new HomeController();

            var orders = new List<Order>();
            orders = controller.FetchOrderFromJSON();
            var rateSlabs = new List<RateSlab>();
            rateSlabs = orders.Find(l => l.ProductName == "Blueberry Muffin").RateSlabs;
            Order order = new Order("Blueberry Muffin", rateSlabs, 4);
            controller.ProcessEachOrder(order);

            Assert.AreEqual(order.RemainingQuantity, 0);
            Assert.AreEqual(order.ProductTotalPrice, 19.9);

        }

        [TestMethod]
        public void Test_BlueBerryMuffin_5()
        {
            // Arrange
            HomeController controller = new HomeController();

            var orders = new List<Order>();
            orders = controller.FetchOrderFromJSON();
            var rateSlabs = new List<RateSlab>();
            rateSlabs = orders.Find(l => l.ProductName == "Blueberry Muffin").RateSlabs;
            Order order = new Order("Blueberry Muffin", rateSlabs, 5);
            controller.ProcessEachOrder(order);

            Assert.AreEqual(order.RemainingQuantity, 0);
            Assert.AreEqual(order.ProductTotalPrice, 16.95);

        }

        [TestMethod]
        public void Test_BlueBerryMuffin_6()
        {
            // Arrange
            HomeController controller = new HomeController();

            var orders = new List<Order>();
            orders = controller.FetchOrderFromJSON();
            var rateSlabs = new List<RateSlab>();
            rateSlabs = orders.Find(l => l.ProductName == "Blueberry Muffin").RateSlabs;
            Order order = new Order("Blueberry Muffin", rateSlabs, 6);
            controller.ProcessEachOrder(order);

            Assert.AreEqual(order.RemainingQuantity, 0);
            Assert.AreEqual(order.ProductTotalPrice, 29.85);

        }

        [TestMethod]
        public void Test_BlueBerryMuffin_10()
        {
            // Arrange
            HomeController controller = new HomeController();

            var orders = new List<Order>();
            orders = controller.FetchOrderFromJSON();
            var rateSlabs = new List<RateSlab>();
            rateSlabs = orders.Find(l => l.ProductName == "Blueberry Muffin").RateSlabs;
            Order order = new Order("Blueberry Muffin", rateSlabs, 10);
            controller.ProcessEachOrder(order);

            Assert.AreEqual(order.RemainingQuantity, 0);
            Assert.AreEqual(33.9,order.ProductTotalPrice);

        }

        [TestMethod]
        public void Test_BlueBerryMuffin_14()
        {
            // Arrange
            HomeController controller = new HomeController();

            var orders = new List<Order>();
            orders = controller.FetchOrderFromJSON();
            var rateSlabs = new List<RateSlab>();
            rateSlabs = orders.Find(l => l.ProductName == "Blueberry Muffin").RateSlabs;
            Order order = new Order("Blueberry Muffin", rateSlabs, 14);
            controller.ProcessEachOrder(order);

            Assert.AreEqual(order.RemainingQuantity, 0);
            Assert.AreEqual(order.ProductTotalPrice, 53.8);

        }

        public void Test_BlueBerryMuffin_15()
        {
            // Arrange
            HomeController controller = new HomeController();

            var orders = new List<Order>();
            orders = controller.FetchOrderFromJSON();
            var rateSlabs = new List<RateSlab>();
            rateSlabs = orders.Find(l => l.ProductName == "Blueberry Muffin").RateSlabs;
            Order order = new Order("Blueberry Muffin", rateSlabs, 15);
            controller.ProcessEachOrder(order);

            Assert.AreEqual(order.RemainingQuantity, 0);
            Assert.AreEqual(order.ProductTotalPrice, 32.85);

        }


        [TestMethod]
        public void Test_Croissant_1()
        {
            // Arrange
            HomeController controller = new HomeController();

            var orders = new List<Order>();
            orders = controller.FetchOrderFromJSON();
            var rateSlabs = new List<RateSlab>();
            rateSlabs = orders.Find(l => l.ProductName == "Croissant").RateSlabs;
            Order order = new Order("Croissant", rateSlabs, 1);
            controller.ProcessEachOrder(order);

            Assert.AreEqual(order.RemainingQuantity, 1);
            Assert.AreEqual(order.ProductTotalPrice, 0);
        }


        [TestMethod]
        public void Test_Croissant_2()
        {
            // Arrange
            HomeController controller = new HomeController();

            var orders = new List<Order>();
            orders = controller.FetchOrderFromJSON();
            var rateSlabs = new List<RateSlab>();
            rateSlabs = orders.Find(l => l.ProductName == "Croissant").RateSlabs;
            Order order = new Order("Croissant", rateSlabs, 2);
            controller.ProcessEachOrder(order);

            Assert.AreEqual(order.RemainingQuantity, 2);
            Assert.AreEqual(order.ProductTotalPrice, 0);
        }

        [TestMethod]
        public void Test_Croissant_3()
        {
            // Arrange
            HomeController controller = new HomeController();

            var orders = new List<Order>();
            orders = controller.FetchOrderFromJSON();
            var rateSlabs = new List<RateSlab>();
            rateSlabs = orders.Find(l => l.ProductName == "Croissant").RateSlabs;
            Order order = new Order("Croissant", rateSlabs, 3);
            controller.ProcessEachOrder(order);

            Assert.AreEqual(order.RemainingQuantity, 0);
            Assert.AreEqual(order.ProductTotalPrice, 5.95);

        }


        [TestMethod]
        public void Test_Croissant_4()
        {
            // Arrange
            HomeController controller = new HomeController();

            var orders = new List<Order>();
            orders = controller.FetchOrderFromJSON();
            var rateSlabs = new List<RateSlab>();
            rateSlabs = orders.Find(l => l.ProductName == "Croissant").RateSlabs;
            Order order = new Order("Croissant", rateSlabs, 4);
            controller.ProcessEachOrder(order);

            Assert.AreEqual(order.RemainingQuantity, 1);
            Assert.AreEqual(order.ProductTotalPrice, 5.95);

        }

        [TestMethod]
        public void Test_Croissant_5()
        {
            // Arrange
            HomeController controller = new HomeController();

            var orders = new List<Order>();
            orders = controller.FetchOrderFromJSON();
            var rateSlabs = new List<RateSlab>();
            rateSlabs = orders.Find(l => l.ProductName == "Croissant").RateSlabs;
            Order order = new Order("Croissant", rateSlabs, 5);
            controller.ProcessEachOrder(order);

            Assert.AreEqual(order.RemainingQuantity, 0);
            Assert.AreEqual(order.ProductTotalPrice, 9.95);

        }

        [TestMethod]
        public void Test_Croissant_6()
        {
            // Arrange
            HomeController controller = new HomeController();

            var orders = new List<Order>();
            orders = controller.FetchOrderFromJSON();
            var rateSlabs = new List<RateSlab>();
            rateSlabs = orders.Find(l => l.ProductName == "Croissant").RateSlabs;
            Order order = new Order("Croissant", rateSlabs, 6);
            controller.ProcessEachOrder(order);

            Assert.AreEqual(order.RemainingQuantity, 0);
            Assert.AreEqual(order.ProductTotalPrice, 11.9);

        }

        [TestMethod]
        public void Test_Croissant_10()
        {
            // Arrange
            HomeController controller = new HomeController();

            var orders = new List<Order>();
            orders = controller.FetchOrderFromJSON();
            var rateSlabs = new List<RateSlab>();
            rateSlabs = orders.Find(l => l.ProductName == "Croissant").RateSlabs;
            Order order = new Order("Croissant", rateSlabs, 10);
            controller.ProcessEachOrder(order);

            Assert.AreEqual(order.RemainingQuantity, 0);
            Assert.AreEqual(order.ProductTotalPrice, 19.9);

        }

        [TestMethod]
        public void Test_Croissant_12()
        {
            // Arrange
            HomeController controller = new HomeController();

            var orders = new List<Order>();
            orders = controller.FetchOrderFromJSON();
            var rateSlabs = new List<RateSlab>();
            rateSlabs = orders.Find(l => l.ProductName == "Croissant").RateSlabs;
            Order order = new Order("Croissant", rateSlabs, 12);
            controller.ProcessEachOrder(order);

            Assert.AreEqual(order.RemainingQuantity, 0);
            Assert.AreEqual(order.ProductTotalPrice, 22.9);

        }

        [TestMethod]
        public void Test_Croissant_13()
        {
            // Arrange
            HomeController controller = new HomeController();

            var orders = new List<Order>();
            orders = controller.FetchOrderFromJSON();
            var rateSlabs = new List<RateSlab>();
            rateSlabs = orders.Find(l => l.ProductName == "Croissant").RateSlabs;
            Order order = new Order("Croissant", rateSlabs, 13);
            controller.ProcessEachOrder(order);

            Assert.AreEqual(order.RemainingQuantity, 0);
            Assert.AreEqual(order.ProductTotalPrice, 25.85);

        }

        [TestMethod]
        public void Test_Croissant_17()
        {
            // Arrange
            HomeController controller = new HomeController();

            var orders = new List<Order>();
            orders = controller.FetchOrderFromJSON();
            var rateSlabs = new List<RateSlab>();
            rateSlabs = orders.Find(l => l.ProductName == "Croissant").RateSlabs;
            Order order = new Order("Croissant", rateSlabs, 17);
            controller.ProcessEachOrder(order);

            Assert.AreEqual(order.RemainingQuantity, 0);
            Assert.AreEqual(order.ProductTotalPrice, 32.85);

        }
    }
}
