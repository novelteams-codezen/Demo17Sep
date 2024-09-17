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
    /// The accountsettlementService responsible for managing accountsettlement related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting accountsettlement information.
    /// </remarks>
    public interface IAccountSettlementService
    {
        /// <summary>Retrieves a specific accountsettlement by its primary key</summary>
        /// <param name="id">The primary key of the accountsettlement</param>
        /// <returns>The accountsettlement data</returns>
        AccountSettlement GetById(Guid id);

        /// <summary>Retrieves a list of accountsettlements based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of accountsettlements</returns>
        List<AccountSettlement> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc");

        /// <summary>Adds a new accountsettlement</summary>
        /// <param name="model">The accountsettlement data to be added</param>
        /// <returns>The result of the operation</returns>
        Guid Create(AccountSettlement model);

        /// <summary>Updates a specific accountsettlement by its primary key</summary>
        /// <param name="id">The primary key of the accountsettlement</param>
        /// <param name="updatedEntity">The accountsettlement data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Update(Guid id, AccountSettlement updatedEntity);

        /// <summary>Updates a specific accountsettlement by its primary key</summary>
        /// <param name="id">The primary key of the accountsettlement</param>
        /// <param name="updatedEntity">The accountsettlement data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Patch(Guid id, JsonPatchDocument<AccountSettlement> updatedEntity);

        /// <summary>Deletes a specific accountsettlement by its primary key</summary>
        /// <param name="id">The primary key of the accountsettlement</param>
        /// <returns>The result of the operation</returns>
        bool Delete(Guid id);
    }

    /// <summary>
    /// The accountsettlementService responsible for managing accountsettlement related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting accountsettlement information.
    /// </remarks>
    public class AccountSettlementService : IAccountSettlementService
    {
        private Demo17SepContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the AccountSettlement class.
        /// </summary>
        /// <param name="dbContext">dbContext value to set.</param>
        public AccountSettlementService(Demo17SepContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>Retrieves a specific accountsettlement by its primary key</summary>
        /// <param name="id">The primary key of the accountsettlement</param>
        /// <returns>The accountsettlement data</returns>
        public AccountSettlement GetById(Guid id)
        {
            var entityData = _dbContext.AccountSettlement.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            return entityData;
        }

        /// <summary>Retrieves a list of accountsettlements based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of accountsettlements</returns>/// <exception cref="Exception"></exception>
        public List<AccountSettlement> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            var result = GetAccountSettlement(filters, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return result;
        }

        /// <summary>Adds a new accountsettlement</summary>
        /// <param name="model">The accountsettlement data to be added</param>
        /// <returns>The result of the operation</returns>
        public Guid Create(AccountSettlement model)
        {
            model.Id = CreateAccountSettlement(model);
            return model.Id;
        }

        /// <summary>Updates a specific accountsettlement by its primary key</summary>
        /// <param name="id">The primary key of the accountsettlement</param>
        /// <param name="updatedEntity">The accountsettlement data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Update(Guid id, AccountSettlement updatedEntity)
        {
            UpdateAccountSettlement(id, updatedEntity);
            return true;
        }

        /// <summary>Updates a specific accountsettlement by its primary key</summary>
        /// <param name="id">The primary key of the accountsettlement</param>
        /// <param name="updatedEntity">The accountsettlement data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Patch(Guid id, JsonPatchDocument<AccountSettlement> updatedEntity)
        {
            PatchAccountSettlement(id, updatedEntity);
            return true;
        }

        /// <summary>Deletes a specific accountsettlement by its primary key</summary>
        /// <param name="id">The primary key of the accountsettlement</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Delete(Guid id)
        {
            DeleteAccountSettlement(id);
            return true;
        }
        #region
        private List<AccountSettlement> GetAccountSettlement(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            if (pageSize < 1)
            {
                throw new ApplicationException("Page size invalid!");
            }

            if (pageNumber < 1)
            {
                throw new ApplicationException("Page mumber invalid!");
            }

            var query = _dbContext.AccountSettlement.IncludeRelated().AsQueryable();
            int skip = (pageNumber - 1) * pageSize;
            var result = FilterService<AccountSettlement>.ApplyFilter(query, filters, searchTerm);
            if (!string.IsNullOrEmpty(sortField))
            {
                var parameter = Expression.Parameter(typeof(AccountSettlement), "b");
                var property = Expression.Property(parameter, sortField);
                var lambda = Expression.Lambda<Func<AccountSettlement, object>>(Expression.Convert(property, typeof(object)), parameter);
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

        private Guid CreateAccountSettlement(AccountSettlement model)
        {
            _dbContext.AccountSettlement.Add(model);
            _dbContext.SaveChanges();
            return model.Id;
        }

        private void UpdateAccountSettlement(Guid id, AccountSettlement updatedEntity)
        {
            _dbContext.AccountSettlement.Update(updatedEntity);
            _dbContext.SaveChanges();
        }

        private bool DeleteAccountSettlement(Guid id)
        {
            var entityData = _dbContext.AccountSettlement.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            if (entityData == null)
            {
                throw new ApplicationException("No data found!");
            }

            _dbContext.AccountSettlement.Remove(entityData);
            _dbContext.SaveChanges();
            return true;
        }

        private void PatchAccountSettlement(Guid id, JsonPatchDocument<AccountSettlement> updatedEntity)
        {
            if (updatedEntity == null)
            {
                throw new ApplicationException("Patch document is missing!");
            }

            var existingEntity = _dbContext.AccountSettlement.FirstOrDefault(t => t.Id == id);
            if (existingEntity == null)
            {
                throw new ApplicationException("No data found!");
            }

            updatedEntity.ApplyTo(existingEntity);
            _dbContext.AccountSettlement.Update(existingEntity);
            _dbContext.SaveChanges();
        }
        #endregion
    }
}