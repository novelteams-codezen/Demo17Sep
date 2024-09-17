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
    /// The visitchiefcomplaintparameterService responsible for managing visitchiefcomplaintparameter related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting visitchiefcomplaintparameter information.
    /// </remarks>
    public interface IVisitChiefComplaintParameterService
    {
        /// <summary>Retrieves a specific visitchiefcomplaintparameter by its primary key</summary>
        /// <param name="id">The primary key of the visitchiefcomplaintparameter</param>
        /// <returns>The visitchiefcomplaintparameter data</returns>
        VisitChiefComplaintParameter GetById(Guid id);

        /// <summary>Retrieves a list of visitchiefcomplaintparameters based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of visitchiefcomplaintparameters</returns>
        List<VisitChiefComplaintParameter> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc");

        /// <summary>Adds a new visitchiefcomplaintparameter</summary>
        /// <param name="model">The visitchiefcomplaintparameter data to be added</param>
        /// <returns>The result of the operation</returns>
        Guid Create(VisitChiefComplaintParameter model);

        /// <summary>Updates a specific visitchiefcomplaintparameter by its primary key</summary>
        /// <param name="id">The primary key of the visitchiefcomplaintparameter</param>
        /// <param name="updatedEntity">The visitchiefcomplaintparameter data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Update(Guid id, VisitChiefComplaintParameter updatedEntity);

        /// <summary>Updates a specific visitchiefcomplaintparameter by its primary key</summary>
        /// <param name="id">The primary key of the visitchiefcomplaintparameter</param>
        /// <param name="updatedEntity">The visitchiefcomplaintparameter data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Patch(Guid id, JsonPatchDocument<VisitChiefComplaintParameter> updatedEntity);

        /// <summary>Deletes a specific visitchiefcomplaintparameter by its primary key</summary>
        /// <param name="id">The primary key of the visitchiefcomplaintparameter</param>
        /// <returns>The result of the operation</returns>
        bool Delete(Guid id);
    }

    /// <summary>
    /// The visitchiefcomplaintparameterService responsible for managing visitchiefcomplaintparameter related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting visitchiefcomplaintparameter information.
    /// </remarks>
    public class VisitChiefComplaintParameterService : IVisitChiefComplaintParameterService
    {
        private Demo17SepContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the VisitChiefComplaintParameter class.
        /// </summary>
        /// <param name="dbContext">dbContext value to set.</param>
        public VisitChiefComplaintParameterService(Demo17SepContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>Retrieves a specific visitchiefcomplaintparameter by its primary key</summary>
        /// <param name="id">The primary key of the visitchiefcomplaintparameter</param>
        /// <returns>The visitchiefcomplaintparameter data</returns>
        public VisitChiefComplaintParameter GetById(Guid id)
        {
            var entityData = _dbContext.VisitChiefComplaintParameter.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            return entityData;
        }

        /// <summary>Retrieves a list of visitchiefcomplaintparameters based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of visitchiefcomplaintparameters</returns>/// <exception cref="Exception"></exception>
        public List<VisitChiefComplaintParameter> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            var result = GetVisitChiefComplaintParameter(filters, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return result;
        }

        /// <summary>Adds a new visitchiefcomplaintparameter</summary>
        /// <param name="model">The visitchiefcomplaintparameter data to be added</param>
        /// <returns>The result of the operation</returns>
        public Guid Create(VisitChiefComplaintParameter model)
        {
            model.Id = CreateVisitChiefComplaintParameter(model);
            return model.Id;
        }

        /// <summary>Updates a specific visitchiefcomplaintparameter by its primary key</summary>
        /// <param name="id">The primary key of the visitchiefcomplaintparameter</param>
        /// <param name="updatedEntity">The visitchiefcomplaintparameter data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Update(Guid id, VisitChiefComplaintParameter updatedEntity)
        {
            UpdateVisitChiefComplaintParameter(id, updatedEntity);
            return true;
        }

        /// <summary>Updates a specific visitchiefcomplaintparameter by its primary key</summary>
        /// <param name="id">The primary key of the visitchiefcomplaintparameter</param>
        /// <param name="updatedEntity">The visitchiefcomplaintparameter data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Patch(Guid id, JsonPatchDocument<VisitChiefComplaintParameter> updatedEntity)
        {
            PatchVisitChiefComplaintParameter(id, updatedEntity);
            return true;
        }

        /// <summary>Deletes a specific visitchiefcomplaintparameter by its primary key</summary>
        /// <param name="id">The primary key of the visitchiefcomplaintparameter</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Delete(Guid id)
        {
            DeleteVisitChiefComplaintParameter(id);
            return true;
        }
        #region
        private List<VisitChiefComplaintParameter> GetVisitChiefComplaintParameter(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            if (pageSize < 1)
            {
                throw new ApplicationException("Page size invalid!");
            }

            if (pageNumber < 1)
            {
                throw new ApplicationException("Page mumber invalid!");
            }

            var query = _dbContext.VisitChiefComplaintParameter.IncludeRelated().AsQueryable();
            int skip = (pageNumber - 1) * pageSize;
            var result = FilterService<VisitChiefComplaintParameter>.ApplyFilter(query, filters, searchTerm);
            if (!string.IsNullOrEmpty(sortField))
            {
                var parameter = Expression.Parameter(typeof(VisitChiefComplaintParameter), "b");
                var property = Expression.Property(parameter, sortField);
                var lambda = Expression.Lambda<Func<VisitChiefComplaintParameter, object>>(Expression.Convert(property, typeof(object)), parameter);
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

        private Guid CreateVisitChiefComplaintParameter(VisitChiefComplaintParameter model)
        {
            _dbContext.VisitChiefComplaintParameter.Add(model);
            _dbContext.SaveChanges();
            return model.Id;
        }

        private void UpdateVisitChiefComplaintParameter(Guid id, VisitChiefComplaintParameter updatedEntity)
        {
            _dbContext.VisitChiefComplaintParameter.Update(updatedEntity);
            _dbContext.SaveChanges();
        }

        private bool DeleteVisitChiefComplaintParameter(Guid id)
        {
            var entityData = _dbContext.VisitChiefComplaintParameter.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            if (entityData == null)
            {
                throw new ApplicationException("No data found!");
            }

            _dbContext.VisitChiefComplaintParameter.Remove(entityData);
            _dbContext.SaveChanges();
            return true;
        }

        private void PatchVisitChiefComplaintParameter(Guid id, JsonPatchDocument<VisitChiefComplaintParameter> updatedEntity)
        {
            if (updatedEntity == null)
            {
                throw new ApplicationException("Patch document is missing!");
            }

            var existingEntity = _dbContext.VisitChiefComplaintParameter.FirstOrDefault(t => t.Id == id);
            if (existingEntity == null)
            {
                throw new ApplicationException("No data found!");
            }

            updatedEntity.ApplyTo(existingEntity);
            _dbContext.VisitChiefComplaintParameter.Update(existingEntity);
            _dbContext.SaveChanges();
        }
        #endregion
    }
}