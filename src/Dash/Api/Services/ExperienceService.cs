using Dash.Api.Operations;
using ServiceStack;

namespace Dash.Api.Services
{
    public class ExperienceService : Service
    {
        private IDataService DataService { get; set; }

        public ExperienceService(IDataService dataStore)
        {
            DataService = dataStore;
        }

        public ExperienceResponse Get(Experience request)
        {
            var experiences = DataService.GetExperiences();
            return new ExperienceResponse {Total = experiences.Count, Results = experiences};
        }
    }
}
