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
    /// Controller responsible for managing productclassification related operations.
    /// </summary>
    /// <remarks>
    /// This Controller provides endpoints for adding, retrieving, updating, and deleting productclassification information.
    /// </remarks>
    [Route("api/productclassification")]
    [Authorize]
    public class ProductClassificationController : ControllerBase
    {
        private readonly IProductClassificationService _productClassificationService;

        /// <summary>
        /// Initializes a new instance of the ProductClassificationController class with the specified context.
        /// </summary>
        /// <param name="iproductclassificationservice">The iproductclassificationservice to be used by the controller.</param>
        public ProductClassificationController(IProductClassificationService iproductclassificationservice)
        {
            _productClassificationService = iproductclassificationservice;
        }

        /// <summary>Adds a new productclassification</summary>
        /// <param name="model">The productclassification data to be added</param>
        /// <returns>The result of the operation</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        [UserAuthorize("ProductClassification",Entitlements.Create)]
        public IActionResult Post([FromBody] ProductClassification model)
        {
            var id = _productClassificationService.Create(model);
            return Ok(new { id });
        }

        /// <summary>Retrieves a list of productclassifications based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of productclassifications</returns>
        [HttpGet]
        [UserAuthorize("ProductClassification",Entitlements.Read)]
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

            var result = _productClassificationService.Get(filterCriteria, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return Ok(result);
        }

        /// <summary>Retrieves a specific productclassification by its primary key</summary>
        /// <param name="id">The primary key of the productclassification</param>
        /// <returns>The productclassification data</returns>
        [HttpGet]
        [Route("{id:Guid}")]
        [UserAuthorize("ProductClassification",Entitlements.Read)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        public IActionResult GetById([FromRoute] Guid id)
        {
            var result = _productClassificationService.GetById(id);
            return Ok(result);
        }

        /// <summary>Deletes a specific productclassification by its primary key</summary>
        /// <param name="id">The primary key of the productclassification</param>
        /// <returns>The result of the operation</returns>
        [HttpDelete]
        [UserAuthorize("ProductClassification",Entitlements.Delete)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        [Route("{id:Guid}")]
        public IActionResult DeleteById([FromRoute] Guid id)
        {
            var status = _productClassificationService.Delete(id);
            return Ok(new { status });
        }

        /// <summary>Updates a specific productclassification by its primary key</summary>
        /// <param name="id">The primary key of the productclassification</param>
        /// <param name="updatedEntity">The productclassification data to be updated</param>
        /// <returns>The result of the operation</returns>
        [HttpPut]
        [UserAuthorize("ProductClassification",Entitlements.Update)]
        [Route("{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public IActionResult UpdateById(Guid id, [FromBody] ProductClassification updatedEntity)
        {
            if (id != updatedEntity.Id)
            {
                return BadRequest("Mismatched Id");
            }

            var status = _productClassificationService.Update(id, updatedEntity);
            return Ok(new { status });
        }

        /// <summary>Updates a specific productclassification by its primary key</summary>
        /// <param name="id">The primary key of the productclassification</param>
        /// <param name="updatedEntity">The productclassification data to be updated</param>
        /// <returns>The result of the operation</returns>
        [HttpPatch]
        [UserAuthorize("ProductClassification",Entitlements.Update)]
        [Route("{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public IActionResult UpdateById(Guid id, [FromBody] JsonPatchDocument<ProductClassification> updatedEntity)
        {
            if (updatedEntity == null)
                return BadRequest("Patch document is missing.");
            var status = _productClassificationService.Patch(id, updatedEntity);
            return Ok(new { status });
        }
    }
}