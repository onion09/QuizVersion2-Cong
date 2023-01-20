using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Quiz2.Models.DBEntities
{
    [Table("Category")]
    public class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }    

    }


    [Table("Question")]
    public class Question
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int QuestionId { get; set;}
        [NotMapped]
        public int QuesionInSessionId { get; set;}
        public int CategoryId { get; set; }   
        public string QuesContent { get; set; }
        [InverseProperty("Question")]
        public List<Option> Options { get; set; }

        //[ForeignKey("QuestionLogId")]
        //[JsonIgnore]
        //public List<QuestionLog> questionLogs { get; set; } 
    }

    [Table("Options")]
    public class Option
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OptionId { get; set; }
        public int QuestionId { get; set; } 
        public string OptionValue { get; set; }
        public bool ShouldChoose { get; set; }


        [ForeignKey("QuestionId")]
        [JsonIgnore]
        public Question Question { get; set; }
    }
}
