using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using System;
using System.Drawing;
using System.Collections.Generic;

namespace Meanders_tools
{
    public class ME_Object_Param : GH_PersistentParam<ME_Object_Goo>
    {
        public ME_Object_Param()
            : base(
                "ME Object",
                "ME Obj",
                "Meanders Object",
                "Meanders Tools",
                "Objects")
        {
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("A7F3C2D1-6E54-4B91-9F28-21D8E6C04A73"); }
        }

        protected override Bitmap Icon
        {
            get { return null; }
        }

        protected override GH_GetterResult Prompt_Singular(ref ME_Object_Goo value)
        {
            return GH_GetterResult.cancel;
        }

        protected override GH_GetterResult Prompt_Plural(
            ref List<ME_Object_Goo> values)
        {
            return GH_GetterResult.cancel;
        }
    }
}