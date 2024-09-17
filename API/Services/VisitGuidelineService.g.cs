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
    /// The visitguidelineService responsible for managing visitguideline related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting visitguideline information.
    /// </remarks>
    public interface IVisitGuidelineService
    {
        /// <summary>Retrieves a specific visitguideline by its primary key</summary>
        /// <param name="id">The primary key of the visitguideline</param>
        /// <returns>The visitguideline data</returns>
        VisitGuideline GetById(Guid id);

        /// <summary>Retrieves a list of visitguidelines based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of visitguidelines</returns>
        List<VisitGuideline> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc");

        /// <summary>Adds a new visitguideline</summary>
        /// <param name="model">The visitguideline data to be added</param>
        /// <returns>The result of the operation</returns>
        Guid Create(VisitGuideline model);

        /// <summary>Updates a specific visitguideline by its primary key</summary>
        /// <param name="id">The primary key of the visitguideline</param>
        /// <param name="updatedEntity">The visitguideline data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Update(Guid id, VisitGuideline updatedEntity);

        /// <summary>Updates a specific visitguideline by its primary key</summary>
        /// <param name="id">The primary key of the visitguideline</param>
        /// <param name="updatedEntity">The visitguideline data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Patch(Guid id, JsonPatchDocument<VisitGuideline> updatedEntity);

        /// <summary>Deletes a specific visitguideline by its primary key</summary>
        /// <param name="id">The primary key of the visitguideline</param>
        /// <returns>The result of the operation</returns>
        bool Delete(Guid id);
    }

    /// <summary>
    /// The visitguidelineService responsible for managing visitguideline related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting visitguideline information.
    /// </remarks>
    public class VisitGuidelineService : IVisitGuidelineService
    {
        private Demo17SepContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the VisitGuideline class.
        /// </summary>
        /// <param name="dbContext">dbContext value to set.</param>
        public VisitGuidelineService(Demo17SepContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>Retrieves a specific visitguideline by its primary key</summary>
        /// <param name="id">The primary key of the visitguideline</param>
        /// <returns>The visitguideline data</returns>
        public VisitGuideline GetById(Guid id)
        {
            var entityData = _dbContext.VisitGuideline.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            return entityData;
        }

        /// <summary>Retrieves a list of visitguidelines based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of visitguidelines</returns>/// <exception cref="Exception"></exception>
        public List<VisitGuideline> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            var result = GetVisitGuideline(filters, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return result;
        }

        /// <summary>Adds a new visitguideline</summary>
        /// <param name="model">The visitguideline data to be added</param>
        /// <returns>The result of the operation</returns>
        public Guid Create(VisitGuideline model)
        {
            model.Id = CreateVisitGuideline(model);
            return model.Id;
        }

        /// <summary>Updates a specific visitguideline by its primary key</summary>
        /// <param name="id">The primary key of the visitguideline</param>
        /// <param name="updatedEntity">The visitguideline data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Update(Guid id, VisitGuideline updatedEntity)
        {
            UpdateVisitGuideline(id, updatedEntity);
            return true;
        }

        /// <summary>Updates a specific visitguideline by its primary key</summary>
        /// <param name="id">The primary key of the visitguideline</param>
        /// <param name="updatedEntity">The visitguideline data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Patch(Guid id, JsonPatchDocument<VisitGuideline> updatedEntity)
        {
            PatchVisitGuideline(id, updatedEntity);
            return true;
        }

        /// <summary>Deletes a specific visitguideline by its primary key</summary>
        /// <param name="id">The primary key of the visitguideline</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Delete(Guid id)
        {
            DeleteVisitGuideline(id);
            return true;
        }
        #region
        private List<VisitGuideline> GetVisitGuideline(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            if (pageSize < 1)
            {
                throw new ApplicationException("Page size invalid!");
            }

            if (pageNumber < 1)
            {
                throw new ApplicationException("Page mumber invalid!");
            }

            var query = _dbContext.VisitGuideline.IncludeRelated().AsQueryable();
            int skip = (pageNumber - 1) * pageSize;
            var result = FilterService<VisitGuideline>.ApplyFilter(query, filters, searchTerm);
            if (!string.IsNullOrEmpty(sortField))
            {
                var parameter = Expression.Parameter(typeof(VisitGuideline), "b");
                var property = Expression.Property(parameter, sortField);
                var lambda = Expression.Lambda<Func<VisitGuideline, object>>(Expression.Convert(property, typeof(object)), parameter);
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

        private Guid CreateVisitGuideline(VisitGuideline model)
        {
            _dbContext.VisitGuideline.Add(model);
            _dbContext.SaveChanges();
            return model.Id;
        }

        private void UpdateVisitGuideline(Guid id, VisitGuideline updatedEntity)
        {
            _dbContext.VisitGuideline.Update(updatedEntity);
            _dbContext.SaveChanges();
        }

        private bool DeleteVisitGuideline(Guid id)
        {
            var entityData = _dbContext.VisitGuideline.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            if (entityData == null)
            {
                throw new ApplicationException("No data found!");
            }

            _dbContext.VisitGuideline.Remove(entityData);
            _dbContext.SaveChanges();
            return true;
        }

        private void PatchVisitGuideline(Guid id, JsonPatchDocument<VisitGuideline> updatedEntity)
        {
            if (updatedEntity == null)
            {
                throw new ApplicationException("Patch document is missing!");
            }

            var existingEntity = _dbContext.VisitGuideline.FirstOrDefault(t => t.Id == id);
            if (existingEntity == null)
            {
                throw new ApplicationException("No data found!");
            }

            updatedEntity.ApplyTo(existingEntity);
            _dbContext.VisitGuideline.Update(existingEntity);
            _dbContext.SaveChanges();
        }
        #endregion
    }
}