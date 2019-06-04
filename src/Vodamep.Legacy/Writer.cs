using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Vodamep.Data;
using Vodamep.Hkpv.Model;
using Vodamep.Legacy.Reader;

namespace Vodamep.Legacy
{
    public class Writer
    {
        Dictionary<string, string> unknonwStaffIds = new Dictionary<string, string>();

        public string Write(string path, ReadResult data, bool asJson = false)
        {
            var date = data.L.Select(x => x.Datum).First().FirstDateInMonth();
            var report = new HkpvReport()
            {
                FromD = date,
                ToD = date.LastDateInMonth(),
                Institution = new Institution() { Id = data.V.Vereinsnummer, Name = data.V.Bezeichnung }
            };
            foreach (var a in data.A)
            {
                var name = (Familyname: a.Name_1, Givenname: a.Name_2);
                if (string.IsNullOrEmpty(name.Givenname)) name = GetName(a.Name_1);

                var (Postcode, City) = GetPostCodeCity(a);

                report.AddPerson(new Person()
                {
                    Id = GetId(a.Adressnummer),
                    BirthdayD = a.Geburtsdatum,
                    FamilyName = (name.Familyname ?? string.Empty).Trim(),
                    GivenName = (name.Givenname ?? string.Empty).Trim(),
                    Ssn = (a.Versicherungsnummer ?? string.Empty).Trim(),
                    Insurance = (a.Versicherung ?? string.Empty).Trim(),
                    Nationality = (a.Staatsbuergerschaft ?? string.Empty).Trim(),
                    CareAllowance = (CareAllowance)a.Pflegestufe,
                    Postcode = Postcode,
                    City = City,
                    Gender = GetGender(a.Geschlecht)
                });
            }

            foreach (var p in data.P)
            {
                var (Familyname, Givenname) = GetName(p.Pflegername);

                Staff staff = new Staff()
                {
                    Id = GetStaffID(p.Pflegernummer, data.V.Vereinsnummer),
                    FamilyName = Familyname,
                    GivenName = Givenname,
                    Qualification = p.Berufstitel
                };


                var anstellungen = data.S.Where(x => x.Pflegernummer == p.Pflegernummer).OrderBy(x => x.Von.GetValueOrDefault(DateTime.MinValue));
                foreach (var anstellung in anstellungen)
                {
                    if (anstellung.Von == null)
                        anstellung.Von = DateTime.MinValue;

                    if (anstellung.Bis == null)
                        anstellung.Bis = DateTime.MaxValue;


                    if (anstellung.Von < date)
                        anstellung.Von = date;

                    if (anstellung.Bis > date.LastDateInMonth())
                        anstellung.Bis = date.LastDateInMonth();


                    Debug.WriteLine(anstellung.Von + " " + anstellung.Bis);


                    Employment employment = new Employment()
                    {
                        FromD = anstellung.Von,
                        ToD = anstellung.Bis,
                        HoursPerWeek = 40 * anstellung.VZAE,
                    };

                    staff.Employments.Add(employment);
                }



                report.Staffs.Add(staff);
            }

            foreach (var l in data.L.GroupBy(x => new { x.Datum, x.Adressnummer, x.Pfleger }))
            {
                var a = new Activity()
                {
                    StaffId = GetStaffID(l.Key.Pfleger, data.V.Vereinsnummer),
                    DateD = l.Key.Datum
                };
                foreach (var entry in l.OrderBy(x => x.Leistung))
                {
                    var t = (ActivityType)entry.Leistung;
                    for (var i = 0; i < entry.Anzahl; i++)
                        a.Entries.Add(t);
                }

                if (a.RequiresPersonId())
                {
                    a.PersonId = GetId(l.Key.Adressnummer);
                }
                report.Activities.Add(a);
            }

            var filename = report.AsSorted().WriteToPath(path, asJson: asJson, compressed: false);

            if (unknonwStaffIds.Count > 0)
                Console.WriteLine($"Unbekannte PflegerIds: {String.Join(", ", unknonwStaffIds.Keys.ToArray())}");
            return filename;
        }


