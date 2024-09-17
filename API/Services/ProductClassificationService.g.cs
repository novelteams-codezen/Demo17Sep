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
    /// The productclassificationService responsible for managing productclassification related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting productclassification information.
    /// </remarks>
    public interface IProductClassificationService
    {
        /// <summary>Retrieves a specific productclassification by its primary key</summary>
        /// <param name="id">The primary key of the productclassification</param>
        /// <returns>The productclassification data</returns>
        ProductClassification GetById(Guid id);

        /// <summary>Retrieves a list of productclassifications based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of productclassifications</returns>
        List<ProductClassification> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc");

        /// <summary>Adds a new productclassification</summary>
        /// <param name="model">The productclassification data to be added</param>
        /// <returns>The result of the operation</returns>
        Guid Create(ProductClassification model);

        /// <summary>Updates a specific productclassification by its primary key</summary>
        /// <param name="id">The primary key of the productclassification</param>
        /// <param name="updatedEntity">The productclassification data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Update(Guid id, ProductClassification updatedEntity);

        /// <summary>Updates a specific productclassification by its primary key</summary>
        /// <param name="id">The primary key of the productclassification</param>
        /// <param name="updatedEntity">The productclassification data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Patch(Guid id, JsonPatchDocument<ProductClassification> updatedEntity);

        /// <summary>Deletes a specific productclassification by its primary key</summary>
        /// <param name="id">The primary key of the productclassification</param>
        /// <returns>The result of the operation</returns>
        bool Delete(Guid id);
    }

    /// <summary>
    /// The productclassificationService responsible for managing productclassification related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting productclassification information.
    /// </remarks>
    public class ProductClassificationService : IProductClassificationService
    {
        private Demo17SepContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the ProductClassification class.
        /// </summary>
        /// <param name="dbContext">dbContext value to set.</param>
        public ProductClassificationService(Demo17SepContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>Retrieves a specific productclassification by its primary key</summary>
        /// <param name="id">The primary key of the productclassification</param>
        /// <returns>The productclassification data</returns>
        public ProductClassification GetById(Guid id)
        {
            var entityData = _dbContext.ProductClassification.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            return entityData;
        }

        /// <summary>Retrieves a list of productclassifications based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of productclassifications</returns>/// <exception cref="Exception"></exception>
        public List<ProductClassification> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            var result = GetProductClassification(filters, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return result;
        }

        /// <summary>Adds a new productclassification</summary>
        /// <param name="model">The productclassification data to be added</param>
        /// <returns>The result of the operation</returns>
        public Guid Create(ProductClassification model)
        {
            model.Id = CreateProductClassification(model);
            return model.Id;
        }

        /// <summary>Updates a specific productclassification by its primary key</summary>
        /// <param name="id">The primary key of the productclassification</param>
        /// <param name="updatedEntity">The productclassification data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Update(Guid id, ProductClassification updatedEntity)
        {
            UpdateProductClassification(id, updatedEntity);
            return true;
        }

        /// <summary>Updates a specific productclassification by its primary key</summary>
        /// <param name="id">The primary key of the productclassification</param>
        /// <param name="updatedEntity">The productclassification data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Patch(Guid id, JsonPatchDocument<ProductClassification> updatedEntity)
        {
            PatchProductClassification(id, updatedEntity);
            return true;
        }

        /// <summary>Deletes a specific productclassification by its primary key</summary>
        /// <param name="id">The primary key of the productclassification</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Delete(Guid id)
        {
            DeleteProductClassification(id);
            return true;
        }
        #region
        private List<ProductClassification> GetProductClassification(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            if (pageSize < 1)
            {
                throw new ApplicationException("Page size invalid!");
            }

            if (pageNumber < 1)
            {
                throw new ApplicationException("Page mumber invalid!");
            }

            var query = _dbContext.ProductClassification.IncludeRelated().AsQueryable();
            int skip = (pageNumber - 1) * pageSize;
            var result = FilterService<ProductClassification>.ApplyFilter(query, filters, searchTerm);
            if (!string.IsNullOrEmpty(sortField))
            {
                var parameter = Expression.Parameter(typeof(ProductClassification), "b");
                var property = Expression.Property(parameter, sortField);
                var lambda = Expression.Lambda<Func<ProductClassification, object>>(Expression.Convert(property, typeof(object)), parameter);
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

        private Guid CreateProductClassification(ProductClassification model)
        {
            _dbContext.ProductClassification.Add(model);
            _dbContext.SaveChanges();
            return model.Id;
        }

        private void UpdateProductClassification(Guid id, ProductClassification updatedEntity)
        {
            _dbContext.ProductClassification.Update(updatedEntity);
            _dbContext.SaveChanges();
        }

        private bool DeleteProductClassification(Guid id)
        {
            var entityData = _dbContext.ProductClassification.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            if (entityData == null)
            {
                throw new ApplicationException("No data found!");
            }

            _dbContext.ProductClassification.Remove(entityData);
            _dbContext.SaveChanges();
            return true;
        }

        private void PatchProductClassification(Guid id, JsonPatchDocument<ProductClassification> updatedEntity)
        {
            if (updatedEntity == null)
            {
                throw new ApplicationException("Patch document is missing!");
            }

            var existingEntity = _dbContext.ProductClassification.FirstOrDefault(t => t.Id == id);
            if (existingEntity == null)
            {
                throw new ApplicationException("No data found!");
            }

            updatedEntity.ApplyTo(existingEntity);
            _dbContext.ProductClassification.Update(existingEntity);
            _dbContext.SaveChanges();
        }
        #endregion
    }
}