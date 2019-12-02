using System.Collections.Generic;

namespace TemplatesDemo
{
    class ObjModel
    {
        public string Name { get; set; }

        public List<Order> Orders { get; set; }

        public class Order
        {
            public int Id { get; set; }
            public int Amount { get; set; }
        }
    }

  
}
