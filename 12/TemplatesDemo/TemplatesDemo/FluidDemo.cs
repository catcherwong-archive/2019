namespace TemplatesDemo
{
    using Fluid;
    using System;
    using System.Collections.Generic;


    class FluidDemo
    {
        public void RenderSimpleText()
        {
            string text = "Hello, {{name}} ";

            string name1 = "catcher wong";
            string name2 = "Catcher Wong";

            if (FluidTemplate.TryParse(text, out var template))
            {
                var context1 = new TemplateContext();
                context1.SetValue("name", name1);
                Console.WriteLine(template.Render(context1));

                var context2 = new TemplateContext();                
                context2.SetValue("Name", name2);
                Console.WriteLine(template.Render(context2));
            }
        }


        public void RenderObject()
        {
            string text = @"Hello {{model.Name}}, your orders' information are as follow, 

            you have {{model.Orders | size }} orders                

            {% for order in model.Orders %}
                {{order.Id}}-{{order.Amount}}
            {% endfor %}
            ";

            var obj = new ObjModel
            {
                Name = "Catcher Wong",
                Orders = new List<ObjModel.Order>
                {
                    new ObjModel.Order { Id = 1, Amount = 100 },
                    new ObjModel.Order { Id = 2, Amount = 300 }
                }
            };


            if (FluidTemplate.TryParse(text, out var template))
            {
                var context = new TemplateContext();
                context.MemberAccessStrategy.Register(obj.GetType());
                context.MemberAccessStrategy.Register(typeof(ObjModel.Order));
                context.SetValue("model", obj);
                Console.WriteLine(template.Render(context));
            }
        }
    }
}
