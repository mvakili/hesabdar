using FsCheck;
using Hesabdar.Controllers;
using Hesabdar.Models;
using HesabdarTest.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace HesabdarTest.Controllers
{
    [TestFixture]
    public class DealerTests
    {
        protected DealerController sut;
        HesabdarContext context;

        [SetUp]
        public async Task InitializeAsync()
        {
            var db = new DbContextOptionsBuilder<HesabdarContext>()
                .UseInMemoryDatabase(databaseName: "test", new InMemoryDatabaseRoot());

            context = new HesabdarContext(db.Options);
            context.Database.EnsureCreated();
            this.sut = new DealerController(context);
            await TestDataGenerator.GenerateSeedDealersAsync(sut);

        }

        [TearDown]
        public void Cleanup()
        {
            context = null;
        }

        #region GetSuggestedDealers
        [Test]
        public void GetSuggestedDealers_RandomInput_ShouldBeValidOkResult()
        {
            //init
            var suggestednameGenerator = Arb.Generate<string>().ToArbitrary();


            Prop.ForAll(suggestednameGenerator, (name) =>
            {
                //execution
                var result = sut.GetSuggestedDealers(name);

                //assertion
                Assert.IsInstanceOf<OkObjectResult>(result);
            }).QuickCheckThrowOnFailure();
        }

        [Test]
        public void GetSuggestedDealers_RandomNonNullInput_ShouldBeValidEnumerable()
        {
            //init
            var suggestednameGenerator = Arb.Generate<string>().Where(i => i != null).ToArbitrary();

            Prop.ForAll(suggestednameGenerator, (name) =>
            {
                //execution
                var result = sut.GetSuggestedDealers(name);

                //assertion
                var dealers = result.GetObject<IEnumerable<Dealer>>();

            }).QuickCheckThrowOnFailure();
        }

        [Test]
        public void GetSuggestedDealers_RandomNonNullInput_ShouldNotBeEmpty()
        {
            //init
            var dealersResult = sut.Dealers();
            var givenDealers = dealersResult.GetObject<PagedResult<Dealer>>().Queryable.ToArray();
            var dealersArb = ControllerHelper.ChooseFrom(givenDealers).Where(i => i.Name != null).ToArbitrary();


            Prop.ForAll(dealersArb, (dealer) =>
            {
                //execution
                var subname = dealer.Name.Substring(0, Gen.Choose(0, dealer.Name.Length).Sample(0, 1).Single());
                var result = sut.GetSuggestedDealers(subname);
                var dealers = result.GetObject<IEnumerable<Dealer>>().ToList();

                //assertion
                Assert.AreNotEqual(dealers.Count, 0);

            }).QuickCheckThrowOnFailure();
        }

        [Test]
        public void GetSuggestedDealers_RandomNonNullInput_ShouldContainsSubName()
        {
            //init
            var dealersResult = sut.Dealers();
            var givenDealers = dealersResult.GetObject<PagedResult<Dealer>>().Queryable.ToArray();
            var dealersArb = ControllerHelper.ChooseFrom(givenDealers).Where(i => i.Name != null).ToArbitrary();

            Prop.ForAll(dealersArb, (dealer) =>
            {
                //execution
                var subName = dealer.Name.Substring(0, Gen.Choose(0, dealer.Name.Length).Sample(0, 1).Single());
                var result = sut.GetSuggestedDealers(subName);

                //assertion
                var dealers = result.GetObject<IEnumerable<Dealer>>().ToList();

                foreach (var d in dealers)
                {
                    Assert.IsTrue(d.Name.ToLower().Contains(subName.ToLower()), $" Expected: {d.Name} contains: {subName}");
                }

            }).QuickCheckThrowOnFailure();
        }

        #endregion

        #region DeleteDealer

        [Test]
        public void DeleteDealer_RandomSelectedDeal_ShouldNotFound()
        {
            Prop.ForAll<int[]>((i) =>
            {
                //init
                var dealersResult = sut.Dealers();
                var givenDealers = dealersResult.GetObject<PagedResult<Dealer>>().Queryable.ToArray();

                if (givenDealers.Length != 0)
                {
                    var dealer = ControllerHelper.ChooseFrom(givenDealers).Sample(0, 1).Single();

                    //execution
                    var result = sut.DeleteDealer(dealer.Id).Result;
                    Assert.IsInstanceOf<OkObjectResult>(result);

                    //assertion
                    var updatedResult = sut.GetDealer(dealer.Id).Result;
                    Assert.IsInstanceOf<NotFoundResult>(updatedResult);
                }


            }).QuickCheckThrowOnFailure();
        }

        [Test]
        public void DeleteDealer_DuplicateRandomDelete_ShouldBeNotFoundResult()
        {
            Prop.ForAll<int[]>((i) =>
            {
                //init
                var dealersResult = sut.Dealers();
                var givenDealers = dealersResult.GetObject<PagedResult<Dealer>>().Queryable.ToArray();

                if (givenDealers.Length != 0)
                {
                    var dealer = ControllerHelper.ChooseFrom(givenDealers).Sample(0, 1).Single();

                    //execution
                    var result1 = sut.DeleteDealer(dealer.Id).Result;
                    Assert.IsInstanceOf<OkObjectResult>(result1);
                    var result2 = sut.DeleteDealer(dealer.Id).Result;
                    //assertion
                    Assert.IsInstanceOf<NotFoundResult>(result2);

                }


            }).QuickCheckThrowOnFailure();
        }

        [Test]
        public void DeleteDealer_RandomDealer_ShouldBeNotFoundResult()
        {
            //init
            var dealersResult = sut.Dealers(1, int.MaxValue);
            var givenDealers = dealersResult.GetObject<PagedResult<Dealer>>().Queryable.ToArray();
            var dealers = new List<int>();

            foreach (var item in givenDealers)
            {
                dealers.Add(item.Id);
            }

            var dealerArb = Arb.Generate<int>().Where(i => !dealers.Contains(i)).ToArbitrary();
            Prop.ForAll(dealerArb, (id) =>
            {
                //execution
                var result = sut.DeleteDealer(id: id).Result;

                //assertion
                Assert.IsInstanceOf<NotFoundResult>(result);

            }).QuickCheckThrowOnFailure();


        }

        #endregion


    }

}