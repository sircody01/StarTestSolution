using System;

namespace Star.Core.Exceptions
{
    /// <summary>
    /// This exception will be thrown when an unsupported test configuration is chosen when
    /// executing tests from MTM.
    /// </summary>
    public class UnsupportedConfigurationException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnsupportedConfigurationException" /> class.
        /// </summary>
        /// <param name="innerException">The inner exception.</param>
        public UnsupportedConfigurationException(Exception innerException)
            : base(ErrorMessages.InvalidConfigurationSelected, innerException)
        {
        }

        /// <summary>
        /// Gets a message that describes the current exception.
        /// </summary>
        /// <value>The message.</value>
        public override string Message => ErrorMessages.InvalidConfigurationSelected;
    }
}
