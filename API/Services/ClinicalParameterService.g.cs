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
    /// The clinicalparameterService responsible for managing clinicalparameter related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting clinicalparameter information.
    /// </remarks>
    public interface IClinicalParameterService
    {
        /// <summary>Retrieves a specific clinicalparameter by its primary key</summary>
        /// <param name="id">The primary key of the clinicalparameter</param>
        /// <returns>The clinicalparameter data</returns>
        ClinicalParameter GetById(Guid id);

        /// <summary>Retrieves a list of clinicalparameters based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of clinicalparameters</returns>
        List<ClinicalParameter> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc");

        /// <summary>Adds a new clinicalparameter</summary>
        /// <param name="model">The clinicalparameter data to be added</param>
        /// <returns>The result of the operation</returns>
        Guid Create(ClinicalParameter model);

        /// <summary>Updates a specific clinicalparameter by its primary key</summary>
        /// <param name="id">The primary key of the clinicalparameter</param>
        /// <param name="updatedEntity">The clinicalparameter data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Update(Guid id, ClinicalParameter updatedEntity);

        /// <summary>Updates a specific clinicalparameter by its primary key</summary>
        /// <param name="id">The primary key of the clinicalparameter</param>
        /// <param name="updatedEntity">The clinicalparameter data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Patch(Guid id, JsonPatchDocument<ClinicalParameter> updatedEntity);

        /// <summary>Deletes a specific clinicalparameter by its primary key</summary>
        /// <param name="id">The primary key of the clinicalparameter</param>
        /// <returns>The result of the operation</returns>
        bool Delete(Guid id);
    }

    /// <summary>
    /// The clinicalparameterService responsible for managing clinicalparameter related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting clinicalparameter information.
    /// </remarks>
    public class ClinicalParameterService : IClinicalParameterService
    {
        private Demo17SepContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the ClinicalParameter class.
        /// </summary>
        /// <param name="dbContext">dbContext value to set.</param>
        public ClinicalParameterService(Demo17SepContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>Retrieves a specific clinicalparameter by its primary key</summary>
        /// <param name="id">The primary key of the clinicalparameter</param>
        /// <returns>The clinicalparameter data</returns>
        public ClinicalParameter GetById(Guid id)
        {
            var entityData = _dbContext.ClinicalParameter.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            return entityData;
        }

        /// <summary>Retrieves a list of clinicalparameters based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of clinicalparameters</returns>/// <exception cref="Exception"></exception>
        public List<ClinicalParameter> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            var result = GetClinicalParameter(filters, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return result;
        }

        /// <summary>Adds a new clinicalparameter</summary>
        /// <param name="model">The clinicalparameter data to be added</param>
        /// <returns>The result of the operation</returns>
        public Guid Create(ClinicalParameter model)
        {
            model.Id = CreateClinicalParameter(model);
            return model.Id;
        }

        /// <summary>Updates a specific clinicalparameter by its primary key</summary>
        /// <param name="id">The primary key of the clinicalparameter</param>
        /// <param name="updatedEntity">The clinicalparameter data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Update(Guid id, ClinicalParameter updatedEntity)
        {
            UpdateClinicalParameter(id, updatedEntity);
            return true;
        }

        /// <summary>Updates a specific clinicalparameter by its primary key</summary>
        /// <param name="id">The primary key of the clinicalparameter</param>
        /// <param name="updatedEntity">The clinicalparameter data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Patch(Guid id, JsonPatchDocument<ClinicalParameter> updatedEntity)
        {
            PatchClinicalParameter(id, updatedEntity);
            return true;
        }

        /// <summary>Deletes a specific clinicalparameter by its primary key</summary>
        /// <param name="id">The primary key of the clinicalparameter</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Delete(Guid id)
        {
            DeleteClinicalParameter(id);
            return true;
        }
        #region
        private List<ClinicalParameter> GetClinicalParameter(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            if (pageSize < 1)
            {
                throw new ApplicationException("Page size invalid!");
            }

            if (pageNumber < 1)
            {
                throw new ApplicationException("Page mumber invalid!");
            }

            var query = _dbContext.ClinicalParameter.IncludeRelated().AsQueryable();
            int skip = (pageNumber - 1) * pageSize;
            var result = FilterService<ClinicalParameter>.ApplyFilter(query, filters, searchTerm);
            if (!string.IsNullOrEmpty(sortField))
            {
                var parameter = Expression.Parameter(typeof(ClinicalParameter), "b");
                var property = Expression.Property(parameter, sortField);
                var lambda = Expression.Lambda<Func<ClinicalParameter, object>>(Expression.Convert(property, typeof(object)), parameter);
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

        private Guid CreateClinicalParameter(ClinicalParameter model)
        {
            _dbContext.ClinicalParameter.Add(model);
            _dbContext.SaveChanges();
            return model.Id;
        }

        private void UpdateClinicalParameter(Guid id, ClinicalParameter updatedEntity)
        {
            _dbContext.ClinicalParameter.Update(updatedEntity);
            _dbContext.SaveChanges();
        }

        private bool DeleteClinicalParameter(Guid id)
        {
            var entityData = _dbContext.ClinicalParameter.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            if (entityData == null)
            {
                throw new ApplicationException("No data found!");
            }

            _dbContext.ClinicalParameter.Remove(entityData);
            _dbContext.SaveChanges();
            return true;
        }

        private void PatchClinicalParameter(Guid id, JsonPatchDocument<ClinicalParameter> updatedEntity)
        {
            if (updatedEntity == null)
            {
                throw new ApplicationException("Patch document is missing!");
            }

            var existingEntity = _dbContext.ClinicalParameter.FirstOrDefault(t => t.Id == id);
            if (existingEntity == null)
            {
                throw new ApplicationException("No data found!");
            }

            updatedEntity.ApplyTo(existingEntity);
            _dbContext.ClinicalParameter.Update(existingEntity);
            _dbContext.SaveChanges();
        }
        #endregion
    }
}