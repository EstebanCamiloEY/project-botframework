using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;

namespace Sample_BF.Dialogs.Rk
{
    public class CloseRKDialog : BaseDialog
    {
        private readonly IStatePropertyAccessor<ConversationData> _conversationDataAccessor;

        public CloseRKDialog(ConversationState conversationState) : base(nameof(CloseRKDialog), conversationState)
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
