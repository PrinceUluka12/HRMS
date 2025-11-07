using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.SignalR
{
    public class EmployeeIdUserIdProvider : IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
        {
            // Look for a claim named "employeeId" first, fall back to NameIdentifier
            var ctx = connection.User;
            if (ctx == null || !ctx.Identity?.IsAuthenticated == true)
                return null;

            // Prefer OID (Azure AD user object ID)
            var claim = ctx.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier");

            return claim?.Value;
        }
    }
}
