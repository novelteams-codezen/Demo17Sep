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
    /// The appointmentreminderlogService responsible for managing appointmentreminderlog related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting appointmentreminderlog information.
    /// </remarks>
    public interface IAppointmentReminderLogService
    {
        /// <summary>Retrieves a specific appointmentreminderlog by its primary key</summary>
        /// <param name="id">The primary key of the appointmentreminderlog</param>
        /// <returns>The appointmentreminderlog data</returns>
        AppointmentReminderLog GetById(Guid id);

        /// <summary>Retrieves a list of appointmentreminderlogs based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of appointmentreminderlogs</returns>
        List<AppointmentReminderLog> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc");

        /// <summary>Adds a new appointmentreminderlog</summary>
        /// <param name="model">The appointmentreminderlog data to be added</param>
        /// <returns>The result of the operation</returns>
        Guid Create(AppointmentReminderLog model);

        /// <summary>Updates a specific appointmentreminderlog by its primary key</summary>
        /// <param name="id">The primary key of the appointmentreminderlog</param>
        /// <param name="updatedEntity">The appointmentreminderlog data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Update(Guid id, AppointmentReminderLog updatedEntity);

        /// <summary>Updates a specific appointmentreminderlog by its primary key</summary>
        /// <param name="id">The primary key of the appointmentreminderlog</param>
        /// <param name="updatedEntity">The appointmentreminderlog data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Patch(Guid id, JsonPatchDocument<AppointmentReminderLog> updatedEntity);

        /// <summary>Deletes a specific appointmentreminderlog by its primary key</summary>
        /// <param name="id">The primary key of the appointmentreminderlog</param>
        /// <returns>The result of the operation</returns>
        bool Delete(Guid id);
    }

    /// <summary>
    /// The appointmentreminderlogService responsible for managing appointmentreminderlog related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting appointmentreminderlog information.
    /// </remarks>
    public class AppointmentReminderLogService : IAppointmentReminderLogService
    {
        private Demo17SepContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the AppointmentReminderLog class.
        /// </summary>
        /// <param name="dbContext">dbContext value to set.</param>
        public AppointmentReminderLogService(Demo17SepContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>Retrieves a specific appointmentreminderlog by its primary key</summary>
        /// <param name="id">The primary key of the appointmentreminderlog</param>
        /// <returns>The appointmentreminderlog data</returns>
        public AppointmentReminderLog GetById(Guid id)
        {
            var entityData = _dbContext.AppointmentReminderLog.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            return entityData;
        }

        /// <summary>Retrieves a list of appointmentreminderlogs based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of appointmentreminderlogs</returns>/// <exception cref="Exception"></exception>
        public List<AppointmentReminderLog> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            var result = GetAppointmentReminderLog(filters, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return result;
        }

        /// <summary>Adds a new appointmentreminderlog</summary>
        /// <param name="model">The appointmentreminderlog data to be added</param>
        /// <returns>The result of the operation</returns>
        public Guid Create(AppointmentReminderLog model)
        {
            model.Id = CreateAppointmentReminderLog(model);
            return model.Id;
        }

        /// <summary>Updates a specific appointmentreminderlog by its primary key</summary>
        /// <param name="id">The primary key of the appointmentreminderlog</param>
        /// <param name="updatedEntity">The appointmentreminderlog data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Update(Guid id, AppointmentReminderLog updatedEntity)
        {
            UpdateAppointmentReminderLog(id, updatedEntity);
            return true;
        }

        /// <summary>Updates a specific appointmentreminderlog by its primary key</summary>
        /// <param name="id">The primary key of the appointmentreminderlog</param>
        /// <param name="updatedEntity">The appointmentreminderlog data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Patch(Guid id, JsonPatchDocument<AppointmentReminderLog> updatedEntity)
        {
            PatchAppointmentReminderLog(id, updatedEntity);
            return true;
        }

        /// <summary>Deletes a specific appointmentreminderlog by its primary key</summary>
        /// <param name="id">The primary key of the appointmentreminderlog</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Delete(Guid id)
        {
            DeleteAppointmentReminderLog(id);
            return true;
        }
        #region
        private List<AppointmentReminderLog> GetAppointmentReminderLog(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            if (pageSize < 1)
            {
                throw new ApplicationException("Page size invalid!");
            }

            if (pageNumber < 1)
            {
                throw new ApplicationException("Page mumber invalid!");
            }

            var query = _dbContext.AppointmentReminderLog.IncludeRelated().AsQueryable();
            int skip = (pageNumber - 1) * pageSize;
            var result = FilterService<AppointmentReminderLog>.ApplyFilter(query, filters, searchTerm);
            if (!string.IsNullOrEmpty(sortField))
            {
                var parameter = Expression.Parameter(typeof(AppointmentReminderLog), "b");
                var property = Expression.Property(parameter, sortField);
                var lambda = Expression.Lambda<Func<AppointmentReminderLog, object>>(Expression.Convert(property, typeof(object)), parameter);
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

        private Guid CreateAppointmentReminderLog(AppointmentReminderLog model)
        {
            _dbContext.AppointmentReminderLog.Add(model);
            _dbContext.SaveChanges();
            return model.Id;
        }

        private void UpdateAppointmentReminderLog(Guid id, AppointmentReminderLog updatedEntity)
        {
            _dbContext.AppointmentReminderLog.Update(updatedEntity);
            _dbContext.SaveChanges();
        }

        private bool DeleteAppointmentReminderLog(Guid id)
        {
            var entityData = _dbContext.AppointmentReminderLog.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            if (entityData == null)
            {
                throw new ApplicationException("No data found!");
            }

            _dbContext.AppointmentReminderLog.Remove(entityData);
            _dbContext.SaveChanges();
            return true;
        }

        private void PatchAppointmentReminderLog(Guid id, JsonPatchDocument<AppointmentReminderLog> updatedEntity)
        {
            if (updatedEntity == null)
            {
                throw new ApplicationException("Patch document is missing!");
            }

            var existingEntity = _dbContext.AppointmentReminderLog.FirstOrDefault(t => t.Id == id);
            if (existingEntity == null)
            {
                throw new ApplicationException("No data found!");
            }

            updatedEntity.ApplyTo(existingEntity);
            _dbContext.AppointmentReminderLog.Update(existingEntity);
            _dbContext.SaveChanges();
        }
        #endregion
    }
}