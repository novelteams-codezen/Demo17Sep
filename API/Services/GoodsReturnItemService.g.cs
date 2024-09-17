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
    /// The goodsreturnitemService responsible for managing goodsreturnitem related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting goodsreturnitem information.
    /// </remarks>
    public interface IGoodsReturnItemService
    {
        /// <summary>Retrieves a specific goodsreturnitem by its primary key</summary>
        /// <param name="id">The primary key of the goodsreturnitem</param>
        /// <returns>The goodsreturnitem data</returns>
        GoodsReturnItem GetById(Guid id);

        /// <summary>Retrieves a list of goodsreturnitems based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of goodsreturnitems</returns>
        List<GoodsReturnItem> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc");

        /// <summary>Adds a new goodsreturnitem</summary>
        /// <param name="model">The goodsreturnitem data to be added</param>
        /// <returns>The result of the operation</returns>
        Guid Create(GoodsReturnItem model);

        /// <summary>Updates a specific goodsreturnitem by its primary key</summary>
        /// <param name="id">The primary key of the goodsreturnitem</param>
        /// <param name="updatedEntity">The goodsreturnitem data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Update(Guid id, GoodsReturnItem updatedEntity);

        /// <summary>Updates a specific goodsreturnitem by its primary key</summary>
        /// <param name="id">The primary key of the goodsreturnitem</param>
        /// <param name="updatedEntity">The goodsreturnitem data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Patch(Guid id, JsonPatchDocument<GoodsReturnItem> updatedEntity);

        /// <summary>Deletes a specific goodsreturnitem by its primary key</summary>
        /// <param name="id">The primary key of the goodsreturnitem</param>
        /// <returns>The result of the operation</returns>
        bool Delete(Guid id);
    }

    /// <summary>
    /// The goodsreturnitemService responsible for managing goodsreturnitem related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting goodsreturnitem information.
    /// </remarks>
    public class GoodsReturnItemService : IGoodsReturnItemService
    {
        private Demo17SepContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the GoodsReturnItem class.
        /// </summary>
        /// <param name="dbContext">dbContext value to set.</param>
        public GoodsReturnItemService(Demo17SepContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>Retrieves a specific goodsreturnitem by its primary key</summary>
        /// <param name="id">The primary key of the goodsreturnitem</param>
        /// <returns>The goodsreturnitem data</returns>
        public GoodsReturnItem GetById(Guid id)
        {
            var entityData = _dbContext.GoodsReturnItem.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            return entityData;
        }

        /// <summary>Retrieves a list of goodsreturnitems based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of goodsreturnitems</returns>/// <exception cref="Exception"></exception>
        public List<GoodsReturnItem> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            var result = GetGoodsReturnItem(filters, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return result;
        }

        /// <summary>Adds a new goodsreturnitem</summary>
        /// <param name="model">The goodsreturnitem data to be added</param>
        /// <returns>The result of the operation</returns>
        public Guid Create(GoodsReturnItem model)
        {
            model.Id = CreateGoodsReturnItem(model);
            return model.Id;
        }

        /// <summary>Updates a specific goodsreturnitem by its primary key</summary>
        /// <param name="id">The primary key of the goodsreturnitem</param>
        /// <param name="updatedEntity">The goodsreturnitem data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Update(Guid id, GoodsReturnItem updatedEntity)
        {
            UpdateGoodsReturnItem(id, updatedEntity);
            return true;
        }

        /// <summary>Updates a specific goodsreturnitem by its primary key</summary>
        /// <param name="id">The primary key of the goodsreturnitem</param>
        /// <param name="updatedEntity">The goodsreturnitem data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Patch(Guid id, JsonPatchDocument<GoodsReturnItem> updatedEntity)
        {
            PatchGoodsReturnItem(id, updatedEntity);
            return true;
        }

        /// <summary>Deletes a specific goodsreturnitem by its primary key</summary>
        /// <param name="id">The primary key of the goodsreturnitem</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Delete(Guid id)
        {
            DeleteGoodsReturnItem(id);
            return true;
        }
        #region
        private List<GoodsReturnItem> GetGoodsReturnItem(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            if (pageSize < 1)
            {
                throw new ApplicationException("Page size invalid!");
            }

            if (pageNumber < 1)
            {
                throw new ApplicationException("Page mumber invalid!");
            }

            var query = _dbContext.GoodsReturnItem.IncludeRelated().AsQueryable();
            int skip = (pageNumber - 1) * pageSize;
            var result = FilterService<GoodsReturnItem>.ApplyFilter(query, filters, searchTerm);
            if (!string.IsNullOrEmpty(sortField))
            {
                var parameter = Expression.Parameter(typeof(GoodsReturnItem), "b");
                var property = Expression.Property(parameter, sortField);
                var lambda = Expression.Lambda<Func<GoodsReturnItem, object>>(Expression.Convert(property, typeof(object)), parameter);
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

        private Guid CreateGoodsReturnItem(GoodsReturnItem model)
        {
            _dbContext.GoodsReturnItem.Add(model);
            _dbContext.SaveChanges();
            return model.Id;
        }

        private void UpdateGoodsReturnItem(Guid id, GoodsReturnItem updatedEntity)
        {
            _dbContext.GoodsReturnItem.Update(updatedEntity);
            _dbContext.SaveChanges();
        }

        private bool DeleteGoodsReturnItem(Guid id)
        {
            var entityData = _dbContext.GoodsReturnItem.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            if (entityData == null)
            {
                throw new ApplicationException("No data found!");
            }

            _dbContext.GoodsReturnItem.Remove(entityData);
            _dbContext.SaveChanges();
            return true;
        }

        private void PatchGoodsReturnItem(Guid id, JsonPatchDocument<GoodsReturnItem> updatedEntity)
        {
            if (updatedEntity == null)
            {
                throw new ApplicationException("Patch document is missing!");
            }

            var existingEntity = _dbContext.GoodsReturnItem.FirstOrDefault(t => t.Id == id);
            if (existingEntity == null)
            {
                throw new ApplicationException("No data found!");
            }

            updatedEntity.ApplyTo(existingEntity);
            _dbContext.GoodsReturnItem.Update(existingEntity);
            _dbContext.SaveChanges();
        }
        #endregion
    }
}