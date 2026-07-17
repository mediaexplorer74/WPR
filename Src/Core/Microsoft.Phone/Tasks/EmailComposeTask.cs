namespace Microsoft.Phone.Tasks
{
    public class EmailComposeTask
    {
        public string? To { get; set; }

        public string? Cc { get; set; }

        public string? Bcc { get; set; }

        public string? Subject { get; set; }

        public string? Body { get; set; }

        public void Show()
        {
            // Desktop hosts cannot invoke the Windows Phone mail composer.
        }
    }
}
