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
    /// The bloodgroupService responsible for managing bloodgroup related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting bloodgroup information.
    /// </remarks>
    public interface IBloodGroupService
    {
        /// <summary>Retrieves a specific bloodgroup by its primary key</summary>
        /// <param name="id">The primary key of the bloodgroup</param>
        /// <returns>The bloodgroup data</returns>
        BloodGroup GetById(Guid id);

        /// <summary>Retrieves a list of bloodgroups based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of bloodgroups</returns>
        List<BloodGroup> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc");

        /// <summary>Adds a new bloodgroup</summary>
        /// <param name="model">The bloodgroup data to be added</param>
        /// <returns>The result of the operation</returns>
        Guid Create(BloodGroup model);

        /// <summary>Updates a specific bloodgroup by its primary key</summary>
        /// <param name="id">The primary key of the bloodgroup</param>
        /// <param name="updatedEntity">The bloodgroup data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Update(Guid id, BloodGroup updatedEntity);

        /// <summary>Updates a specific bloodgroup by its primary key</summary>
        /// <param name="id">The primary key of the bloodgroup</param>
        /// <param name="updatedEntity">The bloodgroup data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Patch(Guid id, JsonPatchDocument<BloodGroup> updatedEntity);

        /// <summary>Deletes a specific bloodgroup by its primary key</summary>
        /// <param name="id">The primary key of the bloodgroup</param>
        /// <returns>The result of the operation</returns>
        bool Delete(Guid id);
    }

    /// <summary>
    /// The bloodgroupService responsible for managing bloodgroup related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting bloodgroup information.
    /// </remarks>
    public class BloodGroupService : IBloodGroupService
    {
        private Demo17SepContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the BloodGroup class.
        /// </summary>
        /// <param name="dbContext">dbContext value to set.</param>
        public BloodGroupService(Demo17SepContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>Retrieves a specific bloodgroup by its primary key</summary>
        /// <param name="id">The primary key of the bloodgroup</param>
        /// <returns>The bloodgroup data</returns>
        public BloodGroup GetById(Guid id)
        {
            var entityData = _dbContext.BloodGroup.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            return entityData;
        }

        /// <summary>Retrieves a list of bloodgroups based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of bloodgroups</returns>/// <exception cref="Exception"></exception>
        public List<BloodGroup> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            var result = GetBloodGroup(filters, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return result;
        }

        /// <summary>Adds a new bloodgroup</summary>
        /// <param name="model">The bloodgroup data to be added</param>
        /// <returns>The result of the operation</returns>
        public Guid Create(BloodGroup model)
        {
            model.Id = CreateBloodGroup(model);
            return model.Id;
        }

        /// <summary>Updates a specific bloodgroup by its primary key</summary>
        /// <param name="id">The primary key of the bloodgroup</param>
        /// <param name="updatedEntity">The bloodgroup data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Update(Guid id, BloodGroup updatedEntity)
        {
            UpdateBloodGroup(id, updatedEntity);
            return true;
        }

        /// <summary>Updates a specific bloodgroup by its primary key</summary>
        /// <param name="id">The primary key of the bloodgroup</param>
        /// <param name="updatedEntity">The bloodgroup data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Patch(Guid id, JsonPatchDocument<BloodGroup> updatedEntity)
        {
            PatchBloodGroup(id, updatedEntity);
            return true;
        }

        /// <summary>Deletes a specific bloodgroup by its primary key</summary>
        /// <param name="id">The primary key of the bloodgroup</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Delete(Guid id)
        {
            DeleteBloodGroup(id);
            return true;
        }
        #region
        private List<BloodGroup> GetBloodGroup(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            if (pageSize < 1)
            {
                throw new ApplicationException("Page size invalid!");
            }

            if (pageNumber < 1)
            {
                throw new ApplicationException("Page mumber invalid!");
            }

            var query = _dbContext.BloodGroup.IncludeRelated().AsQueryable();
            int skip = (pageNumber - 1) * pageSize;
            var result = FilterService<BloodGroup>.ApplyFilter(query, filters, searchTerm);
            if (!string.IsNullOrEmpty(sortField))
            {
                var parameter = Expression.Parameter(typeof(BloodGroup), "b");
                var property = Expression.Property(parameter, sortField);
                var lambda = Expression.Lambda<Func<BloodGroup, object>>(Expression.Convert(property, typeof(object)), parameter);
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

        private Guid CreateBloodGroup(BloodGroup model)
        {
            _dbContext.BloodGroup.Add(model);
            _dbContext.SaveChanges();
            return model.Id;
        }

        private void UpdateBloodGroup(Guid id, BloodGroup updatedEntity)
        {
            _dbContext.BloodGroup.Update(updatedEntity);
            _dbContext.SaveChanges();
        }

        private bool DeleteBloodGroup(Guid id)
        {
            var entityData = _dbContext.BloodGroup.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            if (entityData == null)
            {
                throw new ApplicationException("No data found!");
            }

            _dbContext.BloodGroup.Remove(entityData);
            _dbContext.SaveChanges();
            return true;
        }

        private void PatchBloodGroup(Guid id, JsonPatchDocument<BloodGroup> updatedEntity)
        {
            if (updatedEntity == null)
            {
                throw new ApplicationException("Patch document is missing!");
            }

            var existingEntity = _dbContext.BloodGroup.FirstOrDefault(t => t.Id == id);
            if (existingEntity == null)
            {
                throw new ApplicationException("No data found!");
            }

            updatedEntity.ApplyTo(existingEntity);
            _dbContext.BloodGroup.Update(existingEntity);
            _dbContext.SaveChanges();
        }
        #endregion
    }
}