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
    /// The formulationService responsible for managing formulation related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting formulation information.
    /// </remarks>
    public interface IFormulationService
    {
        /// <summary>Retrieves a specific formulation by its primary key</summary>
        /// <param name="id">The primary key of the formulation</param>
        /// <returns>The formulation data</returns>
        Formulation GetById(Guid id);

        /// <summary>Retrieves a list of formulations based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of formulations</returns>
        List<Formulation> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc");

        /// <summary>Adds a new formulation</summary>
        /// <param name="model">The formulation data to be added</param>
        /// <returns>The result of the operation</returns>
        Guid Create(Formulation model);

        /// <summary>Updates a specific formulation by its primary key</summary>
        /// <param name="id">The primary key of the formulation</param>
        /// <param name="updatedEntity">The formulation data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Update(Guid id, Formulation updatedEntity);

        /// <summary>Updates a specific formulation by its primary key</summary>
        /// <param name="id">The primary key of the formulation</param>
        /// <param name="updatedEntity">The formulation data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Patch(Guid id, JsonPatchDocument<Formulation> updatedEntity);

        /// <summary>Deletes a specific formulation by its primary key</summary>
        /// <param name="id">The primary key of the formulation</param>
        /// <returns>The result of the operation</returns>
        bool Delete(Guid id);
    }

    /// <summary>
    /// The formulationService responsible for managing formulation related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting formulation information.
    /// </remarks>
    public class FormulationService : IFormulationService
    {
        private Demo17SepContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the Formulation class.
        /// </summary>
        /// <param name="dbContext">dbContext value to set.</param>
        public FormulationService(Demo17SepContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>Retrieves a specific formulation by its primary key</summary>
        /// <param name="id">The primary key of the formulation</param>
        /// <returns>The formulation data</returns>
        public Formulation GetById(Guid id)
        {
            var entityData = _dbContext.Formulation.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            return entityData;
        }

        /// <summary>Retrieves a list of formulations based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of formulations</returns>/// <exception cref="Exception"></exception>
        public List<Formulation> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            var result = GetFormulation(filters, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return result;
        }

        /// <summary>Adds a new formulation</summary>
        /// <param name="model">The formulation data to be added</param>
        /// <returns>The result of the operation</returns>
        public Guid Create(Formulation model)
        {
            model.Id = CreateFormulation(model);
            return model.Id;
        }

        /// <summary>Updates a specific formulation by its primary key</summary>
        /// <param name="id">The primary key of the formulation</param>
        /// <param name="updatedEntity">The formulation data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Update(Guid id, Formulation updatedEntity)
        {
            UpdateFormulation(id, updatedEntity);
            return true;
        }

        /// <summary>Updates a specific formulation by its primary key</summary>
        /// <param name="id">The primary key of the formulation</param>
        /// <param name="updatedEntity">The formulation data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Patch(Guid id, JsonPatchDocument<Formulation> updatedEntity)
        {
            PatchFormulation(id, updatedEntity);
            return true;
        }

        /// <summary>Deletes a specific formulation by its primary key</summary>
        /// <param name="id">The primary key of the formulation</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Delete(Guid id)
        {
            DeleteFormulation(id);
            return true;
        }
        #region
        private List<Formulation> GetFormulation(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            if (pageSize < 1)
            {
                throw new ApplicationException("Page size invalid!");
            }

            if (pageNumber < 1)
            {
                throw new ApplicationException("Page mumber invalid!");
            }

            var query = _dbContext.Formulation.IncludeRelated().AsQueryable();
            int skip = (pageNumber - 1) * pageSize;
            var result = FilterService<Formulation>.ApplyFilter(query, filters, searchTerm);
            if (!string.IsNullOrEmpty(sortField))
            {
                var parameter = Expression.Parameter(typeof(Formulation), "b");
                var property = Expression.Property(parameter, sortField);
                var lambda = Expression.Lambda<Func<Formulation, object>>(Expression.Convert(property, typeof(object)), parameter);
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

        private Guid CreateFormulation(Formulation model)
        {
            _dbContext.Formulation.Add(model);
            _dbContext.SaveChanges();
            return model.Id;
        }

        private void UpdateFormulation(Guid id, Formulation updatedEntity)
        {
            _dbContext.Formulation.Update(updatedEntity);
            _dbContext.SaveChanges();
        }

        private bool DeleteFormulation(Guid id)
        {
            var entityData = _dbContext.Formulation.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            if (entityData == null)
            {
                throw new ApplicationException("No data found!");
            }

            _dbContext.Formulation.Remove(entityData);
            _dbContext.SaveChanges();
            return true;
        }

        private void PatchFormulation(Guid id, JsonPatchDocument<Formulation> updatedEntity)
        {
            if (updatedEntity == null)
            {
                throw new ApplicationException("Patch document is missing!");
            }

            var existingEntity = _dbContext.Formulation.FirstOrDefault(t => t.Id == id);
            if (existingEntity == null)
            {
                throw new ApplicationException("No data found!");
            }

            updatedEntity.ApplyTo(existingEntity);
            _dbContext.Formulation.Update(existingEntity);
            _dbContext.SaveChanges();
        }
        #endregion
    }
}