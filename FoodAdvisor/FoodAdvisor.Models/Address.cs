using System.ComponentModel.DataAnnotations;

namespace FoodAdvisor.Models
{
    public class Address
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the street.
        /// </summary>
        [Required]
        public string Street { get; set; }

        /// <summary>
        /// Gets or sets the zip code.
        /// </summary>
        [Required]
        [RegularExpression(@"^\d{5}$", ErrorMessage = "It's not a right french zip code (example: 38000)")]
        public string ZipCode { get; set; }

        /// <summary>
        /// Gets or sets the city.
        /// </summary>
        [Required]
        public string City { get; set; }

        /// <summary>
        /// Gets or sets the restaurant identifier.
        /// </summary>
        public int RestaurantId { get; set; }
    }
}
