using System;
using System.Collections.Generic;
using ServiceStack;

namespace Dash.Api.Operations
{
    [Route("/api/experience", "GET POST")]
    [Authenticate(ApplyTo.Post | ApplyTo.Put | ApplyTo.Delete)] 
    public class Experience
    {
        public DateTime Timestamp { get; set; }
        public string Name { get; set; }
        public string Machine { get; set; }
        public string Version { get; set; }
        public string ExperienceType { get; set; }
        public string StackInstallResult { get; set; }
        public string StackModificationResult { get; set; }
        public string MachineRebootResult { get; set; }
        public string AitLaunchResult { get; set; }
        public string SupervisorResult { get; set; }
        public string TellerSessionResult { get; set; }
        public string OtherResult { get; set; }

        public string Result
        {
            get
            {
                if (StackInstallResult == "Success" || StackModificationResult == "Success" || MachineRebootResult == "Success" || AitLaunchResult == "Success" || SupervisorResult == "Success" || TellerSessionResult == "Success" || OtherResult == "Success")
                    return "Success";
                else if (StackInstallResult == "Failure" || StackModificationResult == "Failure" || MachineRebootResult == "Failure" || AitLaunchResult == "Failure" || SupervisorResult == "Failure" || TellerSessionResult == "Failure" || OtherResult == "Failure")
                    return "Failure";
                else
                    return "Unavailable Result";
            }
        }
    }
    
    public class ExperienceResponse : ResponseStatus
    {
        public int Total { get; set; }
        public List<Experience> Results { get; set; }
    }
}
