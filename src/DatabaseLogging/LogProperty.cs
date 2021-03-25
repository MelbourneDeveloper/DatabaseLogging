namespace DatabaseLogging
{
    public class LogProperty
    {
        public LogProperty(string key, string value)
        {
            Key = key;
            Value = value;
        }

        public string Key { get; }
        public string Value { get; }
    }
}
