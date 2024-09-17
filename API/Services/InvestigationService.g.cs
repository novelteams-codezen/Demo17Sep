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
    /// The investigationService responsible for managing investigation related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting investigation information.
    /// </remarks>
    public interface IInvestigationService
    {
        /// <summary>Retrieves a specific investigation by its primary key</summary>
        /// <param name="id">The primary key of the investigation</param>
        /// <returns>The investigation data</returns>
        Investigation GetById(Guid id);

        /// <summary>Retrieves a list of investigations based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of investigations</returns>
        List<Investigation> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc");

        /// <summary>Adds a new investigation</summary>
        /// <param name="model">The investigation data to be added</param>
        /// <returns>The result of the operation</returns>
        Guid Create(Investigation model);

        /// <summary>Updates a specific investigation by its primary key</summary>
        /// <param name="id">The primary key of the investigation</param>
        /// <param name="updatedEntity">The investigation data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Update(Guid id, Investigation updatedEntity);

        /// <summary>Updates a specific investigation by its primary key</summary>
        /// <param name="id">The primary key of the investigation</param>
        /// <param name="updatedEntity">The investigation data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Patch(Guid id, JsonPatchDocument<Investigation> updatedEntity);

        /// <summary>Deletes a specific investigation by its primary key</summary>
        /// <param name="id">The primary key of the investigation</param>
        /// <returns>The result of the operation</returns>
        bool Delete(Guid id);
    }

    /// <summary>
    /// The investigationService responsible for managing investigation related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting investigation information.
    /// </remarks>
    public class InvestigationService : IInvestigationService
    {
        private Demo17SepContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the Investigation class.
        /// </summary>
        /// <param name="dbContext">dbContext value to set.</param>
        public InvestigationService(Demo17SepContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>Retrieves a specific investigation by its primary key</summary>
        /// <param name="id">The primary key of the investigation</param>
        /// <returns>The investigation data</returns>
        public Investigation GetById(Guid id)
        {
            var entityData = _dbContext.Investigation.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            return entityData;
        }

        /// <summary>Retrieves a list of investigations based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of investigations</returns>/// <exception cref="Exception"></exception>
        public List<Investigation> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            var result = GetInvestigation(filters, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return result;
        }

        /// <summary>Adds a new investigation</summary>
        /// <param name="model">The investigation data to be added</param>
        /// <returns>The result of the operation</returns>
        public Guid Create(Investigation model)
        {
            model.Id = CreateInvestigation(model);
            return model.Id;
        }

        /// <summary>Updates a specific investigation by its primary key</summary>
        /// <param name="id">The primary key of the investigation</param>
        /// <param name="updatedEntity">The investigation data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Update(Guid id, Investigation updatedEntity)
        {
            UpdateInvestigation(id, updatedEntity);
            return true;
        }

        /// <summary>Updates a specific investigation by its primary key</summary>
        /// <param name="id">The primary key of the investigation</param>
        /// <param name="updatedEntity">The investigation data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Patch(Guid id, JsonPatchDocument<Investigation> updatedEntity)
        {
            PatchInvestigation(id, updatedEntity);
            return true;
        }

        /// <summary>Deletes a specific investigation by its primary key</summary>
        /// <param name="id">The primary key of the investigation</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Delete(Guid id)
        {
            DeleteInvestigation(id);
            return true;
        }
        #region
        private List<Investigation> GetInvestigation(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            if (pageSize < 1)
            {
                throw new ApplicationException("Page size invalid!");
            }

            if (pageNumber < 1)
            {
                throw new ApplicationException("Page mumber invalid!");
            }

            var query = _dbContext.Investigation.IncludeRelated().AsQueryable();
            int skip = (pageNumber - 1) * pageSize;
            var result = FilterService<Investigation>.ApplyFilter(query, filters, searchTerm);
            if (!string.IsNullOrEmpty(sortField))
            {
                var parameter = Expression.Parameter(typeof(Investigation), "b");
                var property = Expression.Property(parameter, sortField);
                var lambda = Expression.Lambda<Func<Investigation, object>>(Expression.Convert(property, typeof(object)), parameter);
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

        private Guid CreateInvestigation(Investigation model)
        {
            _dbContext.Investigation.Add(model);
            _dbContext.SaveChanges();
            return model.Id;
        }

        private void UpdateInvestigation(Guid id, Investigation updatedEntity)
        {
            _dbContext.Investigation.Update(updatedEntity);
            _dbContext.SaveChanges();
        }

        private bool DeleteInvestigation(Guid id)
        {
            var entityData = _dbContext.Investigation.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            if (entityData == null)
            {
                throw new ApplicationException("No data found!");
            }

            _dbContext.Investigation.Remove(entityData);
            _dbContext.SaveChanges();
            return true;
        }

        private void PatchInvestigation(Guid id, JsonPatchDocument<Investigation> updatedEntity)
        {
            if (updatedEntity == null)
            {
                throw new ApplicationException("Patch document is missing!");
            }

            var existingEntity = _dbContext.Investigation.FirstOrDefault(t => t.Id == id);
            if (existingEntity == null)
            {
                throw new ApplicationException("No data found!");
            }

            updatedEntity.ApplyTo(existingEntity);
            _dbContext.Investigation.Update(existingEntity);
            _dbContext.SaveChanges();
        }
        #endregion
    }
}