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
    /// The uomService responsible for managing uom related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting uom information.
    /// </remarks>
    public interface IUomService
    {
        /// <summary>Retrieves a specific uom by its primary key</summary>
        /// <param name="id">The primary key of the uom</param>
        /// <returns>The uom data</returns>
        Uom GetById(Guid id);

        /// <summary>Retrieves a list of uoms based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of uoms</returns>
        List<Uom> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc");

        /// <summary>Adds a new uom</summary>
        /// <param name="model">The uom data to be added</param>
        /// <returns>The result of the operation</returns>
        Guid Create(Uom model);

        /// <summary>Updates a specific uom by its primary key</summary>
        /// <param name="id">The primary key of the uom</param>
        /// <param name="updatedEntity">The uom data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Update(Guid id, Uom updatedEntity);

        /// <summary>Updates a specific uom by its primary key</summary>
        /// <param name="id">The primary key of the uom</param>
        /// <param name="updatedEntity">The uom data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Patch(Guid id, JsonPatchDocument<Uom> updatedEntity);

        /// <summary>Deletes a specific uom by its primary key</summary>
        /// <param name="id">The primary key of the uom</param>
        /// <returns>The result of the operation</returns>
        bool Delete(Guid id);
    }

    /// <summary>
    /// The uomService responsible for managing uom related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting uom information.
    /// </remarks>
    public class UomService : IUomService
    {
        private Demo17SepContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the Uom class.
        /// </summary>
        /// <param name="dbContext">dbContext value to set.</param>
        public UomService(Demo17SepContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>Retrieves a specific uom by its primary key</summary>
        /// <param name="id">The primary key of the uom</param>
        /// <returns>The uom data</returns>
        public Uom GetById(Guid id)
        {
            var entityData = _dbContext.Uom.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            return entityData;
        }

        /// <summary>Retrieves a list of uoms based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of uoms</returns>/// <exception cref="Exception"></exception>
        public List<Uom> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            var result = GetUom(filters, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return result;
        }

        /// <summary>Adds a new uom</summary>
        /// <param name="model">The uom data to be added</param>
        /// <returns>The result of the operation</returns>
        public Guid Create(Uom model)
        {
            model.Id = CreateUom(model);
            return model.Id;
        }

        /// <summary>Updates a specific uom by its primary key</summary>
        /// <param name="id">The primary key of the uom</param>
        /// <param name="updatedEntity">The uom data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Update(Guid id, Uom updatedEntity)
        {
            UpdateUom(id, updatedEntity);
            return true;
        }

        /// <summary>Updates a specific uom by its primary key</summary>
        /// <param name="id">The primary key of the uom</param>
        /// <param name="updatedEntity">The uom data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Patch(Guid id, JsonPatchDocument<Uom> updatedEntity)
        {
            PatchUom(id, updatedEntity);
            return true;
        }

        /// <summary>Deletes a specific uom by its primary key</summary>
        /// <param name="id">The primary key of the uom</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Delete(Guid id)
        {
            DeleteUom(id);
            return true;
        }
        #region
        private List<Uom> GetUom(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            if (pageSize < 1)
            {
                throw new ApplicationException("Page size invalid!");
            }

            if (pageNumber < 1)
            {
                throw new ApplicationException("Page mumber invalid!");
            }

            var query = _dbContext.Uom.IncludeRelated().AsQueryable();
            int skip = (pageNumber - 1) * pageSize;
            var result = FilterService<Uom>.ApplyFilter(query, filters, searchTerm);
            if (!string.IsNullOrEmpty(sortField))
            {
                var parameter = Expression.Parameter(typeof(Uom), "b");
                var property = Expression.Property(parameter, sortField);
                var lambda = Expression.Lambda<Func<Uom, object>>(Expression.Convert(property, typeof(object)), parameter);
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

        private Guid CreateUom(Uom model)
        {
            _dbContext.Uom.Add(model);
            _dbContext.SaveChanges();
            return model.Id;
        }

        private void UpdateUom(Guid id, Uom updatedEntity)
        {
            _dbContext.Uom.Update(updatedEntity);
            _dbContext.SaveChanges();
        }

        private bool DeleteUom(Guid id)
        {
            var entityData = _dbContext.Uom.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            if (entityData == null)
            {
                throw new ApplicationException("No data found!");
            }

            _dbContext.Uom.Remove(entityData);
            _dbContext.SaveChanges();
            return true;
        }

        private void PatchUom(Guid id, JsonPatchDocument<Uom> updatedEntity)
        {
            if (updatedEntity == null)
            {
                throw new ApplicationException("Patch document is missing!");
            }

            var existingEntity = _dbContext.Uom.FirstOrDefault(t => t.Id == id);
            if (existingEntity == null)
            {
                throw new ApplicationException("No data found!");
            }

            updatedEntity.ApplyTo(existingEntity);
            _dbContext.Uom.Update(existingEntity);
            _dbContext.SaveChanges();
        }
        #endregion
    }
}