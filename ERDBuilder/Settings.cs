using Microsoft.Xrm.Sdk.Metadata;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace LinkeD365.ERDBuilder
{
    /// <summary>
    /// This class can help you to store settings for your plugin
    /// </summary>
    /// <remarks>
    /// This class must be XML serializable
    /// </remarks>
    ///
    /// 
    public class Settings
    {
        private List<string> _selectedEntities = new List<string>();
        private List<Table> tables = new List<Table>();

        public string Name { get; set; }
        public string LastUsedOrganizationWebappUrl { get; set; }

        public List<int> RelationshipMaps { get; set; } = new List<int>();

        public List<int> Display { get; set; } = new List<int>();

        public List<int> Hide { get; set; } = new List<int>();

        public decimal Level { get; set; } = 1;

        public List<string> SelectedEntities
        {
            get { return _selectedEntities; }
            set
            {
                foreach (string entityName in value)
                {
                    Tables.Add(new Table(entityName));
                }
            }
        }
        public List<Table> Tables
        {
            get
            {
                if (!tables.Any())
                {
                    if (SelectedEntities.Count == 0) return tables;
                    if (!Helper.AllTables.Any()) return tables;

                    foreach (string entName in SelectedEntities)
                    {
                        var table = Helper.AllTables.FirstOrDefault(tab => tab.DisplayName == entName);
                        if (table == null) continue;
                        tables.Add(new Table { DisplayName = table.DisplayName, Custom = table.Custom, Logical = table.Logical });
                    }
                }
                return tables;
            }
            set => tables = value;
        }
        public override bool Equals(object obj)
        {
            return obj is Settings setting &&
                   Name == setting.Name;
        }

        public override int GetHashCode()
        {
            return 539060726 + EqualityComparer<string>.Default.GetHashCode(Name);
        }

        public override string ToString()
        {
            return Name;
        }
    }

    public class AllSettings
    {
        public List<Settings> Settings { get; set; } = new List<Settings>();

        public VisioDisplayConfig VisioDisplayConfig { get; set; } = new VisioDisplayConfig();
    }

    public class VisioDisplayConfig
    {
        private int levels;
        [Category("Relationships")]
        [DisplayName("One-To-Many")]
        public bool OneToMany { get; set; }
        [Category("Relationships")]
        [DisplayName("Many-To-One")]
        public bool ManyToOne { get; set; }
        [Category("Relationships")]
        [DisplayName("Many-To-Many")]
        public bool ManyToMany { get; set; }
        [Category("Relationships")]
        [DisplayName("Only Between selected Tables")]
        [DefaultValue(true)]
        public bool OnlySelected { get; set; } = true;

        [Category("Display Names or Logical")]
        [DisplayName("Use Display Names for Tables ")]
        [DefaultValue(true)]
        public bool TableDisplay { get; set; } = true;
        [Category("Display Names or Logical")]
        [DisplayName("Use Display Names for Columns")]
        [DefaultValue(true)]
        public bool ColumnDisplay { get; set; } = true;

        [Category("Tables")]
        [DisplayName("Hide System")]
        [DefaultValue(true)]
        public bool HideSystem { get; set; } = true;
        [Category("Tables")]
        [DisplayName("Hide Activity Tables")]
        [DefaultValue(true)]
        public bool HideActivity { get; set; } = true;
        [Category("Tables")]
        [DisplayName("Hide Parent Tables")]
        [DefaultValue(true)]
        public bool HideParent { get; set; }

        [Category("Relationships")]
        [DefaultValue(1)]
        [DisplayName("Max Level Count")]
        
        public int Levels { get => levels; set { levels = value > 3 ? 3 : value < 1 ? 1 : value; } }
    }

    public class Table : IEquatable<Table>, INotifyPropertyChanged
    {
        private bool selected;

        [DisplayName("   ")]
        public bool Selected
        {
            get => selected;
            set
            {
                if (value != selected) { selected = value; NotifyPropertyChanged(); }
            }
        }
        [DisplayName("Table")]
        [ReadOnly(true)]
        public string DisplayName { get; set; }

        public Table(string displayName)
        {
            DisplayName = displayName;
        }
        public Table() { }
        public Table(EntityMetadata entity)
        {
            Entity = entity;
            DisplayName = Entity.DisplayName?.UserLocalizedLabel?.Label?.ToString() ?? Entity.LogicalName;
            Logical = Entity.LogicalName;
            Custom = Entity.IsCustomEntity ?? false;
        }

        public Table(Table clone)
        {
            Entity = clone.Entity;
            DisplayName = clone.DisplayName;
            Logical = clone.Logical;
            Custom = clone.Custom;
        }

        [DisplayName("Logical Name")]
        [ReadOnly(true)]
        public string Logical { get; set; }

        [ReadOnly(true)]
        public bool Custom { get; set; }

        [XmlIgnore]
        [DisplayName("Columns")]
        public string ColumnList { get { return string.Join(", ", Columns.Where(col => col.Selected).Select(col => col.DisplayName)); } }
        public SBList<Column> Columns { get; set; } = new SBList<Column>();

        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public override bool Equals(object obj)
        {
            return Equals(obj as Table);
        }

        public bool Equals(Table other)
        {
            return other != null &&
                   Logical == other.Logical;
        }

        public override string ToString()
        {
            return Logical;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        [XmlIgnore]
        [Browsable(false)]
        public EntityMetadata Entity { get; set; }
    }

    public class Column : INotifyPropertyChanged
    {
        public Column() { }
        public Column(AttributeMetadata attribute)
        {
            Attribute = attribute;
            DisplayName = Attribute.DisplayName?.UserLocalizedLabel?.Label?.ToString() ?? Attribute.LogicalName;
            LogicalName = Attribute.LogicalName;
            Custom = Attribute.IsCustomAttribute ?? false;
        }

        private bool selected;

        [DisplayName("   ")]
        public bool Selected
        {
            get => selected;
            set
            {
                if (value != selected) { selected = value; NotifyPropertyChanged(); }
            }
        }
        [DisplayName("Column")]
        [ReadOnly(true)]
        public string DisplayName { get; set; }

        [DisplayName("Logical Name")]
        [ReadOnly(true)]
        public string LogicalName { get; set; }
        [ReadOnly(true)]
        public bool Custom { get; set; }
        [Browsable(false)]
        [XmlIgnore]
        public AttributeMetadata Attribute { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    class ColumnComparer : IEqualityComparer<Column>
    {
        public bool Equals(Column x, Column y)
        {
            return x.LogicalName == y.LogicalName;
        }

        public int GetHashCode(Column obj)
        {
            return obj.LogicalName.GetHashCode();
        }
    }

    class TableComparer : IEqualityComparer<Table>
    {
        public bool Equals(Table x, Table y)
        {
            return x.Logical == y.Logical;
        }

        public int GetHashCode(Table obj)
        {
            return obj.Logical.GetHashCode();
        }
    }
    class SortTableComparer : IComparer<Table>
    {
        SortOrder sortOrder = SortOrder.None;
        string colName;

        public SortTableComparer(SortOrder sortingOrder, string column) { sortOrder = sortingOrder; colName = column; }

        public int Compare(Table x, Table y)
        {
            int returnValue = 1;

            if (sortOrder == SortOrder.Ascending)
            {
                if (colName == "DisplayName") returnValue = x.DisplayName.CompareTo(y.DisplayName);
                else returnValue = x.Logical.CompareTo(y.Logical);
            }
            else
            {
                if (colName == "DisplayName") returnValue = y.DisplayName.CompareTo(x.DisplayName);
                else returnValue = y.Logical.CompareTo(x.Logical);
            }

            return returnValue;
        }
    }
}