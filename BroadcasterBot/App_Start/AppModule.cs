using Autofac;
using BroadcasterBot.Data;
using BroadcasterBot.Dialogs;
using BroadcasterBot.Dialogs.Factory;
using BroadcasterBot.MessageRouting;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Internals.Fibers;

namespace BroadcasterBot
{
    public class AppModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterType<RootDialog>().As<IDialog<object>>().InstancePerDependency();
            builder.RegisterAssemblyTypes(typeof(RootDialog).Assembly)
                .Except<RootDialog>()
                .Where(t => typeof(IDialog<>).IsAssignableFrom(t) || typeof(IDialog).IsAssignableFrom(t))
                .AsSelf()
                .InstancePerDependency();

            builder
                .RegisterType<DialogFactory>()
                .Keyed<IDialogFactory>(FiberModule.Key_DoNotSerialize)
                .As<IDialogFactory>()
                .SingleInstance();

            builder
                .RegisterType<UsersConversationsRepository>()
                .Keyed<IUsersConversationsRepository>(FiberModule.Key_DoNotSerialize)
                .As<IUsersConversationsRepository>()
                .SingleInstance();

            builder
                .RegisterType<MessageRouter>()
                .Keyed<IMessageRouter>(FiberModule.Key_DoNotSerialize)
                .As<IMessageRouter>()
                .SingleInstance();
        }
    }
}