using System.Collections.Generic;

namespace TVSeriesApp.Models
{
    public class UserRoleViewModel
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; }
    }

    public class EditUserViewModel
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public List<string> UserRoles { get; set; }
        public List<string> AllRoles { get; set; }
        public List<string> SelectedRoles { get; set; }
    }
}