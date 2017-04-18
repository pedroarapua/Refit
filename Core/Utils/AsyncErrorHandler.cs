using System;
using System.Diagnostics;

namespace Core
{
    public static class AsyncErrorHandler
    {
        public static void HandleException(Exception exception)
        {
            Debug.WriteLine(exception.Message);
        }
    }
}
