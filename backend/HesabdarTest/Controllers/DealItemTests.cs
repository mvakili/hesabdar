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
    public class DealItemTests
    {
        protected DealItemController sut;
        protected DealController dealController;
        protected MaterialController materialController;
        HesabdarContext context;

        [SetUp]
        public async Task InitializeAsync()
        {
            var db = new DbContextOptionsBuilder<HesabdarContext>()
                .UseInMemoryDatabase(databaseName: "test", new InMemoryDatabaseRoot());

            context = new HesabdarContext(db.Options);
            context.Database.EnsureCreated();

            sut = new DealItemController(context);
            materialController = new MaterialController(context);
            var dealerController = new DealerController(context);
            dealController = new DealController(context);
            await TestDataGenerator.GenerateSeedMaterialAsync(materialController);
            await TestDataGenerator.GenerateSeedDealersAsync(dealerController);
            var materials = materialController.GetMaterials(1, Int32.MaxValue).GetObject<PagedResult<Material>>().Queryable.ToList();
            var dealers = dealerController.Dealers(1, Int32.MaxValue).GetObject<PagedResult<Dealer>>().Queryable.ToList();
            await TestDataGenerator.GenerateSeedDealAsync(dealController, dealers, materials);

        }

        [TearDown]
        public void Cleanup()
        {
            context = null;
        }

        #region GetDealItemsOfDeal
        [Test]
        public void GetDealItemsOfDeal_SelectedRandomDeal_ShouldContainsItem()
        {
            //init
            var dealsResult = dealController.Deals(1, int.MaxValue).GetObject<PagedResult<Deal>>().Queryable.ToArray();
            var dealsArb = ControllerHelper.ChooseFrom(dealsResult).ToArbitrary();
            Prop.ForAll(dealsArb, (deal) =>
            {
                //execution
                var result = sut.GetDealItemsOfDeal(deal.Id).GetObject<IEnumerable<DealItem>>();

                //assertion
                Assert.IsNotEmpty(result);
            }).QuickCheckThrowOnFailure();
        }


        [Test]
        public void GetDealItemsOfDeal_SelectedRandomDeal_ShouldBelongToDeal()
        {
            //init
            var dealsResult = dealController.Deals(1, int.MaxValue).GetObject<PagedResult<Deal>>().Queryable.ToArray();
            var dealsArb = ControllerHelper.ChooseFrom(dealsResult).ToArbitrary();

            Prop.ForAll(dealsArb, (deal) =>
            {
                //execution
                var result = sut.GetDealItemsOfDeal(deal.Id).GetObject<IEnumerable<DealItem>>();

                //assertion
                foreach (var r in result)
                {
                    Assert.AreEqual(r.DealId, deal.Id);
                }
            }).QuickCheckThrowOnFailure();
        }

        #endregion
        #region GetDealItem
        [Test]
        public void GetDealItem_RandomSelectInputId_ShouldBeValid()
        {
            //init
            var dealItems = new List<DealItem>();
            var dealsResult = dealController.Deals(1, int.MaxValue).GetObject<PagedResult<Deal>>().Queryable.ToArray();
            foreach (var item in dealsResult)
            {
                var dealitemofdeal = sut.GetDealItemsOfDeal(item.Id).GetObject<IEnumerable<DealItem>>().ToList();
                dealItems.AddRange(dealitemofdeal);
            }
            var dealItemArb = ControllerHelper.ChooseFrom(dealItems.ToArray()).ToArbitrary();
            Prop.ForAll(dealItemArb, (dealItem) =>
            {
                //execution
                var result = sut.GetDealItem(id: dealItem.Id).Result;
                Assert.IsInstanceOf<OkObjectResult>(result);

                var resultItem = result.GetObject<DealItem>();

                //assertion
                Assert.AreEqual(resultItem.Id, dealItem.Id);
                Assert.AreEqual(resultItem.PricePerOne, dealItem.PricePerOne);
                Assert.AreEqual(resultItem.Quantity, dealItem.Quantity);
            }).QuickCheckThrowOnFailure();

        }

        [Test]
        public void GetDealItem_RandomInputId_ShouldNotNotFound()
        {
            //init
            var dealItems = new List<int>();
            var dealsResult = dealController.Deals(1, int.MaxValue).GetObject<PagedResult<Deal>>().Queryable.ToArray();
            foreach (var item in dealsResult)
            {
                var dealitemofdeal = sut.GetDealItemsOfDeal(item.Id).GetObject<IEnumerable<DealItem>>().ToList().Select(x => x.Id);
                dealItems.AddRange(dealitemofdeal);
            }

            var dealItemsArbitrary = Arb.Generate<int>().Where(i => !dealItems.Contains(i)).ToArbitrary();
            Prop.ForAll(dealItemsArbitrary, (id) =>
            {
                //execution
                var result = sut.GetDealItem(id: id).Result;

                //assertion
                Assert.IsInstanceOf<NotFoundResult>(result);

            }).QuickCheckThrowOnFailure();

        }

        #endregion
        #region DealItemsOfMaterial
        [Test]
        public void GetDealItemsOfMaterial_SelectedRandomMaterial_ShouldBelongToDealItem()
        {
            //init
            var materialsResult = materialController.GetMaterials(1, int.MaxValue).GetObject<PagedResult<Material>>().Queryable.ToArray();
            var materialsArb = ControllerHelper.ChooseFrom(materialsResult).ToArbitrary();

            Prop.ForAll(materialsArb, (material) =>
            {
                //execution
                var result = sut.DealItemsOfMaterial(material.Id).GetObject<PagedResult<DealItemOfMaterialModel>>().Queryable.ToList();

                //assertion
                foreach (var r in result)
                {
                    Assert.AreEqual(r.MaterialId, material.Id);
                }
            }).QuickCheckThrowOnFailure();
        }
        [Test]
        public void GetDealItemsOfMaterial_RandomInputId_ShouldBeEmpty()
        {
            //init
            var materialtems = materialController.GetMaterials(1, int.MaxValue).GetObject<PagedResult<Material>>().Queryable.Select(x => x.Id).ToList();
            var dealItemsArbitrary = Arb.Generate<int>().Where(i => !materialtems.Contains(i)).ToArbitrary();

            Prop.ForAll(dealItemsArbitrary, (id) =>
            {
                //execution
                var result = sut.DealItemsOfMaterial(materialId: id).GetObject<PagedResult<DealItemOfMaterialModel>>().Queryable.ToList();
                //assertion
                Assert.IsEmpty(result);

            }).QuickCheckThrowOnFailure();

        }
        #endregion
        #region PostDealItem
        [Test]
        public void PostDealItem_InvalidMaterialId_ShouldBeBadRequest()
        {
            //init
            var materialsResult = materialController.GetMaterials(1, int.MaxValue).GetObject<PagedResult<Material>>().Queryable.Select(x => x.Id).ToList();
            var deals = dealController.Deals(1, int.MaxValue).GetObject<PagedResult<Deal>>().Queryable.ToArray();

            var materialIdArb = Arb.Generate<int>().Where(i => !materialsResult.Contains(i)).ToArbitrary();
            var dealsGen = ControllerHelper.ChooseFrom(deals);


            Prop.ForAll(materialIdArb, (materialId) =>
            {
                var deal = dealsGen.Sample(0, 1).Head;


                var dealItem = new DealItem
                {
                    DealId = deal.Id,
                    MaterialId = materialId,
                    PricePerOne = Gen.Choose(1, 1000000).Sample(0, 1).Head,
                    Quantity = Gen.Choose(1, 1000).Sample(0, 1).Head
                };

                //execution
                var result = sut.PostDealItem(dealItem).Result;
                //assertion
                Assert.IsInstanceOf<BadRequestResult>(result);

            }).QuickCheckThrowOnFailure();
        }


        [Test]
        public void PostDealItem_ValidData_ShoulBeValid()
        {
            //init
            var materialsResult = materialController.GetMaterials(1, int.MaxValue).GetObject<PagedResult<Material>>().Queryable.ToArray();
            var deals = dealController.Deals(1, int.MaxValue).GetObject<PagedResult<Deal>>().Queryable.ToArray();

            var materialsGen = ControllerHelper.ChooseFrom(materialsResult);
            var dealsGen = ControllerHelper.ChooseFrom(deals);


            Prop.ForAll<int[]>((i) =>
            {
                var material = materialsGen.Sample(0, 1).Head;
                var deal = dealsGen.Sample(0, 1).Head;


                var dealItem = new DealItem
                {
                    DealId = deal.Id,
                    MaterialId = material.Id,
                    PricePerOne = Gen.Choose(1, 1000000).Sample(0, 1).Head,
                    Quantity = Gen.Choose(1, 1000).Sample(0, 1).Head
                };

                //execution
                var result = sut.PostDealItem(dealItem).Result;

                //assertion
                Assert.IsInstanceOf<CreatedAtActionResult>(result);
            }).QuickCheckThrowOnFailure();
        }
        //New
       [Test]
        public void PostDealItem_NullDealItem_ShouldBeBadRequest()
        {
             Prop.ForAll<int[]>((i) =>
            {
              //execution
                var result = sut.PostDealItem(null).Result;

                //assertion
                Assert.IsInstanceOf<BadRequestResult>(result);
            }).QuickCheckThrowOnFailure();
        }
        [Test]
        public void PostDealItem_InvalidPricePerOne_ShoulBeBadRequest()
        {
            //init
            var materialsResult = materialController.GetMaterials(1, int.MaxValue).GetObject<PagedResult<Material>>().Queryable.ToArray();
            var deals = dealController.Deals(1, int.MaxValue).GetObject<PagedResult<Deal>>().Queryable.ToArray();

            var materialsGen = ControllerHelper.ChooseFrom(materialsResult);
            var dealsGen = ControllerHelper.ChooseFrom(deals);


            Prop.ForAll<int[]>((i) =>
            {
                var material = materialsGen.Sample(0, 1).Head;
                var deal = dealsGen.Sample(0, 1).Head;


                var dealItem = new DealItem
                {
                    DealId = deal.Id,
                    MaterialId = material.Id,
                    PricePerOne = Gen.Choose(-1000000, -1).Sample(0, 1).Head,
                    Quantity = Gen.Choose(1, 1000).Sample(0, 1).Head
                };

                //execution
                var result = sut.PostDealItem(dealItem).Result;
                //assertion
                Assert.IsInstanceOf<BadRequestResult>(result);

            }).QuickCheckThrowOnFailure();
        }
        [Test]
        public void PostDealItem_InvalidQuantity_ShoulBeBadRequest()
        {
            //init
            var materialsResult = materialController.GetMaterials(1, int.MaxValue).GetObject<PagedResult<Material>>().Queryable.ToArray();
            var deals = dealController.Deals(1, int.MaxValue).GetObject<PagedResult<Deal>>().Queryable.ToArray();

            var materialsGen = ControllerHelper.ChooseFrom(materialsResult);
            var dealsGen = ControllerHelper.ChooseFrom(deals);


            Prop.ForAll<int[]>((i) =>
            {
                var material = materialsGen.Sample(0, 1).Head;
                var deal = dealsGen.Sample(0, 1).Head;


                var dealItem = new DealItem
                {
                    DealId = deal.Id,
                    MaterialId = material.Id,
                    PricePerOne = Gen.Choose(1, 1000000).Sample(0, 1).Head,
                    Quantity = Gen.Choose(-1000000, -1).Sample(0, 1).Head
                };

                //execution
                var result = sut.PostDealItem(dealItem).Result;
                //assertion
                Assert.IsInstanceOf<BadRequestResult>(result);

            }).QuickCheckThrowOnFailure();
        }

        #endregion
        #region PutDealItem
        [Test]
        public void PutDealItem_ValidData_ShoulBeValid()
        {
            //init
            var materialsResult = materialController.GetMaterials(1, int.MaxValue).GetObject<PagedResult<Material>>().Queryable.ToArray();
            var deals = dealController.Deals(1, int.MaxValue).GetObject<PagedResult<Deal>>().Queryable.ToArray();
            var materialsGen = ControllerHelper.ChooseFrom(materialsResult);
            var dealsGen = ControllerHelper.ChooseFrom(deals);


            Prop.ForAll<int[]>((i) =>
            {

                //execution
                var deal = dealsGen.Sample(0, 1).Head;
                var dealItems = sut.GetDealItemsOfDeal(deal.Id).GetObject<IEnumerable<DealItem>>().ToArray();
                var dealItem = ControllerHelper.ChooseFrom(dealItems).Sample(0, 1).Head;

                var material = materialsGen.Sample(0, 1).Head;
                dealItem.MaterialId = material.Id;
                dealItem.DealId = deal.Id;
                dealItem.PricePerOne = Gen.Choose(1, 1000000).Sample(0, 1).Head;
                dealItem.Quantity = Gen.Choose(1, 1000).Sample(0, 1).Head;
                var result = sut.PutDealItem(dealItem.Id, dealItem).Result;
                //assertion
                Assert.IsInstanceOf<NoContentResult>(result);
            }).QuickCheckThrowOnFailure();
        }

        [Test]
        public void PutDealItem_ValidData_ShoulBeUpdated()
        {
            //init
            var materialsResult = materialController.GetMaterials(1, int.MaxValue).GetObject<PagedResult<Material>>().Queryable.ToArray();
            var deals = dealController.Deals(1, int.MaxValue).GetObject<PagedResult<Deal>>().Queryable.ToArray();
            var materialsGen = ControllerHelper.ChooseFrom(materialsResult);
            var dealsGen = ControllerHelper.ChooseFrom(deals);

            Prop.ForAll<int[]>((i) =>
            {
                //execution
                var deal = dealsGen.Sample(0, 1).Head;
                var dealItems = sut.GetDealItemsOfDeal(deal.Id).GetObject<IEnumerable<DealItem>>().ToArray();
                var dealItem = ControllerHelper.ChooseFrom(dealItems).Sample(0, 1).Head;

                var material = materialsGen.Sample(0, 1).Head;
                dealItem.MaterialId = material.Id;
                dealItem.DealId = deal.Id;
                var price = Gen.Choose(1, 1000000).Sample(0, 1).Head;
                dealItem.PricePerOne = price;
                var quantity = Gen.Choose(1, 1000).Sample(0, 1).Head;
                dealItem.Quantity = quantity;
                var result = sut.PutDealItem(dealItem.Id, dealItem).Result;

                //assertion
                Assert.IsInstanceOf<NoContentResult>(result);
                var result1 = sut.GetDealItem(dealItem.Id).Result.GetObject<DealItem>();
                Assert.AreEqual(result1.MaterialId, material.Id);
                Assert.AreEqual(result1.PricePerOne, price);
                Assert.AreEqual(result1.Quantity, quantity);

            }).QuickCheckThrowOnFailure();
        }

        [Test]
        public void PutDealItem_InvalidId_ShouldBeBadRequest()
        {
            //init
            var materialsResult = materialController.GetMaterials(1, int.MaxValue).GetObject<PagedResult<Material>>().Queryable.ToArray();
            var deals = dealController.Deals(1, int.MaxValue).GetObject<PagedResult<Deal>>().Queryable.ToArray();
            var materialsGen = ControllerHelper.ChooseFrom(materialsResult);
            var dealsGen = ControllerHelper.ChooseFrom(deals);

            var dealItems = new List<DealItem>();
            foreach (var item in deals)
            {
                var dealitemofdeal = sut.GetDealItemsOfDeal(item.Id).GetObject<IEnumerable<DealItem>>().ToList();
                dealItems.AddRange(dealitemofdeal);
            }
            var dealItemGen = ControllerHelper.ChooseFrom(dealItems.ToArray());

            Prop.ForAll(dealItemGen.ToArbitrary(), (dealItem) =>
            {
                var deal = dealsGen.Sample(0, 1).Head;
                var material = materialsGen.Sample(0, 1).Head;

                dealItem.MaterialId = material.Id;
                dealItem.DealId = deal.Id;
                var price = Gen.Choose(1, 1000000).Sample(0, 1).Head;
                dealItem.PricePerOne = price;
                var quantity = Gen.Choose(1, 1000).Sample(0, 1).Head;
                dealItem.Quantity = quantity;

                var newId = Arb.Generate<int>().Where(it => it != dealItem.Id).Sample(0, 1).Head;

                //execution
                var result = sut.PutDealItem(newId, dealItem).Result;
                //assertion
                Assert.IsInstanceOf<BadRequestResult>(result);
            }).QuickCheckThrowOnFailure();
        }

        [Test]
        public void PutDealItem_InvalidDealItem_ShouldBeNotFound()
        {
            //init
            var materialsResult = materialController.GetMaterials(1, int.MaxValue).GetObject<PagedResult<Material>>().Queryable.ToArray();
            var deals = dealController.Deals(1, int.MaxValue).GetObject<PagedResult<Deal>>().Queryable.ToArray();
            var materialsGen = ControllerHelper.ChooseFrom(materialsResult);
            var dealsGen = ControllerHelper.ChooseFrom(deals);

            var dealItems = new List<int>();
            foreach (var item in deals)
            {
                var dealitemofdeal = sut.GetDealItemsOfDeal(item.Id).GetObject<IEnumerable<DealItem>>().ToList().Select(x => x.Id);
                dealItems.AddRange(dealitemofdeal);
            }

            var invalidDealItemArb = Arb.Generate<int>().Where(i => !dealItems.Contains(i)).ToArbitrary();

            Prop.ForAll(invalidDealItemArb, (i) =>
            {
                var deal = dealsGen.Sample(0, 1).Head;
                var dealItem = new DealItem();

                var material = materialsGen.Sample(0, 1).Head;
                dealItem.MaterialId = material.Id;
                dealItem.DealId = deal.Id;
                var price = Gen.Choose(1, 1000000).Sample(0, 1).Head;
                dealItem.PricePerOne = price;
                var quantity = Gen.Choose(1, 1000).Sample(0, 1).Head;
                dealItem.Quantity = quantity;
                //execution
                var result = sut.PutDealItem(dealItem.Id, dealItem).Result;
                //assertion
                Assert.IsInstanceOf<NotFoundResult>(result);
            }).QuickCheckThrowOnFailure();
        }

        [Test]
        public void PutDealItem_InvalidMaterialId_ShouldbeBadRequest()
        {
            //init
            var materialsResult = materialController.GetMaterials(1, int.MaxValue).GetObject<PagedResult<Material>>().Queryable.Select(x => x.Id).ToList();
            var deals = dealController.Deals(1, int.MaxValue).GetObject<PagedResult<Deal>>().Queryable.ToArray();
            var materialIdArb = Arb.Generate<int>().Where(i => !materialsResult.Contains(i)).ToArbitrary();


            Prop.ForAll(materialIdArb, (materialId) =>
            {
                var deal = ControllerHelper.ChooseFrom(deals).Sample(0, 1).Head;
                var dealItems = sut.GetDealItemsOfDeal(deal.Id).GetObject<IEnumerable<DealItem>>().ToArray();
                var dealItem = ControllerHelper.ChooseFrom(dealItems).Sample(0, 1).Head;
                dealItem.MaterialId = materialId;
                dealItem.DealId = deal.Id;
                var price = Gen.Choose(1, 1000000).Sample(0, 1).Head;
                dealItem.PricePerOne = price;
                var quantity = Gen.Choose(1, 1000).Sample(0, 1).Head;
                dealItem.Quantity = quantity;
                //execution
                var result = sut.PutDealItem(dealItem.Id, dealItem).Result;
                //assertion
                Assert.IsInstanceOf<BadRequestResult>(result);

            }).QuickCheckThrowOnFailure();
        }

        [Test]
        public void PutDealItem_ChangedDealId_ShouldBeBadRequest()
        {
            //init
            var materialsResult = materialController.GetMaterials(1, int.MaxValue).GetObject<PagedResult<Material>>().Queryable.ToArray();
            var deals = dealController.Deals(1, int.MaxValue).GetObject<PagedResult<Deal>>().Queryable.ToArray();
            var materialsGen = ControllerHelper.ChooseFrom(materialsResult);
            var dealsGen = ControllerHelper.ChooseFrom(deals);

            Prop.ForAll<int[]>((i) =>
            {
                //execution
                var deal = dealsGen.Sample(0, 1).Head;
                var dealItems = sut.GetDealItemsOfDeal(deal.Id).GetObject<IEnumerable<DealItem>>().ToArray();
                var dealItem = ControllerHelper.ChooseFrom(dealItems).Sample(0, 1).Head;
                var newDealId = dealsGen.Sample(0, 1).Head.Id;
                var material = materialsGen.Sample(0, 1).Head;
                dealItem.MaterialId = material.Id;
                dealItem.DealId = newDealId;
                var price = Gen.Choose(1, 1000000).Sample(0, 1).Head;
                dealItem.PricePerOne = price;
                var quantity = Gen.Choose(1, 1000).Sample(0, 1).Head;
                dealItem.Quantity = quantity;
                var result = sut.PutDealItem(dealItem.Id, dealItem).Result;
                Assert.IsInstanceOf<BadRequestResult>(result);

            }).QuickCheckThrowOnFailure();
        }

        [Test]
        public void PutDealItem_NegativePrice_ShouldBeBadRequest()
        {
            //init
            var materialsResult = materialController.GetMaterials(1, int.MaxValue).GetObject<PagedResult<Material>>().Queryable.ToArray();
            var deals = dealController.Deals(1, int.MaxValue).GetObject<PagedResult<Deal>>().Queryable.ToArray();
            var materialsGen = ControllerHelper.ChooseFrom(materialsResult);
            var dealsGen = ControllerHelper.ChooseFrom(deals);

            Prop.ForAll<int[]>((i) =>
            {
                //execution
                var deal = dealsGen.Sample(0, 1).Head;
                var dealItems = sut.GetDealItemsOfDeal(deal.Id).GetObject<IEnumerable<DealItem>>().ToArray();
                var dealItem = ControllerHelper.ChooseFrom(dealItems).Sample(0, 1).Head;
                var material = materialsGen.Sample(0, 1).Head;
                dealItem.MaterialId = material.Id;
                dealItem.DealId = deal.Id;
                var price = Gen.Choose(-100000, -1).Sample(0, 1).Head;
                dealItem.PricePerOne = price;
                var quantity = Gen.Choose(1, 1000).Sample(0, 1).Head;
                dealItem.Quantity = quantity;
                var result = sut.PutDealItem(dealItem.Id, dealItem).Result;
                Assert.IsInstanceOf<BadRequestResult>(result);

            }).QuickCheckThrowOnFailure();
        }

        [Test]
        public void PutDealItem_NegativeQuantity_ShouldBeBadRequest()
        {
            //init
            var materialsResult = materialController.GetMaterials(1, int.MaxValue).GetObject<PagedResult<Material>>().Queryable.ToArray();
            var deals = dealController.Deals(1, int.MaxValue).GetObject<PagedResult<Deal>>().Queryable.ToArray();
            var materialsGen = ControllerHelper.ChooseFrom(materialsResult);
            var dealsGen = ControllerHelper.ChooseFrom(deals);

            Prop.ForAll<int[]>((i) =>
            {
                //execution
                var deal = dealsGen.Sample(0, 1).Head;
                var dealItems = sut.GetDealItemsOfDeal(deal.Id).GetObject<IEnumerable<DealItem>>().ToArray();
                var dealItem = ControllerHelper.ChooseFrom(dealItems).Sample(0, 1).Head;
                var material = materialsGen.Sample(0, 1).Head;
                dealItem.MaterialId = material.Id;
                dealItem.DealId = deal.Id;
                var price = Gen.Choose(1, 100000000).Sample(0, 1).Head;
                dealItem.PricePerOne = price;
                var quantity = Gen.Choose(-10000000, -1).Sample(0, 1).Head;
                dealItem.Quantity = quantity;
                var result = sut.PutDealItem(dealItem.Id, dealItem).Result;
                Assert.IsInstanceOf<BadRequestResult>(result);

            }).QuickCheckThrowOnFailure();
        }


        [Test]
        public void PutDealItem_NullDealItem_ShouldBeBadRequest()
        {
            //init
            var materialsResult = materialController.GetMaterials(1, int.MaxValue).GetObject<PagedResult<Material>>().Queryable.ToArray();
            var deals = dealController.Deals(1, int.MaxValue).GetObject<PagedResult<Deal>>().Queryable.ToArray();
            var materialsGen = ControllerHelper.ChooseFrom(materialsResult);
            var dealsGen = ControllerHelper.ChooseFrom(deals);

            Prop.ForAll<int[]>((i) =>
            {
                //execution
                var deal = dealsGen.Sample(0, 1).Head;
                var dealItems = sut.GetDealItemsOfDeal(deal.Id).GetObject<IEnumerable<DealItem>>().ToArray();
                if(dealItems.Any())
                {
                    var dealItem = ControllerHelper.ChooseFrom(dealItems).Sample(0, 1).Head;
                    var result = sut.PutDealItem(dealItem.Id, null).Result;
                    Assert.IsInstanceOf<BadRequestResult>(result);
                }

            }).QuickCheckThrowOnFailure();
        }
        #endregion
        #region DeleteDealItem
        [Test]
        public void DeleteDealItem_RandomSelectedInput_ShoulOkResult()
        {
            //init
            var deals = dealController.Deals(1, int.MaxValue).GetObject<PagedResult<Deal>>().Queryable.ToArray();
            var dealsGen = ControllerHelper.ChooseFrom(deals);

            Prop.ForAll<int[]>((i) =>
            {
                //execution
                var deal = dealsGen.Sample(0, 1).Head;
                var dealItems = sut.GetDealItemsOfDeal(deal.Id).GetObject<IEnumerable<DealItem>>().ToArray();
                if (dealItems.Any())
                {
                    var dealItem = ControllerHelper.ChooseFrom(dealItems).Sample(0, 1).Head.Id;
                    var result = sut.DeleteDealItem(dealItem).Result;

                    //assertion
                    Assert.IsInstanceOf<OkObjectResult>(result);
                }


            }).QuickCheckThrowOnFailure();
        }
        [Test]
        public void DeleteDealItem_InvalidInput_ShouldNotFound()
        {
            //init
            var deals = dealController.Deals(1, int.MaxValue).GetObject<PagedResult<Deal>>().Queryable.ToArray();
            var dealsGen = ControllerHelper.ChooseFrom(deals);

            var dealItems = new List<int>();
            foreach (var item in deals)
            {
                var dealitemofdeal = sut.GetDealItemsOfDeal(item.Id).GetObject<IEnumerable<DealItem>>().ToList().Select(x => x.Id);
                dealItems.AddRange(dealitemofdeal);
            }

            var invalidDealItemArb = Arb.Generate<int>().Where(i => !dealItems.Contains(i)).ToArbitrary();

            Prop.ForAll(invalidDealItemArb, (i) =>
            {
                var result = sut.DeleteDealItem(i).Result;
                Assert.IsInstanceOf<NotFoundResult>(result);
            }).QuickCheckThrowOnFailure();
        }
        [Test]
        public void DeleteDealItem_DuplicateRandomSelectedInput_ShouldBeBadRequest()
        {
            //init
            var deals = dealController.Deals(1, int.MaxValue).GetObject<PagedResult<Deal>>().Queryable.ToArray();
            var dealsGen = ControllerHelper.ChooseFrom(deals);

            Prop.ForAll<int[]>((i) =>
            {
                //execution
                var deal = dealsGen.Sample(0, 1).Head;
                var dealItems = sut.GetDealItemsOfDeal(deal.Id).GetObject<IEnumerable<DealItem>>().ToArray();
                if (dealItems.Any())
                {
                    var dealItem = ControllerHelper.ChooseFrom(dealItems).Sample(0, 1).Head.Id;
                    var result = sut.DeleteDealItem(dealItem).Result;

                    //assertion
                    Assert.IsInstanceOf<OkObjectResult>(result);
                    var result1 = sut.DeleteDealItem(dealItem).Result;

                    //assertion
                    Assert.IsInstanceOf<BadRequestObjectResult>(result1);
                }

            }).QuickCheckThrowOnFailure();
        }
    }
    #endregion
}
