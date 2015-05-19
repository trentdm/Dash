using System;
using System.Collections.Generic;
using System.Linq;
using Dash.Api.Operations;
using ServiceStack;

namespace Dash.Api.Services
{
    public class ExperienceSummaryService : Service
    {
        private IDataService DataService { get; set; }

        public ExperienceSummaryService(IDataService dataStore)
        {
            DataService = dataStore;
        }

        public ExperienceSummaryResponse Get(ExperienceSummary request)
        {
            var experienceGroups = DataService.GetExperiences().Where(e => !string.IsNullOrEmpty(e.TellerSessionResult)).GroupBy(e => e.Machine + "_" + e.Version);
            var summaries = new List<ExperienceSummary>();

            foreach (var group in experienceGroups)
            {
                var successes = group.Count(e => e.TellerSessionResult == "Success");
                var failures = group.Count(e => e.TellerSessionResult == "Failure");
                var rate = (double)successes/(successes + failures);
                summaries.Add(new ExperienceSummary {Setup = group.Key, SuccessCount = successes, FailureCount = failures, SuccessRatio = rate});
            }

            return new ExperienceSummaryResponse {Total = summaries.Count, Results = summaries.OrderBy(s => s.Setup).ToList()};
        }
    }
}
