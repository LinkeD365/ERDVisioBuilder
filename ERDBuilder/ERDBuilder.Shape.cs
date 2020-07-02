using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Documents;
using VisioAutomation.VDX.Elements;
using VisioAutomation.VDX.Enums;
using VisioAutomation.VDX.Sections;
using XrmToolBox.Extensibility;
using VDX = VisioAutomation.VDX;

namespace LinkeD365.ERDBuilder
{
    public class Entity : VDX.Elements.Shape
    {
        private VDX.Sections.Char bold = new VDX.Sections.Char();
        private VDX.Sections.Char attribute = new VDX.Sections.Char();

        private ParagraphFormat _left;

        private ParagraphFormat Left
        {
            get
            {
                if (_left == null)
                {
                    _left = new ParagraphFormat();
                    _left.HorzAlign.Result = ParaHorizontalAlignment.Left;
                }
                return _left;
            }
        }

        //private TextBlock _textBlock;

        public Entity(string entityName, double pinx, double piny, int fontId) : base(0, pinx, piny, 1.2, 0.8)
        {
            TextBlock = new TextBlock();
            TextBlock.VerticalAlign.Result = 0;

            attribute.Style.Result = CharStyle.None;
            attribute.Font.Result = fontId;
            attribute.Size.Result = 6;
            bold.Style.Result = CharStyle.Bold;
            bold.Font.Result = fontId;
            bold.Size.Result = 8;

            CharFormats = new List<VDX.Sections.Char>();
            ParaFormats = new List<ParagraphFormat>();

            ParaFormats.Add(Left);
            CharFormats.Add(bold);
            CharFormats.Add(attribute);
            XForm.Height.Formula = "GUARD(TEXTHEIGHT(TheText,Width))";
            Text.Add(entityName, 0, 0, null);
        }

        //public new TextBlock TextBlock
        //{
        //    get
        //    {
        //        if (_textBlock == null) _textBlock = new TextBlock();

        //        return _textBlock;
        //    }
        //    set
        //    {
        //        _textBlock = value;
        //    }
        //}
    }

    public partial class ERDBuilderControl : PluginControlBase
    {
        public void ConnectShape(Entity firstEntity, Entity secondEntity, string fieldName)
        {
            var connector = Shape.CreateDynamicConnector(doc);
            connector.XForm1D.EndY.Result = 0;
            connector.Line = new Line();
            connector.Line.EndArrow.Result = 29;
            connector.Line.BeginArrow.Result = 30;
            page.Shapes.Add(connector);
            connector.Geom = new Geom();
            connector.Geom.Rows.Add(new MoveTo(1, 3));
            connector.Geom.Rows.Add(new LineTo(5, 3));
            secondEntity.Text.Add("\n" + fieldName, 1, 0, null);
            page.ConnectShapesViaConnector(connector, firstEntity, secondEntity);
        }

        private void AddEntity(Entity entity)
        {
            string entityName = entity.Name;
            page.Shapes.Add(entity);
            entity.Name = entityName;
            addedEntities.Add(entity);
        }

        private Entity AddEntity(string entityName, double pinx, double piny)
        {
            var entity = new Entity(entityName, pinx, piny, face.ID);
            page.Shapes.Add(entity);
            entity.Name = entityName;
            addedEntities.Add(entity);
            return entity;
        }

        private List<Entity> addedEntities = new List<Entity>();

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
    }
}