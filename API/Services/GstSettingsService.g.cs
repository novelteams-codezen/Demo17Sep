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
    /// The gstsettingsService responsible for managing gstsettings related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting gstsettings information.
    /// </remarks>
    public interface IGstSettingsService
    {
        /// <summary>Retrieves a specific gstsettings by its primary key</summary>
        /// <param name="id">The primary key of the gstsettings</param>
        /// <returns>The gstsettings data</returns>
        GstSettings GetById(Guid id);

        /// <summary>Retrieves a list of gstsettingss based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of gstsettingss</returns>
        List<GstSettings> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc");

        /// <summary>Adds a new gstsettings</summary>
        /// <param name="model">The gstsettings data to be added</param>
        /// <returns>The result of the operation</returns>
        Guid Create(GstSettings model);

        /// <summary>Updates a specific gstsettings by its primary key</summary>
        /// <param name="id">The primary key of the gstsettings</param>
        /// <param name="updatedEntity">The gstsettings data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Update(Guid id, GstSettings updatedEntity);

        /// <summary>Updates a specific gstsettings by its primary key</summary>
        /// <param name="id">The primary key of the gstsettings</param>
        /// <param name="updatedEntity">The gstsettings data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Patch(Guid id, JsonPatchDocument<GstSettings> updatedEntity);

        /// <summary>Deletes a specific gstsettings by its primary key</summary>
        /// <param name="id">The primary key of the gstsettings</param>
        /// <returns>The result of the operation</returns>
        bool Delete(Guid id);
    }

    /// <summary>
    /// The gstsettingsService responsible for managing gstsettings related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting gstsettings information.
    /// </remarks>
    public class GstSettingsService : IGstSettingsService
    {
        private Demo17SepContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the GstSettings class.
        /// </summary>
        /// <param name="dbContext">dbContext value to set.</param>
        public GstSettingsService(Demo17SepContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>Retrieves a specific gstsettings by its primary key</summary>
        /// <param name="id">The primary key of the gstsettings</param>
        /// <returns>The gstsettings data</returns>
        public GstSettings GetById(Guid id)
        {
            var entityData = _dbContext.GstSettings.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            return entityData;
        }

        /// <summary>Retrieves a list of gstsettingss based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of gstsettingss</returns>/// <exception cref="Exception"></exception>
        public List<GstSettings> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            var result = GetGstSettings(filters, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return result;
        }

        /// <summary>Adds a new gstsettings</summary>
        /// <param name="model">The gstsettings data to be added</param>
        /// <returns>The result of the operation</returns>
        public Guid Create(GstSettings model)
        {
            model.Id = CreateGstSettings(model);
            return model.Id;
        }

        /// <summary>Updates a specific gstsettings by its primary key</summary>
        /// <param name="id">The primary key of the gstsettings</param>
        /// <param name="updatedEntity">The gstsettings data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Update(Guid id, GstSettings updatedEntity)
        {
            UpdateGstSettings(id, updatedEntity);
            return true;
        }

        /// <summary>Updates a specific gstsettings by its primary key</summary>
        /// <param name="id">The primary key of the gstsettings</param>
        /// <param name="updatedEntity">The gstsettings data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Patch(Guid id, JsonPatchDocument<GstSettings> updatedEntity)
        {
            PatchGstSettings(id, updatedEntity);
            return true;
        }

        /// <summary>Deletes a specific gstsettings by its primary key</summary>
        /// <param name="id">The primary key of the gstsettings</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Delete(Guid id)
        {
            DeleteGstSettings(id);
            return true;
        }
        #region
        private List<GstSettings> GetGstSettings(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            if (pageSize < 1)
            {
                throw new ApplicationException("Page size invalid!");
            }

            if (pageNumber < 1)
            {
                throw new ApplicationException("Page mumber invalid!");
            }

            var query = _dbContext.GstSettings.IncludeRelated().AsQueryable();
            int skip = (pageNumber - 1) * pageSize;
            var result = FilterService<GstSettings>.ApplyFilter(query, filters, searchTerm);
            if (!string.IsNullOrEmpty(sortField))
            {
                var parameter = Expression.Parameter(typeof(GstSettings), "b");
                var property = Expression.Property(parameter, sortField);
                var lambda = Expression.Lambda<Func<GstSettings, object>>(Expression.Convert(property, typeof(object)), parameter);
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

        private Guid CreateGstSettings(GstSettings model)
        {
            _dbContext.GstSettings.Add(model);
            _dbContext.SaveChanges();
            return model.Id;
        }

        private void UpdateGstSettings(Guid id, GstSettings updatedEntity)
        {
            _dbContext.GstSettings.Update(updatedEntity);
            _dbContext.SaveChanges();
        }

        private bool DeleteGstSettings(Guid id)
        {
            var entityData = _dbContext.GstSettings.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            if (entityData == null)
            {
                throw new ApplicationException("No data found!");
            }

            _dbContext.GstSettings.Remove(entityData);
            _dbContext.SaveChanges();
            return true;
        }

        private void PatchGstSettings(Guid id, JsonPatchDocument<GstSettings> updatedEntity)
        {
            if (updatedEntity == null)
            {
                throw new ApplicationException("Patch document is missing!");
            }

            var existingEntity = _dbContext.GstSettings.FirstOrDefault(t => t.Id == id);
            if (existingEntity == null)
            {
                throw new ApplicationException("No data found!");
            }

            updatedEntity.ApplyTo(existingEntity);
            _dbContext.GstSettings.Update(existingEntity);
            _dbContext.SaveChanges();
        }
        #endregion
    }
}