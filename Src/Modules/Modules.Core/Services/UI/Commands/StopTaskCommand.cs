namespace Modules.Core.Services.UI.Commands
{
    using Common.Command;

    internal sealed class StopTaskCommand : ICommand
    {
        public long TaskId { get; set; }
    }
}