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
    /// The doctorfavouritemedicationService responsible for managing doctorfavouritemedication related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting doctorfavouritemedication information.
    /// </remarks>
    public interface IDoctorFavouriteMedicationService
    {
        /// <summary>Retrieves a specific doctorfavouritemedication by its primary key</summary>
        /// <param name="id">The primary key of the doctorfavouritemedication</param>
        /// <returns>The doctorfavouritemedication data</returns>
        DoctorFavouriteMedication GetById(Guid id);

        /// <summary>Retrieves a list of doctorfavouritemedications based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of doctorfavouritemedications</returns>
        List<DoctorFavouriteMedication> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc");

        /// <summary>Adds a new doctorfavouritemedication</summary>
        /// <param name="model">The doctorfavouritemedication data to be added</param>
        /// <returns>The result of the operation</returns>
        Guid Create(DoctorFavouriteMedication model);

        /// <summary>Updates a specific doctorfavouritemedication by its primary key</summary>
        /// <param name="id">The primary key of the doctorfavouritemedication</param>
        /// <param name="updatedEntity">The doctorfavouritemedication data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Update(Guid id, DoctorFavouriteMedication updatedEntity);

        /// <summary>Updates a specific doctorfavouritemedication by its primary key</summary>
        /// <param name="id">The primary key of the doctorfavouritemedication</param>
        /// <param name="updatedEntity">The doctorfavouritemedication data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Patch(Guid id, JsonPatchDocument<DoctorFavouriteMedication> updatedEntity);

        /// <summary>Deletes a specific doctorfavouritemedication by its primary key</summary>
        /// <param name="id">The primary key of the doctorfavouritemedication</param>
        /// <returns>The result of the operation</returns>
        bool Delete(Guid id);
    }

    /// <summary>
    /// The doctorfavouritemedicationService responsible for managing doctorfavouritemedication related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting doctorfavouritemedication information.
    /// </remarks>
    public class DoctorFavouriteMedicationService : IDoctorFavouriteMedicationService
    {
        private Demo17SepContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the DoctorFavouriteMedication class.
        /// </summary>
        /// <param name="dbContext">dbContext value to set.</param>
        public DoctorFavouriteMedicationService(Demo17SepContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>Retrieves a specific doctorfavouritemedication by its primary key</summary>
        /// <param name="id">The primary key of the doctorfavouritemedication</param>
        /// <returns>The doctorfavouritemedication data</returns>
        public DoctorFavouriteMedication GetById(Guid id)
        {
            var entityData = _dbContext.DoctorFavouriteMedication.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            return entityData;
        }

        /// <summary>Retrieves a list of doctorfavouritemedications based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of doctorfavouritemedications</returns>/// <exception cref="Exception"></exception>
        public List<DoctorFavouriteMedication> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            var result = GetDoctorFavouriteMedication(filters, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return result;
        }

        /// <summary>Adds a new doctorfavouritemedication</summary>
        /// <param name="model">The doctorfavouritemedication data to be added</param>
        /// <returns>The result of the operation</returns>
        public Guid Create(DoctorFavouriteMedication model)
        {
            model.Id = CreateDoctorFavouriteMedication(model);
            return model.Id;
        }

        /// <summary>Updates a specific doctorfavouritemedication by its primary key</summary>
        /// <param name="id">The primary key of the doctorfavouritemedication</param>
        /// <param name="updatedEntity">The doctorfavouritemedication data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Update(Guid id, DoctorFavouriteMedication updatedEntity)
        {
            UpdateDoctorFavouriteMedication(id, updatedEntity);
            return true;
        }

        /// <summary>Updates a specific doctorfavouritemedication by its primary key</summary>
        /// <param name="id">The primary key of the doctorfavouritemedication</param>
        /// <param name="updatedEntity">The doctorfavouritemedication data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Patch(Guid id, JsonPatchDocument<DoctorFavouriteMedication> updatedEntity)
        {
            PatchDoctorFavouriteMedication(id, updatedEntity);
            return true;
        }

        /// <summary>Deletes a specific doctorfavouritemedication by its primary key</summary>
        /// <param name="id">The primary key of the doctorfavouritemedication</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Delete(Guid id)
        {
            DeleteDoctorFavouriteMedication(id);
            return true;
        }
        #region
        private List<DoctorFavouriteMedication> GetDoctorFavouriteMedication(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            if (pageSize < 1)
            {
                throw new ApplicationException("Page size invalid!");
            }

            if (pageNumber < 1)
            {
                throw new ApplicationException("Page mumber invalid!");
            }

            var query = _dbContext.DoctorFavouriteMedication.IncludeRelated().AsQueryable();
            int skip = (pageNumber - 1) * pageSize;
            var result = FilterService<DoctorFavouriteMedication>.ApplyFilter(query, filters, searchTerm);
            if (!string.IsNullOrEmpty(sortField))
            {
                var parameter = Expression.Parameter(typeof(DoctorFavouriteMedication), "b");
                var property = Expression.Property(parameter, sortField);
                var lambda = Expression.Lambda<Func<DoctorFavouriteMedication, object>>(Expression.Convert(property, typeof(object)), parameter);
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

        private Guid CreateDoctorFavouriteMedication(DoctorFavouriteMedication model)
        {
            _dbContext.DoctorFavouriteMedication.Add(model);
            _dbContext.SaveChanges();
            return model.Id;
        }

        private void UpdateDoctorFavouriteMedication(Guid id, DoctorFavouriteMedication updatedEntity)
        {
            _dbContext.DoctorFavouriteMedication.Update(updatedEntity);
            _dbContext.SaveChanges();
        }

        private bool DeleteDoctorFavouriteMedication(Guid id)
        {
            var entityData = _dbContext.DoctorFavouriteMedication.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            if (entityData == null)
            {
                throw new ApplicationException("No data found!");
            }

            _dbContext.DoctorFavouriteMedication.Remove(entityData);
            _dbContext.SaveChanges();
            return true;
        }

        private void PatchDoctorFavouriteMedication(Guid id, JsonPatchDocument<DoctorFavouriteMedication> updatedEntity)
        {
            if (updatedEntity == null)
            {
                throw new ApplicationException("Patch document is missing!");
            }

            var existingEntity = _dbContext.DoctorFavouriteMedication.FirstOrDefault(t => t.Id == id);
            if (existingEntity == null)
            {
                throw new ApplicationException("No data found!");
            }

            updatedEntity.ApplyTo(existingEntity);
            _dbContext.DoctorFavouriteMedication.Update(existingEntity);
            _dbContext.SaveChanges();
        }
        #endregion
    }
}