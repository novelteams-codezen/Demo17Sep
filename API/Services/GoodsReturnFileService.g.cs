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
    /// The goodsreturnfileService responsible for managing goodsreturnfile related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting goodsreturnfile information.
    /// </remarks>
    public interface IGoodsReturnFileService
    {
        /// <summary>Retrieves a specific goodsreturnfile by its primary key</summary>
        /// <param name="id">The primary key of the goodsreturnfile</param>
        /// <returns>The goodsreturnfile data</returns>
        GoodsReturnFile GetById(Guid id);

        /// <summary>Retrieves a list of goodsreturnfiles based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of goodsreturnfiles</returns>
        List<GoodsReturnFile> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc");

        /// <summary>Adds a new goodsreturnfile</summary>
        /// <param name="model">The goodsreturnfile data to be added</param>
        /// <returns>The result of the operation</returns>
        Guid Create(GoodsReturnFile model);

        /// <summary>Updates a specific goodsreturnfile by its primary key</summary>
        /// <param name="id">The primary key of the goodsreturnfile</param>
        /// <param name="updatedEntity">The goodsreturnfile data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Update(Guid id, GoodsReturnFile updatedEntity);

        /// <summary>Updates a specific goodsreturnfile by its primary key</summary>
        /// <param name="id">The primary key of the goodsreturnfile</param>
        /// <param name="updatedEntity">The goodsreturnfile data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Patch(Guid id, JsonPatchDocument<GoodsReturnFile> updatedEntity);

        /// <summary>Deletes a specific goodsreturnfile by its primary key</summary>
        /// <param name="id">The primary key of the goodsreturnfile</param>
        /// <returns>The result of the operation</returns>
        bool Delete(Guid id);
    }

    /// <summary>
    /// The goodsreturnfileService responsible for managing goodsreturnfile related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting goodsreturnfile information.
    /// </remarks>
    public class GoodsReturnFileService : IGoodsReturnFileService
    {
        private Demo17SepContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the GoodsReturnFile class.
        /// </summary>
        /// <param name="dbContext">dbContext value to set.</param>
        public GoodsReturnFileService(Demo17SepContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>Retrieves a specific goodsreturnfile by its primary key</summary>
        /// <param name="id">The primary key of the goodsreturnfile</param>
        /// <returns>The goodsreturnfile data</returns>
        public GoodsReturnFile GetById(Guid id)
        {
            var entityData = _dbContext.GoodsReturnFile.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            return entityData;
        }

        /// <summary>Retrieves a list of goodsreturnfiles based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of goodsreturnfiles</returns>/// <exception cref="Exception"></exception>
        public List<GoodsReturnFile> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            var result = GetGoodsReturnFile(filters, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return result;
        }

        /// <summary>Adds a new goodsreturnfile</summary>
        /// <param name="model">The goodsreturnfile data to be added</param>
        /// <returns>The result of the operation</returns>
        public Guid Create(GoodsReturnFile model)
        {
            model.Id = CreateGoodsReturnFile(model);
            return model.Id;
        }

        /// <summary>Updates a specific goodsreturnfile by its primary key</summary>
        /// <param name="id">The primary key of the goodsreturnfile</param>
        /// <param name="updatedEntity">The goodsreturnfile data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Update(Guid id, GoodsReturnFile updatedEntity)
        {
            UpdateGoodsReturnFile(id, updatedEntity);
            return true;
        }

        /// <summary>Updates a specific goodsreturnfile by its primary key</summary>
        /// <param name="id">The primary key of the goodsreturnfile</param>
        /// <param name="updatedEntity">The goodsreturnfile data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Patch(Guid id, JsonPatchDocument<GoodsReturnFile> updatedEntity)
        {
            PatchGoodsReturnFile(id, updatedEntity);
            return true;
        }

        /// <summary>Deletes a specific goodsreturnfile by its primary key</summary>
        /// <param name="id">The primary key of the goodsreturnfile</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Delete(Guid id)
        {
            DeleteGoodsReturnFile(id);
            return true;
        }
        #region
        private List<GoodsReturnFile> GetGoodsReturnFile(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            if (pageSize < 1)
            {
                throw new ApplicationException("Page size invalid!");
            }

            if (pageNumber < 1)
            {
                throw new ApplicationException("Page mumber invalid!");
            }

            var query = _dbContext.GoodsReturnFile.IncludeRelated().AsQueryable();
            int skip = (pageNumber - 1) * pageSize;
            var result = FilterService<GoodsReturnFile>.ApplyFilter(query, filters, searchTerm);
            if (!string.IsNullOrEmpty(sortField))
            {
                var parameter = Expression.Parameter(typeof(GoodsReturnFile), "b");
                var property = Expression.Property(parameter, sortField);
                var lambda = Expression.Lambda<Func<GoodsReturnFile, object>>(Expression.Convert(property, typeof(object)), parameter);
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

        private Guid CreateGoodsReturnFile(GoodsReturnFile model)
        {
            _dbContext.GoodsReturnFile.Add(model);
            _dbContext.SaveChanges();
            return model.Id;
        }

        private void UpdateGoodsReturnFile(Guid id, GoodsReturnFile updatedEntity)
        {
            _dbContext.GoodsReturnFile.Update(updatedEntity);
            _dbContext.SaveChanges();
        }

        private bool DeleteGoodsReturnFile(Guid id)
        {
            var entityData = _dbContext.GoodsReturnFile.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            if (entityData == null)
            {
                throw new ApplicationException("No data found!");
            }

            _dbContext.GoodsReturnFile.Remove(entityData);
            _dbContext.SaveChanges();
            return true;
        }

        private void PatchGoodsReturnFile(Guid id, JsonPatchDocument<GoodsReturnFile> updatedEntity)
        {
            if (updatedEntity == null)
            {
                throw new ApplicationException("Patch document is missing!");
            }

            var existingEntity = _dbContext.GoodsReturnFile.FirstOrDefault(t => t.Id == id);
            if (existingEntity == null)
            {
                throw new ApplicationException("No data found!");
            }

            updatedEntity.ApplyTo(existingEntity);
            _dbContext.GoodsReturnFile.Update(existingEntity);
            _dbContext.SaveChanges();
        }
        #endregion
    }
}