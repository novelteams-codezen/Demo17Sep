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
    /// The goodsreceiptactivityhistoryService responsible for managing goodsreceiptactivityhistory related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting goodsreceiptactivityhistory information.
    /// </remarks>
    public interface IGoodsReceiptActivityHistoryService
    {
        /// <summary>Retrieves a specific goodsreceiptactivityhistory by its primary key</summary>
        /// <param name="id">The primary key of the goodsreceiptactivityhistory</param>
        /// <returns>The goodsreceiptactivityhistory data</returns>
        GoodsReceiptActivityHistory GetById(Guid id);

        /// <summary>Retrieves a list of goodsreceiptactivityhistorys based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of goodsreceiptactivityhistorys</returns>
        List<GoodsReceiptActivityHistory> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc");

        /// <summary>Adds a new goodsreceiptactivityhistory</summary>
        /// <param name="model">The goodsreceiptactivityhistory data to be added</param>
        /// <returns>The result of the operation</returns>
        Guid Create(GoodsReceiptActivityHistory model);

        /// <summary>Updates a specific goodsreceiptactivityhistory by its primary key</summary>
        /// <param name="id">The primary key of the goodsreceiptactivityhistory</param>
        /// <param name="updatedEntity">The goodsreceiptactivityhistory data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Update(Guid id, GoodsReceiptActivityHistory updatedEntity);

        /// <summary>Updates a specific goodsreceiptactivityhistory by its primary key</summary>
        /// <param name="id">The primary key of the goodsreceiptactivityhistory</param>
        /// <param name="updatedEntity">The goodsreceiptactivityhistory data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Patch(Guid id, JsonPatchDocument<GoodsReceiptActivityHistory> updatedEntity);

        /// <summary>Deletes a specific goodsreceiptactivityhistory by its primary key</summary>
        /// <param name="id">The primary key of the goodsreceiptactivityhistory</param>
        /// <returns>The result of the operation</returns>
        bool Delete(Guid id);
    }

    /// <summary>
    /// The goodsreceiptactivityhistoryService responsible for managing goodsreceiptactivityhistory related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting goodsreceiptactivityhistory information.
    /// </remarks>
    public class GoodsReceiptActivityHistoryService : IGoodsReceiptActivityHistoryService
    {
        private Demo17SepContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the GoodsReceiptActivityHistory class.
        /// </summary>
        /// <param name="dbContext">dbContext value to set.</param>
        public GoodsReceiptActivityHistoryService(Demo17SepContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>Retrieves a specific goodsreceiptactivityhistory by its primary key</summary>
        /// <param name="id">The primary key of the goodsreceiptactivityhistory</param>
        /// <returns>The goodsreceiptactivityhistory data</returns>
        public GoodsReceiptActivityHistory GetById(Guid id)
        {
            var entityData = _dbContext.GoodsReceiptActivityHistory.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            return entityData;
        }

        /// <summary>Retrieves a list of goodsreceiptactivityhistorys based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of goodsreceiptactivityhistorys</returns>/// <exception cref="Exception"></exception>
        public List<GoodsReceiptActivityHistory> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            var result = GetGoodsReceiptActivityHistory(filters, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return result;
        }

        /// <summary>Adds a new goodsreceiptactivityhistory</summary>
        /// <param name="model">The goodsreceiptactivityhistory data to be added</param>
        /// <returns>The result of the operation</returns>
        public Guid Create(GoodsReceiptActivityHistory model)
        {
            model.Id = CreateGoodsReceiptActivityHistory(model);
            return model.Id;
        }

        /// <summary>Updates a specific goodsreceiptactivityhistory by its primary key</summary>
        /// <param name="id">The primary key of the goodsreceiptactivityhistory</param>
        /// <param name="updatedEntity">The goodsreceiptactivityhistory data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Update(Guid id, GoodsReceiptActivityHistory updatedEntity)
        {
            UpdateGoodsReceiptActivityHistory(id, updatedEntity);
            return true;
        }

        /// <summary>Updates a specific goodsreceiptactivityhistory by its primary key</summary>
        /// <param name="id">The primary key of the goodsreceiptactivityhistory</param>
        /// <param name="updatedEntity">The goodsreceiptactivityhistory data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Patch(Guid id, JsonPatchDocument<GoodsReceiptActivityHistory> updatedEntity)
        {
            PatchGoodsReceiptActivityHistory(id, updatedEntity);
            return true;
        }

        /// <summary>Deletes a specific goodsreceiptactivityhistory by its primary key</summary>
        /// <param name="id">The primary key of the goodsreceiptactivityhistory</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Delete(Guid id)
        {
            DeleteGoodsReceiptActivityHistory(id);
            return true;
        }
        #region
        private List<GoodsReceiptActivityHistory> GetGoodsReceiptActivityHistory(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            if (pageSize < 1)
            {
                throw new ApplicationException("Page size invalid!");
            }

            if (pageNumber < 1)
            {
                throw new ApplicationException("Page mumber invalid!");
            }

            var query = _dbContext.GoodsReceiptActivityHistory.IncludeRelated().AsQueryable();
            int skip = (pageNumber - 1) * pageSize;
            var result = FilterService<GoodsReceiptActivityHistory>.ApplyFilter(query, filters, searchTerm);
            if (!string.IsNullOrEmpty(sortField))
            {
                var parameter = Expression.Parameter(typeof(GoodsReceiptActivityHistory), "b");
                var property = Expression.Property(parameter, sortField);
                var lambda = Expression.Lambda<Func<GoodsReceiptActivityHistory, object>>(Expression.Convert(property, typeof(object)), parameter);
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

        private Guid CreateGoodsReceiptActivityHistory(GoodsReceiptActivityHistory model)
        {
            _dbContext.GoodsReceiptActivityHistory.Add(model);
            _dbContext.SaveChanges();
            return model.Id;
        }

        private void UpdateGoodsReceiptActivityHistory(Guid id, GoodsReceiptActivityHistory updatedEntity)
        {
            _dbContext.GoodsReceiptActivityHistory.Update(updatedEntity);
            _dbContext.SaveChanges();
        }

        private bool DeleteGoodsReceiptActivityHistory(Guid id)
        {
            var entityData = _dbContext.GoodsReceiptActivityHistory.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            if (entityData == null)
            {
                throw new ApplicationException("No data found!");
            }

            _dbContext.GoodsReceiptActivityHistory.Remove(entityData);
            _dbContext.SaveChanges();
            return true;
        }

        private void PatchGoodsReceiptActivityHistory(Guid id, JsonPatchDocument<GoodsReceiptActivityHistory> updatedEntity)
        {
            if (updatedEntity == null)
            {
                throw new ApplicationException("Patch document is missing!");
            }

            var existingEntity = _dbContext.GoodsReceiptActivityHistory.FirstOrDefault(t => t.Id == id);
            if (existingEntity == null)
            {
                throw new ApplicationException("No data found!");
            }

            updatedEntity.ApplyTo(existingEntity);
            _dbContext.GoodsReceiptActivityHistory.Update(existingEntity);
            _dbContext.SaveChanges();
        }
        #endregion
    }
}