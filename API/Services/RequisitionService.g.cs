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
    /// The requisitionService responsible for managing requisition related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting requisition information.
    /// </remarks>
    public interface IRequisitionService
    {
        /// <summary>Retrieves a specific requisition by its primary key</summary>
        /// <param name="id">The primary key of the requisition</param>
        /// <returns>The requisition data</returns>
        Requisition GetById(Guid id);

        /// <summary>Retrieves a list of requisitions based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of requisitions</returns>
        List<Requisition> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc");

        /// <summary>Adds a new requisition</summary>
        /// <param name="model">The requisition data to be added</param>
        /// <returns>The result of the operation</returns>
        Guid Create(Requisition model);

        /// <summary>Updates a specific requisition by its primary key</summary>
        /// <param name="id">The primary key of the requisition</param>
        /// <param name="updatedEntity">The requisition data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Update(Guid id, Requisition updatedEntity);

        /// <summary>Updates a specific requisition by its primary key</summary>
        /// <param name="id">The primary key of the requisition</param>
        /// <param name="updatedEntity">The requisition data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Patch(Guid id, JsonPatchDocument<Requisition> updatedEntity);

        /// <summary>Deletes a specific requisition by its primary key</summary>
        /// <param name="id">The primary key of the requisition</param>
        /// <returns>The result of the operation</returns>
        bool Delete(Guid id);
    }

    /// <summary>
    /// The requisitionService responsible for managing requisition related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting requisition information.
    /// </remarks>
    public class RequisitionService : IRequisitionService
    {
        private Demo17SepContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the Requisition class.
        /// </summary>
        /// <param name="dbContext">dbContext value to set.</param>
        public RequisitionService(Demo17SepContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>Retrieves a specific requisition by its primary key</summary>
        /// <param name="id">The primary key of the requisition</param>
        /// <returns>The requisition data</returns>
        public Requisition GetById(Guid id)
        {
            var entityData = _dbContext.Requisition.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            return entityData;
        }

        /// <summary>Retrieves a list of requisitions based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of requisitions</returns>/// <exception cref="Exception"></exception>
        public List<Requisition> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            var result = GetRequisition(filters, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return result;
        }

        /// <summary>Adds a new requisition</summary>
        /// <param name="model">The requisition data to be added</param>
        /// <returns>The result of the operation</returns>
        public Guid Create(Requisition model)
        {
            model.Id = CreateRequisition(model);
            return model.Id;
        }

        /// <summary>Updates a specific requisition by its primary key</summary>
        /// <param name="id">The primary key of the requisition</param>
        /// <param name="updatedEntity">The requisition data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Update(Guid id, Requisition updatedEntity)
        {
            UpdateRequisition(id, updatedEntity);
            return true;
        }

        /// <summary>Updates a specific requisition by its primary key</summary>
        /// <param name="id">The primary key of the requisition</param>
        /// <param name="updatedEntity">The requisition data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Patch(Guid id, JsonPatchDocument<Requisition> updatedEntity)
        {
            PatchRequisition(id, updatedEntity);
            return true;
        }

        /// <summary>Deletes a specific requisition by its primary key</summary>
        /// <param name="id">The primary key of the requisition</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Delete(Guid id)
        {
            DeleteRequisition(id);
            return true;
        }
        #region
        private List<Requisition> GetRequisition(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            if (pageSize < 1)
            {
                throw new ApplicationException("Page size invalid!");
            }

            if (pageNumber < 1)
            {
                throw new ApplicationException("Page mumber invalid!");
            }

            var query = _dbContext.Requisition.IncludeRelated().AsQueryable();
            int skip = (pageNumber - 1) * pageSize;
            var result = FilterService<Requisition>.ApplyFilter(query, filters, searchTerm);
            if (!string.IsNullOrEmpty(sortField))
            {
                var parameter = Expression.Parameter(typeof(Requisition), "b");
                var property = Expression.Property(parameter, sortField);
                var lambda = Expression.Lambda<Func<Requisition, object>>(Expression.Convert(property, typeof(object)), parameter);
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

        private Guid CreateRequisition(Requisition model)
        {
            _dbContext.Requisition.Add(model);
            _dbContext.SaveChanges();
            return model.Id;
        }

        private void UpdateRequisition(Guid id, Requisition updatedEntity)
        {
            _dbContext.Requisition.Update(updatedEntity);
            _dbContext.SaveChanges();
        }

        private bool DeleteRequisition(Guid id)
        {
            var entityData = _dbContext.Requisition.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            if (entityData == null)
            {
                throw new ApplicationException("No data found!");
            }

            _dbContext.Requisition.Remove(entityData);
            _dbContext.SaveChanges();
            return true;
        }

        private void PatchRequisition(Guid id, JsonPatchDocument<Requisition> updatedEntity)
        {
            if (updatedEntity == null)
            {
                throw new ApplicationException("Patch document is missing!");
            }

            var existingEntity = _dbContext.Requisition.FirstOrDefault(t => t.Id == id);
            if (existingEntity == null)
            {
                throw new ApplicationException("No data found!");
            }

            updatedEntity.ApplyTo(existingEntity);
            _dbContext.Requisition.Update(existingEntity);
            _dbContext.SaveChanges();
        }
        #endregion
    }
}