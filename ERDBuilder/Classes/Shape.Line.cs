using System.Linq;
using System.Xml.Linq;

namespace LinkeD365.ERDBuilder
{
    public class Line : BaseShape
    {
        public Line() : base()
        {
            Shape = new XElement(Line);
            Shape.SetAttributeValue("NameU", "Line." + Id);
            
        }

        public BaseShape ChildShape { get; protected set; }

        private XElement connectStart;
        private XElement connectEnd;
        private int connectorNo;

        public XElement ConnectStart
        {
            get
            {
                if (connectStart == null)
                    connectStart = new XElement("Connect",
                        new XAttribute("FromSheet", Id),
                        new XAttribute("FromCell", "BeginX"),
                        new XAttribute("FromPart", "9"),
                        new XAttribute("ToSheet", ParentShape.Id),
                        new XAttribute("ToCell", "PinX"),//"Connections.X" + (connectorNo + 1)),
                        new XAttribute("ToPart", "3"));//101 + connectorNo) ;);
                return connectStart;
            }
        }

        public XElement ConnectEnd
        {
            get
            {
                if (connectEnd == null)
                    connectEnd = new XElement("Connect",
                        new XAttribute("FromSheet", Id),
                        new XAttribute("FromCell", "EndX"),
                        new XAttribute("FromPart", "12"),
                        new XAttribute("ToSheet", ChildShape.Id),
                        new XAttribute("ToCell", "PinX"),
                        new XAttribute("ToPart", "3")
                        );
                return connectEnd;
            }
        }

        public void Connect(BaseShape parent, BaseShape child, int current, int children)
        {
            Shape.Elements().Where(el => el.Attribute("N").Value == "BegTrigger").First()
                .SetAttributeValue("F", "_XFTRIGGER(Sheet." + parent.Id + "!EventXFMod)");
            Shape.Elements().Where(el => el.Attribute("N").Value == "EndTrigger").First()
                .SetAttributeValue("F", "_XFTRIGGER(Sheet." + child.Id + "!EventXFMod)");
            var connection = XElement.Parse("<Row T='Connection' IX='" + (current + 6) + "'>" +
                                        //  "<Cell N='X' V='0' U = 'MM' F = 'Width*1/5' />" +
                                        //    "<Cell N='X' F = 'Width*" + current + '/' + (children + 1) + "'/>" +
                                        "<Cell N='Y' F = 'Height*1/" + (current + 1) + "'/>" +
                                            //  "<Cell N = 'Y' V = '0' U = 'MM' F = 'Height*0' />" +
                                            "<Cell N = 'X' V = '0' U = 'MM' F = 'Width' />" +
                                            "<Cell N = 'DirX' V = '0' />" +
                                            "<Cell N = 'DirY' V = '0' />" +
                                            "<Cell N = 'Type' V = '0' />" +
                                            "<Cell N = 'AutoGen' V = '0' />" +
                                            "<Cell N = 'Prompt' V = '' F = 'No Formula' />" +
                                            "</Row>");
            ParentShape = parent;
            ChildShape = child;
            connectorNo = current;

            ParentShape.Connections.Add(connection);

            Utils.Connects.Add(ConnectStart);
            Utils.Connects.Add(ConnectEnd);
        }
    }
}
