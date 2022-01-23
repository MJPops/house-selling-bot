using HouseSellingBot.Models;
using HouseSellingBot.PersonalExceptions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HouseSellingBot.Repositories
{
    public class HousesRepositore
    {
        private static readonly AppDBContext dBContext = new();

        /// <summary>
        /// Added house in the database.
        /// </summary>
        /// <param name="house">The house being add.</param>
        public static async Task AddHouseAsync(House house)
        {
            dBContext.Houses.Add(house);
            await dBContext.SaveChangesAsync();
        }
        /// <summary>
        /// Update house in the database.
        /// </summary>
        /// <param name="house">The house being updated.</param>
        public static async Task UpdateHouseAsync(House house)
        {
            dBContext.Houses.Update(house);
            await dBContext.SaveChangesAsync();
        }
        /// <summary>
        /// Remove house from the database.
        /// </summary>
        /// <param name="house">The house being remove.</param>
        public static async Task RemoveHouseAsync(House house)
        {
            dBContext.Houses.Remove(house);
            await dBContext.SaveChangesAsync();
        }
        /// <summary>
        /// Returns all houses from the database.
        /// </summary>
        /// <returns><see cref="IEnumerable{T}"/> from <see cref="House"/></returns>
        /// <exception cref="NotFoundException"></exception>
        public static async Task<IEnumerable<House>> GetAllHousesAsync()
        {
            var houses = await dBContext.Houses.ToListAsync();
            if (houses == null)
            {
                throw new NotFoundException();
            }
            return houses;
        }
        /// <summary>
        /// Returns houses from the database with the appropriate type.
        /// </summary>
        /// <param name="type">The type of house you are looking for.</param>
        /// <returns><see cref="IEnumerable{T}"/> from <see cref="House"/></returns>
        /// <exception cref="NotFoundException"></exception>
        public static async Task<IEnumerable<House>> GetHousesByTypeAsync(string type)
        {
            var houses = await dBContext.Houses.Where(h => h.Type == type).ToListAsync();
            if (houses == null)
            {
                throw new NotFoundException();
            }
            return houses;
        }
        /// <summary>
        /// Returns houses from the database with the appropriate district.
        /// </summary>
        /// <param name="district">The district of house you are looking for.</param>
        /// <returns><see cref="IEnumerable{T}"/> from <see cref="House"/></returns>
        /// <exception cref="NotFoundException"></exception>
        public static async Task<IEnumerable<House>> GetHouseByDistrictAsync(string district)
        {
            var houses = await dBContext.Houses.Where(h => h.District == district).ToListAsync();
            if (houses == null)
            {
                throw new NotFoundException();
            }
            return houses;
        }
        /// <summary>
        /// Returns houses from the database with the given number of rooms.
        /// </summary>
        /// <param name="roomsNumber">The number of rooms in the desired house.</param>
        /// <returns><see cref="IEnumerable{T}"/> from <see cref="House"/></returns>
        /// <exception cref="NotFoundException"></exception>
        public static async Task<IEnumerable<House>> GetHousesByRoomsNumberAsync(int roomsNumber)
        {
            var houses = await dBContext.Houses.Where(h => h.RoomsNumber == roomsNumber).ToListAsync();
            if (houses == null)
            {
                throw new NotFoundException();
            }
            return houses;
        }
        /// <summary>
        /// Returns houses from the database that are priced higher than the specified price.
        /// </summary>
        /// <param name="price">Price border.</param>
        /// <returns><see cref="IEnumerable{T}"/> from <see cref="House"/></returns>
        /// <exception cref="NotFoundException"></exception>
        public static async Task<IEnumerable<House>> GetHouseWithHigherPrice(float price)
        {
            var houses = await dBContext.Houses.Where(h => h.Price >= price).ToListAsync();
            if (houses == null)
            {
                throw new NotFoundException();
            }
            return houses;
        }
        /// <summary>
        /// Returns houses from the database that are priced below a specified price.
        /// </summary>
        /// <param name="price">Price border.</param>
        /// <returns><see cref="IEnumerable{T}"/> from <see cref="House"/></returns>
        /// <exception cref="NotFoundException"></exception>
        public static async Task<IEnumerable<House>> GetHouseWithLowerPrice(float price)
        {
            var houses = await dBContext.Houses.Where(h => h.Price <= price).ToListAsync();
            if (houses == null)
            {
                throw new NotFoundException();
            }
            return houses;
        }
        /// <summary>
        /// Returns houses from the database whose price belongs to the given range.
        /// </summary>
        /// <param name="lowerPrice">The lower limit of the range.</param>
        /// <param name="higherPrice">The upper limit of the range.</param>
        /// <returns><see cref="IEnumerable{T}"/> from <see cref="House"/></returns>
        /// <exception cref="NotFoundException"></exception>
        public static async Task<IEnumerable<House>> GetHouseWithPriceInBetween(float lowerPrice,
            float higherPrice)
        {
            var houses = await dBContext.Houses.
                Where(h => h.Price >= lowerPrice && h.Price <= higherPrice).ToListAsync();
            if (houses == null)
            {
                throw new NotFoundException();
            }
            return houses;
        }
        /// <summary>
        /// Returns houses from the database that are footage higher than the specified price.
        /// </summary>
        /// <param name="footage">Footage border.</param>
        /// <returns><see cref="IEnumerable{T}"/> from <see cref="House"/></returns>
        /// <exception cref="NotFoundException"></exception>
        public static async Task<IEnumerable<House>> GetHouseWithHigherFootage(float footage)
        {
            var houses = await dBContext.Houses.Where(h => h.Footage >= footage).ToListAsync();
            if (houses == null)
            {
                throw new NotFoundException();
            }
            return houses;
        }
        /// <summary>
        /// Returns houses from the database that are footage below a specified price.
        /// </summary>
        /// <param name="footage">Footage border.</param>
        /// <returns><see cref="IEnumerable{T}"/> from <see cref="House"/></returns>
        /// <exception cref="NotFoundException"></exception>
        public static async Task<IEnumerable<House>> GetHouseWithLowerFootage(float footage)
        {
            var houses = await dBContext.Houses.Where(h => h.Footage <= footage).ToListAsync();
            if (houses == null)
            {
                throw new NotFoundException();
            }
            return houses;
        }
        /// <summary>
        /// Returns houses from the database whose footage belongs to the given range.
        /// </summary>
        /// <param name="lowerFootage">The lower limit of the range.</param>
        /// <param name="higherFootage">The upper limit of the range.</param>
        /// <returns><see cref="IEnumerable{T}"/> from <see cref="House"/></returns>
        /// <exception cref="NotFoundException"></exception>
        public static async Task<IEnumerable<House>> GetHouseWithFootageInBetween(float lowerFootage,
            float higherFootage)
        {
            var houses = await dBContext.Houses.
                Where(h => h.Footage >= lowerFootage && h.Price <= higherFootage).ToListAsync();
            if (houses == null)
            {
                throw new NotFoundException();
            }
            return houses;
        }
    }
}
