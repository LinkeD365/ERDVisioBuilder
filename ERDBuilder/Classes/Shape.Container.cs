using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;


namespace LinkeD365.ERDBuilder
{
    public partial class VisContainer : BaseShape
    {
        public string ContainerName { get; private set; }


        private List<VisTable> VisTables { get; set; } = new List<VisTable>();

        private double Width
        {
            get
            {
                return Double.Parse(Shape.Elements().First(el => el.Attribute("N").Value == "Width").Attribute("V").Value);
            }
            set { Shape.Elements().First(el => el.Attribute("N").Value == "Width").SetAttributeValue("V", value); }
        }

        private double Height
        {
            get
            {
                return Double.Parse(Shape.Elements().First(el => el.Attribute("N").Value == "Height").Attribute("V").Value);
            }
            set { Shape.Elements().First(el => el.Attribute("N").Value == "Height").SetAttributeValue("V", value); }
        }


        public VisContainer(string containerName, string colour, double pinX, double pinY)
        {
            ContainerName = containerName;
            Shape = new XElement(GetTemplateShape("Container"));
            //Utils.Shapes.Add(this);
            AddText(containerName);
            ChangeColour(colour);
            PinX = pinX; PinY = pinY;
            SetPosition();
        }

        private void ChangeColour(string colour)
        {
            XElement shapeNode = Shape.Elements(Shape.GetDefaultNamespace() + "Shapes").First().FirstNode as XElement;
            //   Shape.Elements().First(el => el.Attribute("N").Value == "FillForegnd").SetAttributeValue("V", colour);
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(new NameTable());
            nsmgr.AddNamespace("NS", NS.NamespaceName);
            foreach (var el in shapeNode.XPathSelectElements(".//NS:Section/NS:Row/NS:Cell[@V='#cdd9ed']", nsmgr)) // find all the standard colour & change to new one
            {
                el.SetAttributeValue("V", colour);
                el.SetAttributeValue("F", null);
            }
        }

        internal void AddShape(VisTable table)
        {
            VisTables.Add(table);
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(new NameTable());
            nsmgr.AddNamespace("NS", NS.NamespaceName);
            XElement relContainer = Shape.XPathSelectElement(".//NS:Cell[@N='Relationships']", nsmgr);
            if (relContainer == null)
            {
                relContainer = new XElement(Shape.GetDefaultNamespace() + "Cell");
                relContainer.SetAttributeValue("N", "Relationships");
                relContainer.SetAttributeValue("V", 0);
                Shape.Add(relContainer);
            }
            relContainer.SetAttributeValue("F", "SUM(DEPENDSON(1,Sheet."  + String.Join("!SheetRef(),Sheet.", VisTables.Select(tbl => tbl.Id.ToString())).ToString() + "!SheetRef()))");

            XElement relTable = table.Shape.XPathSelectElement(".//NS:Cell[@N='Relationships']", nsmgr);
            if (relTable == null)
            {
                relTable = new XElement(Shape.GetDefaultNamespace() + "Cell");
                relTable.SetAttributeValue("N", "Relationships");
                relTable.SetAttributeValue("V", 0);
                table.Shape.Add(relTable);
            }
            relTable.SetAttributeValue("F", "SUM(DEPENDSON(4,Sheet." + this.Id + "!SheetRef()))");

            table.PinX = SetTableX();
            table.PinY = SetTableY();
            table.SetPosition();

            ExpandContainer();
        }

        private void ExpandContainer()
        {
            if (VisTables.Min(t => t.PinX) <= PinX) PinX = VisTables.Min(t => t.PinX) - xBuffer;

            if (VisTables.Max(t => t.PinX) + xWidth > PinX + Width) Width = -PinX + VisTables.Max(t => t.PinX) + xWidth + xBuffer;

            if (VisTables.Max(t => t.PinY) + yHeight > PinY + Height) Height = (VisTables.Max(t => t.PinY) + yHeight + yBuffer) - PinY;
            // if (table.PinX + xMultiplier) > this.
            SetPosition();
        }

        private double SetTableX()
        {
            switch (VisTables.Count)
            {
                case 1:
                case 4:
                case 6:
                case 9:
                case 12:
                case 15:
                    return PinX + xBuffer;
                case 2:
                case 3:
                case 8:
                case 11:
                case 14:
                    return PinX + xBuffer + xWidth;
                case 5:
                    return PinX - xWidth;
                case 7:
                case 10:
                case 13:
                    return PinX + 2 * (xBuffer + xWidth);
                default:
                    return PinX;

            }
        }

        private double SetTableY()
        {
            switch (VisTables.Count)
            {
                case 1:
                case 2:
                case 6:
                    return PinY + yBuffer;
                case 3:
                case 4:
                case 5:
                    return PinY + yBuffer + yHeight;

                case 7:
                case 8:
                case 9:
                    return PinY + 2 * (yBuffer + yHeight);
                case 10:
                case 11:
                case 12:
                    return PinY + 3 * (yBuffer + yHeight);
                case 13:
                case 14:
                case 15:
                    return PinY + 4 * (yBuffer + yHeight);
                default: return PinY;
            }
        }
    }
}
