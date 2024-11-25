using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentUpload.Service
{
    public class RelativePathGenerator
    {
        public static Dictionary<string, string> RelativePaths = new Dictionary<string, string>()
        {
            {"Compliance","\\{0}\\{1}\\Compliance" },
        };


    }
}
