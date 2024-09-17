using Microsoft.AspNetCore.Mvc;
using Demo17Sep.Models;
using Demo17Sep.Services;
using Demo17Sep.Entities;
using Demo17Sep.Filter;
using Demo17Sep.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;

namespace Demo17Sep.Controllers
{
    /// <summary>
    /// Controller responsible for managing pricelistversion related operations.
    /// </summary>
    /// <remarks>
    /// This Controller provides endpoints for adding, retrieving, updating, and deleting pricelistversion information.
    /// </remarks>
    [Route("api/pricelistversion")]
    [Authorize]
    public class PriceListVersionController : ControllerBase
    {
        private readonly IPriceListVersionService _priceListVersionService;

        /// <summary>
        /// Initializes a new instance of the PriceListVersionController class with the specified context.
        /// </summary>
        /// <param name="ipricelistversionservice">The ipricelistversionservice to be used by the controller.</param>
        public PriceListVersionController(IPriceListVersionService ipricelistversionservice)
        {
            _priceListVersionService = ipricelistversionservice;
        }

        /// <summary>Adds a new pricelistversion</summary>
        /// <param name="model">The pricelistversion data to be added</param>
        /// <returns>The result of the operation</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        [UserAuthorize("PriceListVersion",Entitlements.Create)]
        public IActionResult Post([FromBody] PriceListVersion model)
        {
            var id = _priceListVersionService.Create(model);
            return Ok(new { id });
        }

        /// <summary>Retrieves a list of pricelistversions based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of pricelistversions</returns>
        [HttpGet]
        [UserAuthorize("PriceListVersion",Entitlements.Read)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public IActionResult Get([FromQuery] string filters, string searchTerm, int pageNumber = 1, int pageSize = 10, string sortField = null, string sortOrder = "asc")
        {
            List<FilterCriteria> filterCriteria = null;
            if (pageSize < 1)
            {
                return BadRequest("Page size invalid.");
            }

            if (pageNumber < 1)
            {
                return BadRequest("Page mumber invalid.");
            }

            if (!string.IsNullOrEmpty(filters))
            {
                filterCriteria = JsonHelper.Deserialize<List<FilterCriteria>>(filters);
            }

            var result = _priceListVersionService.Get(filterCriteria, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return Ok(result);
        }

        /// <summary>Retrieves a specific pricelistversion by its primary key</summary>
        /// <param name="id">The primary key of the pricelistversion</param>
        /// <returns>The pricelistversion data</returns>
        [HttpGet]
        [Route("{id:Guid}")]
        [UserAuthorize("PriceListVersion",Entitlements.Read)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        public IActionResult GetById([FromRoute] Guid id)
        {
            var result = _priceListVersionService.GetById(id);
            return Ok(result);
        }

        /// <summary>Deletes a specific pricelistversion by its primary key</summary>
        /// <param name="id">The primary key of the pricelistversion</param>
        /// <returns>The result of the operation</returns>
        [HttpDelete]
        [UserAuthorize("PriceListVersion",Entitlements.Delete)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        [Route("{id:Guid}")]
        public IActionResult DeleteById([FromRoute] Guid id)
        {
            var status = _priceListVersionService.Delete(id);
            return Ok(new { status });
        }

        /// <summary>Updates a specific pricelistversion by its primary key</summary>
        /// <param name="id">The primary key of the pricelistversion</param>
        /// <param name="updatedEntity">The pricelistversion data to be updated</param>
        /// <returns>The result of the operation</returns>
        [HttpPut]
        [UserAuthorize("PriceListVersion",Entitlements.Update)]
        [Route("{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public IActionResult UpdateById(Guid id, [FromBody] PriceListVersion updatedEntity)
        {
            if (id != updatedEntity.Id)
            {
                return BadRequest("Mismatched Id");
            }

            var status = _priceListVersionService.Update(id, updatedEntity);
            return Ok(new { status });
        }

        /// <summary>Updates a specific pricelistversion by its primary key</summary>
        /// <param name="id">The primary key of the pricelistversion</param>
        /// <param name="updatedEntity">The pricelistversion data to be updated</param>
        /// <returns>The result of the operation</returns>
        [HttpPatch]
        [UserAuthorize("PriceListVersion",Entitlements.Update)]
        [Route("{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public IActionResult UpdateById(Guid id, [FromBody] JsonPatchDocument<PriceListVersion> updatedEntity)
        {
            if (updatedEntity == null)
                return BadRequest("Patch document is missing.");
            var status = _priceListVersionService.Patch(id, updatedEntity);
            return Ok(new { status });
        }
    }
}