using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
using Microsoft.Identity.Client;
using Sample_BF.AzureFunctions;

namespace Sample_BF.Dialogs.Rk
{
    public class OpenRKDialog : BaseDialog
    {
        private readonly IStatePropertyAccessor<ConversationData> _conversationDataAccessor;

        public OpenRKDialog(ConversationState conversationState) : base(nameof(OpenRKDialog), conversationState)
        {
            _conversationDataAccessor = conversationState.CreateProperty<ConversationData>("ConversationData");

            // This array defines how the Waterfall will execute.
            var waterfallSteps = new WaterfallStep[]
            {
                IntialStepAsync,
                GetResumeStep,
                EndStepAsync
            };
            AddDialog(new TextPrompt(nameof(TextPrompt)));
            // Add named dialogs to the DialogSet. These names are saved in the dialog state.
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), waterfallSteps));
            
            // The initial child Dialog to run.
            InitialDialogId = nameof(WaterfallDialog);
        }

        #region Steps
        private async Task<DialogTurnResult> IntialStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var conversationData = await _conversationDataAccessor.GetAsync(stepContext.Context, () => new ConversationData(), cancellationToken);
            conversationData.rks = new List<Claim>();
            conversationData.rks.Add(new Claim("FM456789123", 50, "Fibra", ""));
            var message = "Dime el motivo por el que quieres abrir la reclamacion";
            return await stepContext.PromptAsync(nameof(TextPrompt),
               new PromptOptions
               {
                   Prompt = MessageFactory.Text(message),
                   RetryPrompt = MessageFactory.Text(message),
               }, cancellationToken);
        }
        private async Task<DialogTurnResult> GetResumeStep(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var conversationData = await _conversationDataAccessor.GetAsync(stepContext.Context, () => new ConversationData(), cancellationToken);
            conversationData.rks.FirstOrDefault().ClientComment = stepContext.Context.Activity.Text;
            Claim rk = new Claim("FM456789123", 50, "Fibra", conversationData.rks.FirstOrDefault().ClientComment);
            GetOpenAIResume openai = new GetOpenAIResume();
            var resume = openai.GetAsync(rk);
            rk.OpenAIComment = resume.Result;
            if(resume.Result != "False")
            {
                return await stepContext.PromptAsync(nameof(TextPrompt),
                           new PromptOptions
                           {
                               Prompt = MessageFactory.Text("Se ha insertado la reclamación correctamente."),
                               RetryPrompt = MessageFactory.Text("Se ha insertado la reclamación correctamente."),
                           }, cancellationToken);
            }
            else
            {
                return await stepContext.PromptAsync(nameof(TextPrompt),
                           new PromptOptions
                           {
                               Prompt = MessageFactory.Text("Ha habido un error. Disculpe las molestias."),
                               RetryPrompt = MessageFactory.Text("Ha habido un error. Disculpe las molestias."),
                           }, cancellationToken);
            }
            
        }
        private async Task<DialogTurnResult> EndStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            return await stepContext.EndDialogAsync();
        }
        #endregion
    }
}
