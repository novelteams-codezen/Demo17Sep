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
    /// The patientpregnancyService responsible for managing patientpregnancy related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting patientpregnancy information.
    /// </remarks>
    public interface IPatientPregnancyService
    {
        /// <summary>Retrieves a specific patientpregnancy by its primary key</summary>
        /// <param name="id">The primary key of the patientpregnancy</param>
        /// <returns>The patientpregnancy data</returns>
        PatientPregnancy GetById(Guid id);

        /// <summary>Retrieves a list of patientpregnancys based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of patientpregnancys</returns>
        List<PatientPregnancy> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc");

        /// <summary>Adds a new patientpregnancy</summary>
        /// <param name="model">The patientpregnancy data to be added</param>
        /// <returns>The result of the operation</returns>
        Guid Create(PatientPregnancy model);

        /// <summary>Updates a specific patientpregnancy by its primary key</summary>
        /// <param name="id">The primary key of the patientpregnancy</param>
        /// <param name="updatedEntity">The patientpregnancy data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Update(Guid id, PatientPregnancy updatedEntity);

        /// <summary>Updates a specific patientpregnancy by its primary key</summary>
        /// <param name="id">The primary key of the patientpregnancy</param>
        /// <param name="updatedEntity">The patientpregnancy data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Patch(Guid id, JsonPatchDocument<PatientPregnancy> updatedEntity);

        /// <summary>Deletes a specific patientpregnancy by its primary key</summary>
        /// <param name="id">The primary key of the patientpregnancy</param>
        /// <returns>The result of the operation</returns>
        bool Delete(Guid id);
    }

    /// <summary>
    /// The patientpregnancyService responsible for managing patientpregnancy related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting patientpregnancy information.
    /// </remarks>
    public class PatientPregnancyService : IPatientPregnancyService
    {
        private Demo17SepContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the PatientPregnancy class.
        /// </summary>
        /// <param name="dbContext">dbContext value to set.</param>
        public PatientPregnancyService(Demo17SepContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>Retrieves a specific patientpregnancy by its primary key</summary>
        /// <param name="id">The primary key of the patientpregnancy</param>
        /// <returns>The patientpregnancy data</returns>
        public PatientPregnancy GetById(Guid id)
        {
            var entityData = _dbContext.PatientPregnancy.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            return entityData;
        }

        /// <summary>Retrieves a list of patientpregnancys based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of patientpregnancys</returns>/// <exception cref="Exception"></exception>
        public List<PatientPregnancy> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            var result = GetPatientPregnancy(filters, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return result;
        }

        /// <summary>Adds a new patientpregnancy</summary>
        /// <param name="model">The patientpregnancy data to be added</param>
        /// <returns>The result of the operation</returns>
        public Guid Create(PatientPregnancy model)
        {
            model.Id = CreatePatientPregnancy(model);
            return model.Id;
        }

        /// <summary>Updates a specific patientpregnancy by its primary key</summary>
        /// <param name="id">The primary key of the patientpregnancy</param>
        /// <param name="updatedEntity">The patientpregnancy data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Update(Guid id, PatientPregnancy updatedEntity)
        {
            UpdatePatientPregnancy(id, updatedEntity);
            return true;
        }

        /// <summary>Updates a specific patientpregnancy by its primary key</summary>
        /// <param name="id">The primary key of the patientpregnancy</param>
        /// <param name="updatedEntity">The patientpregnancy data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Patch(Guid id, JsonPatchDocument<PatientPregnancy> updatedEntity)
        {
            PatchPatientPregnancy(id, updatedEntity);
            return true;
        }

        /// <summary>Deletes a specific patientpregnancy by its primary key</summary>
        /// <param name="id">The primary key of the patientpregnancy</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Delete(Guid id)
        {
            DeletePatientPregnancy(id);
            return true;
        }
        #region
        private List<PatientPregnancy> GetPatientPregnancy(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            if (pageSize < 1)
            {
                throw new ApplicationException("Page size invalid!");
            }

            if (pageNumber < 1)
            {
                throw new ApplicationException("Page mumber invalid!");
            }

            var query = _dbContext.PatientPregnancy.IncludeRelated().AsQueryable();
            int skip = (pageNumber - 1) * pageSize;
            var result = FilterService<PatientPregnancy>.ApplyFilter(query, filters, searchTerm);
            if (!string.IsNullOrEmpty(sortField))
            {
                var parameter = Expression.Parameter(typeof(PatientPregnancy), "b");
                var property = Expression.Property(parameter, sortField);
                var lambda = Expression.Lambda<Func<PatientPregnancy, object>>(Expression.Convert(property, typeof(object)), parameter);
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

        private Guid CreatePatientPregnancy(PatientPregnancy model)
        {
            _dbContext.PatientPregnancy.Add(model);
            _dbContext.SaveChanges();
            return model.Id;
        }

        private void UpdatePatientPregnancy(Guid id, PatientPregnancy updatedEntity)
        {
            _dbContext.PatientPregnancy.Update(updatedEntity);
            _dbContext.SaveChanges();
        }

        private bool DeletePatientPregnancy(Guid id)
        {
            var entityData = _dbContext.PatientPregnancy.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            if (entityData == null)
            {
                throw new ApplicationException("No data found!");
            }

            _dbContext.PatientPregnancy.Remove(entityData);
            _dbContext.SaveChanges();
            return true;
        }

        private void PatchPatientPregnancy(Guid id, JsonPatchDocument<PatientPregnancy> updatedEntity)
        {
            if (updatedEntity == null)
            {
                throw new ApplicationException("Patch document is missing!");
            }

            var existingEntity = _dbContext.PatientPregnancy.FirstOrDefault(t => t.Id == id);
            if (existingEntity == null)
            {
                throw new ApplicationException("No data found!");
            }

            updatedEntity.ApplyTo(existingEntity);
            _dbContext.PatientPregnancy.Update(existingEntity);
            _dbContext.SaveChanges();
        }
        #endregion
    }
}