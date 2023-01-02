using LinkeD365.ERDBuilder;
using Microsoft.Xrm.Sdk;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LinkeD365.ERDBuilder
{

    public static partial class Utils
    {
        private const string aiEndpoint = "https://dc.services.visualstudio.com/v2/track";
        private static double pageWidth = 11;
        private static double pageHeight = 8;
        private const string aiKey = "cc383234-dfdb-429a-a970-d17847361df3";
        private static AppInsights ai;

        public static AppInsights Ai
        {
            get
            {
                if (ai == null)
                {
                    ai = new AppInsights(aiEndpoint, aiKey, Assembly.GetExecutingAssembly());
                }
                return ai;
            }
        }
        public static IOrganizationService Service { get; internal set; }
        public static bool TableDisplayNames { get; internal set; }
        public static bool ColumnsDisplayName { get; internal set; }
        public static bool OnlyBetweenTable { get; internal set; }
        public static bool OneToMany { get; internal set; }
        public static bool ManyToOne { get; internal set; }
        public static bool ManyToMany { get; internal set; }

        public static bool HideActivity { get; internal set; }

        public static decimal NoLevels { get; internal set; }
        public static List<string> HiddenList { get; internal set; }
        public static bool HideParent { get; internal set; }
        public static bool ShowFK { get; internal set; }
        public static bool ShowPK { get; internal set; }

        private static double xMultiplier = 1.8;
        private static double yMultiplier = 1.2;
        public static List<string> VisioTemplates = new List<string>() { "Table", "Connector" };
        private static double nextX
        {
            get
            {
                switch (addedTables.Count)
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

        private static double nextY
        {
            get
            {
                switch (addedTables.Count)
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
        public static int ActionCount { get; set; }

        private static Package templatePackage;

        private static PackagePart templateDocument;

        private static PackagePart templatePages;
        private static PackagePart templatePage;

        public static XDocument XmlPage;
        private static List<VisTable> addedTables;
        private static JObject wfObject;
        public static List<BaseShape> Shapes;

        //public static List<StageNode> StageNodes;
        private static XElement connects;
        private static List<string> addedMtoM = new List<string>();
    }
}
