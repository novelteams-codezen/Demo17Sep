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
    /// The patientcategoryService responsible for managing patientcategory related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting patientcategory information.
    /// </remarks>
    public interface IPatientCategoryService
    {
        /// <summary>Retrieves a specific patientcategory by its primary key</summary>
        /// <param name="id">The primary key of the patientcategory</param>
        /// <returns>The patientcategory data</returns>
        PatientCategory GetById(Guid id);

        /// <summary>Retrieves a list of patientcategorys based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of patientcategorys</returns>
        List<PatientCategory> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc");

        /// <summary>Adds a new patientcategory</summary>
        /// <param name="model">The patientcategory data to be added</param>
        /// <returns>The result of the operation</returns>
        Guid Create(PatientCategory model);

        /// <summary>Updates a specific patientcategory by its primary key</summary>
        /// <param name="id">The primary key of the patientcategory</param>
        /// <param name="updatedEntity">The patientcategory data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Update(Guid id, PatientCategory updatedEntity);

        /// <summary>Updates a specific patientcategory by its primary key</summary>
        /// <param name="id">The primary key of the patientcategory</param>
        /// <param name="updatedEntity">The patientcategory data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Patch(Guid id, JsonPatchDocument<PatientCategory> updatedEntity);

        /// <summary>Deletes a specific patientcategory by its primary key</summary>
        /// <param name="id">The primary key of the patientcategory</param>
        /// <returns>The result of the operation</returns>
        bool Delete(Guid id);
    }

    /// <summary>
    /// The patientcategoryService responsible for managing patientcategory related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting patientcategory information.
    /// </remarks>
    public class PatientCategoryService : IPatientCategoryService
    {
        private Demo17SepContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the PatientCategory class.
        /// </summary>
        /// <param name="dbContext">dbContext value to set.</param>
        public PatientCategoryService(Demo17SepContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>Retrieves a specific patientcategory by its primary key</summary>
        /// <param name="id">The primary key of the patientcategory</param>
        /// <returns>The patientcategory data</returns>
        public PatientCategory GetById(Guid id)
        {
            var entityData = _dbContext.PatientCategory.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            return entityData;
        }

        /// <summary>Retrieves a list of patientcategorys based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of patientcategorys</returns>/// <exception cref="Exception"></exception>
        public List<PatientCategory> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            var result = GetPatientCategory(filters, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return result;
        }

        /// <summary>Adds a new patientcategory</summary>
        /// <param name="model">The patientcategory data to be added</param>
        /// <returns>The result of the operation</returns>
        public Guid Create(PatientCategory model)
        {
            model.Id = CreatePatientCategory(model);
            return model.Id;
        }

        /// <summary>Updates a specific patientcategory by its primary key</summary>
        /// <param name="id">The primary key of the patientcategory</param>
        /// <param name="updatedEntity">The patientcategory data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Update(Guid id, PatientCategory updatedEntity)
        {
            UpdatePatientCategory(id, updatedEntity);
            return true;
        }

        /// <summary>Updates a specific patientcategory by its primary key</summary>
        /// <param name="id">The primary key of the patientcategory</param>
        /// <param name="updatedEntity">The patientcategory data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Patch(Guid id, JsonPatchDocument<PatientCategory> updatedEntity)
        {
            PatchPatientCategory(id, updatedEntity);
            return true;
        }

        /// <summary>Deletes a specific patientcategory by its primary key</summary>
        /// <param name="id">The primary key of the patientcategory</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Delete(Guid id)
        {
            DeletePatientCategory(id);
            return true;
        }
        #region
        private List<PatientCategory> GetPatientCategory(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            if (pageSize < 1)
            {
                throw new ApplicationException("Page size invalid!");
            }

            if (pageNumber < 1)
            {
                throw new ApplicationException("Page mumber invalid!");
            }

            var query = _dbContext.PatientCategory.IncludeRelated().AsQueryable();
            int skip = (pageNumber - 1) * pageSize;
            var result = FilterService<PatientCategory>.ApplyFilter(query, filters, searchTerm);
            if (!string.IsNullOrEmpty(sortField))
            {
                var parameter = Expression.Parameter(typeof(PatientCategory), "b");
                var property = Expression.Property(parameter, sortField);
                var lambda = Expression.Lambda<Func<PatientCategory, object>>(Expression.Convert(property, typeof(object)), parameter);
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

        private Guid CreatePatientCategory(PatientCategory model)
        {
            _dbContext.PatientCategory.Add(model);
            _dbContext.SaveChanges();
            return model.Id;
        }

        private void UpdatePatientCategory(Guid id, PatientCategory updatedEntity)
        {
            _dbContext.PatientCategory.Update(updatedEntity);
            _dbContext.SaveChanges();
        }

        private bool DeletePatientCategory(Guid id)
        {
            var entityData = _dbContext.PatientCategory.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            if (entityData == null)
            {
                throw new ApplicationException("No data found!");
            }

            _dbContext.PatientCategory.Remove(entityData);
            _dbContext.SaveChanges();
            return true;
        }

        private void PatchPatientCategory(Guid id, JsonPatchDocument<PatientCategory> updatedEntity)
        {
            if (updatedEntity == null)
            {
                throw new ApplicationException("Patch document is missing!");
            }

            var existingEntity = _dbContext.PatientCategory.FirstOrDefault(t => t.Id == id);
            if (existingEntity == null)
            {
                throw new ApplicationException("No data found!");
            }

            updatedEntity.ApplyTo(existingEntity);
            _dbContext.PatientCategory.Update(existingEntity);
            _dbContext.SaveChanges();
        }
        #endregion
    }
}