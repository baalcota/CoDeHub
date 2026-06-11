using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using Eto.Forms;

using Rhino;
using Rhino.Commands;
using Rhino.Input;
using Rhino.Input.Custom;

namespace CoDeHub
{
    public class CoDeHubSync : Rhino.Commands.Command
    {
        public CoDeHubSync()
        {
            Instance = this;
        }
        public static CoDeHubSync Instance { get; private set; }

        public override string EnglishName => "CoDeHubSync";

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            // CoDeHub Folder Directory
            string tyrDir = "O:\\STH\\923096\\CoDe Hub\\Snippets";

            try
            {
                // Get User Local folder (user objects). Independent of Rhino version (7.0, 8.0, etc...)
                string rhinoDir = Rhino.RhinoApp.GetDataDirectory(true, false);
                string[] list_path = rhinoDir.Split(new string[] { "McNeel" }, StringSplitOptions.None);
                string localDir = list_path[0] + "Grasshopper\\UserObjects";

                // Filter .ghuser files
                string[] ghuser_files = Directory.GetFiles(tyrDir, "*.ghuser");
                
                if (ghuser_files.Length > 0)
                {
                    foreach (string ghu_file in ghuser_files)
                    {
                        // Get User Object file Name
                        string fName = Path.GetFileName(ghu_file);
                        try
                        {
                            // Copy file from TyrDir --> LocalDir (overwrites identical files!)
                            File.Copy(Path.Combine(tyrDir, fName), Path.Combine(localDir, fName), true);
                            Rhino.RhinoApp.WriteLine(fName+" downloaded successfully");
                        }
                        catch
                        {
                            return Result.CancelModelessDialog;
                        }
                    }

                    return Result.Success;
                }
                else 
                {
                    Rhino.RhinoApp.WriteLine("No User Object were found in CoDe Hub");
                    return Result.CancelModelessDialog; 
                }
            }
            catch
            {
                Rhino.RhinoApp.WriteLine("Unsuccessful Sync, contact CoDe Hub");
                return Result.CancelModelessDialog;
            }
        }
    }
}
