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
    /// The specialisationService responsible for managing specialisation related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting specialisation information.
    /// </remarks>
    public interface ISpecialisationService
    {
        /// <summary>Retrieves a specific specialisation by its primary key</summary>
        /// <param name="id">The primary key of the specialisation</param>
        /// <returns>The specialisation data</returns>
        Specialisation GetById(Guid id);

        /// <summary>Retrieves a list of specialisations based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of specialisations</returns>
        List<Specialisation> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc");

        /// <summary>Adds a new specialisation</summary>
        /// <param name="model">The specialisation data to be added</param>
        /// <returns>The result of the operation</returns>
        Guid Create(Specialisation model);

        /// <summary>Updates a specific specialisation by its primary key</summary>
        /// <param name="id">The primary key of the specialisation</param>
        /// <param name="updatedEntity">The specialisation data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Update(Guid id, Specialisation updatedEntity);

        /// <summary>Updates a specific specialisation by its primary key</summary>
        /// <param name="id">The primary key of the specialisation</param>
        /// <param name="updatedEntity">The specialisation data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Patch(Guid id, JsonPatchDocument<Specialisation> updatedEntity);

        /// <summary>Deletes a specific specialisation by its primary key</summary>
        /// <param name="id">The primary key of the specialisation</param>
        /// <returns>The result of the operation</returns>
        bool Delete(Guid id);
    }

    /// <summary>
    /// The specialisationService responsible for managing specialisation related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting specialisation information.
    /// </remarks>
    public class SpecialisationService : ISpecialisationService
    {
        private Demo17SepContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the Specialisation class.
        /// </summary>
        /// <param name="dbContext">dbContext value to set.</param>
        public SpecialisationService(Demo17SepContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>Retrieves a specific specialisation by its primary key</summary>
        /// <param name="id">The primary key of the specialisation</param>
        /// <returns>The specialisation data</returns>
        public Specialisation GetById(Guid id)
        {
            var entityData = _dbContext.Specialisation.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            return entityData;
        }

        /// <summary>Retrieves a list of specialisations based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of specialisations</returns>/// <exception cref="Exception"></exception>
        public List<Specialisation> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            var result = GetSpecialisation(filters, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return result;
        }

        /// <summary>Adds a new specialisation</summary>
        /// <param name="model">The specialisation data to be added</param>
        /// <returns>The result of the operation</returns>
        public Guid Create(Specialisation model)
        {
            model.Id = CreateSpecialisation(model);
            return model.Id;
        }

        /// <summary>Updates a specific specialisation by its primary key</summary>
        /// <param name="id">The primary key of the specialisation</param>
        /// <param name="updatedEntity">The specialisation data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Update(Guid id, Specialisation updatedEntity)
        {
            UpdateSpecialisation(id, updatedEntity);
            return true;
        }

        /// <summary>Updates a specific specialisation by its primary key</summary>
        /// <param name="id">The primary key of the specialisation</param>
        /// <param name="updatedEntity">The specialisation data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Patch(Guid id, JsonPatchDocument<Specialisation> updatedEntity)
        {
            PatchSpecialisation(id, updatedEntity);
            return true;
        }

        /// <summary>Deletes a specific specialisation by its primary key</summary>
        /// <param name="id">The primary key of the specialisation</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Delete(Guid id)
        {
            DeleteSpecialisation(id);
            return true;
        }
        #region
        private List<Specialisation> GetSpecialisation(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            if (pageSize < 1)
            {
                throw new ApplicationException("Page size invalid!");
            }

            if (pageNumber < 1)
            {
                throw new ApplicationException("Page mumber invalid!");
            }

            var query = _dbContext.Specialisation.IncludeRelated().AsQueryable();
            int skip = (pageNumber - 1) * pageSize;
            var result = FilterService<Specialisation>.ApplyFilter(query, filters, searchTerm);
            if (!string.IsNullOrEmpty(sortField))
            {
                var parameter = Expression.Parameter(typeof(Specialisation), "b");
                var property = Expression.Property(parameter, sortField);
                var lambda = Expression.Lambda<Func<Specialisation, object>>(Expression.Convert(property, typeof(object)), parameter);
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

        private Guid CreateSpecialisation(Specialisation model)
        {
            _dbContext.Specialisation.Add(model);
            _dbContext.SaveChanges();
            return model.Id;
        }

        private void UpdateSpecialisation(Guid id, Specialisation updatedEntity)
        {
            _dbContext.Specialisation.Update(updatedEntity);
            _dbContext.SaveChanges();
        }

        private bool DeleteSpecialisation(Guid id)
        {
            var entityData = _dbContext.Specialisation.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            if (entityData == null)
            {
                throw new ApplicationException("No data found!");
            }

            _dbContext.Specialisation.Remove(entityData);
            _dbContext.SaveChanges();
            return true;
        }

        private void PatchSpecialisation(Guid id, JsonPatchDocument<Specialisation> updatedEntity)
        {
            if (updatedEntity == null)
            {
                throw new ApplicationException("Patch document is missing!");
            }

            var existingEntity = _dbContext.Specialisation.FirstOrDefault(t => t.Id == id);
            if (existingEntity == null)
            {
                throw new ApplicationException("No data found!");
            }

            updatedEntity.ApplyTo(existingEntity);
            _dbContext.Specialisation.Update(existingEntity);
            _dbContext.SaveChanges();
        }
        #endregion
    }
}