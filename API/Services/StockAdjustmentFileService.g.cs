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
    /// The stockadjustmentfileService responsible for managing stockadjustmentfile related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting stockadjustmentfile information.
    /// </remarks>
    public interface IStockAdjustmentFileService
    {
        /// <summary>Retrieves a specific stockadjustmentfile by its primary key</summary>
        /// <param name="id">The primary key of the stockadjustmentfile</param>
        /// <returns>The stockadjustmentfile data</returns>
        StockAdjustmentFile GetById(Guid id);

        /// <summary>Retrieves a list of stockadjustmentfiles based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of stockadjustmentfiles</returns>
        List<StockAdjustmentFile> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc");

        /// <summary>Adds a new stockadjustmentfile</summary>
        /// <param name="model">The stockadjustmentfile data to be added</param>
        /// <returns>The result of the operation</returns>
        Guid Create(StockAdjustmentFile model);

        /// <summary>Updates a specific stockadjustmentfile by its primary key</summary>
        /// <param name="id">The primary key of the stockadjustmentfile</param>
        /// <param name="updatedEntity">The stockadjustmentfile data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Update(Guid id, StockAdjustmentFile updatedEntity);

        /// <summary>Updates a specific stockadjustmentfile by its primary key</summary>
        /// <param name="id">The primary key of the stockadjustmentfile</param>
        /// <param name="updatedEntity">The stockadjustmentfile data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Patch(Guid id, JsonPatchDocument<StockAdjustmentFile> updatedEntity);

        /// <summary>Deletes a specific stockadjustmentfile by its primary key</summary>
        /// <param name="id">The primary key of the stockadjustmentfile</param>
        /// <returns>The result of the operation</returns>
        bool Delete(Guid id);
    }

    /// <summary>
    /// The stockadjustmentfileService responsible for managing stockadjustmentfile related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting stockadjustmentfile information.
    /// </remarks>
    public class StockAdjustmentFileService : IStockAdjustmentFileService
    {
        private Demo17SepContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the StockAdjustmentFile class.
        /// </summary>
        /// <param name="dbContext">dbContext value to set.</param>
        public StockAdjustmentFileService(Demo17SepContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>Retrieves a specific stockadjustmentfile by its primary key</summary>
        /// <param name="id">The primary key of the stockadjustmentfile</param>
        /// <returns>The stockadjustmentfile data</returns>
        public StockAdjustmentFile GetById(Guid id)
        {
            var entityData = _dbContext.StockAdjustmentFile.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            return entityData;
        }

        /// <summary>Retrieves a list of stockadjustmentfiles based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of stockadjustmentfiles</returns>/// <exception cref="Exception"></exception>
        public List<StockAdjustmentFile> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            var result = GetStockAdjustmentFile(filters, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return result;
        }

        /// <summary>Adds a new stockadjustmentfile</summary>
        /// <param name="model">The stockadjustmentfile data to be added</param>
        /// <returns>The result of the operation</returns>
        public Guid Create(StockAdjustmentFile model)
        {
            model.Id = CreateStockAdjustmentFile(model);
            return model.Id;
        }

        /// <summary>Updates a specific stockadjustmentfile by its primary key</summary>
        /// <param name="id">The primary key of the stockadjustmentfile</param>
        /// <param name="updatedEntity">The stockadjustmentfile data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Update(Guid id, StockAdjustmentFile updatedEntity)
        {
            UpdateStockAdjustmentFile(id, updatedEntity);
            return true;
        }

        /// <summary>Updates a specific stockadjustmentfile by its primary key</summary>
        /// <param name="id">The primary key of the stockadjustmentfile</param>
        /// <param name="updatedEntity">The stockadjustmentfile data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Patch(Guid id, JsonPatchDocument<StockAdjustmentFile> updatedEntity)
        {
            PatchStockAdjustmentFile(id, updatedEntity);
            return true;
        }

        /// <summary>Deletes a specific stockadjustmentfile by its primary key</summary>
        /// <param name="id">The primary key of the stockadjustmentfile</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Delete(Guid id)
        {
            DeleteStockAdjustmentFile(id);
            return true;
        }
        #region
        private List<StockAdjustmentFile> GetStockAdjustmentFile(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            if (pageSize < 1)
            {
                throw new ApplicationException("Page size invalid!");
            }

            if (pageNumber < 1)
            {
                throw new ApplicationException("Page mumber invalid!");
            }

            var query = _dbContext.StockAdjustmentFile.IncludeRelated().AsQueryable();
            int skip = (pageNumber - 1) * pageSize;
            var result = FilterService<StockAdjustmentFile>.ApplyFilter(query, filters, searchTerm);
            if (!string.IsNullOrEmpty(sortField))
            {
                var parameter = Expression.Parameter(typeof(StockAdjustmentFile), "b");
                var property = Expression.Property(parameter, sortField);
                var lambda = Expression.Lambda<Func<StockAdjustmentFile, object>>(Expression.Convert(property, typeof(object)), parameter);
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

        private Guid CreateStockAdjustmentFile(StockAdjustmentFile model)
        {
            _dbContext.StockAdjustmentFile.Add(model);
            _dbContext.SaveChanges();
            return model.Id;
        }

        private void UpdateStockAdjustmentFile(Guid id, StockAdjustmentFile updatedEntity)
        {
            _dbContext.StockAdjustmentFile.Update(updatedEntity);
            _dbContext.SaveChanges();
        }

        private bool DeleteStockAdjustmentFile(Guid id)
        {
            var entityData = _dbContext.StockAdjustmentFile.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            if (entityData == null)
            {
                throw new ApplicationException("No data found!");
            }

            _dbContext.StockAdjustmentFile.Remove(entityData);
            _dbContext.SaveChanges();
            return true;
        }

        private void PatchStockAdjustmentFile(Guid id, JsonPatchDocument<StockAdjustmentFile> updatedEntity)
        {
            if (updatedEntity == null)
            {
                throw new ApplicationException("Patch document is missing!");
            }

            var existingEntity = _dbContext.StockAdjustmentFile.FirstOrDefault(t => t.Id == id);
            if (existingEntity == null)
            {
                throw new ApplicationException("No data found!");
            }

            updatedEntity.ApplyTo(existingEntity);
            _dbContext.StockAdjustmentFile.Update(existingEntity);
            _dbContext.SaveChanges();
        }
        #endregion
    }
}