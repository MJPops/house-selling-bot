using HouseSellingBot.Models;
using HouseSellingBot.PersonalExceptions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace HouseSellingBot.Repositories
{
    public class HousesRepositore
    {
        private static HttpClient httpClient = new();
        private static string ServerRoot = "https:///";

        /// <summary>
        /// Returns a house from the database with the corresponding ID.
        /// </summary>
        /// <param name="houseId">The id of the house you are looking for.</param>
        /// <exception cref="No tFoundException"></exception>
        public static async Task<House> GetHouseByIdAsync(int houseId)
        {
            HttpResponseMessage httpResponse = await httpClient.
                GetAsync(ServerRoot + "Houses/Get/" + houseId.ToString());
            var jsonRequest = await httpResponse.Content.ReadAsStringAsync();
            var house = JsonConvert.DeserializeObject<House>(jsonRequest);

            if (house == null)
            {
                throw new NotFoundException();
            }
            return house;
        }
        /// <summary>
        /// Returns all houses from the database.
        /// </summary>
        /// <returns><see cref="IEnumerable{T}"/> from <see cref="House"/></returns>
        /// <exception cref="NotFoundException"></exception>
        public static async Task<IEnumerable<House>> GetAllHousesAsync()
        {
            HttpResponseMessage httpResponse = await httpClient.GetAsync(ServerRoot + "Houses/Get");
            var jsonRequest = await httpResponse.Content.ReadAsStringAsync();
            var houses = JsonConvert.DeserializeObject<IEnumerable<House>>(jsonRequest);

            if (!houses.Any())
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
            var houses = (await GetAllHousesAsync()).Where(h => h.Type == type);
            if (!houses.Any())
            {
                throw new NotFoundException();
            }
            return houses;
        }
        /// <summary>
        /// Returns houses from the database with the appropriate metro.
        /// </summary>
        /// <param name="metro">The metro station which corresponds to the houses.</param>
        /// <returns><see cref="IEnumerable{T}"/> from <see cref="House"/></returns>
        /// <exception cref="NotFoundException"></exception>
        public static async Task<IEnumerable<House>> GetHousesByMetroAsync(string metro)
        {
            var houses = (await GetAllHousesAsync()).Where(h => h.Metro == metro);
            if (!houses.Any())
            {
                throw new NotFoundException();
            }
            return houses;
        }
        /// <summary>
        /// Returns houses from the database with the appropriate rent type.
        /// </summary>
        /// <param name="rentType">The rent type of house you are looking for.</param>
        /// <returns><see cref="IEnumerable{T}"/> from <see cref="House"/></returns>
        /// <exception cref="NotFoundException"></exception>
        public static async Task<IEnumerable<House>> GetHousesByRentTypeAsync(string rentType)
        {
            var houses = (await GetAllHousesAsync()).Where(h => h.RentType == rentType);
            if (!houses.Any())
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
            var houses = (await GetAllHousesAsync()).Where(h => h.District == district);
            if (!houses.Any())
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
            var houses = (await GetAllHousesAsync()).Where(h => h.RoomsNumber == roomsNumber);
            if (!houses.Any())
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
            var houses = (await GetAllHousesAsync()).Where(h => h.Price >= price);
            if (!houses.Any())
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
            var houses = (await GetAllHousesAsync()).Where(h => h.Price <= price);
            if (!houses.Any())
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
            var houses = (await GetAllHousesAsync()).Where(h => h.Price >= lowerPrice && h.Price <= higherPrice);
            if (!houses.Any())
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
            var houses = (await GetAllHousesAsync()).Where(h => h.Footage >= footage);
            if (!houses.Any())
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
            var houses = (await GetAllHousesAsync()).Where(h => h.Footage <= footage);
            if (!houses.Any())
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
            var houses = (await GetAllHousesAsync()).
                Where(h => h.Footage >= lowerFootage && h.Footage <= higherFootage);
            if (!houses.Any())
            {
                throw new NotFoundException();
            }
            return houses;
        }

        /// <summary>
        /// Added house in the database.
        /// </summary>
        /// <param name="house">The house being add.</param>
        public static async Task AddHouseAsync(House house)
        {
            using var dBContext = new AppDBContext();
            dBContext.Houses.Add(house);
            await dBContext.SaveChangesAsync();
        }
        /// <summary>
        /// Update house in the database.
        /// </summary>
        /// <param name="house">The house being updated.</param>
        public static async Task UpdateHouseAsync(House house)
        {
            using var dBContext = new AppDBContext();
            dBContext.Houses.Update(house);
            await dBContext.SaveChangesAsync();
        }
        /// <summary>
        /// Remove house from the database.
        /// </summary>
        /// <param name="house">The house being remove.</param>
        public static async Task RemoveHouseAsync(int houseId)
        {
            using var dBContext = new AppDBContext();
            var house = await GetHouseByIdAsync(houseId);
            dBContext.Houses.Remove(house);
            await dBContext.SaveChangesAsync();
        }
    }
}
