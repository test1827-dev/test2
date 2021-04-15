// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;

namespace Microsoft.BotBuilderSamples.Bots
{
    public class QnABot<T> : ActivityHandler where T : Microsoft.Bot.Builder.Dialogs.Dialog
    {
        protected readonly BotState ConversationState;
        protected readonly Microsoft.Bot.Builder.Dialogs.Dialog Dialog;
        protected readonly BotState UserState;

        private const string WelcomeMessage = "Hola test hhhhh¿Cómo podemos apoyarte?\r\n" +
                                              "Tengo estas opciones,elige la más " +
                                                "cercana a tus dudas.\r\n" +
                                                "1.- ¿Que puedo empeñar?\r\n" +
                                                "2.- ¿Cuales son los metodos de pago?\r\n" +
                                                "3.- Encuentra tu sucursales\r\n" +
                                                "4.- Duda o comentarios\r\n" +
                                                "5.- Hablar con un ejecutivo\r\n" +
                                                "Escriba el numero de la mas parecida";

        //private const string InfoMessage = "Tengo estas opciones,elige la más " +
        //                                    "cercana a tus dudas.\r\n" +
        //                                    "1.- ¿Que puedo empeñar?\r\n" +
        //                                    "2.- ¿Cuales son los metodos de pago?\r\n" +
        //                                    "3.- Encuentra tu sucursales\r\n" +
        //                                    "4.- Duda o comentarios\r\n" +
        //                                    "5.- Hablar con un ejecutivo\r\n" +
        //                                    "Escriba el numero de la mas parecida";

        //private const string LocaleMessage = "You can use the activity's 'GetLocale()' method to welcome the user " +
        //                                     "using the locale received from the channel. " +
        //                                     "If you are using the Emulator, you can set this value in Settings.";




        public QnABot(ConversationState conversationState, UserState userState, T dialog)
        {
            ConversationState = conversationState;
            UserState = userState;
            Dialog = dialog;
        }

        public override async Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken = default)
        {
            await base.OnTurnAsync(turnContext, cancellationToken);

            // Save any state changes that might have occured during the turn.
            await ConversationState.SaveChangesAsync(turnContext, false, cancellationToken);
            await UserState.SaveChangesAsync(turnContext, false, cancellationToken);
        }

        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken) =>
            // Run the Dialog with the new message Activity.
            await Dialog.RunAsync(turnContext, ConversationState.CreateProperty<DialogState>(nameof(DialogState)), cancellationToken);

        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            foreach (var member in membersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    //await turnContext.SendActivityAsync(MessageFactory.Text($"Hello and welcome!"), cancellationToken);
                    //await turnContext.SendActivityAsync($"Hi there - {member.Name}. {WelcomeMessage}", cancellationToken: cancellationToken);
                    await turnContext.SendActivityAsync(WelcomeMessage, cancellationToken: cancellationToken);
                    //await turnContext.SendActivityAsync(InfoMessage, cancellationToken: cancellationToken);
                    //await turnContext.SendActivityAsync($"{LocaleMessage} Current locale is '{turnContext.Activity.GetLocale()}'.", cancellationToken: cancellationToken);
                    //await turnContext.SendActivityAsync(PatternMessage, cancellationToken: cancellationToken);
                }
            }
        }
    }
}
