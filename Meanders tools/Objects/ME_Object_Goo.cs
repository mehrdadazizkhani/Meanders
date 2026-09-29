using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino;

namespace Meanders_tools
{
    public class ME_Object_Goo : GH_Goo<ME_Object>
    {
        public ME_Object_Goo()
        {
            Value = new ME_Object();
        }

        public ME_Object_Goo(ME_Object obj)
        {
            Value = obj;
        }

        public override bool IsValid
        {
            get { return Value != null; }
        }

        public override string TypeName
        {
            get { return "ME Object"; }
        }

        public override string TypeDescription
        {
            get { return "Meanders Object with geometry and attributes."; }
        }

        public override IGH_Goo Duplicate()
        {
            if (Value == null)
                return new ME_Object_Goo();

            return new ME_Object_Goo(
                new ME_Object(
                    Value.Geometry,
                    Value.Attributes
                )
            );
        }

        public override string ToString()
        {
            return Value != null ? Value.ToString() : "Null ME Object";
        }

        public override bool CastFrom(object source)
        {
            if (source is ME_Object obj)
            {
                Value = obj;
                return true;
            }

            return base.CastFrom(source);
        }

        public override bool CastTo<Q>(ref Q target)
        {
            if (typeof(Q).IsAssignableFrom(typeof(ME_Object)))
            {
                object obj = Value;
                target = (Q)obj;
                return true;
            }

            return base.CastTo(ref target);
        }
    }
}