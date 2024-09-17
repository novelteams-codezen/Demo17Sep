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
    /// The purchaseorderlineService responsible for managing purchaseorderline related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting purchaseorderline information.
    /// </remarks>
    public interface IPurchaseOrderLineService
    {
        /// <summary>Retrieves a specific purchaseorderline by its primary key</summary>
        /// <param name="id">The primary key of the purchaseorderline</param>
        /// <returns>The purchaseorderline data</returns>
        PurchaseOrderLine GetById(Guid id);

        /// <summary>Retrieves a list of purchaseorderlines based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of purchaseorderlines</returns>
        List<PurchaseOrderLine> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc");

        /// <summary>Adds a new purchaseorderline</summary>
        /// <param name="model">The purchaseorderline data to be added</param>
        /// <returns>The result of the operation</returns>
        Guid Create(PurchaseOrderLine model);

        /// <summary>Updates a specific purchaseorderline by its primary key</summary>
        /// <param name="id">The primary key of the purchaseorderline</param>
        /// <param name="updatedEntity">The purchaseorderline data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Update(Guid id, PurchaseOrderLine updatedEntity);

        /// <summary>Updates a specific purchaseorderline by its primary key</summary>
        /// <param name="id">The primary key of the purchaseorderline</param>
        /// <param name="updatedEntity">The purchaseorderline data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Patch(Guid id, JsonPatchDocument<PurchaseOrderLine> updatedEntity);

        /// <summary>Deletes a specific purchaseorderline by its primary key</summary>
        /// <param name="id">The primary key of the purchaseorderline</param>
        /// <returns>The result of the operation</returns>
        bool Delete(Guid id);
    }

    /// <summary>
    /// The purchaseorderlineService responsible for managing purchaseorderline related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting purchaseorderline information.
    /// </remarks>
    public class PurchaseOrderLineService : IPurchaseOrderLineService
    {
        private Demo17SepContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the PurchaseOrderLine class.
        /// </summary>
        /// <param name="dbContext">dbContext value to set.</param>
        public PurchaseOrderLineService(Demo17SepContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>Retrieves a specific purchaseorderline by its primary key</summary>
        /// <param name="id">The primary key of the purchaseorderline</param>
        /// <returns>The purchaseorderline data</returns>
        public PurchaseOrderLine GetById(Guid id)
        {
            var entityData = _dbContext.PurchaseOrderLine.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            return entityData;
        }

        /// <summary>Retrieves a list of purchaseorderlines based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of purchaseorderlines</returns>/// <exception cref="Exception"></exception>
        public List<PurchaseOrderLine> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            var result = GetPurchaseOrderLine(filters, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return result;
        }

        /// <summary>Adds a new purchaseorderline</summary>
        /// <param name="model">The purchaseorderline data to be added</param>
        /// <returns>The result of the operation</returns>
        public Guid Create(PurchaseOrderLine model)
        {
            model.Id = CreatePurchaseOrderLine(model);
            return model.Id;
        }

        /// <summary>Updates a specific purchaseorderline by its primary key</summary>
        /// <param name="id">The primary key of the purchaseorderline</param>
        /// <param name="updatedEntity">The purchaseorderline data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Update(Guid id, PurchaseOrderLine updatedEntity)
        {
            UpdatePurchaseOrderLine(id, updatedEntity);
            return true;
        }

        /// <summary>Updates a specific purchaseorderline by its primary key</summary>
        /// <param name="id">The primary key of the purchaseorderline</param>
        /// <param name="updatedEntity">The purchaseorderline data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Patch(Guid id, JsonPatchDocument<PurchaseOrderLine> updatedEntity)
        {
            PatchPurchaseOrderLine(id, updatedEntity);
            return true;
        }

        /// <summary>Deletes a specific purchaseorderline by its primary key</summary>
        /// <param name="id">The primary key of the purchaseorderline</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Delete(Guid id)
        {
            DeletePurchaseOrderLine(id);
            return true;
        }
        #region
        private List<PurchaseOrderLine> GetPurchaseOrderLine(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            if (pageSize < 1)
            {
                throw new ApplicationException("Page size invalid!");
            }

            if (pageNumber < 1)
            {
                throw new ApplicationException("Page mumber invalid!");
            }

            var query = _dbContext.PurchaseOrderLine.IncludeRelated().AsQueryable();
            int skip = (pageNumber - 1) * pageSize;
            var result = FilterService<PurchaseOrderLine>.ApplyFilter(query, filters, searchTerm);
            if (!string.IsNullOrEmpty(sortField))
            {
                var parameter = Expression.Parameter(typeof(PurchaseOrderLine), "b");
                var property = Expression.Property(parameter, sortField);
                var lambda = Expression.Lambda<Func<PurchaseOrderLine, object>>(Expression.Convert(property, typeof(object)), parameter);
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

        private Guid CreatePurchaseOrderLine(PurchaseOrderLine model)
        {
            _dbContext.PurchaseOrderLine.Add(model);
            _dbContext.SaveChanges();
            return model.Id;
        }

        private void UpdatePurchaseOrderLine(Guid id, PurchaseOrderLine updatedEntity)
        {
            _dbContext.PurchaseOrderLine.Update(updatedEntity);
            _dbContext.SaveChanges();
        }

        private bool DeletePurchaseOrderLine(Guid id)
        {
            var entityData = _dbContext.PurchaseOrderLine.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            if (entityData == null)
            {
                throw new ApplicationException("No data found!");
            }

            _dbContext.PurchaseOrderLine.Remove(entityData);
            _dbContext.SaveChanges();
            return true;
        }

        private void PatchPurchaseOrderLine(Guid id, JsonPatchDocument<PurchaseOrderLine> updatedEntity)
        {
            if (updatedEntity == null)
            {
                throw new ApplicationException("Patch document is missing!");
            }

            var existingEntity = _dbContext.PurchaseOrderLine.FirstOrDefault(t => t.Id == id);
            if (existingEntity == null)
            {
                throw new ApplicationException("No data found!");
            }

            updatedEntity.ApplyTo(existingEntity);
            _dbContext.PurchaseOrderLine.Update(existingEntity);
            _dbContext.SaveChanges();
        }
        #endregion
    }
}