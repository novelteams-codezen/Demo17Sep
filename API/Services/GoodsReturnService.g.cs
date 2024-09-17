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
    /// The goodsreturnService responsible for managing goodsreturn related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting goodsreturn information.
    /// </remarks>
    public interface IGoodsReturnService
    {
        /// <summary>Retrieves a specific goodsreturn by its primary key</summary>
        /// <param name="id">The primary key of the goodsreturn</param>
        /// <returns>The goodsreturn data</returns>
        GoodsReturn GetById(Guid id);

        /// <summary>Retrieves a list of goodsreturns based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of goodsreturns</returns>
        List<GoodsReturn> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc");

        /// <summary>Adds a new goodsreturn</summary>
        /// <param name="model">The goodsreturn data to be added</param>
        /// <returns>The result of the operation</returns>
        Guid Create(GoodsReturn model);

        /// <summary>Updates a specific goodsreturn by its primary key</summary>
        /// <param name="id">The primary key of the goodsreturn</param>
        /// <param name="updatedEntity">The goodsreturn data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Update(Guid id, GoodsReturn updatedEntity);

        /// <summary>Updates a specific goodsreturn by its primary key</summary>
        /// <param name="id">The primary key of the goodsreturn</param>
        /// <param name="updatedEntity">The goodsreturn data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Patch(Guid id, JsonPatchDocument<GoodsReturn> updatedEntity);

        /// <summary>Deletes a specific goodsreturn by its primary key</summary>
        /// <param name="id">The primary key of the goodsreturn</param>
        /// <returns>The result of the operation</returns>
        bool Delete(Guid id);
    }

    /// <summary>
    /// The goodsreturnService responsible for managing goodsreturn related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting goodsreturn information.
    /// </remarks>
    public class GoodsReturnService : IGoodsReturnService
    {
        private Demo17SepContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the GoodsReturn class.
        /// </summary>
        /// <param name="dbContext">dbContext value to set.</param>
        public GoodsReturnService(Demo17SepContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>Retrieves a specific goodsreturn by its primary key</summary>
        /// <param name="id">The primary key of the goodsreturn</param>
        /// <returns>The goodsreturn data</returns>
        public GoodsReturn GetById(Guid id)
        {
            var entityData = _dbContext.GoodsReturn.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            return entityData;
        }

        /// <summary>Retrieves a list of goodsreturns based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of goodsreturns</returns>/// <exception cref="Exception"></exception>
        public List<GoodsReturn> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            var result = GetGoodsReturn(filters, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return result;
        }

        /// <summary>Adds a new goodsreturn</summary>
        /// <param name="model">The goodsreturn data to be added</param>
        /// <returns>The result of the operation</returns>
        public Guid Create(GoodsReturn model)
        {
            model.Id = CreateGoodsReturn(model);
            return model.Id;
        }

        /// <summary>Updates a specific goodsreturn by its primary key</summary>
        /// <param name="id">The primary key of the goodsreturn</param>
        /// <param name="updatedEntity">The goodsreturn data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Update(Guid id, GoodsReturn updatedEntity)
        {
            UpdateGoodsReturn(id, updatedEntity);
            return true;
        }

        /// <summary>Updates a specific goodsreturn by its primary key</summary>
        /// <param name="id">The primary key of the goodsreturn</param>
        /// <param name="updatedEntity">The goodsreturn data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Patch(Guid id, JsonPatchDocument<GoodsReturn> updatedEntity)
        {
            PatchGoodsReturn(id, updatedEntity);
            return true;
        }

        /// <summary>Deletes a specific goodsreturn by its primary key</summary>
        /// <param name="id">The primary key of the goodsreturn</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Delete(Guid id)
        {
            DeleteGoodsReturn(id);
            return true;
        }
        #region
        private List<GoodsReturn> GetGoodsReturn(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            if (pageSize < 1)
            {
                throw new ApplicationException("Page size invalid!");
            }

            if (pageNumber < 1)
            {
                throw new ApplicationException("Page mumber invalid!");
            }

            var query = _dbContext.GoodsReturn.IncludeRelated().AsQueryable();
            int skip = (pageNumber - 1) * pageSize;
            var result = FilterService<GoodsReturn>.ApplyFilter(query, filters, searchTerm);
            if (!string.IsNullOrEmpty(sortField))
            {
                var parameter = Expression.Parameter(typeof(GoodsReturn), "b");
                var property = Expression.Property(parameter, sortField);
                var lambda = Expression.Lambda<Func<GoodsReturn, object>>(Expression.Convert(property, typeof(object)), parameter);
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

        private Guid CreateGoodsReturn(GoodsReturn model)
        {
            _dbContext.GoodsReturn.Add(model);
            _dbContext.SaveChanges();
            return model.Id;
        }

        private void UpdateGoodsReturn(Guid id, GoodsReturn updatedEntity)
        {
            _dbContext.GoodsReturn.Update(updatedEntity);
            _dbContext.SaveChanges();
        }

        private bool DeleteGoodsReturn(Guid id)
        {
            var entityData = _dbContext.GoodsReturn.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            if (entityData == null)
            {
                throw new ApplicationException("No data found!");
            }

            _dbContext.GoodsReturn.Remove(entityData);
            _dbContext.SaveChanges();
            return true;
        }

        private void PatchGoodsReturn(Guid id, JsonPatchDocument<GoodsReturn> updatedEntity)
        {
            if (updatedEntity == null)
            {
                throw new ApplicationException("Patch document is missing!");
            }

            var existingEntity = _dbContext.GoodsReturn.FirstOrDefault(t => t.Id == id);
            if (existingEntity == null)
            {
                throw new ApplicationException("No data found!");
            }

            updatedEntity.ApplyTo(existingEntity);
            _dbContext.GoodsReturn.Update(existingEntity);
            _dbContext.SaveChanges();
        }
        #endregion
    }
}