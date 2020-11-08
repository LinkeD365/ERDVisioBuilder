using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public string Name { get; set; }
        public string LastUsedOrganizationWebappUrl { get; set; }

        public List<int> RelationshipMaps { get; set; } = new List<int>();

        public List<int> Display { get; set; } = new List<int>();

        public List<int> Hide { get; set; } = new List<int>();

        public decimal Level { get; set; } = 1;

        public List<string> SelectedEntities { get; set; } = new List<string>();

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
    }
}