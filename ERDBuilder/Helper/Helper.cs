using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Metadata.Query;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;


namespace LinkeD365.ERDBuilder
{ 
    public static class Helper
    {
        public static IOrganizationService Service { get; private set; }

        internal static void CreateConn(IOrganizationService service)
        {

            Service = service;

        }

        public static SBList<Table> AllTables { get; set; } = new SBList<Table>();


        internal static List<EntityMetadata> GetSolutionEntities(Guid[] guids)
        {
            var qry = new QueryExpression("solutioncomponent")
            {
                ColumnSet = new ColumnSet("objectid"),
                NoLock = true,
                Criteria = new FilterExpression
                {
                    Conditions =
                            {
                                new ConditionExpression("solutionid",ConditionOperator.In,
                                                    guids),
                                new ConditionExpression("componenttype", ConditionOperator.Equal, 1)
                            }
                }
            };

            var results = Service.RetrieveMultiple(qry).Entities;
            var entList = results.Select(r => r.GetAttributeValue<Guid>("objectid")).ToList();

            if (entList.Count > 0)
            {
                var eq = new EntityQueryExpression
                {
                    Criteria = new MetadataFilterExpression(LogicalOperator.Or),
                    Properties = new MetadataPropertiesExpression
                    {
                        AllProperties = true
                    },
                    AttributeQuery = new AttributeQueryExpression
                    {
                        Criteria = new MetadataFilterExpression(LogicalOperator.Or)
                        {
                            Conditions =
                                {
                                    new MetadataConditionExpression("LogicalName", MetadataConditionOperator.Equals, "filterout"),
                                }
                        }
                    },
                    KeyQuery = new EntityKeyQueryExpression
                    {
                        Criteria = new MetadataFilterExpression(LogicalOperator.Or)
                        {
                            Conditions =
                                {
                                    new MetadataConditionExpression("LogicalName", MetadataConditionOperator.Equals, "filterout"),
                                }
                        }
                    },
                    RelationshipQuery = new RelationshipQueryExpression
                    {
                        Criteria = new MetadataFilterExpression(LogicalOperator.Or)
                        {
                            Conditions =
                                {
                                    new MetadataConditionExpression("SchemaName", MetadataConditionOperator.Equals, "filterout"),
                                }
                        }
                    }
                };

                entList.ForEach(id => eq.Criteria.Conditions.Add(
                        new MetadataConditionExpression("MetadataId", MetadataConditionOperator.Equals, id)));
                var allEntQry = new RetrieveMetadataChangesRequest
                {
                    Query = eq,
                    ClientVersionStamp = null
                };

                return ((RetrieveMetadataChangesResponse)Service.Execute(allEntQry)).EntityMetadata.ToList();
            }
            return new List<EntityMetadata>();
        }


    }
}
