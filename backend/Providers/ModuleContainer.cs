using api.DataContext;
using Microsoft.AspNetCore.Http;

namespace api.Providers
{
    public class ModuleContainer {
        // Modules
        public HesabdarContext DbContext {get; set;} = new HesabdarContext();
        public ISession Session {get; set;}
    }
}