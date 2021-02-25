using Microsoft.Bot.Builder.AI.QnA;
using Microsoft.Bot.Configuration;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bot.Services
{
	public class QnAMakerService
	{
		public QnAMaker QnAMaker { get; }

		public QnAMakerService(IConfiguration Configuration)
		{
			this.QnAMaker = new QnAMaker(new QnAMakerEndpoint { Host = Configuration["KB:Host"], KnowledgeBaseId = Configuration["KB:Id"], EndpointKey = Configuration["KB:EndpointKey"] });
		}
	}
}
