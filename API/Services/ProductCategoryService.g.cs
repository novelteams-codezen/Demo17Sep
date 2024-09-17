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
    /// The productcategoryService responsible for managing productcategory related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting productcategory information.
    /// </remarks>
    public interface IProductCategoryService
    {
        /// <summary>Retrieves a specific productcategory by its primary key</summary>
        /// <param name="id">The primary key of the productcategory</param>
        /// <returns>The productcategory data</returns>
        ProductCategory GetById(Guid id);

        /// <summary>Retrieves a list of productcategorys based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of productcategorys</returns>
        List<ProductCategory> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc");

        /// <summary>Adds a new productcategory</summary>
        /// <param name="model">The productcategory data to be added</param>
        /// <returns>The result of the operation</returns>
        Guid Create(ProductCategory model);

        /// <summary>Updates a specific productcategory by its primary key</summary>
        /// <param name="id">The primary key of the productcategory</param>
        /// <param name="updatedEntity">The productcategory data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Update(Guid id, ProductCategory updatedEntity);

        /// <summary>Updates a specific productcategory by its primary key</summary>
        /// <param name="id">The primary key of the productcategory</param>
        /// <param name="updatedEntity">The productcategory data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Patch(Guid id, JsonPatchDocument<ProductCategory> updatedEntity);

        /// <summary>Deletes a specific productcategory by its primary key</summary>
        /// <param name="id">The primary key of the productcategory</param>
        /// <returns>The result of the operation</returns>
        bool Delete(Guid id);
    }

    /// <summary>
    /// The productcategoryService responsible for managing productcategory related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting productcategory information.
    /// </remarks>
    public class ProductCategoryService : IProductCategoryService
    {
        private Demo17SepContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the ProductCategory class.
        /// </summary>
        /// <param name="dbContext">dbContext value to set.</param>
        public ProductCategoryService(Demo17SepContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>Retrieves a specific productcategory by its primary key</summary>
        /// <param name="id">The primary key of the productcategory</param>
        /// <returns>The productcategory data</returns>
        public ProductCategory GetById(Guid id)
        {
            var entityData = _dbContext.ProductCategory.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            return entityData;
        }

        /// <summary>Retrieves a list of productcategorys based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of productcategorys</returns>/// <exception cref="Exception"></exception>
        public List<ProductCategory> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            var result = GetProductCategory(filters, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return result;
        }

        /// <summary>Adds a new productcategory</summary>
        /// <param name="model">The productcategory data to be added</param>
        /// <returns>The result of the operation</returns>
        public Guid Create(ProductCategory model)
        {
            model.Id = CreateProductCategory(model);
            return model.Id;
        }

        /// <summary>Updates a specific productcategory by its primary key</summary>
        /// <param name="id">The primary key of the productcategory</param>
        /// <param name="updatedEntity">The productcategory data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Update(Guid id, ProductCategory updatedEntity)
        {
            UpdateProductCategory(id, updatedEntity);
            return true;
        }

        /// <summary>Updates a specific productcategory by its primary key</summary>
        /// <param name="id">The primary key of the productcategory</param>
        /// <param name="updatedEntity">The productcategory data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Patch(Guid id, JsonPatchDocument<ProductCategory> updatedEntity)
        {
            PatchProductCategory(id, updatedEntity);
            return true;
        }

        /// <summary>Deletes a specific productcategory by its primary key</summary>
        /// <param name="id">The primary key of the productcategory</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Delete(Guid id)
        {
            DeleteProductCategory(id);
            return true;
        }
        #region
        private List<ProductCategory> GetProductCategory(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            if (pageSize < 1)
            {
                throw new ApplicationException("Page size invalid!");
            }

            if (pageNumber < 1)
            {
                throw new ApplicationException("Page mumber invalid!");
            }

            var query = _dbContext.ProductCategory.IncludeRelated().AsQueryable();
            int skip = (pageNumber - 1) * pageSize;
            var result = FilterService<ProductCategory>.ApplyFilter(query, filters, searchTerm);
            if (!string.IsNullOrEmpty(sortField))
            {
                var parameter = Expression.Parameter(typeof(ProductCategory), "b");
                var property = Expression.Property(parameter, sortField);
                var lambda = Expression.Lambda<Func<ProductCategory, object>>(Expression.Convert(property, typeof(object)), parameter);
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

        private Guid CreateProductCategory(ProductCategory model)
        {
            _dbContext.ProductCategory.Add(model);
            _dbContext.SaveChanges();
            return model.Id;
        }

        private void UpdateProductCategory(Guid id, ProductCategory updatedEntity)
        {
            _dbContext.ProductCategory.Update(updatedEntity);
            _dbContext.SaveChanges();
        }

        private bool DeleteProductCategory(Guid id)
        {
            var entityData = _dbContext.ProductCategory.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            if (entityData == null)
            {
                throw new ApplicationException("No data found!");
            }

            _dbContext.ProductCategory.Remove(entityData);
            _dbContext.SaveChanges();
            return true;
        }

        private void PatchProductCategory(Guid id, JsonPatchDocument<ProductCategory> updatedEntity)
        {
            if (updatedEntity == null)
            {
                throw new ApplicationException("Patch document is missing!");
            }

            var existingEntity = _dbContext.ProductCategory.FirstOrDefault(t => t.Id == id);
            if (existingEntity == null)
            {
                throw new ApplicationException("No data found!");
            }

            updatedEntity.ApplyTo(existingEntity);
            _dbContext.ProductCategory.Update(existingEntity);
            _dbContext.SaveChanges();
        }
        #endregion
    }
}