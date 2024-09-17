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
    /// The pricelistService responsible for managing pricelist related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting pricelist information.
    /// </remarks>
    public interface IPriceListService
    {
        /// <summary>Retrieves a specific pricelist by its primary key</summary>
        /// <param name="id">The primary key of the pricelist</param>
        /// <returns>The pricelist data</returns>
        PriceList GetById(Guid id);

        /// <summary>Retrieves a list of pricelists based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of pricelists</returns>
        List<PriceList> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc");

        /// <summary>Adds a new pricelist</summary>
        /// <param name="model">The pricelist data to be added</param>
        /// <returns>The result of the operation</returns>
        Guid Create(PriceList model);

        /// <summary>Updates a specific pricelist by its primary key</summary>
        /// <param name="id">The primary key of the pricelist</param>
        /// <param name="updatedEntity">The pricelist data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Update(Guid id, PriceList updatedEntity);

        /// <summary>Updates a specific pricelist by its primary key</summary>
        /// <param name="id">The primary key of the pricelist</param>
        /// <param name="updatedEntity">The pricelist data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Patch(Guid id, JsonPatchDocument<PriceList> updatedEntity);

        /// <summary>Deletes a specific pricelist by its primary key</summary>
        /// <param name="id">The primary key of the pricelist</param>
        /// <returns>The result of the operation</returns>
        bool Delete(Guid id);
    }

    /// <summary>
    /// The pricelistService responsible for managing pricelist related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting pricelist information.
    /// </remarks>
    public class PriceListService : IPriceListService
    {
        private Demo17SepContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the PriceList class.
        /// </summary>
        /// <param name="dbContext">dbContext value to set.</param>
        public PriceListService(Demo17SepContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>Retrieves a specific pricelist by its primary key</summary>
        /// <param name="id">The primary key of the pricelist</param>
        /// <returns>The pricelist data</returns>
        public PriceList GetById(Guid id)
        {
            var entityData = _dbContext.PriceList.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            return entityData;
        }

        /// <summary>Retrieves a list of pricelists based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of pricelists</returns>/// <exception cref="Exception"></exception>
        public List<PriceList> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            var result = GetPriceList(filters, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return result;
        }

        /// <summary>Adds a new pricelist</summary>
        /// <param name="model">The pricelist data to be added</param>
        /// <returns>The result of the operation</returns>
        public Guid Create(PriceList model)
        {
            model.Id = CreatePriceList(model);
            return model.Id;
        }

        /// <summary>Updates a specific pricelist by its primary key</summary>
        /// <param name="id">The primary key of the pricelist</param>
        /// <param name="updatedEntity">The pricelist data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Update(Guid id, PriceList updatedEntity)
        {
            UpdatePriceList(id, updatedEntity);
            return true;
        }

        /// <summary>Updates a specific pricelist by its primary key</summary>
        /// <param name="id">The primary key of the pricelist</param>
        /// <param name="updatedEntity">The pricelist data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Patch(Guid id, JsonPatchDocument<PriceList> updatedEntity)
        {
            PatchPriceList(id, updatedEntity);
            return true;
        }

        /// <summary>Deletes a specific pricelist by its primary key</summary>
        /// <param name="id">The primary key of the pricelist</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Delete(Guid id)
        {
            DeletePriceList(id);
            return true;
        }
        #region
        private List<PriceList> GetPriceList(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            if (pageSize < 1)
            {
                throw new ApplicationException("Page size invalid!");
            }

            if (pageNumber < 1)
            {
                throw new ApplicationException("Page mumber invalid!");
            }

            var query = _dbContext.PriceList.IncludeRelated().AsQueryable();
            int skip = (pageNumber - 1) * pageSize;
            var result = FilterService<PriceList>.ApplyFilter(query, filters, searchTerm);
            if (!string.IsNullOrEmpty(sortField))
            {
                var parameter = Expression.Parameter(typeof(PriceList), "b");
                var property = Expression.Property(parameter, sortField);
                var lambda = Expression.Lambda<Func<PriceList, object>>(Expression.Convert(property, typeof(object)), parameter);
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

        private Guid CreatePriceList(PriceList model)
        {
            _dbContext.PriceList.Add(model);
            _dbContext.SaveChanges();
            return model.Id;
        }

        private void UpdatePriceList(Guid id, PriceList updatedEntity)
        {
            _dbContext.PriceList.Update(updatedEntity);
            _dbContext.SaveChanges();
        }

        private bool DeletePriceList(Guid id)
        {
            var entityData = _dbContext.PriceList.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            if (entityData == null)
            {
                throw new ApplicationException("No data found!");
            }

            _dbContext.PriceList.Remove(entityData);
            _dbContext.SaveChanges();
            return true;
        }

        private void PatchPriceList(Guid id, JsonPatchDocument<PriceList> updatedEntity)
        {
            if (updatedEntity == null)
            {
                throw new ApplicationException("Patch document is missing!");
            }

            var existingEntity = _dbContext.PriceList.FirstOrDefault(t => t.Id == id);
            if (existingEntity == null)
            {
                throw new ApplicationException("No data found!");
            }

            updatedEntity.ApplyTo(existingEntity);
            _dbContext.PriceList.Update(existingEntity);
            _dbContext.SaveChanges();
        }
        #endregion
    }
}