using Microsoft.Xrm.Sdk.Extensions;
using Microsoft.Xrm.Sdk.Metadata;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using VisioAutomation.VDX.Elements;
using VisioAutomation.VDX.Sections;

namespace LinkeD365.ERDBuilder
{
    public partial class ERDBuilderControl
    {
        private List<Entity> addedEntities = new List<Entity>();
        private List<string> addedMtoM = new List<string>();

        private List<string> _hiddenSystem;

        protected List<string> HiddenSystemList
        {
            get
            {
                if (chkListHide.CheckedItems.Contains("Hide System"))
                {
                    if (_hiddenSystem == null)
                    {
                        _hiddenSystem = new List<string>
                        {
                            "principalobjectattributeaccess",
                            "postfollow",
                            "postregarding",
                            "postrole",
                            "syncerror",
                            "bulkdeletefailure",
                            "processsession",
                            "asyncoperation",
                            "userentityinstancedata",
                            "team",
                            "systemuser",
                            "owner",
                            "businessunit",
                            "mailboxtrackingfolder",
                            "duplicaterecord"
                        };
                    }
                    return _hiddenSystem;
                }
                else
                {
                    return new List<string>();
                }
            }
        }

        private double nextX
        {
            get
            {
                switch (addedEntities.Count)
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

        private double nextY
        {
            get
            {
                switch (addedEntities.Count)
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

        /// <summary>
        /// Add relationships for all the child entities
        /// </summary>
        /// <param name="primeEntity">parent entity</param>
        /// <param name="primeEntityMeta">parent meta data</param>
        private void AddAllOneToMany(Entity primeEntity, EntityMetadata primeEntityMeta, decimal levelCount)
        {
            foreach (var child in primeEntityMeta.OneToManyRelationships.Where(em => !HiddenSystemList.Contains(em.ReferencingEntity)))

            {

                if (!AddOneToMany(primeEntity, child))
                {
                    return;
                }

                if (levelCount > 1)
                {
                    var childMeta = Service.GetEntityMetadata(child.ReferencingEntity);
                    var childEntity = addedEntities.First(pe => pe.LogicalName == childMeta.LogicalName);
                    AddAllOneToMany(childEntity, childMeta, --levelCount);
                    if (checkRelationships.CheckedItems.Contains("Many-To-One"))
                    {
                        AddAllManyToOne(childEntity, childMeta, --levelCount);
                    }

                    if (checkRelationships.CheckedItems.Contains("One-To-Many"))
                    {
                        AddAllManyToMany(childEntity, childMeta, --levelCount);
                    }
                }
            }
        }

        /// <summary>
        /// Add all manytoone relationships and entities
        /// 7-7-20 - add call to get child meta info to allow primary key attributes
        /// </summary>
        /// <param name="primeEntity"></param>
        /// <param name="entityMeta"></param>
        /// <param name="levelCount"></param>
        private void AddAllManyToOne(Entity primeEntity, EntityMetadata entityMeta, decimal levelCount)
        {
            foreach (var parent in entityMeta.ManyToOneRelationships.Where(em => !HiddenSystemList.Contains(em.ReferencedEntity)))

            {
                AddManyToOne(primeEntity, parent);

                if (levelCount > 1)
                {
                    var childMeta = Service.GetEntityMetadata(parent.ReferencedEntity);
                    var childEntity = addedEntities.First(pe => pe.LogicalName == childMeta.LogicalName);
                    AddAllManyToOne(childEntity, childMeta, --levelCount);
                    if (checkRelationships.CheckedItems.Contains("One-To-Many"))
                    {
                        AddAllOneToMany(childEntity, childMeta, --levelCount);
                    }

                    if (checkRelationships.CheckedItems.Contains("One-To-Many"))
                    {
                        AddAllManyToMany(childEntity, childMeta, --levelCount);
                    }
                }
            }
        }

        private void AddAllManyToMany(Entity primeEntity, EntityMetadata entityMeta, decimal levelCount)
        {
            foreach (var connected in entityMeta.ManyToManyRelationships.Where(em => !HiddenSystemList.Contains(
                primeEntity.Name == em.Entity1LogicalName ? em.Entity2LogicalName : em.Entity1LogicalName)))
            {
                if (!addedMtoM.Contains(connected.SchemaName))
                {
                    if (!AddManyToMany(primeEntity, connected))
                    {
                        return;
                    }
                }

                if (levelCount > 1)
                {
                    var conMeta = Service.GetEntityMetadata(primeEntity.LogicalName == connected.Entity1LogicalName ? connected.Entity2LogicalName : connected.Entity1LogicalName);
                    var conEntity = addedEntities.First(ce => ce.LogicalName == conMeta.LogicalName);
                    AddAllManyToMany(conEntity, conMeta, --levelCount);
                    if (checkRelationships.CheckedItems.Contains("One-To-Many"))
                    {
                        AddAllOneToMany(conEntity, conMeta, --levelCount);
                    }

                    if (checkRelationships.CheckedItems.Contains("Many-To-One"))
                    {
                        AddAllManyToOne(conEntity, conMeta, --levelCount);
                    }
                }
            }
        }

        private bool AddManyToMany(Entity primeEntity, ManyToManyRelationshipMetadata connected)
        {
            addedMtoM.Add(connected.SchemaName);

            Entity secondary;
            //EntityMetadata secondaryMeta;
            string secondaryName = connected.Entity1LogicalName == primeEntity.LogicalName ? connected.Entity2LogicalName : connected.Entity1LogicalName;
            var secondaryMeta = Service.GetEntityMetadata(secondaryName);
            if ((secondaryMeta.IsActivity ?? false) && HideActivity)
            {
                return false;
            }

            if (addedEntities.All(ent => ent.LogicalName != secondaryName))
            {
                secondary = AddEntity(secondaryMeta, false, nextX, nextY);
            }
            else
            {
                if (secondaryName == primeEntity.LogicalName)
                {
                    if (addedEntities.All(ch => ch.DisplayName != "PARENT: " + primeEntity.DisplayName))
                    {
                        secondary = AddEntity(primeEntity.EntityMeta, true, addedEntities.Count, 0);
                    }
                    else
                    {
                        secondary = (Entity)page.Shapes["Shape." + addedEntities.First(ch => ch.Name == "PARENT: " + primeEntity.DisplayName).ID];
                    }
                }
                else
                {
                    secondary = addedEntities.First(se => se.LogicalName == secondaryName);
                }
            }

            ConnectMulti(primeEntity, secondary, connected);
            return true;
        }

        /// <summary>
        /// Create one-many for all entities listed but that is all
        /// 12-7-20 Added Many to Many
        /// </summary>
        private void AddOnlySelectedRelationships(SBList<Table> tables)
        {
            foreach (var table in tables)
            {
                // var primeEntityMeta = Service.GetEntityMetadata(selectedEntity.SubItems[1].Text);
                var primeEntity = addedEntities.First(pe => pe.LogicalName == table.Logical);
                foreach (var child in primeEntity.EntityMeta.OneToManyRelationships.Where(em => addedEntities.Any(a => a.LogicalName == em.ReferencingEntity)))
                {
                    AddOneToMany(primeEntity, child);
                }

                foreach (var mtom in primeEntity.EntityMeta.ManyToManyRelationships.Where(rel => rel.Entity1LogicalName == primeEntity.Name && addedEntities.Any(a => a.Name == rel.Entity2LogicalName)))
                {
                    if (!addedMtoM.Contains(mtom.SchemaName))
                    {
                        AddManyToMany(primeEntity, mtom);
                    }
                }

                foreach (var mtom in primeEntity.EntityMeta.ManyToManyRelationships.Where(rel => rel.Entity2LogicalName == primeEntity.Name && addedEntities.Any(a => a.Name == rel.Entity1LogicalName)))
                {
                    if (!addedMtoM.Contains(mtom.SchemaName))
                    {
                        AddManyToMany(primeEntity, mtom);
                    }
                }
            }
        }

        private bool HideActivity => chkListHide.CheckedItems.Contains("Hide Activity Entities");
        /// <summary>
        /// Add a one to many relationship
        /// 7-7-2020 added entity meta call to populate Primary key
        /// </summary>
        /// <param name="primeEntity"></param>
        /// <param name="child"></param>
        private bool AddOneToMany(Entity primeEntity, OneToManyRelationshipMetadata child)
        {
            if (addedEntities.All(ent => ent.LogicalName != child.ReferencingEntity))
            {
                var childMeta = Service.GetEntityMetadata(child.ReferencingEntity);
                if ((childMeta.IsActivity ?? false) && HideActivity)
                {
                    return false;
                }

                ConnectShape(primeEntity, AddEntity(childMeta, false, nextX, nextY), child.ReferencingAttribute, child.SchemaName);
            }
            else
            {
                Entity childEntity = addedEntities.First(ch => ch.LogicalName == child.ReferencingEntity);
                Entity childShape = (Entity)page.Shapes["Shape." + childEntity.ID];

                if (childShape == primeEntity)
                {
                    if (addedEntities.All(ch => ch.DisplayName != "PARENT: " + primeEntity.DisplayName))
                    {
                        childShape = AddEntity(primeEntity.EntityMeta, true, addedEntities.Count, 0);
                    }
                    else
                    {
                        childShape = (Entity)page.Shapes["Shape." + addedEntities.First(ch => ch.Name == "PARENT: " + primeEntity.DisplayName).ID];
                    }
                }
                ConnectShape(primeEntity, childShape, child.ReferencingAttribute, child.SchemaName);
            }
            return true;
        }

        private void AddManyToOne(Entity primeEntity, OneToManyRelationshipMetadata parent)
        {
            if (addedEntities.All(ent => ent.LogicalName != parent.ReferencedEntity))
            {
                var childMeta = Service.GetEntityMetadata(parent.ReferencedEntity);
                if ((childMeta.IsActivity ?? false) && HideActivity)
                {
                    return;
                }

                ConnectShape(AddEntity(childMeta, false, nextX, nextY), primeEntity, parent.ReferencingAttribute, parent.SchemaName);
            }
            else
            {
                Entity childEntity = addedEntities.First(ch => ch.LogicalName == parent.ReferencedEntity);
                Entity childShape = (Entity)page.Shapes["Shape." + childEntity.ID];

                if (childShape == primeEntity)
                {
                    if (!addedEntities.Any(ch => ch.DisplayName == "PARENT: " + primeEntity.DisplayName))
                    {
                        childShape = AddEntity(primeEntity.EntityMeta, true, addedEntities.Count, 0);
                    }
                    else
                    {
                        childShape = (Entity)page.Shapes["Shape." + addedEntities.First(ch => ch.DisplayName == "PARENT: " + primeEntity.DisplayName).ID];
                    }
                }
                ConnectShape(childShape, primeEntity, parent.ReferencingAttribute, parent.SchemaName);
            }
        }

        private void ConnectShape(Entity firstEntity, Entity secondEntity, string fieldName, string relName)
        {
            var connector = Shape.CreateDynamicConnector(doc);
            connector.XForm1D.EndY.Result = 0;
            connector.Line = new Line();
            connector.Line.EndArrow.Result = 29;
            connector.Line.BeginArrow.Result = 30;
            connector.CustomProps = new CustomProps();
            connector.CustomProps.Add(new CustomProp("Parent") { Value = firstEntity.Name });
            connector.CustomProps.Add(new CustomProp("Child") { Value = secondEntity.Name });
            connector.CustomProps.Add(new CustomProp("Field") { Value = fieldName });
            connector.CustomProps.Add(new CustomProp("Relationship") { Value = relName });
            page.Shapes.Add(connector);
            connector.Geom = new Geom();
            connector.Geom.Rows.Add(new MoveTo(1, 3));
            connector.Geom.Rows.Add(new LineTo(5, 3));
            string fk = GetFK(secondEntity.EntityMeta, fieldName);
            if (!secondEntity.Texts.Contains(fk))
            {
                secondEntity.Texts.Add(fk);
                secondEntity.Text.Add(fk, 1, 0, null);
                // secondEntity.Text.Add(GetFK(secondEntity.EntityMeta, fieldName), 1, 0, null);
            }

            page.ConnectShapesViaConnector(connector, firstEntity, secondEntity);
        }

        private void ConnectMulti(Entity firstEntity, Entity secondEntity, ManyToManyRelationshipMetadata connected)
        {
            var connector = Shape.CreateDynamicConnector(doc);
            connector.XForm1D.EndY.Result = 0;
            connector.Line = new Line();
            connector.Line.EndArrow.Result = 29;
            connector.Line.BeginArrow.Result = 29;
            connector.CustomProps = new CustomProps();
            connector.CustomProps.Add(new CustomProp("Connected1Entity") { Value = connected.Entity1LogicalName });
            connector.CustomProps.Add(new CustomProp("Connected2Entity") { Value = connected.Entity2LogicalName });
            connector.CustomProps.Add(new CustomProp("Entity1Navigation") { Value = connected.Entity1NavigationPropertyName });
            connector.CustomProps.Add(new CustomProp("Entity2Navigation") { Value = connected.Entity2NavigationPropertyName });
            page.Shapes.Add(connector);
            connector.Geom = new Geom();
            connector.Geom.Rows.Add(new MoveTo(1, 3));
            connector.Geom.Rows.Add(new LineTo(5, 3));

            page.ConnectShapesViaConnector(connector, firstEntity, secondEntity);
        }

        private void AddEntity(Entity entity)
        {
            string entityName = entity.Name;
            page.Shapes.Add(entity);
            entity.Name = entityName;
            addedEntities.Add(entity);
        }

        private Entity AddEntity(EntityMetadata entityMeta, bool parent, double pinx, double piny)
        {
            var entity = new Entity(entityMeta, parent, chkListDisplay.CheckedItems.Contains("Entity Display Names"), pinx, piny, face.ID);
            page.Shapes.Add(entity);
            entity.Name = entity.DisplayName;


            entity.Text.Add(GetPrimaryKey(entityMeta), 1, 0, null);
            addedEntities.Add(entity);
            return entity;
        }

        private void AddEntity(Table table, bool parent, double pinx, double piny)
        {
            var entity = AddEntity(table.Entity, parent, pinx, piny);
            foreach (var column in table.Columns.OrderBy(col => col.DisplayName))
            {
                if (chkListDisplay.CheckedItems.Contains("Attribute Display Names")) entity.Text.Add($"\n{column.DisplayName}", 1, 0, null);
                else entity.Text.Add($"\n{column.LogicalName}", 1, 0, null);
            }

        }

        private string GetPrimaryKey(EntityMetadata entityMeta)
        {
            AttributeMetadata primaryMeta = entityMeta.Attributes.ToList().First(att => att.LogicalName == entityMeta.PrimaryIdAttribute);
            if (chkListDisplay.CheckedItems.Contains("Attribute Display Names"))
            {
                return "\nPK " + primaryMeta?.DisplayName?.UserLocalizedLabel?.Label ?? primaryMeta.LogicalName;
            }

            return "\nPK " + primaryMeta.LogicalName;
        }

        private string GetFK(EntityMetadata entityMeta, string fieldName)
        {
            AttributeMetadata fkMeta = entityMeta.Attributes.ToList().FirstOrDefault(att => att.LogicalName == fieldName);
            if (fkMeta == null)
            {
                return string.Empty;
            }

            if (chkListDisplay.CheckedItems.Contains("Attribute Display Names"))
            {
                return "\nFK " + fkMeta?.DisplayName?.UserLocalizedLabel?.Label ?? fkMeta.LogicalName;
            }

            return "\nFK " + fkMeta.LogicalName;
        }
    }
}