using Dapper;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Vodamep.Legacy.Model;

namespace Vodamep.Legacy.Reader
{
    public class Td45Reader : IReader
    {
        private readonly string _connectionstring;

        public Td45Reader(string connectionstring)
        {
            _connectionstring = connectionstring;
        }

        public ReadResult Read(int year, int month)
        {
            using (var connection = new SqlConnection(_connectionstring))
            {
                connection.Open();

                var from = new DateTime(year, month, 1);
                var to = from.LastDateInMonth().Date.AddHours(23).AddMinutes(59);

                var sqlLeistungen = @"DELETE qryhtblSeniorid;
-- ############ infrage kommende Klienten in Hilfstabelle schreiben ############
-- ############ und zwar alle, die Leistungen mit entsprechendem KISCode haben (unabhängig von Belegung) ############
INSERT qryhtblSeniorID
SELECT SeniorID FROM tblDFN 
WHERE KISCode like '%HK%' AND Datum BETWEEN @from AND @to 
GROUP BY SeniorID;

-- ############ Auswertung der KISCodes vornehmen ############
EXECUTE procKISAuswertung @from, @to;

-- ############ Ergebnisabfrage ############
-- ############ Leistungen 31 und 33 sind dabei, 32 und 34 nicht ############
SELECT q.SeniorID AS Adressnummer, q.PflegerID AS Pfleger, 
	convert(smalldatetime,convert(varchar(8),q.Datum,112),112) AS Datum, 
    convert(int,kbp.PunktID) AS Leistung, Count(q.Wert) AS Anzahl
FROM qryhtblKISAuswertung q
INNER JOIN tblKISBereichPunkt kbp ON q.KISBereichPunktID = kbp.KISBereichPunktID
WHERE kbp.KISBereichID LIKE 'HK%'
GROUP BY q.SeniorID, q.PflegerID, convert(smalldatetime,convert(varchar(8),q.Datum,112),112), convert(int,kbp.PunktID);";

                //Timeout auf 3 Minuten setzen, weil die KISCode-Auswertung relativ lang dauert
                var leistungenCommand = new CommandDefinition(sqlLeistungen, new { from = from, to = to }, commandTimeout: 180);
                var leistungen = connection.Query<LeistungDTO>(leistungenCommand).ToArray();

                if (!leistungen.Any())
                    return ReadResult.Empty;

                var adressnummern = leistungen.Where(x => x.Leistung < 20).Select(x => x.Adressnummer).Distinct().ToArray();

                var sqlAdressen = $@"SELECT s.SeniorID AS Adressnummer, 
    coalesce(s.Nachname, '???') AS Name_1, coalesce(s.Vorname, '???') AS Name_2,
    q.Postleitzahl, q.Ort, s1.Geburtsdatum,
	(SELECT TOP 1 Land FROM tblLand WHERE Bezeichnung LIKE s1.Staatsangehörigkeit) AS Staatsbuergerschaft,
    v.Code AS Versicherung, s1.VNummer1 AS Versicherungsnummer, s1.Geschlecht
FROM tblSenior s
INNER JOIN tblSenior1 s1 ON s.SeniorID = s1.SeniorID
LEFT JOIN tblVersicherung v ON v.Versicherung = s1.Versicherung1
LEFT JOIN(
       SELECT sbp.SeniorID, bp.Land, bp.Ort, bp.Postleitzahl
       FROM tblSeniorBezugspersonen sbp
           INNER JOIN (SELECT SeniorID, min(BezugID) as BezugID FROM tblSeniorBezugspersonen
               WHERE Beziehung Like 'Adresse' GROUP BY SeniorID) q0 ON sbp.BezugID = q0.BezugID
           INNER JOIN tblBezugspersonen bp ON sbp.BPersonID = bp.BPersonID
       ) q ON s.SeniorID = q.SeniorID
WHERE s.SeniorID IN @ids";

                var adressen = connection.Query<AdresseDTO>(sqlAdressen, new { to = to, ids = adressnummern }).ToArray();

                var pflegestufenSql = @"SELECT s.SeniorID,
	(SELECT TOP 1 s2.WertText FROM tblSenior2 s2 
		WHERE s2.ctlName LIKE 'H21'
			AND LEN(COALESCE(s2.WertText,'')) > 0 AND @to between s2.vonDatum and s2.bisDatum
			AND s2.SeniorID = s.SeniorID
		ORDER BY s2.vonDatum DESC) AS Item2
FROM tblSenior s WHERE s.SeniorID IN @ids;";

                var pflegestufen = connection.Query<(int Adressnummer, string Wert)>(pflegestufenSql, new { to = to, ids = adressnummern }).ToArray();

                foreach (var a in adressen)
                {
                    var ps = pflegestufen.Where(x => x.Adressnummer == a.Adressnummer).Select(x => x.Wert).FirstOrDefault();

                    if (string.IsNullOrEmpty(ps))
                        a.Pflegestufe = (int)Hkpv.Model.CareAllowance.Unknown;
                    else
                        a.Pflegestufe = int.Parse(ps);
                }

                var pflegernummern = leistungen.Select(x => x.Pfleger).Distinct().ToArray();

                var sqlPfleger = @"SELECT p.PflegerID AS Pflegernummer, coalesce(p.Nachname, '???') + ' ' + coalesce(p.Vorname, '???') AS Pflegername,
	COALESCE((SELECT TOP 1 CASE WHEN Tätigkeit LIKE 'DGK%' THEN 'DGKP' ELSE Tätigkeit END
		FROM dbo.fnTdPflegerDienstverhältnisInfo(@from, @to) pdv WHERE pdv.PflegerID = p.PflegerID),'') AS Berufstitel
FROM tblPfleger p WHERE p.PflegerID IN @ids;";

                var pfleger = connection.Query<PflegerDTO>(sqlPfleger, new { from = from, to = to, ids = pflegernummern }).ToArray();

                var sqlAnstellung = @"SELECT CASE WHEN AnstellungVon BETWEEN @from AND @to THEN AnstellungVon ELSE @from END AS Von,
	CASE WHEN AnstellungBis BETWEEN @from AND @to THEN AnstellungBis ELSE @to END AS Bis,
	AnstellungWert1/8.0 AS VZAE, PflegerID AS Pflegernummer,
	COALESCE(CASE WHEN Tätigkeit LIKE 'DGK%' THEN 'DGKP' ELSE Tätigkeit END,'') AS Berufstitel
FROM dbo.fnTdPflegerDienstverhältnisInfo(@from, @to) WHERE PflegerID IN @ids;";

                var anstellung = connection.Query<AnstellungDTO>(sqlAnstellung, new { from = from, to = to, ids = pflegernummern }).ToArray();

                var sqlVerein = @"SELECT TOP 1 convert(int,coalesce(Wert,'0')) AS Vereinsnummer, 
	coalesce((SELECT TOP 1 Wert FROM tblEinstellungText 
		WHERE Bezeichnung like 'ConnexiaVereinsbezeichnung'),'') AS Bezeichnung 
FROM tblEinstellungText WHERE Bezeichnung like 'ConnexiaVereinsnummer';";

                var verein = connection.QueryFirst<VereinDTO>(sqlVerein);

                return new ReadResult() { A = adressen, P = pfleger, S = anstellung, L = leistungen, V = verein };
            }
        }
    }
}
