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
    /// Controller responsible for managing contactmember related operations.
    /// </summary>
    /// <remarks>
    /// This Controller provides endpoints for adding, retrieving, updating, and deleting contactmember information.
    /// </remarks>
    [Route("api/contactmember")]
    [Authorize]
    public class ContactMemberController : ControllerBase
    {
        private readonly IContactMemberService _contactMemberService;

        /// <summary>
        /// Initializes a new instance of the ContactMemberController class with the specified context.
        /// </summary>
        /// <param name="icontactmemberservice">The icontactmemberservice to be used by the controller.</param>
        public ContactMemberController(IContactMemberService icontactmemberservice)
        {
            _contactMemberService = icontactmemberservice;
        }

        /// <summary>Adds a new contactmember</summary>
        /// <param name="model">The contactmember data to be added</param>
        /// <returns>The result of the operation</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        [UserAuthorize("ContactMember",Entitlements.Create)]
        public IActionResult Post([FromBody] ContactMember model)
        {
            var id = _contactMemberService.Create(model);
            return Ok(new { id });
        }

        /// <summary>Retrieves a list of contactmembers based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of contactmembers</returns>
        [HttpGet]
        [UserAuthorize("ContactMember",Entitlements.Read)]
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

            var result = _contactMemberService.Get(filterCriteria, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return Ok(result);
        }

        /// <summary>Retrieves a specific contactmember by its primary key</summary>
        /// <param name="id">The primary key of the contactmember</param>
        /// <returns>The contactmember data</returns>
        [HttpGet]
        [Route("{id:Guid}")]
        [UserAuthorize("ContactMember",Entitlements.Read)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        public IActionResult GetById([FromRoute] Guid id)
        {
            var result = _contactMemberService.GetById(id);
            return Ok(result);
        }

        /// <summary>Deletes a specific contactmember by its primary key</summary>
        /// <param name="id">The primary key of the contactmember</param>
        /// <returns>The result of the operation</returns>
        [HttpDelete]
        [UserAuthorize("ContactMember",Entitlements.Delete)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        [Route("{id:Guid}")]
        public IActionResult DeleteById([FromRoute] Guid id)
        {
            var status = _contactMemberService.Delete(id);
            return Ok(new { status });
        }

        /// <summary>Updates a specific contactmember by its primary key</summary>
        /// <param name="id">The primary key of the contactmember</param>
        /// <param name="updatedEntity">The contactmember data to be updated</param>
        /// <returns>The result of the operation</returns>
        [HttpPut]
        [UserAuthorize("ContactMember",Entitlements.Update)]
        [Route("{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public IActionResult UpdateById(Guid id, [FromBody] ContactMember updatedEntity)
        {
            if (id != updatedEntity.Id)
            {
                return BadRequest("Mismatched Id");
            }

            var status = _contactMemberService.Update(id, updatedEntity);
            return Ok(new { status });
        }

        /// <summary>Updates a specific contactmember by its primary key</summary>
        /// <param name="id">The primary key of the contactmember</param>
        /// <param name="updatedEntity">The contactmember data to be updated</param>
        /// <returns>The result of the operation</returns>
        [HttpPatch]
        [UserAuthorize("ContactMember",Entitlements.Update)]
        [Route("{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public IActionResult UpdateById(Guid id, [FromBody] JsonPatchDocument<ContactMember> updatedEntity)
        {
            if (updatedEntity == null)
                return BadRequest("Patch document is missing.");
            var status = _contactMemberService.Patch(id, updatedEntity);
            return Ok(new { status });
        }
    }
}