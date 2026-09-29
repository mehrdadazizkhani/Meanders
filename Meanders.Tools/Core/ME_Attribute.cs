using Rhino;
using Rhino.DocObjects;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;

namespace Meanders.Tools.Core
{
    public class ME_Attribute
    {
        public ObjectAttributes Attributes { get; private set; }

        public ME_Attribute()
        {
            Attributes = new ObjectAttributes();
        }

        public ME_Attribute(ObjectAttributes attributes)
        {
            Attributes = attributes != null
                ? attributes.Duplicate()
                : new ObjectAttributes();
        }

        public ME_Attribute Duplicate()
        {
            return new ME_Attribute(Attributes);
        }

        // ---------------------------------------------------------
        // Name
        // ---------------------------------------------------------

        public string Name
        {
            get { return Attributes.Name ?? string.Empty; }
            set { Attributes.Name = value ?? string.Empty; }
        }

        // ---------------------------------------------------------
        // Layer
        // ---------------------------------------------------------

        public string Layer
        {
            get
            {
                RhinoDoc doc = RhinoDoc.ActiveDoc;

                if (doc == null)
                    return string.Empty;

                int layerIndex = Attributes.LayerIndex;

                if (layerIndex < 0)
                    return string.Empty;

                Layer layer = doc.Layers.FindIndex(layerIndex);

                if (layer == null)
                    return string.Empty;

                return layer.FullPath;
            }

            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    return;

                RhinoDoc doc = RhinoDoc.ActiveDoc;

                if (doc == null)
                    return;

                int layerIndex = doc.Layers.FindByFullPath(value, -1);

                if (layerIndex < 0)
                {
                    Layer newLayer = new Layer();
                    newLayer.Name = value;

                    layerIndex = doc.Layers.Add(newLayer);

                    if (layerIndex < 0)
                        return;
                }

                Attributes.LayerIndex = layerIndex;
            }
        }

        // ---------------------------------------------------------
        // Object Color
        // ---------------------------------------------------------

        public Color ObjectColor
        {
            get { return Attributes.ObjectColor; }

            set
            {
                Attributes.ObjectColor = value;
                Attributes.ColorSource =
                    ObjectColorSource.ColorFromObject;
            }
        }

        // ---------------------------------------------------------
        // User Text
        // ---------------------------------------------------------

        public void SetUserText(string key, string value)
        {
            if (string.IsNullOrWhiteSpace(key))
                return;

            if (string.IsNullOrEmpty(value))
                Attributes.DeleteUserString(key);
            else
                Attributes.SetUserString(key, value);
        }

        public string GetUserText(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                return string.Empty;

            string value = Attributes.GetUserString(key);

            return value ?? string.Empty;
        }

        public Dictionary<string, string> GetUserText()
        {
            Dictionary<string, string> result =
                new Dictionary<string, string>();

            NameValueCollection userStrings =
                Attributes.GetUserStrings();

            if (userStrings == null)
                return result;

            foreach (string key in userStrings.AllKeys)
            {
                if (key == null)
                    continue;

                string value = userStrings[key];

                result[key] = value ?? string.Empty;
            }

            return result;
        }

        // ---------------------------------------------------------
        // Apply User Text
        // ---------------------------------------------------------

        public void SetUserText(
            List<string> keys,
            List<string> values)
        {
            if (keys == null || values == null)
                return;

            if (keys.Count != values.Count)
                return;

            for (int i = 0; i < keys.Count; i++)
            {
                SetUserText(keys[i], values[i]);
            }
        }

        // ---------------------------------------------------------
        // String representation
        // ---------------------------------------------------------

        public override string ToString()
        {
            return "ME Attribute";
        }
    }
}