using Microsoft.Xrm.Sdk;
using System;
using Microsoft.Xrm.Sdk.Data.Exceptions;

namespace EmployeesDataProvider
{
    using System.Net.Http;
    using System.Threading.Tasks;

    public class Retrieve : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            ITracingService tracer = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
            IPluginExecutionContext context =
                (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            IOrganizationServiceFactory factory =
                (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = factory.CreateOrganizationService(context.UserId);

            try
            {
                EntityReference target = (EntityReference)context.InputParameters["Target"];

                tracer.Trace($"Target: {target.Id.ToString()}");

                var employeeHelper = new EmployeeHelper();

                var getEmployeeByIdTask = Task.Run(async () => await GetEmployeeByGuid(tracer, target.Id));
                Task.WaitAll(getEmployeeByIdTask);

                var result = getEmployeeByIdTask.Result;

                tracer.Trace($"Employee found: {result.id}");

                Entity Employee = EmployeeHelper.CreateEmployee(tracer, result);

                tracer.Trace($"Employee created: {Employee.Id}");

                context.OutputParameters["BusinessEntity"] = Employee;
            }
            catch (Exception e)
            {
                throw new InvalidPluginExecutionException(e.Message);
            }

        }

        private static async Task<Employee> GetEmployeeByGuid(ITracingService tracer, Guid id)
        {
            using (HttpClient client = HttpHelper.GetHttpClient())
            {
                string url = Constants.Url + $"/{id}";
                tracer.Trace(url);

                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, new Uri(url));

                HttpResponseMessage response = await client.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                    throw new GenericDataAccessException("Error retrieving Employee data");

                string html = response.Content.ReadAsStringAsync().Result;

                var employee = Utility.DeserializeObject<Employee>(html);

                return employee;
            }
        }
    }
}
