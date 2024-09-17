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
    /// The goodsreceiptitemService responsible for managing goodsreceiptitem related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting goodsreceiptitem information.
    /// </remarks>
    public interface IGoodsReceiptItemService
    {
        /// <summary>Retrieves a specific goodsreceiptitem by its primary key</summary>
        /// <param name="id">The primary key of the goodsreceiptitem</param>
        /// <returns>The goodsreceiptitem data</returns>
        GoodsReceiptItem GetById(Guid id);

        /// <summary>Retrieves a list of goodsreceiptitems based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of goodsreceiptitems</returns>
        List<GoodsReceiptItem> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc");

        /// <summary>Adds a new goodsreceiptitem</summary>
        /// <param name="model">The goodsreceiptitem data to be added</param>
        /// <returns>The result of the operation</returns>
        Guid Create(GoodsReceiptItem model);

        /// <summary>Updates a specific goodsreceiptitem by its primary key</summary>
        /// <param name="id">The primary key of the goodsreceiptitem</param>
        /// <param name="updatedEntity">The goodsreceiptitem data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Update(Guid id, GoodsReceiptItem updatedEntity);

        /// <summary>Updates a specific goodsreceiptitem by its primary key</summary>
        /// <param name="id">The primary key of the goodsreceiptitem</param>
        /// <param name="updatedEntity">The goodsreceiptitem data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Patch(Guid id, JsonPatchDocument<GoodsReceiptItem> updatedEntity);

        /// <summary>Deletes a specific goodsreceiptitem by its primary key</summary>
        /// <param name="id">The primary key of the goodsreceiptitem</param>
        /// <returns>The result of the operation</returns>
        bool Delete(Guid id);
    }

    /// <summary>
    /// The goodsreceiptitemService responsible for managing goodsreceiptitem related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting goodsreceiptitem information.
    /// </remarks>
    public class GoodsReceiptItemService : IGoodsReceiptItemService
    {
        private Demo17SepContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the GoodsReceiptItem class.
        /// </summary>
        /// <param name="dbContext">dbContext value to set.</param>
        public GoodsReceiptItemService(Demo17SepContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>Retrieves a specific goodsreceiptitem by its primary key</summary>
        /// <param name="id">The primary key of the goodsreceiptitem</param>
        /// <returns>The goodsreceiptitem data</returns>
        public GoodsReceiptItem GetById(Guid id)
        {
            var entityData = _dbContext.GoodsReceiptItem.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            return entityData;
        }

        /// <summary>Retrieves a list of goodsreceiptitems based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of goodsreceiptitems</returns>/// <exception cref="Exception"></exception>
        public List<GoodsReceiptItem> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            var result = GetGoodsReceiptItem(filters, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return result;
        }

        /// <summary>Adds a new goodsreceiptitem</summary>
        /// <param name="model">The goodsreceiptitem data to be added</param>
        /// <returns>The result of the operation</returns>
        public Guid Create(GoodsReceiptItem model)
        {
            model.Id = CreateGoodsReceiptItem(model);
            return model.Id;
        }

        /// <summary>Updates a specific goodsreceiptitem by its primary key</summary>
        /// <param name="id">The primary key of the goodsreceiptitem</param>
        /// <param name="updatedEntity">The goodsreceiptitem data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Update(Guid id, GoodsReceiptItem updatedEntity)
        {
            UpdateGoodsReceiptItem(id, updatedEntity);
            return true;
        }

        /// <summary>Updates a specific goodsreceiptitem by its primary key</summary>
        /// <param name="id">The primary key of the goodsreceiptitem</param>
        /// <param name="updatedEntity">The goodsreceiptitem data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Patch(Guid id, JsonPatchDocument<GoodsReceiptItem> updatedEntity)
        {
            PatchGoodsReceiptItem(id, updatedEntity);
            return true;
        }

        /// <summary>Deletes a specific goodsreceiptitem by its primary key</summary>
        /// <param name="id">The primary key of the goodsreceiptitem</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Delete(Guid id)
        {
            DeleteGoodsReceiptItem(id);
            return true;
        }
        #region
        private List<GoodsReceiptItem> GetGoodsReceiptItem(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            if (pageSize < 1)
            {
                throw new ApplicationException("Page size invalid!");
            }

            if (pageNumber < 1)
            {
                throw new ApplicationException("Page mumber invalid!");
            }

            var query = _dbContext.GoodsReceiptItem.IncludeRelated().AsQueryable();
            int skip = (pageNumber - 1) * pageSize;
            var result = FilterService<GoodsReceiptItem>.ApplyFilter(query, filters, searchTerm);
            if (!string.IsNullOrEmpty(sortField))
            {
                var parameter = Expression.Parameter(typeof(GoodsReceiptItem), "b");
                var property = Expression.Property(parameter, sortField);
                var lambda = Expression.Lambda<Func<GoodsReceiptItem, object>>(Expression.Convert(property, typeof(object)), parameter);
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

        private Guid CreateGoodsReceiptItem(GoodsReceiptItem model)
        {
            _dbContext.GoodsReceiptItem.Add(model);
            _dbContext.SaveChanges();
            return model.Id;
        }

        private void UpdateGoodsReceiptItem(Guid id, GoodsReceiptItem updatedEntity)
        {
            _dbContext.GoodsReceiptItem.Update(updatedEntity);
            _dbContext.SaveChanges();
        }

        private bool DeleteGoodsReceiptItem(Guid id)
        {
            var entityData = _dbContext.GoodsReceiptItem.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            if (entityData == null)
            {
                throw new ApplicationException("No data found!");
            }

            _dbContext.GoodsReceiptItem.Remove(entityData);
            _dbContext.SaveChanges();
            return true;
        }

        private void PatchGoodsReceiptItem(Guid id, JsonPatchDocument<GoodsReceiptItem> updatedEntity)
        {
            if (updatedEntity == null)
            {
                throw new ApplicationException("Patch document is missing!");
            }

            var existingEntity = _dbContext.GoodsReceiptItem.FirstOrDefault(t => t.Id == id);
            if (existingEntity == null)
            {
                throw new ApplicationException("No data found!");
            }

            updatedEntity.ApplyTo(existingEntity);
            _dbContext.GoodsReceiptItem.Update(existingEntity);
            _dbContext.SaveChanges();
        }
        #endregion
    }
}