using System;
using System.IO;

namespace Vodamep.Client
{
    public abstract class HandlerBase
    {

        public void Send(SendArgs args)
        {
            string[] files = GetFiles(args.File);

            foreach (var file in files)
            {
                this.Send(args, file);
            }
        }

        public void Validate(ValidateArgs args)
        {
            string[] files = GetFiles(args.File);

            foreach (var file in files)
            {
                this.Validate(args, file);
            }
        }



        /// <summary>
        /// Liefert die Dateien anhand der übergebenen Argumente
        /// - Wildcard * möglich
        /// - Oder einzelnes File
        /// </summary>
        protected string[] GetFiles(string filePattern)
        {
            var wildcard = filePattern.IndexOf("*");

            string[] files;

            if (wildcard >= 0)
            {
                if (wildcard == 0)
                {
                    files = Directory.GetFiles(Directory.GetCurrentDirectory(), filePattern);
                }
                else
                {
                    var dirIndex = filePattern.Substring(0, wildcard).LastIndexOf(@"\");
                    var dir = filePattern.Substring(0, dirIndex);
                    var pattern = filePattern.Substring(dirIndex + 1);
                    files = Directory.GetFiles(dir, pattern);
                }
            }
            else
            {
                files = new[] { filePattern };
            }

            return files;
        }


        public abstract void PackFile(PackFileArgs args);

        public abstract void PackRandom(PackRandomArgs args);

        protected abstract void Send(SendArgs args, string file);

        protected abstract void Validate(ValidateArgs args, string file);

        public virtual void ValidatePrevious(ValidatePreviousArgs args)
        {
            HandleFailure("Für dieses Modul nicht verfügbar.");
        }
        
        protected void HandleFailure(string message = null)
        {
            throw new Exception(message);
        }
    }
}
