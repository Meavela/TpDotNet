using System;
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
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the score.
        /// </summary>
        public int Score { get; set; }

        /// <summary>
        /// Gets or sets the comment.
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Gets or sets the restaurant identifier.
        /// </summary>
        //public int RestaurantId { get; set; }
    }
}
