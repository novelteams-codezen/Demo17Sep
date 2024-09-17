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
    /// The dispenseitemdosageService responsible for managing dispenseitemdosage related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting dispenseitemdosage information.
    /// </remarks>
    public interface IDispenseItemDosageService
    {
        /// <summary>Retrieves a specific dispenseitemdosage by its primary key</summary>
        /// <param name="id">The primary key of the dispenseitemdosage</param>
        /// <returns>The dispenseitemdosage data</returns>
        DispenseItemDosage GetById(Guid id);

        /// <summary>Retrieves a list of dispenseitemdosages based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of dispenseitemdosages</returns>
        List<DispenseItemDosage> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc");

        /// <summary>Adds a new dispenseitemdosage</summary>
        /// <param name="model">The dispenseitemdosage data to be added</param>
        /// <returns>The result of the operation</returns>
        Guid Create(DispenseItemDosage model);

        /// <summary>Updates a specific dispenseitemdosage by its primary key</summary>
        /// <param name="id">The primary key of the dispenseitemdosage</param>
        /// <param name="updatedEntity">The dispenseitemdosage data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Update(Guid id, DispenseItemDosage updatedEntity);

        /// <summary>Updates a specific dispenseitemdosage by its primary key</summary>
        /// <param name="id">The primary key of the dispenseitemdosage</param>
        /// <param name="updatedEntity">The dispenseitemdosage data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Patch(Guid id, JsonPatchDocument<DispenseItemDosage> updatedEntity);

        /// <summary>Deletes a specific dispenseitemdosage by its primary key</summary>
        /// <param name="id">The primary key of the dispenseitemdosage</param>
        /// <returns>The result of the operation</returns>
        bool Delete(Guid id);
    }

    /// <summary>
    /// The dispenseitemdosageService responsible for managing dispenseitemdosage related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting dispenseitemdosage information.
    /// </remarks>
    public class DispenseItemDosageService : IDispenseItemDosageService
    {
        private Demo17SepContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the DispenseItemDosage class.
        /// </summary>
        /// <param name="dbContext">dbContext value to set.</param>
        public DispenseItemDosageService(Demo17SepContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>Retrieves a specific dispenseitemdosage by its primary key</summary>
        /// <param name="id">The primary key of the dispenseitemdosage</param>
        /// <returns>The dispenseitemdosage data</returns>
        public DispenseItemDosage GetById(Guid id)
        {
            var entityData = _dbContext.DispenseItemDosage.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            return entityData;
        }

        /// <summary>Retrieves a list of dispenseitemdosages based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of dispenseitemdosages</returns>/// <exception cref="Exception"></exception>
        public List<DispenseItemDosage> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            var result = GetDispenseItemDosage(filters, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return result;
        }

        /// <summary>Adds a new dispenseitemdosage</summary>
        /// <param name="model">The dispenseitemdosage data to be added</param>
        /// <returns>The result of the operation</returns>
        public Guid Create(DispenseItemDosage model)
        {
            model.Id = CreateDispenseItemDosage(model);
            return model.Id;
        }

        /// <summary>Updates a specific dispenseitemdosage by its primary key</summary>
        /// <param name="id">The primary key of the dispenseitemdosage</param>
        /// <param name="updatedEntity">The dispenseitemdosage data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Update(Guid id, DispenseItemDosage updatedEntity)
        {
            UpdateDispenseItemDosage(id, updatedEntity);
            return true;
        }

        /// <summary>Updates a specific dispenseitemdosage by its primary key</summary>
        /// <param name="id">The primary key of the dispenseitemdosage</param>
        /// <param name="updatedEntity">The dispenseitemdosage data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Patch(Guid id, JsonPatchDocument<DispenseItemDosage> updatedEntity)
        {
            PatchDispenseItemDosage(id, updatedEntity);
            return true;
        }

        /// <summary>Deletes a specific dispenseitemdosage by its primary key</summary>
        /// <param name="id">The primary key of the dispenseitemdosage</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Delete(Guid id)
        {
            DeleteDispenseItemDosage(id);
            return true;
        }
        #region
        private List<DispenseItemDosage> GetDispenseItemDosage(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            if (pageSize < 1)
            {
                throw new ApplicationException("Page size invalid!");
            }

            if (pageNumber < 1)
            {
                throw new ApplicationException("Page mumber invalid!");
            }

            var query = _dbContext.DispenseItemDosage.IncludeRelated().AsQueryable();
            int skip = (pageNumber - 1) * pageSize;
            var result = FilterService<DispenseItemDosage>.ApplyFilter(query, filters, searchTerm);
            if (!string.IsNullOrEmpty(sortField))
            {
                var parameter = Expression.Parameter(typeof(DispenseItemDosage), "b");
                var property = Expression.Property(parameter, sortField);
                var lambda = Expression.Lambda<Func<DispenseItemDosage, object>>(Expression.Convert(property, typeof(object)), parameter);
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

        private Guid CreateDispenseItemDosage(DispenseItemDosage model)
        {
            _dbContext.DispenseItemDosage.Add(model);
            _dbContext.SaveChanges();
            return model.Id;
        }

        private void UpdateDispenseItemDosage(Guid id, DispenseItemDosage updatedEntity)
        {
            _dbContext.DispenseItemDosage.Update(updatedEntity);
            _dbContext.SaveChanges();
        }

        private bool DeleteDispenseItemDosage(Guid id)
        {
            var entityData = _dbContext.DispenseItemDosage.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            if (entityData == null)
            {
                throw new ApplicationException("No data found!");
            }

            _dbContext.DispenseItemDosage.Remove(entityData);
            _dbContext.SaveChanges();
            return true;
        }

        private void PatchDispenseItemDosage(Guid id, JsonPatchDocument<DispenseItemDosage> updatedEntity)
        {
            if (updatedEntity == null)
            {
                throw new ApplicationException("Patch document is missing!");
            }

            var existingEntity = _dbContext.DispenseItemDosage.FirstOrDefault(t => t.Id == id);
            if (existingEntity == null)
            {
                throw new ApplicationException("No data found!");
            }

            updatedEntity.ApplyTo(existingEntity);
            _dbContext.DispenseItemDosage.Update(existingEntity);
            _dbContext.SaveChanges();
        }
        #endregion
    }
}