using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace AspnetIdentityRoleBasedTutorial.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string? Name { get; set; }
        public string? ProfilePicture { get; set; }

        public int? Age
        {
            get
            {
                if (Birthday.HasValue)
                {
                    var today = DateTime.Today;
                    var age = today.Year - Birthday.Value.Year;
                    
                    if (Birthday > today.AddYears(-age))
                    {
                        age--;
                    }
                    return age;
                }
                return null; 
            }
        }



        [DataType(DataType.Date)]
        public DateTime? Birthday { get; set; }
        public string? EducationLevel {  get; set; }
        public int ToeicScore { get; set; }
        public string? Programming { get; set; }
        public string? Experience { get; set; }
        public string? Address { get; set;}
        public string? RoleName { get; set; }


    }
}
