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
    /// The pricelistitemService responsible for managing pricelistitem related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting pricelistitem information.
    /// </remarks>
    public interface IPriceListItemService
    {
        /// <summary>Retrieves a specific pricelistitem by its primary key</summary>
        /// <param name="id">The primary key of the pricelistitem</param>
        /// <returns>The pricelistitem data</returns>
        PriceListItem GetById(Guid id);

        /// <summary>Retrieves a list of pricelistitems based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of pricelistitems</returns>
        List<PriceListItem> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc");

        /// <summary>Adds a new pricelistitem</summary>
        /// <param name="model">The pricelistitem data to be added</param>
        /// <returns>The result of the operation</returns>
        Guid Create(PriceListItem model);

        /// <summary>Updates a specific pricelistitem by its primary key</summary>
        /// <param name="id">The primary key of the pricelistitem</param>
        /// <param name="updatedEntity">The pricelistitem data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Update(Guid id, PriceListItem updatedEntity);

        /// <summary>Updates a specific pricelistitem by its primary key</summary>
        /// <param name="id">The primary key of the pricelistitem</param>
        /// <param name="updatedEntity">The pricelistitem data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Patch(Guid id, JsonPatchDocument<PriceListItem> updatedEntity);

        /// <summary>Deletes a specific pricelistitem by its primary key</summary>
        /// <param name="id">The primary key of the pricelistitem</param>
        /// <returns>The result of the operation</returns>
        bool Delete(Guid id);
    }

    /// <summary>
    /// The pricelistitemService responsible for managing pricelistitem related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting pricelistitem information.
    /// </remarks>
    public class PriceListItemService : IPriceListItemService
    {
        private Demo17SepContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the PriceListItem class.
        /// </summary>
        /// <param name="dbContext">dbContext value to set.</param>
        public PriceListItemService(Demo17SepContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>Retrieves a specific pricelistitem by its primary key</summary>
        /// <param name="id">The primary key of the pricelistitem</param>
        /// <returns>The pricelistitem data</returns>
        public PriceListItem GetById(Guid id)
        {
            var entityData = _dbContext.PriceListItem.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            return entityData;
        }

        /// <summary>Retrieves a list of pricelistitems based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of pricelistitems</returns>/// <exception cref="Exception"></exception>
        public List<PriceListItem> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            var result = GetPriceListItem(filters, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return result;
        }

        /// <summary>Adds a new pricelistitem</summary>
        /// <param name="model">The pricelistitem data to be added</param>
        /// <returns>The result of the operation</returns>
        public Guid Create(PriceListItem model)
        {
            model.Id = CreatePriceListItem(model);
            return model.Id;
        }

        /// <summary>Updates a specific pricelistitem by its primary key</summary>
        /// <param name="id">The primary key of the pricelistitem</param>
        /// <param name="updatedEntity">The pricelistitem data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Update(Guid id, PriceListItem updatedEntity)
        {
            UpdatePriceListItem(id, updatedEntity);
            return true;
        }

        /// <summary>Updates a specific pricelistitem by its primary key</summary>
        /// <param name="id">The primary key of the pricelistitem</param>
        /// <param name="updatedEntity">The pricelistitem data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Patch(Guid id, JsonPatchDocument<PriceListItem> updatedEntity)
        {
            PatchPriceListItem(id, updatedEntity);
            return true;
        }

        /// <summary>Deletes a specific pricelistitem by its primary key</summary>
        /// <param name="id">The primary key of the pricelistitem</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Delete(Guid id)
        {
            DeletePriceListItem(id);
            return true;
        }
        #region
        private List<PriceListItem> GetPriceListItem(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            if (pageSize < 1)
            {
                throw new ApplicationException("Page size invalid!");
            }

            if (pageNumber < 1)
            {
                throw new ApplicationException("Page mumber invalid!");
            }

            var query = _dbContext.PriceListItem.IncludeRelated().AsQueryable();
            int skip = (pageNumber - 1) * pageSize;
            var result = FilterService<PriceListItem>.ApplyFilter(query, filters, searchTerm);
            if (!string.IsNullOrEmpty(sortField))
            {
                var parameter = Expression.Parameter(typeof(PriceListItem), "b");
                var property = Expression.Property(parameter, sortField);
                var lambda = Expression.Lambda<Func<PriceListItem, object>>(Expression.Convert(property, typeof(object)), parameter);
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

        private Guid CreatePriceListItem(PriceListItem model)
        {
            _dbContext.PriceListItem.Add(model);
            _dbContext.SaveChanges();
            return model.Id;
        }

        private void UpdatePriceListItem(Guid id, PriceListItem updatedEntity)
        {
            _dbContext.PriceListItem.Update(updatedEntity);
            _dbContext.SaveChanges();
        }

        private bool DeletePriceListItem(Guid id)
        {
            var entityData = _dbContext.PriceListItem.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            if (entityData == null)
            {
                throw new ApplicationException("No data found!");
            }

            _dbContext.PriceListItem.Remove(entityData);
            _dbContext.SaveChanges();
            return true;
        }

        private void PatchPriceListItem(Guid id, JsonPatchDocument<PriceListItem> updatedEntity)
        {
            if (updatedEntity == null)
            {
                throw new ApplicationException("Patch document is missing!");
            }

            var existingEntity = _dbContext.PriceListItem.FirstOrDefault(t => t.Id == id);
            if (existingEntity == null)
            {
                throw new ApplicationException("No data found!");
            }

            updatedEntity.ApplyTo(existingEntity);
            _dbContext.PriceListItem.Update(existingEntity);
            _dbContext.SaveChanges();
        }
        #endregion
    }
}