using Microsoft.Xrm.Sdk.Metadata;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace LinkeD365.ERDBuilder
{
    internal class VisTable : BaseShape
    {
        //protected EntityMetadata tableMeta;
        private bool parent;
        private bool tableDisplayNames;
        public string TableName { get { return TableMeta.LogicalName; } }
        private string displayName;
        public string DisplayName
        {
            get
            {
                if (displayName == null)
                {
                    displayName = parent ? "PARENT: " : string.Empty;
                    if (!tableDisplayNames) displayName = displayName + TableMeta.LogicalName;
                    else displayName = displayName + TableMeta.DisplayName.UserLocalizedLabel.Label;
                }
                return displayName;
            }
        }

        public EntityMetadata TableMeta { get; internal set; }
        public string LogicalName { get { return TableMeta.LogicalName; } }
        public Table Table { get; set; }
        public VisTable(EntityMetadata tableMeta, bool parent, bool tableDisplayNames, double pinX, double pinY)
        {
            TableMeta = tableMeta;
            this.parent = parent;
            this.tableDisplayNames = tableDisplayNames;
            PinX = pinX;
            PinY = pinY;

            Shape = new XElement(GetTemplateShape("Table"));
            Utils.Shapes.Add(this);
            AddText(DisplayName);

            SetPosition();
        }

        internal void AddField(string fieldName)
         {
            var fieldText = Shape.Descendants().Where(el => el.Name.LocalName == "Text").Last();


            var sb = new StringBuilder(fieldText.Value);
            sb.AppendLine(fieldName);

            fieldText.ReplaceWith(XElement.Parse($"<Text><![CDATA[{sb.ToString()}]]></Text>", LoadOptions.SetBaseUri));

        }

        internal void Connect(VisTable child, string referencedAttribute, string schemaName)
        {
            AddLine(child, referencedAttribute, schemaName);
            //throw new NotImplementedException();
        }

        private void AddLine(VisTable child, string referencedAttribute, string schemaName)
        {
            var line = AddLine(child);

            line.AddProp("Parent", DisplayName);
            line.AddProp("Child", child.DisplayName);
            line.AddProp("Field", referencedAttribute);
            line.AddProp("Relationship", schemaName);

            if (Utils.ShowFK) AddFK(child, referencedAttribute, schemaName);
        }

        private void AddFK(VisTable child, string referencedAttribute, string schemaName)
        {

           // var fieldText = Shape.Descendants().Where(el => el.Name.LocalName == "Text").Last();
            var field = child.TableMeta.Attributes.FirstOrDefault(att => att.LogicalName == referencedAttribute);
            if (field == null) return;
            var displayName = "FK " + (Utils.ColumnsDisplayName ? field.DisplayName.UserLocalizedLabel.Label : field.LogicalName);
            child.AddField(displayName);
            //if (!fieldText.ToString().Contains(displayName)) return;
            //var sb = new StringBuilder(fieldText.Value == "Fields\n" ? "" : fieldText.Value);
            //sb.AppendLine(displayName);

            //fieldText.ReplaceWith(XElement.Parse($"<Text><![CDATA[{sb}]]></Text>", LoadOptions.SetBaseUri));

        }

        internal void ConnectMulti(VisTable child, string navPropParent, string navPropChild)
        {
            var line = AddLine(child);
            line.AddProp("Connected1Table", DisplayName);
            line.AddProp("Connected2Table", child.DisplayName);
            line.AddProp("Table1Navigation", navPropParent);
            line.AddProp("Table2Navigation", navPropChild);
        }

        internal void ClearFields()
        {
            var fieldText = Shape.Descendants().Where(el => el.Name.LocalName == "Text").Last();
            fieldText.ReplaceWith(XElement.Parse($"<Text></Text>", LoadOptions.SetBaseUri));
            // throw new NotImplementedException();
        }
    }
}
