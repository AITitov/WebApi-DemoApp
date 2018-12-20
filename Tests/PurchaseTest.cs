using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApplication.Controllers;
using WebApplication.Models;

namespace Tests
{
    [TestClass]
    public class PurchaseTest
    {
        #region Параметры
            const string categoryName = "МАТСТР";
            const string title = "плита";
            const string itemCode = "М000005874";
            const string tmcRefId = "E0D95CD1-9F7C-3E08-3D7D-955AB43B5420";
            const string tmcRefIdTest = "00000000-0000-0000-0000-000000000000";
        #endregion



        [TestMethod]
        public void GetCategories()
        {
            var controller = new PurchaseController();

            var getCategories = controller.Category();
            Assert.IsTrue(getCategories.success == true, getCategories.message);
            Assert.IsTrue(getCategories.total > 0, "Количество найденных категорий - 0");
        }
        [TestMethod]
        public void GetItemsByCategory()
        {
            var controller = new PurchaseController();

            var getItems = controller.Get_ItemsByCategory(categoryName);
            Assert.IsTrue(getItems.success == true, getItems.message);
            Assert.IsTrue(getItems.total > 0, "Количество найденных товаров - 0");
        }
        [TestMethod]
        public void GetItemsByTitle()
        {
            var controller = new PurchaseController();

            var getItems = controller.Get_ItemsByTitle(title);
            Assert.IsTrue(getItems.success == true, getItems.message);
            Assert.IsTrue(getItems.total > 0, "Количество найденных товаров - 0");
        }
        [TestMethod]
        public void GetMeasure()
        {
            var controller = new PurchaseController();

            var getMeasure = controller.Measure(itemCode);
            Assert.IsTrue(getMeasure.success == true, getMeasure.message);
            Assert.IsTrue(getMeasure.total > 0, "Количество найденных единиц измерения - 0");
        }

        [TestMethod]
        public void GetSupplyPositions()
        {
            var controller = new PurchaseController();

            var getSupplyPositions = controller.Get_SupplyItems(tmcRefId);
            Assert.IsTrue(getSupplyPositions.success == true, getSupplyPositions.message);
            Assert.IsTrue(getSupplyPositions.total > 0, "Количество найденных единиц измерения - 0");
        }

        [TestMethod]
        public void CreateSupplyPositions()
        {
            var controller = new PurchaseController();

            SupplyTicket supplyTicket = new SupplyTicket();
            supplyTicket.SupplyTicketRefId = Guid.Parse("00000000-0000-0000-0000-000000000000");
            supplyTicket.SupplyItems = new List<SupplyItem>();

            SupplyItem supplyItem = new SupplyItem
            {
                ID = 10017,
                Number = 1,
                ItemCode = "М000004990",
                ItemTitle = "!!!TEST!!!",
                SupplyItemRefId = Guid.NewGuid()
            };
            supplyTicket.SupplyItems.Add(supplyItem);

            supplyItem = new SupplyItem
            {
                Number = 2,
                ItemCode = "М000005883",
                SupplyItemRefId = Guid.NewGuid()
            };
            supplyTicket.SupplyItems.Add(supplyItem);

            supplyItem = new SupplyItem
            {
                Number = 4,
                ItemCode = "М000028820",
                SupplyItemRefId = Guid.NewGuid()
            };
            supplyTicket.SupplyItems.Add(supplyItem);

            var createSupplyPositions = controller.Create_SupplyItems(supplyTicket);
            Assert.IsTrue(createSupplyPositions.success == true, createSupplyPositions.message);
        }
    }
}
