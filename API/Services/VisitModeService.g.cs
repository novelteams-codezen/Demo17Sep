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
    /// The visitmodeService responsible for managing visitmode related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting visitmode information.
    /// </remarks>
    public interface IVisitModeService
    {
        /// <summary>Retrieves a specific visitmode by its primary key</summary>
        /// <param name="id">The primary key of the visitmode</param>
        /// <returns>The visitmode data</returns>
        VisitMode GetById(Guid id);

        /// <summary>Retrieves a list of visitmodes based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of visitmodes</returns>
        List<VisitMode> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc");

        /// <summary>Adds a new visitmode</summary>
        /// <param name="model">The visitmode data to be added</param>
        /// <returns>The result of the operation</returns>
        Guid Create(VisitMode model);

        /// <summary>Updates a specific visitmode by its primary key</summary>
        /// <param name="id">The primary key of the visitmode</param>
        /// <param name="updatedEntity">The visitmode data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Update(Guid id, VisitMode updatedEntity);

        /// <summary>Updates a specific visitmode by its primary key</summary>
        /// <param name="id">The primary key of the visitmode</param>
        /// <param name="updatedEntity">The visitmode data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Patch(Guid id, JsonPatchDocument<VisitMode> updatedEntity);

        /// <summary>Deletes a specific visitmode by its primary key</summary>
        /// <param name="id">The primary key of the visitmode</param>
        /// <returns>The result of the operation</returns>
        bool Delete(Guid id);
    }

    /// <summary>
    /// The visitmodeService responsible for managing visitmode related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting visitmode information.
    /// </remarks>
    public class VisitModeService : IVisitModeService
    {
        private Demo17SepContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the VisitMode class.
        /// </summary>
        /// <param name="dbContext">dbContext value to set.</param>
        public VisitModeService(Demo17SepContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>Retrieves a specific visitmode by its primary key</summary>
        /// <param name="id">The primary key of the visitmode</param>
        /// <returns>The visitmode data</returns>
        public VisitMode GetById(Guid id)
        {
            var entityData = _dbContext.VisitMode.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            return entityData;
        }

        /// <summary>Retrieves a list of visitmodes based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of visitmodes</returns>/// <exception cref="Exception"></exception>
        public List<VisitMode> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            var result = GetVisitMode(filters, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return result;
        }

        /// <summary>Adds a new visitmode</summary>
        /// <param name="model">The visitmode data to be added</param>
        /// <returns>The result of the operation</returns>
        public Guid Create(VisitMode model)
        {
            model.Id = CreateVisitMode(model);
            return model.Id;
        }

        /// <summary>Updates a specific visitmode by its primary key</summary>
        /// <param name="id">The primary key of the visitmode</param>
        /// <param name="updatedEntity">The visitmode data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Update(Guid id, VisitMode updatedEntity)
        {
            UpdateVisitMode(id, updatedEntity);
            return true;
        }

        /// <summary>Updates a specific visitmode by its primary key</summary>
        /// <param name="id">The primary key of the visitmode</param>
        /// <param name="updatedEntity">The visitmode data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Patch(Guid id, JsonPatchDocument<VisitMode> updatedEntity)
        {
            PatchVisitMode(id, updatedEntity);
            return true;
        }

        /// <summary>Deletes a specific visitmode by its primary key</summary>
        /// <param name="id">The primary key of the visitmode</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Delete(Guid id)
        {
            DeleteVisitMode(id);
            return true;
        }
        #region
        private List<VisitMode> GetVisitMode(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            if (pageSize < 1)
            {
                throw new ApplicationException("Page size invalid!");
            }

            if (pageNumber < 1)
            {
                throw new ApplicationException("Page mumber invalid!");
            }

            var query = _dbContext.VisitMode.IncludeRelated().AsQueryable();
            int skip = (pageNumber - 1) * pageSize;
            var result = FilterService<VisitMode>.ApplyFilter(query, filters, searchTerm);
            if (!string.IsNullOrEmpty(sortField))
            {
                var parameter = Expression.Parameter(typeof(VisitMode), "b");
                var property = Expression.Property(parameter, sortField);
                var lambda = Expression.Lambda<Func<VisitMode, object>>(Expression.Convert(property, typeof(object)), parameter);
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

        private Guid CreateVisitMode(VisitMode model)
        {
            _dbContext.VisitMode.Add(model);
            _dbContext.SaveChanges();
            return model.Id;
        }

        private void UpdateVisitMode(Guid id, VisitMode updatedEntity)
        {
            _dbContext.VisitMode.Update(updatedEntity);
            _dbContext.SaveChanges();
        }

        private bool DeleteVisitMode(Guid id)
        {
            var entityData = _dbContext.VisitMode.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            if (entityData == null)
            {
                throw new ApplicationException("No data found!");
            }

            _dbContext.VisitMode.Remove(entityData);
            _dbContext.SaveChanges();
            return true;
        }

        private void PatchVisitMode(Guid id, JsonPatchDocument<VisitMode> updatedEntity)
        {
            if (updatedEntity == null)
            {
                throw new ApplicationException("Patch document is missing!");
            }

            var existingEntity = _dbContext.VisitMode.FirstOrDefault(t => t.Id == id);
            if (existingEntity == null)
            {
                throw new ApplicationException("No data found!");
            }

            updatedEntity.ApplyTo(existingEntity);
            _dbContext.VisitMode.Update(existingEntity);
            _dbContext.SaveChanges();
        }
        #endregion
    }
}