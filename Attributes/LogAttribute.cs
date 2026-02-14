namespace BluntServe.Attributes
{
    /// <summary>
    /// 自定义特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class LogAttribute: Attribute
    {
        public string Operation { get; }
        public string Module { get; }

        public LogAttribute(string operation, string module = "")
        {
            Operation = operation;
            Module = module;
        }
    }
}
