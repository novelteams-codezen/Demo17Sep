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
    /// The invoicelineService responsible for managing invoiceline related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting invoiceline information.
    /// </remarks>
    public interface IInvoiceLineService
    {
        /// <summary>Retrieves a specific invoiceline by its primary key</summary>
        /// <param name="id">The primary key of the invoiceline</param>
        /// <returns>The invoiceline data</returns>
        InvoiceLine GetById(Guid id);

        /// <summary>Retrieves a list of invoicelines based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of invoicelines</returns>
        List<InvoiceLine> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc");

        /// <summary>Adds a new invoiceline</summary>
        /// <param name="model">The invoiceline data to be added</param>
        /// <returns>The result of the operation</returns>
        Guid Create(InvoiceLine model);

        /// <summary>Updates a specific invoiceline by its primary key</summary>
        /// <param name="id">The primary key of the invoiceline</param>
        /// <param name="updatedEntity">The invoiceline data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Update(Guid id, InvoiceLine updatedEntity);

        /// <summary>Updates a specific invoiceline by its primary key</summary>
        /// <param name="id">The primary key of the invoiceline</param>
        /// <param name="updatedEntity">The invoiceline data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Patch(Guid id, JsonPatchDocument<InvoiceLine> updatedEntity);

        /// <summary>Deletes a specific invoiceline by its primary key</summary>
        /// <param name="id">The primary key of the invoiceline</param>
        /// <returns>The result of the operation</returns>
        bool Delete(Guid id);
    }

    /// <summary>
    /// The invoicelineService responsible for managing invoiceline related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting invoiceline information.
    /// </remarks>
    public class InvoiceLineService : IInvoiceLineService
    {
        private Demo17SepContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the InvoiceLine class.
        /// </summary>
        /// <param name="dbContext">dbContext value to set.</param>
        public InvoiceLineService(Demo17SepContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>Retrieves a specific invoiceline by its primary key</summary>
        /// <param name="id">The primary key of the invoiceline</param>
        /// <returns>The invoiceline data</returns>
        public InvoiceLine GetById(Guid id)
        {
            var entityData = _dbContext.InvoiceLine.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            return entityData;
        }

        /// <summary>Retrieves a list of invoicelines based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of invoicelines</returns>/// <exception cref="Exception"></exception>
        public List<InvoiceLine> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            var result = GetInvoiceLine(filters, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return result;
        }

        /// <summary>Adds a new invoiceline</summary>
        /// <param name="model">The invoiceline data to be added</param>
        /// <returns>The result of the operation</returns>
        public Guid Create(InvoiceLine model)
        {
            model.Id = CreateInvoiceLine(model);
            return model.Id;
        }

        /// <summary>Updates a specific invoiceline by its primary key</summary>
        /// <param name="id">The primary key of the invoiceline</param>
        /// <param name="updatedEntity">The invoiceline data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Update(Guid id, InvoiceLine updatedEntity)
        {
            UpdateInvoiceLine(id, updatedEntity);
            return true;
        }

        /// <summary>Updates a specific invoiceline by its primary key</summary>
        /// <param name="id">The primary key of the invoiceline</param>
        /// <param name="updatedEntity">The invoiceline data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Patch(Guid id, JsonPatchDocument<InvoiceLine> updatedEntity)
        {
            PatchInvoiceLine(id, updatedEntity);
            return true;
        }

        /// <summary>Deletes a specific invoiceline by its primary key</summary>
        /// <param name="id">The primary key of the invoiceline</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Delete(Guid id)
        {
            DeleteInvoiceLine(id);
            return true;
        }
        #region
        private List<InvoiceLine> GetInvoiceLine(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            if (pageSize < 1)
            {
                throw new ApplicationException("Page size invalid!");
            }

            if (pageNumber < 1)
            {
                throw new ApplicationException("Page mumber invalid!");
            }

            var query = _dbContext.InvoiceLine.IncludeRelated().AsQueryable();
            int skip = (pageNumber - 1) * pageSize;
            var result = FilterService<InvoiceLine>.ApplyFilter(query, filters, searchTerm);
            if (!string.IsNullOrEmpty(sortField))
            {
                var parameter = Expression.Parameter(typeof(InvoiceLine), "b");
                var property = Expression.Property(parameter, sortField);
                var lambda = Expression.Lambda<Func<InvoiceLine, object>>(Expression.Convert(property, typeof(object)), parameter);
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

        private Guid CreateInvoiceLine(InvoiceLine model)
        {
            _dbContext.InvoiceLine.Add(model);
            _dbContext.SaveChanges();
            return model.Id;
        }

        private void UpdateInvoiceLine(Guid id, InvoiceLine updatedEntity)
        {
            _dbContext.InvoiceLine.Update(updatedEntity);
            _dbContext.SaveChanges();
        }

        private bool DeleteInvoiceLine(Guid id)
        {
            var entityData = _dbContext.InvoiceLine.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            if (entityData == null)
            {
                throw new ApplicationException("No data found!");
            }

            _dbContext.InvoiceLine.Remove(entityData);
            _dbContext.SaveChanges();
            return true;
        }

        private void PatchInvoiceLine(Guid id, JsonPatchDocument<InvoiceLine> updatedEntity)
        {
            if (updatedEntity == null)
            {
                throw new ApplicationException("Patch document is missing!");
            }

            var existingEntity = _dbContext.InvoiceLine.FirstOrDefault(t => t.Id == id);
            if (existingEntity == null)
            {
                throw new ApplicationException("No data found!");
            }

            updatedEntity.ApplyTo(existingEntity);
            _dbContext.InvoiceLine.Update(existingEntity);
            _dbContext.SaveChanges();
        }
        #endregion
    }
}