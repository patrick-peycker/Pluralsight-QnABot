using Bot.Models;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.AI.QnA;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Extensions.Configuration;
using System;

namespace Bot.Services
{
	public class StateService
	{
		// Fields
		private readonly IConfiguration configuration;

		// State
		public UserState UserState { get; }
		public ConversationState ConversationState { get; set; }
		public QnAMaker QnAMaker { get; set; }


		// IDs
		public static string UserProfileId { get; } = $"{nameof(StateService)}.UserProfile";
		public static string ConversationDataId { get; } = $"{nameof(ConversationData)}.ConversationData";
		public static string DialogStateId { get; } = $"{nameof(StateService)}.DialogState";

		// Accessors
		public IStatePropertyAccessor<UserProfile> UserProfileAccessor { get; set; }
		public IStatePropertyAccessor<ConversationData> ConversationDataAccessor { get; set; }
		public IStatePropertyAccessor<DialogState> DialogStateAccessor { get; set; }

		public StateService(IConfiguration configuration, UserState userState, ConversationState conversationState)
		{
			this.configuration = configuration;
			UserState = userState ?? throw new ArgumentNullException($"{nameof(userState)} in State Service Class");
			ConversationState = conversationState ?? throw new ArgumentNullException($"{nameof(conversationState)} in State Service Class");
			QnAMaker = new QnAMaker(new QnAMakerEndpoint { Host = configuration["KB:Host"], KnowledgeBaseId = configuration["KB:Id"], EndpointKey = configuration["KB:EndpointKey"] });

			InitializeAccessors();
		}

		public void InitializeAccessors()
		{
			UserProfileAccessor = UserState.CreateProperty<UserProfile>(UserProfileId);
			ConversationDataAccessor = UserState.CreateProperty<ConversationData>(ConversationDataId);
			DialogStateAccessor = ConversationState.CreateProperty<DialogState>(DialogStateId);
		}
	}
}
