using System;

namespace DatabaseLogging
{
    //TODO: Not sure we can't just use the standard NullScope

#pragma warning disable CA1063 // Implement IDisposable Correctly
    public class NullScope : IDisposable
#pragma warning restore CA1063 // Implement IDisposable Correctly
    {
        public static NullScope Instance { get; } = new NullScope();

        private NullScope()
        {
        }

        /// <inheritdoc />
#pragma warning disable CA1063 // Implement IDisposable Correctly
#pragma warning disable CA1816 // Dispose methods should call SuppressFinalize
        public void Dispose()
#pragma warning restore CA1816 // Dispose methods should call SuppressFinalize
#pragma warning restore CA1063 // Implement IDisposable Correctly
        {
        }
    }
}
