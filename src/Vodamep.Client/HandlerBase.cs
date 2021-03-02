using System;
using System.IO;

namespace Vodamep.Client
{
    public abstract class HandlerBase
    {

        public void Send(SendArgs args)
        {
            var wildcard = args.File.IndexOf("*");

            string[] files;

            if (wildcard >= 0)
            {
                if (wildcard == 0)
                {
                    files = Directory.GetFiles(Directory.GetCurrentDirectory(), args.File);
                }
                else
                {
                    var dirIndex = args.File.Substring(0, wildcard).LastIndexOf(@"\");
                    var dir = args.File.Substring(0, dirIndex);
                    var pattern = args.File.Substring(dirIndex + 1);
                    files = Directory.GetFiles(dir, pattern);
                }
            }
            else
            {
                files = new[] { args.File };
            }


            foreach (var file in files)
            {
                this.Send(args, file);
            }
        }

        public void Validate(ValidateArgs args)
        {
            var wildcard = args.File.IndexOf("*");

            string[] files;

            if (wildcard >= 0)
            {
                if (wildcard == 0)
                {
                    files = Directory.GetFiles(Directory.GetCurrentDirectory(), args.File);
                }
                else
                {
                    var dirIndex = args.File.Substring(0, wildcard).LastIndexOf(@"\");
                    var dir = args.File.Substring(0, dirIndex);
                    var pattern = args.File.Substring(dirIndex + 1);
                    files = Directory.GetFiles(dir, pattern);
                }
            }
            else
            {
                files = new[] { args.File };
            }

            foreach (var file in files)
            {
                this.Validate(args, file);
            }
        }

        public abstract void PackFile(PackFileArgs args);

        public abstract void PackRandom(PackRandomArgs args);

        protected abstract void Send(SendArgs args, string file);

        protected abstract void Validate(ValidateArgs args, string file);

        protected void HandleFailure(string message = null)
        {
            throw new Exception(message);
        }
    }
}
