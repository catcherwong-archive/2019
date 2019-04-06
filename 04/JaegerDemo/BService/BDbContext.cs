namespace BService
{
    using Microsoft.EntityFrameworkCore;

    public class BDbContext : DbContext
    {
        public BDbContext(DbContextOptions<BDbContext> options)
            : base(options)
        {
        }

        public DbSet<DemoObj> DemoObjs { get; set; }

        public void Seed()
        {
            Database.OpenConnection();
            Database.EnsureCreated();
            Database.Migrate();

            DemoObjs.Add(new DemoObj(1, "Catcher"));
            DemoObjs.Add(new DemoObj(2, "Kobe"));
            DemoObjs.Add(new DemoObj(3, "James"));
            DemoObjs.Add(new DemoObj(4, "Jack"));

            SaveChanges();
        }
    }

    public class DemoObj
    {
        public int Id { get ; private set; }
        public string Name { get; private set; }

        public DemoObj(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
