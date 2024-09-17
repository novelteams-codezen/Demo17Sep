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
    /// The visitdiagnosisService responsible for managing visitdiagnosis related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting visitdiagnosis information.
    /// </remarks>
    public interface IVisitDiagnosisService
    {
        /// <summary>Retrieves a specific visitdiagnosis by its primary key</summary>
        /// <param name="id">The primary key of the visitdiagnosis</param>
        /// <returns>The visitdiagnosis data</returns>
        VisitDiagnosis GetById(Guid id);

        /// <summary>Retrieves a list of visitdiagnosiss based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of visitdiagnosiss</returns>
        List<VisitDiagnosis> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc");

        /// <summary>Adds a new visitdiagnosis</summary>
        /// <param name="model">The visitdiagnosis data to be added</param>
        /// <returns>The result of the operation</returns>
        Guid Create(VisitDiagnosis model);

        /// <summary>Updates a specific visitdiagnosis by its primary key</summary>
        /// <param name="id">The primary key of the visitdiagnosis</param>
        /// <param name="updatedEntity">The visitdiagnosis data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Update(Guid id, VisitDiagnosis updatedEntity);

        /// <summary>Updates a specific visitdiagnosis by its primary key</summary>
        /// <param name="id">The primary key of the visitdiagnosis</param>
        /// <param name="updatedEntity">The visitdiagnosis data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Patch(Guid id, JsonPatchDocument<VisitDiagnosis> updatedEntity);

        /// <summary>Deletes a specific visitdiagnosis by its primary key</summary>
        /// <param name="id">The primary key of the visitdiagnosis</param>
        /// <returns>The result of the operation</returns>
        bool Delete(Guid id);
    }

    /// <summary>
    /// The visitdiagnosisService responsible for managing visitdiagnosis related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting visitdiagnosis information.
    /// </remarks>
    public class VisitDiagnosisService : IVisitDiagnosisService
    {
        private Demo17SepContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the VisitDiagnosis class.
        /// </summary>
        /// <param name="dbContext">dbContext value to set.</param>
        public VisitDiagnosisService(Demo17SepContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>Retrieves a specific visitdiagnosis by its primary key</summary>
        /// <param name="id">The primary key of the visitdiagnosis</param>
        /// <returns>The visitdiagnosis data</returns>
        public VisitDiagnosis GetById(Guid id)
        {
            var entityData = _dbContext.VisitDiagnosis.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            return entityData;
        }

        /// <summary>Retrieves a list of visitdiagnosiss based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of visitdiagnosiss</returns>/// <exception cref="Exception"></exception>
        public List<VisitDiagnosis> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            var result = GetVisitDiagnosis(filters, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return result;
        }

        /// <summary>Adds a new visitdiagnosis</summary>
        /// <param name="model">The visitdiagnosis data to be added</param>
        /// <returns>The result of the operation</returns>
        public Guid Create(VisitDiagnosis model)
        {
            model.Id = CreateVisitDiagnosis(model);
            return model.Id;
        }

        /// <summary>Updates a specific visitdiagnosis by its primary key</summary>
        /// <param name="id">The primary key of the visitdiagnosis</param>
        /// <param name="updatedEntity">The visitdiagnosis data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Update(Guid id, VisitDiagnosis updatedEntity)
        {
            UpdateVisitDiagnosis(id, updatedEntity);
            return true;
        }

        /// <summary>Updates a specific visitdiagnosis by its primary key</summary>
        /// <param name="id">The primary key of the visitdiagnosis</param>
        /// <param name="updatedEntity">The visitdiagnosis data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Patch(Guid id, JsonPatchDocument<VisitDiagnosis> updatedEntity)
        {
            PatchVisitDiagnosis(id, updatedEntity);
            return true;
        }

        /// <summary>Deletes a specific visitdiagnosis by its primary key</summary>
        /// <param name="id">The primary key of the visitdiagnosis</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Delete(Guid id)
        {
            DeleteVisitDiagnosis(id);
            return true;
        }
        #region
        private List<VisitDiagnosis> GetVisitDiagnosis(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            if (pageSize < 1)
            {
                throw new ApplicationException("Page size invalid!");
            }

            if (pageNumber < 1)
            {
                throw new ApplicationException("Page mumber invalid!");
            }

            var query = _dbContext.VisitDiagnosis.IncludeRelated().AsQueryable();
            int skip = (pageNumber - 1) * pageSize;
            var result = FilterService<VisitDiagnosis>.ApplyFilter(query, filters, searchTerm);
            if (!string.IsNullOrEmpty(sortField))
            {
                var parameter = Expression.Parameter(typeof(VisitDiagnosis), "b");
                var property = Expression.Property(parameter, sortField);
                var lambda = Expression.Lambda<Func<VisitDiagnosis, object>>(Expression.Convert(property, typeof(object)), parameter);
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

        private Guid CreateVisitDiagnosis(VisitDiagnosis model)
        {
            _dbContext.VisitDiagnosis.Add(model);
            _dbContext.SaveChanges();
            return model.Id;
        }

        private void UpdateVisitDiagnosis(Guid id, VisitDiagnosis updatedEntity)
        {
            _dbContext.VisitDiagnosis.Update(updatedEntity);
            _dbContext.SaveChanges();
        }

        private bool DeleteVisitDiagnosis(Guid id)
        {
            var entityData = _dbContext.VisitDiagnosis.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            if (entityData == null)
            {
                throw new ApplicationException("No data found!");
            }

            _dbContext.VisitDiagnosis.Remove(entityData);
            _dbContext.SaveChanges();
            return true;
        }

        private void PatchVisitDiagnosis(Guid id, JsonPatchDocument<VisitDiagnosis> updatedEntity)
        {
            if (updatedEntity == null)
            {
                throw new ApplicationException("Patch document is missing!");
            }

            var existingEntity = _dbContext.VisitDiagnosis.FirstOrDefault(t => t.Id == id);
            if (existingEntity == null)
            {
                throw new ApplicationException("No data found!");
            }

            updatedEntity.ApplyTo(existingEntity);
            _dbContext.VisitDiagnosis.Update(existingEntity);
            _dbContext.SaveChanges();
        }
        #endregion
    }
}