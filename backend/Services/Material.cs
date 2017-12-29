using System.Linq;
using api.DataContext;
using api.Models;
using Microsoft.AspNetCore.Http;
using api.Models.Data.Materials;
using api.Providers;

namespace api.Services
{
    public partial class MaterialService : BaseService {
        public MaterialService() : base() {}
        public MaterialService(ModuleContainer container) : base(container) {}

    }
}