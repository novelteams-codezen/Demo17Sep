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
    /// The dispenseactivityhistoryService responsible for managing dispenseactivityhistory related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting dispenseactivityhistory information.
    /// </remarks>
    public interface IDispenseActivityHistoryService
    {
        /// <summary>Retrieves a specific dispenseactivityhistory by its primary key</summary>
        /// <param name="id">The primary key of the dispenseactivityhistory</param>
        /// <returns>The dispenseactivityhistory data</returns>
        DispenseActivityHistory GetById(Guid id);

        /// <summary>Retrieves a list of dispenseactivityhistorys based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of dispenseactivityhistorys</returns>
        List<DispenseActivityHistory> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc");

        /// <summary>Adds a new dispenseactivityhistory</summary>
        /// <param name="model">The dispenseactivityhistory data to be added</param>
        /// <returns>The result of the operation</returns>
        Guid Create(DispenseActivityHistory model);

        /// <summary>Updates a specific dispenseactivityhistory by its primary key</summary>
        /// <param name="id">The primary key of the dispenseactivityhistory</param>
        /// <param name="updatedEntity">The dispenseactivityhistory data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Update(Guid id, DispenseActivityHistory updatedEntity);

        /// <summary>Updates a specific dispenseactivityhistory by its primary key</summary>
        /// <param name="id">The primary key of the dispenseactivityhistory</param>
        /// <param name="updatedEntity">The dispenseactivityhistory data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Patch(Guid id, JsonPatchDocument<DispenseActivityHistory> updatedEntity);

        /// <summary>Deletes a specific dispenseactivityhistory by its primary key</summary>
        /// <param name="id">The primary key of the dispenseactivityhistory</param>
        /// <returns>The result of the operation</returns>
        bool Delete(Guid id);
    }

    /// <summary>
    /// The dispenseactivityhistoryService responsible for managing dispenseactivityhistory related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting dispenseactivityhistory information.
    /// </remarks>
    public class DispenseActivityHistoryService : IDispenseActivityHistoryService
    {
        private Demo17SepContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the DispenseActivityHistory class.
        /// </summary>
        /// <param name="dbContext">dbContext value to set.</param>
        public DispenseActivityHistoryService(Demo17SepContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>Retrieves a specific dispenseactivityhistory by its primary key</summary>
        /// <param name="id">The primary key of the dispenseactivityhistory</param>
        /// <returns>The dispenseactivityhistory data</returns>
        public DispenseActivityHistory GetById(Guid id)
        {
            var entityData = _dbContext.DispenseActivityHistory.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            return entityData;
        }

        /// <summary>Retrieves a list of dispenseactivityhistorys based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of dispenseactivityhistorys</returns>/// <exception cref="Exception"></exception>
        public List<DispenseActivityHistory> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            var result = GetDispenseActivityHistory(filters, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return result;
        }

        /// <summary>Adds a new dispenseactivityhistory</summary>
        /// <param name="model">The dispenseactivityhistory data to be added</param>
        /// <returns>The result of the operation</returns>
        public Guid Create(DispenseActivityHistory model)
        {
            model.Id = CreateDispenseActivityHistory(model);
            return model.Id;
        }

        /// <summary>Updates a specific dispenseactivityhistory by its primary key</summary>
        /// <param name="id">The primary key of the dispenseactivityhistory</param>
        /// <param name="updatedEntity">The dispenseactivityhistory data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Update(Guid id, DispenseActivityHistory updatedEntity)
        {
            UpdateDispenseActivityHistory(id, updatedEntity);
            return true;
        }

        /// <summary>Updates a specific dispenseactivityhistory by its primary key</summary>
        /// <param name="id">The primary key of the dispenseactivityhistory</param>
        /// <param name="updatedEntity">The dispenseactivityhistory data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Patch(Guid id, JsonPatchDocument<DispenseActivityHistory> updatedEntity)
        {
            PatchDispenseActivityHistory(id, updatedEntity);
            return true;
        }

        /// <summary>Deletes a specific dispenseactivityhistory by its primary key</summary>
        /// <param name="id">The primary key of the dispenseactivityhistory</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Delete(Guid id)
        {
            DeleteDispenseActivityHistory(id);
            return true;
        }
        #region
        private List<DispenseActivityHistory> GetDispenseActivityHistory(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            if (pageSize < 1)
            {
                throw new ApplicationException("Page size invalid!");
            }

            if (pageNumber < 1)
            {
                throw new ApplicationException("Page mumber invalid!");
            }

            var query = _dbContext.DispenseActivityHistory.IncludeRelated().AsQueryable();
            int skip = (pageNumber - 1) * pageSize;
            var result = FilterService<DispenseActivityHistory>.ApplyFilter(query, filters, searchTerm);
            if (!string.IsNullOrEmpty(sortField))
            {
                var parameter = Expression.Parameter(typeof(DispenseActivityHistory), "b");
                var property = Expression.Property(parameter, sortField);
                var lambda = Expression.Lambda<Func<DispenseActivityHistory, object>>(Expression.Convert(property, typeof(object)), parameter);
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

        private Guid CreateDispenseActivityHistory(DispenseActivityHistory model)
        {
            _dbContext.DispenseActivityHistory.Add(model);
            _dbContext.SaveChanges();
            return model.Id;
        }

        private void UpdateDispenseActivityHistory(Guid id, DispenseActivityHistory updatedEntity)
        {
            _dbContext.DispenseActivityHistory.Update(updatedEntity);
            _dbContext.SaveChanges();
        }

        private bool DeleteDispenseActivityHistory(Guid id)
        {
            var entityData = _dbContext.DispenseActivityHistory.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            if (entityData == null)
            {
                throw new ApplicationException("No data found!");
            }

            _dbContext.DispenseActivityHistory.Remove(entityData);
            _dbContext.SaveChanges();
            return true;
        }

        private void PatchDispenseActivityHistory(Guid id, JsonPatchDocument<DispenseActivityHistory> updatedEntity)
        {
            if (updatedEntity == null)
            {
                throw new ApplicationException("Patch document is missing!");
            }

            var existingEntity = _dbContext.DispenseActivityHistory.FirstOrDefault(t => t.Id == id);
            if (existingEntity == null)
            {
                throw new ApplicationException("No data found!");
            }

            updatedEntity.ApplyTo(existingEntity);
            _dbContext.DispenseActivityHistory.Update(existingEntity);
            _dbContext.SaveChanges();
        }
        #endregion
    }
}