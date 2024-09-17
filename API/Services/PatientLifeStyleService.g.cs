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
    /// The patientlifestyleService responsible for managing patientlifestyle related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting patientlifestyle information.
    /// </remarks>
    public interface IPatientLifeStyleService
    {
        /// <summary>Retrieves a specific patientlifestyle by its primary key</summary>
        /// <param name="id">The primary key of the patientlifestyle</param>
        /// <returns>The patientlifestyle data</returns>
        PatientLifeStyle GetById(Guid id);

        /// <summary>Retrieves a list of patientlifestyles based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of patientlifestyles</returns>
        List<PatientLifeStyle> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc");

        /// <summary>Adds a new patientlifestyle</summary>
        /// <param name="model">The patientlifestyle data to be added</param>
        /// <returns>The result of the operation</returns>
        Guid Create(PatientLifeStyle model);

        /// <summary>Updates a specific patientlifestyle by its primary key</summary>
        /// <param name="id">The primary key of the patientlifestyle</param>
        /// <param name="updatedEntity">The patientlifestyle data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Update(Guid id, PatientLifeStyle updatedEntity);

        /// <summary>Updates a specific patientlifestyle by its primary key</summary>
        /// <param name="id">The primary key of the patientlifestyle</param>
        /// <param name="updatedEntity">The patientlifestyle data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Patch(Guid id, JsonPatchDocument<PatientLifeStyle> updatedEntity);

        /// <summary>Deletes a specific patientlifestyle by its primary key</summary>
        /// <param name="id">The primary key of the patientlifestyle</param>
        /// <returns>The result of the operation</returns>
        bool Delete(Guid id);
    }

    /// <summary>
    /// The patientlifestyleService responsible for managing patientlifestyle related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting patientlifestyle information.
    /// </remarks>
    public class PatientLifeStyleService : IPatientLifeStyleService
    {
        private Demo17SepContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the PatientLifeStyle class.
        /// </summary>
        /// <param name="dbContext">dbContext value to set.</param>
        public PatientLifeStyleService(Demo17SepContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>Retrieves a specific patientlifestyle by its primary key</summary>
        /// <param name="id">The primary key of the patientlifestyle</param>
        /// <returns>The patientlifestyle data</returns>
        public PatientLifeStyle GetById(Guid id)
        {
            var entityData = _dbContext.PatientLifeStyle.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            return entityData;
        }

        /// <summary>Retrieves a list of patientlifestyles based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of patientlifestyles</returns>/// <exception cref="Exception"></exception>
        public List<PatientLifeStyle> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            var result = GetPatientLifeStyle(filters, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return result;
        }

        /// <summary>Adds a new patientlifestyle</summary>
        /// <param name="model">The patientlifestyle data to be added</param>
        /// <returns>The result of the operation</returns>
        public Guid Create(PatientLifeStyle model)
        {
            model.Id = CreatePatientLifeStyle(model);
            return model.Id;
        }

        /// <summary>Updates a specific patientlifestyle by its primary key</summary>
        /// <param name="id">The primary key of the patientlifestyle</param>
        /// <param name="updatedEntity">The patientlifestyle data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Update(Guid id, PatientLifeStyle updatedEntity)
        {
            UpdatePatientLifeStyle(id, updatedEntity);
            return true;
        }

        /// <summary>Updates a specific patientlifestyle by its primary key</summary>
        /// <param name="id">The primary key of the patientlifestyle</param>
        /// <param name="updatedEntity">The patientlifestyle data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Patch(Guid id, JsonPatchDocument<PatientLifeStyle> updatedEntity)
        {
            PatchPatientLifeStyle(id, updatedEntity);
            return true;
        }

        /// <summary>Deletes a specific patientlifestyle by its primary key</summary>
        /// <param name="id">The primary key of the patientlifestyle</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Delete(Guid id)
        {
            DeletePatientLifeStyle(id);
            return true;
        }
        #region
        private List<PatientLifeStyle> GetPatientLifeStyle(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            if (pageSize < 1)
            {
                throw new ApplicationException("Page size invalid!");
            }

            if (pageNumber < 1)
            {
                throw new ApplicationException("Page mumber invalid!");
            }

            var query = _dbContext.PatientLifeStyle.IncludeRelated().AsQueryable();
            int skip = (pageNumber - 1) * pageSize;
            var result = FilterService<PatientLifeStyle>.ApplyFilter(query, filters, searchTerm);
            if (!string.IsNullOrEmpty(sortField))
            {
                var parameter = Expression.Parameter(typeof(PatientLifeStyle), "b");
                var property = Expression.Property(parameter, sortField);
                var lambda = Expression.Lambda<Func<PatientLifeStyle, object>>(Expression.Convert(property, typeof(object)), parameter);
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

        private Guid CreatePatientLifeStyle(PatientLifeStyle model)
        {
            _dbContext.PatientLifeStyle.Add(model);
            _dbContext.SaveChanges();
            return model.Id;
        }

        private void UpdatePatientLifeStyle(Guid id, PatientLifeStyle updatedEntity)
        {
            _dbContext.PatientLifeStyle.Update(updatedEntity);
            _dbContext.SaveChanges();
        }

        private bool DeletePatientLifeStyle(Guid id)
        {
            var entityData = _dbContext.PatientLifeStyle.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            if (entityData == null)
            {
                throw new ApplicationException("No data found!");
            }

            _dbContext.PatientLifeStyle.Remove(entityData);
            _dbContext.SaveChanges();
            return true;
        }

        private void PatchPatientLifeStyle(Guid id, JsonPatchDocument<PatientLifeStyle> updatedEntity)
        {
            if (updatedEntity == null)
            {
                throw new ApplicationException("Patch document is missing!");
            }

            var existingEntity = _dbContext.PatientLifeStyle.FirstOrDefault(t => t.Id == id);
            if (existingEntity == null)
            {
                throw new ApplicationException("No data found!");
            }

            updatedEntity.ApplyTo(existingEntity);
            _dbContext.PatientLifeStyle.Update(existingEntity);
            _dbContext.SaveChanges();
        }
        #endregion
    }
}