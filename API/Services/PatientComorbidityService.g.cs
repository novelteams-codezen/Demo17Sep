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
    /// The patientcomorbidityService responsible for managing patientcomorbidity related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting patientcomorbidity information.
    /// </remarks>
    public interface IPatientComorbidityService
    {
        /// <summary>Retrieves a specific patientcomorbidity by its primary key</summary>
        /// <param name="id">The primary key of the patientcomorbidity</param>
        /// <returns>The patientcomorbidity data</returns>
        PatientComorbidity GetById(Guid id);

        /// <summary>Retrieves a list of patientcomorbiditys based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of patientcomorbiditys</returns>
        List<PatientComorbidity> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc");

        /// <summary>Adds a new patientcomorbidity</summary>
        /// <param name="model">The patientcomorbidity data to be added</param>
        /// <returns>The result of the operation</returns>
        Guid Create(PatientComorbidity model);

        /// <summary>Updates a specific patientcomorbidity by its primary key</summary>
        /// <param name="id">The primary key of the patientcomorbidity</param>
        /// <param name="updatedEntity">The patientcomorbidity data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Update(Guid id, PatientComorbidity updatedEntity);

        /// <summary>Updates a specific patientcomorbidity by its primary key</summary>
        /// <param name="id">The primary key of the patientcomorbidity</param>
        /// <param name="updatedEntity">The patientcomorbidity data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Patch(Guid id, JsonPatchDocument<PatientComorbidity> updatedEntity);

        /// <summary>Deletes a specific patientcomorbidity by its primary key</summary>
        /// <param name="id">The primary key of the patientcomorbidity</param>
        /// <returns>The result of the operation</returns>
        bool Delete(Guid id);
    }

    /// <summary>
    /// The patientcomorbidityService responsible for managing patientcomorbidity related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting patientcomorbidity information.
    /// </remarks>
    public class PatientComorbidityService : IPatientComorbidityService
    {
        private Demo17SepContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the PatientComorbidity class.
        /// </summary>
        /// <param name="dbContext">dbContext value to set.</param>
        public PatientComorbidityService(Demo17SepContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>Retrieves a specific patientcomorbidity by its primary key</summary>
        /// <param name="id">The primary key of the patientcomorbidity</param>
        /// <returns>The patientcomorbidity data</returns>
        public PatientComorbidity GetById(Guid id)
        {
            var entityData = _dbContext.PatientComorbidity.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            return entityData;
        }

        /// <summary>Retrieves a list of patientcomorbiditys based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of patientcomorbiditys</returns>/// <exception cref="Exception"></exception>
        public List<PatientComorbidity> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            var result = GetPatientComorbidity(filters, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return result;
        }

        /// <summary>Adds a new patientcomorbidity</summary>
        /// <param name="model">The patientcomorbidity data to be added</param>
        /// <returns>The result of the operation</returns>
        public Guid Create(PatientComorbidity model)
        {
            model.Id = CreatePatientComorbidity(model);
            return model.Id;
        }

        /// <summary>Updates a specific patientcomorbidity by its primary key</summary>
        /// <param name="id">The primary key of the patientcomorbidity</param>
        /// <param name="updatedEntity">The patientcomorbidity data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Update(Guid id, PatientComorbidity updatedEntity)
        {
            UpdatePatientComorbidity(id, updatedEntity);
            return true;
        }

        /// <summary>Updates a specific patientcomorbidity by its primary key</summary>
        /// <param name="id">The primary key of the patientcomorbidity</param>
        /// <param name="updatedEntity">The patientcomorbidity data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Patch(Guid id, JsonPatchDocument<PatientComorbidity> updatedEntity)
        {
            PatchPatientComorbidity(id, updatedEntity);
            return true;
        }

        /// <summary>Deletes a specific patientcomorbidity by its primary key</summary>
        /// <param name="id">The primary key of the patientcomorbidity</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Delete(Guid id)
        {
            DeletePatientComorbidity(id);
            return true;
        }
        #region
        private List<PatientComorbidity> GetPatientComorbidity(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            if (pageSize < 1)
            {
                throw new ApplicationException("Page size invalid!");
            }

            if (pageNumber < 1)
            {
                throw new ApplicationException("Page mumber invalid!");
            }

            var query = _dbContext.PatientComorbidity.IncludeRelated().AsQueryable();
            int skip = (pageNumber - 1) * pageSize;
            var result = FilterService<PatientComorbidity>.ApplyFilter(query, filters, searchTerm);
            if (!string.IsNullOrEmpty(sortField))
            {
                var parameter = Expression.Parameter(typeof(PatientComorbidity), "b");
                var property = Expression.Property(parameter, sortField);
                var lambda = Expression.Lambda<Func<PatientComorbidity, object>>(Expression.Convert(property, typeof(object)), parameter);
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

        private Guid CreatePatientComorbidity(PatientComorbidity model)
        {
            _dbContext.PatientComorbidity.Add(model);
            _dbContext.SaveChanges();
            return model.Id;
        }

        private void UpdatePatientComorbidity(Guid id, PatientComorbidity updatedEntity)
        {
            _dbContext.PatientComorbidity.Update(updatedEntity);
            _dbContext.SaveChanges();
        }

        private bool DeletePatientComorbidity(Guid id)
        {
            var entityData = _dbContext.PatientComorbidity.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            if (entityData == null)
            {
                throw new ApplicationException("No data found!");
            }

            _dbContext.PatientComorbidity.Remove(entityData);
            _dbContext.SaveChanges();
            return true;
        }

        private void PatchPatientComorbidity(Guid id, JsonPatchDocument<PatientComorbidity> updatedEntity)
        {
            if (updatedEntity == null)
            {
                throw new ApplicationException("Patch document is missing!");
            }

            var existingEntity = _dbContext.PatientComorbidity.FirstOrDefault(t => t.Id == id);
            if (existingEntity == null)
            {
                throw new ApplicationException("No data found!");
            }

            updatedEntity.ApplyTo(existingEntity);
            _dbContext.PatientComorbidity.Update(existingEntity);
            _dbContext.SaveChanges();
        }
        #endregion
    }
}