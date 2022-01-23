using System;

namespace HouseSellingBot.PersonalExceptions
{
    /// <summary>
    /// Fires when the searched object is not found in the database.
    /// </summary>
    public class NotFoundException : Exception
    {
        /// <summary>
        /// Fires when the searched object is not found in the database.
        /// </summary>
        public NotFoundException()
        {

        }
        /// <summary>
        /// Fires when the searched object is not found in the database.
        /// </summary>
        /// <param name="message">Clarification on the found object.</param>
        public NotFoundException(string message) : base(message)
        {

        }
    }
}
