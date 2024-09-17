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
    /// The goodsreceiptpurchaseorderrelationService responsible for managing goodsreceiptpurchaseorderrelation related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting goodsreceiptpurchaseorderrelation information.
    /// </remarks>
    public interface IGoodsReceiptPurchaseOrderRelationService
    {
        /// <summary>Retrieves a specific goodsreceiptpurchaseorderrelation by its primary key</summary>
        /// <param name="id">The primary key of the goodsreceiptpurchaseorderrelation</param>
        /// <returns>The goodsreceiptpurchaseorderrelation data</returns>
        GoodsReceiptPurchaseOrderRelation GetById(Guid id);

        /// <summary>Retrieves a list of goodsreceiptpurchaseorderrelations based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of goodsreceiptpurchaseorderrelations</returns>
        List<GoodsReceiptPurchaseOrderRelation> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc");

        /// <summary>Adds a new goodsreceiptpurchaseorderrelation</summary>
        /// <param name="model">The goodsreceiptpurchaseorderrelation data to be added</param>
        /// <returns>The result of the operation</returns>
        Guid Create(GoodsReceiptPurchaseOrderRelation model);

        /// <summary>Updates a specific goodsreceiptpurchaseorderrelation by its primary key</summary>
        /// <param name="id">The primary key of the goodsreceiptpurchaseorderrelation</param>
        /// <param name="updatedEntity">The goodsreceiptpurchaseorderrelation data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Update(Guid id, GoodsReceiptPurchaseOrderRelation updatedEntity);

        /// <summary>Updates a specific goodsreceiptpurchaseorderrelation by its primary key</summary>
        /// <param name="id">The primary key of the goodsreceiptpurchaseorderrelation</param>
        /// <param name="updatedEntity">The goodsreceiptpurchaseorderrelation data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Patch(Guid id, JsonPatchDocument<GoodsReceiptPurchaseOrderRelation> updatedEntity);

        /// <summary>Deletes a specific goodsreceiptpurchaseorderrelation by its primary key</summary>
        /// <param name="id">The primary key of the goodsreceiptpurchaseorderrelation</param>
        /// <returns>The result of the operation</returns>
        bool Delete(Guid id);
    }

    /// <summary>
    /// The goodsreceiptpurchaseorderrelationService responsible for managing goodsreceiptpurchaseorderrelation related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting goodsreceiptpurchaseorderrelation information.
    /// </remarks>
    public class GoodsReceiptPurchaseOrderRelationService : IGoodsReceiptPurchaseOrderRelationService
    {
        private Demo17SepContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the GoodsReceiptPurchaseOrderRelation class.
        /// </summary>
        /// <param name="dbContext">dbContext value to set.</param>
        public GoodsReceiptPurchaseOrderRelationService(Demo17SepContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>Retrieves a specific goodsreceiptpurchaseorderrelation by its primary key</summary>
        /// <param name="id">The primary key of the goodsreceiptpurchaseorderrelation</param>
        /// <returns>The goodsreceiptpurchaseorderrelation data</returns>
        public GoodsReceiptPurchaseOrderRelation GetById(Guid id)
        {
            var entityData = _dbContext.GoodsReceiptPurchaseOrderRelation.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            return entityData;
        }

        /// <summary>Retrieves a list of goodsreceiptpurchaseorderrelations based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of goodsreceiptpurchaseorderrelations</returns>/// <exception cref="Exception"></exception>
        public List<GoodsReceiptPurchaseOrderRelation> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            var result = GetGoodsReceiptPurchaseOrderRelation(filters, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return result;
        }

        /// <summary>Adds a new goodsreceiptpurchaseorderrelation</summary>
        /// <param name="model">The goodsreceiptpurchaseorderrelation data to be added</param>
        /// <returns>The result of the operation</returns>
        public Guid Create(GoodsReceiptPurchaseOrderRelation model)
        {
            model.Id = CreateGoodsReceiptPurchaseOrderRelation(model);
            return model.Id;
        }

        /// <summary>Updates a specific goodsreceiptpurchaseorderrelation by its primary key</summary>
        /// <param name="id">The primary key of the goodsreceiptpurchaseorderrelation</param>
        /// <param name="updatedEntity">The goodsreceiptpurchaseorderrelation data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Update(Guid id, GoodsReceiptPurchaseOrderRelation updatedEntity)
        {
            UpdateGoodsReceiptPurchaseOrderRelation(id, updatedEntity);
            return true;
        }

        /// <summary>Updates a specific goodsreceiptpurchaseorderrelation by its primary key</summary>
        /// <param name="id">The primary key of the goodsreceiptpurchaseorderrelation</param>
        /// <param name="updatedEntity">The goodsreceiptpurchaseorderrelation data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Patch(Guid id, JsonPatchDocument<GoodsReceiptPurchaseOrderRelation> updatedEntity)
        {
            PatchGoodsReceiptPurchaseOrderRelation(id, updatedEntity);
            return true;
        }

        /// <summary>Deletes a specific goodsreceiptpurchaseorderrelation by its primary key</summary>
        /// <param name="id">The primary key of the goodsreceiptpurchaseorderrelation</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Delete(Guid id)
        {
            DeleteGoodsReceiptPurchaseOrderRelation(id);
            return true;
        }
        #region
        private List<GoodsReceiptPurchaseOrderRelation> GetGoodsReceiptPurchaseOrderRelation(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            if (pageSize < 1)
            {
                throw new ApplicationException("Page size invalid!");
            }

            if (pageNumber < 1)
            {
                throw new ApplicationException("Page mumber invalid!");
            }

            var query = _dbContext.GoodsReceiptPurchaseOrderRelation.IncludeRelated().AsQueryable();
            int skip = (pageNumber - 1) * pageSize;
            var result = FilterService<GoodsReceiptPurchaseOrderRelation>.ApplyFilter(query, filters, searchTerm);
            if (!string.IsNullOrEmpty(sortField))
            {
                var parameter = Expression.Parameter(typeof(GoodsReceiptPurchaseOrderRelation), "b");
                var property = Expression.Property(parameter, sortField);
                var lambda = Expression.Lambda<Func<GoodsReceiptPurchaseOrderRelation, object>>(Expression.Convert(property, typeof(object)), parameter);
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

        private Guid CreateGoodsReceiptPurchaseOrderRelation(GoodsReceiptPurchaseOrderRelation model)
        {
            _dbContext.GoodsReceiptPurchaseOrderRelation.Add(model);
            _dbContext.SaveChanges();
            return model.Id;
        }

        private void UpdateGoodsReceiptPurchaseOrderRelation(Guid id, GoodsReceiptPurchaseOrderRelation updatedEntity)
        {
            _dbContext.GoodsReceiptPurchaseOrderRelation.Update(updatedEntity);
            _dbContext.SaveChanges();
        }

        private bool DeleteGoodsReceiptPurchaseOrderRelation(Guid id)
        {
            var entityData = _dbContext.GoodsReceiptPurchaseOrderRelation.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            if (entityData == null)
            {
                throw new ApplicationException("No data found!");
            }

            _dbContext.GoodsReceiptPurchaseOrderRelation.Remove(entityData);
            _dbContext.SaveChanges();
            return true;
        }

        private void PatchGoodsReceiptPurchaseOrderRelation(Guid id, JsonPatchDocument<GoodsReceiptPurchaseOrderRelation> updatedEntity)
        {
            if (updatedEntity == null)
            {
                throw new ApplicationException("Patch document is missing!");
            }

            var existingEntity = _dbContext.GoodsReceiptPurchaseOrderRelation.FirstOrDefault(t => t.Id == id);
            if (existingEntity == null)
            {
                throw new ApplicationException("No data found!");
            }

            updatedEntity.ApplyTo(existingEntity);
            _dbContext.GoodsReceiptPurchaseOrderRelation.Update(existingEntity);
            _dbContext.SaveChanges();
        }
        #endregion
    }
}