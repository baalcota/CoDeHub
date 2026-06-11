using Eto.Forms;
using Rhino;
using Rhino.Commands;
using Rhino.UI;
using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace CoDeHub
{
    public class CoDeHubUpload : Rhino.Commands.Command
    {
        public CoDeHubUpload()
        {
            Instance = this;
        }

        ///<summary>The only instance of this command.</summary>
        public static CoDeHubUpload Instance { get; private set; }

        public override string EnglishName => "CoDeHubUpload";

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            //Eto.Forms.SaveFileDialog saveFileDialog = new Eto.Forms.SaveFileDialog();
            //saveFileDialog.CurrentFilter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            //saveFileDialog.Title = "Save Your File";

            // CoDeHub Folder Directory
            string tyrDir = "O:\\STH\\923096\\CoDe Hub\\Snippets";

            // Get User Local folder (user objects). Independent of Rhino version (7.0, 8.0, etc...)
            string rhinoDir = Rhino.RhinoApp.GetDataDirectory(true, false);
            string[] list_path = rhinoDir.Split(new string[] { "McNeel" }, StringSplitOptions.None);
            string localDir = list_path[0] + "Grasshopper\\UserObjects";

            var openDialog = new Eto.Forms.OpenFileDialog()
            {
                Directory = new Uri(localDir),
                Filters = { new FileFilter("Grasshopper User File (*.ghuser)", "*.ghuser") },
                CurrentFilterIndex = 0,
                CheckFileExists = true,
                MultiSelect = true,
                Title = "Select UserObjects to upload"
            };

            var result = openDialog.ShowDialog(RhinoEtoApp.MainWindow);
            
            if (result == DialogResult.Ok)
            {
                // Get User Object files in local folder
                foreach (string ghu_file in openDialog.Filenames) 
                {
                    try
                    {
                        string fName = Path.GetFileName(ghu_file);

                        // Copy file from LocalDir --> TyrDir (overwrites identical files!)
                        File.Copy(Path.Combine(localDir, fName), Path.Combine(tyrDir, fName), true);
                        Rhino.RhinoApp.WriteLine(fName + " uploaded successfully");
                    }
                    catch { return Result.CancelModelessDialog; }
                }

            }

            return Result.Success;
        }
    }
}