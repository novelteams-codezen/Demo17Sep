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
    /// The comorbidityService responsible for managing comorbidity related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting comorbidity information.
    /// </remarks>
    public interface IComorbidityService
    {
        /// <summary>Retrieves a specific comorbidity by its primary key</summary>
        /// <param name="id">The primary key of the comorbidity</param>
        /// <returns>The comorbidity data</returns>
        Comorbidity GetById(Guid id);

        /// <summary>Retrieves a list of comorbiditys based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of comorbiditys</returns>
        List<Comorbidity> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc");

        /// <summary>Adds a new comorbidity</summary>
        /// <param name="model">The comorbidity data to be added</param>
        /// <returns>The result of the operation</returns>
        Guid Create(Comorbidity model);

        /// <summary>Updates a specific comorbidity by its primary key</summary>
        /// <param name="id">The primary key of the comorbidity</param>
        /// <param name="updatedEntity">The comorbidity data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Update(Guid id, Comorbidity updatedEntity);

        /// <summary>Updates a specific comorbidity by its primary key</summary>
        /// <param name="id">The primary key of the comorbidity</param>
        /// <param name="updatedEntity">The comorbidity data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Patch(Guid id, JsonPatchDocument<Comorbidity> updatedEntity);

        /// <summary>Deletes a specific comorbidity by its primary key</summary>
        /// <param name="id">The primary key of the comorbidity</param>
        /// <returns>The result of the operation</returns>
        bool Delete(Guid id);
    }

    /// <summary>
    /// The comorbidityService responsible for managing comorbidity related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting comorbidity information.
    /// </remarks>
    public class ComorbidityService : IComorbidityService
    {
        private Demo17SepContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the Comorbidity class.
        /// </summary>
        /// <param name="dbContext">dbContext value to set.</param>
        public ComorbidityService(Demo17SepContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>Retrieves a specific comorbidity by its primary key</summary>
        /// <param name="id">The primary key of the comorbidity</param>
        /// <returns>The comorbidity data</returns>
        public Comorbidity GetById(Guid id)
        {
            var entityData = _dbContext.Comorbidity.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            return entityData;
        }

        /// <summary>Retrieves a list of comorbiditys based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of comorbiditys</returns>/// <exception cref="Exception"></exception>
        public List<Comorbidity> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            var result = GetComorbidity(filters, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return result;
        }

        /// <summary>Adds a new comorbidity</summary>
        /// <param name="model">The comorbidity data to be added</param>
        /// <returns>The result of the operation</returns>
        public Guid Create(Comorbidity model)
        {
            model.Id = CreateComorbidity(model);
            return model.Id;
        }

        /// <summary>Updates a specific comorbidity by its primary key</summary>
        /// <param name="id">The primary key of the comorbidity</param>
        /// <param name="updatedEntity">The comorbidity data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Update(Guid id, Comorbidity updatedEntity)
        {
            UpdateComorbidity(id, updatedEntity);
            return true;
        }

        /// <summary>Updates a specific comorbidity by its primary key</summary>
        /// <param name="id">The primary key of the comorbidity</param>
        /// <param name="updatedEntity">The comorbidity data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Patch(Guid id, JsonPatchDocument<Comorbidity> updatedEntity)
        {
            PatchComorbidity(id, updatedEntity);
            return true;
        }

        /// <summary>Deletes a specific comorbidity by its primary key</summary>
        /// <param name="id">The primary key of the comorbidity</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Delete(Guid id)
        {
            DeleteComorbidity(id);
            return true;
        }
        #region
        private List<Comorbidity> GetComorbidity(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            if (pageSize < 1)
            {
                throw new ApplicationException("Page size invalid!");
            }

            if (pageNumber < 1)
            {
                throw new ApplicationException("Page mumber invalid!");
            }

            var query = _dbContext.Comorbidity.IncludeRelated().AsQueryable();
            int skip = (pageNumber - 1) * pageSize;
            var result = FilterService<Comorbidity>.ApplyFilter(query, filters, searchTerm);
            if (!string.IsNullOrEmpty(sortField))
            {
                var parameter = Expression.Parameter(typeof(Comorbidity), "b");
                var property = Expression.Property(parameter, sortField);
                var lambda = Expression.Lambda<Func<Comorbidity, object>>(Expression.Convert(property, typeof(object)), parameter);
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

        private Guid CreateComorbidity(Comorbidity model)
        {
            _dbContext.Comorbidity.Add(model);
            _dbContext.SaveChanges();
            return model.Id;
        }

        private void UpdateComorbidity(Guid id, Comorbidity updatedEntity)
        {
            _dbContext.Comorbidity.Update(updatedEntity);
            _dbContext.SaveChanges();
        }

        private bool DeleteComorbidity(Guid id)
        {
            var entityData = _dbContext.Comorbidity.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            if (entityData == null)
            {
                throw new ApplicationException("No data found!");
            }

            _dbContext.Comorbidity.Remove(entityData);
            _dbContext.SaveChanges();
            return true;
        }

        private void PatchComorbidity(Guid id, JsonPatchDocument<Comorbidity> updatedEntity)
        {
            if (updatedEntity == null)
            {
                throw new ApplicationException("Patch document is missing!");
            }

            var existingEntity = _dbContext.Comorbidity.FirstOrDefault(t => t.Id == id);
            if (existingEntity == null)
            {
                throw new ApplicationException("No data found!");
            }

            updatedEntity.ApplyTo(existingEntity);
            _dbContext.Comorbidity.Update(existingEntity);
            _dbContext.SaveChanges();
        }
        #endregion
    }
}