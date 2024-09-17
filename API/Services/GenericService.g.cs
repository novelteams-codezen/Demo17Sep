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
    /// The genericService responsible for managing generic related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting generic information.
    /// </remarks>
    public interface IGenericService
    {
        /// <summary>Retrieves a specific generic by its primary key</summary>
        /// <param name="id">The primary key of the generic</param>
        /// <returns>The generic data</returns>
        Generic GetById(Guid id);

        /// <summary>Retrieves a list of generics based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of generics</returns>
        List<Generic> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc");

        /// <summary>Adds a new generic</summary>
        /// <param name="model">The generic data to be added</param>
        /// <returns>The result of the operation</returns>
        Guid Create(Generic model);

        /// <summary>Updates a specific generic by its primary key</summary>
        /// <param name="id">The primary key of the generic</param>
        /// <param name="updatedEntity">The generic data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Update(Guid id, Generic updatedEntity);

        /// <summary>Updates a specific generic by its primary key</summary>
        /// <param name="id">The primary key of the generic</param>
        /// <param name="updatedEntity">The generic data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Patch(Guid id, JsonPatchDocument<Generic> updatedEntity);

        /// <summary>Deletes a specific generic by its primary key</summary>
        /// <param name="id">The primary key of the generic</param>
        /// <returns>The result of the operation</returns>
        bool Delete(Guid id);
    }

    /// <summary>
    /// The genericService responsible for managing generic related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting generic information.
    /// </remarks>
    public class GenericService : IGenericService
    {
        private Demo17SepContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the Generic class.
        /// </summary>
        /// <param name="dbContext">dbContext value to set.</param>
        public GenericService(Demo17SepContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>Retrieves a specific generic by its primary key</summary>
        /// <param name="id">The primary key of the generic</param>
        /// <returns>The generic data</returns>
        public Generic GetById(Guid id)
        {
            var entityData = _dbContext.Generic.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            return entityData;
        }

        /// <summary>Retrieves a list of generics based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of generics</returns>/// <exception cref="Exception"></exception>
        public List<Generic> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            var result = GetGeneric(filters, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return result;
        }

        /// <summary>Adds a new generic</summary>
        /// <param name="model">The generic data to be added</param>
        /// <returns>The result of the operation</returns>
        public Guid Create(Generic model)
        {
            model.Id = CreateGeneric(model);
            return model.Id;
        }

        /// <summary>Updates a specific generic by its primary key</summary>
        /// <param name="id">The primary key of the generic</param>
        /// <param name="updatedEntity">The generic data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Update(Guid id, Generic updatedEntity)
        {
            UpdateGeneric(id, updatedEntity);
            return true;
        }

        /// <summary>Updates a specific generic by its primary key</summary>
        /// <param name="id">The primary key of the generic</param>
        /// <param name="updatedEntity">The generic data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Patch(Guid id, JsonPatchDocument<Generic> updatedEntity)
        {
            PatchGeneric(id, updatedEntity);
            return true;
        }

        /// <summary>Deletes a specific generic by its primary key</summary>
        /// <param name="id">The primary key of the generic</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Delete(Guid id)
        {
            DeleteGeneric(id);
            return true;
        }
        #region
        private List<Generic> GetGeneric(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            if (pageSize < 1)
            {
                throw new ApplicationException("Page size invalid!");
            }

            if (pageNumber < 1)
            {
                throw new ApplicationException("Page mumber invalid!");
            }

            var query = _dbContext.Generic.IncludeRelated().AsQueryable();
            int skip = (pageNumber - 1) * pageSize;
            var result = FilterService<Generic>.ApplyFilter(query, filters, searchTerm);
            if (!string.IsNullOrEmpty(sortField))
            {
                var parameter = Expression.Parameter(typeof(Generic), "b");
                var property = Expression.Property(parameter, sortField);
                var lambda = Expression.Lambda<Func<Generic, object>>(Expression.Convert(property, typeof(object)), parameter);
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

        private Guid CreateGeneric(Generic model)
        {
            _dbContext.Generic.Add(model);
            _dbContext.SaveChanges();
            return model.Id;
        }

        private void UpdateGeneric(Guid id, Generic updatedEntity)
        {
            _dbContext.Generic.Update(updatedEntity);
            _dbContext.SaveChanges();
        }

        private bool DeleteGeneric(Guid id)
        {
            var entityData = _dbContext.Generic.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            if (entityData == null)
            {
                throw new ApplicationException("No data found!");
            }

            _dbContext.Generic.Remove(entityData);
            _dbContext.SaveChanges();
            return true;
        }

        private void PatchGeneric(Guid id, JsonPatchDocument<Generic> updatedEntity)
        {
            if (updatedEntity == null)
            {
                throw new ApplicationException("Patch document is missing!");
            }

            var existingEntity = _dbContext.Generic.FirstOrDefault(t => t.Id == id);
            if (existingEntity == null)
            {
                throw new ApplicationException("No data found!");
            }

            updatedEntity.ApplyTo(existingEntity);
            _dbContext.Generic.Update(existingEntity);
            _dbContext.SaveChanges();
        }
        #endregion
    }
}