using FsCheck;
using Hesabdar.Controllers;
using Hesabdar.Models;
using HesabdarTest.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace HesabdarTest.Controllers
{
    [TestFixture]
    public class StatisticsTests
    {
        protected StatisticsController sut;
        protected DealController dealController;
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
            dealController = new DealController(context);
            sut = new StatisticsController(context);
            await TestDataGenerator.GenerateSeedMaterialAsync(materialController);
            await TestDataGenerator.GenerateSeedDealersAsync(dealerController);
            var materials = materialController.GetMaterials(1, Int32.MaxValue).GetObject<PagedResult<Material>>().Queryable.ToList();
            var dealers = dealerController.Dealers(1, Int32.MaxValue).GetObject<PagedResult<Dealer>>().Queryable.ToList();
            await TestDataGenerator.GenerateSeedDealAsync(dealController, dealers, materials, 50);

        }

        [TearDown]
        public void Cleanup()
        {
            context = null;
        }

        [Test]
        public void GetGeneral_InvalidDealerId_ShouldBadRequest()
        {
            //init
            var dealersResult = dealerController.Dealers();
            var givenDealers = dealersResult.GetObject<PagedResult<Dealer>>().Queryable.Select(i => i.Id).ToList();
            var dealersArb = Arb.Generate<int>().Where(i => !givenDealers.Contains(i)).ToArbitrary();
            Prop.ForAll(dealersArb, (dealerId) =>
            {
                var result = sut.GetGeneral(dealerId).Result;
                Assert.IsInstanceOf<BadRequestObjectResult>(result);

            }).QuickCheckThrowOnFailure();
        }
        //New
        [Test]
        public void GetGenerall_NullDealerId_ShouldEqualToIdOne() {
            int? dealersIdNull = null;
            var generalsOne = sut.GetGeneral(1).Result.GetObject<GeneraStatisticsModel>();
            var generalsNull = sut.GetGeneral(dealersIdNull).Result.GetObject<GeneraStatisticsModel>();

            Assert.AreEqual(generalsNull.todaySalesPrice,generalsOne.todaySalesPrice);

            Assert.AreEqual(generalsNull.todayPurchasePrice, generalsOne.todayPurchasePrice);
            Assert.AreEqual(generalsNull.todayPurchaseInvoicesCount,generalsOne.todayPurchaseInvoicesCount);
            Assert.AreEqual(generalsNull.todaySaleInvoicesCount,generalsOne.todaySaleInvoicesCount);
           }

        //New
        [Test]
        public void GetWeeklySalesAndPurches_NullDealerId_ShouldEqualToIdOne()
        {
            int? dealersIdNull = null;
            var weeklyOne = sut.GetWeeklyPurchaseAndSalePrice(1).Result.GetObject<IEnumerable<DatePurchaseAndSalePriceModel>>();
            var weeklyNull = sut.GetWeeklyPurchaseAndSalePrice(dealersIdNull).Result.GetObject<IEnumerable<DatePurchaseAndSalePriceModel>>();

            foreach (var item in weeklyOne)
            {
                var weekNull = weeklyNull.Single(i => i.Date.Date == item.Date.Date);

                Assert.AreEqual(item.PurchasesAmount, weekNull.PurchasesAmount);
                Assert.AreEqual(item.SalesAmount, weekNull.SalesAmount);
            }

        }

        //New
        [Test]
        public void GetMonthlySalesAndPurches_NullDealerId_ShouldEqualToIdOne()
        {
            int? dealersIdNull = null;
            var monthlyOne = sut.GetMonthlyPurchaseAndSalePrice(1).Result.GetObject<IEnumerable<DatePurchaseAndSalePriceModel>>();
            var monthlyNull = sut.GetMonthlyPurchaseAndSalePrice(dealersIdNull).Result.GetObject<IEnumerable<DatePurchaseAndSalePriceModel>>();

            foreach (var item in monthlyOne)
            {
                var monthNull = monthlyNull.Single(i => i.Date.Date == item.Date.Date);

                Assert.AreEqual(item.PurchasesAmount, monthNull.PurchasesAmount);
                Assert.AreEqual(item.SalesAmount, monthNull.SalesAmount);
            }

        }

        //New
        [Test]
        public void GetTwoWeeklyPurchesPrice_NullDealerId_ShouldEqualToIdOne()
        {
            int? dealersIdNull = null;
            var twoweeklyOne = sut.GetTwoWeeksPurchasesPrice(1).Result.GetObject<IEnumerable<AmountOverDateModel>>();
            var twoweekNull = sut.GetTwoWeeksPurchasesPrice(dealersIdNull).Result.GetObject<IEnumerable<AmountOverDateModel>>();

            foreach (var item in twoweeklyOne)
            {
                var twoweekyNull = twoweekNull.Single(i => i.Date.Date == item.Date.Date);

                Assert.AreEqual(item.Amount, twoweekyNull.Amount);

            }

        }

        //New
        [Test]
        public void GetTwoWeeklySalesPrice_NullDealerId_ShouldEqualToIdOne()
        {
            int? dealersIdNull = null;
            var twoweeklyOne = sut.GetTwoWeeksSalesPrice(1).Result.GetObject<IEnumerable<AmountOverDateModel>>();
            var twoweekNull = sut.GetTwoWeeksSalesPrice(dealersIdNull).Result.GetObject<IEnumerable<AmountOverDateModel>>();

            foreach (var item in twoweeklyOne)
            {
                var twoweekyNull = twoweekNull.Single(i => i.Date.Date == item.Date.Date);

                Assert.AreEqual(item.Amount, twoweekyNull.Amount);

            }

        }


        [Test]
        public void GetGeneralWeekPurchaseAndSalePrice_ShouldBeIntegrated()
        {
            var dealersResult = dealerController.Dealers();
            var givenDealers = dealersResult.GetObject<PagedResult<Dealer>>().Queryable.Select(i => i.Id).ToArray();
            var dealersArb = ControllerHelper.ChooseFrom(givenDealers).ToArbitrary();
            Prop.ForAll(dealersArb, (dealerId) =>
           {
               var generals = sut.GetGeneral(dealerId).Result.GetObject<GeneraStatisticsModel>();
               decimal salesPrice = generals.todaySalesPrice;
               decimal purchasePrice = generals.todayPurchasePrice;
               var weekly = sut.GetWeeklyPurchaseAndSalePrice(dealerId).Result.GetObject<IEnumerable<DatePurchaseAndSalePriceModel>>();

               var todayStats = weekly.Where(x => ((DateTime)x.Date).Date == DateTime.Now.Date).First();
               decimal salesAmount = todayStats.SalesAmount;
               decimal purchasesAmount = todayStats.PurchasesAmount;

               Assert.AreEqual(salesPrice, salesAmount);
               Assert.AreEqual(purchasePrice, purchasesAmount);

           }).QuickCheckThrowOnFailure();
        }


        [Test]
        public void GetWeeklyTwoWeeklyPurchaseAndSalePrice_ShouldBeIntegrated()
        {
            var dealersResult = dealerController.Dealers();
            var givenDealers = dealersResult.GetObject<PagedResult<Dealer>>().Queryable.Select(i => i.Id).ToArray();
            var dealersArb = ControllerHelper.ChooseFrom(givenDealers).ToArbitrary();
            Prop.ForAll(dealersArb, (dealerId) =>
            {
                var weekly = sut.GetWeeklyPurchaseAndSalePrice(dealerId).Result.GetObject<IEnumerable<DatePurchaseAndSalePriceModel>>();

                var twoWeekPurchasePrice = sut.GetTwoWeeksPurchasesPrice(dealerId).Result.GetObject<IEnumerable<AmountOverDateModel>>();
                var twoWeekSalesPrice = sut.GetTwoWeeksSalesPrice(dealerId).Result.GetObject<IEnumerable<AmountOverDateModel>>();

                foreach (var daily in weekly)
                {
                    var dayPurchase = twoWeekPurchasePrice.Where(p => ((DateTime)p.Date).Date == ((DateTime)daily.Date).Date).Single();
                    var daySale = twoWeekSalesPrice.Where(s => ((DateTime)s.Date).Date == ((DateTime)daily.Date).Date).Single();

                    Assert.AreEqual(daily.PurchasesAmount, dayPurchase.Amount);
                    Assert.AreEqual(daily.SalesAmount, daySale.Amount);
                }

            }).QuickCheckThrowOnFailure();
        }

        [Test]
        public void GetTwoWeeklyMonthlyPurchaseAndSalePrice_ShouldBeIntegrated()
        {
            var dealersResult = dealerController.Dealers();
            var givenDealers = dealersResult.GetObject<PagedResult<Dealer>>().Queryable.Select(i => i.Id).ToArray();
            var dealersArb = ControllerHelper.ChooseFrom(givenDealers).ToArbitrary();
            Prop.ForAll(dealersArb, (dealerId) =>
            {
                var monthly = sut.GetMonthlyPurchaseAndSalePrice(dealerId).Result.GetObject<IEnumerable<DatePurchaseAndSalePriceModel>>();
                var twoWeekPurchasePrice = sut.GetTwoWeeksPurchasesPrice(dealerId).Result.GetObject<IEnumerable<AmountOverDateModel>>();
                var twoWeekSalesPrice = sut.GetTwoWeeksSalesPrice(dealerId).Result.GetObject<IEnumerable<AmountOverDateModel>>();

                foreach (var daily in monthly)
                {
                    var dayPurchase = twoWeekPurchasePrice.Where(p => ((DateTime)p.Date).Date == ((DateTime)daily.Date).Date).SingleOrDefault();
                    var daySale = twoWeekSalesPrice.Where(s => ((DateTime)s.Date).Date == ((DateTime)daily.Date).Date).SingleOrDefault();

                    if (dayPurchase != null && daySale != null)
                    {
                        Assert.AreEqual(daily.PurchasesAmount, dayPurchase.Amount);
                        Assert.AreEqual(daily.SalesAmount, daySale.Amount);
                    }
                }

            }).QuickCheckThrowOnFailure();
        }

        [Test]
        public void GetMaterialAmountOverTime_ShouldBeIntegrated()
        {
            var dealersResult = dealerController.Dealers().GetObject<PagedResult<Dealer>>().Queryable.ToArray();
            var dealersGen = ControllerHelper.ChooseFrom(dealersResult);

            var materialsResult = materialController.GetMaterials(1, int.MaxValue).GetObject<PagedResult<Material>>().Queryable.ToArray();
            var materialsGen = ControllerHelper.ChooseFrom(materialsResult);

            Prop.ForAll<int[]>((i) =>
            {
                var material = materialsGen.Sample(0, 1).Head;
                var dealer = dealersGen.Sample(0, 1).Head;
                //100 to 50 days later
                var dateEnd = DateTime.Now.AddDays(Gen.Choose(-50, 0).Sample(0, 1).Head);
                var dateStart = dateEnd.AddDays(Gen.Choose(-50, 0).Sample(0, 1).Head);
                //130 to 20 days later
                var dateEndMargin = dateEnd.AddDays(Gen.Choose(0, 30).Sample(0, 1).Head);
                var dateStartMargin = dateStart.AddDays(Gen.Choose(-30, 0).Sample(0, 1).Head);


                var materialOverTime = sut.GetMaterialAmountOverTime(material.Id, dealer.Id, dateStart, dateEnd).Result.GetObject<IEnumerable<AmountOverDateModel>>();
                var materialOverTimeMargined = sut.GetMaterialAmountOverTime(material.Id, dealer.Id, dateStartMargin, dateEndMargin).Result.GetObject<IEnumerable<AmountOverDateModel>>();

                foreach (var item in materialOverTime)
                {
                    var marginedItem = materialOverTimeMargined.Where(m => ((DateTime)m.Date).Date == ((DateTime)item.Date).Date).Single();

                    Assert.AreEqual(item.Amount, marginedItem.Amount);
                }

            }).QuickCheckThrowOnFailure();
        }


        //New
        [Test]
        public void GetMaterialAmountOverTime_NullItems_ShouldBeEqualToIdOne()
        {
            var materialsResult = materialController.GetMaterials(1, int.MaxValue).GetObject<PagedResult<Material>>().Queryable.ToArray();
            var materialsGen = ControllerHelper.ChooseFrom(materialsResult);

            Prop.ForAll<int[]>((i) =>
            {
                var material = materialsGen.Sample(0, 1).Head;
                
                var materialOverTime = sut.GetMaterialAmountOverTime(material.Id,1, DateTime.Now.Date, DateTime.Now.Date).Result.GetObject<IEnumerable<AmountOverDateModel>>();
                var materialOverTimeNullable = sut.GetMaterialAmountOverTime(material.Id,null , null, null).Result.GetObject<IEnumerable<AmountOverDateModel>>();

                foreach (var item in materialOverTime)
                {
                    var nulledItem = materialOverTimeNullable.Where(m => ((DateTime)m.Date).Date == ((DateTime)item.Date).Date).Single();

                    Assert.AreEqual(item.Amount, nulledItem.Amount);
                }

            }).QuickCheckThrowOnFailure();
        }
    }

}