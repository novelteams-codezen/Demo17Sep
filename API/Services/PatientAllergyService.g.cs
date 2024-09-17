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
    /// The patientallergyService responsible for managing patientallergy related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting patientallergy information.
    /// </remarks>
    public interface IPatientAllergyService
    {
        /// <summary>Retrieves a specific patientallergy by its primary key</summary>
        /// <param name="id">The primary key of the patientallergy</param>
        /// <returns>The patientallergy data</returns>
        PatientAllergy GetById(Guid id);

        /// <summary>Retrieves a list of patientallergys based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of patientallergys</returns>
        List<PatientAllergy> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc");

        /// <summary>Adds a new patientallergy</summary>
        /// <param name="model">The patientallergy data to be added</param>
        /// <returns>The result of the operation</returns>
        Guid Create(PatientAllergy model);

        /// <summary>Updates a specific patientallergy by its primary key</summary>
        /// <param name="id">The primary key of the patientallergy</param>
        /// <param name="updatedEntity">The patientallergy data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Update(Guid id, PatientAllergy updatedEntity);

        /// <summary>Updates a specific patientallergy by its primary key</summary>
        /// <param name="id">The primary key of the patientallergy</param>
        /// <param name="updatedEntity">The patientallergy data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Patch(Guid id, JsonPatchDocument<PatientAllergy> updatedEntity);

        /// <summary>Deletes a specific patientallergy by its primary key</summary>
        /// <param name="id">The primary key of the patientallergy</param>
        /// <returns>The result of the operation</returns>
        bool Delete(Guid id);
    }

    /// <summary>
    /// The patientallergyService responsible for managing patientallergy related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting patientallergy information.
    /// </remarks>
    public class PatientAllergyService : IPatientAllergyService
    {
        private Demo17SepContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the PatientAllergy class.
        /// </summary>
        /// <param name="dbContext">dbContext value to set.</param>
        public PatientAllergyService(Demo17SepContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>Retrieves a specific patientallergy by its primary key</summary>
        /// <param name="id">The primary key of the patientallergy</param>
        /// <returns>The patientallergy data</returns>
        public PatientAllergy GetById(Guid id)
        {
            var entityData = _dbContext.PatientAllergy.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            return entityData;
        }

        /// <summary>Retrieves a list of patientallergys based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of patientallergys</returns>/// <exception cref="Exception"></exception>
        public List<PatientAllergy> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            var result = GetPatientAllergy(filters, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return result;
        }

        /// <summary>Adds a new patientallergy</summary>
        /// <param name="model">The patientallergy data to be added</param>
        /// <returns>The result of the operation</returns>
        public Guid Create(PatientAllergy model)
        {
            model.Id = CreatePatientAllergy(model);
            return model.Id;
        }

        /// <summary>Updates a specific patientallergy by its primary key</summary>
        /// <param name="id">The primary key of the patientallergy</param>
        /// <param name="updatedEntity">The patientallergy data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Update(Guid id, PatientAllergy updatedEntity)
        {
            UpdatePatientAllergy(id, updatedEntity);
            return true;
        }

        /// <summary>Updates a specific patientallergy by its primary key</summary>
        /// <param name="id">The primary key of the patientallergy</param>
        /// <param name="updatedEntity">The patientallergy data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Patch(Guid id, JsonPatchDocument<PatientAllergy> updatedEntity)
        {
            PatchPatientAllergy(id, updatedEntity);
            return true;
        }

        /// <summary>Deletes a specific patientallergy by its primary key</summary>
        /// <param name="id">The primary key of the patientallergy</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Delete(Guid id)
        {
            DeletePatientAllergy(id);
            return true;
        }
        #region
        private List<PatientAllergy> GetPatientAllergy(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            if (pageSize < 1)
            {
                throw new ApplicationException("Page size invalid!");
            }

            if (pageNumber < 1)
            {
                throw new ApplicationException("Page mumber invalid!");
            }

            var query = _dbContext.PatientAllergy.IncludeRelated().AsQueryable();
            int skip = (pageNumber - 1) * pageSize;
            var result = FilterService<PatientAllergy>.ApplyFilter(query, filters, searchTerm);
            if (!string.IsNullOrEmpty(sortField))
            {
                var parameter = Expression.Parameter(typeof(PatientAllergy), "b");
                var property = Expression.Property(parameter, sortField);
                var lambda = Expression.Lambda<Func<PatientAllergy, object>>(Expression.Convert(property, typeof(object)), parameter);
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

        private Guid CreatePatientAllergy(PatientAllergy model)
        {
            _dbContext.PatientAllergy.Add(model);
            _dbContext.SaveChanges();
            return model.Id;
        }

        private void UpdatePatientAllergy(Guid id, PatientAllergy updatedEntity)
        {
            _dbContext.PatientAllergy.Update(updatedEntity);
            _dbContext.SaveChanges();
        }

        private bool DeletePatientAllergy(Guid id)
        {
            var entityData = _dbContext.PatientAllergy.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            if (entityData == null)
            {
                throw new ApplicationException("No data found!");
            }

            _dbContext.PatientAllergy.Remove(entityData);
            _dbContext.SaveChanges();
            return true;
        }

        private void PatchPatientAllergy(Guid id, JsonPatchDocument<PatientAllergy> updatedEntity)
        {
            if (updatedEntity == null)
            {
                throw new ApplicationException("Patch document is missing!");
            }

            var existingEntity = _dbContext.PatientAllergy.FirstOrDefault(t => t.Id == id);
            if (existingEntity == null)
            {
                throw new ApplicationException("No data found!");
            }

            updatedEntity.ApplyTo(existingEntity);
            _dbContext.PatientAllergy.Update(existingEntity);
            _dbContext.SaveChanges();
        }
        #endregion
    }
}