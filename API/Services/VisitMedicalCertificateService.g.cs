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
    /// The visitmedicalcertificateService responsible for managing visitmedicalcertificate related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting visitmedicalcertificate information.
    /// </remarks>
    public interface IVisitMedicalCertificateService
    {
        /// <summary>Retrieves a specific visitmedicalcertificate by its primary key</summary>
        /// <param name="id">The primary key of the visitmedicalcertificate</param>
        /// <returns>The visitmedicalcertificate data</returns>
        VisitMedicalCertificate GetById(Guid id);

        /// <summary>Retrieves a list of visitmedicalcertificates based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of visitmedicalcertificates</returns>
        List<VisitMedicalCertificate> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc");

        /// <summary>Adds a new visitmedicalcertificate</summary>
        /// <param name="model">The visitmedicalcertificate data to be added</param>
        /// <returns>The result of the operation</returns>
        Guid Create(VisitMedicalCertificate model);

        /// <summary>Updates a specific visitmedicalcertificate by its primary key</summary>
        /// <param name="id">The primary key of the visitmedicalcertificate</param>
        /// <param name="updatedEntity">The visitmedicalcertificate data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Update(Guid id, VisitMedicalCertificate updatedEntity);

        /// <summary>Updates a specific visitmedicalcertificate by its primary key</summary>
        /// <param name="id">The primary key of the visitmedicalcertificate</param>
        /// <param name="updatedEntity">The visitmedicalcertificate data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Patch(Guid id, JsonPatchDocument<VisitMedicalCertificate> updatedEntity);

        /// <summary>Deletes a specific visitmedicalcertificate by its primary key</summary>
        /// <param name="id">The primary key of the visitmedicalcertificate</param>
        /// <returns>The result of the operation</returns>
        bool Delete(Guid id);
    }

    /// <summary>
    /// The visitmedicalcertificateService responsible for managing visitmedicalcertificate related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting visitmedicalcertificate information.
    /// </remarks>
    public class VisitMedicalCertificateService : IVisitMedicalCertificateService
    {
        private Demo17SepContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the VisitMedicalCertificate class.
        /// </summary>
        /// <param name="dbContext">dbContext value to set.</param>
        public VisitMedicalCertificateService(Demo17SepContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>Retrieves a specific visitmedicalcertificate by its primary key</summary>
        /// <param name="id">The primary key of the visitmedicalcertificate</param>
        /// <returns>The visitmedicalcertificate data</returns>
        public VisitMedicalCertificate GetById(Guid id)
        {
            var entityData = _dbContext.VisitMedicalCertificate.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            return entityData;
        }

        /// <summary>Retrieves a list of visitmedicalcertificates based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of visitmedicalcertificates</returns>/// <exception cref="Exception"></exception>
        public List<VisitMedicalCertificate> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            var result = GetVisitMedicalCertificate(filters, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return result;
        }

        /// <summary>Adds a new visitmedicalcertificate</summary>
        /// <param name="model">The visitmedicalcertificate data to be added</param>
        /// <returns>The result of the operation</returns>
        public Guid Create(VisitMedicalCertificate model)
        {
            model.Id = CreateVisitMedicalCertificate(model);
            return model.Id;
        }

        /// <summary>Updates a specific visitmedicalcertificate by its primary key</summary>
        /// <param name="id">The primary key of the visitmedicalcertificate</param>
        /// <param name="updatedEntity">The visitmedicalcertificate data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Update(Guid id, VisitMedicalCertificate updatedEntity)
        {
            UpdateVisitMedicalCertificate(id, updatedEntity);
            return true;
        }

        /// <summary>Updates a specific visitmedicalcertificate by its primary key</summary>
        /// <param name="id">The primary key of the visitmedicalcertificate</param>
        /// <param name="updatedEntity">The visitmedicalcertificate data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Patch(Guid id, JsonPatchDocument<VisitMedicalCertificate> updatedEntity)
        {
            PatchVisitMedicalCertificate(id, updatedEntity);
            return true;
        }

        /// <summary>Deletes a specific visitmedicalcertificate by its primary key</summary>
        /// <param name="id">The primary key of the visitmedicalcertificate</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Delete(Guid id)
        {
            DeleteVisitMedicalCertificate(id);
            return true;
        }
        #region
        private List<VisitMedicalCertificate> GetVisitMedicalCertificate(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            if (pageSize < 1)
            {
                throw new ApplicationException("Page size invalid!");
            }

            if (pageNumber < 1)
            {
                throw new ApplicationException("Page mumber invalid!");
            }

            var query = _dbContext.VisitMedicalCertificate.IncludeRelated().AsQueryable();
            int skip = (pageNumber - 1) * pageSize;
            var result = FilterService<VisitMedicalCertificate>.ApplyFilter(query, filters, searchTerm);
            if (!string.IsNullOrEmpty(sortField))
            {
                var parameter = Expression.Parameter(typeof(VisitMedicalCertificate), "b");
                var property = Expression.Property(parameter, sortField);
                var lambda = Expression.Lambda<Func<VisitMedicalCertificate, object>>(Expression.Convert(property, typeof(object)), parameter);
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

        private Guid CreateVisitMedicalCertificate(VisitMedicalCertificate model)
        {
            _dbContext.VisitMedicalCertificate.Add(model);
            _dbContext.SaveChanges();
            return model.Id;
        }

        private void UpdateVisitMedicalCertificate(Guid id, VisitMedicalCertificate updatedEntity)
        {
            _dbContext.VisitMedicalCertificate.Update(updatedEntity);
            _dbContext.SaveChanges();
        }

        private bool DeleteVisitMedicalCertificate(Guid id)
        {
            var entityData = _dbContext.VisitMedicalCertificate.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            if (entityData == null)
            {
                throw new ApplicationException("No data found!");
            }

            _dbContext.VisitMedicalCertificate.Remove(entityData);
            _dbContext.SaveChanges();
            return true;
        }

        private void PatchVisitMedicalCertificate(Guid id, JsonPatchDocument<VisitMedicalCertificate> updatedEntity)
        {
            if (updatedEntity == null)
            {
                throw new ApplicationException("Patch document is missing!");
            }

            var existingEntity = _dbContext.VisitMedicalCertificate.FirstOrDefault(t => t.Id == id);
            if (existingEntity == null)
            {
                throw new ApplicationException("No data found!");
            }

            updatedEntity.ApplyTo(existingEntity);
            _dbContext.VisitMedicalCertificate.Update(existingEntity);
            _dbContext.SaveChanges();
        }
        #endregion
    }
}