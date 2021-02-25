using Bot.Services;
using Microsoft.Bot.Builder.AI.QnA.Dialogs;
using Microsoft.Bot.Builder.Dialogs;
using System;

namespace Bot.Dialogs
{
	public class QnADialog : QnAMakerDialog
	{
		private readonly StateService stateService;

		public QnADialog(string dialogId, StateService stateService) : base(dialogId)
		{
			this.stateService = stateService ?? throw new ArgumentNullException($"{nameof(stateService)} QnADialog");
		}
	}
}
