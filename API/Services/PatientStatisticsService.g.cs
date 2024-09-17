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
    /// The patientstatisticsService responsible for managing patientstatistics related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting patientstatistics information.
    /// </remarks>
    public interface IPatientStatisticsService
    {
        /// <summary>Retrieves a specific patientstatistics by its primary key</summary>
        /// <param name="id">The primary key of the patientstatistics</param>
        /// <returns>The patientstatistics data</returns>
        PatientStatistics GetById(Guid id);

        /// <summary>Retrieves a list of patientstatisticss based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of patientstatisticss</returns>
        List<PatientStatistics> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc");

        /// <summary>Adds a new patientstatistics</summary>
        /// <param name="model">The patientstatistics data to be added</param>
        /// <returns>The result of the operation</returns>
        Guid Create(PatientStatistics model);

        /// <summary>Updates a specific patientstatistics by its primary key</summary>
        /// <param name="id">The primary key of the patientstatistics</param>
        /// <param name="updatedEntity">The patientstatistics data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Update(Guid id, PatientStatistics updatedEntity);

        /// <summary>Updates a specific patientstatistics by its primary key</summary>
        /// <param name="id">The primary key of the patientstatistics</param>
        /// <param name="updatedEntity">The patientstatistics data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Patch(Guid id, JsonPatchDocument<PatientStatistics> updatedEntity);

        /// <summary>Deletes a specific patientstatistics by its primary key</summary>
        /// <param name="id">The primary key of the patientstatistics</param>
        /// <returns>The result of the operation</returns>
        bool Delete(Guid id);
    }

    /// <summary>
    /// The patientstatisticsService responsible for managing patientstatistics related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting patientstatistics information.
    /// </remarks>
    public class PatientStatisticsService : IPatientStatisticsService
    {
        private Demo17SepContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the PatientStatistics class.
        /// </summary>
        /// <param name="dbContext">dbContext value to set.</param>
        public PatientStatisticsService(Demo17SepContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>Retrieves a specific patientstatistics by its primary key</summary>
        /// <param name="id">The primary key of the patientstatistics</param>
        /// <returns>The patientstatistics data</returns>
        public PatientStatistics GetById(Guid id)
        {
            var entityData = _dbContext.PatientStatistics.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            return entityData;
        }

        /// <summary>Retrieves a list of patientstatisticss based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of patientstatisticss</returns>/// <exception cref="Exception"></exception>
        public List<PatientStatistics> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            var result = GetPatientStatistics(filters, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return result;
        }

        /// <summary>Adds a new patientstatistics</summary>
        /// <param name="model">The patientstatistics data to be added</param>
        /// <returns>The result of the operation</returns>
        public Guid Create(PatientStatistics model)
        {
            model.Id = CreatePatientStatistics(model);
            return model.Id;
        }

        /// <summary>Updates a specific patientstatistics by its primary key</summary>
        /// <param name="id">The primary key of the patientstatistics</param>
        /// <param name="updatedEntity">The patientstatistics data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Update(Guid id, PatientStatistics updatedEntity)
        {
            UpdatePatientStatistics(id, updatedEntity);
            return true;
        }

        /// <summary>Updates a specific patientstatistics by its primary key</summary>
        /// <param name="id">The primary key of the patientstatistics</param>
        /// <param name="updatedEntity">The patientstatistics data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Patch(Guid id, JsonPatchDocument<PatientStatistics> updatedEntity)
        {
            PatchPatientStatistics(id, updatedEntity);
            return true;
        }

        /// <summary>Deletes a specific patientstatistics by its primary key</summary>
        /// <param name="id">The primary key of the patientstatistics</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Delete(Guid id)
        {
            DeletePatientStatistics(id);
            return true;
        }
        #region
        private List<PatientStatistics> GetPatientStatistics(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            if (pageSize < 1)
            {
                throw new ApplicationException("Page size invalid!");
            }

            if (pageNumber < 1)
            {
                throw new ApplicationException("Page mumber invalid!");
            }

            var query = _dbContext.PatientStatistics.IncludeRelated().AsQueryable();
            int skip = (pageNumber - 1) * pageSize;
            var result = FilterService<PatientStatistics>.ApplyFilter(query, filters, searchTerm);
            if (!string.IsNullOrEmpty(sortField))
            {
                var parameter = Expression.Parameter(typeof(PatientStatistics), "b");
                var property = Expression.Property(parameter, sortField);
                var lambda = Expression.Lambda<Func<PatientStatistics, object>>(Expression.Convert(property, typeof(object)), parameter);
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

        private Guid CreatePatientStatistics(PatientStatistics model)
        {
            _dbContext.PatientStatistics.Add(model);
            _dbContext.SaveChanges();
            return model.Id;
        }

        private void UpdatePatientStatistics(Guid id, PatientStatistics updatedEntity)
        {
            _dbContext.PatientStatistics.Update(updatedEntity);
            _dbContext.SaveChanges();
        }

        private bool DeletePatientStatistics(Guid id)
        {
            var entityData = _dbContext.PatientStatistics.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            if (entityData == null)
            {
                throw new ApplicationException("No data found!");
            }

            _dbContext.PatientStatistics.Remove(entityData);
            _dbContext.SaveChanges();
            return true;
        }

        private void PatchPatientStatistics(Guid id, JsonPatchDocument<PatientStatistics> updatedEntity)
        {
            if (updatedEntity == null)
            {
                throw new ApplicationException("Patch document is missing!");
            }

            var existingEntity = _dbContext.PatientStatistics.FirstOrDefault(t => t.Id == id);
            if (existingEntity == null)
            {
                throw new ApplicationException("No data found!");
            }

            updatedEntity.ApplyTo(existingEntity);
            _dbContext.PatientStatistics.Update(existingEntity);
            _dbContext.SaveChanges();
        }
        #endregion
    }
}