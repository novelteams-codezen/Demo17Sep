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
    /// The requisitionlineService responsible for managing requisitionline related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting requisitionline information.
    /// </remarks>
    public interface IRequisitionLineService
    {
        /// <summary>Retrieves a specific requisitionline by its primary key</summary>
        /// <param name="id">The primary key of the requisitionline</param>
        /// <returns>The requisitionline data</returns>
        RequisitionLine GetById(Guid id);

        /// <summary>Retrieves a list of requisitionlines based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of requisitionlines</returns>
        List<RequisitionLine> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc");

        /// <summary>Adds a new requisitionline</summary>
        /// <param name="model">The requisitionline data to be added</param>
        /// <returns>The result of the operation</returns>
        Guid Create(RequisitionLine model);

        /// <summary>Updates a specific requisitionline by its primary key</summary>
        /// <param name="id">The primary key of the requisitionline</param>
        /// <param name="updatedEntity">The requisitionline data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Update(Guid id, RequisitionLine updatedEntity);

        /// <summary>Updates a specific requisitionline by its primary key</summary>
        /// <param name="id">The primary key of the requisitionline</param>
        /// <param name="updatedEntity">The requisitionline data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Patch(Guid id, JsonPatchDocument<RequisitionLine> updatedEntity);

        /// <summary>Deletes a specific requisitionline by its primary key</summary>
        /// <param name="id">The primary key of the requisitionline</param>
        /// <returns>The result of the operation</returns>
        bool Delete(Guid id);
    }

    /// <summary>
    /// The requisitionlineService responsible for managing requisitionline related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting requisitionline information.
    /// </remarks>
    public class RequisitionLineService : IRequisitionLineService
    {
        private Demo17SepContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the RequisitionLine class.
        /// </summary>
        /// <param name="dbContext">dbContext value to set.</param>
        public RequisitionLineService(Demo17SepContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>Retrieves a specific requisitionline by its primary key</summary>
        /// <param name="id">The primary key of the requisitionline</param>
        /// <returns>The requisitionline data</returns>
        public RequisitionLine GetById(Guid id)
        {
            var entityData = _dbContext.RequisitionLine.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            return entityData;
        }

        /// <summary>Retrieves a list of requisitionlines based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of requisitionlines</returns>/// <exception cref="Exception"></exception>
        public List<RequisitionLine> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            var result = GetRequisitionLine(filters, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return result;
        }

        /// <summary>Adds a new requisitionline</summary>
        /// <param name="model">The requisitionline data to be added</param>
        /// <returns>The result of the operation</returns>
        public Guid Create(RequisitionLine model)
        {
            model.Id = CreateRequisitionLine(model);
            return model.Id;
        }

        /// <summary>Updates a specific requisitionline by its primary key</summary>
        /// <param name="id">The primary key of the requisitionline</param>
        /// <param name="updatedEntity">The requisitionline data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Update(Guid id, RequisitionLine updatedEntity)
        {
            UpdateRequisitionLine(id, updatedEntity);
            return true;
        }

        /// <summary>Updates a specific requisitionline by its primary key</summary>
        /// <param name="id">The primary key of the requisitionline</param>
        /// <param name="updatedEntity">The requisitionline data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Patch(Guid id, JsonPatchDocument<RequisitionLine> updatedEntity)
        {
            PatchRequisitionLine(id, updatedEntity);
            return true;
        }

        /// <summary>Deletes a specific requisitionline by its primary key</summary>
        /// <param name="id">The primary key of the requisitionline</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Delete(Guid id)
        {
            DeleteRequisitionLine(id);
            return true;
        }
        #region
        private List<RequisitionLine> GetRequisitionLine(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            if (pageSize < 1)
            {
                throw new ApplicationException("Page size invalid!");
            }

            if (pageNumber < 1)
            {
                throw new ApplicationException("Page mumber invalid!");
            }

            var query = _dbContext.RequisitionLine.IncludeRelated().AsQueryable();
            int skip = (pageNumber - 1) * pageSize;
            var result = FilterService<RequisitionLine>.ApplyFilter(query, filters, searchTerm);
            if (!string.IsNullOrEmpty(sortField))
            {
                var parameter = Expression.Parameter(typeof(RequisitionLine), "b");
                var property = Expression.Property(parameter, sortField);
                var lambda = Expression.Lambda<Func<RequisitionLine, object>>(Expression.Convert(property, typeof(object)), parameter);
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

        private Guid CreateRequisitionLine(RequisitionLine model)
        {
            _dbContext.RequisitionLine.Add(model);
            _dbContext.SaveChanges();
            return model.Id;
        }

        private void UpdateRequisitionLine(Guid id, RequisitionLine updatedEntity)
        {
            _dbContext.RequisitionLine.Update(updatedEntity);
            _dbContext.SaveChanges();
        }

        private bool DeleteRequisitionLine(Guid id)
        {
            var entityData = _dbContext.RequisitionLine.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            if (entityData == null)
            {
                throw new ApplicationException("No data found!");
            }

            _dbContext.RequisitionLine.Remove(entityData);
            _dbContext.SaveChanges();
            return true;
        }

        private void PatchRequisitionLine(Guid id, JsonPatchDocument<RequisitionLine> updatedEntity)
        {
            if (updatedEntity == null)
            {
                throw new ApplicationException("Patch document is missing!");
            }

            var existingEntity = _dbContext.RequisitionLine.FirstOrDefault(t => t.Id == id);
            if (existingEntity == null)
            {
                throw new ApplicationException("No data found!");
            }

            updatedEntity.ApplyTo(existingEntity);
            _dbContext.RequisitionLine.Update(existingEntity);
            _dbContext.SaveChanges();
        }
        #endregion
    }
}