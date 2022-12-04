using ERDBuilder.Properties;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Extensions;
//using LinkeD365.ERDBuilder.Properties;
using Microsoft.Xrm.Sdk.Metadata;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Reflection;
using System.Web.Services.Description;
using System.Xml.Linq;

namespace LinkeD365.ERDBuilder
{

    public static partial class Utils
    {
        private const string aiEndpoint = "https://dc.services.visualstudio.com/v2/track";
        private static double pageWidth = 11;
        private static double pageHeight = 8;
        private const string aiKey = "cc383234-dfdb-429a-a970-d17847361df3";
        private static AppInsights ai;

        public static AppInsights Ai
        {
            get
            {
                if (ai == null)
                {
                    ai = new AppInsights(aiEndpoint, aiKey, Assembly.GetExecutingAssembly());
                }
                return ai;
            }
        }
        public static IOrganizationService Service { get; internal set; }
        public static bool TableDisplayNames { get; internal set; }
        public static bool ColumnsDisplayName { get; internal set; }
        public static bool OnlyBetweenTable { get; internal set; }
        public static bool OneToMany { get; internal set; }
        public static bool ManyToOne { get; internal set; }
        public static bool ManyToMany { get; internal set; }

        public static bool HideActivity { get; internal set; }
        private static double xMultiplier = 1.8;
        private static double yMultiplier = 1.2;
        public static List<string> VisioTemplates = new List<string>() { "Table", "Connector" };
        private static double nextX
        {
            get
            {
                switch (addedTables.Count)
                {
                    case 1:
                    case 2:
                    case 8:
                    case 13:
                    case 23:
                    case 32:
                    case 47:
                    case 61:
                    case 79:
                        return (pageWidth / 2) + xMultiplier;

                    case 3:
                    case 7:
                    case 14:
                    case 22:
                    case 33:
                    case 46:
                    case 62:
                    case 78:
                        return (pageWidth / 2);

                    case int i when (i >= 4 && i <= 6):
                    case 15:
                    case 21:
                    case 34:
                    case 45:
                    case 63:
                    case 77:
                        return (pageWidth / 2) - xMultiplier;

                    case int i when (i >= 9 && i <= 12):
                    case 24:
                    case 31:
                    case 48:
                    case 60:
                    case 80:
                        return (pageWidth / 2) + (2 * xMultiplier);

                    case int i when (i >= 25 && i <= 30):
                    case 49:
                    case 58:
                    case 81:

                        return (pageWidth / 2) + (3 * xMultiplier);

                    case int i when (i >= 16 && i <= 20):
                    case 44:
                    case 35:
                    case 76:
                    case 64:
                        return (pageWidth / 2) - (2 * xMultiplier);

                    case int i when (i >= 36 && i <= 43):
                    case 65:
                    case 75:
                        return (pageWidth / 2) - (3 * xMultiplier);

                    case int i when (i >= 50 && i <= 57):
                    case 82:
                        return (pageWidth / 2) + (4 * xMultiplier);

                    case int i when (i >= 66 && i <= 74):

                        return (pageWidth / 2) - (4 * xMultiplier);

                    default:
                        return (pageWidth / 2) + (5 * xMultiplier);
                }
            }
        }

        private static double nextY
        {
            get
            {
                switch (addedTables.Count)
                {
                    case 1:
                    case 5:
                    case 10:
                    case 18:
                    case 40:
                    case 53:
                    case 70:
                        return (pageHeight / 2);

                    case int i when (i >= 2 && i <= 4):
                    case 11:
                    case 17:
                    case 28:
                    case 38:
                    case 54:
                    case 69:
                        return (pageHeight / 2) + (1 * yMultiplier);

                    case int i when (i >= 6 && i <= 9):
                    case 19:
                    case 26:
                    case 41:
                    case 52:
                    case 71:
                        return (pageHeight / 2) - (1 * yMultiplier);

                    case int i when (i >= 12 && i <= 16):
                    case 29:
                    case 37:
                    case 55:
                    case 68:
                        return (pageHeight / 2) + (2 * yMultiplier);

                    case int i when (i >= 20 && i <= 25):
                    case 42:
                    case 51:
                    case 72:
                        return (pageHeight / 2) - (2 * yMultiplier);

                    case int i when (i >= 30 && i <= 36):
                    case 56:
                    case 67:

                        return (pageHeight / 2) + (3 * yMultiplier);

                    case int i when (i >= 43 && i <= 50):
                    case 73:
                        return (pageHeight / 2) - (3 * yMultiplier);

                    case int i when (i >= 57 && i <= 66):
                        return (pageHeight / 2) + (4 * yMultiplier);

                    case int i when (i >= 74 && i <= 82):
                        return (pageHeight / 2) - (4 * yMultiplier);

                    default:
                        return (pageHeight / 2) + (5 * yMultiplier);
                }
            }
        }
        public static int ActionCount { get; set; }

