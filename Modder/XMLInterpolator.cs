using System.Xml;

namespace Modder
{
    public class XMLInterpolator
    {
        Dictionary<string, string> Replacements { get; set; }
        public XMLInterpolator(string path, string here)
        {
            Replacements = new()
            {
                {"DEFAULT:PATH", path},
                {"DEFAULT:HERE", here}
            };
        }
        public Dictionary<string, string> Interpolate(XmlDocument doc)
        {
            if (doc.DocumentElement == null)
                return Replacements;

            foreach(XmlNode node in doc.DocumentElement.ChildNodes)
                this.InterpolateNode(node);

            return Replacements;
        }

        public void InterpolateNode(XmlNode node, string preText = "")
        {
            if (node.NodeType == XmlNodeType.Element)
            {
                if (node.Attributes != null)
                    foreach(XmlAttribute attribute in node.Attributes)
                        this.InterpolateValue(attribute.Value, preText, false);

                foreach(XmlNode childNode in node.ChildNodes)
                    this.InterpolateNode(childNode, preText + node.Name + ":");
            }
            else if (node.NodeType == XmlNodeType.Text)
                this.InterpolateValue(node.Value, preText);
        }

        protected string InterpolateValue(string? value, string preText, bool save = true)
        {
            string newValue = this.InterpolateText(value);
            if (save)
                Replacements.Add(preText.Remove(preText.Length - 1), newValue);
            return newValue;
        }

        protected string InterpolateText(string? value)
        {
            if (value == null)
                return "";

            return Utils.Interpolate(value, Replacements);
        }
    }
}
