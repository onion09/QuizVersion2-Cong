using Quiz2.Models.DBEntities;

namespace Quiz2.DAO
{
    public class LogDao
    {
        private readonly ApplicationDBContext _dbContext;
        public LogDao(ApplicationDBContext applicationDBContext)
        {
            _dbContext = applicationDBContext;
        }

       
    }
}
