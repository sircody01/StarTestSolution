using System;

namespace Star.Core.Exceptions
{
    public class ScreenshotException : Exception
    {
        public ScreenshotException()
        {
        }

        public ScreenshotException(string message) : base(message)
        {
        }

        public ScreenshotException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
