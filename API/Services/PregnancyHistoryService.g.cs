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
    /// The pregnancyhistoryService responsible for managing pregnancyhistory related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting pregnancyhistory information.
    /// </remarks>
    public interface IPregnancyHistoryService
    {
        /// <summary>Retrieves a specific pregnancyhistory by its primary key</summary>
        /// <param name="id">The primary key of the pregnancyhistory</param>
        /// <returns>The pregnancyhistory data</returns>
        PregnancyHistory GetById(Guid id);

        /// <summary>Retrieves a list of pregnancyhistorys based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of pregnancyhistorys</returns>
        List<PregnancyHistory> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc");

        /// <summary>Adds a new pregnancyhistory</summary>
        /// <param name="model">The pregnancyhistory data to be added</param>
        /// <returns>The result of the operation</returns>
        Guid Create(PregnancyHistory model);

        /// <summary>Updates a specific pregnancyhistory by its primary key</summary>
        /// <param name="id">The primary key of the pregnancyhistory</param>
        /// <param name="updatedEntity">The pregnancyhistory data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Update(Guid id, PregnancyHistory updatedEntity);

        /// <summary>Updates a specific pregnancyhistory by its primary key</summary>
        /// <param name="id">The primary key of the pregnancyhistory</param>
        /// <param name="updatedEntity">The pregnancyhistory data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Patch(Guid id, JsonPatchDocument<PregnancyHistory> updatedEntity);

        /// <summary>Deletes a specific pregnancyhistory by its primary key</summary>
        /// <param name="id">The primary key of the pregnancyhistory</param>
        /// <returns>The result of the operation</returns>
        bool Delete(Guid id);
    }

    /// <summary>
    /// The pregnancyhistoryService responsible for managing pregnancyhistory related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting pregnancyhistory information.
    /// </remarks>
    public class PregnancyHistoryService : IPregnancyHistoryService
    {
        private Demo17SepContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the PregnancyHistory class.
        /// </summary>
        /// <param name="dbContext">dbContext value to set.</param>
        public PregnancyHistoryService(Demo17SepContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>Retrieves a specific pregnancyhistory by its primary key</summary>
        /// <param name="id">The primary key of the pregnancyhistory</param>
        /// <returns>The pregnancyhistory data</returns>
        public PregnancyHistory GetById(Guid id)
        {
            var entityData = _dbContext.PregnancyHistory.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            return entityData;
        }

        /// <summary>Retrieves a list of pregnancyhistorys based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of pregnancyhistorys</returns>/// <exception cref="Exception"></exception>
        public List<PregnancyHistory> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            var result = GetPregnancyHistory(filters, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return result;
        }

        /// <summary>Adds a new pregnancyhistory</summary>
        /// <param name="model">The pregnancyhistory data to be added</param>
        /// <returns>The result of the operation</returns>
        public Guid Create(PregnancyHistory model)
        {
            model.Id = CreatePregnancyHistory(model);
            return model.Id;
        }

        /// <summary>Updates a specific pregnancyhistory by its primary key</summary>
        /// <param name="id">The primary key of the pregnancyhistory</param>
        /// <param name="updatedEntity">The pregnancyhistory data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Update(Guid id, PregnancyHistory updatedEntity)
        {
            UpdatePregnancyHistory(id, updatedEntity);
            return true;
        }

        /// <summary>Updates a specific pregnancyhistory by its primary key</summary>
        /// <param name="id">The primary key of the pregnancyhistory</param>
        /// <param name="updatedEntity">The pregnancyhistory data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Patch(Guid id, JsonPatchDocument<PregnancyHistory> updatedEntity)
        {
            PatchPregnancyHistory(id, updatedEntity);
            return true;
        }

        /// <summary>Deletes a specific pregnancyhistory by its primary key</summary>
        /// <param name="id">The primary key of the pregnancyhistory</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Delete(Guid id)
        {
            DeletePregnancyHistory(id);
            return true;
        }
        #region
        private List<PregnancyHistory> GetPregnancyHistory(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            if (pageSize < 1)
            {
                throw new ApplicationException("Page size invalid!");
            }

            if (pageNumber < 1)
            {
                throw new ApplicationException("Page mumber invalid!");
            }

            var query = _dbContext.PregnancyHistory.IncludeRelated().AsQueryable();
            int skip = (pageNumber - 1) * pageSize;
            var result = FilterService<PregnancyHistory>.ApplyFilter(query, filters, searchTerm);
            if (!string.IsNullOrEmpty(sortField))
            {
                var parameter = Expression.Parameter(typeof(PregnancyHistory), "b");
                var property = Expression.Property(parameter, sortField);
                var lambda = Expression.Lambda<Func<PregnancyHistory, object>>(Expression.Convert(property, typeof(object)), parameter);
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

        private Guid CreatePregnancyHistory(PregnancyHistory model)
        {
            _dbContext.PregnancyHistory.Add(model);
            _dbContext.SaveChanges();
            return model.Id;
        }

        private void UpdatePregnancyHistory(Guid id, PregnancyHistory updatedEntity)
        {
            _dbContext.PregnancyHistory.Update(updatedEntity);
            _dbContext.SaveChanges();
        }

        private bool DeletePregnancyHistory(Guid id)
        {
            var entityData = _dbContext.PregnancyHistory.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            if (entityData == null)
            {
                throw new ApplicationException("No data found!");
            }

            _dbContext.PregnancyHistory.Remove(entityData);
            _dbContext.SaveChanges();
            return true;
        }

        private void PatchPregnancyHistory(Guid id, JsonPatchDocument<PregnancyHistory> updatedEntity)
        {
            if (updatedEntity == null)
            {
                throw new ApplicationException("Patch document is missing!");
            }

            var existingEntity = _dbContext.PregnancyHistory.FirstOrDefault(t => t.Id == id);
            if (existingEntity == null)
            {
                throw new ApplicationException("No data found!");
            }

            updatedEntity.ApplyTo(existingEntity);
            _dbContext.PregnancyHistory.Update(existingEntity);
            _dbContext.SaveChanges();
        }
        #endregion
    }
}