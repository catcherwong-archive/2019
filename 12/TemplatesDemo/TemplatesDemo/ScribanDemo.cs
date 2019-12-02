namespace TemplatesDemo
{
    using Scriban;
    using System;
    using System.Collections.Generic;

    class ScribanDemo
    {
        public void RenderSimpleText()
        {
            string text = "Hello, {{name}} ";

            var tpl = Template.Parse(text);

            var res1 = tpl.Render(new { name = "catcher wong" });
            Console.WriteLine(res1);
            var res2 = tpl.Render(new { Name = "Catcher Wong" });
            Console.WriteLine(res2);
        }


        public void RenderObject()
        {
            string text = @"Hello {{model.name}}, your orders' information are as follow, 

            you have {{model.orders | array.size }} orders                

            {{ for order in model.orders }}
                {{order.id}}-{{order.amount}}
            {{ end }}
            ";

            var tpl = Template.Parse(text);

            var res1 = tpl.Render(new 
            { 
                model = new ObjModel 
                { 
                    Name = "Catcher Wong", 
                    Orders = new List<ObjModel.Order> 
                    { 
                        new ObjModel.Order { Id = 1, Amount = 100 }, 
                        new ObjModel.Order { Id = 2, Amount = 300 } 
                    } 
                } 
            });

            Console.WriteLine(res1);
        }       
    }   
}
