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
    /// The productmanufactureService responsible for managing productmanufacture related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting productmanufacture information.
    /// </remarks>
    public interface IProductManufactureService
    {
        /// <summary>Retrieves a specific productmanufacture by its primary key</summary>
        /// <param name="id">The primary key of the productmanufacture</param>
        /// <returns>The productmanufacture data</returns>
        ProductManufacture GetById(Guid id);

        /// <summary>Retrieves a list of productmanufactures based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of productmanufactures</returns>
        List<ProductManufacture> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc");

        /// <summary>Adds a new productmanufacture</summary>
        /// <param name="model">The productmanufacture data to be added</param>
        /// <returns>The result of the operation</returns>
        Guid Create(ProductManufacture model);

        /// <summary>Updates a specific productmanufacture by its primary key</summary>
        /// <param name="id">The primary key of the productmanufacture</param>
        /// <param name="updatedEntity">The productmanufacture data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Update(Guid id, ProductManufacture updatedEntity);

        /// <summary>Updates a specific productmanufacture by its primary key</summary>
        /// <param name="id">The primary key of the productmanufacture</param>
        /// <param name="updatedEntity">The productmanufacture data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Patch(Guid id, JsonPatchDocument<ProductManufacture> updatedEntity);

        /// <summary>Deletes a specific productmanufacture by its primary key</summary>
        /// <param name="id">The primary key of the productmanufacture</param>
        /// <returns>The result of the operation</returns>
        bool Delete(Guid id);
    }

    /// <summary>
    /// The productmanufactureService responsible for managing productmanufacture related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting productmanufacture information.
    /// </remarks>
    public class ProductManufactureService : IProductManufactureService
    {
        private Demo17SepContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the ProductManufacture class.
        /// </summary>
        /// <param name="dbContext">dbContext value to set.</param>
        public ProductManufactureService(Demo17SepContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>Retrieves a specific productmanufacture by its primary key</summary>
        /// <param name="id">The primary key of the productmanufacture</param>
        /// <returns>The productmanufacture data</returns>
        public ProductManufacture GetById(Guid id)
        {
            var entityData = _dbContext.ProductManufacture.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            return entityData;
        }

        /// <summary>Retrieves a list of productmanufactures based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of productmanufactures</returns>/// <exception cref="Exception"></exception>
        public List<ProductManufacture> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            var result = GetProductManufacture(filters, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return result;
        }

        /// <summary>Adds a new productmanufacture</summary>
        /// <param name="model">The productmanufacture data to be added</param>
        /// <returns>The result of the operation</returns>
        public Guid Create(ProductManufacture model)
        {
            model.Id = CreateProductManufacture(model);
            return model.Id;
        }

        /// <summary>Updates a specific productmanufacture by its primary key</summary>
        /// <param name="id">The primary key of the productmanufacture</param>
        /// <param name="updatedEntity">The productmanufacture data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Update(Guid id, ProductManufacture updatedEntity)
        {
            UpdateProductManufacture(id, updatedEntity);
            return true;
        }

        /// <summary>Updates a specific productmanufacture by its primary key</summary>
        /// <param name="id">The primary key of the productmanufacture</param>
        /// <param name="updatedEntity">The productmanufacture data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Patch(Guid id, JsonPatchDocument<ProductManufacture> updatedEntity)
        {
            PatchProductManufacture(id, updatedEntity);
            return true;
        }

        /// <summary>Deletes a specific productmanufacture by its primary key</summary>
        /// <param name="id">The primary key of the productmanufacture</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Delete(Guid id)
        {
            DeleteProductManufacture(id);
            return true;
        }
        #region
        private List<ProductManufacture> GetProductManufacture(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            if (pageSize < 1)
            {
                throw new ApplicationException("Page size invalid!");
            }

            if (pageNumber < 1)
            {
                throw new ApplicationException("Page mumber invalid!");
            }

            var query = _dbContext.ProductManufacture.IncludeRelated().AsQueryable();
            int skip = (pageNumber - 1) * pageSize;
            var result = FilterService<ProductManufacture>.ApplyFilter(query, filters, searchTerm);
            if (!string.IsNullOrEmpty(sortField))
            {
                var parameter = Expression.Parameter(typeof(ProductManufacture), "b");
                var property = Expression.Property(parameter, sortField);
                var lambda = Expression.Lambda<Func<ProductManufacture, object>>(Expression.Convert(property, typeof(object)), parameter);
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

        private Guid CreateProductManufacture(ProductManufacture model)
        {
            _dbContext.ProductManufacture.Add(model);
            _dbContext.SaveChanges();
            return model.Id;
        }

        private void UpdateProductManufacture(Guid id, ProductManufacture updatedEntity)
        {
            _dbContext.ProductManufacture.Update(updatedEntity);
            _dbContext.SaveChanges();
        }

        private bool DeleteProductManufacture(Guid id)
        {
            var entityData = _dbContext.ProductManufacture.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            if (entityData == null)
            {
                throw new ApplicationException("No data found!");
            }

            _dbContext.ProductManufacture.Remove(entityData);
            _dbContext.SaveChanges();
            return true;
        }

        private void PatchProductManufacture(Guid id, JsonPatchDocument<ProductManufacture> updatedEntity)
        {
            if (updatedEntity == null)
            {
                throw new ApplicationException("Patch document is missing!");
            }

            var existingEntity = _dbContext.ProductManufacture.FirstOrDefault(t => t.Id == id);
            if (existingEntity == null)
            {
                throw new ApplicationException("No data found!");
            }

            updatedEntity.ApplyTo(existingEntity);
            _dbContext.ProductManufacture.Update(existingEntity);
            _dbContext.SaveChanges();
        }
        #endregion
    }
}