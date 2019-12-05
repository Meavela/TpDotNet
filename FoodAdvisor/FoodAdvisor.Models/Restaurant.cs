using System.ComponentModel.DataAnnotations;

namespace FoodAdvisor.Models
{
    public class Restaurant
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the phone.
        /// </summary>
        [Required]
        public string Phone { get; set; }

        /// <summary>
        /// Gets or sets the comment.
        /// </summary>
        [Required]
        public string Comment { get; set; }

        /// <summary>
        /// Gets or sets the mail owner.
        /// </summary>
        [Required]
        public string MailOwner { get; set; }

        /// <summary>
        /// Gets or sets the address.
        /// </summary>
        public Address Address { get; set; }

        /// <summary>
        /// Gets or sets the grade.
        /// </summary>
        public Grade Grade { get; set; }

    }
}
