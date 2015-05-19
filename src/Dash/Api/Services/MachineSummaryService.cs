using System;
using System.Collections.Generic;
using System.Linq;
using Dash.Api.Operations;
using ServiceStack;

namespace Dash.Api.Services
{
    public class MachineSummaryService : Service
    {
        private IDataService DataService { get; set; }

        public MachineSummaryService(IDataService dataService)
        {
            DataService = dataService;
        }

        public MachineSummaryResponse Get(MachineSummary request)
        {
            var response = new List<MachineSummary>();
            var experiences = DataService.GetExperiences();

            foreach (var machine in experiences.GroupBy(m => m.Machine).OrderBy(g => g.Key))
            {
                var machineRoot = new Node {Name = machine.Key};
                var machineSummary = new MachineSummary
                {
                    Name = machine.Key,
                    Root = machineRoot
                };
                response.Add(machineSummary);

                foreach (var result in machine.GroupBy(m => m.Result).OrderBy(m => m.Key))
                {
                    var resultSummary = new Node {Name = result.Key};
                    machineRoot.Children.Add(resultSummary);

                    foreach (var version in result.GroupBy(m => m.Version).OrderBy(g => g.Key))
                    {
                        var versionNode = new Node { Name = version.Key };
                        resultSummary.Children.Add(versionNode);

                        foreach (var type in version.GroupBy(v => v.ExperienceType).OrderBy(g => g.Key))
                        {
                            var typeNode = new Node { Name = type.Key };
                            versionNode.Children.Add(typeNode);
                            
                            foreach (var name in type.GroupBy(t => t.Name))
                            {
                                foreach (var experience in name)
                                {
                                    typeNode.Children.Add(experience == name.First() ? new Node { Name = experience.Name } : new Node());
                                }
                            }
                        }
                    }
                }

                machineSummary.Successes = GetSuccessCount(machineSummary);
                machineSummary.Failures = GetFailureCount(machineSummary);
                machineSummary.SuccessRatio = GetSuccessRatio(machineSummary);
            }

            response.Insert(0, GetOverview(experiences));
            return new MachineSummaryResponse { Total = response.Count, Results = response };
        }

        private int GetSuccessCount(MachineSummary machineSummary)
        {
            try
            {
                return machineSummary.Root.Children
                    .Where(n => n.Name == "Success")
                    .SelectMany(n => n.Children)
                    .SelectMany(n => n.Children)
                    .SelectMany(n => n.Children).Count();
            }
            catch
            {
                return 0;
            }
        }

        private int GetFailureCount(MachineSummary machineSummary)
        {
            try
            {
                return machineSummary.Root.Children
                    .Where(n => n.Name == "Failure")
                    .SelectMany(n => n.Children)
                    .SelectMany(n => n.Children)
                    .SelectMany(n => n.Children).Count();
            }
            catch (Exception)
            {
                return 0;
            }
        }

        private double GetSuccessRatio(MachineSummary machineSummary)
        {
            try
            {
                if (machineSummary.Successes == 0)
                    return 0;
                if (machineSummary.Failures == 0)
                    return 1;
                return Math.Round((double) machineSummary.Successes / (machineSummary.Successes + machineSummary.Failures), 2, MidpointRounding.AwayFromZero);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        private MachineSummary GetOverview(List<Experience> experiences)
        {
            var overview = new MachineSummary
            {
                Name = "Overview",
                Root = new Node{Name = "Overview"}
            };

            foreach (var result in experiences.GroupBy(m => m.Result).OrderBy(m => m.Key))
            {
                var resultSummary = new Node {Name = result.Key};
                overview.Root.Children.Add(resultSummary);

                foreach (var machine in result.GroupBy(m => m.Machine).OrderBy(g => g.Key))
                {
                    var machineNode = new Node {Name = machine.Key};
                    resultSummary.Children.Add(machineNode);

                    foreach (var type in machine.GroupBy(v => v.ExperienceType).OrderBy(g => g.Key))
                        {
                            var typeNode = new Node {Name = type.Key};
                            machineNode.Children.Add(typeNode);

                            foreach (var name in type.GroupBy(t => t.Name))
                            {
                                foreach (var experience in name)
                                {
                                    typeNode.Children.Add(experience == name.First() ? new Node {Name = experience.Name} : new Node());
                                }
                            }
                        }
                }
            }

            overview.Successes = GetSuccessCount(overview);
            overview.Failures = GetFailureCount(overview);
            overview.SuccessRatio = GetSuccessRatio(overview);

            return overview;
        }
    }
}