        private static Package templatePackage;

        private static PackagePart templateDocument;

        private static PackagePart templatePages;
        private static PackagePart templatePage;

        public static XDocument XmlPage;
        private static List<VisTable> addedTables;
        private static JObject wfObject;
        public static List<BaseShape> Shapes;

        //public static List<StageNode> StageNodes;
        private static XElement connects;
        private static List<string> addedMtoM = new List<string>();

        public static XElement Connects
        {
            get
            {
                if (connects == null)
                {
                    IEnumerable<XElement> elements =
                      from element in XmlPage.Descendants()
                      where element.Name.LocalName == "Connects"
                      select element;
                    if (!elements.Any())
                    {
                        IEnumerable<XElement> pageContents =
                      from element in XmlPage.Descendants()
                      where element.Name.LocalName == "PageContents"
                      select element;
                        connects = new XElement("Connects");
                        pageContents.FirstOrDefault().Add(connects);
                    }
                    else
                    {
                        connects = elements.FirstOrDefault();
                    }
                }
                return connects;
            }
        }

        public static decimal NoLevels { get; internal set; }
        public static List<string> HiddenList { get; internal set; }
        public static bool HideParent { get; internal set; }

        internal static void CreateVisio(SBList<Table> selectedTables, string fileName)
        {
            if (templatePackage == null)
            {

                File.WriteAllBytes(fileName, Resources.VisioTemplateERD);

                templatePackage = Package.Open(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                templateDocument = GetPackagePart(templatePackage, "http://schemas.microsoft.com/visio/2010/relationships/document");
                templatePages = GetPackagePart(templatePackage, templateDocument, "http://schemas.microsoft.com/visio/2010/relationships/pages");
                templatePage = GetPackagePart(templatePackage, templatePages, "http://schemas.microsoft.com/visio/2010/relationships/page");
            }
            connects = null;

            XmlPage = GetXMLFromPart(templatePage);
            addedTables = new List<VisTable>();
            addedMtoM.Clear();

            Shapes = new List<BaseShape>();
            addedTables.Clear();
            foreach (Table table in selectedTables)
            {
                if (addedTables.Count == 0) AddTable(table, false, pageWidth / 2, pageHeight / 2);
                else AddTable(table, false, nextX, nextY);

            }
            if (OnlyBetweenTable) AddOnlyBetweenRels(selectedTables);
            else
            {
                foreach (var table in selectedTables)
                {
                    if (OneToMany) AddAllOneToMany(addedTables.First(prim => prim.LogicalName == table.Logical), NoLevels);
                    if (ManyToOne) AddAllManyToOne(addedTables.First(prim => prim.LogicalName == table.Logical), NoLevels);
                    if (ManyToMany) AddAllManyToMany(addedTables.First(prim => prim.LogicalName == table.Logical), NoLevels);
                }
            }

            RemoveTemplateShapes();

            CreateNewPage("ERDBuilder", 1);
        }

        private static void AddAllOneToMany(VisTable primTable, decimal noLevels)
        {
            foreach (var childRel in primTable.TableMeta.OneToManyRelationships.Where(em => !HiddenList.Contains(em.ReferencingEntity)))
            {
                if (AddOneToMany(primTable, childRel))

                {
                    if (noLevels > 1)
                    {
                        AddAllOneToMany(addedTables.First(pe => pe.LogicalName == childRel.ReferencingEntity), --noLevels);
                        if (ManyToOne) AddAllManyToOne(addedTables.First(pe => pe.LogicalName == childRel.ReferencingEntity), --noLevels);
                        if (ManyToMany) AddAllManyToMany(addedTables.First(pe => pe.LogicalName == childRel.ReferencingEntity), --noLevels);

                    }
                }
            }
        }

        private static void AddAllManyToOne(VisTable primTable, decimal noLevels)
        {
            foreach (var childRel in primTable.TableMeta.ManyToOneRelationships.Where(em => !HiddenList.Contains(em.ReferencedEntity)))
            {
                if (AddManyToOne(primTable, childRel))
                {

                    if (noLevels > 1)
                    {
                        AddAllManyToOne(addedTables.First(pe => pe.LogicalName == childRel.ReferencingEntity), --noLevels);
                        if (OneToMany) AddAllOneToMany(addedTables.First(pe => pe.LogicalName == childRel.ReferencingEntity), --noLevels);
                        if (ManyToMany) AddAllManyToMany(addedTables.First(pe => pe.LogicalName == childRel.ReferencingEntity), --noLevels);

                    }
                }
            }
        }

        private static void AddAllManyToMany(VisTable primeTable, decimal noLevels)
        {
            foreach (var childRel in primeTable.TableMeta.ManyToManyRelationships.Where(em => !HiddenList.Contains(
                primeTable.LogicalName == em.Entity1LogicalName ? em.Entity2LogicalName : em.Entity1LogicalName)))
            {
                if (AddManyToMany(primeTable, childRel))
                {

                    if (noLevels > 1)
                    {
                        AddAllManyToMany(addedTables.First(pe => pe.LogicalName == childRel.Entity1LogicalName || pe.LogicalName == childRel.Entity2LogicalName), --noLevels);
                        if (OneToMany) AddAllOneToMany(addedTables.First(pe => pe.LogicalName == childRel.Entity1LogicalName || pe.LogicalName == childRel.Entity2LogicalName), --noLevels);
                        if (ManyToOne) AddAllManyToOne(addedTables.First(pe => pe.LogicalName == childRel.Entity1LogicalName || pe.LogicalName == childRel.Entity2LogicalName), --noLevels);

                    }
                }
            }
        }

        private static void AddOnlyBetweenRels(SBList<Table> selectedTables)
        {
            foreach (var table in selectedTables)
            {
                // var primeEntityMeta = Service.GetEntityMetadata(selectedEntity.SubItems[1].Text);
                var primTable = addedTables.First(pe => pe.TableName == table.Logical);
                foreach (var child in primTable.TableMeta.OneToManyRelationships.Where(em => addedTables.Any(a => a.TableName == em.ReferencingEntity)))
                {
                    AddOneToMany(primTable, child);
                }

                foreach (var mtom in primTable.TableMeta.ManyToManyRelationships.Where(rel => rel.Entity1LogicalName == table.Logical
                    && addedTables.Any(a => a.TableName == rel.Entity2LogicalName)))
                {
                    if (!addedMtoM.Contains(mtom.SchemaName))
                    {
                        AddManyToMany(primTable, mtom);
                    }
                }

                foreach (var mtom in primTable.TableMeta.ManyToManyRelationships.Where(rel => rel.Entity2LogicalName == primTable.LogicalName
                    && addedTables.Any(a => a.LogicalName == rel.Entity1LogicalName)))
                {
                    if (!addedMtoM.Contains(mtom.SchemaName))
                    {
                        AddManyToMany(primTable, mtom);
                    }
                }
            }
        }

        private static bool AddManyToMany(VisTable primTable, ManyToManyRelationshipMetadata connected)
        {
            addedMtoM.Add(connected.SchemaName);

            VisTable secondary;
            //EntityMetadata secondaryMeta;
            string secondaryName = connected.Entity1LogicalName == primTable.LogicalName ? connected.Entity2LogicalName : connected.Entity1LogicalName;
            var secondaryMeta = Service.GetEntityMetadata(secondaryName);
            if ((secondaryMeta.IsActivity ?? false) && HideActivity)
            {
                return false;
            }

            if (addedTables.All(ent => ent.LogicalName != secondaryName))
            {
                secondary = AddTable(secondaryMeta, false, nextX, nextY);
            }
            else
            {
                if (secondaryName == primTable.LogicalName)
                {
                    if (HideParent) secondary = null;
                    else
                    {
                        if (addedTables.All(ch => ch.DisplayName != "PARENT: " + primTable.DisplayName))
                        {
                            secondary = AddTable(secondaryMeta, true, nextX, nextY);
                        }
                        else
                        {
                            secondary = addedTables.First(se => se.Name == "PARENT: " + primTable.DisplayName);
                        }
                    }
                }
                else
                {
                    secondary = addedTables.First(se => se.LogicalName == secondaryName);
                }
            }

            if (secondary != null) primTable.ConnectMulti(secondary, connected.Entity1NavigationPropertyName, connected.Entity2NavigationPropertyName);
            //ConnectMulti(primeEntity, secondary, connected);
            return true;
        }
        private static bool AddManyToOne(VisTable primTable, OneToManyRelationshipMetadata parent)
        {
            if (addedTables.All(vt => vt.TableName != parent.ReferencedEntity))
            {
                // New Table

                var childMeta = Service.GetEntityMetadata(parent.ReferencingEntity);
                if ((childMeta.IsActivity ?? false) && HideActivity)
                {
                    return false;
                }
                ((VisTable)Shapes.First(shp => ((VisTable)shp).TableName == primTable.TableName))
                        .Connect(AddTable(childMeta, false, nextX, nextY), parent.ReferencedAttribute, parent.SchemaName);
                //ConnectShape(primeEntity, AddEntity(childMeta, false, nextX, nextY), child.ReferencingAttribute, child.SchemaName);
            }
            else //Already seen this, are we adding as parent?
            {
                var childTable = addedTables.First(ch => ch.TableName == parent.ReferencedEntity);
                //Entity childShape = (Entity)page.Shapes["Shape." + childEntity.ID];
                if (childTable == primTable) 
                { // Parent/Child are same
                    if (!HideParent)
                    {
                        if (addedTables.All(ch => ch.TableName != "PARENT: " + primTable.DisplayName))
                        {
                            childTable = AddTable(primTable.TableMeta, true, nextX, nextY);
                        }
                        else
                        {
                            childTable = addedTables.First(ch => ch.DisplayName == "PARENT: " + primTable.DisplayName);// (Entity)page.Shapes["Shape." + addedEntities.First(ch => ch.Name == "PARENT: " + primeTable.DisplayName).ID];
                        }
                        primTable.Connect(childTable, parent.ReferencedAttribute, parent.SchemaName);
                    }
                }
                else primTable.Connect(childTable, parent.ReferencedAttribute, parent.SchemaName);
                //ConnectShape(primeEntity, childShape, child.ReferencingAttribute, childRel.SchemaName);
            }
            return true;
        }
        private static bool AddOneToMany(VisTable primeTable, OneToManyRelationshipMetadata childRel)
        {
            if (addedTables.All(vt => vt.TableName != childRel.ReferencingEntity))
            {

                var childMeta = Service.GetEntityMetadata(childRel.ReferencingEntity);
                if ((childMeta.IsActivity ?? false) && HideActivity)
                {
                    return false;
                }
                ((VisTable)Shapes.First(shp => ((VisTable)shp).TableName == primeTable.TableName))
                        .Connect(AddTable(childMeta, false, nextX, nextY), childRel.ReferencedAttribute, childRel.SchemaName);
                //ConnectShape(primeEntity, AddEntity(childMeta, false, nextX, nextY), child.ReferencingAttribute, child.SchemaName);
            }
            else
            {
                var childTable = addedTables.First(ch => ch.TableName == childRel.ReferencingEntity);
                //Entity childShape = (Entity)page.Shapes["Shape." + childEntity.ID];
                if (childTable == primeTable)

                //if (childShape == primeEntity)
                {
                    if (!HideParent)
                    {
                        if (addedTables.All(ch => ch.DisplayName != "PARENT: " + primeTable.DisplayName))
                        {
                            childTable = AddTable(primeTable.TableMeta, true, nextX, nextY);
                        }
                        else
                        {
                            childTable = addedTables.First(ch => ch.DisplayName == "PARENT: " + primeTable.DisplayName);// (Entity)page.Shapes["Shape." + addedEntities.First(ch => ch.Name == "PARENT: " + primeTable.DisplayName).ID];
                        }
                        primeTable.Connect(childTable, childRel.ReferencedAttribute, childRel.SchemaName);
                    }
                }
                else primeTable.Connect(childTable, childRel.ReferencedAttribute, childRel.SchemaName);
                //if (!HideParent) primeTable.Connect(childTable, childRel.ReferencedAttribute, childRel.SchemaName);
                //ConnectShape(primeEntity, childShape, child.ReferencingAttribute, childRel.SchemaName);
            }
            return true;
        }
        private static VisTable AddTable(EntityMetadata tableMeta, bool parent, double pinX, double pinY)
        {
            var table = new VisTable(tableMeta, parent, TableDisplayNames, pinX, pinY);
            addedTables.Add(table);
            table.ClearFields();
            return table;
        }
        private static void AddTable(Table table, bool parent, double pinX, double pinY)
        {
            var visTable = AddTable(table.Entity, parent, pinX, pinY);
            if (table.Columns.Any())
            {
                foreach (var column in table.Columns.OrderBy(col => col.DisplayName))
                {
                    if (Utils.ColumnsDisplayName) visTable.AddField(column.DisplayName);
                    else visTable.AddField(column.LogicalName);
                }
            }
            else visTable.ClearFields();

        }

        private static void RemoveTemplateShapes()
        {
            foreach (var shapeName in Utils.VisioTemplates)
                Shapes.First().GetTemplateShape(shapeName).Remove();
        }

        internal static void CompleteVisio(string fileName)
        {
            RemoveTemplate();
            RecalcDocument(templatePackage);

            templatePackage.Close();
            templatePackage = null;
        }


    }
}
