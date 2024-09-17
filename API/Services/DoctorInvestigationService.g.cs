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
    /// The doctorinvestigationService responsible for managing doctorinvestigation related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting doctorinvestigation information.
    /// </remarks>
    public interface IDoctorInvestigationService
    {
        /// <summary>Retrieves a specific doctorinvestigation by its primary key</summary>
        /// <param name="id">The primary key of the doctorinvestigation</param>
        /// <returns>The doctorinvestigation data</returns>
        DoctorInvestigation GetById(Guid id);

        /// <summary>Retrieves a list of doctorinvestigations based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of doctorinvestigations</returns>
        List<DoctorInvestigation> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc");

        /// <summary>Adds a new doctorinvestigation</summary>
        /// <param name="model">The doctorinvestigation data to be added</param>
        /// <returns>The result of the operation</returns>
        Guid Create(DoctorInvestigation model);

        /// <summary>Updates a specific doctorinvestigation by its primary key</summary>
        /// <param name="id">The primary key of the doctorinvestigation</param>
        /// <param name="updatedEntity">The doctorinvestigation data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Update(Guid id, DoctorInvestigation updatedEntity);

        /// <summary>Updates a specific doctorinvestigation by its primary key</summary>
        /// <param name="id">The primary key of the doctorinvestigation</param>
        /// <param name="updatedEntity">The doctorinvestigation data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Patch(Guid id, JsonPatchDocument<DoctorInvestigation> updatedEntity);

        /// <summary>Deletes a specific doctorinvestigation by its primary key</summary>
        /// <param name="id">The primary key of the doctorinvestigation</param>
        /// <returns>The result of the operation</returns>
        bool Delete(Guid id);
    }

    /// <summary>
    /// The doctorinvestigationService responsible for managing doctorinvestigation related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting doctorinvestigation information.
    /// </remarks>
    public class DoctorInvestigationService : IDoctorInvestigationService
    {
        private Demo17SepContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the DoctorInvestigation class.
        /// </summary>
        /// <param name="dbContext">dbContext value to set.</param>
        public DoctorInvestigationService(Demo17SepContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>Retrieves a specific doctorinvestigation by its primary key</summary>
        /// <param name="id">The primary key of the doctorinvestigation</param>
        /// <returns>The doctorinvestigation data</returns>
        public DoctorInvestigation GetById(Guid id)
        {
            var entityData = _dbContext.DoctorInvestigation.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            return entityData;
        }

        /// <summary>Retrieves a list of doctorinvestigations based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of doctorinvestigations</returns>/// <exception cref="Exception"></exception>
        public List<DoctorInvestigation> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            var result = GetDoctorInvestigation(filters, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return result;
        }

        /// <summary>Adds a new doctorinvestigation</summary>
        /// <param name="model">The doctorinvestigation data to be added</param>
        /// <returns>The result of the operation</returns>
        public Guid Create(DoctorInvestigation model)
        {
            model.Id = CreateDoctorInvestigation(model);
            return model.Id;
        }

        /// <summary>Updates a specific doctorinvestigation by its primary key</summary>
        /// <param name="id">The primary key of the doctorinvestigation</param>
        /// <param name="updatedEntity">The doctorinvestigation data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Update(Guid id, DoctorInvestigation updatedEntity)
        {
            UpdateDoctorInvestigation(id, updatedEntity);
            return true;
        }

        /// <summary>Updates a specific doctorinvestigation by its primary key</summary>
        /// <param name="id">The primary key of the doctorinvestigation</param>
        /// <param name="updatedEntity">The doctorinvestigation data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Patch(Guid id, JsonPatchDocument<DoctorInvestigation> updatedEntity)
        {
            PatchDoctorInvestigation(id, updatedEntity);
            return true;
        }

        /// <summary>Deletes a specific doctorinvestigation by its primary key</summary>
        /// <param name="id">The primary key of the doctorinvestigation</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Delete(Guid id)
        {
            DeleteDoctorInvestigation(id);
            return true;
        }
        #region
        private List<DoctorInvestigation> GetDoctorInvestigation(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            if (pageSize < 1)
            {
                throw new ApplicationException("Page size invalid!");
            }

            if (pageNumber < 1)
            {
                throw new ApplicationException("Page mumber invalid!");
            }

            var query = _dbContext.DoctorInvestigation.IncludeRelated().AsQueryable();
            int skip = (pageNumber - 1) * pageSize;
            var result = FilterService<DoctorInvestigation>.ApplyFilter(query, filters, searchTerm);
            if (!string.IsNullOrEmpty(sortField))
            {
                var parameter = Expression.Parameter(typeof(DoctorInvestigation), "b");
                var property = Expression.Property(parameter, sortField);
                var lambda = Expression.Lambda<Func<DoctorInvestigation, object>>(Expression.Convert(property, typeof(object)), parameter);
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

        private Guid CreateDoctorInvestigation(DoctorInvestigation model)
        {
            _dbContext.DoctorInvestigation.Add(model);
            _dbContext.SaveChanges();
            return model.Id;
        }

        private void UpdateDoctorInvestigation(Guid id, DoctorInvestigation updatedEntity)
        {
            _dbContext.DoctorInvestigation.Update(updatedEntity);
            _dbContext.SaveChanges();
        }

        private bool DeleteDoctorInvestigation(Guid id)
        {
            var entityData = _dbContext.DoctorInvestigation.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            if (entityData == null)
            {
                throw new ApplicationException("No data found!");
            }

            _dbContext.DoctorInvestigation.Remove(entityData);
            _dbContext.SaveChanges();
            return true;
        }

        private void PatchDoctorInvestigation(Guid id, JsonPatchDocument<DoctorInvestigation> updatedEntity)
        {
            if (updatedEntity == null)
            {
                throw new ApplicationException("Patch document is missing!");
            }

            var existingEntity = _dbContext.DoctorInvestigation.FirstOrDefault(t => t.Id == id);
            if (existingEntity == null)
            {
                throw new ApplicationException("No data found!");
            }

            updatedEntity.ApplyTo(existingEntity);
            _dbContext.DoctorInvestigation.Update(existingEntity);
            _dbContext.SaveChanges();
        }
        #endregion
    }
}