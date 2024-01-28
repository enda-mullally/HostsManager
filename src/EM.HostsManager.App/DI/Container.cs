
using Microsoft.Extensions.DependencyInjection;

namespace EM.HostsManager.App.DI
{
    public partial class Container
    {
        private readonly ServiceCollection _container = [];

        private Container()
        {
        }

        public static Container Create()
        {
            return new Container().RegisterServices();
        }

        public IServiceProvider BuildServiceProvider()
        {
            return _container.BuildServiceProvider();
        }
    }
}
