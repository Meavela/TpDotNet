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
        [RegularExpression(@"^(\d{2}[.]){4}\d{2}$", ErrorMessage = "Example : 12.34.56.78.00")]
        public string Phone { get; set; }

        /// <summary>
        /// Gets or sets the comment.
        /// </summary>
        [Required]
        [StringLength(1000, ErrorMessage = "Comment can't be more than 1000.")]
        public string Comment { get; set; }

        /// <summary>
        /// Gets or sets the mail owner.
        /// </summary>
        [Required]
        [EmailAddress(ErrorMessage = "The MailOwner field is not a valid e-mail address.")]
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
