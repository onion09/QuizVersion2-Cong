using Microsoft.EntityFrameworkCore;

namespace Quiz2.Models.DBEntities
{
    public class ApplicationDBContext:DbContext
    {
        private readonly IConfiguration _configuration;
        public DbSet<Question> Questions { get; set; }
        public DbSet<Option> Options { get; set; }
        public DbSet<QuestionLog> QuestionLogs { get; set; }
        public DbSet<SessionLog> SessionLogs { get; set; }  
        public ApplicationDBContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("MyConn"));
        }

    }
}
