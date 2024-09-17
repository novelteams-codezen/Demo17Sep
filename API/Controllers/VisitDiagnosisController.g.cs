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
    /// Controller responsible for managing visitdiagnosis related operations.
    /// </summary>
    /// <remarks>
    /// This Controller provides endpoints for adding, retrieving, updating, and deleting visitdiagnosis information.
    /// </remarks>
    [Route("api/visitdiagnosis")]
    [Authorize]
    public class VisitDiagnosisController : ControllerBase
    {
        private readonly IVisitDiagnosisService _visitDiagnosisService;

        /// <summary>
        /// Initializes a new instance of the VisitDiagnosisController class with the specified context.
        /// </summary>
        /// <param name="ivisitdiagnosisservice">The ivisitdiagnosisservice to be used by the controller.</param>
        public VisitDiagnosisController(IVisitDiagnosisService ivisitdiagnosisservice)
        {
            _visitDiagnosisService = ivisitdiagnosisservice;
        }

        /// <summary>Adds a new visitdiagnosis</summary>
        /// <param name="model">The visitdiagnosis data to be added</param>
        /// <returns>The result of the operation</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        [UserAuthorize("VisitDiagnosis",Entitlements.Create)]
        public IActionResult Post([FromBody] VisitDiagnosis model)
        {
            var id = _visitDiagnosisService.Create(model);
            return Ok(new { id });
        }

        /// <summary>Retrieves a list of visitdiagnosiss based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of visitdiagnosiss</returns>
        [HttpGet]
        [UserAuthorize("VisitDiagnosis",Entitlements.Read)]
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

            var result = _visitDiagnosisService.Get(filterCriteria, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return Ok(result);
        }

        /// <summary>Retrieves a specific visitdiagnosis by its primary key</summary>
        /// <param name="id">The primary key of the visitdiagnosis</param>
        /// <returns>The visitdiagnosis data</returns>
        [HttpGet]
        [Route("{id:Guid}")]
        [UserAuthorize("VisitDiagnosis",Entitlements.Read)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        public IActionResult GetById([FromRoute] Guid id)
        {
            var result = _visitDiagnosisService.GetById(id);
            return Ok(result);
        }

        /// <summary>Deletes a specific visitdiagnosis by its primary key</summary>
        /// <param name="id">The primary key of the visitdiagnosis</param>
        /// <returns>The result of the operation</returns>
        [HttpDelete]
        [UserAuthorize("VisitDiagnosis",Entitlements.Delete)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        [Route("{id:Guid}")]
        public IActionResult DeleteById([FromRoute] Guid id)
        {
            var status = _visitDiagnosisService.Delete(id);
            return Ok(new { status });
        }

        /// <summary>Updates a specific visitdiagnosis by its primary key</summary>
        /// <param name="id">The primary key of the visitdiagnosis</param>
        /// <param name="updatedEntity">The visitdiagnosis data to be updated</param>
        /// <returns>The result of the operation</returns>
        [HttpPut]
        [UserAuthorize("VisitDiagnosis",Entitlements.Update)]
        [Route("{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public IActionResult UpdateById(Guid id, [FromBody] VisitDiagnosis updatedEntity)
        {
            if (id != updatedEntity.Id)
            {
                return BadRequest("Mismatched Id");
            }

            var status = _visitDiagnosisService.Update(id, updatedEntity);
            return Ok(new { status });
        }

        /// <summary>Updates a specific visitdiagnosis by its primary key</summary>
        /// <param name="id">The primary key of the visitdiagnosis</param>
        /// <param name="updatedEntity">The visitdiagnosis data to be updated</param>
        /// <returns>The result of the operation</returns>
        [HttpPatch]
        [UserAuthorize("VisitDiagnosis",Entitlements.Update)]
        [Route("{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public IActionResult UpdateById(Guid id, [FromBody] JsonPatchDocument<VisitDiagnosis> updatedEntity)
        {
            if (updatedEntity == null)
                return BadRequest("Patch document is missing.");
            var status = _visitDiagnosisService.Patch(id, updatedEntity);
            return Ok(new { status });
        }
    }
}