using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;

namespace Kernel.CrossCuttingConcerns
{
    class CloudRoleNameInitializer : ITelemetryInitializer
    {
        readonly string roleName;
        public CloudRoleNameInitializer(string roleName = null)
        {
            this.roleName = roleName ?? "api";
        }
        public void Initialize(ITelemetry telemetry)
        {
            telemetry.Context.Cloud.RoleName = roleName;
            telemetry.Context.Cloud.RoleInstance = "go away from cloud thing";
        }
    }
}
