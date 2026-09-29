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
        GH_GeometricGoo<ME_Object>,
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

        public override string ToString()
        {
            return Value != null
                ? Value.ToString()
                : "Null ME Object";
        }

        public override BoundingBox Boundingbox
        {
            get
            {
                if (Value == null ||
                    Value.Geometry == null)
                {
                    return BoundingBox.Empty;
                }

                if (Value.Geometry is GH_GeometryGroup ghGroup)
                {
                    return ghGroup.Boundingbox;
                }

                return GetBoundingBox(
                    Value.Geometry);
            }
        }

        public BoundingBox ClippingBox
        {
            get
            {
                return Boundingbox;
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

        public override IGH_GeometricGoo DuplicateGeometry()
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

        public override BoundingBox GetBoundingBox(
            Transform xform)
        {
            if (Value == null ||
                Value.Geometry == null)
            {
                return BoundingBox.Empty;
            }

            if (Value.Geometry is GH_GeometryGroup ghGroup)
            {
                return ghGroup.GetBoundingBox(xform);
            }

            BoundingBox box =
                GetBoundingBox(Value.Geometry);

            if (box.IsValid)
                box.Transform(xform);

            return box;
        }

        public override IGH_GeometricGoo Transform(
            Transform xform)
        {
            if (Value == null ||
                Value.Geometry == null)
            {
                return new ME_Object_Goo();
            }

            if (Value.Geometry is GH_GeometryGroup ghGroup)
            {
                IGH_GeometricGoo transformed =
                    ghGroup.Transform(xform);

                if (transformed == null)
                    return null;

                return new ME_Object_Goo(
                    new ME_Object(
                        transformed,
                        Value.Attributes
                    )
                );
            }

            object geometry =
                Value.Geometry;

            if (geometry is GeometryBase rhinoGeometry)
            {
                GeometryBase duplicate =
                    rhinoGeometry.Duplicate();

                duplicate.Transform(xform);

                return new ME_Object_Goo(
                    new ME_Object(
                        duplicate,
                        Value.Attributes
                    )
                );
            }

            if (geometry is Point3d point)
            {
                Point3d transformedPoint = point;
                transformedPoint.Transform(xform);

                return new ME_Object_Goo(
                    new ME_Object(
                        transformedPoint,
                        Value.Attributes
                    )
                );
            }

            if (geometry is Rectangle3d rectangle)
            {
                Rectangle3d transformedRectangle =
                    rectangle;

                transformedRectangle.Transform(xform);

                return new ME_Object_Goo(
                    new ME_Object(
                        transformedRectangle,
                        Value.Attributes
                    )
                );
            }

            return new ME_Object_Goo(
                new ME_Object(
                    geometry,
                    Value.Attributes
                )
            );
        }

        public override IGH_GeometricGoo Morph(
            SpaceMorph xmorph)
        {
            if (Value == null ||
                Value.Geometry == null)
            {
                return new ME_Object_Goo();
            }

            if (Value.Geometry is GH_GeometryGroup ghGroup)
            {
                IGH_GeometricGoo morphed =
                    ghGroup.Morph(xmorph);

                if (morphed == null)
                    return null;

                return new ME_Object_Goo(
                    new ME_Object(
                        morphed,
                        Value.Attributes
                    )
                );
            }

            object geometry =
                Value.Geometry;

            if (geometry is GeometryBase rhinoGeometry)
            {
                GeometryBase duplicate =
                    rhinoGeometry.Duplicate();

                if (xmorph.Morph(duplicate))
                {
                    return new ME_Object_Goo(
                        new ME_Object(
                            duplicate,
                            Value.Attributes
                        )
                    );
                }
            }

            return new ME_Object_Goo(
                new ME_Object(
                    geometry,
                    Value.Attributes
                )
            );
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
            if (Value == null)
                return base.CastTo(ref target);

            if (typeof(Q).IsAssignableFrom(
                typeof(ME_Object)))
            {
                object obj = Value;
                target = (Q)obj;
                return true;
            }

            if (Value.Geometry != null)
            {
                // unwrap geometry group
                if (Value.Geometry is GH_GeometryGroup group)
                {
                    if (typeof(Q).IsAssignableFrom(typeof(GH_GeometryGroup)))
                    {
                        object obj = group;
                        target = (Q)obj;
                        return true;
                    }
                }

                if (typeof(Q).IsAssignableFrom(
                    Value.Geometry.GetType()))
                {
                    object geometry = Value.Geometry;
                    target = (Q)geometry;
                    return true;
                }
            }

            return base.CastTo(ref target);
        }

        public override bool IsGeometryLoaded
        {
            get
            {
                if (Value == null ||
                    Value.Geometry == null)
                {
                    return false;
                }

                if (Value.Geometry is IGH_GeometricGoo geometricGoo)
                    return geometricGoo.IsGeometryLoaded;

                return true;
            }
        }

        public override bool IsReferencedGeometry
        {
            get
            {
                if (Value == null ||
                    Value.Geometry == null)
                {
                    return false;
                }

                if (Value.Geometry is IGH_GeometricGoo geometricGoo)
                    return geometricGoo.IsReferencedGeometry;

                return false;
            }
        }

        public override Guid ReferenceID
        {
            get
            {
                if (Value == null ||
                    Value.Geometry == null)
                {
                    return Guid.Empty;
                }

                if (Value.Geometry is IGH_GeometricGoo geometricGoo)
                    return geometricGoo.ReferenceID;

                return Guid.Empty;
            }

            set
            {
                if (Value == null ||
                    Value.Geometry == null)
                    return;

                if (Value.Geometry is IGH_GeometricGoo geometricGoo)
                    geometricGoo.ReferenceID = value;
            }
        }

        public override void ClearCaches()
        {
            if (Value == null ||
                Value.Geometry == null)
                return;

            if (Value.Geometry is IGH_GeometricGoo geometricGoo)
                geometricGoo.ClearCaches();
        }

        public override bool LoadGeometry(
    RhinoDoc doc)
        {
            if (Value == null ||
                Value.Geometry == null)
                return false;

            if (Value.Geometry is IGH_GeometricGoo geometricGoo)
                return geometricGoo.LoadGeometry(doc);

            return true;
        }

        public void DrawViewportMeshes(
            GH_PreviewMeshArgs args)
        {
            if (Value == null ||
                Value.Geometry == null)
                return;

            if (Value.Geometry is GH_GeometryGroup ghGroup)
            {
                ghGroup.DrawViewportMeshes(args);
                return;
            }

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

            if (Value.Geometry is GH_GeometryGroup ghGroup)
            {
                ghGroup.DrawViewportWires(args);
                return;
            }

            DrawWires(
                args.Pipeline,
                Value.Geometry);
        }

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

            if (Value.Geometry is GH_GeometryGroup ghGroup)
            {
                return ghGroup.BakeGeometry(
                    doc,
                    bakeAttributes,
                    ref objGuid);
            }

            object geometry =
                Value.Geometry;

            if (geometry is SubD subD)
            {
                objGuid =
                    doc.Objects.AddSubD(
                        subD,
                        bakeAttributes);

                return objGuid != Guid.Empty;
            }

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

        private BoundingBox GetBoundingBox(
            object geometry)
        {
            if (geometry is GeometryBase rhinoGeometry)
            {
                return rhinoGeometry.GetBoundingBox(true);
            }

            if (geometry is BoundingBox bbox)
            {
                return bbox;
            }

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

            if (geometry is Rectangle3d rectangle)
            {
                return rectangle.ToNurbsCurve()
                    .GetBoundingBox(true);
            }

            return BoundingBox.Empty;
        }

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

            if (geometry is SubD subD)
            {
                Mesh subDMesh =
                    Mesh.CreateFromSubD(
                        subD,
                        2);

                if (subDMesh != null)
                {
                    display.DrawMeshShaded(
                        subDMesh,
                        new DisplayMaterial(
                            Color.LightGray));
                }

                return;
            }

            if (geometry is Rectangle3d rectangle)
            {
                Brep[] rectangleBreps =
                    Brep.CreatePlanarBreps(
                        rectangle.ToNurbsCurve(),
                        RhinoDoc.ActiveDoc != null
                            ? RhinoDoc.ActiveDoc.ModelAbsoluteTolerance
                            : 0.01);

                if (rectangleBreps != null &&
                    rectangleBreps.Length > 0)
                {
                    display.DrawBrepShaded(
                        rectangleBreps[0],
                        new DisplayMaterial(
                            Color.LightGray));
                }

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

            if (geometry is SubD subD)
            {
                Mesh subDMesh =
                    Mesh.CreateFromSubD(
                        subD,
                        2);

                if (subDMesh != null)
                {
                    display.DrawMeshWires(
                        subDMesh,
                        Color.Black);
                }

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

                return;
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