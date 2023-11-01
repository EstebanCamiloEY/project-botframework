using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
using Sample_BF.Dialogs.Rk;

namespace Sample_BF.Dialogs
{
    public class CoreDialog : BaseDialog
    {
        private readonly IStatePropertyAccessor<ConversationData> _conversationDataAccessor;

        public CoreDialog(ConversationState conversationState) : base(nameof(CoreDialog), conversationState)
        {
            _conversationDataAccessor = conversationState.CreateProperty<ConversationData>("ConversationData");

            // This array defines how the Waterfall will execute.
            var waterfallSteps = new WaterfallStep[]
            {
                IntialStepAsync,
                AskUserStepAsync,
                OptionStepAsync,
                EndStepAsync,
            };

            // Add named dialogs to the DialogSet. These names are saved in the dialog state.
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), waterfallSteps));

            AddDialog(new ChoicePrompt(nameof(ChoicePrompt)));

            AddDialog(new OpenRKDialog(conversationState));
            AddDialog(new CloseRKDialog(conversationState));
            AddDialog(new ReadRKDialog(conversationState));

            // The initial child Dialog to run.
            InitialDialogId = nameof(WaterfallDialog);
        }

        #region Steps
        private async Task<DialogTurnResult> IntialStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            return await stepContext.NextAsync();
        }
        private async Task<DialogTurnResult> AskUserStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var userProfile = await _conversationDataAccessor.GetAsync(stepContext.Context, () => new ConversationData(), cancellationToken);

            return await stepContext.PromptAsync(nameof(ChoicePrompt),
                new PromptOptions
                {
                    Prompt = MessageFactory.Text("Seleccione una de las opciones"),
                    RetryPrompt = MessageFactory.Text("Opcion no valida."),
                    Choices = ChoiceFactory.ToChoices(new List<string> { "Abrir reclamacion", "Consultar", "Cerrar reclamacion" }),
                }, cancellationToken);
        }
        private async Task<DialogTurnResult> OptionStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var response = ((FoundChoice)stepContext.Result).Value;
            var conversationData = await _conversationDataAccessor.GetAsync(stepContext.Context, () => new ConversationData(), cancellationToken);

            if (response.Equals("Abrir reclamacion"))
                return await stepContext.BeginDialogAsync(nameof(OpenRKDialog));
            else if (response.Equals("Consultar"))
                return await stepContext.BeginDialogAsync(nameof(CloseRKDialog));
            else if (response.Equals("Cerrar reclamacion"))
                return await stepContext.BeginDialogAsync(nameof(ReadRKDialog));

            return await stepContext.NextAsync();
        }
        private async Task<DialogTurnResult> EndStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            return await stepContext.ReplaceDialogAsync(nameof(CoreDialog));
        }
        #endregion

        #region Validators
        #endregion
    }
}
