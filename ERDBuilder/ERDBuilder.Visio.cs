using Microsoft.Xrm.Sdk.Extensions;
using Microsoft.Xrm.Sdk.Metadata;
using System;
using System.Activities.Expressions;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                if (btnHideSystem.Checked)
                {
                    if (_hiddenSystem == null)
                    {
                        _hiddenSystem = new List<string>();
                        _hiddenSystem.Add("principalobjectattributeaccess");
                        _hiddenSystem.Add("postfollow");
                        _hiddenSystem.Add("postregarding");
                        _hiddenSystem.Add("postrole");
                        _hiddenSystem.Add("syncerror");
                        _hiddenSystem.Add("bulkdeletefailure");
                        _hiddenSystem.Add("processsession");
                        _hiddenSystem.Add("asyncoperation");
                        _hiddenSystem.Add("userentityinstancedata");
                        _hiddenSystem.Add("team");
                        _hiddenSystem.Add("systemuser");
                        _hiddenSystem.Add("owner");
                        _hiddenSystem.Add("businessunit");
                        _hiddenSystem.Add("mailboxtrackingfolder");
                        _hiddenSystem.Add("duplicaterecord");
                    }
                    return _hiddenSystem;
                }
                else return new List<string>();
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
                AddOneToMany(primeEntity, child);
                if (levelCount > 1)
                {
                    var childMeta = Service.GetEntityMetadata(child.ReferencingEntity);
                    var childEntity = addedEntities.First(pe => pe.Name == childMeta.LogicalName);
                    AddAllOneToMany(childEntity, childMeta, --levelCount);
                    if (checkRelationships.CheckedItems.Contains("Many-To-One")) AddAllManyToOne(childEntity, childMeta, --levelCount);
                    if (checkRelationships.CheckedItems.Contains("One-To-Many")) AddAllManyToMany(childEntity, childMeta, --levelCount);
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
                    var childEntity = addedEntities.First(pe => pe.Name == childMeta.LogicalName);
                    AddAllManyToOne(childEntity, childMeta, --levelCount);
                    if (checkRelationships.CheckedItems.Contains("One-To-Many")) AddAllOneToMany(childEntity, childMeta, --levelCount);
                    if (checkRelationships.CheckedItems.Contains("One-To-Many")) AddAllManyToMany(childEntity, childMeta, --levelCount);
                }
            }
        }

        private void AddAllManyToMany(Entity primeEntity, EntityMetadata entityMeta, decimal levelCount)
        {
            foreach (var connected in entityMeta.ManyToManyRelationships.Where(em => !HiddenSystemList.Contains(
                primeEntity.Name == em.Entity1LogicalName ? em.Entity2LogicalName : em.Entity1LogicalName)))
            {
                if (!addedMtoM.Contains(connected.SchemaName)) AddManyToMany(primeEntity, connected);

                if (levelCount > 1)
                {
                    var conMeta = Service.GetEntityMetadata(primeEntity.Name == connected.Entity1LogicalName ? connected.Entity2LogicalName : connected.Entity1LogicalName);
                    var conEntity = addedEntities.First(ce => ce.Name == conMeta.LogicalName);
                    AddAllManyToMany(conEntity, conMeta, --levelCount);
                    if (checkRelationships.CheckedItems.Contains("One-To-Many")) AddAllOneToMany(conEntity, conMeta, --levelCount);
                    if (checkRelationships.CheckedItems.Contains("Many-To-One")) AddAllManyToOne(conEntity, conMeta, --levelCount);
                }
            }
        }

        private void AddManyToMany(Entity primeEntity, ManyToManyRelationshipMetadata connected)
        {
            addedMtoM.Add(connected.SchemaName);

            Entity secondary;
            //EntityMetadata secondaryMeta;
            string secondaryName = connected.Entity1LogicalName == primeEntity.Name ? connected.Entity2LogicalName : connected.Entity1LogicalName;
            if (!addedEntities.Any(ent => ent.Name == secondaryName))
            {
                var secondaryMeta = Service.GetEntityMetadata(secondaryName);
                secondary = AddEntity(secondaryName, secondaryMeta.PrimaryIdAttribute, nextX, nextY);
            }
            else secondary = addedEntities.First(se => se.Name == secondaryName);

            ConnectMulti(primeEntity, secondary, connected);
        }

        /// <summary>
        /// Create one-many for all entities listed but that is all
        /// 12-7-20 Added Many to Many
        /// </summary>
        private void AddOnlySelectedRelationships()
        {
            foreach (ListViewItem selectedEntity in listSelected.Items)
            {
                var primeEntityMeta = Service.GetEntityMetadata(selectedEntity.SubItems[1].Text);
                var primeEntity = addedEntities.Where(pe => pe.Name == primeEntityMeta.LogicalName).First();
                foreach (var child in primeEntityMeta.OneToManyRelationships.Where(em => addedEntities.Any(a => a.Name == em.ReferencingEntity)))
                {
                    AddOneToMany(primeEntity, child);
                }

                foreach (var mtom in primeEntityMeta.ManyToManyRelationships.Where(rel => rel.Entity1LogicalName == primeEntity.Name && addedEntities.Any(a => a.Name == rel.Entity2LogicalName)))
                {
                    if (!addedMtoM.Contains(mtom.SchemaName)) AddManyToMany(primeEntity, mtom);
                }

                foreach (var mtom in primeEntityMeta.ManyToManyRelationships.Where(rel => rel.Entity2LogicalName == primeEntity.Name && addedEntities.Any(a => a.Name == rel.Entity1LogicalName)))
                {
                    if (!addedMtoM.Contains(mtom.SchemaName)) AddManyToMany(primeEntity, mtom);
                }
            }
        }

        /// <summary>
        /// Add a one to many relationship
        /// 7-7-2020 added entity meta call to populate Primary key
        /// </summary>
        /// <param name="primeEntity"></param>
        /// <param name="child"></param>
        private void AddOneToMany(Entity primeEntity, OneToManyRelationshipMetadata child)
        {
            if (!addedEntities.Where(ent => ent.Name == child.ReferencingEntity).Any())
            {
                var childMeta = Service.GetEntityMetadata(child.ReferencingEntity);

                ConnectShape(primeEntity, AddEntity(child.ReferencingEntity, childMeta.PrimaryIdAttribute, nextX, nextY), child.ReferencingEntityNavigationPropertyName, child.SchemaName);
            }
            else
            {
                Entity childEntity = addedEntities.Where(ch => ch.Name == child.ReferencingEntity).First();
                Entity childShape = (Entity)page.Shapes["Shape." + childEntity.ID.ToString()];

                if (childShape == primeEntity)
                {
                    if (!addedEntities.Where(ch => ch.Name == "PARENT: " + primeEntity.Name).Any()) childShape = AddEntity("PARENT: " + primeEntity.Name, string.Empty, addedEntities.Count, 0);
                    else childShape = (Entity)page.Shapes["Shape." + addedEntities.Where(ch => ch.Name == "PARENT: " + primeEntity.Name).First().ID.ToString()];
                }
                ConnectShape(primeEntity, childShape, child.ReferencingEntityNavigationPropertyName, child.SchemaName);
            }
        }

        private void AddManyToOne(Entity primeEntity, OneToManyRelationshipMetadata parent)
        {
            if (!addedEntities.Where(ent => ent.Name == parent.ReferencedEntity).Any())
            {
                var childMeta = Service.GetEntityMetadata(parent.ReferencedEntity);

                ConnectShape(AddEntity(parent.ReferencedEntity, childMeta.PrimaryIdAttribute, nextX, nextY), primeEntity, parent.ReferencingAttribute, parent.SchemaName);
            }
            else
            {
                Entity childEntity = addedEntities.Where(ch => ch.Name == parent.ReferencedEntity).First();
                Entity childShape = (Entity)page.Shapes["Shape." + childEntity.ID.ToString()];

                if (childShape == primeEntity)
                {
                    if (!addedEntities.Where(ch => ch.Name == "PARENT: " + primeEntity.Name).Any()) childShape = AddEntity("PARENT: " + primeEntity.Name, string.Empty, addedEntities.Count, 0);
                    else childShape = (Entity)page.Shapes["Shape." + addedEntities.Where(ch => ch.Name == "PARENT: " + primeEntity.Name).First().ID.ToString()];
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

            secondEntity.Text.Add("\nFK " + fieldName, 1, 0, null);
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

        private Entity AddEntity(string entityName, string primaryKey, double pinx, double piny)
        {
            var entity = new Entity(entityName, pinx, piny, face.ID);
            page.Shapes.Add(entity);
            entity.Name = entityName;
            entity.Text.Add(string.IsNullOrEmpty(primaryKey) ? string.Empty : "\nPK " + primaryKey, 1, 0, null);
            addedEntities.Add(entity);
            return entity;
        }
    }
}