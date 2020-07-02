using Microsoft.Xrm.Sdk.Extensions;
using Microsoft.Xrm.Sdk.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LinkeD365.ERDBuilder
{
    public partial class ERDBuilderControl
    {
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
                    var childEntity = addedEntities.Where(pe => pe.Name == childMeta.LogicalName).First();
                    AddAllOneToMany(childEntity, childMeta, --levelCount);
                }
            }
        }

        private void AddAllManyToOne(Entity primeEntity, EntityMetadata entityMeta, decimal levelCount)
        {
            foreach (var child in entityMeta.ManyToOneRelationships.Where(em => !HiddenSystemList.Contains(em.ReferencedEntity)))

            {
                if (!addedEntities.Where(ent => ent.Name == child.ReferencedEntity).Any())
                {
                    //  AddEntity(new Entity(child.ReferencingEntity, page.Shapes.Count, 0));
                    //entity.Name = child.ReferencingEntity;

                    ConnectShape(AddEntity(child.ReferencedEntity, nextX, nextY), primeEntity, child.ReferencingAttribute);
                }
                else
                {
                    Entity childEntity = addedEntities.Where(ch => ch.Name == child.ReferencedEntity).First();
                    Entity childShape = (Entity)page.Shapes["Shape." + childEntity.ID.ToString()];

                    if (childShape == primeEntity)
                    {
                        if (!addedEntities.Where(ch => ch.Name == "PARENT: " + primeEntity.Name).Any()) childShape = AddEntity("PARENT: " + primeEntity.Name, addedEntities.Count, 0);
                        else childShape = (Entity)page.Shapes["Shape." + addedEntities.Where(ch => ch.Name == "PARENT: " + primeEntity.Name).First().ID.ToString()];
                    }
                    ConnectShape(childShape, primeEntity, child.ReferencingAttribute);
                }

                if (levelCount > 1)
                {
                    var childMeta = Service.GetEntityMetadata(child.ReferencedEntity);
                    var childEntity = addedEntities.Where(pe => pe.Name == childMeta.LogicalName).First();
                    AddAllManyToOne(childEntity, childMeta, --levelCount);
                }
            }
        }

        /// <summary>
        /// Create one-many for all entities listed but that is all
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
            }
        }

        private void AddOneToMany(Entity primeEntity, OneToManyRelationshipMetadata child)
        {
            if (!addedEntities.Where(ent => ent.Name == child.ReferencingEntity).Any())
            {
                //  AddEntity(new Entity(child.ReferencingEntity, page.Shapes.Count, 0));
                //entity.Name = child.ReferencingEntity;

                ConnectShape(primeEntity, AddEntity(child.ReferencingEntity, nextX, nextY), child.ReferencingEntityNavigationPropertyName);
            }
            else
            {
                Entity childEntity = addedEntities.Where(ch => ch.Name == child.ReferencingEntity).First();
                Entity childShape = (Entity)page.Shapes["Shape." + childEntity.ID.ToString()];

                if (childShape == primeEntity)
                {
                    if (!addedEntities.Where(ch => ch.Name == "PARENT: " + primeEntity.Name).Any()) childShape = AddEntity("PARENT: " + primeEntity.Name, addedEntities.Count, 0);
                    else childShape = (Entity)page.Shapes["Shape." + addedEntities.Where(ch => ch.Name == "PARENT: " + primeEntity.Name).First().ID.ToString()];
                }
                ConnectShape(primeEntity, childShape, child.ReferencingEntityNavigationPropertyName);
            }
        }
    }
}