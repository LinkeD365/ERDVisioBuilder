﻿using Microsoft.Xrm.Sdk.Metadata;
using System.Collections.Generic;
using VisioAutomation.VDX.Elements;
using VisioAutomation.VDX.Enums;
using VisioAutomation.VDX.Sections;

namespace LinkeD365.ERDBuilder
{
    public class Entity : Shape
    {
        private Char bold = new Char();
        private Char attribute = new Char();
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

        public EntityMetadata EntityMeta { get; set; }

        public string LogicalName
        {
            get
            {
                if (EntityMeta == null)
                {
                    return string.Empty;
                }

                return Parent ? "PARENT: " + EntityMeta.LogicalName : EntityMeta.LogicalName;

            }
        }

        public bool Parent { get; }
        public bool EntityDisplayName { get; }

        public string DisplayName
        {
            get
            {
                if (EntityMeta == null)
                {
                    return string.Empty;
                }

                string displayName = Parent ? "PARENT: " : string.Empty;
                if (EntityDisplayName)
                {
                    return displayName + EntityMeta.DisplayName.UserLocalizedLabel.Label;
                }

                return displayName + EntityMeta.LogicalName;
            }
        }

        public List<string> Texts = new List<string>();
        /// <summary>
        /// Entity constructor
        /// 6-7-2020 added width protection
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="pinx"></param>
        /// <param name="piny"></param>
        /// <param name="fontId"></param>
        public Entity(EntityMetadata entityMeta, bool parent, bool entityDisplayName, double pinx, double piny, int fontId) : base(0, pinx, piny, 1.2, 0.8)
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

            CharFormats = new List<Char>();
            ParaFormats = new List<ParagraphFormat>();

            ParaFormats.Add(Left);
            CharFormats.Add(bold);
            CharFormats.Add(attribute);
            XForm.Height.Formula = "GUARD(TEXTHEIGHT(TheText,Width))";
            XForm.Width.Formula = "GUARD(TEXTWIDTH(TheText))";

            EntityMeta = entityMeta;
            EntityDisplayName = entityDisplayName;
            Parent = parent;
            Texts.Add(DisplayName);
            Text.Add(DisplayName, 0, 0, null);
        }


    }
}