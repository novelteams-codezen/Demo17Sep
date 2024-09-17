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
    /// Controller responsible for managing accountsettlement related operations.
    /// </summary>
    /// <remarks>
    /// This Controller provides endpoints for adding, retrieving, updating, and deleting accountsettlement information.
    /// </remarks>
    [Route("api/accountsettlement")]
    [Authorize]
    public class AccountSettlementController : ControllerBase
    {
        private readonly IAccountSettlementService _accountSettlementService;

        /// <summary>
        /// Initializes a new instance of the AccountSettlementController class with the specified context.
        /// </summary>
        /// <param name="iaccountsettlementservice">The iaccountsettlementservice to be used by the controller.</param>
        public AccountSettlementController(IAccountSettlementService iaccountsettlementservice)
        {
            _accountSettlementService = iaccountsettlementservice;
        }

        /// <summary>Adds a new accountsettlement</summary>
        /// <param name="model">The accountsettlement data to be added</param>
        /// <returns>The result of the operation</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        [UserAuthorize("AccountSettlement",Entitlements.Create)]
        public IActionResult Post([FromBody] AccountSettlement model)
        {
            var id = _accountSettlementService.Create(model);
            return Ok(new { id });
        }

        /// <summary>Retrieves a list of accountsettlements based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of accountsettlements</returns>
        [HttpGet]
        [UserAuthorize("AccountSettlement",Entitlements.Read)]
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

            var result = _accountSettlementService.Get(filterCriteria, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return Ok(result);
        }

        /// <summary>Retrieves a specific accountsettlement by its primary key</summary>
        /// <param name="id">The primary key of the accountsettlement</param>
        /// <returns>The accountsettlement data</returns>
        [HttpGet]
        [Route("{id:Guid}")]
        [UserAuthorize("AccountSettlement",Entitlements.Read)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        public IActionResult GetById([FromRoute] Guid id)
        {
            var result = _accountSettlementService.GetById(id);
            return Ok(result);
        }

        /// <summary>Deletes a specific accountsettlement by its primary key</summary>
        /// <param name="id">The primary key of the accountsettlement</param>
        /// <returns>The result of the operation</returns>
        [HttpDelete]
        [UserAuthorize("AccountSettlement",Entitlements.Delete)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        [Route("{id:Guid}")]
        public IActionResult DeleteById([FromRoute] Guid id)
        {
            var status = _accountSettlementService.Delete(id);
            return Ok(new { status });
        }

        /// <summary>Updates a specific accountsettlement by its primary key</summary>
        /// <param name="id">The primary key of the accountsettlement</param>
        /// <param name="updatedEntity">The accountsettlement data to be updated</param>
        /// <returns>The result of the operation</returns>
        [HttpPut]
        [UserAuthorize("AccountSettlement",Entitlements.Update)]
        [Route("{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public IActionResult UpdateById(Guid id, [FromBody] AccountSettlement updatedEntity)
        {
            if (id != updatedEntity.Id)
            {
                return BadRequest("Mismatched Id");
            }

            var status = _accountSettlementService.Update(id, updatedEntity);
            return Ok(new { status });
        }

        /// <summary>Updates a specific accountsettlement by its primary key</summary>
        /// <param name="id">The primary key of the accountsettlement</param>
        /// <param name="updatedEntity">The accountsettlement data to be updated</param>
        /// <returns>The result of the operation</returns>
        [HttpPatch]
        [UserAuthorize("AccountSettlement",Entitlements.Update)]
        [Route("{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public IActionResult UpdateById(Guid id, [FromBody] JsonPatchDocument<AccountSettlement> updatedEntity)
        {
            if (updatedEntity == null)
                return BadRequest("Patch document is missing.");
            var status = _accountSettlementService.Patch(id, updatedEntity);
            return Ok(new { status });
        }
    }
}