using System.ComponentModel.DataAnnotations;

namespace Quiz2.Models.Data
{
    public class UserInView
    {
        public int userId { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string address { get; set; }
        public DateTime dob { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string role { get; set; }
    }
}
