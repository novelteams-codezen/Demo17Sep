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
    /// The patientService responsible for managing patient related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting patient information.
    /// </remarks>
    public interface IPatientService
    {
        /// <summary>Retrieves a specific patient by its primary key</summary>
        /// <param name="id">The primary key of the patient</param>
        /// <returns>The patient data</returns>
        Patient GetById(Guid id);

        /// <summary>Retrieves a list of patients based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of patients</returns>
        List<Patient> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc");

        /// <summary>Adds a new patient</summary>
        /// <param name="model">The patient data to be added</param>
        /// <returns>The result of the operation</returns>
        Guid Create(Patient model);

        /// <summary>Updates a specific patient by its primary key</summary>
        /// <param name="id">The primary key of the patient</param>
        /// <param name="updatedEntity">The patient data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Update(Guid id, Patient updatedEntity);

        /// <summary>Updates a specific patient by its primary key</summary>
        /// <param name="id">The primary key of the patient</param>
        /// <param name="updatedEntity">The patient data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Patch(Guid id, JsonPatchDocument<Patient> updatedEntity);

        /// <summary>Deletes a specific patient by its primary key</summary>
        /// <param name="id">The primary key of the patient</param>
        /// <returns>The result of the operation</returns>
        bool Delete(Guid id);
    }

    /// <summary>
    /// The patientService responsible for managing patient related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting patient information.
    /// </remarks>
    public class PatientService : IPatientService
    {
        private Demo17SepContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the Patient class.
        /// </summary>
        /// <param name="dbContext">dbContext value to set.</param>
        public PatientService(Demo17SepContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>Retrieves a specific patient by its primary key</summary>
        /// <param name="id">The primary key of the patient</param>
        /// <returns>The patient data</returns>
        public Patient GetById(Guid id)
        {
            var entityData = _dbContext.Patient.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            return entityData;
        }

        /// <summary>Retrieves a list of patients based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of patients</returns>/// <exception cref="Exception"></exception>
        public List<Patient> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            var result = GetPatient(filters, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return result;
        }

        /// <summary>Adds a new patient</summary>
        /// <param name="model">The patient data to be added</param>
        /// <returns>The result of the operation</returns>
        public Guid Create(Patient model)
        {
            model.Id = CreatePatient(model);
            return model.Id;
        }

        /// <summary>Updates a specific patient by its primary key</summary>
        /// <param name="id">The primary key of the patient</param>
        /// <param name="updatedEntity">The patient data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Update(Guid id, Patient updatedEntity)
        {
            UpdatePatient(id, updatedEntity);
            return true;
        }

        /// <summary>Updates a specific patient by its primary key</summary>
        /// <param name="id">The primary key of the patient</param>
        /// <param name="updatedEntity">The patient data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Patch(Guid id, JsonPatchDocument<Patient> updatedEntity)
        {
            PatchPatient(id, updatedEntity);
            return true;
        }

        /// <summary>Deletes a specific patient by its primary key</summary>
        /// <param name="id">The primary key of the patient</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Delete(Guid id)
        {
            DeletePatient(id);
            return true;
        }
        #region
        private List<Patient> GetPatient(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            if (pageSize < 1)
            {
                throw new ApplicationException("Page size invalid!");
            }

            if (pageNumber < 1)
            {
                throw new ApplicationException("Page mumber invalid!");
            }

            var query = _dbContext.Patient.IncludeRelated().AsQueryable();
            int skip = (pageNumber - 1) * pageSize;
            var result = FilterService<Patient>.ApplyFilter(query, filters, searchTerm);
            if (!string.IsNullOrEmpty(sortField))
            {
                var parameter = Expression.Parameter(typeof(Patient), "b");
                var property = Expression.Property(parameter, sortField);
                var lambda = Expression.Lambda<Func<Patient, object>>(Expression.Convert(property, typeof(object)), parameter);
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

        private Guid CreatePatient(Patient model)
        {
            _dbContext.Patient.Add(model);
            _dbContext.SaveChanges();
            return model.Id;
        }

        private void UpdatePatient(Guid id, Patient updatedEntity)
        {
            _dbContext.Patient.Update(updatedEntity);
            _dbContext.SaveChanges();
        }

        private bool DeletePatient(Guid id)
        {
            var entityData = _dbContext.Patient.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            if (entityData == null)
            {
                throw new ApplicationException("No data found!");
            }

            _dbContext.Patient.Remove(entityData);
            _dbContext.SaveChanges();
            return true;
        }

        private void PatchPatient(Guid id, JsonPatchDocument<Patient> updatedEntity)
        {
            if (updatedEntity == null)
            {
                throw new ApplicationException("Patch document is missing!");
            }

            var existingEntity = _dbContext.Patient.FirstOrDefault(t => t.Id == id);
            if (existingEntity == null)
            {
                throw new ApplicationException("No data found!");
            }

            updatedEntity.ApplyTo(existingEntity);
            _dbContext.Patient.Update(existingEntity);
            _dbContext.SaveChanges();
        }
        #endregion
    }
}