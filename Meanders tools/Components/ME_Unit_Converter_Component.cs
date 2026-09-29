using Grasshopper.Kernel;
using System;
using System.Drawing;

namespace Meanders_tools
{
    public class ME_Unit_Converter_Component : GH_Component
    {
        private ME_UnitConverter.LengthUnit _fromUnit =
            ME_UnitConverter.LengthUnit.Millimeter;

        private ME_UnitConverter.LengthUnit _toUnit =
            ME_UnitConverter.LengthUnit.Centimeter;


        public ME_Unit_Converter_Component()
            : base(
                "ME Unit Converter",
                "ME Units",
                "Convert between different measurement units.",
                "Meanders Tools",
                "Units")
        {
        }


        protected override void RegisterInputParams(
            GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter(
                "Value",
                "X",
                "Value to convert.",
                GH_ParamAccess.item);
        }


        protected override void RegisterOutputParams(
            GH_OutputParamManager pManager)
        {
            pManager.AddNumberParameter(
                "Result",
                "Y",
                "Converted value.",
                GH_ParamAccess.item);
        }


        protected override void SolveInstance(
            IGH_DataAccess DA)
        {
            double value = 0;

            if (!DA.GetData(0, ref value))
                return;


            double result =
                ME_UnitConverter.ConvertLength(
                    value,
                    _fromUnit,
                    _toUnit);


            DA.SetData(0, result);
        }


        protected override void AppendAdditionalComponentMenuItems(
            System.Windows.Forms.ToolStripDropDown menu)
        {
            base.AppendAdditionalComponentMenuItems(menu);


            var fromMenu =
                Menu_UnitMenu(
                    "From Unit",
                    true);

            menu.Items.Add(fromMenu);


            var toMenu =
                Menu_UnitMenu(
                    "To Unit",
                    false);

            menu.Items.Add(toMenu);
        }


        private System.Windows.Forms.ToolStripMenuItem Menu_UnitMenu(
            string title,
            bool from)
        {
            var parent =
                new System.Windows.Forms.ToolStripMenuItem(title);


            foreach (
                ME_UnitConverter.LengthUnit unit
                in Enum.GetValues(
                    typeof(ME_UnitConverter.LengthUnit)))
            {
                var item =
                    new System.Windows.Forms.ToolStripMenuItem(
                        unit.ToString());


                item.Click += (sender, e) =>
                {
                    if (from)
                        _fromUnit = unit;
                    else
                        _toUnit = unit;


                    ExpireSolution(true);
                };


                parent.DropDownItems.Add(item);
            }


            return parent;
        }


        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.primary; }
        }


        protected override Bitmap Icon
        {
            get { return null; }
        }


        public override Guid ComponentGuid
        {
            get
            {
                return new Guid(
                    "5A4E9E1D-4B3D-4D0B-8E4B-1C9F9F0D3A71");
            }
        }
    }
}