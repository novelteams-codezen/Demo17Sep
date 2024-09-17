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
    /// The financesettingService responsible for managing financesetting related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting financesetting information.
    /// </remarks>
    public interface IFinanceSettingService
    {
        /// <summary>Retrieves a specific financesetting by its primary key</summary>
        /// <param name="id">The primary key of the financesetting</param>
        /// <returns>The financesetting data</returns>
        FinanceSetting GetById(Guid id);

        /// <summary>Retrieves a list of financesettings based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of financesettings</returns>
        List<FinanceSetting> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc");

        /// <summary>Adds a new financesetting</summary>
        /// <param name="model">The financesetting data to be added</param>
        /// <returns>The result of the operation</returns>
        Guid Create(FinanceSetting model);

        /// <summary>Updates a specific financesetting by its primary key</summary>
        /// <param name="id">The primary key of the financesetting</param>
        /// <param name="updatedEntity">The financesetting data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Update(Guid id, FinanceSetting updatedEntity);

        /// <summary>Updates a specific financesetting by its primary key</summary>
        /// <param name="id">The primary key of the financesetting</param>
        /// <param name="updatedEntity">The financesetting data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Patch(Guid id, JsonPatchDocument<FinanceSetting> updatedEntity);

        /// <summary>Deletes a specific financesetting by its primary key</summary>
        /// <param name="id">The primary key of the financesetting</param>
        /// <returns>The result of the operation</returns>
        bool Delete(Guid id);
    }

    /// <summary>
    /// The financesettingService responsible for managing financesetting related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting financesetting information.
    /// </remarks>
    public class FinanceSettingService : IFinanceSettingService
    {
        private Demo17SepContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the FinanceSetting class.
        /// </summary>
        /// <param name="dbContext">dbContext value to set.</param>
        public FinanceSettingService(Demo17SepContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>Retrieves a specific financesetting by its primary key</summary>
        /// <param name="id">The primary key of the financesetting</param>
        /// <returns>The financesetting data</returns>
        public FinanceSetting GetById(Guid id)
        {
            var entityData = _dbContext.FinanceSetting.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            return entityData;
        }

        /// <summary>Retrieves a list of financesettings based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of financesettings</returns>/// <exception cref="Exception"></exception>
        public List<FinanceSetting> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            var result = GetFinanceSetting(filters, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return result;
        }

        /// <summary>Adds a new financesetting</summary>
        /// <param name="model">The financesetting data to be added</param>
        /// <returns>The result of the operation</returns>
        public Guid Create(FinanceSetting model)
        {
            model.Id = CreateFinanceSetting(model);
            return model.Id;
        }

        /// <summary>Updates a specific financesetting by its primary key</summary>
        /// <param name="id">The primary key of the financesetting</param>
        /// <param name="updatedEntity">The financesetting data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Update(Guid id, FinanceSetting updatedEntity)
        {
            UpdateFinanceSetting(id, updatedEntity);
            return true;
        }

        /// <summary>Updates a specific financesetting by its primary key</summary>
        /// <param name="id">The primary key of the financesetting</param>
        /// <param name="updatedEntity">The financesetting data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Patch(Guid id, JsonPatchDocument<FinanceSetting> updatedEntity)
        {
            PatchFinanceSetting(id, updatedEntity);
            return true;
        }

        /// <summary>Deletes a specific financesetting by its primary key</summary>
        /// <param name="id">The primary key of the financesetting</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Delete(Guid id)
        {
            DeleteFinanceSetting(id);
            return true;
        }
        #region
        private List<FinanceSetting> GetFinanceSetting(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            if (pageSize < 1)
            {
                throw new ApplicationException("Page size invalid!");
            }

            if (pageNumber < 1)
            {
                throw new ApplicationException("Page mumber invalid!");
            }

            var query = _dbContext.FinanceSetting.IncludeRelated().AsQueryable();
            int skip = (pageNumber - 1) * pageSize;
            var result = FilterService<FinanceSetting>.ApplyFilter(query, filters, searchTerm);
            if (!string.IsNullOrEmpty(sortField))
            {
                var parameter = Expression.Parameter(typeof(FinanceSetting), "b");
                var property = Expression.Property(parameter, sortField);
                var lambda = Expression.Lambda<Func<FinanceSetting, object>>(Expression.Convert(property, typeof(object)), parameter);
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

        private Guid CreateFinanceSetting(FinanceSetting model)
        {
            _dbContext.FinanceSetting.Add(model);
            _dbContext.SaveChanges();
            return model.Id;
        }

        private void UpdateFinanceSetting(Guid id, FinanceSetting updatedEntity)
        {
            _dbContext.FinanceSetting.Update(updatedEntity);
            _dbContext.SaveChanges();
        }

        private bool DeleteFinanceSetting(Guid id)
        {
            var entityData = _dbContext.FinanceSetting.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            if (entityData == null)
            {
                throw new ApplicationException("No data found!");
            }

            _dbContext.FinanceSetting.Remove(entityData);
            _dbContext.SaveChanges();
            return true;
        }

        private void PatchFinanceSetting(Guid id, JsonPatchDocument<FinanceSetting> updatedEntity)
        {
            if (updatedEntity == null)
            {
                throw new ApplicationException("Patch document is missing!");
            }

            var existingEntity = _dbContext.FinanceSetting.FirstOrDefault(t => t.Id == id);
            if (existingEntity == null)
            {
                throw new ApplicationException("No data found!");
            }

            updatedEntity.ApplyTo(existingEntity);
            _dbContext.FinanceSetting.Update(existingEntity);
            _dbContext.SaveChanges();
        }
        #endregion
    }
}