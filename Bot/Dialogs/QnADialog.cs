using AdaptiveCards;
using Bot.Services;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.AI.QnA;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Bot.Dialogs
{
	public class QnADialog : ComponentDialog
	{
		private readonly StateService stateService;

		public QnADialog(string dialogId, StateService stateService) : base(dialogId)
		{
			this.stateService = stateService ?? throw new ArgumentNullException($"{nameof(stateService)} in QnA Dialog Class");

			InitializeWaterfallDialog();
		}

		private void InitializeWaterfallDialog()
		{
			// Create Waterfall Steps
			var waterfallSteps = new WaterfallStep[]
			{
				InitialStepAsync,
				FinalStepAsync
			};

			// Add Named Dialogs
			AddDialog(new WaterfallDialog($"{nameof(QnADialog)}.mainFlow", waterfallSteps));

			// Set the starting Dialog
			InitialDialogId = $"{nameof(QnADialog)}.mainFlow";
		}

		private async Task<DialogTurnResult> InitialStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
		{
			var content = await stateService.QnAMaker.GetAnswersAsync(stepContext.Context, new QnAMakerOptions { Top = 1 });

			if (content[0].Answer.StartsWith('{'))
			{
				// Parse the JSON 
				AdaptiveCardParseResult result = AdaptiveCard.FromJson(content[0].Answer);

				// Get card from result
				AdaptiveCard card = result.Card;

				Attachment attachment = new Attachment
				{
					ContentType = AdaptiveCard.ContentType,
					Content = card
				};

				await stepContext.Context.SendActivityAsync(MessageFactory.Attachment(attachment), cancellationToken);
			}

			else
			{
				await stepContext.Context.SendActivityAsync(MessageFactory.Text(content[0].Answer), cancellationToken);
			}

			return await stepContext.NextAsync(null, cancellationToken);
		}

		private async Task<DialogTurnResult> FinalStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
		{
			return await stepContext.EndDialogAsync(null, cancellationToken);

		}
	}
}
