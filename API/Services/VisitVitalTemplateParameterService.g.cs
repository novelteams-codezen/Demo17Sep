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
    /// The visitvitaltemplateparameterService responsible for managing visitvitaltemplateparameter related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting visitvitaltemplateparameter information.
    /// </remarks>
    public interface IVisitVitalTemplateParameterService
    {
        /// <summary>Retrieves a specific visitvitaltemplateparameter by its primary key</summary>
        /// <param name="id">The primary key of the visitvitaltemplateparameter</param>
        /// <returns>The visitvitaltemplateparameter data</returns>
        VisitVitalTemplateParameter GetById(Guid id);

        /// <summary>Retrieves a list of visitvitaltemplateparameters based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of visitvitaltemplateparameters</returns>
        List<VisitVitalTemplateParameter> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc");

        /// <summary>Adds a new visitvitaltemplateparameter</summary>
        /// <param name="model">The visitvitaltemplateparameter data to be added</param>
        /// <returns>The result of the operation</returns>
        Guid Create(VisitVitalTemplateParameter model);

        /// <summary>Updates a specific visitvitaltemplateparameter by its primary key</summary>
        /// <param name="id">The primary key of the visitvitaltemplateparameter</param>
        /// <param name="updatedEntity">The visitvitaltemplateparameter data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Update(Guid id, VisitVitalTemplateParameter updatedEntity);

        /// <summary>Updates a specific visitvitaltemplateparameter by its primary key</summary>
        /// <param name="id">The primary key of the visitvitaltemplateparameter</param>
        /// <param name="updatedEntity">The visitvitaltemplateparameter data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Patch(Guid id, JsonPatchDocument<VisitVitalTemplateParameter> updatedEntity);

        /// <summary>Deletes a specific visitvitaltemplateparameter by its primary key</summary>
        /// <param name="id">The primary key of the visitvitaltemplateparameter</param>
        /// <returns>The result of the operation</returns>
        bool Delete(Guid id);
    }

    /// <summary>
    /// The visitvitaltemplateparameterService responsible for managing visitvitaltemplateparameter related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting visitvitaltemplateparameter information.
    /// </remarks>
    public class VisitVitalTemplateParameterService : IVisitVitalTemplateParameterService
    {
        private Demo17SepContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the VisitVitalTemplateParameter class.
        /// </summary>
        /// <param name="dbContext">dbContext value to set.</param>
        public VisitVitalTemplateParameterService(Demo17SepContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>Retrieves a specific visitvitaltemplateparameter by its primary key</summary>
        /// <param name="id">The primary key of the visitvitaltemplateparameter</param>
        /// <returns>The visitvitaltemplateparameter data</returns>
        public VisitVitalTemplateParameter GetById(Guid id)
        {
            var entityData = _dbContext.VisitVitalTemplateParameter.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            return entityData;
        }

        /// <summary>Retrieves a list of visitvitaltemplateparameters based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of visitvitaltemplateparameters</returns>/// <exception cref="Exception"></exception>
        public List<VisitVitalTemplateParameter> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            var result = GetVisitVitalTemplateParameter(filters, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return result;
        }

        /// <summary>Adds a new visitvitaltemplateparameter</summary>
        /// <param name="model">The visitvitaltemplateparameter data to be added</param>
        /// <returns>The result of the operation</returns>
        public Guid Create(VisitVitalTemplateParameter model)
        {
            model.Id = CreateVisitVitalTemplateParameter(model);
            return model.Id;
        }

        /// <summary>Updates a specific visitvitaltemplateparameter by its primary key</summary>
        /// <param name="id">The primary key of the visitvitaltemplateparameter</param>
        /// <param name="updatedEntity">The visitvitaltemplateparameter data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Update(Guid id, VisitVitalTemplateParameter updatedEntity)
        {
            UpdateVisitVitalTemplateParameter(id, updatedEntity);
            return true;
        }

        /// <summary>Updates a specific visitvitaltemplateparameter by its primary key</summary>
        /// <param name="id">The primary key of the visitvitaltemplateparameter</param>
        /// <param name="updatedEntity">The visitvitaltemplateparameter data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Patch(Guid id, JsonPatchDocument<VisitVitalTemplateParameter> updatedEntity)
        {
            PatchVisitVitalTemplateParameter(id, updatedEntity);
            return true;
        }

        /// <summary>Deletes a specific visitvitaltemplateparameter by its primary key</summary>
        /// <param name="id">The primary key of the visitvitaltemplateparameter</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Delete(Guid id)
        {
            DeleteVisitVitalTemplateParameter(id);
            return true;
        }
        #region
        private List<VisitVitalTemplateParameter> GetVisitVitalTemplateParameter(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            if (pageSize < 1)
            {
                throw new ApplicationException("Page size invalid!");
            }

            if (pageNumber < 1)
            {
                throw new ApplicationException("Page mumber invalid!");
            }

            var query = _dbContext.VisitVitalTemplateParameter.IncludeRelated().AsQueryable();
            int skip = (pageNumber - 1) * pageSize;
            var result = FilterService<VisitVitalTemplateParameter>.ApplyFilter(query, filters, searchTerm);
            if (!string.IsNullOrEmpty(sortField))
            {
                var parameter = Expression.Parameter(typeof(VisitVitalTemplateParameter), "b");
                var property = Expression.Property(parameter, sortField);
                var lambda = Expression.Lambda<Func<VisitVitalTemplateParameter, object>>(Expression.Convert(property, typeof(object)), parameter);
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

        private Guid CreateVisitVitalTemplateParameter(VisitVitalTemplateParameter model)
        {
            _dbContext.VisitVitalTemplateParameter.Add(model);
            _dbContext.SaveChanges();
            return model.Id;
        }

        private void UpdateVisitVitalTemplateParameter(Guid id, VisitVitalTemplateParameter updatedEntity)
        {
            _dbContext.VisitVitalTemplateParameter.Update(updatedEntity);
            _dbContext.SaveChanges();
        }

        private bool DeleteVisitVitalTemplateParameter(Guid id)
        {
            var entityData = _dbContext.VisitVitalTemplateParameter.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            if (entityData == null)
            {
                throw new ApplicationException("No data found!");
            }

            _dbContext.VisitVitalTemplateParameter.Remove(entityData);
            _dbContext.SaveChanges();
            return true;
        }

        private void PatchVisitVitalTemplateParameter(Guid id, JsonPatchDocument<VisitVitalTemplateParameter> updatedEntity)
        {
            if (updatedEntity == null)
            {
                throw new ApplicationException("Patch document is missing!");
            }

            var existingEntity = _dbContext.VisitVitalTemplateParameter.FirstOrDefault(t => t.Id == id);
            if (existingEntity == null)
            {
                throw new ApplicationException("No data found!");
            }

            updatedEntity.ApplyTo(existingEntity);
            _dbContext.VisitVitalTemplateParameter.Update(existingEntity);
            _dbContext.SaveChanges();
        }
        #endregion
    }
}