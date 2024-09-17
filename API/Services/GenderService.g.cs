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
    /// The genderService responsible for managing gender related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting gender information.
    /// </remarks>
    public interface IGenderService
    {
        /// <summary>Retrieves a specific gender by its primary key</summary>
        /// <param name="id">The primary key of the gender</param>
        /// <returns>The gender data</returns>
        Gender GetById(Guid id);

        /// <summary>Retrieves a list of genders based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of genders</returns>
        List<Gender> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc");

        /// <summary>Adds a new gender</summary>
        /// <param name="model">The gender data to be added</param>
        /// <returns>The result of the operation</returns>
        Guid Create(Gender model);

        /// <summary>Updates a specific gender by its primary key</summary>
        /// <param name="id">The primary key of the gender</param>
        /// <param name="updatedEntity">The gender data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Update(Guid id, Gender updatedEntity);

        /// <summary>Updates a specific gender by its primary key</summary>
        /// <param name="id">The primary key of the gender</param>
        /// <param name="updatedEntity">The gender data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Patch(Guid id, JsonPatchDocument<Gender> updatedEntity);

        /// <summary>Deletes a specific gender by its primary key</summary>
        /// <param name="id">The primary key of the gender</param>
        /// <returns>The result of the operation</returns>
        bool Delete(Guid id);
    }

    /// <summary>
    /// The genderService responsible for managing gender related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting gender information.
    /// </remarks>
    public class GenderService : IGenderService
    {
        private Demo17SepContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the Gender class.
        /// </summary>
        /// <param name="dbContext">dbContext value to set.</param>
        public GenderService(Demo17SepContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>Retrieves a specific gender by its primary key</summary>
        /// <param name="id">The primary key of the gender</param>
        /// <returns>The gender data</returns>
        public Gender GetById(Guid id)
        {
            var entityData = _dbContext.Gender.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            return entityData;
        }

        /// <summary>Retrieves a list of genders based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of genders</returns>/// <exception cref="Exception"></exception>
        public List<Gender> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            var result = GetGender(filters, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return result;
        }

        /// <summary>Adds a new gender</summary>
        /// <param name="model">The gender data to be added</param>
        /// <returns>The result of the operation</returns>
        public Guid Create(Gender model)
        {
            model.Id = CreateGender(model);
            return model.Id;
        }

        /// <summary>Updates a specific gender by its primary key</summary>
        /// <param name="id">The primary key of the gender</param>
        /// <param name="updatedEntity">The gender data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Update(Guid id, Gender updatedEntity)
        {
            UpdateGender(id, updatedEntity);
            return true;
        }

        /// <summary>Updates a specific gender by its primary key</summary>
        /// <param name="id">The primary key of the gender</param>
        /// <param name="updatedEntity">The gender data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Patch(Guid id, JsonPatchDocument<Gender> updatedEntity)
        {
            PatchGender(id, updatedEntity);
            return true;
        }

        /// <summary>Deletes a specific gender by its primary key</summary>
        /// <param name="id">The primary key of the gender</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Delete(Guid id)
        {
            DeleteGender(id);
            return true;
        }
        #region
        private List<Gender> GetGender(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            if (pageSize < 1)
            {
                throw new ApplicationException("Page size invalid!");
            }

            if (pageNumber < 1)
            {
                throw new ApplicationException("Page mumber invalid!");
            }

            var query = _dbContext.Gender.IncludeRelated().AsQueryable();
            int skip = (pageNumber - 1) * pageSize;
            var result = FilterService<Gender>.ApplyFilter(query, filters, searchTerm);
            if (!string.IsNullOrEmpty(sortField))
            {
                var parameter = Expression.Parameter(typeof(Gender), "b");
                var property = Expression.Property(parameter, sortField);
                var lambda = Expression.Lambda<Func<Gender, object>>(Expression.Convert(property, typeof(object)), parameter);
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

        private Guid CreateGender(Gender model)
        {
            _dbContext.Gender.Add(model);
            _dbContext.SaveChanges();
            return model.Id;
        }

        private void UpdateGender(Guid id, Gender updatedEntity)
        {
            _dbContext.Gender.Update(updatedEntity);
            _dbContext.SaveChanges();
        }

        private bool DeleteGender(Guid id)
        {
            var entityData = _dbContext.Gender.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            if (entityData == null)
            {
                throw new ApplicationException("No data found!");
            }

            _dbContext.Gender.Remove(entityData);
            _dbContext.SaveChanges();
            return true;
        }

        private void PatchGender(Guid id, JsonPatchDocument<Gender> updatedEntity)
        {
            if (updatedEntity == null)
            {
                throw new ApplicationException("Patch document is missing!");
            }

            var existingEntity = _dbContext.Gender.FirstOrDefault(t => t.Id == id);
            if (existingEntity == null)
            {
                throw new ApplicationException("No data found!");
            }

            updatedEntity.ApplyTo(existingEntity);
            _dbContext.Gender.Update(existingEntity);
            _dbContext.SaveChanges();
        }
        #endregion
    }
}