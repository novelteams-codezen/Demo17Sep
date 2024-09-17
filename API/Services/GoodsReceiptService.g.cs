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
    /// The goodsreceiptService responsible for managing goodsreceipt related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting goodsreceipt information.
    /// </remarks>
    public interface IGoodsReceiptService
    {
        /// <summary>Retrieves a specific goodsreceipt by its primary key</summary>
        /// <param name="id">The primary key of the goodsreceipt</param>
        /// <returns>The goodsreceipt data</returns>
        GoodsReceipt GetById(Guid id);

        /// <summary>Retrieves a list of goodsreceipts based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of goodsreceipts</returns>
        List<GoodsReceipt> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc");

        /// <summary>Adds a new goodsreceipt</summary>
        /// <param name="model">The goodsreceipt data to be added</param>
        /// <returns>The result of the operation</returns>
        Guid Create(GoodsReceipt model);

        /// <summary>Updates a specific goodsreceipt by its primary key</summary>
        /// <param name="id">The primary key of the goodsreceipt</param>
        /// <param name="updatedEntity">The goodsreceipt data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Update(Guid id, GoodsReceipt updatedEntity);

        /// <summary>Updates a specific goodsreceipt by its primary key</summary>
        /// <param name="id">The primary key of the goodsreceipt</param>
        /// <param name="updatedEntity">The goodsreceipt data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Patch(Guid id, JsonPatchDocument<GoodsReceipt> updatedEntity);

        /// <summary>Deletes a specific goodsreceipt by its primary key</summary>
        /// <param name="id">The primary key of the goodsreceipt</param>
        /// <returns>The result of the operation</returns>
        bool Delete(Guid id);
    }

    /// <summary>
    /// The goodsreceiptService responsible for managing goodsreceipt related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting goodsreceipt information.
    /// </remarks>
    public class GoodsReceiptService : IGoodsReceiptService
    {
        private Demo17SepContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the GoodsReceipt class.
        /// </summary>
        /// <param name="dbContext">dbContext value to set.</param>
        public GoodsReceiptService(Demo17SepContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>Retrieves a specific goodsreceipt by its primary key</summary>
        /// <param name="id">The primary key of the goodsreceipt</param>
        /// <returns>The goodsreceipt data</returns>
        public GoodsReceipt GetById(Guid id)
        {
            var entityData = _dbContext.GoodsReceipt.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            return entityData;
        }

        /// <summary>Retrieves a list of goodsreceipts based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of goodsreceipts</returns>/// <exception cref="Exception"></exception>
        public List<GoodsReceipt> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            var result = GetGoodsReceipt(filters, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return result;
        }

        /// <summary>Adds a new goodsreceipt</summary>
        /// <param name="model">The goodsreceipt data to be added</param>
        /// <returns>The result of the operation</returns>
        public Guid Create(GoodsReceipt model)
        {
            model.Id = CreateGoodsReceipt(model);
            return model.Id;
        }

        /// <summary>Updates a specific goodsreceipt by its primary key</summary>
        /// <param name="id">The primary key of the goodsreceipt</param>
        /// <param name="updatedEntity">The goodsreceipt data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Update(Guid id, GoodsReceipt updatedEntity)
        {
            UpdateGoodsReceipt(id, updatedEntity);
            return true;
        }

        /// <summary>Updates a specific goodsreceipt by its primary key</summary>
        /// <param name="id">The primary key of the goodsreceipt</param>
        /// <param name="updatedEntity">The goodsreceipt data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Patch(Guid id, JsonPatchDocument<GoodsReceipt> updatedEntity)
        {
            PatchGoodsReceipt(id, updatedEntity);
            return true;
        }

        /// <summary>Deletes a specific goodsreceipt by its primary key</summary>
        /// <param name="id">The primary key of the goodsreceipt</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Delete(Guid id)
        {
            DeleteGoodsReceipt(id);
            return true;
        }
        #region
        private List<GoodsReceipt> GetGoodsReceipt(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            if (pageSize < 1)
            {
                throw new ApplicationException("Page size invalid!");
            }

            if (pageNumber < 1)
            {
                throw new ApplicationException("Page mumber invalid!");
            }

            var query = _dbContext.GoodsReceipt.IncludeRelated().AsQueryable();
            int skip = (pageNumber - 1) * pageSize;
            var result = FilterService<GoodsReceipt>.ApplyFilter(query, filters, searchTerm);
            if (!string.IsNullOrEmpty(sortField))
            {
                var parameter = Expression.Parameter(typeof(GoodsReceipt), "b");
                var property = Expression.Property(parameter, sortField);
                var lambda = Expression.Lambda<Func<GoodsReceipt, object>>(Expression.Convert(property, typeof(object)), parameter);
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

        private Guid CreateGoodsReceipt(GoodsReceipt model)
        {
            _dbContext.GoodsReceipt.Add(model);
            _dbContext.SaveChanges();
            return model.Id;
        }

        private void UpdateGoodsReceipt(Guid id, GoodsReceipt updatedEntity)
        {
            _dbContext.GoodsReceipt.Update(updatedEntity);
            _dbContext.SaveChanges();
        }

        private bool DeleteGoodsReceipt(Guid id)
        {
            var entityData = _dbContext.GoodsReceipt.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            if (entityData == null)
            {
                throw new ApplicationException("No data found!");
            }

            _dbContext.GoodsReceipt.Remove(entityData);
            _dbContext.SaveChanges();
            return true;
        }

        private void PatchGoodsReceipt(Guid id, JsonPatchDocument<GoodsReceipt> updatedEntity)
        {
            if (updatedEntity == null)
            {
                throw new ApplicationException("Patch document is missing!");
            }

            var existingEntity = _dbContext.GoodsReceipt.FirstOrDefault(t => t.Id == id);
            if (existingEntity == null)
            {
                throw new ApplicationException("No data found!");
            }

            updatedEntity.ApplyTo(existingEntity);
            _dbContext.GoodsReceipt.Update(existingEntity);
            _dbContext.SaveChanges();
        }
        #endregion
    }
}