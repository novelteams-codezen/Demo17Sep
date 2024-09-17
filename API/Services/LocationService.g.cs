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
    /// The locationService responsible for managing location related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting location information.
    /// </remarks>
    public interface ILocationService
    {
        /// <summary>Retrieves a specific location by its primary key</summary>
        /// <param name="id">The primary key of the location</param>
        /// <returns>The location data</returns>
        Location GetById(Guid id);

        /// <summary>Retrieves a list of locations based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of locations</returns>
        List<Location> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc");

        /// <summary>Adds a new location</summary>
        /// <param name="model">The location data to be added</param>
        /// <returns>The result of the operation</returns>
        Guid Create(Location model);

        /// <summary>Updates a specific location by its primary key</summary>
        /// <param name="id">The primary key of the location</param>
        /// <param name="updatedEntity">The location data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Update(Guid id, Location updatedEntity);

        /// <summary>Updates a specific location by its primary key</summary>
        /// <param name="id">The primary key of the location</param>
        /// <param name="updatedEntity">The location data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Patch(Guid id, JsonPatchDocument<Location> updatedEntity);

        /// <summary>Deletes a specific location by its primary key</summary>
        /// <param name="id">The primary key of the location</param>
        /// <returns>The result of the operation</returns>
        bool Delete(Guid id);
    }

    /// <summary>
    /// The locationService responsible for managing location related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting location information.
    /// </remarks>
    public class LocationService : ILocationService
    {
        private Demo17SepContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the Location class.
        /// </summary>
        /// <param name="dbContext">dbContext value to set.</param>
        public LocationService(Demo17SepContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>Retrieves a specific location by its primary key</summary>
        /// <param name="id">The primary key of the location</param>
        /// <returns>The location data</returns>
        public Location GetById(Guid id)
        {
            var entityData = _dbContext.Location.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            return entityData;
        }

        /// <summary>Retrieves a list of locations based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of locations</returns>/// <exception cref="Exception"></exception>
        public List<Location> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            var result = GetLocation(filters, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return result;
        }

        /// <summary>Adds a new location</summary>
        /// <param name="model">The location data to be added</param>
        /// <returns>The result of the operation</returns>
        public Guid Create(Location model)
        {
            model.Id = CreateLocation(model);
            return model.Id;
        }

        /// <summary>Updates a specific location by its primary key</summary>
        /// <param name="id">The primary key of the location</param>
        /// <param name="updatedEntity">The location data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Update(Guid id, Location updatedEntity)
        {
            UpdateLocation(id, updatedEntity);
            return true;
        }

        /// <summary>Updates a specific location by its primary key</summary>
        /// <param name="id">The primary key of the location</param>
        /// <param name="updatedEntity">The location data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Patch(Guid id, JsonPatchDocument<Location> updatedEntity)
        {
            PatchLocation(id, updatedEntity);
            return true;
        }

        /// <summary>Deletes a specific location by its primary key</summary>
        /// <param name="id">The primary key of the location</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Delete(Guid id)
        {
            DeleteLocation(id);
            return true;
        }
        #region
        private List<Location> GetLocation(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            if (pageSize < 1)
            {
                throw new ApplicationException("Page size invalid!");
            }

            if (pageNumber < 1)
            {
                throw new ApplicationException("Page mumber invalid!");
            }

            var query = _dbContext.Location.IncludeRelated().AsQueryable();
            int skip = (pageNumber - 1) * pageSize;
            var result = FilterService<Location>.ApplyFilter(query, filters, searchTerm);
            if (!string.IsNullOrEmpty(sortField))
            {
                var parameter = Expression.Parameter(typeof(Location), "b");
                var property = Expression.Property(parameter, sortField);
                var lambda = Expression.Lambda<Func<Location, object>>(Expression.Convert(property, typeof(object)), parameter);
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

        private Guid CreateLocation(Location model)
        {
            _dbContext.Location.Add(model);
            _dbContext.SaveChanges();
            return model.Id;
        }

        private void UpdateLocation(Guid id, Location updatedEntity)
        {
            _dbContext.Location.Update(updatedEntity);
            _dbContext.SaveChanges();
        }

        private bool DeleteLocation(Guid id)
        {
            var entityData = _dbContext.Location.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            if (entityData == null)
            {
                throw new ApplicationException("No data found!");
            }

            _dbContext.Location.Remove(entityData);
            _dbContext.SaveChanges();
            return true;
        }

        private void PatchLocation(Guid id, JsonPatchDocument<Location> updatedEntity)
        {
            if (updatedEntity == null)
            {
                throw new ApplicationException("Patch document is missing!");
            }

            var existingEntity = _dbContext.Location.FirstOrDefault(t => t.Id == id);
            if (existingEntity == null)
            {
                throw new ApplicationException("No data found!");
            }

            updatedEntity.ApplyTo(existingEntity);
            _dbContext.Location.Update(existingEntity);
            _dbContext.SaveChanges();
        }
        #endregion
    }
}