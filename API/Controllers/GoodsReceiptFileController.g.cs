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
    /// Controller responsible for managing goodsreceiptfile related operations.
    /// </summary>
    /// <remarks>
    /// This Controller provides endpoints for adding, retrieving, updating, and deleting goodsreceiptfile information.
    /// </remarks>
    [Route("api/goodsreceiptfile")]
    [Authorize]
    public class GoodsReceiptFileController : ControllerBase
    {
        private readonly IGoodsReceiptFileService _goodsReceiptFileService;

        /// <summary>
        /// Initializes a new instance of the GoodsReceiptFileController class with the specified context.
        /// </summary>
        /// <param name="igoodsreceiptfileservice">The igoodsreceiptfileservice to be used by the controller.</param>
        public GoodsReceiptFileController(IGoodsReceiptFileService igoodsreceiptfileservice)
        {
            _goodsReceiptFileService = igoodsreceiptfileservice;
        }

        /// <summary>Adds a new goodsreceiptfile</summary>
        /// <param name="model">The goodsreceiptfile data to be added</param>
        /// <returns>The result of the operation</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        [UserAuthorize("GoodsReceiptFile",Entitlements.Create)]
        public IActionResult Post([FromBody] GoodsReceiptFile model)
        {
            var id = _goodsReceiptFileService.Create(model);
            return Ok(new { id });
        }

        /// <summary>Retrieves a list of goodsreceiptfiles based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of goodsreceiptfiles</returns>
        [HttpGet]
        [UserAuthorize("GoodsReceiptFile",Entitlements.Read)]
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

            var result = _goodsReceiptFileService.Get(filterCriteria, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return Ok(result);
        }

        /// <summary>Retrieves a specific goodsreceiptfile by its primary key</summary>
        /// <param name="id">The primary key of the goodsreceiptfile</param>
        /// <returns>The goodsreceiptfile data</returns>
        [HttpGet]
        [Route("{id:Guid}")]
        [UserAuthorize("GoodsReceiptFile",Entitlements.Read)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        public IActionResult GetById([FromRoute] Guid id)
        {
            var result = _goodsReceiptFileService.GetById(id);
            return Ok(result);
        }

        /// <summary>Deletes a specific goodsreceiptfile by its primary key</summary>
        /// <param name="id">The primary key of the goodsreceiptfile</param>
        /// <returns>The result of the operation</returns>
        [HttpDelete]
        [UserAuthorize("GoodsReceiptFile",Entitlements.Delete)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        [Route("{id:Guid}")]
        public IActionResult DeleteById([FromRoute] Guid id)
        {
            var status = _goodsReceiptFileService.Delete(id);
            return Ok(new { status });
        }

        /// <summary>Updates a specific goodsreceiptfile by its primary key</summary>
        /// <param name="id">The primary key of the goodsreceiptfile</param>
        /// <param name="updatedEntity">The goodsreceiptfile data to be updated</param>
        /// <returns>The result of the operation</returns>
        [HttpPut]
        [UserAuthorize("GoodsReceiptFile",Entitlements.Update)]
        [Route("{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public IActionResult UpdateById(Guid id, [FromBody] GoodsReceiptFile updatedEntity)
        {
            if (id != updatedEntity.Id)
            {
                return BadRequest("Mismatched Id");
            }

            var status = _goodsReceiptFileService.Update(id, updatedEntity);
            return Ok(new { status });
        }

        /// <summary>Updates a specific goodsreceiptfile by its primary key</summary>
        /// <param name="id">The primary key of the goodsreceiptfile</param>
        /// <param name="updatedEntity">The goodsreceiptfile data to be updated</param>
        /// <returns>The result of the operation</returns>
        [HttpPatch]
        [UserAuthorize("GoodsReceiptFile",Entitlements.Update)]
        [Route("{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public IActionResult UpdateById(Guid id, [FromBody] JsonPatchDocument<GoodsReceiptFile> updatedEntity)
        {
            if (updatedEntity == null)
                return BadRequest("Patch document is missing.");
            var status = _goodsReceiptFileService.Patch(id, updatedEntity);
            return Ok(new { status });
        }
    }
}