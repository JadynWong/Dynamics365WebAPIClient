using System;
using System.Collections.Generic;
using D365WebApiClient.Standard.Services.WebApiServices;
using D365WebApiClient.Standard.WebApiQueryOptions;
using D365WebApiClient.Standard.WebApiQueryOptions.Options;
using D365WebApiClient.Standard.WebApiQueryOptions.Options.Filter;

namespace Demo.Standard
{
    public class Test
    {
        private readonly IApiClientService _crmApiService;

        public Test(IApiClientService crmApiService)
        {
            this._crmApiService = crmApiService;
        }

        public void TestService()
        {
            var queryParams = new QueryParams()
            {
                ParamDictionary = {
                    {"@p1", "territorycode"},
                    {"@p2", "creditonhold"}
                }
            };
            queryParams.Add(new QueryParam("@p3", "createdon"));
            queryParams.Add("@p4", "statecode");
            var queryOptions = new QueryOptions()
            {
                new QuerySelect("name", "modifiedon", "createdon", "versionnumber"),
                new QueryCount(true),
                new QueryFilter(new FilterExpression()
                {
                    FilterOperator = LogicalOperator.LogicalAnd,
                    Conditions =
                    {
                        new ConditionExpression("@p1", ConditionOperator.Equal, 1),
                        new ConditionExpression("@p2", ConditionOperator.Equal, false),
                        new ConditionExpression("@p3", ConditionOperator.LessThanOrQqual, DateTime.Now)
                    },
                    Filters =
                    {
                        new FilterExpression(LogicalOperator.LogicalOr)
                        {
                            Conditions =
                            {
                                new ConditionExpression("@p4", ConditionOperator.Equal, 0),
                                new ConditionExpression("@p5", ConditionOperator.NotEqual,
                                    Guid.Parse("381512cc-f1cb-e611-80fb-00155d392b01"))
                            }
                        },
                    }
                }),

                new QueryOrderBy(true,"@p3","@p6"),
                queryParams,
                new QueryParam("@p5", "_ownerid_value"),
                new QueryParam("@p6", "name")
            };

            Console.WriteLine(queryOptions);

            var whoImI = _crmApiService.WhoImIAsync().Result;
            Console.WriteLine(whoImI);
            var contact = _crmApiService.ReadAsync("contact").Result;
            Console.WriteLine(contact);

            var account1 = _crmApiService.ReadAsync("account", queryOptions).Result;
            Console.WriteLine(account1);

            var queryOptionsNew = new QueryOptions()
            {
                new QuerySelect("name", "modifiedon", "createdon", "versionnumber"),
                new QueryCount(true),
                new QueryFilter(new FilterExpression()
                {
                    FilterOperator = LogicalOperator.LogicalAnd,
                    Conditions =
                    {
                        new ConditionExpression("@p1", ConditionOperator.Equal, 1),
                        new ConditionExpression("@p2", ConditionOperator.Equal, false),
                        new ConditionExpression("@p3", ConditionOperator.LessThanOrQqual, DateTime.Now)
                    },
                    Filters =
                    {
                        new FilterExpression(LogicalOperator.LogicalOr)
                        {
                            Conditions =
                            {
                                new ConditionExpression("@p4", ConditionOperator.Equal, 0),
                                new ConditionExpression("@p5", ConditionOperator.NotEqual,
                                    Guid.Parse("381512cc-f1cb-e611-80fb-00155d392b01"))
                            }
                        },
                    }
                }),
                new QueryExpand()
                {
                    ExpandNavigationProperties =
                    {
                        {
                            "primarycontactid", new List<QueryOption>()
                            {
                                new QuerySelect("fullname")
                            }
                        },
                        {
                            "Account_Tasks", new List<QueryOption>()
                            {
                                new QuerySelect("subject","scheduledstart"),
                                new QueryTop(3),
                                new QueryOrderBy("scheduledstart")
                            }
                        }
                    }
                },
                new QueryParams()
                {
                    ParamDictionary = {
                        {"@p1", "territorycode"},
                        {"@p2", "creditonhold"},
                        {"@p3", "createdon"},
                        {"@p4", "statecode"},
                        {"@p5", "_ownerid_value"},
                        {"@p6", "name"}
                    }
                }
            };

            var account3 = _crmApiService.ReadAsync("account", queryOptionsNew).Result;
            Console.WriteLine(account3);

            var e = _crmApiService.ReadAsync("account", "$select=name").Result;
            Console.WriteLine(e);

            var f = _crmApiService.ReadAsync("account", "$select=name").Result;
            Console.WriteLine(f);
            Console.ReadKey();
        }
    }
}