        private string GetStaffID(int oldID, string verein)
        {
            if (verein == "06")
            {
                switch (oldID)
                {
                    // Dornbirn
                    case 224: return "1213";
                    case 1215: return "1215";
                    case 1174: return "1174";
                    case 1265: return "1265";
                    case 263: return "1063";
                    case 1098: return "1098";
                    case 1272: return "1272";
                    case 575: return "575";
                    case 1188: return "1188";
                    case 566: return "566";
                    case 1224: return "1224";
                    case 1114: return "1114";
                    case 5034: return "5034";
                    case 1040: return "1040";
                    case 1061: return "1061";
                    case 1175: return "1175";
                    case 1214: return "1214";
                    case 714: return "714";


                    case 89: return "3";
                    case 1213: return "12";
                    case 1063: return "19";
                    case 284: return "20";
                    case 325: return "25";
                    case 339: return "27";
                    case 375: return "30";
                    case 457: return "42";
                    case 478: return "43";
                    case 481: return "44";
                    case 531: return "55";
                    case 542: return "58";
                    case 597: return "66";
                    case 633: return "72";
                    case 634: return "73";
                    case 635: return "74";
                    case 659: return "78";
                    case 677: return "80";
                    case 685: return "82";
                    case 731: return "89";
                    case 1018: return "93";
                    case 1021: return "94";
                    case 1036: return "97";
                    case 1058: return "99";
                    case 1115: return "110";
                    case 1123: return "113";
                    case 1127: return "114";
                    case 1156: return "118";
                    case 1205: return "129";
                    case 1223: return "140";
                    case 1225: return "142";
                    case 1227: return "143";
                    case 1237: return "144";
                    case 1239: return "145";
                    case 1241: return "146";
                    case 1249: return "147";
                    case 1252: return "148";
                    case 1268: return "150";
                    case 1282: return "152";
                    case 1313: return "153";
                    case 1316: return "154";
                    case 1319: return "155";

                    default:

                        if (!unknonwStaffIds.ContainsKey(oldID.ToString()))
                            unknonwStaffIds.Add(oldID.ToString(), oldID.ToString());

                        return "NA" + "-" + oldID;
                }

            }
            else
            {
                switch (oldID)
                {


                    // Götzis, Lustenau, ...
                    case 304: return "304";
                    case 319: return "5015";
                    case 5036: return "5036";
                    case 5007: return "5007";
                    case 1317: return "1317";
                    case 1144: return "1144";
                    case 12: return "214";
                    case 14: return "296";
                    case 60: return "465";
                    case 322: return "1332";
                    case 213: return "1043";
                    case 244: return "1094";
                    case 243: return "1107";
                    case 42: return "702";
                    case 11: return "215";
                    case 295: return "1220";
                    case 276: return "1035";
                    case 210: return "1034";
                    case 19: return "348";
                    case 13: return "218";
                    case 298: return "1228";
                    case 310: return "1126";
                    case 313: return "1273";
                    case 214: return "12";
                    case 1007: return "187";
                    case 465: return "60";
                    case 1043: return "213";
                    case 1094: return "244";
                    case 698: return "698";
                    case 1107: return "243";
                    case 724: return "198";
                    case 702: return "42";
                    case 215: return "11";
                    case 1220: return "295";
                    case 1035: return "276";
                    case 1034: return "210";
                    case 1006: return "1006";
                    case 1246: return "1246";
                    case 348: return "15";
                    case 218: return "13";
                    case 1228: return "298";
                    case 1222: return "296";
                    case 296: return "14";
                    case 1126: return "310";
                    case 5015: return "5015";
                    case 1273: return "313";
                    case 1229: return "1229";
                    case 1231: return "1231";
                    case 5013: return "5013";
                    case 5032: return "5032";



                    case -1233: return "76D2CBF6-0F5E-48D0-9550-E71E08BE59AC";
                    case 697: return "00000000-0000-0000-0000-000000001001";
                    case 437: return "00000000-0000-0000-0000-000000001002";
                    case 495: return "00000000-0000-0000-0000-000000001003";
                    case 547: return "39EA2FBC-17BA-4168-8E8A-00AF0783F482";
                    case 1152: return "5141042E-B8E8-4CDF-953E-0258784B3E01";
                    case 1017: return "D10D07C8-B16D-435D-B175-0316D85B124C";
                    case 1184: return "15BB9A2E-2030-43AE-A0BF-036E6BFC21AF";
                    case 604: return "ACFADDAD-9418-47FC-9B94-0419DC502D0A";
                    case 1095: return "F4AC6C10-992D-4C68-81A2-063A2593A638";
                    case 1300: return "B4461298-508F-47DC-BEE7-0739848D063F";
                    case 1170: return "990593F4-676E-488A-B52B-08536A532A06";
                    case 577: return "AE82A61C-5808-487A-8F70-09E9CCBF1C52";
                    case 1218: return "3B582525-7122-49B6-8DCF-09F5C58E534B";
                    case 605: return "D052C271-70A3-498E-9584-0A186C7D926C";
                    case 419: return "3B376EE8-F340-4AF7-B8FA-0B04D527E732";
                    case 539: return "CE698887-63CD-4B4E-8083-0BF63185B9F6";
                    case 669: return "525E18E4-8275-436E-9D67-0C28744F7731";
                    case 5042: return "3A92FB33-F538-4917-9047-0D2E21D65718";
                    case 1178: return "E970400D-6EE0-4677-AB90-0D65152EC289";
                    case 1208: return "273FD943-1E82-470E-83BF-0D7E358A43B5";
                    case 1092: return "BC89C15A-E382-4F1C-AEDC-0E0B47115936";
                    case 1080: return "80756F71-50D5-4639-87F9-10562A9C5173";
                    case 287: return "40353A7A-9264-4B5D-A7D3-11E49AE362F8";
                    case 5024: return "D2D0E904-E261-4809-AAC9-1219CD1131BE";
                    case 627: return "3EE8836D-F14D-44DD-8F78-131371AA9C4A";
                    case 1226: return "D7678DEE-2D57-40C1-87D0-1349D7F0892C";
                    case 1002: return "2966AD22-71C7-4F1E-BC7E-1354DD49525E";
                    case 661: return "E8FE9815-4B68-4DA8-9E33-139174727B71";
                    case 368: return "99811661-5E6B-4A0D-B80F-14605F7F24B5";
                    case 1038: return "9DE16026-A936-4267-87A5-153D1C7F89E3";
                    case 554: return "E3C8E218-8615-468C-A68D-1597CF09DC17";
                    case 237: return "E2830608-3BDD-40B0-B555-1619142FA483";
                    case 1242: return "7A97B7E4-1FE7-4F4C-A3D7-171757677ABE";
                    case 1169: return "7F582894-621C-4D45-9ACA-171DC7111395";
                    case 1081: return "9C627879-F002-46F5-A45A-17C203237506";
                    case 458: return "39905EB3-349D-4F41-847C-19304A826A57";
                    case 1037: return "A1AFD60B-A5B7-43DD-9BDA-1990581E2903";
                    case 525: return "3BF30E54-5081-4510-B272-19945845E4FC";
                    case 1103: return "2EA06820-A69C-4A61-A179-1AA934725B76";
                    case 1247: return "FE932364-1859-4504-8E29-1B6EF495E4C6";
                    case 1179: return "1EA40143-70E3-41F6-BE8B-1B9AB744E110";
                    case 1185: return "77ED79E6-0A6E-480A-B36B-1C4855A9F36A";
                    case 1024: return "F5DD97FF-E007-4ADC-B1EF-1D1CA5311E0E";
                    case 1312: return "7EF08C9B-07FB-448B-A817-1DA394C211D0";
                    case 1001: return "B0A2BF26-DDB0-4DA2-9917-1F334B9CF0F5";
                    case 1167: return "377B2FDA-60E9-47B3-85B9-1F672D1E1188";
                    case 1136: return "01C36BA7-0EEC-4A92-90A1-1F727CDA90B6";
                    case 1191: return "D15E802F-221D-490F-94E2-1FBF3F182FB4";
                    case 1104: return "E7AD3B83-AEDD-4D7C-963B-20DFD9834599";
                    case 624: return "F5BB27A1-51A3-4F90-A94A-2108B552A6CA";
                    case 527: return "7104D684-F2D4-4E10-8F25-2154758011D3";
                    case 335: return "88700002-3CB7-45DD-8ED1-222D7C4B1DDA";
                    case 1189: return "A3DD9923-537F-4577-8151-2275E7EB4F14";
                    case 1251: return "C609EEB6-9900-4D24-94B5-22CC8B8A361B";
                    case 586: return "8FF5DE39-F323-42A2-AABC-22F4ED155317";
                    case 620: return "D55E455B-9D63-4D87-B930-2321FDC4F252";
                    case 144: return "CEB9D9AB-88CB-475E-8831-23EE6008833B";
                    case 589: return "7A2E8F44-AB77-4923-B59B-2568AB1BE182";
                    case 1121: return "725E087A-389E-4B67-8E1E-27ECE3BDC550";
                    case 683: return "62B32DAD-A7D5-433E-B8F6-28747CA4145F";
                    case 452: return "AA3BBDAB-19E5-44F8-AA5D-289FFD083E9D";
                    case 459: return "051E404C-5686-478C-BCC3-2917AE688D94";
                    case 5030: return "3020BB1A-2DE3-461B-85F5-297EF87ED439";
                    case 1204: return "6DD58CDE-3787-4FF9-B87C-29A650EBEE49";
                    case 1069: return "301BE1E5-6123-4A1D-A6F7-2AF41C70FD76";
                    case 5019: return "CC281C43-597A-4499-9CF3-2AFEE8B82B94";
                    case 747: return "9DCB44AE-6A20-43B0-B639-2BF7CFAE8788";
                    case 613: return "7763A6A8-5E8B-4063-890C-2C39E49823B5";
                    case 1128: return "11E355C8-7E3D-49D3-B192-2C62BDB6B164";
                    case 1074: return "A7BBF823-CE55-4855-89C0-2CBA7E180A7C";
                    case 1014: return "7644A201-D4A9-4FD1-9EAE-2DB47ADA68D2";
                    case 743: return "C7995CA6-1D75-4B14-9B68-2E2786594569";
                    case 1299: return "3F0707B0-2F24-464B-82CC-2E5B44335CB9";
                    case 1301: return "86B80B95-B051-4C0B-9222-2E6DF323B28E";
                    case 1161: return "3564052F-FAE1-4CFC-A11D-2FDB0CBD5AC4";
                    case 397: return "EAFAB335-B787-4F6C-B1E5-30395758BBC0";
                    case 1044: return "1271C401-64D2-45D4-B6C4-30A3827E0974";
                    case 570: return "D84F0F05-93D3-4FB0-A7D5-3226AAF7164C";
                    case 581: return "EB51D38B-CAF0-429F-8872-329AC47BB599";
                    case 1302: return "22EE0ADC-8293-4D0E-B5EF-329EAEE29A0A";
                    case 1290: return "2D3E6E4E-89A7-4B80-A1EF-33441B1D5A89";
                    case 405: return "B87B1E6E-474D-4317-93E0-33E54D717DC3";
                    case 1192: return "4B408BD3-443F-41EB-A376-34DE9343AD70";
                    case 1070: return "00BE1D5B-B6D7-4410-A48F-3532C657CDC8";
                    case 1141: return "D9684206-6312-4FD9-BD00-35702C2D0449";
                    case 1166: return "A8FAEAB3-8CCA-4145-B153-37512402224E";
                    case 666: return "5CB02B22-17FC-4F9B-B6E1-37D648EDD19B";
                    case 370: return "CE27DE98-7E76-49F3-94BB-3808DAFF9824";
                    case 125: return "1DA5F07F-2E5F-4D79-883C-38E73D47FE0E";
                    case 1285: return "A19B5766-CF37-44FE-BB85-38F1A78FB73A";
                    case 1288: return "6DC399A7-DC56-4FA4-9EFF-38F42022F619";
                    case 5049: return "5441A765-EF69-4FB0-B9DF-3935D14AB321";
                    case 664: return "5D839CE1-D42F-4C71-A0F4-394264DD20D1";
                    case 1245: return "4FF1BB4E-9963-42F3-8457-3A0C3A5839FF";
                    case 427: return "C2FB5AE1-3D68-431B-B460-3B37D94DE8ED";
                    case 1274: return "52797CE4-4ECC-41A5-A445-3C94EF71C060";
                    case 1275: return "4D2381A3-64CB-4585-AC52-3CD214F7EBB5";
                    case 208: return "2F7F5308-443A-46F9-9C0D-3ED7D2EE93A7";
                    case 1091: return "72FBF00A-4340-485C-B2CC-40B60C8C54BE";
                    case 462: return "DEB86B88-EACC-4795-A43E-40C7A1AE0713";
                    case 1120: return "B564EA3D-2CFD-4A40-B32F-4167D0CB9F58";
                    case 288: return "58496B1E-2801-42FE-8767-41FD914B9EBE";
                    case 736: return "CC2629B0-E4A6-4A65-B2A4-420E8638202B";
                    case 1262: return "1CB3FE3A-C326-4720-9049-427F10E4A82F";
                    case 1177: return "12FA11B3-AAC2-498C-9505-42E6F288F743";
                    case 622: return "044261F6-53FD-4FC4-89D0-42EA37798658";
                    case 411: return "1C0CA977-0A39-46F9-9D5D-434751FCD632";
                    case 568: return "26601F7B-67CD-4CB6-8D6E-441C9A59B7D7";
                    case 1298: return "314EDAED-5A49-4FED-A9C0-4496C93C8F9E";
                    case 1089: return "5281789E-2F98-4B51-8BE1-451978EB5D6F";
                    case 5012: return "EAFF5ED0-1B74-4A2B-8D40-45669A4E53F6";
                    case 1052: return "AB61B442-FE3D-4B43-9FAC-457CA03F6FC5";
                    case 684: return "ACD846D6-D637-44FE-BC33-472A26C7E06F";
                    case 713: return "A90C10B5-13BF-4077-8A70-474384B8CCC0";
                    case 1013: return "2088BC5E-52D3-4058-AE28-47A3EEC57D27";
                    case 1286: return "CC4AD061-7E77-49CF-A680-47F13ACD5DE1";
                    case 187: return "B38F4F52-29DF-42A8-8290-48F676E4DD14";
                    case 5009: return "7C055C62-28AB-45D4-8F0C-495DEB055DBF";
                    case 657: return "5273A0AF-4B5A-4BA4-9EBA-49EAAAB04578";
                    case 740: return "ABEC3383-FA11-44CC-A9C0-4A4331CB3E7A";
                    case 1108: return "D68F174C-FD6C-471C-851F-4B80B0BF375B";
                    case 574: return "AA8845D5-24E2-4CD6-8B7B-4BEF3257FC7A";
                    case 1048: return "C53732F6-4CEB-4EAB-A1A1-4C2E2187DBD0";
                    case 5003: return "D91C157F-5D33-46A9-833D-4C91D363B8B1";
                    case 448: return "A6B3AAD1-6ABB-4DC5-9A16-4D6026E2AE68";
                    case 456: return "6B6D0C6B-7D16-4F99-893D-4EBF52CEAF18";
                    case 1100: return "183110DB-46EE-44E6-9322-4F52AAFE3CB5";
                    case 356: return "1EE08FD4-1E5B-4CE2-8285-50E334229553";
                    case 1039: return "0C81B95A-BA5C-4F35-9067-51FDEE50BA01";
                    case 631: return "57717FFC-1223-4850-9E63-531DDBDDD1DA";
                    case 646: return "690F3215-B4B8-497F-B3B5-537EA5034117";
                    case 1033: return "B84BC278-E719-47ED-B3C9-53CFC3B153B1";
                    case 687: return "EBECCDDE-E909-4165-BF03-54045C172B65";
                    case 69: return "0A3A74E3-A55E-4B64-AA5C-554ECC8BA9F8";
                    case 707: return "C84C11E8-7452-4B6F-98F4-559F92536BAF";
                    case 1284: return "F6E8A2D4-719B-4A0C-82B1-565D88EE81B8";
                    case 1248: return "8BFC82B6-3D29-47E1-B9B0-573886295119";
                    case 265: return "43212400-F349-4EC7-915C-57950337C030";
                    case 1150: return "455AE3B4-251E-41AD-940D-57C03E890349";
                    case 580: return "3EF8F365-E1E9-4D6B-8D1F-57F488E8364C";
                    case 1102: return "56310A07-2F40-4D4F-8FB4-5867BE791A59";
                    case 5043: return "75712B70-7EE9-4D12-89E8-59255575E9B6";
                    case 5041: return "97BB5E86-EFDB-4ABB-88E8-59D35039FABD";
                    case 696: return "7B49D5DA-ECF1-4C22-B15D-5AC913194FE9";
                    case 449: return "6BFB535C-7CA0-4797-9E5A-5AD737760B16";
                    case 445: return "C29B3419-B339-4C84-BC2A-5AF6D84B9DFB";
                    case 1012: return "D721A903-255C-49FB-867E-5C50E0416742";
                    case 1008: return "D3EA662D-AAD2-4793-9769-5C7864CDD3F4";
                    case 5016: return "9CCEF0CE-50E7-493D-AB20-5D96343A652D";
                    case 1125: return "907E588E-1D1D-4102-B7EA-5D9F10955AAB";
                    case 485: return "C9BB96E6-061B-4644-8509-5F9D88B4418C";
                    case 1124: return "EFF3F9A9-9B80-4127-AF2F-612F6AF27C57";
                    case 1172: return "AA8501D9-F5C2-40C6-B367-6198124E2D18";
                    case 703: return "D72687E6-3B9D-4D6D-954C-6201CDC922FC";
                    case 466: return "F06B22D4-D5B8-48DC-9CD4-6289E8C54C73";
                    case 1090: return "0372892C-8955-4E23-8513-62FDD9BCE380";
                    case 5044: return "4536447A-F2A6-4F50-896A-63AB402E0D4B";
                    case 732: return "0ED70DDC-A89F-4D38-BDFB-63E8435A9B9C";
                    case 595: return "EC44CF07-CE00-4229-AAAE-63ED0BE2D374";
                    case 1304: return "96126793-BE84-4111-A805-6415F71ACF94";
                    case 467: return "B0A909D5-2CC7-41CE-AD0A-645868743B8A";
                    case 5051: return "8A3BBB7C-38E1-465E-A19B-65A506D9F161";
                    case 5054: return "50A29F26-D9DC-4700-A419-6613DD38B2AB";
                    case 708: return "90A1B7D8-ABE3-467E-AD59-679FF1FFB9FC";
                    case 386: return "C7A468CA-F290-4DD3-B40A-68130326DA73";
                    case 1131: return "1AAEAA2D-5A09-4F04-B614-68FAEB7D70C5";
                    case 719: return "E5A539EF-11E3-43A1-B550-6959C85217F3";
                    case 680: return "4D13BB9B-AEBB-476E-9D0C-69B5B2318F89";
                    case 256: return "4F08615D-DA6E-4767-89D2-69DB377D1BC3";
                    case 5008: return "A83650AE-BF82-4186-A82D-6A69E44B7785";
                    case 1173: return "7AD707E6-33FA-43EE-A1E9-6BD805276619";
                    case 721: return "DE683163-3555-4425-8B39-6CB5666AF973";
                    case 1066: return "E1515D1F-5E66-428F-8CFB-6CBDE4CFFC2A";
                    case 1046: return "DF282E20-A793-4228-AAF5-6D6116EF3FB4";
                    case 695: return "D2BD9A68-A96F-4689-AC86-6D9EF8635DC7";
                    case 1254: return "ED8FC86D-B5D1-4770-BAAE-6DCD4D52113F";
                    case 1291: return "CF18AED5-50BA-42B0-A6A7-6DF0EC9438E1";
                    case 1310: return "52E4DF59-2477-4245-8B66-6F4E8057C74A";
                    case 537: return "50A7DD8B-7684-486F-8043-6FD57189BEE5";
                    case 5029: return "F28A9980-A93B-42FD-B62B-710B82B195B6";
                    case 1158: return "4DD45FEF-840A-406D-BF2D-715481F95A91";
                    case 1065: return "9FECEA22-FF71-45F5-AF31-715D1AB14738";
                    case 644: return "8411511C-7493-4A23-9991-71976E50F0DC";
                    case 491: return "F5E88D81-795D-40D3-86AF-7197CC7DA96B";
                    case 301: return "91C277CA-D8EE-4FEE-A55C-7287E477218B";
                    case 1071: return "7F55340F-5A0D-4941-9EDF-72ADEBAD57A3";
                    case 691: return "07F989DD-EC06-4D54-93A2-73099706FF29";
                    case 729: return "EEAF3765-A752-4EC3-BF9C-7398DD04F3AD";
                    case 1059: return "66927953-1071-4873-8441-73CC9DAF39C7";
                    case 1106: return "FEFEF37C-9E47-46CF-8F53-75930A6C539E";
                    case 1315: return "93D8DB61-B055-4420-9181-760A0A8CFC6E";
                    case 93: return "CF92AC8A-EECF-411B-ABC8-77180EC3C44D";
                    case 1195: return "1CEC0345-5E38-43A2-A9DB-77BB2E05F442";
                    case 5045: return "BA5751F1-4D41-4D6B-92EA-77FB9A43E5DD";
                    case 1259: return "16B9B863-19E1-4971-81FA-78B22035FF30";
                    case 5040: return "D84DF59F-90A9-4FA3-9B48-799B2325F556";
                    case 494: return "FDAEC353-B3F2-4E35-9A75-7A9D32171EE8";
                    case 639: return "A3AFB2C2-5098-4BF0-9322-7AFFF02C18B2";
                    case 587: return "FCE9456B-4390-4717-87D8-7B34CA04FF34";
                    case 1201: return "2D8ED68F-2318-4FAB-BA7E-7B648BD9403A";
                    case 1068: return "27AFA2FE-3F6B-4FA6-A967-7BBCEDE3E630";
                    case -1129: return "7489B737-8F91-46DE-AE43-7BEFB04BB84D";
                    case 1157: return "D7BC6349-AC4B-471E-AF48-7C43A7F5731F";
                    case 1020: return "EB6304AF-DC6A-4EAD-8CD4-7D56134294A6";
                    case 1045: return "6259D335-2836-4A36-B806-7E4515F0F9E6";
                    case 1180: return "2DFA3A0E-BE80-40BA-94BB-7EA19C76DEA5";
                    case 741: return "F487B868-797F-4824-AF49-7EA332EA8CD7";
                    case 201: return "C02074DD-ED68-4A07-8C79-7F8B24009F16";
                    case 1139: return "BC79AE32-34ED-4A0C-ADFF-7FD935B99A83";
                    case 315: return "FC767609-5F42-46E7-9C6B-805D7B14F805";
                    case 1212: return "914C4048-2C5C-4628-8D96-814D94BCF700";
                    case 1255: return "4F3B5094-AAF6-4F18-A01B-81D859E5F4FD";
                    case 5039: return "FDAF034D-B3E1-4DC0-81DA-81ECDAFFA756";
                    case 1093: return "C8007DBE-AE4D-413E-AF66-827025C45C54";
                    case -32768: return "87623499-BAA6-4F6A-A5D7-8283B92BB394";
                    case 1181: return "89A8A8B8-7439-468D-9532-8312090FE004";
                    case 387: return "85526712-7DE9-4227-B519-8395BA2D10B3";
                    case 305: return "D61D7DB0-4D50-4E23-A687-84104703A949";
                    case 1145: return "F842878D-E21C-415D-9EFD-845EC71A6D7F";
                    case 1287: return "22629A87-9A23-4644-A0A8-87151A669096";
                    case 1211: return "A55E08BA-D469-4484-9750-874E60AF4384";
                    case 513: return "64DD44E8-08D8-48FB-9410-87ADE67433E9";
                    case 569: return "D3D2710F-47B4-4963-9FF3-88B8CD2900F0";
                    case 1293: return "A4CB66D0-9A4D-442A-8ECB-89A356E53E4B";
                    case 1133: return "134CB59F-94D5-4DC0-BC7C-8A6EEB90600B";
                    case 1163: return "80B9B02E-4A94-4AD1-B6B7-8AA56F229E79";
                    case 5010: return "57A98E8C-856B-4F5D-BE1E-8B7BF59F19A9";
                    case 705: return "96894CBD-555D-4AA4-8C8E-8BB1143F6247";
                    case 410: return "E78A8FEB-187E-42E6-BEDC-8BE0FC1345FD";
                    case 608: return "D8C6B15B-489A-42CC-88A0-8D4B8F6E414E";
                    case 1096: return "C768C3B2-1A47-4486-9103-8D5FD8CFBA08";
                    case 1305: return "BE80E923-494B-448E-80F4-8E8196C3B382";
                    case 240: return "1DA10D7E-17E7-47B8-B994-8EA9036CB17E";
                    case 1232: return "7087B9F9-E3AE-4282-BBEE-8EE471E15362";
                    case 588: return "DD2F2BC1-EB98-40B5-BD07-8EF5EC242690";
                    case 5002: return "49BD2D81-6F02-4CDB-9E09-8F926B2E09FF";
                    case 52: return "4A7B5229-9AF2-45E9-82EC-8FB1C331ACCF";
                    case 1283: return "12844BEB-AFE3-4378-B9AA-8FCC1937EA70";
                    case 1263: return "F6DCA493-932F-4114-A9D8-9017FFCE8B62";
                    case 15: return "42A3BAAA-3FB4-460E-80EB-902A01AFF31F";
                    case 1186: return "1AD44955-D1FB-42D4-983C-9168C82D67C4";
                    case 658: return "92B7A759-92AE-49F0-B764-91A038A99EB2";
                    case 470: return "57BC64B3-00A8-4D18-AF7E-9349ABA6ADA0";
                    case 1165: return "A60B04B4-4773-4226-AA43-93C1F0EC10AD";
                    case 1206: return "538D3DD0-972A-4E8E-A4F8-942CA5FE018D";
                    case 450: return "DD17710E-4521-4AD1-8CBB-94B870DD8A9F";
                    case 642: return "DF864B58-F9DC-4EA8-9EA8-951529A4AC02";
                    case 1216: return "5C8AFB71-FCF9-40C3-849B-951EEB6D410C";
                    case 1311: return "6C1F6125-0DFC-4EA4-B651-96FD10DBF6B2";
                    case 6097: return "B43813A3-7E3A-44D5-B5AB-97325E13D5C3";
                    case 1307: return "7040CBA9-F1B3-4921-B7F8-974B04D1AFA3";
                    case 652: return "EAB8B442-8ADB-4F44-A0A4-983DFB9D7D9F";
                    case 0: return "EA331863-80EF-490E-B345-984AF0453DA9";
                    case 1296: return "ED203EC7-45CB-4FD0-BA27-9863665AC347";
                    case 618: return "46E4E399-F42F-4E0B-9DCC-9A4C84648C12";
                    case 316: return "3569A671-D969-426B-9E4E-9B4709A66FE1";
                    case 623: return "5C32C24F-A0E6-444A-96F6-9B9C8DEC3908";
                    case 1303: return "727073F7-C012-4A70-886A-9BA75B323FAB";
                    case 1135: return "65749F25-C16E-4EB3-9C31-9BC0AEF08E28";
                    case 320: return "576F6A71-1C59-490A-B67A-9BCEE000B929";
                    case 1197: return "42EB0294-8526-40E5-9B0B-9C54EA2C3359";
                    case -1186: return "DFC035E3-E51C-4A8E-BD14-9C9748F0F0AB";
                    case 678: return "EA111889-F455-4F35-8D4C-9DBCD09E0578";
                    case 1132: return "DE894944-1210-45B2-B886-9E3FBD38DC66";
                    case 1250: return "42FAAD21-3CB5-4FCB-80EF-9F5A199AEE79";
                    case 1257: return "8E57569B-0A73-487F-992E-9FC236CDF741";
                    case 504: return "185D0B0D-71BE-4394-998F-A09CC992A180";
                    case 1244: return "2EF21BDE-6CBB-46C9-B783-A130309B9DC2";
                    case 572: return "B92C88E5-665A-4F03-B1A7-A223E2CBAC63";
                    case 453: return "23213F2E-11B1-449C-AE05-A31CACB45613";
                    case 274: return "614BC3DA-4DE5-4CC1-B28E-A3DA02FB4431";
                    case 395: return "B1780D11-6BE3-423B-B41F-A4804E419D61";
                    case 1183: return "258D94B2-72E4-4BA1-A1D4-A4D1008D15C9";
                    case 1200: return "23701BDD-8179-47D9-BF20-A5061A347DFE";
                    case 1116: return "F165ABA8-3A68-4DA0-8F6D-A642E0E25F69";
                    case 1060: return "6B3631C1-3B3E-4095-B95F-A7C8FD32F385";
                    case 5033: return "391BA484-550B-45A0-9D85-A8B8F718C8AE";
                    case 1217: return "B1473C01-574C-4838-A84F-A9A6962A6D10";
                    case 486: return "2DAC0A9D-BE65-4B1E-8308-AA502F6A66AC";
                    case 673: return "D675B55B-8CB5-4054-97CB-AA50BB4A616A";
                    case 6125: return "6BB0627F-E065-428A-BF71-AA5445B09F07";
                    case 1314: return "BDA57373-024D-4EF8-B658-AB8D76467F6A";
                    case 645: return "525DD0FD-321C-4ECA-B103-AD0F7EC46270";
                    case 173: return "FC098D2E-D43F-49A2-898A-ADBFF8ACDFDB";
                    case 1203: return "674BAEAA-8D2C-49E0-9A38-AE572D441ED6";
                    case 198: return "DD99B2BB-7444-456E-9E71-AE9B4A379EA3";
                    case 1050: return "EEC8F239-C87F-4198-885F-AF31E7E7A885";
                    case 590: return "F7914C6A-F30E-40EA-9112-B0029F49BF66";
                    case 662: return "17EF003F-5057-412E-89B3-B238004D6E13";
                    case 424: return "9E4B12D3-41DF-4E2D-91F3-B30E50AB0002";
                    case 27: return "79C19F71-2964-45C1-A546-B345AC3977A3";
                    case 1112: return "088CC460-F289-4662-A3CB-B40450F61E59";
                    case 1258: return "264F7AC6-C558-4125-ABD4-B5C02051AC19";
                    case 607: return "DF0C6AB8-311E-45B2-9E37-B5D862EB3367";
                    case 1209: return "E70449CF-6830-445A-8217-B6DB7C4AEF45";
                    case 653: return "E2F4F987-C2B1-4609-95C9-B6FFE2C3CA27";
                    case 521: return "0ECF1874-2ECB-48DD-A511-B759E9D09DD4";
                    case 1027: return "66E654F8-75F8-4B3A-8DA6-B985E2C497EB";
                    case 1256: return "3C5CE523-0C9D-46B5-874C-BA7C59CB68A5";
                    case 1026: return "353AB597-51BF-407F-AF50-BBB22035852F";
                    case 709: return "C3E200E1-5121-41E3-A6FB-BBDCB1D37E73";
                    case 1297: return "19BEF615-7A18-42FB-97EC-BC72F8FF0C3B";
                    case 516: return "DA36BB4B-8138-4ECD-8B9D-BCBDC409CB7D";
                    case 1101: return "C74316CE-1C9C-4343-BBCD-BCFADB1AF748";
                    case 1025: return "5D98AEBB-42E6-4A01-A181-BD5642ABB2A4";
                    case 1153: return "711EE9F2-59DC-4623-8AA8-BF2B8A0EAABC";
                    case 383: return "345C3044-6701-4FE7-9F37-BF757FF28984";
                    case 1142: return "A20B8F41-EF59-4401-9871-BFAB79CDB080";
                    case 1261: return "674B65FB-46EF-41EC-B88C-C04FAEB51685";
                    case 1266: return "43AC15B7-95B9-4EC7-9CC0-C0D6388348A8";
                    case 737: return "CECBB1CD-A965-42DC-9276-C2A6994C2943";
                    case 1238: return "4B88DDE8-786D-430E-AE39-C3191F79F01E";
                    case 654: return "17482CE7-2DE1-4E1E-A248-C454DE908B32";
                    case 676: return "6275DED8-5CF7-4E75-B6CA-C4B55B7BF8DD";
                    case 1279: return "5D45C2F1-3A96-491F-98D4-C4F3E383C92B";
                    case 1129: return "9FC285A4-3C56-4619-A2BB-C60F069448AD";
                    case 394: return "4E78C1EE-058C-4EEB-9C13-C63053EE2DFB";
                    case 351: return "EB37A8B4-2C58-4AFA-8A05-C655639094F1";
                    case 1171: return "E71D44F5-B845-4A02-B23D-C6813584E3EE";
                    case 1003: return "CE5D4707-5F88-4CC7-9BEE-C69B29AB13FD";
                    case 655: return "88A58CB7-1EDC-48CB-BFE3-C7B07203942F";
                    case 1117: return "4637BF1C-7B0B-48F3-9475-C7D7B4C147E6";
                    case 1146: return "72773CA2-5E2F-4777-BACC-C853751FC504";
                    case 1292: return "2964EA5A-4DAD-47A7-ABA3-C86E4B1B8066";
                    case 1219: return "7804A7A0-501C-40DB-A988-C8D014D85C13";
                    case 602: return "BC6CAB04-013C-4550-A214-C967BBF1D0C1";
                    case 1271: return "3A06D713-7D94-49D1-A510-C9ECB6ADC1B8";
                    case 1278: return "B145A68A-B466-46E9-B651-CB13C569A0C1";
                    case 1168: return "B7190928-FEA0-4501-80C6-CBE4101D8B67";
                    case -1232: return "931A3474-5990-4F96-AA9F-CFE302E057A1";
                    case 5023: return "F46DF254-1F75-4F59-A1CD-D04A168ABE38";
                    case 477: return "704ACDE6-7B91-4293-9471-D1676663A578";
                    case 6238: return "4120F30B-31BA-4DA9-9280-D1E74C1A3674";
                    case 5005: return "E99DDB3A-B56D-4EAD-A2F3-D22454BE7C5F";
                    case 157: return "B46E24F3-0E52-4ADB-8A79-D293D5D5EFF4";
                    case 1270: return "FF011072-793C-477F-BEEE-D2ADD0FDE8E5";
                    case 6138: return "814BC15C-1FB2-42DD-BF5C-D3120839AD18";
                    case 476: return "7D00CA5A-9112-4E19-99E6-D32770A7EA93";
                    case 1276: return "8C457A60-0D07-49C6-985F-D33B6574E94E";
                    case 1221: return "ED8E7CE6-8CCD-4EDD-BAEA-D42EF2358D43";
                    case 1267: return "7C724F80-0B4B-4C66-B608-D47B3E8813CA";
                    case 1109: return "2EAF8E41-F166-4800-B440-D507318681FE";
                    case 5035: return "4868DC5D-1E7A-4B2F-8330-D52D9B816A1C";
                    case 1253: return "81FF2C25-E97F-481A-AADA-D534EB8BD93E";
                    case 1295: return "E5FD94BF-1A75-4980-AA7A-D71D42625838";
                    case 1194: return "C1C2AE10-F5A1-4393-8FFD-D7298DEC012E";
                    case 489: return "AA6A8BCF-F76B-44D7-91A4-D7877AFAD4F9";
                    case 710: return "9E6CCF10-0236-4074-8794-D7C5E085FD82";
                    case 446: return "64771132-0104-4B8F-B39C-D7CD404D206C";
                    case 1110: return "143162A4-EE54-4A98-A8BE-D92745E97914";
                    case 1196: return "BB1AF8F3-4B7C-402F-B095-D9DAEBCD2E1F";
                    case 1119: return "5FE8C269-F39B-4484-8053-DA413BDA840E";
                    case 5046: return "3984CAFF-01E9-4768-A3E5-DA7B2297346E";
                    case 715: return "0B554D6C-F372-4486-B747-DA8166624E2C";
                    case 546: return "58EE4C33-F59B-4BA0-88BD-DC8E3F8E6C05";
                    case 5053: return "F290ABF5-3C8F-4F55-A108-DD3A04AF0A2A";
                    case 621: return "2BD10079-5200-46CC-A5EC-DD42779C540B";
                    case 5004: return "08674755-80AF-4489-B5B8-DD79D600CD83";
                    case 1280: return "431D1C54-D873-4CDE-B5F3-DDB2A2CDC885";
                    case 734: return "BFB1947A-6C10-4BB1-801D-DDE530A27F4F";
                    case 1277: return "04BA0FFE-59B4-41C4-80E1-E21BD254E0E5";
                    case 365: return "B4D3FF44-1C99-445E-B4D8-E28E1D969029";
                    case 1193: return "E147FAE9-118B-48E1-A3DA-E2C4372E7964";
                    case 1294: return "C0D0B78A-35BD-4136-8230-E326A0CE8735";
                    case 1148: return "DA001CBA-ECE9-4762-82ED-E34435F9B70C";
                    case 693: return "040EC982-294D-43A1-B269-E40147B3669B";
                    case 5006: return "AB69DA0E-5CDD-40BF-8AA2-E40A1DA056AE";
                    case 560: return "5DB6AE05-20AD-4173-B825-E412CD75A974";
                    case 1130: return "E63F8D4E-C247-4C91-9F8A-E43434E3B2A1";
                    case 616: return "0B27D44B-30C6-4108-8B9C-E66286DF206E";
                    case 266: return "D2FF2380-B122-4833-8868-E6861F70C649";
                    case 738: return "04798543-C28D-4925-85B6-E69F16B66B3B";
                    case 415: return "992930E7-1455-4F44-A7D0-E6D2D63CCD2F";
                    case 1289: return "82901FFD-C348-4890-9A3D-E7C934143113";
                    case 5014: return "8535E173-27D2-4A28-9CA3-E8E2C3B259E5";
                    case 332: return "607A286C-9CE0-4308-A13D-E9632E893E5E";
                    case 327: return "6A1DCECC-6D9E-4D1E-8FF3-EA4CB6F17DD3";
                    case 223: return "B9665814-9B6B-4669-9676-EAB4235401EC";
                    case 1260: return "A3F4BA65-555B-43EF-A88B-EC77248E54C2";
                    case 1202: return "0531FA38-2552-41B4-A3E7-ECD5707ADB96";
                    case 422: return "D71EEDC2-C344-49F4-AC51-ECDEC239A935";
                    case 1309: return "A029960F-D6F7-4831-B1D8-ED69E30D9F58";
                    case 5021: return "67369B60-FFFB-4D0F-9261-ED85392F4A9E";
                    case 1243: return "5523D491-CC82-44AA-A075-EE881B324914";
                    case 1005: return "20ED5CD2-7AD4-436B-8C1C-EE93B00F21CD";
                    case 679: return "8E986645-31C6-4B78-B978-EF5C8E0F2A93";
                    case 1308: return "9FE80E80-ABF7-4CBA-B0C0-F025813D560D";
                    case 345: return "26387C26-590E-42AD-92F8-F09B9914513E";
                    case 1281: return "6660E858-6F37-49F9-9D95-F168248424D6";
                    case 1164: return "14206AE3-FE7E-4B5D-A41B-F341D84AC6B8";
                    case 279: return "FB6C6576-14F7-49E3-8B2A-F36471285281";
                    case 349: return "11BA716E-C0C2-4E5A-BD72-F44113431532";
                    case 722: return "B4971FC5-D45C-4ADF-B02B-F450DDC05070";
                    case 6111: return "63D51D43-38B3-4909-B3B5-F457EF44FFCD";
                    case 1306: return "5B64054D-DEAE-4CAA-8444-F4ED0D5A2FC1";
                    case 1210: return "05B90C1E-AC43-4F9C-8DB7-F569EE51DBAE";
                    case 596: return "01DAD9DC-297D-46D2-92F5-F5E6C496A4E6";
                    case 5050: return "88932999-3B52-4E53-8D4C-F689F50DBDBF";
                    case 5001: return "2EE209FF-182A-4C3E-B205-F7793C2E19AB";
                    case 5052: return "C323BA49-1645-4BFB-A6F7-F833AA661B65";
                    case 735: return "F44982CE-1825-46FA-8FA1-F85DA97C04BB";
                    case 573: return "636720EC-4703-48FA-98E7-F8AC0E9BC9A0";
                    case 716: return "98EBC024-7557-4050-A0DF-F8B62469AA26";
                    case 6143: return "1B0C4BE0-317A-416E-80F6-F98889F516E0";
                    case 67: return "ED8F9EAB-C418-4D79-892B-F99B22415D78";
                    case 1087: return "E8506CB5-4656-4161-9C45-F9CB789C23CB";
                    case 441: return "ADEFA969-A9FE-4FA0-8941-FA699601322F";
                    case 1154: return "10801CE3-8953-409F-9D41-FAC3D631B4A9";
                    case 1162: return "55600560-E713-4029-B0CD-FBC80D58E522";
                    case 742: return "68AC0346-EE5C-4D28-899D-FBFA6BBC3DB2";
                    case 699: return "8CDAD371-D5DA-4D1D-8521-FC04D9D82FCC";
                    case 688: return "01AFC32E-EABE-489A-9E9D-FDABB2E3DE3B";
                    case 1149: return "BF06ABB3-BC38-4263-95F2-FDD39DC821E5";
                    case 487: return "EFD470FE-C9E7-4DAC-91F7-FE3D28A9B1DB";
                    case 1233: return "CFCF60C1-49ED-4112-92DC-FE9A7D2225A3";
                    case 1190: return "4E181559-1DA8-49A7-B573-FEE89FD6EA71";
                    case 6217: return "7F7FCDD2-F4EC-4F12-AC0D-FEF0A6B6DECB";
                    case 390: return "1943E17D-F2C6-4562-8232-FF74EF9FC177";

                    default:

                        if (!unknonwStaffIds.ContainsKey(oldID.ToString()))
                            unknonwStaffIds.Add(oldID.ToString(), oldID.ToString());

                        return "NA" + "-" + oldID;
                }
            }
        }



