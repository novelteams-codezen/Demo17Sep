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
    /// The qualificationService responsible for managing qualification related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting qualification information.
    /// </remarks>
    public interface IQualificationService
    {
        /// <summary>Retrieves a specific qualification by its primary key</summary>
        /// <param name="id">The primary key of the qualification</param>
        /// <returns>The qualification data</returns>
        Qualification GetById(Guid id);

        /// <summary>Retrieves a list of qualifications based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of qualifications</returns>
        List<Qualification> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc");

        /// <summary>Adds a new qualification</summary>
        /// <param name="model">The qualification data to be added</param>
        /// <returns>The result of the operation</returns>
        Guid Create(Qualification model);

        /// <summary>Updates a specific qualification by its primary key</summary>
        /// <param name="id">The primary key of the qualification</param>
        /// <param name="updatedEntity">The qualification data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Update(Guid id, Qualification updatedEntity);

        /// <summary>Updates a specific qualification by its primary key</summary>
        /// <param name="id">The primary key of the qualification</param>
        /// <param name="updatedEntity">The qualification data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Patch(Guid id, JsonPatchDocument<Qualification> updatedEntity);

        /// <summary>Deletes a specific qualification by its primary key</summary>
        /// <param name="id">The primary key of the qualification</param>
        /// <returns>The result of the operation</returns>
        bool Delete(Guid id);
    }

    /// <summary>
    /// The qualificationService responsible for managing qualification related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting qualification information.
    /// </remarks>
    public class QualificationService : IQualificationService
    {
        private Demo17SepContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the Qualification class.
        /// </summary>
        /// <param name="dbContext">dbContext value to set.</param>
        public QualificationService(Demo17SepContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>Retrieves a specific qualification by its primary key</summary>
        /// <param name="id">The primary key of the qualification</param>
        /// <returns>The qualification data</returns>
        public Qualification GetById(Guid id)
        {
            var entityData = _dbContext.Qualification.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            return entityData;
        }

        /// <summary>Retrieves a list of qualifications based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of qualifications</returns>/// <exception cref="Exception"></exception>
        public List<Qualification> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            var result = GetQualification(filters, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return result;
        }

        /// <summary>Adds a new qualification</summary>
        /// <param name="model">The qualification data to be added</param>
        /// <returns>The result of the operation</returns>
        public Guid Create(Qualification model)
        {
            model.Id = CreateQualification(model);
            return model.Id;
        }

        /// <summary>Updates a specific qualification by its primary key</summary>
        /// <param name="id">The primary key of the qualification</param>
        /// <param name="updatedEntity">The qualification data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Update(Guid id, Qualification updatedEntity)
        {
            UpdateQualification(id, updatedEntity);
            return true;
        }

        /// <summary>Updates a specific qualification by its primary key</summary>
        /// <param name="id">The primary key of the qualification</param>
        /// <param name="updatedEntity">The qualification data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Patch(Guid id, JsonPatchDocument<Qualification> updatedEntity)
        {
            PatchQualification(id, updatedEntity);
            return true;
        }

        /// <summary>Deletes a specific qualification by its primary key</summary>
        /// <param name="id">The primary key of the qualification</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Delete(Guid id)
        {
            DeleteQualification(id);
            return true;
        }
        #region
        private List<Qualification> GetQualification(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            if (pageSize < 1)
            {
                throw new ApplicationException("Page size invalid!");
            }

            if (pageNumber < 1)
            {
                throw new ApplicationException("Page mumber invalid!");
            }

            var query = _dbContext.Qualification.IncludeRelated().AsQueryable();
            int skip = (pageNumber - 1) * pageSize;
            var result = FilterService<Qualification>.ApplyFilter(query, filters, searchTerm);
            if (!string.IsNullOrEmpty(sortField))
            {
                var parameter = Expression.Parameter(typeof(Qualification), "b");
                var property = Expression.Property(parameter, sortField);
                var lambda = Expression.Lambda<Func<Qualification, object>>(Expression.Convert(property, typeof(object)), parameter);
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

        private Guid CreateQualification(Qualification model)
        {
            _dbContext.Qualification.Add(model);
            _dbContext.SaveChanges();
            return model.Id;
        }

        private void UpdateQualification(Guid id, Qualification updatedEntity)
        {
            _dbContext.Qualification.Update(updatedEntity);
            _dbContext.SaveChanges();
        }

        private bool DeleteQualification(Guid id)
        {
            var entityData = _dbContext.Qualification.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            if (entityData == null)
            {
                throw new ApplicationException("No data found!");
            }

            _dbContext.Qualification.Remove(entityData);
            _dbContext.SaveChanges();
            return true;
        }

        private void PatchQualification(Guid id, JsonPatchDocument<Qualification> updatedEntity)
        {
            if (updatedEntity == null)
            {
                throw new ApplicationException("Patch document is missing!");
            }

            var existingEntity = _dbContext.Qualification.FirstOrDefault(t => t.Id == id);
            if (existingEntity == null)
            {
                throw new ApplicationException("No data found!");
            }

            updatedEntity.ApplyTo(existingEntity);
            _dbContext.Qualification.Update(existingEntity);
            _dbContext.SaveChanges();
        }
        #endregion
    }
}