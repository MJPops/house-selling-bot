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
        private static AppDBContext dBContext = new AppDBContext();

        /// <summary>
        /// Returns the user with the given ID from the database.
        /// </summary>
        /// <param name="userId">The id of the user you are looking for.</param>
        /// <returns><see cref="User"/></returns>
        /// <exception cref="NotFoundException"></exception>
        public static async Task<User> GetUserByIdAsync(int userId)
        {
            var user = await dBContext.Users.FindAsync(userId);

            if (user == null)
            {
                throw new NotFoundException("The user with this ID is not registered in the database");
            }
            return user;
        }
        /// <summary>
        /// Writes the user to the database.
        /// </summary>
        /// <param name="user">The user being recorded.</param>
        /// <exception cref="AlreadyContainException"></exception>
        public static async Task AddUserAsync(User user)
        {
            if (dBContext.Users.ToList().Contains(user))
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
        /// <param name="user">The user being removed.</param>
        public static async Task RemoveUserAsync(User user)
        {
            dBContext.Users.Remove(user);
            await dBContext.SaveChangesAsync();
        }
        /// <summary>
        /// Clears the filters for the given user.
        /// </summary>
        /// <param name="userId">The id of the user whose filters will be cleared.</param>
        public static async Task ClearUserFiltersAsync(int userId)
        {
            var user = await dBContext.Users.FindAsync(userId);

            user.HouseType = null;
            user.HouseDistrict = null;
            user.HouseRoomsNumbe = null;
            user.LowerPrice = null;
            user.HightPrice = null;
            user.LowerFootage = null;
            user.HightFootage = null;

            await dBContext.SaveChangesAsync();
        }
        /// <summary>
        /// Creates a relationship between home and user in the database.
        /// </summary>
        /// <param name="userId">The user ID to which the favorite home will be added.</param>
        /// <param name="houseId"></param>
        /// <exception cref="AlreadyContainException"></exception>
        public static async Task AddFavoriteHouseToUserAsync(int userId, int houseId)
        {
            var user = await dBContext.Users.FindAsync(userId);
            await dBContext.Houses.Include(h => h.Users).ToListAsync();
            var houseToAdd = await dBContext.Houses.FindAsync(houseId);

            if (user.FavoriteHouses.Contains(houseToAdd))
            {
                throw new AlreadyContainException("This house has already been added to this user");
            }
            user.FavoriteHouses.Add(await dBContext.Houses.FindAsync(houseId));
        }
        /// <summary>
        /// Returns the houses from the database that match the given user's filters.
        /// </summary>
        /// <param name="userId">The user ID for which the selection will be made.</param>
        /// <returns><see cref="IEnumerable{T}"/> from <see cref="House"/></returns>
        /// <exception cref="NotFoundException"></exception>
        /// <exception cref="NoHomesWithTheseFeaturesException"></exception>
        public static async Task<IEnumerable<House>> GetHousesWhithCustomFiltersAsync(int userId)
        {
            var retrievedHouses = await HouseRepositore.GetAllHousesAsync();
            var user = await dBContext.Users.FindAsync(userId);
            if (user == null)
            {
                throw new NotFoundException("The user with this ID is not registered in the database");
            }

            retrievedHouses = from house in retrievedHouses
                              where (user.HouseType == null || house.Type == user.HouseType)
                              && (user.HouseDistrict == null || house.District == user.HouseDistrict)
                              && (user.HouseRoomsNumbe == null || house.RoomsNumber == user.HouseRoomsNumbe)
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


        private static async Task<IEnumerable<House>> SamplingHousesBasedOnPrice
            (float? lowerPrice, float? hightPrice)
        {
            if (lowerPrice == null)
            {
                return await HouseRepositore.GetHouseWithLowerPrice(Convert.ToInt32(hightPrice));
            }
            else if (hightPrice == null)
            {
                return await HouseRepositore.GetHouseWithHigherPrice(Convert.ToInt32(lowerPrice));
            }
            return await HouseRepositore.GetHouseWithPriceInBetween(Convert.ToInt32(lowerPrice),
                Convert.ToInt32(hightPrice));
        }
        private static async Task<IEnumerable<House>> SamplingHousesBasedOnFootage
            (float? lowerFootage, float? hightFootage)
        {
            if (lowerFootage == null)
            {
                return await HouseRepositore.GetHouseWithLowerFootage(Convert.ToInt32(hightFootage));
            }
            else if (hightFootage == null)
            {
                return await HouseRepositore.GetHouseWithHigherFootage(Convert.ToInt32(lowerFootage));
            }
            return await HouseRepositore.GetHouseWithFootageInBetween(Convert.ToInt32(lowerFootage),
                Convert.ToInt32(hightFootage));
        }
    }
}

