using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
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

        public VisTable(EntityMetadata tableMeta, bool parent, bool tableDisplayNames, double pinX, double pinY)
        {
            this.TableMeta = tableMeta;
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


            var sb = new StringBuilder(fieldText.Value == "Fields\n" ? "" : fieldText.Value);
            sb.AppendLine(fieldName);

            fieldText.ReplaceWith(XElement.Parse($"<Text><![CDATA[{sb}]]></Text>", LoadOptions.SetBaseUri));

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
