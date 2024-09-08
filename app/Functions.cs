using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuaFlux
{
    internal class Functions
    {
        public static void TestFunction(string _, string __)
        {
            Utilities.LuaFluxWrite(ConsoleColor.Magenta, "Welcome to LuaFlux!");
        }
    }
}
