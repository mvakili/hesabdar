using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hesabdar;
using Microsoft.EntityFrameworkCore;
using Hesabdar.Controllers;
using Microsoft.AspNetCore.Mvc;
using Hesabdar.Models;

namespace HesabdarTest.Controllers
{
    [TestClass]
    public class MaterialControllerTest {
        MaterialController controller; 
        HesabdarContext context;
        
        public MaterialControllerTest () {
            
        }

        [TestInitialize]
        public void Initialize() {
            var options = new DbContextOptionsBuilder<Hesabdar.Models.HesabdarContext>()
                .UseInMemoryDatabase(databaseName: "Add_writes_to_database")
                .Options;
            context = new Hesabdar.Models.HesabdarContext(options);
            this.controller = new MaterialController(context);
        }

        [TestCleanup]
        public void Cleanup() {
            this.context.Database.EnsureDeleted();
        }
        
        [TestMethod]
        public void AddValidMaterial() {
            var result = controller.PostMaterial(new Material {
                Name = "Test"
            }).Result;

            Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));

            if (result is ObjectResult res && res.Value is Material material) {
                Assert.IsInstanceOfType(material.Id, typeof(int));
            } else {
                Assert.Fail();
            }
        }
        [TestMethod]
        public void GetInvalidMaterial() {
            Assert.IsInstanceOfType(controller.GetMaterial(1).Result, typeof(NotFoundResult));
        }

        
    }
}