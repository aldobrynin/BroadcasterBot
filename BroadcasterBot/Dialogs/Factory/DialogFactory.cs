using Autofac;
using JetBrains.Annotations;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Connector;

namespace BroadcasterBot.Dialogs.Factory
{
    [UsedImplicitly]
    public class DialogFactory : IDialogFactory
    {
        private readonly ILifetimeScope _lifetimeScope;

        public DialogFactory(ILifetimeScope lifetimeScope)
        {
            _lifetimeScope = lifetimeScope;
        }

        public T Create<T>(IMessageActivity messageActivity)
        {
            using (var scope = DialogModule.BeginLifetimeScope(_lifetimeScope, messageActivity))
            {
                return scope.Resolve<T>();
            }
        }
    }
}