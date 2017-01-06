﻿using System.Data.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Text;
using ShoppingCart.Domain;

namespace ShoppingCart.Data.Tests
{
    [TestClass]
    public class ShoppingCartIntegrationTests
    {
        private const string TheUri = "http://www.google.com";
        private readonly StringBuilder _logBuilder = new StringBuilder();
        private string _log;
        private readonly ShoppingCartContext _context;
        private readonly ReferenceContext _refContext;

        public ShoppingCartIntegrationTests()
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<ShoppingCartContext>());
            _context = new ShoppingCartContext();
            _refContext = new ReferenceContext();
            _context.Database.Initialize(force: true);
            SetupLogging();
        }

        [TestMethod]
        public void CanAddNewCartWithProductToCartsDbSet()
        {
            var cart = NewCart.CreateCartFromProductSelection(TheUri, null, 1, 1, 9.99m);
            _context.Carts.Add(cart);
            Assert.AreEqual(1, _context.Carts.Local.Count);
        }

        [TestMethod]
        public void CanStoreCartWithInitialProduct()
        {
            var cart = NewCart.CreateCartFromProductSelection(TheUri, null, 1, 1, 9.99m);
            var data = new WebSiteOrderData(_context, _refContext);
            var resultCart = data.StoreCartWithInitialProduct(cart);
            WriteLog();
            Assert.AreNotEqual(0, resultCart.CartId);
        }

        private void WriteLog()
        {
            Debug.WriteLine(_log);
        }

        private void SetupLogging()
        {
            _context.Database.Log = BuildLogString;
        }

        private void BuildLogString(string message)
        {
            _logBuilder.Append(message);
            _log = _logBuilder.ToString();
        }
    }
}