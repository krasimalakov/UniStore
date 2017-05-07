namespace UniStore.Models.ViewModels.User
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class UserVM
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string UserName { get; set; }

        [Display(Name = "Assigned roles")]
        public List<string> Roles { get; set; }
    }
}