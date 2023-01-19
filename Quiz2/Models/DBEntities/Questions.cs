using Microsoft.Identity.Client;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        public int QuestionsId { get; set;}
        public int CategoriesId { get; set; }   
        public string QuesContent { get; set; }
        [InverseProperty("Question")]
        public List<Option> Options { get; set; }

    }

    [Table("Option")]
    public class Option
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OptionId { get; set; }
        public int QusetionId { get; set; } 
        public string OptionValue { get; set; }
        public bool ShouldChoose { get; set; }

        [ForeignKey("QuestionId")]
        [JsonIgnore]
        public Question Question { get; set; }
    }
}
