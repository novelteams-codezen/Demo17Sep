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
    /// The routeinfoService responsible for managing routeinfo related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting routeinfo information.
    /// </remarks>
    public interface IRouteInfoService
    {
        /// <summary>Retrieves a specific routeinfo by its primary key</summary>
        /// <param name="id">The primary key of the routeinfo</param>
        /// <returns>The routeinfo data</returns>
        RouteInfo GetById(Guid id);

        /// <summary>Retrieves a list of routeinfos based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of routeinfos</returns>
        List<RouteInfo> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc");

        /// <summary>Adds a new routeinfo</summary>
        /// <param name="model">The routeinfo data to be added</param>
        /// <returns>The result of the operation</returns>
        Guid Create(RouteInfo model);

        /// <summary>Updates a specific routeinfo by its primary key</summary>
        /// <param name="id">The primary key of the routeinfo</param>
        /// <param name="updatedEntity">The routeinfo data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Update(Guid id, RouteInfo updatedEntity);

        /// <summary>Updates a specific routeinfo by its primary key</summary>
        /// <param name="id">The primary key of the routeinfo</param>
        /// <param name="updatedEntity">The routeinfo data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Patch(Guid id, JsonPatchDocument<RouteInfo> updatedEntity);

        /// <summary>Deletes a specific routeinfo by its primary key</summary>
        /// <param name="id">The primary key of the routeinfo</param>
        /// <returns>The result of the operation</returns>
        bool Delete(Guid id);
    }

    /// <summary>
    /// The routeinfoService responsible for managing routeinfo related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting routeinfo information.
    /// </remarks>
    public class RouteInfoService : IRouteInfoService
    {
        private Demo17SepContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the RouteInfo class.
        /// </summary>
        /// <param name="dbContext">dbContext value to set.</param>
        public RouteInfoService(Demo17SepContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>Retrieves a specific routeinfo by its primary key</summary>
        /// <param name="id">The primary key of the routeinfo</param>
        /// <returns>The routeinfo data</returns>
        public RouteInfo GetById(Guid id)
        {
            var entityData = _dbContext.RouteInfo.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            return entityData;
        }

        /// <summary>Retrieves a list of routeinfos based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of routeinfos</returns>/// <exception cref="Exception"></exception>
        public List<RouteInfo> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            var result = GetRouteInfo(filters, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return result;
        }

        /// <summary>Adds a new routeinfo</summary>
        /// <param name="model">The routeinfo data to be added</param>
        /// <returns>The result of the operation</returns>
        public Guid Create(RouteInfo model)
        {
            model.Id = CreateRouteInfo(model);
            return model.Id;
        }

        /// <summary>Updates a specific routeinfo by its primary key</summary>
        /// <param name="id">The primary key of the routeinfo</param>
        /// <param name="updatedEntity">The routeinfo data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Update(Guid id, RouteInfo updatedEntity)
        {
            UpdateRouteInfo(id, updatedEntity);
            return true;
        }

        /// <summary>Updates a specific routeinfo by its primary key</summary>
        /// <param name="id">The primary key of the routeinfo</param>
        /// <param name="updatedEntity">The routeinfo data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Patch(Guid id, JsonPatchDocument<RouteInfo> updatedEntity)
        {
            PatchRouteInfo(id, updatedEntity);
            return true;
        }

        /// <summary>Deletes a specific routeinfo by its primary key</summary>
        /// <param name="id">The primary key of the routeinfo</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Delete(Guid id)
        {
            DeleteRouteInfo(id);
            return true;
        }
        #region
        private List<RouteInfo> GetRouteInfo(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            if (pageSize < 1)
            {
                throw new ApplicationException("Page size invalid!");
            }

            if (pageNumber < 1)
            {
                throw new ApplicationException("Page mumber invalid!");
            }

            var query = _dbContext.RouteInfo.IncludeRelated().AsQueryable();
            int skip = (pageNumber - 1) * pageSize;
            var result = FilterService<RouteInfo>.ApplyFilter(query, filters, searchTerm);
            if (!string.IsNullOrEmpty(sortField))
            {
                var parameter = Expression.Parameter(typeof(RouteInfo), "b");
                var property = Expression.Property(parameter, sortField);
                var lambda = Expression.Lambda<Func<RouteInfo, object>>(Expression.Convert(property, typeof(object)), parameter);
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

        private Guid CreateRouteInfo(RouteInfo model)
        {
            _dbContext.RouteInfo.Add(model);
            _dbContext.SaveChanges();
            return model.Id;
        }

        private void UpdateRouteInfo(Guid id, RouteInfo updatedEntity)
        {
            _dbContext.RouteInfo.Update(updatedEntity);
            _dbContext.SaveChanges();
        }

        private bool DeleteRouteInfo(Guid id)
        {
            var entityData = _dbContext.RouteInfo.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            if (entityData == null)
            {
                throw new ApplicationException("No data found!");
            }

            _dbContext.RouteInfo.Remove(entityData);
            _dbContext.SaveChanges();
            return true;
        }

        private void PatchRouteInfo(Guid id, JsonPatchDocument<RouteInfo> updatedEntity)
        {
            if (updatedEntity == null)
            {
                throw new ApplicationException("Patch document is missing!");
            }

            var existingEntity = _dbContext.RouteInfo.FirstOrDefault(t => t.Id == id);
            if (existingEntity == null)
            {
                throw new ApplicationException("No data found!");
            }

            updatedEntity.ApplyTo(existingEntity);
            _dbContext.RouteInfo.Update(existingEntity);
            _dbContext.SaveChanges();
        }
        #endregion
    }
}