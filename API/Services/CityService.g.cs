using Demo17Sep.Models;
using Demo17Sep.Data;
using Demo17Sep.Filter;
using Demo17Sep.Entities;
using Demo17Sep.Logger;
using Microsoft.AspNetCore.JsonPatch;
using System.Linq.Expressions;

namespace Demo17Sep.Services
{
    /// <summary>
    /// The cityService responsible for managing city related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting city information.
    /// </remarks>
    public interface ICityService
    {
        /// <summary>Retrieves a specific city by its primary key</summary>
        /// <param name="id">The primary key of the city</param>
        /// <returns>The city data</returns>
        City GetById(Guid id);

        /// <summary>Retrieves a list of citys based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of citys</returns>
        List<City> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc");

        /// <summary>Adds a new city</summary>
        /// <param name="model">The city data to be added</param>
        /// <returns>The result of the operation</returns>
        Guid Create(City model);

        /// <summary>Updates a specific city by its primary key</summary>
        /// <param name="id">The primary key of the city</param>
        /// <param name="updatedEntity">The city data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Update(Guid id, City updatedEntity);

        /// <summary>Updates a specific city by its primary key</summary>
        /// <param name="id">The primary key of the city</param>
        /// <param name="updatedEntity">The city data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Patch(Guid id, JsonPatchDocument<City> updatedEntity);

        /// <summary>Deletes a specific city by its primary key</summary>
        /// <param name="id">The primary key of the city</param>
        /// <returns>The result of the operation</returns>
        bool Delete(Guid id);
    }

    /// <summary>
    /// The cityService responsible for managing city related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting city information.
    /// </remarks>
    public class CityService : ICityService
    {
        private Demo17SepContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the City class.
        /// </summary>
        /// <param name="dbContext">dbContext value to set.</param>
        public CityService(Demo17SepContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>Retrieves a specific city by its primary key</summary>
        /// <param name="id">The primary key of the city</param>
        /// <returns>The city data</returns>
        public City GetById(Guid id)
        {
            var entityData = _dbContext.City.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            return entityData;
        }

        /// <summary>Retrieves a list of citys based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of citys</returns>/// <exception cref="Exception"></exception>
        public List<City> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            var result = GetCity(filters, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return result;
        }

        /// <summary>Adds a new city</summary>
        /// <param name="model">The city data to be added</param>
        /// <returns>The result of the operation</returns>
        public Guid Create(City model)
        {
            model.Id = CreateCity(model);
            return model.Id;
        }

        /// <summary>Updates a specific city by its primary key</summary>
        /// <param name="id">The primary key of the city</param>
        /// <param name="updatedEntity">The city data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Update(Guid id, City updatedEntity)
        {
            UpdateCity(id, updatedEntity);
            return true;
        }

        /// <summary>Updates a specific city by its primary key</summary>
        /// <param name="id">The primary key of the city</param>
        /// <param name="updatedEntity">The city data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Patch(Guid id, JsonPatchDocument<City> updatedEntity)
        {
            PatchCity(id, updatedEntity);
            return true;
        }

        /// <summary>Deletes a specific city by its primary key</summary>
        /// <param name="id">The primary key of the city</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Delete(Guid id)
        {
            DeleteCity(id);
            return true;
        }
        #region
        private List<City> GetCity(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            if (pageSize < 1)
            {
                throw new ApplicationException("Page size invalid!");
            }

            if (pageNumber < 1)
            {
                throw new ApplicationException("Page mumber invalid!");
            }

            var query = _dbContext.City.IncludeRelated().AsQueryable();
            int skip = (pageNumber - 1) * pageSize;
            var result = FilterService<City>.ApplyFilter(query, filters, searchTerm);
            if (!string.IsNullOrEmpty(sortField))
            {
                var parameter = Expression.Parameter(typeof(City), "b");
                var property = Expression.Property(parameter, sortField);
                var lambda = Expression.Lambda<Func<City, object>>(Expression.Convert(property, typeof(object)), parameter);
                if (sortOrder.Equals("asc", StringComparison.OrdinalIgnoreCase))
                {
                    result = result.OrderBy(lambda);
                }
                else if (sortOrder.Equals("desc", StringComparison.OrdinalIgnoreCase))
                {
                    result = result.OrderByDescending(lambda);
                }
                else
                {
                    throw new ApplicationException("Invalid sort order. Use 'asc' or 'desc'");
                }
            }

            var paginatedResult = result.Skip(skip).Take(pageSize).ToList();
            return paginatedResult;
        }

        private Guid CreateCity(City model)
        {
            _dbContext.City.Add(model);
            _dbContext.SaveChanges();
            return model.Id;
        }

        private void UpdateCity(Guid id, City updatedEntity)
        {
            _dbContext.City.Update(updatedEntity);
            _dbContext.SaveChanges();
        }

        private bool DeleteCity(Guid id)
        {
            var entityData = _dbContext.City.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            if (entityData == null)
            {
                throw new ApplicationException("No data found!");
            }

            _dbContext.City.Remove(entityData);
            _dbContext.SaveChanges();
            return true;
        }

        private void PatchCity(Guid id, JsonPatchDocument<City> updatedEntity)
        {
            if (updatedEntity == null)
            {
                throw new ApplicationException("Patch document is missing!");
            }

            var existingEntity = _dbContext.City.FirstOrDefault(t => t.Id == id);
            if (existingEntity == null)
            {
                throw new ApplicationException("No data found!");
            }

            updatedEntity.ApplyTo(existingEntity);
            _dbContext.City.Update(existingEntity);
            _dbContext.SaveChanges();
        }
        #endregion
    }
}