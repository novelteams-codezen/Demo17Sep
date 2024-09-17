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
    /// The notificationService responsible for managing notification related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting notification information.
    /// </remarks>
    public interface INotificationService
    {
        /// <summary>Retrieves a specific notification by its primary key</summary>
        /// <param name="id">The primary key of the notification</param>
        /// <returns>The notification data</returns>
        Notification GetById(Guid id);

        /// <summary>Retrieves a list of notifications based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of notifications</returns>
        List<Notification> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc");

        /// <summary>Adds a new notification</summary>
        /// <param name="model">The notification data to be added</param>
        /// <returns>The result of the operation</returns>
        Guid Create(Notification model);

        /// <summary>Updates a specific notification by its primary key</summary>
        /// <param name="id">The primary key of the notification</param>
        /// <param name="updatedEntity">The notification data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Update(Guid id, Notification updatedEntity);

        /// <summary>Updates a specific notification by its primary key</summary>
        /// <param name="id">The primary key of the notification</param>
        /// <param name="updatedEntity">The notification data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Patch(Guid id, JsonPatchDocument<Notification> updatedEntity);

        /// <summary>Deletes a specific notification by its primary key</summary>
        /// <param name="id">The primary key of the notification</param>
        /// <returns>The result of the operation</returns>
        bool Delete(Guid id);
    }

    /// <summary>
    /// The notificationService responsible for managing notification related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting notification information.
    /// </remarks>
    public class NotificationService : INotificationService
    {
        private Demo17SepContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the Notification class.
        /// </summary>
        /// <param name="dbContext">dbContext value to set.</param>
        public NotificationService(Demo17SepContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>Retrieves a specific notification by its primary key</summary>
        /// <param name="id">The primary key of the notification</param>
        /// <returns>The notification data</returns>
        public Notification GetById(Guid id)
        {
            var entityData = _dbContext.Notification.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            return entityData;
        }

        /// <summary>Retrieves a list of notifications based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of notifications</returns>/// <exception cref="Exception"></exception>
        public List<Notification> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            var result = GetNotification(filters, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return result;
        }

        /// <summary>Adds a new notification</summary>
        /// <param name="model">The notification data to be added</param>
        /// <returns>The result of the operation</returns>
        public Guid Create(Notification model)
        {
            model.Id = CreateNotification(model);
            return model.Id;
        }

        /// <summary>Updates a specific notification by its primary key</summary>
        /// <param name="id">The primary key of the notification</param>
        /// <param name="updatedEntity">The notification data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Update(Guid id, Notification updatedEntity)
        {
            UpdateNotification(id, updatedEntity);
            return true;
        }

        /// <summary>Updates a specific notification by its primary key</summary>
        /// <param name="id">The primary key of the notification</param>
        /// <param name="updatedEntity">The notification data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Patch(Guid id, JsonPatchDocument<Notification> updatedEntity)
        {
            PatchNotification(id, updatedEntity);
            return true;
        }

        /// <summary>Deletes a specific notification by its primary key</summary>
        /// <param name="id">The primary key of the notification</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Delete(Guid id)
        {
            DeleteNotification(id);
            return true;
        }
        #region
        private List<Notification> GetNotification(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            if (pageSize < 1)
            {
                throw new ApplicationException("Page size invalid!");
            }

            if (pageNumber < 1)
            {
                throw new ApplicationException("Page mumber invalid!");
            }

            var query = _dbContext.Notification.IncludeRelated().AsQueryable();
            int skip = (pageNumber - 1) * pageSize;
            var result = FilterService<Notification>.ApplyFilter(query, filters, searchTerm);
            if (!string.IsNullOrEmpty(sortField))
            {
                var parameter = Expression.Parameter(typeof(Notification), "b");
                var property = Expression.Property(parameter, sortField);
                var lambda = Expression.Lambda<Func<Notification, object>>(Expression.Convert(property, typeof(object)), parameter);
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

        private Guid CreateNotification(Notification model)
        {
            _dbContext.Notification.Add(model);
            _dbContext.SaveChanges();
            return model.Id;
        }

        private void UpdateNotification(Guid id, Notification updatedEntity)
        {
            _dbContext.Notification.Update(updatedEntity);
            _dbContext.SaveChanges();
        }

        private bool DeleteNotification(Guid id)
        {
            var entityData = _dbContext.Notification.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            if (entityData == null)
            {
                throw new ApplicationException("No data found!");
            }

            _dbContext.Notification.Remove(entityData);
            _dbContext.SaveChanges();
            return true;
        }

        private void PatchNotification(Guid id, JsonPatchDocument<Notification> updatedEntity)
        {
            if (updatedEntity == null)
            {
                throw new ApplicationException("Patch document is missing!");
            }

            var existingEntity = _dbContext.Notification.FirstOrDefault(t => t.Id == id);
            if (existingEntity == null)
            {
                throw new ApplicationException("No data found!");
            }

            updatedEntity.ApplyTo(existingEntity);
            _dbContext.Notification.Update(existingEntity);
            _dbContext.SaveChanges();
        }
        #endregion
    }
}