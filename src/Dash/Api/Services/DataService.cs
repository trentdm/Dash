using System;
using System.Collections.Generic;
using System.Linq;
using Dash.Api.Operations;
using Dash.Api.Utils;
using Google.GData.Client;
using Google.GData.Spreadsheets;

namespace Dash.Api.Services
{
    public interface IDataService
    {
        List<Experience> GetExperiences();
    }

    public class DataService : IDataService
    {
        private static List<Experience> _experiences;
        private static DateTime _lastUpdate;

        public DataService()
        {
            _experiences = new List<Experience>();
        }

        public List<Experience> GetExperiences()
        {
            if (_lastUpdate.AddMinutes(5) < DateTime.Now)
            {
                lock (_experiences)
                {
                    _experiences = GetExperiencesFromSheet();
                    _lastUpdate = DateTime.Now;
                }
            }

            return _experiences;
        }

        private List<Experience> GetExperiencesFromSheet()
        {
            var experiences = new List<Experience>();
            var service = GetService();

            foreach (var entry in GetSpreadSheetEntries(service))
                foreach (var worksheet in GetWorkSheetEntries(entry, service))
                    foreach (var row in GetListEntries(worksheet, service))
                        experiences.Add(row.Deserialize<Experience>());

            return experiences;
        }

        private SpreadsheetsService GetService()
        {
            var environment = EnvironmentSerializer.GetEnvironment();
            var service = new SpreadsheetsService("Engineering Stack Experience (Responses)");
            service.setUserCredentials(environment.GmailUser, environment.GmailPass);
            return service;
        }

        private IEnumerable<AtomEntry> GetSpreadSheetEntries(SpreadsheetsService service)
        {
            var spreadSheetQuery = new SpreadsheetQuery();
            return service.Query(spreadSheetQuery).Entries;
        }

        private IEnumerable<WorksheetEntry> GetWorkSheetEntries(AtomEntry entry, SpreadsheetsService service)
        {
            var link = entry.Links.FindService(GDataSpreadsheetsNameTable.WorksheetRel, null);
            var workSheetQuery = new WorksheetQuery(link.HRef.ToString());
            var workSheetFeed = service.Query(workSheetQuery);
            return workSheetFeed.Entries
                .Where(e => e.Title.Text == "Form Responses 1")
                .Cast<WorksheetEntry>();
        }

        private IEnumerable<ListEntry> GetListEntries(WorksheetEntry worksheet, SpreadsheetsService service)
        {
            var listFeedLink = worksheet.Links.FindService(GDataSpreadsheetsNameTable.ListRel, null);
            var listQuery = new ListQuery(listFeedLink.HRef.ToString());
            var listFeed = service.Query(listQuery);
            return listFeed.Entries.Cast<ListEntry>();
        }
    }
}
