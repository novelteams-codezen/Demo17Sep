using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Demo17Sep.Entities
{
#pragma warning disable
    /// <summary> 
    /// Represents a title entity with essential details
    /// </summary>
    public class Title
    {
        /// <summary>
        /// Primary key for the Title 
        /// </summary>
        [Key]
        [Required]
        public Guid Id { get; set; }
        /// <summary>
        /// TenantId of the Title 
        /// </summary>
        public Guid? TenantId { get; set; }

        /// <summary>
        /// Required field Name of the Title 
        /// </summary>
        [Required]
        public string Name { get; set; }
    }
}