using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MindEngine
{
    class Program
    {
        static void Main()
        {
            using (var engine = new MMEngine())
            {
                engine.Run();
            }
        }
    }
}
