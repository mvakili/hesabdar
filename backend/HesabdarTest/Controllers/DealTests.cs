using FsCheck;
using Hesabdar.Controllers;
using Hesabdar.Models;
using HesabdarTest.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using NUnit.Framework;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace HesabdarTest.Controllers
{
    [TestFixture]
    public class DealTests
    {
        protected DealController sut;
        protected MaterialController materialController;
        private DealerController dealerController;
        HesabdarContext context;

        [SetUp]
        public async Task InitializeAsync()
        {
            var db = new DbContextOptionsBuilder<HesabdarContext>()
                .UseInMemoryDatabase(databaseName: "test", new InMemoryDatabaseRoot());

            context = new HesabdarContext(db.Options);
            context.Database.EnsureCreated();

            materialController = new MaterialController(context);
            dealerController = new DealerController(context);
            sut = new DealController(context);
            await TestDataGenerator.GenerateSeedMaterialAsync(materialController);
            await TestDataGenerator.GenerateSeedDealersAsync(dealerController);
            var materials = materialController.GetMaterials(1, Int32.MaxValue).GetObject<PagedResult<Material>>().Queryable.ToList();
            var dealers = dealerController.Dealers(1, Int32.MaxValue).GetObject<PagedResult<Dealer>>().Queryable.ToList();
            await TestDataGenerator.GenerateSeedDealAsync(sut, dealers, materials);

        }


        [TearDown]
        public void Cleanup()
        {
            context = null;
        }

        [Test]
        public void GetDealsOfDealer_RandomRange_ShouldBeIntegrated()
        {
            //init
            var dealersResult = dealerController.Dealers();
            var givenDealers = dealersResult.GetObject<PagedResult<Dealer>>().Queryable.ToArray();
            var dealersArb = ControllerHelper.ChooseFrom(givenDealers).ToArbitrary();
            Prop.ForAll(dealersArb, (dealer) =>
            {
                var deals = sut.GetDealsOfDealer(dealer.Id, 1, int.MaxValue).Result.GetObject<PagedResult<Deal>>().Queryable.ToList();
                var sales = sut.GetSalesOfDealer(dealer.Id, 1, int.MaxValue).GetObject<PagedResult<Deal>>().Queryable.ToList();
                var purchases = sut.GetPurchasesOfDealer(dealer.Id, 1, int.MaxValue).GetObject<PagedResult<Deal>>().Queryable.ToList();
                var union = sales.Union(purchases).ToList();

                foreach (var item in deals)
                {
                    var u = union.Single(i => i.Id == item.Id);


                    Assert.AreEqual(u.SellerId, item.SellerId);
                    Assert.AreEqual(u.BuyerId, item.BuyerId);
                    Assert.AreEqual(u.DealPriceId, item.DealPriceId);
                    Assert.AreEqual(u.DealPaymentId, item.DealPaymentId);
                    Assert.AreEqual(u.DealTime, item.DealTime);
                    union.Remove(u);
                }

                Assert.IsEmpty(union);

            }).QuickCheckThrowOnFailure();
        }
        //New
        [Test]
        public void GetDealsOfDealer_NotExistDealer_ShoudBadRequest() {
            var dealersResult = dealerController.Dealers(1, int.MaxValue).GetObject<PagedResult<Dealer>>().Queryable.Select(x => x.Id).ToList();
            dealersResult.Add(1);
            var dealersIdArb = Arb.Generate<int>().Where(i => !dealersResult.Contains(i)).ToArbitrary();
            Prop.ForAll(dealersIdArb, (dealer) =>
            {
                //execution
                var result = sut.GetDealsOfDealer(dealer, 1, int.MaxValue).Result;
               //assertion
                Assert.IsInstanceOf<BadRequestResult>(result);

            }).QuickCheckThrowOnFailure();

        }
        //New
        [Test]
        public void GetSalesOfDealer_NullDealerId_ShoudBadRequest()
        {
            int? dealersIdArb =null;
            var sales = sut.GetSalesOfDealer(dealersIdArb, 1, int.MaxValue).GetObject<PagedResult<Deal>>().Queryable.ToList();
            foreach (var item in sales) {
                Assert.AreEqual(1, item.SellerId);
            }

        }
        //New
        [Test]
        public void GetSalesOfDealer_NotExistDealer_ShoudBadRequest()
        {
            var dealersResult = dealerController.Dealers(1, int.MaxValue).GetObject<PagedResult<Dealer>>().Queryable.Select(x => x.Id).ToList();
            dealersResult.Add(1);
            var dealersIdArb = Arb.Generate<int>().Where(i => !dealersResult.Contains(i)).ToArbitrary();
            Prop.ForAll(dealersIdArb, (dealer) =>
            {
                //execution
                var result = sut.GetSalesOfDealer(dealer, 1, int.MaxValue);
               //assertion
                Assert.IsInstanceOf<BadRequestResult>(result);

            }).QuickCheckThrowOnFailure();

        }
        //New
        [Test]
        public void GetPurchasesOfDealer_NotExistDealer_ShoudBadRequest()
        {
            var dealersResult = dealerController.Dealers(1, int.MaxValue).GetObject<PagedResult<Dealer>>().Queryable.Select(x => x.Id).ToList();
            dealersResult.Add(1);
            var dealersIdArb = Arb.Generate<int>().Where(i => !dealersResult.Contains(i)).ToArbitrary();
            Prop.ForAll(dealersIdArb, (dealer) =>
            {
                //execution
                var result = sut.GetPurchasesOfDealer(dealer, 1, int.MaxValue);
               //assertion
                Assert.IsInstanceOf<BadRequestResult>(result);

            }).QuickCheckThrowOnFailure();

        }
        //New
        [Test]
        public void GetPurchasesOfDealer_NullDealerId_ShoudBadRequest()
        {
            int? dealersIdArb = null;
            var purchases = sut.GetPurchasesOfDealer(dealersIdArb, 1, int.MaxValue).GetObject<PagedResult<Deal>>().Queryable.ToList();
            foreach (var item in purchases)
            {
                Assert.AreEqual(1, item.BuyerId);
            }

        }



    }

}