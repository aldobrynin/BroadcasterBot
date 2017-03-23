using Autofac;
using BroadcasterBot.Dialogs;
using Microsoft.Bot.Builder.Dialogs;

namespace BroadcasterBot
{
    public class AppModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterType<RootDialog>().As<IDialog<object>>().InstancePerDependency();
        }
    }
}