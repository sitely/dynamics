using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Workflow;
using System.Activities;

namespace CustomWorkflow
{
    public  class Workflow01 : CodeActivity
    {
        [Input("Key")] public InArgument<string> Key { get; set; }
        [Output("Tax")] public OutArgument<string> Tax { get; set; }
        protected override void Execute(CodeActivityContext executionContext)
        {
            

            //Create the tracing service
            ITracingService tracingService = executionContext.GetExtension<ITracingService>();

            //Create the context
            IWorkflowContext context = executionContext.GetExtension<IWorkflowContext>();
            IOrganizationServiceFactory serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
            IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);
        string key = Key.Get(executionContext);
            QueryByAttribute query = new QueryByAttribute("src_configuration");
            query.ColumnSet = new ColumnSet(new string[] { "src_value" });
            query.AddAttributeValue("src_name", key);
            EntityCollection collection = service.RetrieveMultiple(query);
            if (collection.Entities.Count != 1 )
            {
                tracingService.Trace("Something wrong with retrive multiple");
            }
            Entity config = collection.Entities.FirstOrDefault();
            Tax.Set(executionContext, config.Attributes["src_value"].ToString());
            
            //throw new NotImplementedException();
        }
    }
}
