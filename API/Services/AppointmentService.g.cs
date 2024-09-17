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
    /// The appointmentService responsible for managing appointment related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting appointment information.
    /// </remarks>
    public interface IAppointmentService
    {
        /// <summary>Retrieves a specific appointment by its primary key</summary>
        /// <param name="id">The primary key of the appointment</param>
        /// <returns>The appointment data</returns>
        Appointment GetById(Guid id);

        /// <summary>Retrieves a list of appointments based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of appointments</returns>
        List<Appointment> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc");

        /// <summary>Adds a new appointment</summary>
        /// <param name="model">The appointment data to be added</param>
        /// <returns>The result of the operation</returns>
        Guid Create(Appointment model);

        /// <summary>Updates a specific appointment by its primary key</summary>
        /// <param name="id">The primary key of the appointment</param>
        /// <param name="updatedEntity">The appointment data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Update(Guid id, Appointment updatedEntity);

        /// <summary>Updates a specific appointment by its primary key</summary>
        /// <param name="id">The primary key of the appointment</param>
        /// <param name="updatedEntity">The appointment data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Patch(Guid id, JsonPatchDocument<Appointment> updatedEntity);

        /// <summary>Deletes a specific appointment by its primary key</summary>
        /// <param name="id">The primary key of the appointment</param>
        /// <returns>The result of the operation</returns>
        bool Delete(Guid id);
    }

    /// <summary>
    /// The appointmentService responsible for managing appointment related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting appointment information.
    /// </remarks>
    public class AppointmentService : IAppointmentService
    {
        private Demo17SepContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the Appointment class.
        /// </summary>
        /// <param name="dbContext">dbContext value to set.</param>
        public AppointmentService(Demo17SepContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>Retrieves a specific appointment by its primary key</summary>
        /// <param name="id">The primary key of the appointment</param>
        /// <returns>The appointment data</returns>
        public Appointment GetById(Guid id)
        {
            var entityData = _dbContext.Appointment.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            return entityData;
        }

        /// <summary>Retrieves a list of appointments based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of appointments</returns>/// <exception cref="Exception"></exception>
        public List<Appointment> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            var result = GetAppointment(filters, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return result;
        }

        /// <summary>Adds a new appointment</summary>
        /// <param name="model">The appointment data to be added</param>
        /// <returns>The result of the operation</returns>
        public Guid Create(Appointment model)
        {
            model.Id = CreateAppointment(model);
            return model.Id;
        }

        /// <summary>Updates a specific appointment by its primary key</summary>
        /// <param name="id">The primary key of the appointment</param>
        /// <param name="updatedEntity">The appointment data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Update(Guid id, Appointment updatedEntity)
        {
            UpdateAppointment(id, updatedEntity);
            return true;
        }

        /// <summary>Updates a specific appointment by its primary key</summary>
        /// <param name="id">The primary key of the appointment</param>
        /// <param name="updatedEntity">The appointment data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Patch(Guid id, JsonPatchDocument<Appointment> updatedEntity)
        {
            PatchAppointment(id, updatedEntity);
            return true;
        }

        /// <summary>Deletes a specific appointment by its primary key</summary>
        /// <param name="id">The primary key of the appointment</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Delete(Guid id)
        {
            DeleteAppointment(id);
            return true;
        }
        #region
        private List<Appointment> GetAppointment(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            if (pageSize < 1)
            {
                throw new ApplicationException("Page size invalid!");
            }

            if (pageNumber < 1)
            {
                throw new ApplicationException("Page mumber invalid!");
            }

            var query = _dbContext.Appointment.IncludeRelated().AsQueryable();
            int skip = (pageNumber - 1) * pageSize;
            var result = FilterService<Appointment>.ApplyFilter(query, filters, searchTerm);
            if (!string.IsNullOrEmpty(sortField))
            {
                var parameter = Expression.Parameter(typeof(Appointment), "b");
                var property = Expression.Property(parameter, sortField);
                var lambda = Expression.Lambda<Func<Appointment, object>>(Expression.Convert(property, typeof(object)), parameter);
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

        private Guid CreateAppointment(Appointment model)
        {
            _dbContext.Appointment.Add(model);
            _dbContext.SaveChanges();
            return model.Id;
        }

        private void UpdateAppointment(Guid id, Appointment updatedEntity)
        {
            _dbContext.Appointment.Update(updatedEntity);
            _dbContext.SaveChanges();
        }

        private bool DeleteAppointment(Guid id)
        {
            var entityData = _dbContext.Appointment.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            if (entityData == null)
            {
                throw new ApplicationException("No data found!");
            }

            _dbContext.Appointment.Remove(entityData);
            _dbContext.SaveChanges();
            return true;
        }

        private void PatchAppointment(Guid id, JsonPatchDocument<Appointment> updatedEntity)
        {
            if (updatedEntity == null)
            {
                throw new ApplicationException("Patch document is missing!");
            }

            var existingEntity = _dbContext.Appointment.FirstOrDefault(t => t.Id == id);
            if (existingEntity == null)
            {
                throw new ApplicationException("No data found!");
            }

            updatedEntity.ApplyTo(existingEntity);
            _dbContext.Appointment.Update(existingEntity);
            _dbContext.SaveChanges();
        }
        #endregion
    }
}