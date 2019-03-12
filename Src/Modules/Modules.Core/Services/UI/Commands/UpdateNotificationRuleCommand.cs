namespace Modules.Core.Services.UI.Commands
{
    using Common.Command;

    public sealed class UpdateNotificationRuleCommand : ICommand
    {
        public long RuleId { get; set; }

        public string Query { get; set; }
    }
}