using HouseSellingBot.Models;
using HouseSellingBot.PersonalExceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HouseSellingBot.Repositories
{
    /// <summary>
    /// A class containing data processing methods for registered users.
    /// </summary>
    public class UsersRepositore
    {
        public static AppDBContext dBContext;

        /// <summary>
        /// Returns true if a user with the given chatId is found in the database. Otherwise, it's false.
        /// </summary>
        /// <param name="chatId">ChatID of the user you are looking for.</param>
        public static async Task<bool> UserIsRegisteredAsync(long chatId)
        {
            try
            {
                await GetUserByChatIdAsync(chatId);
                return true;
            }
            catch (NotFoundException)
            {
                return false;
            }
        }
        /// <summary>
        /// Return true if the user role is admin. Otherwise, it's false.
        /// </summary>
        /// <param name="chatId">The chatID of the user being checked.</param>
        public static async Task<bool> UserIsAdminAsync(long chatId)
        {
            try
            {
                var user = await GetUserByChatIdAsync(chatId);
                if (user.Role == "admin")
                {
                    return true;
                }
                return false;
            }
            catch (NotFoundException)
            {
                return false;
            }
        }
        /// <summary>
        /// Return true if the user role is director. Otherwise, it's false.
        /// </summary>
        /// <param name="chatId">The chatID of the user being checked.</param>
        public static async Task<bool> UserIsDirectorAsync(long chatId)
        {
            try
            {
                var user = await GetUserByChatIdAsync(chatId);
                if (user.Role == "director")
                {
                    return true;
                }
                return false;
            }
            catch (NotFoundException)
            {
                return false;
            }
        }

        /// <summary>
        /// Returns the director's chatID.
        /// </summary>
        /// <exception cref="NotFoundException"></exception>
        public static long GetDirectorChatId()
        {
            var director = dBContext.Users.FirstOrDefault(u => u.Role == "director");
            if (director == null)
            {
                throw new NotFoundException();
            }
            return director.ChatId;
        }
        /// <summary>
        /// Returns the director's chatID.
        /// </summary>
        /// <exception cref="NotFoundException"></exception>
        public static async Task<long> GetDirectorChatIdAsync()
        {
            var director = await dBContext.Users.FirstOrDefaultAsync(u => u.Role == "director");
            if (director == null)
            {
                throw new NotFoundException();
            }
            return director.ChatId;
        }

        /// <summary>
        /// Returns a user from the database, with the given chatId.
        /// </summary>
        /// <param name="chatId">ChatId of the user you are looking for.</param>
        /// <returns><see cref="User"/></returns>
        /// <exception cref="NotFoundException"></exception>
        public static async Task<User> GetUserByChatIdAsync(long chatId)
        {
            var user = await dBContext.Users.FirstOrDefaultAsync(u => u.ChatId == chatId);
            if (user == null)
            {
                throw new NotFoundException();
            }
            await dBContext.Houses.Include(h => h.Users).ToListAsync();
            return user;
        }
        /// <summary>
        /// Returns all users from the database who are registered as administrators.
        /// </summary>
        /// <exception cref="NotFoundException"></exception>
        public static async Task<IEnumerable<User>> GetAllAdminAsync()
        {
            var admins = await dBContext.Users.Where(u => u.Role == "admin").ToListAsync();
            if (admins.Any())
            {
                return admins;
            }
            throw new NotFoundException();
        }
        /// <summary>
        /// Returns the houses from the database that match the given user's filters.
        /// </summary>
        /// <param name="chatId">The user ID for which the selection will be made.</param>
        /// <returns><see cref="IEnumerable{T}"/> from <see cref="House"/></returns>
        /// <exception cref="NoHomesWithTheseFeaturesException"></exception>
        public static async Task<IEnumerable<House>> GetHousesWhthCustomFiltersAsync(long chatId)
        {
            var retrievedHouses = await HousesRepositore.GetAllHousesAsync();
            var user = await GetUserByChatIdAsync(chatId);

            retrievedHouses = from house in retrievedHouses
                              where (user.HouseType == null || house.Type == user.HouseType)
                              && (user.HouseDistrict == null || house.District == user.HouseDistrict)
                              && (user.HouseRoomsNumbe == null || house.RoomsNumber == user.HouseRoomsNumbe)
                              && (user.HouseMetro == null || house.Metro == user.HouseMetro)
                              select house;

            if (user.LowerPrice != null || user.HightPrice != null)
            {
                retrievedHouses = retrievedHouses.Intersect(
                    await SamplingHousesBasedOnPrice(user.LowerPrice, user.HightPrice));
            }
            if (user.LowerFootage != null || user.HightFootage != null)
            {
                retrievedHouses = retrievedHouses.Intersect(
                    await SamplingHousesBasedOnFootage(user.LowerFootage, user.HightFootage));
            }

            if (!retrievedHouses.Any())
            {
                throw new NoHomesWithTheseFeaturesException();
            }

            return retrievedHouses;
        }

        /// <summary>
        /// Writes the user to the database.
        /// </summary>
        /// <param name="user">The user being recorded.</param>
        /// <exception cref="AlreadyContainException"></exception>
        public static async Task AddUserAsync(User user)
        {
            if (dBContext.Users.Where(u => u.ChatId == user.ChatId).Any())
            {
                throw new AlreadyContainException();
            }
            dBContext.Users.Add(user);
            await dBContext.SaveChangesAsync();
        }
        /// <summary>
        /// Update user in the database.
        /// </summary>
        /// <param name="user">The user being update.</param>
        public static async Task UpdateUserAsync(User user)
        {
            dBContext.Users.Update(user);
            await dBContext.SaveChangesAsync();
        }
        /// <summary>
        /// Remove user from the database.
        /// </summary>
        /// <param name="chatId">ChatID of the user to be deleted.</param>
        public static async Task RemoveUserByChatIdAsync(long chatId)
        {
            var user = await GetUserByChatIdAsync(chatId);
            dBContext.Users.Remove(user);
            await dBContext.SaveChangesAsync();
        }
        /// <summary>
        /// Creates a relationship between home and user in the database.
        /// </summary>
        /// <param name="chatId">The user ID to which the favorite home will be added.</param>
        /// <param name="houseId"></param>
        /// <exception cref="AlreadyContainException"></exception>
        public static async Task AddFavoriteHouseToUserAsync(long chatId, int houseId)
        {
            var user = await GetUserByChatIdAsync(chatId);
            await dBContext.Houses.Include(h => h.Users).ToListAsync();

            if (user.FavoriteHouses.Where(h => h.Id == houseId).Any())
            {
                throw new AlreadyContainException("This house has already been added to this user");
            }
            user.FavoriteHouses.Add(await dBContext.Houses.FindAsync(houseId));
            await dBContext.SaveChangesAsync();
        }
        /// <summary>
        /// Removes the association of a home and a user in the database.
        /// </summary>
        /// <exception cref="AlreadyContainException"></exception>
        public static async Task RemoveFromFavoritHousesAsync(long chatId, int houseId)
        {
            var user = await GetUserByChatIdAsync(chatId);
            await dBContext.Houses.Include(h => h.Users).ToListAsync();

            if (!user.FavoriteHouses.Where(h => h.Id == houseId).Any())
            {
                throw new AlreadyContainException("This house has already been added to this user");
            }
            user.FavoriteHouses.Remove(await dBContext.Houses.FindAsync(houseId));
            await dBContext.SaveChangesAsync();
        }
        /// <summary>
        /// Clears the filters for the given user.
        /// </summary>
        /// <param name="chatId">The id of the user whose filters will be cleared.</param>
        public static async Task ClearUserFiltersAsync(long chatId)
        {
            var user = await GetUserByChatIdAsync(chatId);

            user.HouseType = null;
            user.HouseMetro = null;
            user.HouseDistrict = null;
            user.HouseRoomsNumbe = null;
            user.LowerPrice = null;
            user.HightPrice = null;
            user.LowerFootage = null;
            user.HightFootage = null;

            await dBContext.SaveChangesAsync();
        }


        private static async Task<IEnumerable<House>> SamplingHousesBasedOnPrice
            (float? lowerPrice, float? hightPrice)
        {
            if (lowerPrice == null)
            {
                return await HousesRepositore.GetHouseWithLowerPrice(Convert.ToInt32(hightPrice));
            }
            else if (hightPrice == null)
            {
                return await HousesRepositore.GetHouseWithHigherPrice(Convert.ToInt32(lowerPrice));
            }
            return await HousesRepositore.GetHouseWithPriceInBetween(Convert.ToInt32(lowerPrice),
                Convert.ToInt32(hightPrice));
        }
        private static async Task<IEnumerable<House>> SamplingHousesBasedOnFootage
            (float? lowerFootage, float? hightFootage)
        {
            if (lowerFootage == null)
            {
                return await HousesRepositore.GetHouseWithLowerFootage(Convert.ToInt32(hightFootage));
            }
            else if (hightFootage == null)
            {
                return await HousesRepositore.GetHouseWithHigherFootage(Convert.ToInt32(lowerFootage));
            }
            return await HousesRepositore.GetHouseWithFootageInBetween(Convert.ToInt32(lowerFootage),
                Convert.ToInt32(hightFootage));
        }
    }
}

