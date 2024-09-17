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
    /// The visitService responsible for managing visit related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting visit information.
    /// </remarks>
    public interface IVisitService
    {
        /// <summary>Retrieves a specific visit by its primary key</summary>
        /// <param name="id">The primary key of the visit</param>
        /// <returns>The visit data</returns>
        Visit GetById(Guid id);

        /// <summary>Retrieves a list of visits based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of visits</returns>
        List<Visit> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc");

        /// <summary>Adds a new visit</summary>
        /// <param name="model">The visit data to be added</param>
        /// <returns>The result of the operation</returns>
        Guid Create(Visit model);

        /// <summary>Updates a specific visit by its primary key</summary>
        /// <param name="id">The primary key of the visit</param>
        /// <param name="updatedEntity">The visit data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Update(Guid id, Visit updatedEntity);

        /// <summary>Updates a specific visit by its primary key</summary>
        /// <param name="id">The primary key of the visit</param>
        /// <param name="updatedEntity">The visit data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Patch(Guid id, JsonPatchDocument<Visit> updatedEntity);

        /// <summary>Deletes a specific visit by its primary key</summary>
        /// <param name="id">The primary key of the visit</param>
        /// <returns>The result of the operation</returns>
        bool Delete(Guid id);
    }

    /// <summary>
    /// The visitService responsible for managing visit related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting visit information.
    /// </remarks>
    public class VisitService : IVisitService
    {
        private Demo17SepContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the Visit class.
        /// </summary>
        /// <param name="dbContext">dbContext value to set.</param>
        public VisitService(Demo17SepContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>Retrieves a specific visit by its primary key</summary>
        /// <param name="id">The primary key of the visit</param>
        /// <returns>The visit data</returns>
        public Visit GetById(Guid id)
        {
            var entityData = _dbContext.Visit.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            return entityData;
        }

        /// <summary>Retrieves a list of visits based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of visits</returns>/// <exception cref="Exception"></exception>
        public List<Visit> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            var result = GetVisit(filters, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return result;
        }

        /// <summary>Adds a new visit</summary>
        /// <param name="model">The visit data to be added</param>
        /// <returns>The result of the operation</returns>
        public Guid Create(Visit model)
        {
            model.Id = CreateVisit(model);
            return model.Id;
        }

        /// <summary>Updates a specific visit by its primary key</summary>
        /// <param name="id">The primary key of the visit</param>
        /// <param name="updatedEntity">The visit data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Update(Guid id, Visit updatedEntity)
        {
            UpdateVisit(id, updatedEntity);
            return true;
        }

        /// <summary>Updates a specific visit by its primary key</summary>
        /// <param name="id">The primary key of the visit</param>
        /// <param name="updatedEntity">The visit data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Patch(Guid id, JsonPatchDocument<Visit> updatedEntity)
        {
            PatchVisit(id, updatedEntity);
            return true;
        }

        /// <summary>Deletes a specific visit by its primary key</summary>
        /// <param name="id">The primary key of the visit</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Delete(Guid id)
        {
            DeleteVisit(id);
            return true;
        }
        #region
        private List<Visit> GetVisit(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            if (pageSize < 1)
            {
                throw new ApplicationException("Page size invalid!");
            }

            if (pageNumber < 1)
            {
                throw new ApplicationException("Page mumber invalid!");
            }

            var query = _dbContext.Visit.IncludeRelated().AsQueryable();
            int skip = (pageNumber - 1) * pageSize;
            var result = FilterService<Visit>.ApplyFilter(query, filters, searchTerm);
            if (!string.IsNullOrEmpty(sortField))
            {
                var parameter = Expression.Parameter(typeof(Visit), "b");
                var property = Expression.Property(parameter, sortField);
                var lambda = Expression.Lambda<Func<Visit, object>>(Expression.Convert(property, typeof(object)), parameter);
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

        private Guid CreateVisit(Visit model)
        {
            _dbContext.Visit.Add(model);
            _dbContext.SaveChanges();
            return model.Id;
        }

        private void UpdateVisit(Guid id, Visit updatedEntity)
        {
            _dbContext.Visit.Update(updatedEntity);
            _dbContext.SaveChanges();
        }

        private bool DeleteVisit(Guid id)
        {
            var entityData = _dbContext.Visit.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            if (entityData == null)
            {
                throw new ApplicationException("No data found!");
            }

            _dbContext.Visit.Remove(entityData);
            _dbContext.SaveChanges();
            return true;
        }

        private void PatchVisit(Guid id, JsonPatchDocument<Visit> updatedEntity)
        {
            if (updatedEntity == null)
            {
                throw new ApplicationException("Patch document is missing!");
            }

            var existingEntity = _dbContext.Visit.FirstOrDefault(t => t.Id == id);
            if (existingEntity == null)
            {
                throw new ApplicationException("No data found!");
            }

            updatedEntity.ApplyTo(existingEntity);
            _dbContext.Visit.Update(existingEntity);
            _dbContext.SaveChanges();
        }
        #endregion
    }
}