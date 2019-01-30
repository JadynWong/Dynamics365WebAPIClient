# Dynamics365 WebAPI Client

## Support

* IFD - ADFS3.0
* IFD - ADFS4.0
* On-Premise

## Demo Code
```         
 var queryOptions = new QueryOptions(){}
        new QuerySelect("fullname", "modifiedon", "createdon","versionnumber"),
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
                        new ConditionExpression("@p5", ConditionOperator.NotEqual, Guid.Parse("381512cc-f1cb-e611-80fb-00155d392b01"))
                    }
                },

            }
        }),
        new QueryParam("@p1", "territorycode"),
        new QueryParam("@p2", "creditonhold"),
        new QueryParam("@p3", "createdon"),
        new QueryParam("@p4", "statecode"),
        new QueryParam("@p5", "_ownerid_value"),
        new QueryParam("@p6", "fullname"));
    Console.WriteLine(queryOptions);
    var crmApiService = new WebApiService();
    //var a = crmApiService.WhoImIAsync().Result;
    //Console.WriteLine(a);
    var a = crmApiService.ReadAsync("contact").Result;
    Console.WriteLine(a);

    var d = crmApiService.ReadAsync("contact", queryOptions).Result;
    Console.WriteLine(d);

    var b = crmApiService.ReadAsync("systemuser", "$select=fullname").Result;
    Console.WriteLine(b);

    var c = crmApiService.ReadAsync("systemuser", "$select=fullname").Result;
    Console.WriteLine(c);
```
