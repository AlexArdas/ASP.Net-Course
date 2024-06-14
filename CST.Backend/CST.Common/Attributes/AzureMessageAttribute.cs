namespace CST.Common.Attributes
{
    public class AzureMessageAttribute : Attribute
    {
        public string QueueName { get; }

        public bool IsOutgoing { get; }

        public AzureMessageAttribute(string queueName)
        {
            QueueName = queueName;
        }

        public AzureMessageAttribute(string queueName, bool isOutgoing)
        {
            QueueName = queueName;
            IsOutgoing = isOutgoing;
        }
    }
}
