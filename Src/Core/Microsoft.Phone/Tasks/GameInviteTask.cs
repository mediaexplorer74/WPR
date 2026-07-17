namespace Microsoft.Phone.Tasks
{
    public sealed class GameInviteTask : ChooserBase<TaskEventArgs>
    {
        public string? SessionId { get; set; }
    }
}
