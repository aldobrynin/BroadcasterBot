namespace BroadcasterBot.Dialogs.Broadcaster
{
    public class BroadcasterCommand
    {
        public const string SendMessage = "All";
        public const string SendFeedback = "Dialog";
        public const string Exit = "exit";

        public BroadcasterCommand(string value, string description)
        {
            Value = value;
            Description = description;
        }

        public string Value { get; private set; }
        public string Description { get; private set; }


        public static BroadcasterCommand[] SupportedBroadcasterCommands => new[]
        {
            new BroadcasterCommand(SendMessage, "Отправить сообщение"),
            new BroadcasterCommand(SendFeedback, "Отправить сообщение с обратной связью"),
            new BroadcasterCommand(Exit, "Логаут")

        };
    }
}