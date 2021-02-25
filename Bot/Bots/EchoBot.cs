// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with Bot Builder V4 SDK Template for Visual Studio EchoBot v4.11.1

using Bot.Services;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Bot.Bots
{
	public class EchoBot<T> : ActivityHandler where T : Dialog
	{
		private readonly StateService stateService;
		private readonly QnAMakerService QnAMakerService;
		private readonly Dialog dialog;

		public EchoBot(T dialog, StateService stateService, QnAMakerService QnAMakerService)
		{
			this.dialog = dialog ?? throw new ArgumentNullException($"{nameof(dialog)} in Dialog Bot");
			this.stateService = stateService ?? throw new ArgumentNullException($"{nameof(stateService)} in Greeting Bot Class");
			this.QnAMakerService = QnAMakerService ?? throw new ArgumentNullException($"{nameof(QnAMakerService)} in Greeting Bot Class");
		}

		public override async Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken = default(CancellationToken))
		{
			await base.OnTurnAsync(turnContext, cancellationToken);

			// Save any state changes that might have occured during the turn
			await stateService.UserState.SaveChangesAsync(turnContext, false, cancellationToken);
			await stateService.ConversationState.SaveChangesAsync(turnContext, false, cancellationToken);
		}

		protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
		{
			// Run the dialog with the new message Activity.
			await dialog.RunAsync(turnContext, stateService.DialogStateAccessor, cancellationToken);
		}

		protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
		{
			foreach (var member in membersAdded)
			{
				if (member.Id != turnContext.Activity.Recipient.Id)
				{
					await turnContext.SendActivityAsync(MessageFactory.Text($"Hello and welcome!"), cancellationToken);
				}
			}
		}
	}
}
