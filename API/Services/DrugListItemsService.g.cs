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
    /// The druglistitemsService responsible for managing druglistitems related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting druglistitems information.
    /// </remarks>
    public interface IDrugListItemsService
    {
        /// <summary>Retrieves a specific druglistitems by its primary key</summary>
        /// <param name="id">The primary key of the druglistitems</param>
        /// <returns>The druglistitems data</returns>
        DrugListItems GetById(Guid id);

        /// <summary>Retrieves a list of druglistitemss based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of druglistitemss</returns>
        List<DrugListItems> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc");

        /// <summary>Adds a new druglistitems</summary>
        /// <param name="model">The druglistitems data to be added</param>
        /// <returns>The result of the operation</returns>
        Guid Create(DrugListItems model);

        /// <summary>Updates a specific druglistitems by its primary key</summary>
        /// <param name="id">The primary key of the druglistitems</param>
        /// <param name="updatedEntity">The druglistitems data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Update(Guid id, DrugListItems updatedEntity);

        /// <summary>Updates a specific druglistitems by its primary key</summary>
        /// <param name="id">The primary key of the druglistitems</param>
        /// <param name="updatedEntity">The druglistitems data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Patch(Guid id, JsonPatchDocument<DrugListItems> updatedEntity);

        /// <summary>Deletes a specific druglistitems by its primary key</summary>
        /// <param name="id">The primary key of the druglistitems</param>
        /// <returns>The result of the operation</returns>
        bool Delete(Guid id);
    }

    /// <summary>
    /// The druglistitemsService responsible for managing druglistitems related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting druglistitems information.
    /// </remarks>
    public class DrugListItemsService : IDrugListItemsService
    {
        private Demo17SepContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the DrugListItems class.
        /// </summary>
        /// <param name="dbContext">dbContext value to set.</param>
        public DrugListItemsService(Demo17SepContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>Retrieves a specific druglistitems by its primary key</summary>
        /// <param name="id">The primary key of the druglistitems</param>
        /// <returns>The druglistitems data</returns>
        public DrugListItems GetById(Guid id)
        {
            var entityData = _dbContext.DrugListItems.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            return entityData;
        }

        /// <summary>Retrieves a list of druglistitemss based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of druglistitemss</returns>/// <exception cref="Exception"></exception>
        public List<DrugListItems> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            var result = GetDrugListItems(filters, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return result;
        }

        /// <summary>Adds a new druglistitems</summary>
        /// <param name="model">The druglistitems data to be added</param>
        /// <returns>The result of the operation</returns>
        public Guid Create(DrugListItems model)
        {
            model.Id = CreateDrugListItems(model);
            return model.Id;
        }

        /// <summary>Updates a specific druglistitems by its primary key</summary>
        /// <param name="id">The primary key of the druglistitems</param>
        /// <param name="updatedEntity">The druglistitems data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Update(Guid id, DrugListItems updatedEntity)
        {
            UpdateDrugListItems(id, updatedEntity);
            return true;
        }

        /// <summary>Updates a specific druglistitems by its primary key</summary>
        /// <param name="id">The primary key of the druglistitems</param>
        /// <param name="updatedEntity">The druglistitems data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Patch(Guid id, JsonPatchDocument<DrugListItems> updatedEntity)
        {
            PatchDrugListItems(id, updatedEntity);
            return true;
        }

        /// <summary>Deletes a specific druglistitems by its primary key</summary>
        /// <param name="id">The primary key of the druglistitems</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Delete(Guid id)
        {
            DeleteDrugListItems(id);
            return true;
        }
        #region
        private List<DrugListItems> GetDrugListItems(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            if (pageSize < 1)
            {
                throw new ApplicationException("Page size invalid!");
            }

            if (pageNumber < 1)
            {
                throw new ApplicationException("Page mumber invalid!");
            }

            var query = _dbContext.DrugListItems.IncludeRelated().AsQueryable();
            int skip = (pageNumber - 1) * pageSize;
            var result = FilterService<DrugListItems>.ApplyFilter(query, filters, searchTerm);
            if (!string.IsNullOrEmpty(sortField))
            {
                var parameter = Expression.Parameter(typeof(DrugListItems), "b");
                var property = Expression.Property(parameter, sortField);
                var lambda = Expression.Lambda<Func<DrugListItems, object>>(Expression.Convert(property, typeof(object)), parameter);
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

        private Guid CreateDrugListItems(DrugListItems model)
        {
            _dbContext.DrugListItems.Add(model);
            _dbContext.SaveChanges();
            return model.Id;
        }

        private void UpdateDrugListItems(Guid id, DrugListItems updatedEntity)
        {
            _dbContext.DrugListItems.Update(updatedEntity);
            _dbContext.SaveChanges();
        }

        private bool DeleteDrugListItems(Guid id)
        {
            var entityData = _dbContext.DrugListItems.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            if (entityData == null)
            {
                throw new ApplicationException("No data found!");
            }

            _dbContext.DrugListItems.Remove(entityData);
            _dbContext.SaveChanges();
            return true;
        }

        private void PatchDrugListItems(Guid id, JsonPatchDocument<DrugListItems> updatedEntity)
        {
            if (updatedEntity == null)
            {
                throw new ApplicationException("Patch document is missing!");
            }

            var existingEntity = _dbContext.DrugListItems.FirstOrDefault(t => t.Id == id);
            if (existingEntity == null)
            {
                throw new ApplicationException("No data found!");
            }

            updatedEntity.ApplyTo(existingEntity);
            _dbContext.DrugListItems.Update(existingEntity);
            _dbContext.SaveChanges();
        }
        #endregion
    }
}