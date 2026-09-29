using Grasshopper;
using Grasshopper.Kernel;
using System.Drawing;
using System.IO;
using System.Reflection;

namespace Meanders_tools
{
    public class Meanders_toolsPriority : GH_AssemblyPriority
    {
        public override GH_LoadingInstruction PriorityLoad()
        {
            Stream stream =
                Assembly.GetExecutingAssembly()
                .GetManifestResourceStream(
                    "Meanders_tools.Resources.Meanders.png");

            if (stream != null)
            {
                Bitmap icon = new Bitmap(stream);

                Instances.ComponentServer.AddCategoryIcon(
                    "Meanders",
                    icon);
            }

            return GH_LoadingInstruction.Proceed;
        }
    }
}