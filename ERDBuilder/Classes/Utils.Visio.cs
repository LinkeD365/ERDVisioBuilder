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
            //Helper.Containers.Clear();
            AddContainers(selectedTables);
            foreach (Table table in selectedTables)
            {
                AddToContainer(AddTable(table, false, addedTables.Any() ? nextX : pageWidth / 2, addedTables.Any() ? nextY : pageHeight / 2));
            }

            //  AddContainer(addedTables[0]);
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

        private static void AddContainers(SBList<Table> selectedTables)
        {
            int countContainer = Helper.Containers.Where(cnt => selectedTables.Any(tbl => tbl.ContainerName == cnt.Title)).Count();
            foreach (Container container in Helper.Containers.Where(cnt => selectedTables.Any(tbl => tbl.ContainerName == cnt.Title)))
            {
                Shapes.Add(new VisContainer(container.Title, container.Colour, 2 * pageWidth * Shapes.OfType<VisContainer>().Count() / 3, pageHeight));
            }
        }

        private static void AddToContainer(VisTable visTable)
        {
            if (visTable.Table.ContainerName != string.Empty)
            {
                var container = Shapes.OfType<VisContainer>().FirstOrDefault(cnt => cnt.ContainerName == visTable.Table.ContainerName);
                if (container == null) return;
                container.AddShape(visTable);
            }
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
        private static VisTable AddTable(Table table, bool parent, double pinX, double pinY)
        {

            var visTable = AddTable(table.Entity, parent, pinX, pinY);
            visTable.Table = table;
            if (ShowPK) AddPK(visTable);
            if (table.Columns.Any())
            {
                foreach (var column in table.Columns.OrderBy(col => col.DisplayName))
                {
                    if (ColumnsDisplayName) visTable.AddField(column.DisplayName);
                    else visTable.AddField(column.LogicalName);
                }
            }
            //else visTable.ClearFields();
            return visTable;
        }

        private static void AddPK(VisTable visTable)
        {
            AttributeMetadata primaryMeta = visTable.TableMeta.Attributes.ToList().First(att => att.LogicalName == visTable.TableMeta.PrimaryIdAttribute);

            visTable.AddField("PK " + (Utils.ColumnsDisplayName ? primaryMeta?.DisplayName?.UserLocalizedLabel?.Label
                ?? primaryMeta.LogicalName : primaryMeta.LogicalName));
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
