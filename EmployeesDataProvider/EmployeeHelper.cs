using Microsoft.Xrm.Sdk;
using System;

namespace EmployeesDataProvider
{
    class EmployeeHelper
    {
        public static Entity CreateEmployee(ITracingService tracer, Employee result)
        {
            tracer.Trace($"Result Id: {result.id}");


            return new Entity("amrc_employee")
                       {
                           ["amrc_employee_id"] = result.id,
                           ["amrc_employee_username"] = result.Username,
                           ["amrc_employee_surname"] = result.Surname,
                           ["amrc_employee_forename"] = result.Forename,
                           ["amrc_employee_status"] = result.Status
            };
        }
    }
}
