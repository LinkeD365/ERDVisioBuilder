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
        public int FontId;

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

        /// <summary>
        /// Entity constructor
        /// 6-7-2020 added width protection
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="pinx"></param>
        /// <param name="piny"></param>
        /// <param name="fontId"></param>
        public Entity(string entityName, double pinx, double piny, int fontId) : base(0, pinx, piny, 1.2, 0.8)
        {
            FontId = fontId;
            TextBlock = new TextBlock();
            TextBlock.VerticalAlign.Result = 0;

            attribute.Style.Result = CharStyle.None;
            attribute.Font.Result = FontId;
            attribute.Size.Result = 6;
            bold.Style.Result = CharStyle.Bold;
            bold.Font.Result = FontId;
            bold.Size.Result = 8;

            CharFormats = new List<VDX.Sections.Char>();
            ParaFormats = new List<ParagraphFormat>();

            ParaFormats.Add(Left);
            CharFormats.Add(bold);
            CharFormats.Add(attribute);
            XForm.Height.Formula = "GUARD(TEXTHEIGHT(TheText,Width))";
            XForm.Width.Formula = "GUARD(TEXTWIDTH(TheText))";
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
}