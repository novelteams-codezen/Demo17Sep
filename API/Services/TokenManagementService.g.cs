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
    /// The tokenmanagementService responsible for managing tokenmanagement related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting tokenmanagement information.
    /// </remarks>
    public interface ITokenManagementService
    {
        /// <summary>Retrieves a specific tokenmanagement by its primary key</summary>
        /// <param name="id">The primary key of the tokenmanagement</param>
        /// <returns>The tokenmanagement data</returns>
        TokenManagement GetById(Guid id);

        /// <summary>Retrieves a list of tokenmanagements based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of tokenmanagements</returns>
        List<TokenManagement> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc");

        /// <summary>Adds a new tokenmanagement</summary>
        /// <param name="model">The tokenmanagement data to be added</param>
        /// <returns>The result of the operation</returns>
        Guid Create(TokenManagement model);

        /// <summary>Updates a specific tokenmanagement by its primary key</summary>
        /// <param name="id">The primary key of the tokenmanagement</param>
        /// <param name="updatedEntity">The tokenmanagement data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Update(Guid id, TokenManagement updatedEntity);

        /// <summary>Updates a specific tokenmanagement by its primary key</summary>
        /// <param name="id">The primary key of the tokenmanagement</param>
        /// <param name="updatedEntity">The tokenmanagement data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Patch(Guid id, JsonPatchDocument<TokenManagement> updatedEntity);

        /// <summary>Deletes a specific tokenmanagement by its primary key</summary>
        /// <param name="id">The primary key of the tokenmanagement</param>
        /// <returns>The result of the operation</returns>
        bool Delete(Guid id);
    }

    /// <summary>
    /// The tokenmanagementService responsible for managing tokenmanagement related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting tokenmanagement information.
    /// </remarks>
    public class TokenManagementService : ITokenManagementService
    {
        private Demo17SepContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the TokenManagement class.
        /// </summary>
        /// <param name="dbContext">dbContext value to set.</param>
        public TokenManagementService(Demo17SepContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>Retrieves a specific tokenmanagement by its primary key</summary>
        /// <param name="id">The primary key of the tokenmanagement</param>
        /// <returns>The tokenmanagement data</returns>
        public TokenManagement GetById(Guid id)
        {
            var entityData = _dbContext.TokenManagement.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            return entityData;
        }

        /// <summary>Retrieves a list of tokenmanagements based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of tokenmanagements</returns>/// <exception cref="Exception"></exception>
        public List<TokenManagement> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            var result = GetTokenManagement(filters, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return result;
        }

        /// <summary>Adds a new tokenmanagement</summary>
        /// <param name="model">The tokenmanagement data to be added</param>
        /// <returns>The result of the operation</returns>
        public Guid Create(TokenManagement model)
        {
            model.Id = CreateTokenManagement(model);
            return model.Id;
        }

        /// <summary>Updates a specific tokenmanagement by its primary key</summary>
        /// <param name="id">The primary key of the tokenmanagement</param>
        /// <param name="updatedEntity">The tokenmanagement data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Update(Guid id, TokenManagement updatedEntity)
        {
            UpdateTokenManagement(id, updatedEntity);
            return true;
        }

        /// <summary>Updates a specific tokenmanagement by its primary key</summary>
        /// <param name="id">The primary key of the tokenmanagement</param>
        /// <param name="updatedEntity">The tokenmanagement data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Patch(Guid id, JsonPatchDocument<TokenManagement> updatedEntity)
        {
            PatchTokenManagement(id, updatedEntity);
            return true;
        }

        /// <summary>Deletes a specific tokenmanagement by its primary key</summary>
        /// <param name="id">The primary key of the tokenmanagement</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Delete(Guid id)
        {
            DeleteTokenManagement(id);
            return true;
        }
        #region
        private List<TokenManagement> GetTokenManagement(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            if (pageSize < 1)
            {
                throw new ApplicationException("Page size invalid!");
            }

            if (pageNumber < 1)
            {
                throw new ApplicationException("Page mumber invalid!");
            }

            var query = _dbContext.TokenManagement.IncludeRelated().AsQueryable();
            int skip = (pageNumber - 1) * pageSize;
            var result = FilterService<TokenManagement>.ApplyFilter(query, filters, searchTerm);
            if (!string.IsNullOrEmpty(sortField))
            {
                var parameter = Expression.Parameter(typeof(TokenManagement), "b");
                var property = Expression.Property(parameter, sortField);
                var lambda = Expression.Lambda<Func<TokenManagement, object>>(Expression.Convert(property, typeof(object)), parameter);
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

        private Guid CreateTokenManagement(TokenManagement model)
        {
            _dbContext.TokenManagement.Add(model);
            _dbContext.SaveChanges();
            return model.Id;
        }

        private void UpdateTokenManagement(Guid id, TokenManagement updatedEntity)
        {
            _dbContext.TokenManagement.Update(updatedEntity);
            _dbContext.SaveChanges();
        }

        private bool DeleteTokenManagement(Guid id)
        {
            var entityData = _dbContext.TokenManagement.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            if (entityData == null)
            {
                throw new ApplicationException("No data found!");
            }

            _dbContext.TokenManagement.Remove(entityData);
            _dbContext.SaveChanges();
            return true;
        }

        private void PatchTokenManagement(Guid id, JsonPatchDocument<TokenManagement> updatedEntity)
        {
            if (updatedEntity == null)
            {
                throw new ApplicationException("Patch document is missing!");
            }

            var existingEntity = _dbContext.TokenManagement.FirstOrDefault(t => t.Id == id);
            if (existingEntity == null)
            {
                throw new ApplicationException("No data found!");
            }

            updatedEntity.ApplyTo(existingEntity);
            _dbContext.TokenManagement.Update(existingEntity);
            _dbContext.SaveChanges();
        }
        #endregion
    }
}