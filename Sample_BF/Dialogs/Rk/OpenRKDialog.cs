using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;

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
                EndStepAsync,
            };

            // Add named dialogs to the DialogSet. These names are saved in the dialog state.
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), waterfallSteps));

            // The initial child Dialog to run.
            InitialDialogId = nameof(WaterfallDialog);
        }

        #region Steps
        private async Task<DialogTurnResult> IntialStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            return await stepContext.NextAsync();
        }
        private async Task<DialogTurnResult> EndStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            return await stepContext.EndDialogAsync();
        }
        #endregion
    }
}
