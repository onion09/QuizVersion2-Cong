using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quiz2.Models.DBEntities
{
    [Table("UserInfo")]

    public class Account
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int userId { get; set; }
        [Required]
        [Display(Name = "User Name")]
        public string username { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string password { get; set; }    
        public string email { get; set; }   
        public string phone { get; set; }
        public string address { get;set; }
        public DateTime? dob { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string role { get; set; }


    }
}
