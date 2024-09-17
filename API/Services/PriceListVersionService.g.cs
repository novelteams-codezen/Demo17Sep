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
    /// The pricelistversionService responsible for managing pricelistversion related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting pricelistversion information.
    /// </remarks>
    public interface IPriceListVersionService
    {
        /// <summary>Retrieves a specific pricelistversion by its primary key</summary>
        /// <param name="id">The primary key of the pricelistversion</param>
        /// <returns>The pricelistversion data</returns>
        PriceListVersion GetById(Guid id);

        /// <summary>Retrieves a list of pricelistversions based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of pricelistversions</returns>
        List<PriceListVersion> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc");

        /// <summary>Adds a new pricelistversion</summary>
        /// <param name="model">The pricelistversion data to be added</param>
        /// <returns>The result of the operation</returns>
        Guid Create(PriceListVersion model);

        /// <summary>Updates a specific pricelistversion by its primary key</summary>
        /// <param name="id">The primary key of the pricelistversion</param>
        /// <param name="updatedEntity">The pricelistversion data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Update(Guid id, PriceListVersion updatedEntity);

        /// <summary>Updates a specific pricelistversion by its primary key</summary>
        /// <param name="id">The primary key of the pricelistversion</param>
        /// <param name="updatedEntity">The pricelistversion data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Patch(Guid id, JsonPatchDocument<PriceListVersion> updatedEntity);

        /// <summary>Deletes a specific pricelistversion by its primary key</summary>
        /// <param name="id">The primary key of the pricelistversion</param>
        /// <returns>The result of the operation</returns>
        bool Delete(Guid id);
    }

    /// <summary>
    /// The pricelistversionService responsible for managing pricelistversion related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting pricelistversion information.
    /// </remarks>
    public class PriceListVersionService : IPriceListVersionService
    {
        private Demo17SepContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the PriceListVersion class.
        /// </summary>
        /// <param name="dbContext">dbContext value to set.</param>
        public PriceListVersionService(Demo17SepContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>Retrieves a specific pricelistversion by its primary key</summary>
        /// <param name="id">The primary key of the pricelistversion</param>
        /// <returns>The pricelistversion data</returns>
        public PriceListVersion GetById(Guid id)
        {
            var entityData = _dbContext.PriceListVersion.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            return entityData;
        }

        /// <summary>Retrieves a list of pricelistversions based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of pricelistversions</returns>/// <exception cref="Exception"></exception>
        public List<PriceListVersion> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            var result = GetPriceListVersion(filters, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return result;
        }

        /// <summary>Adds a new pricelistversion</summary>
        /// <param name="model">The pricelistversion data to be added</param>
        /// <returns>The result of the operation</returns>
        public Guid Create(PriceListVersion model)
        {
            model.Id = CreatePriceListVersion(model);
            return model.Id;
        }

        /// <summary>Updates a specific pricelistversion by its primary key</summary>
        /// <param name="id">The primary key of the pricelistversion</param>
        /// <param name="updatedEntity">The pricelistversion data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Update(Guid id, PriceListVersion updatedEntity)
        {
            UpdatePriceListVersion(id, updatedEntity);
            return true;
        }

        /// <summary>Updates a specific pricelistversion by its primary key</summary>
        /// <param name="id">The primary key of the pricelistversion</param>
        /// <param name="updatedEntity">The pricelistversion data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Patch(Guid id, JsonPatchDocument<PriceListVersion> updatedEntity)
        {
            PatchPriceListVersion(id, updatedEntity);
            return true;
        }

        /// <summary>Deletes a specific pricelistversion by its primary key</summary>
        /// <param name="id">The primary key of the pricelistversion</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Delete(Guid id)
        {
            DeletePriceListVersion(id);
            return true;
        }
        #region
        private List<PriceListVersion> GetPriceListVersion(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            if (pageSize < 1)
            {
                throw new ApplicationException("Page size invalid!");
            }

            if (pageNumber < 1)
            {
                throw new ApplicationException("Page mumber invalid!");
            }

            var query = _dbContext.PriceListVersion.IncludeRelated().AsQueryable();
            int skip = (pageNumber - 1) * pageSize;
            var result = FilterService<PriceListVersion>.ApplyFilter(query, filters, searchTerm);
            if (!string.IsNullOrEmpty(sortField))
            {
                var parameter = Expression.Parameter(typeof(PriceListVersion), "b");
                var property = Expression.Property(parameter, sortField);
                var lambda = Expression.Lambda<Func<PriceListVersion, object>>(Expression.Convert(property, typeof(object)), parameter);
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

        private Guid CreatePriceListVersion(PriceListVersion model)
        {
            _dbContext.PriceListVersion.Add(model);
            _dbContext.SaveChanges();
            return model.Id;
        }

        private void UpdatePriceListVersion(Guid id, PriceListVersion updatedEntity)
        {
            _dbContext.PriceListVersion.Update(updatedEntity);
            _dbContext.SaveChanges();
        }

        private bool DeletePriceListVersion(Guid id)
        {
            var entityData = _dbContext.PriceListVersion.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            if (entityData == null)
            {
                throw new ApplicationException("No data found!");
            }

            _dbContext.PriceListVersion.Remove(entityData);
            _dbContext.SaveChanges();
            return true;
        }

        private void PatchPriceListVersion(Guid id, JsonPatchDocument<PriceListVersion> updatedEntity)
        {
            if (updatedEntity == null)
            {
                throw new ApplicationException("Patch document is missing!");
            }

            var existingEntity = _dbContext.PriceListVersion.FirstOrDefault(t => t.Id == id);
            if (existingEntity == null)
            {
                throw new ApplicationException("No data found!");
            }

            updatedEntity.ApplyTo(existingEntity);
            _dbContext.PriceListVersion.Update(existingEntity);
            _dbContext.SaveChanges();
        }
        #endregion
    }
}