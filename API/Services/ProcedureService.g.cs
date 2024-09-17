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
    /// The procedureService responsible for managing procedure related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting procedure information.
    /// </remarks>
    public interface IProcedureService
    {
        /// <summary>Retrieves a specific procedure by its primary key</summary>
        /// <param name="id">The primary key of the procedure</param>
        /// <returns>The procedure data</returns>
        Procedure GetById(Guid id);

        /// <summary>Retrieves a list of procedures based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of procedures</returns>
        List<Procedure> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc");

        /// <summary>Adds a new procedure</summary>
        /// <param name="model">The procedure data to be added</param>
        /// <returns>The result of the operation</returns>
        Guid Create(Procedure model);

        /// <summary>Updates a specific procedure by its primary key</summary>
        /// <param name="id">The primary key of the procedure</param>
        /// <param name="updatedEntity">The procedure data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Update(Guid id, Procedure updatedEntity);

        /// <summary>Updates a specific procedure by its primary key</summary>
        /// <param name="id">The primary key of the procedure</param>
        /// <param name="updatedEntity">The procedure data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Patch(Guid id, JsonPatchDocument<Procedure> updatedEntity);

        /// <summary>Deletes a specific procedure by its primary key</summary>
        /// <param name="id">The primary key of the procedure</param>
        /// <returns>The result of the operation</returns>
        bool Delete(Guid id);
    }

    /// <summary>
    /// The procedureService responsible for managing procedure related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting procedure information.
    /// </remarks>
    public class ProcedureService : IProcedureService
    {
        private Demo17SepContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the Procedure class.
        /// </summary>
        /// <param name="dbContext">dbContext value to set.</param>
        public ProcedureService(Demo17SepContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>Retrieves a specific procedure by its primary key</summary>
        /// <param name="id">The primary key of the procedure</param>
        /// <returns>The procedure data</returns>
        public Procedure GetById(Guid id)
        {
            var entityData = _dbContext.Procedure.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            return entityData;
        }

        /// <summary>Retrieves a list of procedures based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of procedures</returns>/// <exception cref="Exception"></exception>
        public List<Procedure> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            var result = GetProcedure(filters, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return result;
        }

        /// <summary>Adds a new procedure</summary>
        /// <param name="model">The procedure data to be added</param>
        /// <returns>The result of the operation</returns>
        public Guid Create(Procedure model)
        {
            model.Id = CreateProcedure(model);
            return model.Id;
        }

        /// <summary>Updates a specific procedure by its primary key</summary>
        /// <param name="id">The primary key of the procedure</param>
        /// <param name="updatedEntity">The procedure data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Update(Guid id, Procedure updatedEntity)
        {
            UpdateProcedure(id, updatedEntity);
            return true;
        }

        /// <summary>Updates a specific procedure by its primary key</summary>
        /// <param name="id">The primary key of the procedure</param>
        /// <param name="updatedEntity">The procedure data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Patch(Guid id, JsonPatchDocument<Procedure> updatedEntity)
        {
            PatchProcedure(id, updatedEntity);
            return true;
        }

        /// <summary>Deletes a specific procedure by its primary key</summary>
        /// <param name="id">The primary key of the procedure</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Delete(Guid id)
        {
            DeleteProcedure(id);
            return true;
        }
        #region
        private List<Procedure> GetProcedure(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            if (pageSize < 1)
            {
                throw new ApplicationException("Page size invalid!");
            }

            if (pageNumber < 1)
            {
                throw new ApplicationException("Page mumber invalid!");
            }

            var query = _dbContext.Procedure.IncludeRelated().AsQueryable();
            int skip = (pageNumber - 1) * pageSize;
            var result = FilterService<Procedure>.ApplyFilter(query, filters, searchTerm);
            if (!string.IsNullOrEmpty(sortField))
            {
                var parameter = Expression.Parameter(typeof(Procedure), "b");
                var property = Expression.Property(parameter, sortField);
                var lambda = Expression.Lambda<Func<Procedure, object>>(Expression.Convert(property, typeof(object)), parameter);
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

        private Guid CreateProcedure(Procedure model)
        {
            _dbContext.Procedure.Add(model);
            _dbContext.SaveChanges();
            return model.Id;
        }

        private void UpdateProcedure(Guid id, Procedure updatedEntity)
        {
            _dbContext.Procedure.Update(updatedEntity);
            _dbContext.SaveChanges();
        }

        private bool DeleteProcedure(Guid id)
        {
            var entityData = _dbContext.Procedure.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            if (entityData == null)
            {
                throw new ApplicationException("No data found!");
            }

            _dbContext.Procedure.Remove(entityData);
            _dbContext.SaveChanges();
            return true;
        }

        private void PatchProcedure(Guid id, JsonPatchDocument<Procedure> updatedEntity)
        {
            if (updatedEntity == null)
            {
                throw new ApplicationException("Patch document is missing!");
            }

            var existingEntity = _dbContext.Procedure.FirstOrDefault(t => t.Id == id);
            if (existingEntity == null)
            {
                throw new ApplicationException("No data found!");
            }

            updatedEntity.ApplyTo(existingEntity);
            _dbContext.Procedure.Update(existingEntity);
            _dbContext.SaveChanges();
        }
        #endregion
    }
}