using System;

namespace Epam.Demo.Core
{
    public class ExceptionAttribute : Attribute
    {
        public int RetrieTimes { get; set; }

        public string ExceptionName { get; set; }

        public ExceptionAttribute(int retriesTimes, string exceptionName)
        {
            RetrieTimes = retriesTimes;
            ExceptionName = exceptionName;
        }
    }
}