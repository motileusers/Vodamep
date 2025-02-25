﻿using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Vodamep.StatLp.Model;

namespace Vodamep.StatLp
{
    internal class StatLpReportSerializer
    {
        public StatLpReport Deserialize(Stream stream)
        {
            using (var ms = new MemoryStream())
            {
                stream.CopyTo(ms);

                return Deserialize(ms.ToArray());
            }
        }
        public StatLpReport Deserialize(byte[] data)
        {

            if (IsPkZipCompressedData(data))
            {
                using (var ms = new MemoryStream(data))
                using (var archive = new ZipArchive(ms))
                {
                    using (var ms2 = new MemoryStream())
                    {
                        archive.Entries.First().Open().CopyTo(ms2);
                        data = ms2.ToArray();
                    };
                }
            }

            var isJson = System.Text.Encoding.UTF8.GetString(data.Take(10).ToArray()).TrimStart().StartsWith("{");

            StatLpReport r;

            if (isJson)
            {
                var json = System.Text.Encoding.UTF8.GetString(data);

                r = StatLpReport.Parser.ParseJson(json);

            }
            else
            {
                r = StatLpReport.Parser.ParseFrom(data);
            }

            return r;

        }

        public StatLpReport DeserializeFile(string file)
        {
            if (!File.Exists(file))
                return null;

            return Deserialize(File.ReadAllBytes(file));
        }

        public void WriteToFile(StatLpReport report, string filename, bool asJson, bool compressed = true)
        {
            using (var ms = WriteToStream(report, asJson, compressed))
            {
                ms.Position = 0;
                using (var s = File.Create(filename))
                {
                    ms.CopyTo(s);
                }
            }
        }

        public string WriteToPath(StatLpReport report, string path, bool asJson, bool compressed = true)
        {
            var filename = Path.Combine(path, new StatLpReportFilenameHandler().GetFileName(report, asJson, compressed));

            WriteToFile(report, filename, asJson, compressed);
            return filename;
        }

        public MemoryStream WriteToStream(StatLpReport report, bool asJson, bool compressed = true)
        {
            report = report.AsSorted();
            var ms = new MemoryStream();
            var result = ms;

            if (asJson)
            {
                var ss = new StreamWriter(ms);
                Google.Protobuf.JsonFormatter.Default.WriteValue(ss, report);
                ss.Flush();
                ms.Position = 0;
            }
            else
            {
                Google.Protobuf.MessageExtensions.WriteTo(report, ms);
                ms.Position = 0;
            }

            if (compressed)
            {
                var filename = new StatLpReportFilenameHandler().GetFileName(report, asJson, false);
                result = ZipStream(ms, filename);
                ms.Dispose();
            }

            result.Position = 0;
            return result;
        }

        private MemoryStream ZipStream(Stream data, string filename)
        {
            var ms = new MemoryStream();
            using (ZipArchive arch = new ZipArchive(ms, ZipArchiveMode.Create, leaveOpen: true))
            {
                var zipEntry = arch.CreateEntry(filename);
                using (var zipStream = zipEntry.Open())
                {
                    data.CopyTo(zipStream);
                }
                return ms;
            }
        }


        private const int ZIP_LEAD_BYTES = 0x04034b50;

        private bool IsPkZipCompressedData(byte[] data)
        {
            // if the first 4 bytes of the array are the ZIP signature then it is compressed data
            return (BitConverter.ToInt32(data, 0) == ZIP_LEAD_BYTES);
        }
    }
}