        private Gender GetGender(string value)
        {
            if (string.IsNullOrEmpty(value))
                return Gender.UndefinedGender;

            switch (value.ToLower().Substring(0, 1))
            {
                case "m":
                    return Gender.Male;
                case "w":
                case "f":
                    return Gender.Female;

                default:
                    return Gender.UndefinedGender;
            }
        }

        private (string Familyname, string Givenname) GetName(string name)
        {
            var names = name.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();

            while (names.Count < 2)
                names.Add("- - -");

            return (names[0], string.Join(" ", names.Skip(1)));

        }

        private string GetId(int id) => $"{id}";

        private (string Postcode, string City) GetPostCodeCity(Model.AdresseDTO a)
        {

            var plz = (a.Postleitzahl ?? string.Empty).Trim();
            var ort = (a.Ort ?? string.Empty).Trim();

            if (plz.Length > 4)
            {
                // im Datenbestand gibt es "gebastelte" Postleitzahlen mit 6 Zeichen
                // z.b. 690060 Möggers
                // entweder nur die ersten vier Zeichen verwenden, oder die PLZ anhand des Ortsnames ermitteln

                if (Postcode_CityProvider.Instance.IsValid($"{plz.Substring(0, 4)} {ort}"))
                {
                    plz = plz.Substring(0, 4);
                }
                else
                {
                    var plz2 = Postcode_CityProvider.Instance.GetCSV().Where(x => x.EndsWith($" {ort};")).FirstOrDefault();
                    if (!string.IsNullOrEmpty(plz2))
                        plz = plz2;
                }
            }

            return (plz, ort);
        }
    }
}
