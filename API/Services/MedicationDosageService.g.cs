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
    /// The medicationdosageService responsible for managing medicationdosage related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting medicationdosage information.
    /// </remarks>
    public interface IMedicationDosageService
    {
        /// <summary>Retrieves a specific medicationdosage by its primary key</summary>
        /// <param name="id">The primary key of the medicationdosage</param>
        /// <returns>The medicationdosage data</returns>
        MedicationDosage GetById(Guid id);

        /// <summary>Retrieves a list of medicationdosages based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of medicationdosages</returns>
        List<MedicationDosage> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc");

        /// <summary>Adds a new medicationdosage</summary>
        /// <param name="model">The medicationdosage data to be added</param>
        /// <returns>The result of the operation</returns>
        Guid Create(MedicationDosage model);

        /// <summary>Updates a specific medicationdosage by its primary key</summary>
        /// <param name="id">The primary key of the medicationdosage</param>
        /// <param name="updatedEntity">The medicationdosage data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Update(Guid id, MedicationDosage updatedEntity);

        /// <summary>Updates a specific medicationdosage by its primary key</summary>
        /// <param name="id">The primary key of the medicationdosage</param>
        /// <param name="updatedEntity">The medicationdosage data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Patch(Guid id, JsonPatchDocument<MedicationDosage> updatedEntity);

        /// <summary>Deletes a specific medicationdosage by its primary key</summary>
        /// <param name="id">The primary key of the medicationdosage</param>
        /// <returns>The result of the operation</returns>
        bool Delete(Guid id);
    }

    /// <summary>
    /// The medicationdosageService responsible for managing medicationdosage related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting medicationdosage information.
    /// </remarks>
    public class MedicationDosageService : IMedicationDosageService
    {
        private Demo17SepContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the MedicationDosage class.
        /// </summary>
        /// <param name="dbContext">dbContext value to set.</param>
        public MedicationDosageService(Demo17SepContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>Retrieves a specific medicationdosage by its primary key</summary>
        /// <param name="id">The primary key of the medicationdosage</param>
        /// <returns>The medicationdosage data</returns>
        public MedicationDosage GetById(Guid id)
        {
            var entityData = _dbContext.MedicationDosage.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            return entityData;
        }

        /// <summary>Retrieves a list of medicationdosages based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of medicationdosages</returns>/// <exception cref="Exception"></exception>
        public List<MedicationDosage> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            var result = GetMedicationDosage(filters, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return result;
        }

        /// <summary>Adds a new medicationdosage</summary>
        /// <param name="model">The medicationdosage data to be added</param>
        /// <returns>The result of the operation</returns>
        public Guid Create(MedicationDosage model)
        {
            model.Id = CreateMedicationDosage(model);
            return model.Id;
        }

        /// <summary>Updates a specific medicationdosage by its primary key</summary>
        /// <param name="id">The primary key of the medicationdosage</param>
        /// <param name="updatedEntity">The medicationdosage data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Update(Guid id, MedicationDosage updatedEntity)
        {
            UpdateMedicationDosage(id, updatedEntity);
            return true;
        }

        /// <summary>Updates a specific medicationdosage by its primary key</summary>
        /// <param name="id">The primary key of the medicationdosage</param>
        /// <param name="updatedEntity">The medicationdosage data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Patch(Guid id, JsonPatchDocument<MedicationDosage> updatedEntity)
        {
            PatchMedicationDosage(id, updatedEntity);
            return true;
        }

        /// <summary>Deletes a specific medicationdosage by its primary key</summary>
        /// <param name="id">The primary key of the medicationdosage</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Delete(Guid id)
        {
            DeleteMedicationDosage(id);
            return true;
        }
        #region
        private List<MedicationDosage> GetMedicationDosage(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            if (pageSize < 1)
            {
                throw new ApplicationException("Page size invalid!");
            }

            if (pageNumber < 1)
            {
                throw new ApplicationException("Page mumber invalid!");
            }

            var query = _dbContext.MedicationDosage.IncludeRelated().AsQueryable();
            int skip = (pageNumber - 1) * pageSize;
            var result = FilterService<MedicationDosage>.ApplyFilter(query, filters, searchTerm);
            if (!string.IsNullOrEmpty(sortField))
            {
                var parameter = Expression.Parameter(typeof(MedicationDosage), "b");
                var property = Expression.Property(parameter, sortField);
                var lambda = Expression.Lambda<Func<MedicationDosage, object>>(Expression.Convert(property, typeof(object)), parameter);
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

        private Guid CreateMedicationDosage(MedicationDosage model)
        {
            _dbContext.MedicationDosage.Add(model);
            _dbContext.SaveChanges();
            return model.Id;
        }

        private void UpdateMedicationDosage(Guid id, MedicationDosage updatedEntity)
        {
            _dbContext.MedicationDosage.Update(updatedEntity);
            _dbContext.SaveChanges();
        }

        private bool DeleteMedicationDosage(Guid id)
        {
            var entityData = _dbContext.MedicationDosage.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            if (entityData == null)
            {
                throw new ApplicationException("No data found!");
            }

            _dbContext.MedicationDosage.Remove(entityData);
            _dbContext.SaveChanges();
            return true;
        }

        private void PatchMedicationDosage(Guid id, JsonPatchDocument<MedicationDosage> updatedEntity)
        {
            if (updatedEntity == null)
            {
                throw new ApplicationException("Patch document is missing!");
            }

            var existingEntity = _dbContext.MedicationDosage.FirstOrDefault(t => t.Id == id);
            if (existingEntity == null)
            {
                throw new ApplicationException("No data found!");
            }

            updatedEntity.ApplyTo(existingEntity);
            _dbContext.MedicationDosage.Update(existingEntity);
            _dbContext.SaveChanges();
        }
        #endregion
    }
}