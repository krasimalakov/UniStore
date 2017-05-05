using Microsoft.Owin;
using UniStore.App;

[assembly: OwinStartup(typeof(Startup))]

namespace UniStore.App
{
    using Owin;

    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            this.ConfigureAuth(app);
        }
    }
}