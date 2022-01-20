using System;

namespace HouseSellingBot.PersonalExceptions
{
    /// <summary>
    /// Fires if the model link is already established.
    /// </summary>
    public class AlreadyContainException : Exception
    {
        /// <summary>
        /// Fires if the model link is already established.
        /// </summary>
        public AlreadyContainException()
        {

        }
        /// <summary>
        /// Fires if the model link is already established.
        /// </summary>
        /// <param name="message">Clarification about the objects that caused the error.</param>
        public AlreadyContainException(string message) : base(message)
        {

        }
    }
}
