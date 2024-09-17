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
    /// The diagnosisService responsible for managing diagnosis related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting diagnosis information.
    /// </remarks>
    public interface IDiagnosisService
    {
        /// <summary>Retrieves a specific diagnosis by its primary key</summary>
        /// <param name="id">The primary key of the diagnosis</param>
        /// <returns>The diagnosis data</returns>
        Diagnosis GetById(Guid id);

        /// <summary>Retrieves a list of diagnosiss based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of diagnosiss</returns>
        List<Diagnosis> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc");

        /// <summary>Adds a new diagnosis</summary>
        /// <param name="model">The diagnosis data to be added</param>
        /// <returns>The result of the operation</returns>
        Guid Create(Diagnosis model);

        /// <summary>Updates a specific diagnosis by its primary key</summary>
        /// <param name="id">The primary key of the diagnosis</param>
        /// <param name="updatedEntity">The diagnosis data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Update(Guid id, Diagnosis updatedEntity);

        /// <summary>Updates a specific diagnosis by its primary key</summary>
        /// <param name="id">The primary key of the diagnosis</param>
        /// <param name="updatedEntity">The diagnosis data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Patch(Guid id, JsonPatchDocument<Diagnosis> updatedEntity);

        /// <summary>Deletes a specific diagnosis by its primary key</summary>
        /// <param name="id">The primary key of the diagnosis</param>
        /// <returns>The result of the operation</returns>
        bool Delete(Guid id);
    }

    /// <summary>
    /// The diagnosisService responsible for managing diagnosis related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting diagnosis information.
    /// </remarks>
    public class DiagnosisService : IDiagnosisService
    {
        private Demo17SepContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the Diagnosis class.
        /// </summary>
        /// <param name="dbContext">dbContext value to set.</param>
        public DiagnosisService(Demo17SepContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>Retrieves a specific diagnosis by its primary key</summary>
        /// <param name="id">The primary key of the diagnosis</param>
        /// <returns>The diagnosis data</returns>
        public Diagnosis GetById(Guid id)
        {
            var entityData = _dbContext.Diagnosis.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            return entityData;
        }

        /// <summary>Retrieves a list of diagnosiss based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of diagnosiss</returns>/// <exception cref="Exception"></exception>
        public List<Diagnosis> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            var result = GetDiagnosis(filters, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return result;
        }

        /// <summary>Adds a new diagnosis</summary>
        /// <param name="model">The diagnosis data to be added</param>
        /// <returns>The result of the operation</returns>
        public Guid Create(Diagnosis model)
        {
            model.Id = CreateDiagnosis(model);
            return model.Id;
        }

        /// <summary>Updates a specific diagnosis by its primary key</summary>
        /// <param name="id">The primary key of the diagnosis</param>
        /// <param name="updatedEntity">The diagnosis data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Update(Guid id, Diagnosis updatedEntity)
        {
            UpdateDiagnosis(id, updatedEntity);
            return true;
        }

        /// <summary>Updates a specific diagnosis by its primary key</summary>
        /// <param name="id">The primary key of the diagnosis</param>
        /// <param name="updatedEntity">The diagnosis data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Patch(Guid id, JsonPatchDocument<Diagnosis> updatedEntity)
        {
            PatchDiagnosis(id, updatedEntity);
            return true;
        }

        /// <summary>Deletes a specific diagnosis by its primary key</summary>
        /// <param name="id">The primary key of the diagnosis</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Delete(Guid id)
        {
            DeleteDiagnosis(id);
            return true;
        }
        #region
        private List<Diagnosis> GetDiagnosis(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            if (pageSize < 1)
            {
                throw new ApplicationException("Page size invalid!");
            }

            if (pageNumber < 1)
            {
                throw new ApplicationException("Page mumber invalid!");
            }

            var query = _dbContext.Diagnosis.IncludeRelated().AsQueryable();
            int skip = (pageNumber - 1) * pageSize;
            var result = FilterService<Diagnosis>.ApplyFilter(query, filters, searchTerm);
            if (!string.IsNullOrEmpty(sortField))
            {
                var parameter = Expression.Parameter(typeof(Diagnosis), "b");
                var property = Expression.Property(parameter, sortField);
                var lambda = Expression.Lambda<Func<Diagnosis, object>>(Expression.Convert(property, typeof(object)), parameter);
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

        private Guid CreateDiagnosis(Diagnosis model)
        {
            _dbContext.Diagnosis.Add(model);
            _dbContext.SaveChanges();
            return model.Id;
        }

        private void UpdateDiagnosis(Guid id, Diagnosis updatedEntity)
        {
            _dbContext.Diagnosis.Update(updatedEntity);
            _dbContext.SaveChanges();
        }

        private bool DeleteDiagnosis(Guid id)
        {
            var entityData = _dbContext.Diagnosis.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            if (entityData == null)
            {
                throw new ApplicationException("No data found!");
            }

            _dbContext.Diagnosis.Remove(entityData);
            _dbContext.SaveChanges();
            return true;
        }

        private void PatchDiagnosis(Guid id, JsonPatchDocument<Diagnosis> updatedEntity)
        {
            if (updatedEntity == null)
            {
                throw new ApplicationException("Patch document is missing!");
            }

            var existingEntity = _dbContext.Diagnosis.FirstOrDefault(t => t.Id == id);
            if (existingEntity == null)
            {
                throw new ApplicationException("No data found!");
            }

            updatedEntity.ApplyTo(existingEntity);
            _dbContext.Diagnosis.Update(existingEntity);
            _dbContext.SaveChanges();
        }
        #endregion
    }
}