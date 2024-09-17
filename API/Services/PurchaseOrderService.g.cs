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
    /// The purchaseorderService responsible for managing purchaseorder related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting purchaseorder information.
    /// </remarks>
    public interface IPurchaseOrderService
    {
        /// <summary>Retrieves a specific purchaseorder by its primary key</summary>
        /// <param name="id">The primary key of the purchaseorder</param>
        /// <returns>The purchaseorder data</returns>
        PurchaseOrder GetById(Guid id);

        /// <summary>Retrieves a list of purchaseorders based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of purchaseorders</returns>
        List<PurchaseOrder> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc");

        /// <summary>Adds a new purchaseorder</summary>
        /// <param name="model">The purchaseorder data to be added</param>
        /// <returns>The result of the operation</returns>
        Guid Create(PurchaseOrder model);

        /// <summary>Updates a specific purchaseorder by its primary key</summary>
        /// <param name="id">The primary key of the purchaseorder</param>
        /// <param name="updatedEntity">The purchaseorder data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Update(Guid id, PurchaseOrder updatedEntity);

        /// <summary>Updates a specific purchaseorder by its primary key</summary>
        /// <param name="id">The primary key of the purchaseorder</param>
        /// <param name="updatedEntity">The purchaseorder data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Patch(Guid id, JsonPatchDocument<PurchaseOrder> updatedEntity);

        /// <summary>Deletes a specific purchaseorder by its primary key</summary>
        /// <param name="id">The primary key of the purchaseorder</param>
        /// <returns>The result of the operation</returns>
        bool Delete(Guid id);
    }

    /// <summary>
    /// The purchaseorderService responsible for managing purchaseorder related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting purchaseorder information.
    /// </remarks>
    public class PurchaseOrderService : IPurchaseOrderService
    {
        private Demo17SepContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the PurchaseOrder class.
        /// </summary>
        /// <param name="dbContext">dbContext value to set.</param>
        public PurchaseOrderService(Demo17SepContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>Retrieves a specific purchaseorder by its primary key</summary>
        /// <param name="id">The primary key of the purchaseorder</param>
        /// <returns>The purchaseorder data</returns>
        public PurchaseOrder GetById(Guid id)
        {
            var entityData = _dbContext.PurchaseOrder.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            return entityData;
        }

        /// <summary>Retrieves a list of purchaseorders based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of purchaseorders</returns>/// <exception cref="Exception"></exception>
        public List<PurchaseOrder> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            var result = GetPurchaseOrder(filters, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return result;
        }

        /// <summary>Adds a new purchaseorder</summary>
        /// <param name="model">The purchaseorder data to be added</param>
        /// <returns>The result of the operation</returns>
        public Guid Create(PurchaseOrder model)
        {
            model.Id = CreatePurchaseOrder(model);
            return model.Id;
        }

        /// <summary>Updates a specific purchaseorder by its primary key</summary>
        /// <param name="id">The primary key of the purchaseorder</param>
        /// <param name="updatedEntity">The purchaseorder data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Update(Guid id, PurchaseOrder updatedEntity)
        {
            UpdatePurchaseOrder(id, updatedEntity);
            return true;
        }

        /// <summary>Updates a specific purchaseorder by its primary key</summary>
        /// <param name="id">The primary key of the purchaseorder</param>
        /// <param name="updatedEntity">The purchaseorder data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Patch(Guid id, JsonPatchDocument<PurchaseOrder> updatedEntity)
        {
            PatchPurchaseOrder(id, updatedEntity);
            return true;
        }

        /// <summary>Deletes a specific purchaseorder by its primary key</summary>
        /// <param name="id">The primary key of the purchaseorder</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Delete(Guid id)
        {
            DeletePurchaseOrder(id);
            return true;
        }
        #region
        private List<PurchaseOrder> GetPurchaseOrder(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            if (pageSize < 1)
            {
                throw new ApplicationException("Page size invalid!");
            }

            if (pageNumber < 1)
            {
                throw new ApplicationException("Page mumber invalid!");
            }

            var query = _dbContext.PurchaseOrder.IncludeRelated().AsQueryable();
            int skip = (pageNumber - 1) * pageSize;
            var result = FilterService<PurchaseOrder>.ApplyFilter(query, filters, searchTerm);
            if (!string.IsNullOrEmpty(sortField))
            {
                var parameter = Expression.Parameter(typeof(PurchaseOrder), "b");
                var property = Expression.Property(parameter, sortField);
                var lambda = Expression.Lambda<Func<PurchaseOrder, object>>(Expression.Convert(property, typeof(object)), parameter);
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

        private Guid CreatePurchaseOrder(PurchaseOrder model)
        {
            _dbContext.PurchaseOrder.Add(model);
            _dbContext.SaveChanges();
            return model.Id;
        }

        private void UpdatePurchaseOrder(Guid id, PurchaseOrder updatedEntity)
        {
            _dbContext.PurchaseOrder.Update(updatedEntity);
            _dbContext.SaveChanges();
        }

        private bool DeletePurchaseOrder(Guid id)
        {
            var entityData = _dbContext.PurchaseOrder.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            if (entityData == null)
            {
                throw new ApplicationException("No data found!");
            }

            _dbContext.PurchaseOrder.Remove(entityData);
            _dbContext.SaveChanges();
            return true;
        }

        private void PatchPurchaseOrder(Guid id, JsonPatchDocument<PurchaseOrder> updatedEntity)
        {
            if (updatedEntity == null)
            {
                throw new ApplicationException("Patch document is missing!");
            }

            var existingEntity = _dbContext.PurchaseOrder.FirstOrDefault(t => t.Id == id);
            if (existingEntity == null)
            {
                throw new ApplicationException("No data found!");
            }

            updatedEntity.ApplyTo(existingEntity);
            _dbContext.PurchaseOrder.Update(existingEntity);
            _dbContext.SaveChanges();
        }
        #endregion
    }
}