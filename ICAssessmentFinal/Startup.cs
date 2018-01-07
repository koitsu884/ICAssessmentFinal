using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ICAssessmentFinal.Startup))]
namespace ICAssessmentFinal
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
