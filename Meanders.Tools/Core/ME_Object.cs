using Grasshopper.Kernel.Types;
using Rhino.DocObjects;
using System;

namespace Meanders.Tools.Core
{
    public class ME_Object
    {
        public object Geometry { get; set; }

        public ObjectAttributes Attributes { get; set; }

        public ME_Object()
        {
            Geometry = null;
            Attributes = new ObjectAttributes();
        }

        public ME_Object(object geometry, ObjectAttributes attributes = null)
        {
            Geometry = geometry;
            Attributes = attributes != null
                ? attributes.Duplicate()
                : new ObjectAttributes();
        }

        public override string ToString()
        {
            return "ME Object";
        }
    }
}