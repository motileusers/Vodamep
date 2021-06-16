using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text.RegularExpressions;
using Vodamep.Api.CmdQry;
using Vodamep.Hkpv;
using Vodamep.Hkpv.Model;
using Vodamep.ReportBase;

namespace Vodamep.Api.Engines.FileSystem
{
    public class FileEngine : IEngine
    {
        private readonly string _path;
        private readonly ILogger<FileEngine> _logger;
        private AuthContext _authContext;

        public FileEngine(FileEngineConfiguration configuration, ILogger<FileEngine> logger)
        {
            this._path = configuration.Path;
            this._logger = logger;

            _logger?.LogInformation("Uses Directory {path}", _path);

            if (!Directory.Exists(_path))
            {
                _logger?.LogInformation("Creates Directory {path}", _path);
                Directory.CreateDirectory(_path);
            }
        }
        public void Execute(ICommand cmd)
        {
            switch (cmd)
            {
                case ReportSaveCommand save:
                    this.Save(save.Report);
                    return;
                case TestCommand test:
                    return;
            }
        }

        private void Save(IReport report)
        {
            var (info, lastFilename) = GetLastFile(report.Institution.Id);

            if (info != null && string.Equals(GetFilename(report, info.Id), lastFilename, StringComparison.CurrentCultureIgnoreCase))
            {
                _logger.LogInformation("Report already exits. Skip saving.");
                return;
            }

            var filename = Path.Combine(_path, GetFilename(report, (info?.Id ?? 0) + 1));

            report.WriteToFile(filename, asJson: false, compressed: true);

            _logger.LogInformation("Report saved: {filename}", filename);
        }

        public void Login(IPrincipal principal)
        {
            this._authContext = principal != null ? new AuthContext(principal) : null;
        }

        private static Regex _filenamePattern = new Regex(@"^(?<id>\d+)__(?<institution>.+?)_(?<year>\d+)_(?<month>\d+)_(?<hash>.+?)\.(zip|hkpv|json)$");

        private string GetFilename(IReport report, int id) => $"{id:00000000}__{new ReportFilenameHandlerFactory().Create(report.ReportType).GetFileName(report, false, true)}";

        private (ReportInfo info, string filename) GetLastFile(string institution)
        {
            var currentId = Directory.GetFiles(_path).OrderByDescending(x => x)
                .Select(x => Path.GetFileName(x))
                .Select(x => new { Filename = x, Match = _filenamePattern.Match(x) })
                .Where(x => x.Match.Success)
                .Where(x => string.IsNullOrEmpty(institution) || string.Equals(x.Match.Groups["institution"].Value, institution, StringComparison.CurrentCultureIgnoreCase))
                .FirstOrDefault();

            if (currentId != null)
            {
                var info = new ReportInfo()
                {
                    Id = int.Parse(currentId.Match.Groups["id"].Value),
                    Institution = currentId.Match.Groups["institution"].Value,
                    Year = int.Parse(currentId.Match.Groups["year"].Value),
                    Month = int.Parse(currentId.Match.Groups["month"].Value),
                    HashSHA256 = currentId.Match.Groups["hash"].Value
                };

                return (info, currentId.Filename);
            }

            return (null, string.Empty);
        }
    }
}
