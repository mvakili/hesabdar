using api.Providers;

namespace api.Services 
{
    public abstract class BaseService {
        public BaseService (ModuleContainer container)
        {
            this.Modules = container;
        }
        public ModuleContainer Modules {get; set;}

    }
}