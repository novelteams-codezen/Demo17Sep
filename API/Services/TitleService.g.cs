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
    /// The titleService responsible for managing title related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting title information.
    /// </remarks>
    public interface ITitleService
    {
        /// <summary>Retrieves a specific title by its primary key</summary>
        /// <param name="id">The primary key of the title</param>
        /// <returns>The title data</returns>
        Title GetById(Guid id);

        /// <summary>Retrieves a list of titles based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of titles</returns>
        List<Title> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc");

        /// <summary>Adds a new title</summary>
        /// <param name="model">The title data to be added</param>
        /// <returns>The result of the operation</returns>
        Guid Create(Title model);

        /// <summary>Updates a specific title by its primary key</summary>
        /// <param name="id">The primary key of the title</param>
        /// <param name="updatedEntity">The title data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Update(Guid id, Title updatedEntity);

        /// <summary>Updates a specific title by its primary key</summary>
        /// <param name="id">The primary key of the title</param>
        /// <param name="updatedEntity">The title data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Patch(Guid id, JsonPatchDocument<Title> updatedEntity);

        /// <summary>Deletes a specific title by its primary key</summary>
        /// <param name="id">The primary key of the title</param>
        /// <returns>The result of the operation</returns>
        bool Delete(Guid id);
    }

    /// <summary>
    /// The titleService responsible for managing title related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting title information.
    /// </remarks>
    public class TitleService : ITitleService
    {
        private Demo17SepContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the Title class.
        /// </summary>
        /// <param name="dbContext">dbContext value to set.</param>
        public TitleService(Demo17SepContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>Retrieves a specific title by its primary key</summary>
        /// <param name="id">The primary key of the title</param>
        /// <returns>The title data</returns>
        public Title GetById(Guid id)
        {
            var entityData = _dbContext.Title.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            return entityData;
        }

        /// <summary>Retrieves a list of titles based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of titles</returns>/// <exception cref="Exception"></exception>
        public List<Title> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            var result = GetTitle(filters, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return result;
        }

        /// <summary>Adds a new title</summary>
        /// <param name="model">The title data to be added</param>
        /// <returns>The result of the operation</returns>
        public Guid Create(Title model)
        {
            model.Id = CreateTitle(model);
            return model.Id;
        }

        /// <summary>Updates a specific title by its primary key</summary>
        /// <param name="id">The primary key of the title</param>
        /// <param name="updatedEntity">The title data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Update(Guid id, Title updatedEntity)
        {
            UpdateTitle(id, updatedEntity);
            return true;
        }

        /// <summary>Updates a specific title by its primary key</summary>
        /// <param name="id">The primary key of the title</param>
        /// <param name="updatedEntity">The title data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Patch(Guid id, JsonPatchDocument<Title> updatedEntity)
        {
            PatchTitle(id, updatedEntity);
            return true;
        }

        /// <summary>Deletes a specific title by its primary key</summary>
        /// <param name="id">The primary key of the title</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Delete(Guid id)
        {
            DeleteTitle(id);
            return true;
        }
        #region
        private List<Title> GetTitle(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            if (pageSize < 1)
            {
                throw new ApplicationException("Page size invalid!");
            }

            if (pageNumber < 1)
            {
                throw new ApplicationException("Page mumber invalid!");
            }

            var query = _dbContext.Title.IncludeRelated().AsQueryable();
            int skip = (pageNumber - 1) * pageSize;
            var result = FilterService<Title>.ApplyFilter(query, filters, searchTerm);
            if (!string.IsNullOrEmpty(sortField))
            {
                var parameter = Expression.Parameter(typeof(Title), "b");
                var property = Expression.Property(parameter, sortField);
                var lambda = Expression.Lambda<Func<Title, object>>(Expression.Convert(property, typeof(object)), parameter);
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

        private Guid CreateTitle(Title model)
        {
            _dbContext.Title.Add(model);
            _dbContext.SaveChanges();
            return model.Id;
        }

        private void UpdateTitle(Guid id, Title updatedEntity)
        {
            _dbContext.Title.Update(updatedEntity);
            _dbContext.SaveChanges();
        }

        private bool DeleteTitle(Guid id)
        {
            var entityData = _dbContext.Title.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            if (entityData == null)
            {
                throw new ApplicationException("No data found!");
            }

            _dbContext.Title.Remove(entityData);
            _dbContext.SaveChanges();
            return true;
        }

        private void PatchTitle(Guid id, JsonPatchDocument<Title> updatedEntity)
        {
            if (updatedEntity == null)
            {
                throw new ApplicationException("Patch document is missing!");
            }

            var existingEntity = _dbContext.Title.FirstOrDefault(t => t.Id == id);
            if (existingEntity == null)
            {
                throw new ApplicationException("No data found!");
            }

            updatedEntity.ApplyTo(existingEntity);
            _dbContext.Title.Update(existingEntity);
            _dbContext.SaveChanges();
        }
        #endregion
    }
}