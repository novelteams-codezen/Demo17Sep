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
    /// The patientpharmacyqueueService responsible for managing patientpharmacyqueue related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting patientpharmacyqueue information.
    /// </remarks>
    public interface IPatientPharmacyQueueService
    {
        /// <summary>Retrieves a specific patientpharmacyqueue by its primary key</summary>
        /// <param name="id">The primary key of the patientpharmacyqueue</param>
        /// <returns>The patientpharmacyqueue data</returns>
        PatientPharmacyQueue GetById(Guid id);

        /// <summary>Retrieves a list of patientpharmacyqueues based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of patientpharmacyqueues</returns>
        List<PatientPharmacyQueue> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc");

        /// <summary>Adds a new patientpharmacyqueue</summary>
        /// <param name="model">The patientpharmacyqueue data to be added</param>
        /// <returns>The result of the operation</returns>
        Guid Create(PatientPharmacyQueue model);

        /// <summary>Updates a specific patientpharmacyqueue by its primary key</summary>
        /// <param name="id">The primary key of the patientpharmacyqueue</param>
        /// <param name="updatedEntity">The patientpharmacyqueue data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Update(Guid id, PatientPharmacyQueue updatedEntity);

        /// <summary>Updates a specific patientpharmacyqueue by its primary key</summary>
        /// <param name="id">The primary key of the patientpharmacyqueue</param>
        /// <param name="updatedEntity">The patientpharmacyqueue data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Patch(Guid id, JsonPatchDocument<PatientPharmacyQueue> updatedEntity);

        /// <summary>Deletes a specific patientpharmacyqueue by its primary key</summary>
        /// <param name="id">The primary key of the patientpharmacyqueue</param>
        /// <returns>The result of the operation</returns>
        bool Delete(Guid id);
    }

    /// <summary>
    /// The patientpharmacyqueueService responsible for managing patientpharmacyqueue related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting patientpharmacyqueue information.
    /// </remarks>
    public class PatientPharmacyQueueService : IPatientPharmacyQueueService
    {
        private Demo17SepContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the PatientPharmacyQueue class.
        /// </summary>
        /// <param name="dbContext">dbContext value to set.</param>
        public PatientPharmacyQueueService(Demo17SepContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>Retrieves a specific patientpharmacyqueue by its primary key</summary>
        /// <param name="id">The primary key of the patientpharmacyqueue</param>
        /// <returns>The patientpharmacyqueue data</returns>
        public PatientPharmacyQueue GetById(Guid id)
        {
            var entityData = _dbContext.PatientPharmacyQueue.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            return entityData;
        }

        /// <summary>Retrieves a list of patientpharmacyqueues based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of patientpharmacyqueues</returns>/// <exception cref="Exception"></exception>
        public List<PatientPharmacyQueue> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            var result = GetPatientPharmacyQueue(filters, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return result;
        }

        /// <summary>Adds a new patientpharmacyqueue</summary>
        /// <param name="model">The patientpharmacyqueue data to be added</param>
        /// <returns>The result of the operation</returns>
        public Guid Create(PatientPharmacyQueue model)
        {
            model.Id = CreatePatientPharmacyQueue(model);
            return model.Id;
        }

        /// <summary>Updates a specific patientpharmacyqueue by its primary key</summary>
        /// <param name="id">The primary key of the patientpharmacyqueue</param>
        /// <param name="updatedEntity">The patientpharmacyqueue data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Update(Guid id, PatientPharmacyQueue updatedEntity)
        {
            UpdatePatientPharmacyQueue(id, updatedEntity);
            return true;
        }

        /// <summary>Updates a specific patientpharmacyqueue by its primary key</summary>
        /// <param name="id">The primary key of the patientpharmacyqueue</param>
        /// <param name="updatedEntity">The patientpharmacyqueue data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Patch(Guid id, JsonPatchDocument<PatientPharmacyQueue> updatedEntity)
        {
            PatchPatientPharmacyQueue(id, updatedEntity);
            return true;
        }

        /// <summary>Deletes a specific patientpharmacyqueue by its primary key</summary>
        /// <param name="id">The primary key of the patientpharmacyqueue</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Delete(Guid id)
        {
            DeletePatientPharmacyQueue(id);
            return true;
        }
        #region
        private List<PatientPharmacyQueue> GetPatientPharmacyQueue(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            if (pageSize < 1)
            {
                throw new ApplicationException("Page size invalid!");
            }

            if (pageNumber < 1)
            {
                throw new ApplicationException("Page mumber invalid!");
            }

            var query = _dbContext.PatientPharmacyQueue.IncludeRelated().AsQueryable();
            int skip = (pageNumber - 1) * pageSize;
            var result = FilterService<PatientPharmacyQueue>.ApplyFilter(query, filters, searchTerm);
            if (!string.IsNullOrEmpty(sortField))
            {
                var parameter = Expression.Parameter(typeof(PatientPharmacyQueue), "b");
                var property = Expression.Property(parameter, sortField);
                var lambda = Expression.Lambda<Func<PatientPharmacyQueue, object>>(Expression.Convert(property, typeof(object)), parameter);
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

        private Guid CreatePatientPharmacyQueue(PatientPharmacyQueue model)
        {
            _dbContext.PatientPharmacyQueue.Add(model);
            _dbContext.SaveChanges();
            return model.Id;
        }

        private void UpdatePatientPharmacyQueue(Guid id, PatientPharmacyQueue updatedEntity)
        {
            _dbContext.PatientPharmacyQueue.Update(updatedEntity);
            _dbContext.SaveChanges();
        }

        private bool DeletePatientPharmacyQueue(Guid id)
        {
            var entityData = _dbContext.PatientPharmacyQueue.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            if (entityData == null)
            {
                throw new ApplicationException("No data found!");
            }

            _dbContext.PatientPharmacyQueue.Remove(entityData);
            _dbContext.SaveChanges();
            return true;
        }

        private void PatchPatientPharmacyQueue(Guid id, JsonPatchDocument<PatientPharmacyQueue> updatedEntity)
        {
            if (updatedEntity == null)
            {
                throw new ApplicationException("Patch document is missing!");
            }

            var existingEntity = _dbContext.PatientPharmacyQueue.FirstOrDefault(t => t.Id == id);
            if (existingEntity == null)
            {
                throw new ApplicationException("No data found!");
            }

            updatedEntity.ApplyTo(existingEntity);
            _dbContext.PatientPharmacyQueue.Update(existingEntity);
            _dbContext.SaveChanges();
        }
        #endregion
    }
}