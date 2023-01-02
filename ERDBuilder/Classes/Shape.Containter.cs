using LinkeD365.ERDBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace inkeD365.ERDBuilder
{
    public class VisContainer : BaseShape
    {
        public string ContainerName { get; private set; }
        public VisContainer(string containerName)
        {
            ContainerName = containerName;
            Shape = new XElement(GetTemplateShape("Container"));

            Utils.Shapes.Add(this);
        }

        internal void AddShape(VisTable visTable)
        {
            XElement relationshipField;
            try
            {
                relationshipField = Shape.Elements().FirstOrDefault(el => el.Attribute("N").Value == "Relationships");
            }
            catch (Exception)
            {

                relationshipField = new XElement(Shape.GetDefaultNamespace() + "Cell");
                relationshipField.SetAttributeValue("N", "Relationships");

                Shape.Add(relationshipField);
            }


            //if (relationshipField == null)
            //{
            //    relationshipField = new XElement("Cell");
            //    relationshipField.Attribute("N").Value = "Relationships";

            //    Shape.Add(relationshipField);
            //}
            relationshipField.SetAttributeValue("V", 0);
            if (relationshipField.Attribute("F") == null) relationshipField.SetAttributeValue("F", "SUM(DEPENDSON(1,Sheet." + visTable.Id + "!SheetRef()))");

            try { relationshipField = visTable.Shape.Elements().FirstOrDefault(el => el.Attribute("N").Value == "Relationships"); }
            catch(Exception)
            {
                relationshipField = new XElement(visTable.Shape.GetDefaultNamespace() + "Cell");
                relationshipField.SetAttributeValue("N", "Relationships");

                visTable.Shape.Add(relationshipField);
            }

            relationshipField.SetAttributeValue("V", 0);
            relationshipField.SetAttributeValue("F", "SUM(DEPENDSON(1,Sheet." + this.Id + "!SheetRef()))");
         //   if (relationshipField.Attribute("F") == null) relationshipField.SetAttributeValue("F", "SUM(DEPENDSON(1,Sheet." + visTable.Id + "!SheetRef()))");

            // if (relationshipField.SetAttributeValue("F", relationshipField))
        }
    }
}
