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
    /// The invoiceService responsible for managing invoice related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting invoice information.
    /// </remarks>
    public interface IInvoiceService
    {
        /// <summary>Retrieves a specific invoice by its primary key</summary>
        /// <param name="id">The primary key of the invoice</param>
        /// <returns>The invoice data</returns>
        Invoice GetById(Guid id);

        /// <summary>Retrieves a list of invoices based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of invoices</returns>
        List<Invoice> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc");

        /// <summary>Adds a new invoice</summary>
        /// <param name="model">The invoice data to be added</param>
        /// <returns>The result of the operation</returns>
        Guid Create(Invoice model);

        /// <summary>Updates a specific invoice by its primary key</summary>
        /// <param name="id">The primary key of the invoice</param>
        /// <param name="updatedEntity">The invoice data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Update(Guid id, Invoice updatedEntity);

        /// <summary>Updates a specific invoice by its primary key</summary>
        /// <param name="id">The primary key of the invoice</param>
        /// <param name="updatedEntity">The invoice data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Patch(Guid id, JsonPatchDocument<Invoice> updatedEntity);

        /// <summary>Deletes a specific invoice by its primary key</summary>
        /// <param name="id">The primary key of the invoice</param>
        /// <returns>The result of the operation</returns>
        bool Delete(Guid id);
    }

    /// <summary>
    /// The invoiceService responsible for managing invoice related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting invoice information.
    /// </remarks>
    public class InvoiceService : IInvoiceService
    {
        private Demo17SepContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the Invoice class.
        /// </summary>
        /// <param name="dbContext">dbContext value to set.</param>
        public InvoiceService(Demo17SepContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>Retrieves a specific invoice by its primary key</summary>
        /// <param name="id">The primary key of the invoice</param>
        /// <returns>The invoice data</returns>
        public Invoice GetById(Guid id)
        {
            var entityData = _dbContext.Invoice.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            return entityData;
        }

        /// <summary>Retrieves a list of invoices based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of invoices</returns>/// <exception cref="Exception"></exception>
        public List<Invoice> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            var result = GetInvoice(filters, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return result;
        }

        /// <summary>Adds a new invoice</summary>
        /// <param name="model">The invoice data to be added</param>
        /// <returns>The result of the operation</returns>
        public Guid Create(Invoice model)
        {
            model.Id = CreateInvoice(model);
            return model.Id;
        }

        /// <summary>Updates a specific invoice by its primary key</summary>
        /// <param name="id">The primary key of the invoice</param>
        /// <param name="updatedEntity">The invoice data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Update(Guid id, Invoice updatedEntity)
        {
            UpdateInvoice(id, updatedEntity);
            return true;
        }

        /// <summary>Updates a specific invoice by its primary key</summary>
        /// <param name="id">The primary key of the invoice</param>
        /// <param name="updatedEntity">The invoice data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Patch(Guid id, JsonPatchDocument<Invoice> updatedEntity)
        {
            PatchInvoice(id, updatedEntity);
            return true;
        }

        /// <summary>Deletes a specific invoice by its primary key</summary>
        /// <param name="id">The primary key of the invoice</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Delete(Guid id)
        {
            DeleteInvoice(id);
            return true;
        }
        #region
        private List<Invoice> GetInvoice(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            if (pageSize < 1)
            {
                throw new ApplicationException("Page size invalid!");
            }

            if (pageNumber < 1)
            {
                throw new ApplicationException("Page mumber invalid!");
            }

            var query = _dbContext.Invoice.IncludeRelated().AsQueryable();
            int skip = (pageNumber - 1) * pageSize;
            var result = FilterService<Invoice>.ApplyFilter(query, filters, searchTerm);
            if (!string.IsNullOrEmpty(sortField))
            {
                var parameter = Expression.Parameter(typeof(Invoice), "b");
                var property = Expression.Property(parameter, sortField);
                var lambda = Expression.Lambda<Func<Invoice, object>>(Expression.Convert(property, typeof(object)), parameter);
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

        private Guid CreateInvoice(Invoice model)
        {
            _dbContext.Invoice.Add(model);
            _dbContext.SaveChanges();
            return model.Id;
        }

        private void UpdateInvoice(Guid id, Invoice updatedEntity)
        {
            _dbContext.Invoice.Update(updatedEntity);
            _dbContext.SaveChanges();
        }

        private bool DeleteInvoice(Guid id)
        {
            var entityData = _dbContext.Invoice.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            if (entityData == null)
            {
                throw new ApplicationException("No data found!");
            }

            _dbContext.Invoice.Remove(entityData);
            _dbContext.SaveChanges();
            return true;
        }

        private void PatchInvoice(Guid id, JsonPatchDocument<Invoice> updatedEntity)
        {
            if (updatedEntity == null)
            {
                throw new ApplicationException("Patch document is missing!");
            }

            var existingEntity = _dbContext.Invoice.FirstOrDefault(t => t.Id == id);
            if (existingEntity == null)
            {
                throw new ApplicationException("No data found!");
            }

            updatedEntity.ApplyTo(existingEntity);
            _dbContext.Invoice.Update(existingEntity);
            _dbContext.SaveChanges();
        }
        #endregion
    }
}