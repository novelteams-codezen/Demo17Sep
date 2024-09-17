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
    /// The paymentmodeService responsible for managing paymentmode related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting paymentmode information.
    /// </remarks>
    public interface IPaymentModeService
    {
        /// <summary>Retrieves a specific paymentmode by its primary key</summary>
        /// <param name="id">The primary key of the paymentmode</param>
        /// <returns>The paymentmode data</returns>
        PaymentMode GetById(Guid id);

        /// <summary>Retrieves a list of paymentmodes based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of paymentmodes</returns>
        List<PaymentMode> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc");

        /// <summary>Adds a new paymentmode</summary>
        /// <param name="model">The paymentmode data to be added</param>
        /// <returns>The result of the operation</returns>
        Guid Create(PaymentMode model);

        /// <summary>Updates a specific paymentmode by its primary key</summary>
        /// <param name="id">The primary key of the paymentmode</param>
        /// <param name="updatedEntity">The paymentmode data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Update(Guid id, PaymentMode updatedEntity);

        /// <summary>Updates a specific paymentmode by its primary key</summary>
        /// <param name="id">The primary key of the paymentmode</param>
        /// <param name="updatedEntity">The paymentmode data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Patch(Guid id, JsonPatchDocument<PaymentMode> updatedEntity);

        /// <summary>Deletes a specific paymentmode by its primary key</summary>
        /// <param name="id">The primary key of the paymentmode</param>
        /// <returns>The result of the operation</returns>
        bool Delete(Guid id);
    }

    /// <summary>
    /// The paymentmodeService responsible for managing paymentmode related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting paymentmode information.
    /// </remarks>
    public class PaymentModeService : IPaymentModeService
    {
        private Demo17SepContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the PaymentMode class.
        /// </summary>
        /// <param name="dbContext">dbContext value to set.</param>
        public PaymentModeService(Demo17SepContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>Retrieves a specific paymentmode by its primary key</summary>
        /// <param name="id">The primary key of the paymentmode</param>
        /// <returns>The paymentmode data</returns>
        public PaymentMode GetById(Guid id)
        {
            var entityData = _dbContext.PaymentMode.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            return entityData;
        }

        /// <summary>Retrieves a list of paymentmodes based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of paymentmodes</returns>/// <exception cref="Exception"></exception>
        public List<PaymentMode> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            var result = GetPaymentMode(filters, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return result;
        }

        /// <summary>Adds a new paymentmode</summary>
        /// <param name="model">The paymentmode data to be added</param>
        /// <returns>The result of the operation</returns>
        public Guid Create(PaymentMode model)
        {
            model.Id = CreatePaymentMode(model);
            return model.Id;
        }

        /// <summary>Updates a specific paymentmode by its primary key</summary>
        /// <param name="id">The primary key of the paymentmode</param>
        /// <param name="updatedEntity">The paymentmode data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Update(Guid id, PaymentMode updatedEntity)
        {
            UpdatePaymentMode(id, updatedEntity);
            return true;
        }

        /// <summary>Updates a specific paymentmode by its primary key</summary>
        /// <param name="id">The primary key of the paymentmode</param>
        /// <param name="updatedEntity">The paymentmode data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Patch(Guid id, JsonPatchDocument<PaymentMode> updatedEntity)
        {
            PatchPaymentMode(id, updatedEntity);
            return true;
        }

        /// <summary>Deletes a specific paymentmode by its primary key</summary>
        /// <param name="id">The primary key of the paymentmode</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Delete(Guid id)
        {
            DeletePaymentMode(id);
            return true;
        }
        #region
        private List<PaymentMode> GetPaymentMode(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            if (pageSize < 1)
            {
                throw new ApplicationException("Page size invalid!");
            }

            if (pageNumber < 1)
            {
                throw new ApplicationException("Page mumber invalid!");
            }

            var query = _dbContext.PaymentMode.IncludeRelated().AsQueryable();
            int skip = (pageNumber - 1) * pageSize;
            var result = FilterService<PaymentMode>.ApplyFilter(query, filters, searchTerm);
            if (!string.IsNullOrEmpty(sortField))
            {
                var parameter = Expression.Parameter(typeof(PaymentMode), "b");
                var property = Expression.Property(parameter, sortField);
                var lambda = Expression.Lambda<Func<PaymentMode, object>>(Expression.Convert(property, typeof(object)), parameter);
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

        private Guid CreatePaymentMode(PaymentMode model)
        {
            _dbContext.PaymentMode.Add(model);
            _dbContext.SaveChanges();
            return model.Id;
        }

        private void UpdatePaymentMode(Guid id, PaymentMode updatedEntity)
        {
            _dbContext.PaymentMode.Update(updatedEntity);
            _dbContext.SaveChanges();
        }

        private bool DeletePaymentMode(Guid id)
        {
            var entityData = _dbContext.PaymentMode.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            if (entityData == null)
            {
                throw new ApplicationException("No data found!");
            }

            _dbContext.PaymentMode.Remove(entityData);
            _dbContext.SaveChanges();
            return true;
        }

        private void PatchPaymentMode(Guid id, JsonPatchDocument<PaymentMode> updatedEntity)
        {
            if (updatedEntity == null)
            {
                throw new ApplicationException("Patch document is missing!");
            }

            var existingEntity = _dbContext.PaymentMode.FirstOrDefault(t => t.Id == id);
            if (existingEntity == null)
            {
                throw new ApplicationException("No data found!");
            }

            updatedEntity.ApplyTo(existingEntity);
            _dbContext.PaymentMode.Update(existingEntity);
            _dbContext.SaveChanges();
        }
        #endregion
    }
}