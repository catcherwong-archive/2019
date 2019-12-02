namespace TemplatesDemo
{
    using System;

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("==========ScribanDemo Begin============");

            var sd = new ScribanDemo();
            sd.RenderSimpleText();
            sd.RenderObject();

            Console.WriteLine("==========ScribanDemo End============");

            Console.WriteLine("==========FluidDemo Begin============");

            var fd = new FluidDemo();
            fd.RenderSimpleText();
            fd.RenderObject();

            Console.WriteLine("==========FluidDemo End============");

            Console.ReadKey();
        }
    }
}
