// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with Bot Builder V4 SDK Template for Visual Studio EchoBot v4.10.3

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using System.Linq;
using Microsoft.Bot.Builder.AI.QnA;

namespace EchoBot3.Bots
{
    public class EchoBot : ActivityHandler
    {
        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        { 
           await turnContext.SendActivityAsync(MessageFactory.Text($"Echo: {turnContext.Activity.Text}"), cancellationToken);
           await AccessQnAMaker(turnContext, cancellationToken);
        }
        public QnAMaker EchoBotQnA { get; private set; }
            public EchoBot(QnAMakerEndpoint endpoint)
            {
            // connects to QnA Maker endpoint for each turn
            EchoBotQnA = new QnAMaker(endpoint);
            }

        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            var welcomeText = "Hi I am Jarvis !! What Can I do for you !";
            foreach (var member in membersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    await turnContext.SendActivityAsync(MessageFactory.Text(welcomeText, welcomeText), cancellationToken);
                }
            }
        }
    private async Task AccessQnAMaker(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
{
   var results = await EchoBotQnA.GetAnswersAsync(turnContext);
   if (results.Any())
   {
      await turnContext.SendActivityAsync(MessageFactory.Text("QnA Maker Returned: " + results.First().Answer), cancellationToken);
   }
   else
   {
      await turnContext.SendActivityAsync(MessageFactory.Text("Sorry, could not find an answer in the Q and A system."), cancellationToken);
   }
}
    }
    
}
