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
    /// The patienthospitalisationhistoryService responsible for managing patienthospitalisationhistory related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting patienthospitalisationhistory information.
    /// </remarks>
    public interface IPatientHospitalisationHistoryService
    {
        /// <summary>Retrieves a specific patienthospitalisationhistory by its primary key</summary>
        /// <param name="id">The primary key of the patienthospitalisationhistory</param>
        /// <returns>The patienthospitalisationhistory data</returns>
        PatientHospitalisationHistory GetById(Guid id);

        /// <summary>Retrieves a list of patienthospitalisationhistorys based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of patienthospitalisationhistorys</returns>
        List<PatientHospitalisationHistory> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc");

        /// <summary>Adds a new patienthospitalisationhistory</summary>
        /// <param name="model">The patienthospitalisationhistory data to be added</param>
        /// <returns>The result of the operation</returns>
        Guid Create(PatientHospitalisationHistory model);

        /// <summary>Updates a specific patienthospitalisationhistory by its primary key</summary>
        /// <param name="id">The primary key of the patienthospitalisationhistory</param>
        /// <param name="updatedEntity">The patienthospitalisationhistory data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Update(Guid id, PatientHospitalisationHistory updatedEntity);

        /// <summary>Updates a specific patienthospitalisationhistory by its primary key</summary>
        /// <param name="id">The primary key of the patienthospitalisationhistory</param>
        /// <param name="updatedEntity">The patienthospitalisationhistory data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Patch(Guid id, JsonPatchDocument<PatientHospitalisationHistory> updatedEntity);

        /// <summary>Deletes a specific patienthospitalisationhistory by its primary key</summary>
        /// <param name="id">The primary key of the patienthospitalisationhistory</param>
        /// <returns>The result of the operation</returns>
        bool Delete(Guid id);
    }

    /// <summary>
    /// The patienthospitalisationhistoryService responsible for managing patienthospitalisationhistory related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting patienthospitalisationhistory information.
    /// </remarks>
    public class PatientHospitalisationHistoryService : IPatientHospitalisationHistoryService
    {
        private Demo17SepContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the PatientHospitalisationHistory class.
        /// </summary>
        /// <param name="dbContext">dbContext value to set.</param>
        public PatientHospitalisationHistoryService(Demo17SepContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>Retrieves a specific patienthospitalisationhistory by its primary key</summary>
        /// <param name="id">The primary key of the patienthospitalisationhistory</param>
        /// <returns>The patienthospitalisationhistory data</returns>
        public PatientHospitalisationHistory GetById(Guid id)
        {
            var entityData = _dbContext.PatientHospitalisationHistory.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            return entityData;
        }

        /// <summary>Retrieves a list of patienthospitalisationhistorys based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of patienthospitalisationhistorys</returns>/// <exception cref="Exception"></exception>
        public List<PatientHospitalisationHistory> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            var result = GetPatientHospitalisationHistory(filters, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return result;
        }

        /// <summary>Adds a new patienthospitalisationhistory</summary>
        /// <param name="model">The patienthospitalisationhistory data to be added</param>
        /// <returns>The result of the operation</returns>
        public Guid Create(PatientHospitalisationHistory model)
        {
            model.Id = CreatePatientHospitalisationHistory(model);
            return model.Id;
        }

        /// <summary>Updates a specific patienthospitalisationhistory by its primary key</summary>
        /// <param name="id">The primary key of the patienthospitalisationhistory</param>
        /// <param name="updatedEntity">The patienthospitalisationhistory data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Update(Guid id, PatientHospitalisationHistory updatedEntity)
        {
            UpdatePatientHospitalisationHistory(id, updatedEntity);
            return true;
        }

        /// <summary>Updates a specific patienthospitalisationhistory by its primary key</summary>
        /// <param name="id">The primary key of the patienthospitalisationhistory</param>
        /// <param name="updatedEntity">The patienthospitalisationhistory data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Patch(Guid id, JsonPatchDocument<PatientHospitalisationHistory> updatedEntity)
        {
            PatchPatientHospitalisationHistory(id, updatedEntity);
            return true;
        }

        /// <summary>Deletes a specific patienthospitalisationhistory by its primary key</summary>
        /// <param name="id">The primary key of the patienthospitalisationhistory</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Delete(Guid id)
        {
            DeletePatientHospitalisationHistory(id);
            return true;
        }
        #region
        private List<PatientHospitalisationHistory> GetPatientHospitalisationHistory(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            if (pageSize < 1)
            {
                throw new ApplicationException("Page size invalid!");
            }

            if (pageNumber < 1)
            {
                throw new ApplicationException("Page mumber invalid!");
            }

            var query = _dbContext.PatientHospitalisationHistory.IncludeRelated().AsQueryable();
            int skip = (pageNumber - 1) * pageSize;
            var result = FilterService<PatientHospitalisationHistory>.ApplyFilter(query, filters, searchTerm);
            if (!string.IsNullOrEmpty(sortField))
            {
                var parameter = Expression.Parameter(typeof(PatientHospitalisationHistory), "b");
                var property = Expression.Property(parameter, sortField);
                var lambda = Expression.Lambda<Func<PatientHospitalisationHistory, object>>(Expression.Convert(property, typeof(object)), parameter);
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

        private Guid CreatePatientHospitalisationHistory(PatientHospitalisationHistory model)
        {
            _dbContext.PatientHospitalisationHistory.Add(model);
            _dbContext.SaveChanges();
            return model.Id;
        }

        private void UpdatePatientHospitalisationHistory(Guid id, PatientHospitalisationHistory updatedEntity)
        {
            _dbContext.PatientHospitalisationHistory.Update(updatedEntity);
            _dbContext.SaveChanges();
        }

        private bool DeletePatientHospitalisationHistory(Guid id)
        {
            var entityData = _dbContext.PatientHospitalisationHistory.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            if (entityData == null)
            {
                throw new ApplicationException("No data found!");
            }

            _dbContext.PatientHospitalisationHistory.Remove(entityData);
            _dbContext.SaveChanges();
            return true;
        }

        private void PatchPatientHospitalisationHistory(Guid id, JsonPatchDocument<PatientHospitalisationHistory> updatedEntity)
        {
            if (updatedEntity == null)
            {
                throw new ApplicationException("Patch document is missing!");
            }

            var existingEntity = _dbContext.PatientHospitalisationHistory.FirstOrDefault(t => t.Id == id);
            if (existingEntity == null)
            {
                throw new ApplicationException("No data found!");
            }

            updatedEntity.ApplyTo(existingEntity);
            _dbContext.PatientHospitalisationHistory.Update(existingEntity);
            _dbContext.SaveChanges();
        }
        #endregion
    }
}