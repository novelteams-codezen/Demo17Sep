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
    /// The invoicefileService responsible for managing invoicefile related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting invoicefile information.
    /// </remarks>
    public interface IInvoiceFileService
    {
        /// <summary>Retrieves a specific invoicefile by its primary key</summary>
        /// <param name="id">The primary key of the invoicefile</param>
        /// <returns>The invoicefile data</returns>
        InvoiceFile GetById(Guid id);

        /// <summary>Retrieves a list of invoicefiles based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of invoicefiles</returns>
        List<InvoiceFile> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc");

        /// <summary>Adds a new invoicefile</summary>
        /// <param name="model">The invoicefile data to be added</param>
        /// <returns>The result of the operation</returns>
        Guid Create(InvoiceFile model);

        /// <summary>Updates a specific invoicefile by its primary key</summary>
        /// <param name="id">The primary key of the invoicefile</param>
        /// <param name="updatedEntity">The invoicefile data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Update(Guid id, InvoiceFile updatedEntity);

        /// <summary>Updates a specific invoicefile by its primary key</summary>
        /// <param name="id">The primary key of the invoicefile</param>
        /// <param name="updatedEntity">The invoicefile data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Patch(Guid id, JsonPatchDocument<InvoiceFile> updatedEntity);

        /// <summary>Deletes a specific invoicefile by its primary key</summary>
        /// <param name="id">The primary key of the invoicefile</param>
        /// <returns>The result of the operation</returns>
        bool Delete(Guid id);
    }

    /// <summary>
    /// The invoicefileService responsible for managing invoicefile related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting invoicefile information.
    /// </remarks>
    public class InvoiceFileService : IInvoiceFileService
    {
        private Demo17SepContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the InvoiceFile class.
        /// </summary>
        /// <param name="dbContext">dbContext value to set.</param>
        public InvoiceFileService(Demo17SepContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>Retrieves a specific invoicefile by its primary key</summary>
        /// <param name="id">The primary key of the invoicefile</param>
        /// <returns>The invoicefile data</returns>
        public InvoiceFile GetById(Guid id)
        {
            var entityData = _dbContext.InvoiceFile.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            return entityData;
        }

        /// <summary>Retrieves a list of invoicefiles based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of invoicefiles</returns>/// <exception cref="Exception"></exception>
        public List<InvoiceFile> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            var result = GetInvoiceFile(filters, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return result;
        }

        /// <summary>Adds a new invoicefile</summary>
        /// <param name="model">The invoicefile data to be added</param>
        /// <returns>The result of the operation</returns>
        public Guid Create(InvoiceFile model)
        {
            model.Id = CreateInvoiceFile(model);
            return model.Id;
        }

        /// <summary>Updates a specific invoicefile by its primary key</summary>
        /// <param name="id">The primary key of the invoicefile</param>
        /// <param name="updatedEntity">The invoicefile data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Update(Guid id, InvoiceFile updatedEntity)
        {
            UpdateInvoiceFile(id, updatedEntity);
            return true;
        }

        /// <summary>Updates a specific invoicefile by its primary key</summary>
        /// <param name="id">The primary key of the invoicefile</param>
        /// <param name="updatedEntity">The invoicefile data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Patch(Guid id, JsonPatchDocument<InvoiceFile> updatedEntity)
        {
            PatchInvoiceFile(id, updatedEntity);
            return true;
        }

        /// <summary>Deletes a specific invoicefile by its primary key</summary>
        /// <param name="id">The primary key of the invoicefile</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Delete(Guid id)
        {
            DeleteInvoiceFile(id);
            return true;
        }
        #region
        private List<InvoiceFile> GetInvoiceFile(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            if (pageSize < 1)
            {
                throw new ApplicationException("Page size invalid!");
            }

            if (pageNumber < 1)
            {
                throw new ApplicationException("Page mumber invalid!");
            }

            var query = _dbContext.InvoiceFile.IncludeRelated().AsQueryable();
            int skip = (pageNumber - 1) * pageSize;
            var result = FilterService<InvoiceFile>.ApplyFilter(query, filters, searchTerm);
            if (!string.IsNullOrEmpty(sortField))
            {
                var parameter = Expression.Parameter(typeof(InvoiceFile), "b");
                var property = Expression.Property(parameter, sortField);
                var lambda = Expression.Lambda<Func<InvoiceFile, object>>(Expression.Convert(property, typeof(object)), parameter);
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

        private Guid CreateInvoiceFile(InvoiceFile model)
        {
            _dbContext.InvoiceFile.Add(model);
            _dbContext.SaveChanges();
            return model.Id;
        }

        private void UpdateInvoiceFile(Guid id, InvoiceFile updatedEntity)
        {
            _dbContext.InvoiceFile.Update(updatedEntity);
            _dbContext.SaveChanges();
        }

        private bool DeleteInvoiceFile(Guid id)
        {
            var entityData = _dbContext.InvoiceFile.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            if (entityData == null)
            {
                throw new ApplicationException("No data found!");
            }

            _dbContext.InvoiceFile.Remove(entityData);
            _dbContext.SaveChanges();
            return true;
        }

        private void PatchInvoiceFile(Guid id, JsonPatchDocument<InvoiceFile> updatedEntity)
        {
            if (updatedEntity == null)
            {
                throw new ApplicationException("Patch document is missing!");
            }

            var existingEntity = _dbContext.InvoiceFile.FirstOrDefault(t => t.Id == id);
            if (existingEntity == null)
            {
                throw new ApplicationException("No data found!");
            }

            updatedEntity.ApplyTo(existingEntity);
            _dbContext.InvoiceFile.Update(existingEntity);
            _dbContext.SaveChanges();
        }
        #endregion
    }
}