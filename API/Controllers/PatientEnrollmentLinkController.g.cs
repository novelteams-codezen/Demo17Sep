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
    /// Controller responsible for managing patientenrollmentlink related operations.
    /// </summary>
    /// <remarks>
    /// This Controller provides endpoints for adding, retrieving, updating, and deleting patientenrollmentlink information.
    /// </remarks>
    [Route("api/patientenrollmentlink")]
    [Authorize]
    public class PatientEnrollmentLinkController : ControllerBase
    {
        private readonly IPatientEnrollmentLinkService _patientEnrollmentLinkService;

        /// <summary>
        /// Initializes a new instance of the PatientEnrollmentLinkController class with the specified context.
        /// </summary>
        /// <param name="ipatientenrollmentlinkservice">The ipatientenrollmentlinkservice to be used by the controller.</param>
        public PatientEnrollmentLinkController(IPatientEnrollmentLinkService ipatientenrollmentlinkservice)
        {
            _patientEnrollmentLinkService = ipatientenrollmentlinkservice;
        }

        /// <summary>Adds a new patientenrollmentlink</summary>
        /// <param name="model">The patientenrollmentlink data to be added</param>
        /// <returns>The result of the operation</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        [UserAuthorize("PatientEnrollmentLink",Entitlements.Create)]
        public IActionResult Post([FromBody] PatientEnrollmentLink model)
        {
            var id = _patientEnrollmentLinkService.Create(model);
            return Ok(new { id });
        }

        /// <summary>Retrieves a list of patientenrollmentlinks based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of patientenrollmentlinks</returns>
        [HttpGet]
        [UserAuthorize("PatientEnrollmentLink",Entitlements.Read)]
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

            var result = _patientEnrollmentLinkService.Get(filterCriteria, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return Ok(result);
        }

        /// <summary>Retrieves a specific patientenrollmentlink by its primary key</summary>
        /// <param name="id">The primary key of the patientenrollmentlink</param>
        /// <returns>The patientenrollmentlink data</returns>
        [HttpGet]
        [Route("{id:Guid}")]
        [UserAuthorize("PatientEnrollmentLink",Entitlements.Read)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        public IActionResult GetById([FromRoute] Guid id)
        {
            var result = _patientEnrollmentLinkService.GetById(id);
            return Ok(result);
        }

        /// <summary>Deletes a specific patientenrollmentlink by its primary key</summary>
        /// <param name="id">The primary key of the patientenrollmentlink</param>
        /// <returns>The result of the operation</returns>
        [HttpDelete]
        [UserAuthorize("PatientEnrollmentLink",Entitlements.Delete)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        [Route("{id:Guid}")]
        public IActionResult DeleteById([FromRoute] Guid id)
        {
            var status = _patientEnrollmentLinkService.Delete(id);
            return Ok(new { status });
        }

        /// <summary>Updates a specific patientenrollmentlink by its primary key</summary>
        /// <param name="id">The primary key of the patientenrollmentlink</param>
        /// <param name="updatedEntity">The patientenrollmentlink data to be updated</param>
        /// <returns>The result of the operation</returns>
        [HttpPut]
        [UserAuthorize("PatientEnrollmentLink",Entitlements.Update)]
        [Route("{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public IActionResult UpdateById(Guid id, [FromBody] PatientEnrollmentLink updatedEntity)
        {
            if (id != updatedEntity.Id)
            {
                return BadRequest("Mismatched Id");
            }

            var status = _patientEnrollmentLinkService.Update(id, updatedEntity);
            return Ok(new { status });
        }

        /// <summary>Updates a specific patientenrollmentlink by its primary key</summary>
        /// <param name="id">The primary key of the patientenrollmentlink</param>
        /// <param name="updatedEntity">The patientenrollmentlink data to be updated</param>
        /// <returns>The result of the operation</returns>
        [HttpPatch]
        [UserAuthorize("PatientEnrollmentLink",Entitlements.Update)]
        [Route("{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public IActionResult UpdateById(Guid id, [FromBody] JsonPatchDocument<PatientEnrollmentLink> updatedEntity)
        {
            if (updatedEntity == null)
                return BadRequest("Patch document is missing.");
            var status = _patientEnrollmentLinkService.Patch(id, updatedEntity);
            return Ok(new { status });
        }
    }
}