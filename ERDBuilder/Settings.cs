﻿using System;
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
    public class Settings
    {
        public string LastUsedOrganizationWebappUrl { get; set; }

        public List<int> RelationshipMaps { get; set; } = new List<int>();

        public decimal Level { get; set; } = 1;

        public List<string> SelectedEntities { get; set; } = new List<string>();
    }
}