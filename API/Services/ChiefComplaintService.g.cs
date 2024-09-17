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
    /// The chiefcomplaintService responsible for managing chiefcomplaint related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting chiefcomplaint information.
    /// </remarks>
    public interface IChiefComplaintService
    {
        /// <summary>Retrieves a specific chiefcomplaint by its primary key</summary>
        /// <param name="id">The primary key of the chiefcomplaint</param>
        /// <returns>The chiefcomplaint data</returns>
        ChiefComplaint GetById(Guid id);

        /// <summary>Retrieves a list of chiefcomplaints based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of chiefcomplaints</returns>
        List<ChiefComplaint> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc");

        /// <summary>Adds a new chiefcomplaint</summary>
        /// <param name="model">The chiefcomplaint data to be added</param>
        /// <returns>The result of the operation</returns>
        Guid Create(ChiefComplaint model);

        /// <summary>Updates a specific chiefcomplaint by its primary key</summary>
        /// <param name="id">The primary key of the chiefcomplaint</param>
        /// <param name="updatedEntity">The chiefcomplaint data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Update(Guid id, ChiefComplaint updatedEntity);

        /// <summary>Updates a specific chiefcomplaint by its primary key</summary>
        /// <param name="id">The primary key of the chiefcomplaint</param>
        /// <param name="updatedEntity">The chiefcomplaint data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Patch(Guid id, JsonPatchDocument<ChiefComplaint> updatedEntity);

        /// <summary>Deletes a specific chiefcomplaint by its primary key</summary>
        /// <param name="id">The primary key of the chiefcomplaint</param>
        /// <returns>The result of the operation</returns>
        bool Delete(Guid id);
    }

    /// <summary>
    /// The chiefcomplaintService responsible for managing chiefcomplaint related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting chiefcomplaint information.
    /// </remarks>
    public class ChiefComplaintService : IChiefComplaintService
    {
        private Demo17SepContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the ChiefComplaint class.
        /// </summary>
        /// <param name="dbContext">dbContext value to set.</param>
        public ChiefComplaintService(Demo17SepContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>Retrieves a specific chiefcomplaint by its primary key</summary>
        /// <param name="id">The primary key of the chiefcomplaint</param>
        /// <returns>The chiefcomplaint data</returns>
        public ChiefComplaint GetById(Guid id)
        {
            var entityData = _dbContext.ChiefComplaint.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            return entityData;
        }

        /// <summary>Retrieves a list of chiefcomplaints based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of chiefcomplaints</returns>/// <exception cref="Exception"></exception>
        public List<ChiefComplaint> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            var result = GetChiefComplaint(filters, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return result;
        }

        /// <summary>Adds a new chiefcomplaint</summary>
        /// <param name="model">The chiefcomplaint data to be added</param>
        /// <returns>The result of the operation</returns>
        public Guid Create(ChiefComplaint model)
        {
            model.Id = CreateChiefComplaint(model);
            return model.Id;
        }

        /// <summary>Updates a specific chiefcomplaint by its primary key</summary>
        /// <param name="id">The primary key of the chiefcomplaint</param>
        /// <param name="updatedEntity">The chiefcomplaint data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Update(Guid id, ChiefComplaint updatedEntity)
        {
            UpdateChiefComplaint(id, updatedEntity);
            return true;
        }

        /// <summary>Updates a specific chiefcomplaint by its primary key</summary>
        /// <param name="id">The primary key of the chiefcomplaint</param>
        /// <param name="updatedEntity">The chiefcomplaint data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Patch(Guid id, JsonPatchDocument<ChiefComplaint> updatedEntity)
        {
            PatchChiefComplaint(id, updatedEntity);
            return true;
        }

        /// <summary>Deletes a specific chiefcomplaint by its primary key</summary>
        /// <param name="id">The primary key of the chiefcomplaint</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Delete(Guid id)
        {
            DeleteChiefComplaint(id);
            return true;
        }
        #region
        private List<ChiefComplaint> GetChiefComplaint(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            if (pageSize < 1)
            {
                throw new ApplicationException("Page size invalid!");
            }

            if (pageNumber < 1)
            {
                throw new ApplicationException("Page mumber invalid!");
            }

            var query = _dbContext.ChiefComplaint.IncludeRelated().AsQueryable();
            int skip = (pageNumber - 1) * pageSize;
            var result = FilterService<ChiefComplaint>.ApplyFilter(query, filters, searchTerm);
            if (!string.IsNullOrEmpty(sortField))
            {
                var parameter = Expression.Parameter(typeof(ChiefComplaint), "b");
                var property = Expression.Property(parameter, sortField);
                var lambda = Expression.Lambda<Func<ChiefComplaint, object>>(Expression.Convert(property, typeof(object)), parameter);
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

        private Guid CreateChiefComplaint(ChiefComplaint model)
        {
            _dbContext.ChiefComplaint.Add(model);
            _dbContext.SaveChanges();
            return model.Id;
        }

        private void UpdateChiefComplaint(Guid id, ChiefComplaint updatedEntity)
        {
            _dbContext.ChiefComplaint.Update(updatedEntity);
            _dbContext.SaveChanges();
        }

        private bool DeleteChiefComplaint(Guid id)
        {
            var entityData = _dbContext.ChiefComplaint.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            if (entityData == null)
            {
                throw new ApplicationException("No data found!");
            }

            _dbContext.ChiefComplaint.Remove(entityData);
            _dbContext.SaveChanges();
            return true;
        }

        private void PatchChiefComplaint(Guid id, JsonPatchDocument<ChiefComplaint> updatedEntity)
        {
            if (updatedEntity == null)
            {
                throw new ApplicationException("Patch document is missing!");
            }

            var existingEntity = _dbContext.ChiefComplaint.FirstOrDefault(t => t.Id == id);
            if (existingEntity == null)
            {
                throw new ApplicationException("No data found!");
            }

            updatedEntity.ApplyTo(existingEntity);
            _dbContext.ChiefComplaint.Update(existingEntity);
            _dbContext.SaveChanges();
        }
        #endregion
    }
}