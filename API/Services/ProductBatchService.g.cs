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
    /// The productbatchService responsible for managing productbatch related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting productbatch information.
    /// </remarks>
    public interface IProductBatchService
    {
        /// <summary>Retrieves a specific productbatch by its primary key</summary>
        /// <param name="id">The primary key of the productbatch</param>
        /// <returns>The productbatch data</returns>
        ProductBatch GetById(Guid id);

        /// <summary>Retrieves a list of productbatchs based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of productbatchs</returns>
        List<ProductBatch> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc");

        /// <summary>Adds a new productbatch</summary>
        /// <param name="model">The productbatch data to be added</param>
        /// <returns>The result of the operation</returns>
        Guid Create(ProductBatch model);

        /// <summary>Updates a specific productbatch by its primary key</summary>
        /// <param name="id">The primary key of the productbatch</param>
        /// <param name="updatedEntity">The productbatch data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Update(Guid id, ProductBatch updatedEntity);

        /// <summary>Updates a specific productbatch by its primary key</summary>
        /// <param name="id">The primary key of the productbatch</param>
        /// <param name="updatedEntity">The productbatch data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Patch(Guid id, JsonPatchDocument<ProductBatch> updatedEntity);

        /// <summary>Deletes a specific productbatch by its primary key</summary>
        /// <param name="id">The primary key of the productbatch</param>
        /// <returns>The result of the operation</returns>
        bool Delete(Guid id);
    }

    /// <summary>
    /// The productbatchService responsible for managing productbatch related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting productbatch information.
    /// </remarks>
    public class ProductBatchService : IProductBatchService
    {
        private Demo17SepContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the ProductBatch class.
        /// </summary>
        /// <param name="dbContext">dbContext value to set.</param>
        public ProductBatchService(Demo17SepContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>Retrieves a specific productbatch by its primary key</summary>
        /// <param name="id">The primary key of the productbatch</param>
        /// <returns>The productbatch data</returns>
        public ProductBatch GetById(Guid id)
        {
            var entityData = _dbContext.ProductBatch.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            return entityData;
        }

        /// <summary>Retrieves a list of productbatchs based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of productbatchs</returns>/// <exception cref="Exception"></exception>
        public List<ProductBatch> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            var result = GetProductBatch(filters, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return result;
        }

        /// <summary>Adds a new productbatch</summary>
        /// <param name="model">The productbatch data to be added</param>
        /// <returns>The result of the operation</returns>
        public Guid Create(ProductBatch model)
        {
            model.Id = CreateProductBatch(model);
            return model.Id;
        }

        /// <summary>Updates a specific productbatch by its primary key</summary>
        /// <param name="id">The primary key of the productbatch</param>
        /// <param name="updatedEntity">The productbatch data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Update(Guid id, ProductBatch updatedEntity)
        {
            UpdateProductBatch(id, updatedEntity);
            return true;
        }

        /// <summary>Updates a specific productbatch by its primary key</summary>
        /// <param name="id">The primary key of the productbatch</param>
        /// <param name="updatedEntity">The productbatch data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Patch(Guid id, JsonPatchDocument<ProductBatch> updatedEntity)
        {
            PatchProductBatch(id, updatedEntity);
            return true;
        }

        /// <summary>Deletes a specific productbatch by its primary key</summary>
        /// <param name="id">The primary key of the productbatch</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Delete(Guid id)
        {
            DeleteProductBatch(id);
            return true;
        }
        #region
        private List<ProductBatch> GetProductBatch(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            if (pageSize < 1)
            {
                throw new ApplicationException("Page size invalid!");
            }

            if (pageNumber < 1)
            {
                throw new ApplicationException("Page mumber invalid!");
            }

            var query = _dbContext.ProductBatch.IncludeRelated().AsQueryable();
            int skip = (pageNumber - 1) * pageSize;
            var result = FilterService<ProductBatch>.ApplyFilter(query, filters, searchTerm);
            if (!string.IsNullOrEmpty(sortField))
            {
                var parameter = Expression.Parameter(typeof(ProductBatch), "b");
                var property = Expression.Property(parameter, sortField);
                var lambda = Expression.Lambda<Func<ProductBatch, object>>(Expression.Convert(property, typeof(object)), parameter);
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

        private Guid CreateProductBatch(ProductBatch model)
        {
            _dbContext.ProductBatch.Add(model);
            _dbContext.SaveChanges();
            return model.Id;
        }

        private void UpdateProductBatch(Guid id, ProductBatch updatedEntity)
        {
            _dbContext.ProductBatch.Update(updatedEntity);
            _dbContext.SaveChanges();
        }

        private bool DeleteProductBatch(Guid id)
        {
            var entityData = _dbContext.ProductBatch.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            if (entityData == null)
            {
                throw new ApplicationException("No data found!");
            }

            _dbContext.ProductBatch.Remove(entityData);
            _dbContext.SaveChanges();
            return true;
        }

        private void PatchProductBatch(Guid id, JsonPatchDocument<ProductBatch> updatedEntity)
        {
            if (updatedEntity == null)
            {
                throw new ApplicationException("Patch document is missing!");
            }

            var existingEntity = _dbContext.ProductBatch.FirstOrDefault(t => t.Id == id);
            if (existingEntity == null)
            {
                throw new ApplicationException("No data found!");
            }

            updatedEntity.ApplyTo(existingEntity);
            _dbContext.ProductBatch.Update(existingEntity);
            _dbContext.SaveChanges();
        }
        #endregion
    }
}