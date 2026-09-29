using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino;
using Rhino.Display;
using Rhino.DocObjects;
using Rhino.Geometry;
using System;
using System.Drawing;

namespace Meanders_tools
{
    public class ME_Object_Goo :
        GH_Goo<ME_Object>,
        IGH_PreviewData,
        IGH_BakeAwareData
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
            get
            {
                return Value != null &&
                       Value.Geometry != null;
            }
        }

        public override string TypeName
        {
            get { return "ME Object"; }
        }

        public override string TypeDescription
        {
            get
            {
                return "Meanders Object with geometry and attributes.";
            }
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
            return Value != null
                ? Value.ToString()
                : "Null ME Object";
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

            if (Value != null &&
                Value.Geometry != null &&
                typeof(Q).IsAssignableFrom(
                    Value.Geometry.GetType()))
            {
                object geometry = Value.Geometry;
                target = (Q)geometry;
                return true;
            }

            return base.CastTo(ref target);
        }

        // ------------------------------------------------------------
        // Preview
        // ------------------------------------------------------------

        public BoundingBox ClippingBox
        {
            get
            {
                if (Value == null ||
                    Value.Geometry == null)
                    return BoundingBox.Empty;

                return GetBoundingBox(
                    Value.Geometry);
            }
        }

        public void DrawViewportMeshes(
            GH_PreviewMeshArgs args)
        {
            if (Value == null ||
                Value.Geometry == null)
                return;

            DrawMeshes(
                args.Pipeline,
                Value.Geometry);
        }

        public void DrawViewportWires(
            GH_PreviewWireArgs args)
        {
            if (Value == null ||
                Value.Geometry == null)
                return;

            DrawWires(
                args.Pipeline,
                Value.Geometry);
        }

        // ------------------------------------------------------------
        // Bake
        // ------------------------------------------------------------

        public bool BakeGeometry(
            RhinoDoc doc,
            ObjectAttributes att,
            out Guid objGuid)
        {
            objGuid = Guid.Empty;

            if (doc == null ||
                Value == null ||
                Value.Geometry == null)
            {
                return false;
            }

            ObjectAttributes bakeAttributes;

            if (Value.Attributes != null)
            {
                bakeAttributes =
                    Value.Attributes.Duplicate();
            }
            else if (att != null)
            {
                bakeAttributes =
                    att.Duplicate();
            }
            else
            {
                bakeAttributes =
                    new ObjectAttributes();
            }

            object geometry =
                Value.Geometry;

            if (geometry is GeometryBase rhinoGeometry)
            {
                objGuid =
                    doc.Objects.Add(
                        rhinoGeometry,
                        bakeAttributes);

                return objGuid != Guid.Empty;
            }

            if (geometry is Point3d point)
            {
                objGuid =
                    doc.Objects.AddPoint(
                        point,
                        bakeAttributes);

                return objGuid != Guid.Empty;
            }
            if (geometry is Rectangle3d rectangle)
            {
                objGuid =
                    doc.Objects.AddCurve(
                        rectangle.ToNurbsCurve(),
                        bakeAttributes);

                return objGuid != Guid.Empty;
            }

            return false;
        }

        // ------------------------------------------------------------
        // Bounding Box
        // ------------------------------------------------------------

        private BoundingBox GetBoundingBox(
            object geometry)
        {
            if (geometry is GeometryBase rhinoGeometry)
                return rhinoGeometry.GetBoundingBox(true);

            if (geometry is BoundingBox bbox)
                return bbox;

            if (geometry is Rhino.Geometry.Point point)
            {
                return new BoundingBox(
                    point.Location,
                    point.Location);
            }

            if (geometry is Point3d point3d)
            {
                return new BoundingBox(
                    point3d,
                    point3d);
            }

            return BoundingBox.Empty;
        }

        // ------------------------------------------------------------
        // Preview Drawing
        // ------------------------------------------------------------

        private void DrawMeshes(
            DisplayPipeline display,
            object geometry)
        {
            if (geometry is Mesh mesh)
            {
                display.DrawMeshShaded(
                    mesh,
                    new DisplayMaterial(
                        Color.LightGray));

                return;
            }

            if (geometry is Brep brep)
            {
                display.DrawBrepShaded(
                    brep,
                    new DisplayMaterial(
                        Color.LightGray));

                return;
            }
        }

        private void DrawWires(
            DisplayPipeline display,
            object geometry)
        {
            if (geometry is Mesh mesh)
            {
                display.DrawMeshWires(
                    mesh,
                    Color.Black);

                return;
            }

            if (geometry is Brep brep)
            {
                display.DrawBrepWires(
                    brep,
                    Color.Black);

                return;
            }

            if (geometry is Curve curve)
            {
                display.DrawCurve(
                    curve,
                    Color.Black,
                    1);

                return;
            }

            if (geometry is Rhino.Geometry.Point point)
            {
                display.DrawPoint(
                    point.Location,
                    PointStyle.Simple,
                    3,
                    Color.Black);

                return;
            }

            if (geometry is Point3d point3d)
            {
                display.DrawPoint(
                    point3d,
                    PointStyle.Simple,
                    3,
                    Color.Black);
            }
            if (geometry is Rectangle3d rectangle)
            {
                display.DrawCurve(
                    rectangle.ToNurbsCurve(),
                    Color.Black,
                    1);

                return;
            }
        }
    }
}