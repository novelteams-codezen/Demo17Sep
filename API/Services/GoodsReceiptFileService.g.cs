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
    /// The goodsreceiptfileService responsible for managing goodsreceiptfile related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting goodsreceiptfile information.
    /// </remarks>
    public interface IGoodsReceiptFileService
    {
        /// <summary>Retrieves a specific goodsreceiptfile by its primary key</summary>
        /// <param name="id">The primary key of the goodsreceiptfile</param>
        /// <returns>The goodsreceiptfile data</returns>
        GoodsReceiptFile GetById(Guid id);

        /// <summary>Retrieves a list of goodsreceiptfiles based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of goodsreceiptfiles</returns>
        List<GoodsReceiptFile> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc");

        /// <summary>Adds a new goodsreceiptfile</summary>
        /// <param name="model">The goodsreceiptfile data to be added</param>
        /// <returns>The result of the operation</returns>
        Guid Create(GoodsReceiptFile model);

        /// <summary>Updates a specific goodsreceiptfile by its primary key</summary>
        /// <param name="id">The primary key of the goodsreceiptfile</param>
        /// <param name="updatedEntity">The goodsreceiptfile data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Update(Guid id, GoodsReceiptFile updatedEntity);

        /// <summary>Updates a specific goodsreceiptfile by its primary key</summary>
        /// <param name="id">The primary key of the goodsreceiptfile</param>
        /// <param name="updatedEntity">The goodsreceiptfile data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Patch(Guid id, JsonPatchDocument<GoodsReceiptFile> updatedEntity);

        /// <summary>Deletes a specific goodsreceiptfile by its primary key</summary>
        /// <param name="id">The primary key of the goodsreceiptfile</param>
        /// <returns>The result of the operation</returns>
        bool Delete(Guid id);
    }

    /// <summary>
    /// The goodsreceiptfileService responsible for managing goodsreceiptfile related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting goodsreceiptfile information.
    /// </remarks>
    public class GoodsReceiptFileService : IGoodsReceiptFileService
    {
        private Demo17SepContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the GoodsReceiptFile class.
        /// </summary>
        /// <param name="dbContext">dbContext value to set.</param>
        public GoodsReceiptFileService(Demo17SepContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>Retrieves a specific goodsreceiptfile by its primary key</summary>
        /// <param name="id">The primary key of the goodsreceiptfile</param>
        /// <returns>The goodsreceiptfile data</returns>
        public GoodsReceiptFile GetById(Guid id)
        {
            var entityData = _dbContext.GoodsReceiptFile.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            return entityData;
        }

        /// <summary>Retrieves a list of goodsreceiptfiles based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of goodsreceiptfiles</returns>/// <exception cref="Exception"></exception>
        public List<GoodsReceiptFile> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            var result = GetGoodsReceiptFile(filters, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return result;
        }

        /// <summary>Adds a new goodsreceiptfile</summary>
        /// <param name="model">The goodsreceiptfile data to be added</param>
        /// <returns>The result of the operation</returns>
        public Guid Create(GoodsReceiptFile model)
        {
            model.Id = CreateGoodsReceiptFile(model);
            return model.Id;
        }

        /// <summary>Updates a specific goodsreceiptfile by its primary key</summary>
        /// <param name="id">The primary key of the goodsreceiptfile</param>
        /// <param name="updatedEntity">The goodsreceiptfile data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Update(Guid id, GoodsReceiptFile updatedEntity)
        {
            UpdateGoodsReceiptFile(id, updatedEntity);
            return true;
        }

        /// <summary>Updates a specific goodsreceiptfile by its primary key</summary>
        /// <param name="id">The primary key of the goodsreceiptfile</param>
        /// <param name="updatedEntity">The goodsreceiptfile data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Patch(Guid id, JsonPatchDocument<GoodsReceiptFile> updatedEntity)
        {
            PatchGoodsReceiptFile(id, updatedEntity);
            return true;
        }

        /// <summary>Deletes a specific goodsreceiptfile by its primary key</summary>
        /// <param name="id">The primary key of the goodsreceiptfile</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Delete(Guid id)
        {
            DeleteGoodsReceiptFile(id);
            return true;
        }
        #region
        private List<GoodsReceiptFile> GetGoodsReceiptFile(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            if (pageSize < 1)
            {
                throw new ApplicationException("Page size invalid!");
            }

            if (pageNumber < 1)
            {
                throw new ApplicationException("Page mumber invalid!");
            }

            var query = _dbContext.GoodsReceiptFile.IncludeRelated().AsQueryable();
            int skip = (pageNumber - 1) * pageSize;
            var result = FilterService<GoodsReceiptFile>.ApplyFilter(query, filters, searchTerm);
            if (!string.IsNullOrEmpty(sortField))
            {
                var parameter = Expression.Parameter(typeof(GoodsReceiptFile), "b");
                var property = Expression.Property(parameter, sortField);
                var lambda = Expression.Lambda<Func<GoodsReceiptFile, object>>(Expression.Convert(property, typeof(object)), parameter);
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

        private Guid CreateGoodsReceiptFile(GoodsReceiptFile model)
        {
            _dbContext.GoodsReceiptFile.Add(model);
            _dbContext.SaveChanges();
            return model.Id;
        }

        private void UpdateGoodsReceiptFile(Guid id, GoodsReceiptFile updatedEntity)
        {
            _dbContext.GoodsReceiptFile.Update(updatedEntity);
            _dbContext.SaveChanges();
        }

        private bool DeleteGoodsReceiptFile(Guid id)
        {
            var entityData = _dbContext.GoodsReceiptFile.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            if (entityData == null)
            {
                throw new ApplicationException("No data found!");
            }

            _dbContext.GoodsReceiptFile.Remove(entityData);
            _dbContext.SaveChanges();
            return true;
        }

        private void PatchGoodsReceiptFile(Guid id, JsonPatchDocument<GoodsReceiptFile> updatedEntity)
        {
            if (updatedEntity == null)
            {
                throw new ApplicationException("Patch document is missing!");
            }

            var existingEntity = _dbContext.GoodsReceiptFile.FirstOrDefault(t => t.Id == id);
            if (existingEntity == null)
            {
                throw new ApplicationException("No data found!");
            }

            updatedEntity.ApplyTo(existingEntity);
            _dbContext.GoodsReceiptFile.Update(existingEntity);
            _dbContext.SaveChanges();
        }
        #endregion
    }
}