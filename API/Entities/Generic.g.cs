using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Demo17Sep.Entities
{
#pragma warning disable
    /// <summary> 
    /// Represents a generic entity with essential details
    /// </summary>
    public class Generic
    {
        /// <summary>
        /// TenantId of the Generic 
        /// </summary>
        public Guid? TenantId { get; set; }

        /// <summary>
        /// Primary key for the Generic 
        /// </summary>
        [Key]
        [Required]
        public Guid Id { get; set; }
        /// <summary>
        /// ItemName of the Generic 
        /// </summary>
        public string? ItemName { get; set; }
    }
}