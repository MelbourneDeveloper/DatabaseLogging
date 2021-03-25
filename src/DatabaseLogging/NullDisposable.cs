using System;

namespace DatabaseLogging
{
#pragma warning disable CA1063 // Implement IDisposable Correctly
    public class NullDisposable : IDisposable
#pragma warning restore CA1063 // Implement IDisposable Correctly
    {
#pragma warning disable CA1063 // Implement IDisposable Correctly
#pragma warning disable CA1816 // Dispose methods should call SuppressFinalize
        public void Dispose()
#pragma warning restore CA1816 // Dispose methods should call SuppressFinalize
#pragma warning restore CA1063 // Implement IDisposable Correctly
        {
        }
    }

}
