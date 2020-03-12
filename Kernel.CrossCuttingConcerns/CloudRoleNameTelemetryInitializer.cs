using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;

namespace Kernel.CrossCuttingConcerns
{
    class CloudRoleNameTelemetryInitializer : ITelemetryInitializer
    {
        readonly string _roleName;
        readonly string _roleInstance;
        public CloudRoleNameTelemetryInitializer(string roleName = null, string roleInstance = null)
        {
            _roleName = roleName ?? "api";
            _roleInstance = roleInstance;
        }
        public void Initialize(ITelemetry telemetry)
        {
            telemetry.Context.Cloud.RoleName = _roleName;
            if(!string.IsNullOrWhiteSpace(_roleInstance))
                telemetry.Context.Cloud.RoleInstance = _roleInstance;
        }
    }
}
