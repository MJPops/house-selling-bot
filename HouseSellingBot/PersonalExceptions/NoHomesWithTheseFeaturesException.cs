using System;

namespace HouseSellingBot.PersonalExceptions
{
    /// <summary>
    /// Fires when no houses are found that match the given filters.
    /// </summary>
    public class NoHomesWithTheseFeaturesException : Exception
    {
        /// <summary>
        /// Fires when no houses are found that match the given filters.
        /// </summary>
        public NoHomesWithTheseFeaturesException()
        {

        }
        /// <summary>
        /// Fires when no houses are found that match the given filters.
        /// </summary>
        public NoHomesWithTheseFeaturesException(string message) : base(message)
        {

        }
    }
}
