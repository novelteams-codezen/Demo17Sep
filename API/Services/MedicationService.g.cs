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
    /// The medicationService responsible for managing medication related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting medication information.
    /// </remarks>
    public interface IMedicationService
    {
        /// <summary>Retrieves a specific medication by its primary key</summary>
        /// <param name="id">The primary key of the medication</param>
        /// <returns>The medication data</returns>
        Medication GetById(Guid id);

        /// <summary>Retrieves a list of medications based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of medications</returns>
        List<Medication> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc");

        /// <summary>Adds a new medication</summary>
        /// <param name="model">The medication data to be added</param>
        /// <returns>The result of the operation</returns>
        Guid Create(Medication model);

        /// <summary>Updates a specific medication by its primary key</summary>
        /// <param name="id">The primary key of the medication</param>
        /// <param name="updatedEntity">The medication data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Update(Guid id, Medication updatedEntity);

        /// <summary>Updates a specific medication by its primary key</summary>
        /// <param name="id">The primary key of the medication</param>
        /// <param name="updatedEntity">The medication data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Patch(Guid id, JsonPatchDocument<Medication> updatedEntity);

        /// <summary>Deletes a specific medication by its primary key</summary>
        /// <param name="id">The primary key of the medication</param>
        /// <returns>The result of the operation</returns>
        bool Delete(Guid id);
    }

    /// <summary>
    /// The medicationService responsible for managing medication related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting medication information.
    /// </remarks>
    public class MedicationService : IMedicationService
    {
        private Demo17SepContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the Medication class.
        /// </summary>
        /// <param name="dbContext">dbContext value to set.</param>
        public MedicationService(Demo17SepContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>Retrieves a specific medication by its primary key</summary>
        /// <param name="id">The primary key of the medication</param>
        /// <returns>The medication data</returns>
        public Medication GetById(Guid id)
        {
            var entityData = _dbContext.Medication.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            return entityData;
        }

        /// <summary>Retrieves a list of medications based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of medications</returns>/// <exception cref="Exception"></exception>
        public List<Medication> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            var result = GetMedication(filters, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return result;
        }

        /// <summary>Adds a new medication</summary>
        /// <param name="model">The medication data to be added</param>
        /// <returns>The result of the operation</returns>
        public Guid Create(Medication model)
        {
            model.Id = CreateMedication(model);
            return model.Id;
        }

        /// <summary>Updates a specific medication by its primary key</summary>
        /// <param name="id">The primary key of the medication</param>
        /// <param name="updatedEntity">The medication data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Update(Guid id, Medication updatedEntity)
        {
            UpdateMedication(id, updatedEntity);
            return true;
        }

        /// <summary>Updates a specific medication by its primary key</summary>
        /// <param name="id">The primary key of the medication</param>
        /// <param name="updatedEntity">The medication data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Patch(Guid id, JsonPatchDocument<Medication> updatedEntity)
        {
            PatchMedication(id, updatedEntity);
            return true;
        }

        /// <summary>Deletes a specific medication by its primary key</summary>
        /// <param name="id">The primary key of the medication</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Delete(Guid id)
        {
            DeleteMedication(id);
            return true;
        }
        #region
        private List<Medication> GetMedication(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            if (pageSize < 1)
            {
                throw new ApplicationException("Page size invalid!");
            }

            if (pageNumber < 1)
            {
                throw new ApplicationException("Page mumber invalid!");
            }

            var query = _dbContext.Medication.IncludeRelated().AsQueryable();
            int skip = (pageNumber - 1) * pageSize;
            var result = FilterService<Medication>.ApplyFilter(query, filters, searchTerm);
            if (!string.IsNullOrEmpty(sortField))
            {
                var parameter = Expression.Parameter(typeof(Medication), "b");
                var property = Expression.Property(parameter, sortField);
                var lambda = Expression.Lambda<Func<Medication, object>>(Expression.Convert(property, typeof(object)), parameter);
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

        private Guid CreateMedication(Medication model)
        {
            _dbContext.Medication.Add(model);
            _dbContext.SaveChanges();
            return model.Id;
        }

        private void UpdateMedication(Guid id, Medication updatedEntity)
        {
            _dbContext.Medication.Update(updatedEntity);
            _dbContext.SaveChanges();
        }

        private bool DeleteMedication(Guid id)
        {
            var entityData = _dbContext.Medication.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            if (entityData == null)
            {
                throw new ApplicationException("No data found!");
            }

            _dbContext.Medication.Remove(entityData);
            _dbContext.SaveChanges();
            return true;
        }

        private void PatchMedication(Guid id, JsonPatchDocument<Medication> updatedEntity)
        {
            if (updatedEntity == null)
            {
                throw new ApplicationException("Patch document is missing!");
            }

            var existingEntity = _dbContext.Medication.FirstOrDefault(t => t.Id == id);
            if (existingEntity == null)
            {
                throw new ApplicationException("No data found!");
            }

            updatedEntity.ApplyTo(existingEntity);
            _dbContext.Medication.Update(existingEntity);
            _dbContext.SaveChanges();
        }
        #endregion
    }
}