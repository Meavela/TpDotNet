using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FoodAdvisor.Models
{
    public class Grade
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        [JsonConverter(typeof(JsonDoubleToDateConverter))]
        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the score.
        /// </summary>
        [Required]
        [Range(0,10, ErrorMessage = "Score has to be between 0 and 10")]
        public int Score { get; set; }

        /// <summary>
        /// Gets or sets the comment.
        /// </summary>
        [Required]
        [StringLength(255, ErrorMessage = "Comment can't be more than 255.")]
        public string Comment { get; set; }

        /// <summary>
        /// Gets or sets the restaurant identifier.
        /// </summary>
        public int RestaurantId { get; set; }
    }
}
