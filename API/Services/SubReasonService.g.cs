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
    /// The subreasonService responsible for managing subreason related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting subreason information.
    /// </remarks>
    public interface ISubReasonService
    {
        /// <summary>Retrieves a specific subreason by its primary key</summary>
        /// <param name="id">The primary key of the subreason</param>
        /// <returns>The subreason data</returns>
        SubReason GetById(Guid id);

        /// <summary>Retrieves a list of subreasons based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of subreasons</returns>
        List<SubReason> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc");

        /// <summary>Adds a new subreason</summary>
        /// <param name="model">The subreason data to be added</param>
        /// <returns>The result of the operation</returns>
        Guid Create(SubReason model);

        /// <summary>Updates a specific subreason by its primary key</summary>
        /// <param name="id">The primary key of the subreason</param>
        /// <param name="updatedEntity">The subreason data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Update(Guid id, SubReason updatedEntity);

        /// <summary>Updates a specific subreason by its primary key</summary>
        /// <param name="id">The primary key of the subreason</param>
        /// <param name="updatedEntity">The subreason data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Patch(Guid id, JsonPatchDocument<SubReason> updatedEntity);

        /// <summary>Deletes a specific subreason by its primary key</summary>
        /// <param name="id">The primary key of the subreason</param>
        /// <returns>The result of the operation</returns>
        bool Delete(Guid id);
    }

    /// <summary>
    /// The subreasonService responsible for managing subreason related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting subreason information.
    /// </remarks>
    public class SubReasonService : ISubReasonService
    {
        private Demo17SepContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the SubReason class.
        /// </summary>
        /// <param name="dbContext">dbContext value to set.</param>
        public SubReasonService(Demo17SepContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>Retrieves a specific subreason by its primary key</summary>
        /// <param name="id">The primary key of the subreason</param>
        /// <returns>The subreason data</returns>
        public SubReason GetById(Guid id)
        {
            var entityData = _dbContext.SubReason.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            return entityData;
        }

        /// <summary>Retrieves a list of subreasons based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of subreasons</returns>/// <exception cref="Exception"></exception>
        public List<SubReason> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            var result = GetSubReason(filters, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return result;
        }

        /// <summary>Adds a new subreason</summary>
        /// <param name="model">The subreason data to be added</param>
        /// <returns>The result of the operation</returns>
        public Guid Create(SubReason model)
        {
            model.Id = CreateSubReason(model);
            return model.Id;
        }

        /// <summary>Updates a specific subreason by its primary key</summary>
        /// <param name="id">The primary key of the subreason</param>
        /// <param name="updatedEntity">The subreason data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Update(Guid id, SubReason updatedEntity)
        {
            UpdateSubReason(id, updatedEntity);
            return true;
        }

        /// <summary>Updates a specific subreason by its primary key</summary>
        /// <param name="id">The primary key of the subreason</param>
        /// <param name="updatedEntity">The subreason data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Patch(Guid id, JsonPatchDocument<SubReason> updatedEntity)
        {
            PatchSubReason(id, updatedEntity);
            return true;
        }

        /// <summary>Deletes a specific subreason by its primary key</summary>
        /// <param name="id">The primary key of the subreason</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Delete(Guid id)
        {
            DeleteSubReason(id);
            return true;
        }
        #region
        private List<SubReason> GetSubReason(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            if (pageSize < 1)
            {
                throw new ApplicationException("Page size invalid!");
            }

            if (pageNumber < 1)
            {
                throw new ApplicationException("Page mumber invalid!");
            }

            var query = _dbContext.SubReason.IncludeRelated().AsQueryable();
            int skip = (pageNumber - 1) * pageSize;
            var result = FilterService<SubReason>.ApplyFilter(query, filters, searchTerm);
            if (!string.IsNullOrEmpty(sortField))
            {
                var parameter = Expression.Parameter(typeof(SubReason), "b");
                var property = Expression.Property(parameter, sortField);
                var lambda = Expression.Lambda<Func<SubReason, object>>(Expression.Convert(property, typeof(object)), parameter);
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

        private Guid CreateSubReason(SubReason model)
        {
            _dbContext.SubReason.Add(model);
            _dbContext.SaveChanges();
            return model.Id;
        }

        private void UpdateSubReason(Guid id, SubReason updatedEntity)
        {
            _dbContext.SubReason.Update(updatedEntity);
            _dbContext.SaveChanges();
        }

        private bool DeleteSubReason(Guid id)
        {
            var entityData = _dbContext.SubReason.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            if (entityData == null)
            {
                throw new ApplicationException("No data found!");
            }

            _dbContext.SubReason.Remove(entityData);
            _dbContext.SaveChanges();
            return true;
        }

        private void PatchSubReason(Guid id, JsonPatchDocument<SubReason> updatedEntity)
        {
            if (updatedEntity == null)
            {
                throw new ApplicationException("Patch document is missing!");
            }

            var existingEntity = _dbContext.SubReason.FirstOrDefault(t => t.Id == id);
            if (existingEntity == null)
            {
                throw new ApplicationException("No data found!");
            }

            updatedEntity.ApplyTo(existingEntity);
            _dbContext.SubReason.Update(existingEntity);
            _dbContext.SaveChanges();
        }
        #endregion
    }
}