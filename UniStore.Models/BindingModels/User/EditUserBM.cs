namespace UniStore.Models.BindingModels.User
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class EditUserBM
    {
        [Required]
        public string Id { get; set; }

        public string Name { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string UserName { get; set; }

        public string Address { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }

        [Display(Name = "Assigned roles")]
        public IList<string> Roles { get; set; } = new List<string>();

        public string NewRole { get; set; }
    }
}