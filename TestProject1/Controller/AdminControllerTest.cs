using Microsoft.AspNetCore.Mvc;
using Moq;
using Quiz2.Controllers;
using Quiz2.DAO;
using Quiz2.Models.DBEntities;
using QuizProject.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject1.Controller
{
    public class AdminControllerTest
    {
        public Mock<QuestionDao> questionDaoMock = new Mock<QuestionDao>();
        public Mock<AccountDao> accountDaoMock = new Mock<AccountDao>();
        public List<Question> questionListMock;
        public AdminController adminController;
        public Option option;
        public int id;
        public AdminControllerTest()
        {
            questionListMock= new List<Question>()
            {
                new Question()
                {
                    QuestionId= 1, CategoryId=1,QuesContent="testQuestion1",
                    Options = new List<Option>()
                    {
                        new Option() {OptionId=1, OptionValue="option1",ShouldChoose= true },
                        new Option() {OptionId=2, OptionValue="option2",ShouldChoose= false}
                    }
                },
                new Question()
                {
                    QuestionId= 2, CategoryId=1,QuesContent="testQuestion2",
                    Options = new List<Option>()
                    {
                        new Option() {OptionId=3, OptionValue="option3",ShouldChoose= true },
                        new Option() {OptionId=4, OptionValue="option4",ShouldChoose= false}
                    }
                },
                new Question()
                {
                    QuestionId= 3, CategoryId=2,QuesContent="testQuestion3",
                    Options = new List<Option>()
                    {
                        new Option() {OptionId=5, OptionValue="option5",ShouldChoose= true },
                        new Option() {OptionId=6, OptionValue="option6",ShouldChoose= false}
                    }
                },
            };
            option  = new Option() { OptionId=1,OptionValue="testOption",ShouldChoose= false};
            id = 1;
        }

        [Fact]
        public void AdminControllerGetAllQuestions_ReturnsViewResultWithExpectedModel()
        {
            
            questionDaoMock.Setup(qd => qd.GetAllQuestions()).Returns(questionListMock);
            AdminController adminController = new AdminController(questionDaoMock.Object, accountDaoMock.Object);

            //act
            var result = adminController.GetAlQuestions() as ViewResult;

            //Assert
            Assert.NotNull(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Question>>(result.Model);

            Assert.Equal(questionListMock.Count(), model.Count());
        }
        [Fact]
        public void AdminController_Get_DeleteOption()
        {
            questionDaoMock.Setup(qd => qd.GetOptionById(It.IsAny<int>())).Returns(option);
            AdminController adminController = new AdminController(questionDaoMock.Object, accountDaoMock.Object);
            var result = adminController.DeleteOption(1);
            var viewResult = result as ViewResult;
            Assert.NotNull(viewResult);
            Assert.IsType<Option>(viewResult.Model);
            Assert.Equal(option, viewResult.Model);
        }
    }
}
