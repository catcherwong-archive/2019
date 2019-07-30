namespace MyBasedServiceA
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class FakeUserInfoDb
    {
        public static List<DemoUser> DB = new List<DemoUser>()
        {
            new DemoUser{ Id = 1, Age = 18, Name = "catcher", CreateTime = 1000 },
            new DemoUser{ Id = 2, Age = 19, Name = "kobe", CreateTime = 1000 },
        };

        public static DemoUser GetById(int id)
        {
            return DB.FirstOrDefault(x => x.Id.Equals(id));
        }

        public static List<DemoUser> GetList(int id, string name)
        {            
            return DB;
        }

        public static bool Save(string name, int age)
        {
            int id = new Random().Next(100, 100000);
            DB.Add(new DemoUser { Id = id, Age = age, Name = $"{name}-{id}", CreateTime = 2000 });
            return true;
        }           
    }

    public class DemoUser
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Age { get; set; } 
            
        public long CreateTime { get; set; }
    }
}
