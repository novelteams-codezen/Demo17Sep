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
    /// The requisitionfileService responsible for managing requisitionfile related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting requisitionfile information.
    /// </remarks>
    public interface IRequisitionFileService
    {
        /// <summary>Retrieves a specific requisitionfile by its primary key</summary>
        /// <param name="id">The primary key of the requisitionfile</param>
        /// <returns>The requisitionfile data</returns>
        RequisitionFile GetById(Guid id);

        /// <summary>Retrieves a list of requisitionfiles based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of requisitionfiles</returns>
        List<RequisitionFile> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc");

        /// <summary>Adds a new requisitionfile</summary>
        /// <param name="model">The requisitionfile data to be added</param>
        /// <returns>The result of the operation</returns>
        Guid Create(RequisitionFile model);

        /// <summary>Updates a specific requisitionfile by its primary key</summary>
        /// <param name="id">The primary key of the requisitionfile</param>
        /// <param name="updatedEntity">The requisitionfile data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Update(Guid id, RequisitionFile updatedEntity);

        /// <summary>Updates a specific requisitionfile by its primary key</summary>
        /// <param name="id">The primary key of the requisitionfile</param>
        /// <param name="updatedEntity">The requisitionfile data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Patch(Guid id, JsonPatchDocument<RequisitionFile> updatedEntity);

        /// <summary>Deletes a specific requisitionfile by its primary key</summary>
        /// <param name="id">The primary key of the requisitionfile</param>
        /// <returns>The result of the operation</returns>
        bool Delete(Guid id);
    }

    /// <summary>
    /// The requisitionfileService responsible for managing requisitionfile related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting requisitionfile information.
    /// </remarks>
    public class RequisitionFileService : IRequisitionFileService
    {
        private Demo17SepContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the RequisitionFile class.
        /// </summary>
        /// <param name="dbContext">dbContext value to set.</param>
        public RequisitionFileService(Demo17SepContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>Retrieves a specific requisitionfile by its primary key</summary>
        /// <param name="id">The primary key of the requisitionfile</param>
        /// <returns>The requisitionfile data</returns>
        public RequisitionFile GetById(Guid id)
        {
            var entityData = _dbContext.RequisitionFile.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            return entityData;
        }

        /// <summary>Retrieves a list of requisitionfiles based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of requisitionfiles</returns>/// <exception cref="Exception"></exception>
        public List<RequisitionFile> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            var result = GetRequisitionFile(filters, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return result;
        }

        /// <summary>Adds a new requisitionfile</summary>
        /// <param name="model">The requisitionfile data to be added</param>
        /// <returns>The result of the operation</returns>
        public Guid Create(RequisitionFile model)
        {
            model.Id = CreateRequisitionFile(model);
            return model.Id;
        }

        /// <summary>Updates a specific requisitionfile by its primary key</summary>
        /// <param name="id">The primary key of the requisitionfile</param>
        /// <param name="updatedEntity">The requisitionfile data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Update(Guid id, RequisitionFile updatedEntity)
        {
            UpdateRequisitionFile(id, updatedEntity);
            return true;
        }

        /// <summary>Updates a specific requisitionfile by its primary key</summary>
        /// <param name="id">The primary key of the requisitionfile</param>
        /// <param name="updatedEntity">The requisitionfile data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Patch(Guid id, JsonPatchDocument<RequisitionFile> updatedEntity)
        {
            PatchRequisitionFile(id, updatedEntity);
            return true;
        }

        /// <summary>Deletes a specific requisitionfile by its primary key</summary>
        /// <param name="id">The primary key of the requisitionfile</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Delete(Guid id)
        {
            DeleteRequisitionFile(id);
            return true;
        }
        #region
        private List<RequisitionFile> GetRequisitionFile(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            if (pageSize < 1)
            {
                throw new ApplicationException("Page size invalid!");
            }

            if (pageNumber < 1)
            {
                throw new ApplicationException("Page mumber invalid!");
            }

            var query = _dbContext.RequisitionFile.IncludeRelated().AsQueryable();
            int skip = (pageNumber - 1) * pageSize;
            var result = FilterService<RequisitionFile>.ApplyFilter(query, filters, searchTerm);
            if (!string.IsNullOrEmpty(sortField))
            {
                var parameter = Expression.Parameter(typeof(RequisitionFile), "b");
                var property = Expression.Property(parameter, sortField);
                var lambda = Expression.Lambda<Func<RequisitionFile, object>>(Expression.Convert(property, typeof(object)), parameter);
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

        private Guid CreateRequisitionFile(RequisitionFile model)
        {
            _dbContext.RequisitionFile.Add(model);
            _dbContext.SaveChanges();
            return model.Id;
        }

        private void UpdateRequisitionFile(Guid id, RequisitionFile updatedEntity)
        {
            _dbContext.RequisitionFile.Update(updatedEntity);
            _dbContext.SaveChanges();
        }

        private bool DeleteRequisitionFile(Guid id)
        {
            var entityData = _dbContext.RequisitionFile.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            if (entityData == null)
            {
                throw new ApplicationException("No data found!");
            }

            _dbContext.RequisitionFile.Remove(entityData);
            _dbContext.SaveChanges();
            return true;
        }

        private void PatchRequisitionFile(Guid id, JsonPatchDocument<RequisitionFile> updatedEntity)
        {
            if (updatedEntity == null)
            {
                throw new ApplicationException("Patch document is missing!");
            }

            var existingEntity = _dbContext.RequisitionFile.FirstOrDefault(t => t.Id == id);
            if (existingEntity == null)
            {
                throw new ApplicationException("No data found!");
            }

            updatedEntity.ApplyTo(existingEntity);
            _dbContext.RequisitionFile.Update(existingEntity);
            _dbContext.SaveChanges();
        }
        #endregion
    }
}