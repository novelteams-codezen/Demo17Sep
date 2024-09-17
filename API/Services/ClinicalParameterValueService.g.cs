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
    /// The clinicalparametervalueService responsible for managing clinicalparametervalue related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting clinicalparametervalue information.
    /// </remarks>
    public interface IClinicalParameterValueService
    {
        /// <summary>Retrieves a specific clinicalparametervalue by its primary key</summary>
        /// <param name="id">The primary key of the clinicalparametervalue</param>
        /// <returns>The clinicalparametervalue data</returns>
        ClinicalParameterValue GetById(Guid id);

        /// <summary>Retrieves a list of clinicalparametervalues based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of clinicalparametervalues</returns>
        List<ClinicalParameterValue> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc");

        /// <summary>Adds a new clinicalparametervalue</summary>
        /// <param name="model">The clinicalparametervalue data to be added</param>
        /// <returns>The result of the operation</returns>
        Guid Create(ClinicalParameterValue model);

        /// <summary>Updates a specific clinicalparametervalue by its primary key</summary>
        /// <param name="id">The primary key of the clinicalparametervalue</param>
        /// <param name="updatedEntity">The clinicalparametervalue data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Update(Guid id, ClinicalParameterValue updatedEntity);

        /// <summary>Updates a specific clinicalparametervalue by its primary key</summary>
        /// <param name="id">The primary key of the clinicalparametervalue</param>
        /// <param name="updatedEntity">The clinicalparametervalue data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Patch(Guid id, JsonPatchDocument<ClinicalParameterValue> updatedEntity);

        /// <summary>Deletes a specific clinicalparametervalue by its primary key</summary>
        /// <param name="id">The primary key of the clinicalparametervalue</param>
        /// <returns>The result of the operation</returns>
        bool Delete(Guid id);
    }

    /// <summary>
    /// The clinicalparametervalueService responsible for managing clinicalparametervalue related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting clinicalparametervalue information.
    /// </remarks>
    public class ClinicalParameterValueService : IClinicalParameterValueService
    {
        private Demo17SepContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the ClinicalParameterValue class.
        /// </summary>
        /// <param name="dbContext">dbContext value to set.</param>
        public ClinicalParameterValueService(Demo17SepContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>Retrieves a specific clinicalparametervalue by its primary key</summary>
        /// <param name="id">The primary key of the clinicalparametervalue</param>
        /// <returns>The clinicalparametervalue data</returns>
        public ClinicalParameterValue GetById(Guid id)
        {
            var entityData = _dbContext.ClinicalParameterValue.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            return entityData;
        }

        /// <summary>Retrieves a list of clinicalparametervalues based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of clinicalparametervalues</returns>/// <exception cref="Exception"></exception>
        public List<ClinicalParameterValue> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            var result = GetClinicalParameterValue(filters, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return result;
        }

        /// <summary>Adds a new clinicalparametervalue</summary>
        /// <param name="model">The clinicalparametervalue data to be added</param>
        /// <returns>The result of the operation</returns>
        public Guid Create(ClinicalParameterValue model)
        {
            model.Id = CreateClinicalParameterValue(model);
            return model.Id;
        }

        /// <summary>Updates a specific clinicalparametervalue by its primary key</summary>
        /// <param name="id">The primary key of the clinicalparametervalue</param>
        /// <param name="updatedEntity">The clinicalparametervalue data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Update(Guid id, ClinicalParameterValue updatedEntity)
        {
            UpdateClinicalParameterValue(id, updatedEntity);
            return true;
        }

        /// <summary>Updates a specific clinicalparametervalue by its primary key</summary>
        /// <param name="id">The primary key of the clinicalparametervalue</param>
        /// <param name="updatedEntity">The clinicalparametervalue data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Patch(Guid id, JsonPatchDocument<ClinicalParameterValue> updatedEntity)
        {
            PatchClinicalParameterValue(id, updatedEntity);
            return true;
        }

        /// <summary>Deletes a specific clinicalparametervalue by its primary key</summary>
        /// <param name="id">The primary key of the clinicalparametervalue</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Delete(Guid id)
        {
            DeleteClinicalParameterValue(id);
            return true;
        }
        #region
        private List<ClinicalParameterValue> GetClinicalParameterValue(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            if (pageSize < 1)
            {
                throw new ApplicationException("Page size invalid!");
            }

            if (pageNumber < 1)
            {
                throw new ApplicationException("Page mumber invalid!");
            }

            var query = _dbContext.ClinicalParameterValue.IncludeRelated().AsQueryable();
            int skip = (pageNumber - 1) * pageSize;
            var result = FilterService<ClinicalParameterValue>.ApplyFilter(query, filters, searchTerm);
            if (!string.IsNullOrEmpty(sortField))
            {
                var parameter = Expression.Parameter(typeof(ClinicalParameterValue), "b");
                var property = Expression.Property(parameter, sortField);
                var lambda = Expression.Lambda<Func<ClinicalParameterValue, object>>(Expression.Convert(property, typeof(object)), parameter);
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

        private Guid CreateClinicalParameterValue(ClinicalParameterValue model)
        {
            _dbContext.ClinicalParameterValue.Add(model);
            _dbContext.SaveChanges();
            return model.Id;
        }

        private void UpdateClinicalParameterValue(Guid id, ClinicalParameterValue updatedEntity)
        {
            _dbContext.ClinicalParameterValue.Update(updatedEntity);
            _dbContext.SaveChanges();
        }

        private bool DeleteClinicalParameterValue(Guid id)
        {
            var entityData = _dbContext.ClinicalParameterValue.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            if (entityData == null)
            {
                throw new ApplicationException("No data found!");
            }

            _dbContext.ClinicalParameterValue.Remove(entityData);
            _dbContext.SaveChanges();
            return true;
        }

        private void PatchClinicalParameterValue(Guid id, JsonPatchDocument<ClinicalParameterValue> updatedEntity)
        {
            if (updatedEntity == null)
            {
                throw new ApplicationException("Patch document is missing!");
            }

            var existingEntity = _dbContext.ClinicalParameterValue.FirstOrDefault(t => t.Id == id);
            if (existingEntity == null)
            {
                throw new ApplicationException("No data found!");
            }

            updatedEntity.ApplyTo(existingEntity);
            _dbContext.ClinicalParameterValue.Update(existingEntity);
            _dbContext.SaveChanges();
        }
        #endregion
    }
}