using FsCheck;
using Hesabdar.Controllers;
using Hesabdar.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HesabdarTest.Controllers
{
    internal class TestDataGenerator
    {
        public static async Task GenerateSeedDealersAsync(DealerController sut)
        {
            var nameGenerator = Arb.Generate<string>().Where(i => i != null);
            var addressGenerator = Arb.Generate<string>();
            var phoneNumberGenerator = Arb.Generate<string>();

            var dealers = Gen.Choose(0, 10).Select(i =>
            {
                return new Dealer
                {
                    Name = nameGenerator.Sample(10, 1).Head,
                    Address = addressGenerator.Sample(50, 1).Head,
                    PhoneNumber = phoneNumberGenerator.Sample(10, 1).Head
                };
            });

            foreach (var item in dealers.Sample(0, 10))
            {
                await sut.PostDealer(item);
            }
        }
        public static async Task GenerateSeedMaterialAsync(MaterialController sut)
        {
            var nameGenerator = Arb.Generate<string>();
            var barcodeGenerator = Arb.Generate<string>();

            var materials = Gen.Choose(0, 10).Select(i =>
            {
                return new Material
                {
                    Name = nameGenerator.Sample(10, 1).Head,
                    Barcode = barcodeGenerator.Sample(50, 1).Head,

                };
            });

            foreach (var item in materials.Sample(0, 10))
            {
                await sut.PostMaterial(item);
            }
        }

        public static async Task GenerateSeedDealAsync(DealController sut, List<Dealer> dealers, List<Material> materials, int count = 50)
        {
            var dealersGenerator = Gen.Elements<Dealer>(dealers).Where(x => x.Id != 1);
            var materialGenerator = Gen.Elements<Material>(materials).NonEmptyListOf();
            var barcodeGenerator = Arb.Generate<string>();

            var sells = Gen.Choose(0, count).Select(i =>
            {
                var date = DateTime.Now;
                date = date.AddDays(Gen.Choose(-90, 0).Sample(0, 1).Head);
                var ds = dealersGenerator.Sample(0, 1).Single();
                return new Deal
                {
                    BuyerId = ds.Id,
                    Items = materialGenerator.Sample(0, 1).Head.Select(m => new DealItem
                    {
                        MaterialId = m.Id,
                        PricePerOne = Gen.Choose(1, 10000).Sample(0, 1).Head,
                        Quantity = Gen.Choose(1, 100).Sample(0, 1).Head
                    }).ToList(),
                    DealTime = date,
                    DealPrice = new Payment
                    {
                        Method = Hesabdar.Models.Enums.PaymentMethod.DealPrice,
                        PayDate = date,
                        Paid = true,
                        Amount = Gen.Choose(1, 10000).Sample(0, 1).Head
                    },
                    DealPayment = new Payment
                    {
                        Method = Gen.OneOf(Gen.Constant(Hesabdar.Models.Enums.PaymentMethod.Cash), Gen.Constant(Hesabdar.Models.Enums.PaymentMethod.Cheque)).Sample(0, 1).Head,
                        PayDate = date,
                        Paid = Gen.OneOf(Gen.Constant(true), Gen.Constant(true)).Sample(0, 1).Head,
                        Amount = Gen.Choose(1, 10000).Sample(0, 1).Head
                    }
                };
            });

            foreach (var item in sells.Sample(0, count))
            {
                await sut.PostSale(item);
            }

            var purchases = Gen.Choose(0, count).Select(i =>
            {
                var ds = dealersGenerator.Sample(0, 1).Single();
                var date = DateTime.Now;
                date = date.AddDays(Gen.Choose(-90, 0).Sample(0, 1).Head);
                return new Deal
                {
                    SellerId = ds.Id,
                    Items = materialGenerator.Sample(0, 1).Head.Select(m => new DealItem
                    {
                        MaterialId = m.Id,
                        PricePerOne = Gen.Choose(1, 10000).Sample(0, 1).Head,
                        Quantity = Gen.Choose(1, 100).Sample(0, 1).Head
                    }).ToList(),
                    DealTime = date,
                    DealPrice = new Payment
                    {
                        Method = Hesabdar.Models.Enums.PaymentMethod.DealPrice,
                        PayDate = date,
                        Paid = true,
                        Amount = Gen.Choose(1, 10000).Sample(0, 1).Head
                    },
                    DealPayment = new Payment
                    {
                        Method = Gen.OneOf(Gen.Constant(Hesabdar.Models.Enums.PaymentMethod.Cash), Gen.Constant(Hesabdar.Models.Enums.PaymentMethod.Cheque)).Sample(0, 1).Head,
                        PayDate = date,
                        Paid = Gen.OneOf(Gen.Constant(true), Gen.Constant(true)).Sample(0, 1).Head,
                        Amount = Gen.Choose(1, 10000).Sample(0, 1).Head
                    }
                };
            });

            foreach (var item in purchases.Sample(0, count))
            {
                await sut.PostPurchase(item);
            }
        }

    }

}