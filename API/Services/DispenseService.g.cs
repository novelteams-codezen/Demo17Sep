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
    /// The dispenseService responsible for managing dispense related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting dispense information.
    /// </remarks>
    public interface IDispenseService
    {
        /// <summary>Retrieves a specific dispense by its primary key</summary>
        /// <param name="id">The primary key of the dispense</param>
        /// <returns>The dispense data</returns>
        Dispense GetById(Guid id);

        /// <summary>Retrieves a list of dispenses based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of dispenses</returns>
        List<Dispense> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc");

        /// <summary>Adds a new dispense</summary>
        /// <param name="model">The dispense data to be added</param>
        /// <returns>The result of the operation</returns>
        Guid Create(Dispense model);

        /// <summary>Updates a specific dispense by its primary key</summary>
        /// <param name="id">The primary key of the dispense</param>
        /// <param name="updatedEntity">The dispense data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Update(Guid id, Dispense updatedEntity);

        /// <summary>Updates a specific dispense by its primary key</summary>
        /// <param name="id">The primary key of the dispense</param>
        /// <param name="updatedEntity">The dispense data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Patch(Guid id, JsonPatchDocument<Dispense> updatedEntity);

        /// <summary>Deletes a specific dispense by its primary key</summary>
        /// <param name="id">The primary key of the dispense</param>
        /// <returns>The result of the operation</returns>
        bool Delete(Guid id);
    }

    /// <summary>
    /// The dispenseService responsible for managing dispense related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting dispense information.
    /// </remarks>
    public class DispenseService : IDispenseService
    {
        private Demo17SepContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the Dispense class.
        /// </summary>
        /// <param name="dbContext">dbContext value to set.</param>
        public DispenseService(Demo17SepContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>Retrieves a specific dispense by its primary key</summary>
        /// <param name="id">The primary key of the dispense</param>
        /// <returns>The dispense data</returns>
        public Dispense GetById(Guid id)
        {
            var entityData = _dbContext.Dispense.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            return entityData;
        }

        /// <summary>Retrieves a list of dispenses based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of dispenses</returns>/// <exception cref="Exception"></exception>
        public List<Dispense> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            var result = GetDispense(filters, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return result;
        }

        /// <summary>Adds a new dispense</summary>
        /// <param name="model">The dispense data to be added</param>
        /// <returns>The result of the operation</returns>
        public Guid Create(Dispense model)
        {
            model.Id = CreateDispense(model);
            return model.Id;
        }

        /// <summary>Updates a specific dispense by its primary key</summary>
        /// <param name="id">The primary key of the dispense</param>
        /// <param name="updatedEntity">The dispense data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Update(Guid id, Dispense updatedEntity)
        {
            UpdateDispense(id, updatedEntity);
            return true;
        }

        /// <summary>Updates a specific dispense by its primary key</summary>
        /// <param name="id">The primary key of the dispense</param>
        /// <param name="updatedEntity">The dispense data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Patch(Guid id, JsonPatchDocument<Dispense> updatedEntity)
        {
            PatchDispense(id, updatedEntity);
            return true;
        }

        /// <summary>Deletes a specific dispense by its primary key</summary>
        /// <param name="id">The primary key of the dispense</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Delete(Guid id)
        {
            DeleteDispense(id);
            return true;
        }
        #region
        private List<Dispense> GetDispense(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            if (pageSize < 1)
            {
                throw new ApplicationException("Page size invalid!");
            }

            if (pageNumber < 1)
            {
                throw new ApplicationException("Page mumber invalid!");
            }

            var query = _dbContext.Dispense.IncludeRelated().AsQueryable();
            int skip = (pageNumber - 1) * pageSize;
            var result = FilterService<Dispense>.ApplyFilter(query, filters, searchTerm);
            if (!string.IsNullOrEmpty(sortField))
            {
                var parameter = Expression.Parameter(typeof(Dispense), "b");
                var property = Expression.Property(parameter, sortField);
                var lambda = Expression.Lambda<Func<Dispense, object>>(Expression.Convert(property, typeof(object)), parameter);
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

        private Guid CreateDispense(Dispense model)
        {
            _dbContext.Dispense.Add(model);
            _dbContext.SaveChanges();
            return model.Id;
        }

        private void UpdateDispense(Guid id, Dispense updatedEntity)
        {
            _dbContext.Dispense.Update(updatedEntity);
            _dbContext.SaveChanges();
        }

        private bool DeleteDispense(Guid id)
        {
            var entityData = _dbContext.Dispense.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            if (entityData == null)
            {
                throw new ApplicationException("No data found!");
            }

            _dbContext.Dispense.Remove(entityData);
            _dbContext.SaveChanges();
            return true;
        }

        private void PatchDispense(Guid id, JsonPatchDocument<Dispense> updatedEntity)
        {
            if (updatedEntity == null)
            {
                throw new ApplicationException("Patch document is missing!");
            }

            var existingEntity = _dbContext.Dispense.FirstOrDefault(t => t.Id == id);
            if (existingEntity == null)
            {
                throw new ApplicationException("No data found!");
            }

            updatedEntity.ApplyTo(existingEntity);
            _dbContext.Dispense.Update(existingEntity);
            _dbContext.SaveChanges();
        }
        #endregion
    }
}