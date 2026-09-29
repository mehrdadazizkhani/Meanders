using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using System;

namespace Meanders_tools
{
    public class ME_Attribute_Goo : GH_Goo<ME_Attribute>
    {
        public ME_Attribute_Goo()
        {
            Value = new ME_Attribute();
        }

        public ME_Attribute_Goo(ME_Attribute attribute)
        {
            Value = attribute;
        }

        public override bool IsValid
        {
            get { return Value != null; }
        }

        public override string TypeName
        {
            get { return "ME Attribute"; }
        }

        public override string TypeDescription
        {
            get { return "Meanders object attributes."; }
        }

        public override IGH_Goo Duplicate()
        {
            if (Value == null)
                return new ME_Attribute_Goo();

            return new ME_Attribute_Goo(
                Value.Duplicate()
            );
        }

        public override string ToString()
        {
            return Value != null
                ? Value.ToString()
                : "Null ME Attribute";
        }

        public override bool CastFrom(object source)
        {
            if (source is ME_Attribute attribute)
            {
                Value = attribute;
                return true;
            }

            if (source is Rhino.DocObjects.ObjectAttributes attributes)
            {
                Value = new ME_Attribute(attributes);
                return true;
            }

            return base.CastFrom(source);
        }

        public override bool CastTo<Q>(ref Q target)
        {
            if (typeof(Q).IsAssignableFrom(typeof(ME_Attribute)))
            {
                object value = Value;
                target = (Q)value;
                return true;
            }

            if (typeof(Q).IsAssignableFrom(
                typeof(Rhino.DocObjects.ObjectAttributes)))
            {
                object value = Value.Attributes;
                target = (Q)value;
                return true;
            }

            return base.CastTo(ref target);
        }
    }
}