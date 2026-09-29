using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Meanders_tools
{
    public class ME_Object_Param :
        GH_Param<ME_Object_Goo>,
        IGH_PreviewObject,
        IGH_BakeAwareObject
    {
        public ME_Object_Param()
            : base(
                "ME Object",
                "ME Obj",
                "Meanders Object",
                "Meanders Tools",
                "Objects",
                GH_ParamAccess.tree)
        {
        }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid(
                    "A7F3C2D1-6E54-4B91-9F28-21D8E6C04A73");
            }
        }

        protected override Bitmap Icon
        {
            get { return null; }
        }

        public bool IsPreviewCapable
        {
            get { return true; }
        }

        public bool Hidden
        {
            get { return false; }
            set { }
        }

        public void DrawViewportMeshes(
            IGH_PreviewArgs args)
        {
            if (VolatileData == null)
                return;

            foreach (ME_Object_Goo meObject
                in VolatileData.AllData(true))
            {
                meObject.DrawViewportMeshes(
                    new GH_PreviewMeshArgs(
                        args.Viewport,
                        args.Display,
                        new Rhino.Display.DisplayMaterial(
                            Color.LightGray),
                        Rhino.Geometry.MeshingParameters.Default));
            }
        }

        public void DrawViewportWires(
            IGH_PreviewArgs args)
        {
            if (VolatileData == null)
                return;

            foreach (ME_Object_Goo meObject
                in VolatileData.AllData(true))
            {
                meObject.DrawViewportWires(
                    new GH_PreviewWireArgs(
                        args.Viewport,
                        args.Display,
                        Color.Black,
                        1));
            }
        }

        public Rhino.Geometry.BoundingBox ClippingBox
        {
            get
            {
                Rhino.Geometry.BoundingBox box =
                    Rhino.Geometry.BoundingBox.Empty;

                if (VolatileData == null)
                    return box;

                foreach (ME_Object_Goo meObject
                    in VolatileData.AllData(true))
                {
                    box.Union(meObject.ClippingBox);
                }

                return box;
            }
        }

        public bool IsBakeCapable
        {
            get { return true; }
        }

        public void BakeGeometry(
            RhinoDoc doc,
            List<Guid> obj_ids)
        {
            if (doc == null ||
                VolatileData == null)
                return;

            foreach (ME_Object_Goo meObject
                in VolatileData.AllData(true))
            {
                Guid objGuid;

                if (meObject.BakeGeometry(
                    doc,
                    null,
                    out objGuid))
                {
                    obj_ids.Add(objGuid);
                }
            }
        }

        public void BakeGeometry(
            RhinoDoc doc,
            Rhino.DocObjects.ObjectAttributes att,
            List<Guid> obj_ids)
        {
            if (doc == null ||
                VolatileData == null)
                return;

            foreach (ME_Object_Goo meObject
                in VolatileData.AllData(true))
            {
                Guid objGuid;

                if (meObject.BakeGeometry(
                    doc,
                    att,
                    out objGuid))
                {
                    obj_ids.Add(objGuid);
                }
            }
        }
    }
}