using Microsoft.VisualBasic.FileIO;
using SoulsFormats;

/*
 * TODO
    * implement partial/full installation
        * PTDE: check if game is unpacked
    * DSR FMG: copy to normal file -AND- _(2)/_ (2) files
    * partial installation: don't include non-event soapstone messages that have special effects (tarkus. is that it?)
 * TESTING
    * make sure FMG entries arent being duped anymore
    * full version DSR
    * dsr installation
    * ptde installation
 * NOTES
    * My usage of Find/FindAll instead of just making dictionaries is truly some psycho shit. but it works.
    * DSR cat AI implementation differs from PTDE. It uses common shared AI, and gave them new npcThinkIds
 */

namespace rando_script
{
    public partial class MainForm : Form
    {

        private string? installFolder = "";
        private bool is_DSR = false;
        private string DCXstring = "";

        public MainForm()
        {
            InitializeComponent();
            b_install.Enabled = false;
            b_restoreBackups.Enabled = false;
            Text += GetVersion();
        }

        private static string GetVersion()
        {
            string version = Application.ProductVersion;
            version = string.Format(" {0}", version);
            return version;
        }

        private void b_install_Click(object sender, EventArgs e)
        {
            if (cb_SimpleInstall.Checked)
                InstallPartial();
            else
                InstallFull();

            GC.Collect();

            System.Media.SystemSounds.Exclamation.Play(); //make noise
            MessageBox.Show("All done!", "Finished", MessageBoxButtons.OK);
        }

        private bool CheckFileExists(string filePath)
        {
            if (File.Exists(filePath) == false)
            {
                if (is_DSR)
                    MessageBox.Show($"Could not find \"{filePath}\", DSR installation has been cancelled." +
                        $"\nTry reinstalling the game.",
                        "Error", MessageBoxButtons.OK);
                else
                    MessageBox.Show($"Could not find \"{filePath}\", PTDE installation has been cancelled." +
                        $"\nPTDE must be unpacked before it can be modded, see installation instructions.",
                        "Error", MessageBoxButtons.OK);
                return false;
            }
            return true;
        }

        private void InstallFull()
        {
            //todo: just 
            var localstr = "DATA\\PTDE";
            if (is_DSR)
            {
                localstr = "DATA\\DSR";
            }

            string[] sourcePathList = Directory.GetFiles(localstr, "*.*", System.IO.SearchOption.AllDirectories);

            foreach (var path in sourcePathList)
            {
                var targetPath = $"{installFolder}\\{path}";
                targetPath = targetPath.Replace(localstr, "");
                if (!CheckFileExists(targetPath))
                    return;
                Create_Backup(targetPath);
                File.Move(path, targetPath, true);
            }

            //non english FMGs
            /*
            if (cb_ModifyAllFMG.Checked)
            {
                foreach (var targetPath in Directory.GetDirectories(installFolder + "\\msg\\"))
                {
                    if (!CheckFileExists(targetPath))
                        return;
                    Create_Backup(targetPath);
                    File.Move($"{localstr}\msg", targetPath);
                }
            }
            */
        }
        private void InstallPartial()
        {
            //var result = MessageBox.Show("Mod compatibility Info", $"To work with other mods, this needs to be installed last.", MessageBoxButtons.OKCancel);

            #region FMG
            List<string> fmgList = new();

            if (cb_ModifyAllFMG.Checked)
                fmgList = Directory.GetDirectories(installFolder + "\\msg\\").ToList();
            else
                fmgList.Add(installFolder + "\\msg\\ENGLISH\\");

            foreach (string path in fmgList)
            {
                FMG fmgBloodstainOld;
                BinderFile bloodFile;
                List<BinderFile> bloodFiles = new(); //for DSR dupe fmgs

                BND3? fmgBNDNew;
                string fmgPath_new;
                if (is_DSR)
                {
                    //DSR
                    var fmgPath_old = "DATA\\DSR\\msg\\ENGLISH\\menu.msgbnd.dcx";
                    if (!CheckFileExists(fmgPath_old))
                        return;
                    var fmgBNDOld = BND3.Read(fmgPath_old);
                    fmgBloodstainOld = FMG.Read(fmgBNDOld.Files.Find(e => e.Name.Contains("Blood_writing_.fmg")).Bytes); //DSR bloodstain messages
                    //fmgEventOld = FMG.Read(fmgBNDOld.Files.Find(e => e.Name.Contains("Event_text.fmg")).Bytes); //DSR event messages

                    fmgPath_new = path + "menu.msgbnd" + DCXstring;
                    if (!CheckFileExists(fmgPath_new))
                        return;
                    fmgBNDNew = BND3.Read(fmgPath_new);
                    bloodFiles = fmgBNDNew.Files.FindAll(e => e.Name.Contains("Blood_writing_.fmg"));
                    bloodFile = bloodFiles[0];
                    //eventFile = fmgBNDNew.Files.Find(e => e.Name.Contains("Event_text.fmg"));
                }
                else
                {
                    //PTDE
                    var fmgPath_old = "DATA\\PTDE\\msg\\ENGLISH\\menu.msgbnd";
                    if (!CheckFileExists(fmgPath_old))
                        return;
                    var fmgBNDOld = BND3.Read(fmgPath_old);
                    fmgBloodstainOld = FMG.Read(fmgBNDOld.Files.Find(e => e.Name.Contains("血文字.fmg")).Bytes); //PTDE bloodstain messages
                    //fmgEventOld = FMG.Read(fmgBNDOld.Files.Find(e => e.Name.Contains("イベントテキスト.fmg")).Bytes); //PTDE event messages

                    fmgPath_new = path + "menu.msgbnd" + DCXstring;
                    if (!CheckFileExists(fmgPath_new))
                        return;
                    fmgBNDNew = BND3.Read(fmgPath_new);
                    bloodFile = fmgBNDNew.Files.Find(e => e.Name.Contains("血文字.fmg"));
                    //eventFile = fmgBNDNew.Files.Find(e => e.Name.Contains("イベントテキスト.fmg"));
                }

                Create_Backup(fmgPath_new);

                var fmgBloodstainNew = FMG.Read(bloodFile.Bytes);
                //bloodstain messages:
                Dictionary<int, FMG.Entry> bloodstainDictOld = new();
                Dictionary<int, FMG.Entry> bloodstainDictNew = new();
                foreach (var entry in fmgBloodstainOld.Entries)
                {
                    bloodstainDictOld.Add(entry.ID, entry);
                }
                foreach (var entry in fmgBloodstainNew.Entries)
                {
                    bloodstainDictNew.Add(entry.ID, entry);
                }

                if (bloodstainDictOld[8100].Text == bloodstainDictNew[8100].Text)
                {
                    //tutorial message matches, mod is probably already installed
                    MessageBox.Show("Community Message Mod already seems to be installed." +
                        "\nIf this is not the case, try restoring backups first. Otherwise, reinstall DS1.",
                        "Error", MessageBoxButtons.OK);
                    return;
                }
                else if (bloodstainDictNew.ContainsKey(20000))
                {
                    MessageBox.Show("Targeted text files cannot be merged with. Message FMG ID 20000 is already occupied" +
                        "\nReport this error message and whichever mods you have installed so it can be fixed.",
                        "Mod Compatibility Error", MessageBoxButtons.OK);
                    return;
                }

                //tutorial messages (8100-8103), (9000-9099)
                fmgBloodstainNew.Entries.RemoveAll(e => e.ID >= 8100 && e.ID <= 8103);
                fmgBloodstainNew.Entries.AddRange(fmgBloodstainOld.Entries.FindAll(e => e.ID >= 8100 && e.ID <= 8103));
                fmgBloodstainNew.Entries.RemoveAll(e => e.ID >= 9000 && e.ID <= 9099);
                fmgBloodstainNew.Entries.AddRange(fmgBloodstainOld.Entries.FindAll(e => e.ID >= 9000 && e.ID <= 9099));
                //community messages (20000-20999)
                fmgBloodstainNew.Entries.RemoveAll(e => e.ID >= 20000 && e.ID <= 20999);
                fmgBloodstainNew.Entries.AddRange(fmgBloodstainOld.Entries.FindAll(e => e.ID >= 20000 && e.ID <= 20999));

                /*
                //tutorial messages (8100-8103), (9000-9099)
                if (fmgBloodstainOld.Entries.Find(e => e.ID == 8100).Text != fmgBloodstainNew.Entries.Find(e => e.ID == 8100).Text)
                { 
                    fmgBloodstainNew.Entries.AddRange(fmgBloodstainOld.Entries.FindAll(e => e.ID >= 8100 && e.ID <= 8103));
                }
                if (fmgBloodstainOld.Entries.Find(e => e.ID == 9000).Text != fmgBloodstainNew.Entries.Find(e => e.ID == 9000).Text)
                {
                    fmgBloodstainNew.Entries.AddRange(fmgBloodstainOld.Entries.FindAll(e => e.ID >= 9000 && e.ID <= 9099));
                }
                    //community messages (20000-20999)
                if (fmgBloodstainOld.Entries.Find(e => e.ID == 20000).Text != fmgBloodstainNew.Entries.Find(e => e.ID == 20000).Text)
                {
                    fmgBloodstainNew.Entries.AddRange(fmgBloodstainOld.Entries.FindAll(e => e.ID >= 20000 && e.ID <= 20999));
                }
                */

                bloodFile.Bytes = fmgBloodstainNew.Write();

                if (is_DSR)
                {
                    //copy to dupe FMG
                    bloodFiles[1].Bytes = bloodFiles[0].Bytes;
                }

                fmgBNDNew.Write(fmgPath_new);
            }
            #endregion

            #region MSB
            List<string> msbList_old = new();
            if (is_DSR)
                Directory.GetFiles("DATA\\DSR\\Map\\MapStudio").ToList();
            else
                Directory.GetFiles("DATA\\PTDE\\Map\\MapStudio").ToList();

            //loop through MSBs
            foreach (string path in msbList_old)
            {
                string msbName = Path.GetFileName(path);
                string msbPath_target = installFolder + "\\map\\MapStudio\\" + msbName;

                if (!CheckFileExists(msbPath_target))
                    return;
                Create_Backup(msbPath_target);

                var msb_old = MSB1.Read(path);
                var msb_new = MSB1.Read(msbPath_target);
                if (msb_old == null || msb_new == null)
                    throw new Exception("Could not find MSB map file");

                //copy over stuff
                List <MSB1.Region> newRegions = msb_old.Regions.Regions.FindAll(e => e.Name.Contains("message"));
                foreach (MSB1.Region n in newRegions)
                {
                    var check = msb_new.Regions.Regions.Find(e => e.Name == n.Name);
                    if (check == null)
                        msb_new.Regions.Regions.Add(n); //add to list
                    else
                        check = n; //update
                }

                var newMessages = msb_old.Events.Messages.FindAll(e => e.Name.Contains("message"));
                foreach (MSB1.Event.Message n in newMessages)
                {
                    var check = msb_new.Events.Messages.Find(e => e.Name == n.Name);
                    if (check == null)
                        msb_new.Events.Messages.Add(n); //add to list
                    else
                        check = n; //update
                }

                /*
                var newSFX = msb_old.Events.SFX.FindAll(e => e.Name.Contains("message"));
                foreach (MSB1.Event.SFX n in newSFX)
                {
                    var check = msb_new.Events.SFX.Find(e => e.Name == n.Name);
                    if (check == null)
                        msb_new.Events.SFX.Add(n); //add to list
                    else
                        check = n; //update
                }
                var newEnemies = msb_old.Parts.Enemies.FindAll(e => e.Name.Contains("message"));
                foreach (MSB1.Part.Enemy n in newEnemies)
                {
                    var check = msb_new.Parts.Enemies.Find(e => e.Name == n.Name);
                    if (check == null)
                        msb_new.Parts.Enemies.Add(n); //add to list
                    else
                        check = n; //update
                }

                var newObjects = msb_old.Parts.Objects.FindAll(e => e.Name.Contains("message"));
                foreach (MSB1.Part.Object n in newObjects)
                {
                    var check = msb_new.Parts.Objects.Find(e => e.Name == n.Name);
                    if (check == null)
                        msb_new.Parts.Objects.Add(n); //add to list
                    else
                        check = n; //update
                }

                //update model lists
                foreach (MSB1.Model.Enemy model in msb_old.Models.Enemies)
                {
                    var modelNew = msb_new.Models.Enemies;
                    if (modelNew.Find(e => e.Name == model.Name) == null)
                    {
                        modelNew.Add(model);
                    }
                }
                foreach (MSB1.Model.Object model in msb_old.Models.Objects)
                {
                    var modelNew = msb_new.Models.Objects;
                    if (modelNew.Find(e => e.Name == model.Name) == null)
                    {
                        modelNew.Add(model);
                    }
                }
                */

                msb_new.Write(msbPath_target);

                //done with an MSB
            }
            #endregion
            
        }

        private void b_browse_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            if (openFileDialog1.FileName == "")
            {
                b_install.Enabled = false;
                b_restoreBackups.Enabled = false;
                return;
            }
            else
            {
                if (Path.GetFileName(openFileDialog1.FileName) == "DarkSoulsRemastered.exe")
                {
                    DCXstring = ".dcx";
                    is_DSR = true;
                }
                else if (Path.GetFileName(openFileDialog1.FileName) == "DARKSOULS.exe")
                {
                    DCXstring = "";
                    is_DSR = false;
                }
                else
                {
                    b_install.Enabled = false;
                    b_restoreBackups.Enabled = false;
                    MessageBox.Show("You must select DarkSoulsRemastered.exe or DARKSOULS.exe", "Invalid selection", MessageBoxButtons.OK);
                    return;
                }

                installFolder = Path.GetDirectoryName(openFileDialog1.FileName);
                b_install.Enabled = true;
                b_restoreBackups.Enabled = true;
            }
        }

        private static void Create_Backup(string path)
        {
            if (File.Exists(path+".msgmodbackup") == false)
                File.Copy(path, path+".msgmodbackup", false); //don't overwrite
        }

        private static void RestoreBackup(string sourcePath, string targetPath)
        {
            FileSystem.DeleteFile(targetPath, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
            File.Move(sourcePath, targetPath, false);
        }

        private void b_restoreBackups_Click(object sender, EventArgs e)
        {
            var bakExt = ".msgmodbackup";
            string[] files = Directory.GetFiles($"{installFolder}",$"*{bakExt}", System.IO.SearchOption.AllDirectories);

            foreach (var file in files)
            {
                RestoreBackup(file, file.Replace(bakExt, ""));
            }

            System.Media.SystemSounds.Exclamation.Play(); //make noise

            if (files.Length > 0)
                MessageBox.Show("Backups restored.", "Restore Backups");
            else
                MessageBox.Show("No backups were found!", "Restore Backups");

        }

        private void b_SimpleInstallInfo_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Simple Installation does not install Special Effect Messages." +
                "\nThese message do something special when read," +
                "\nlike summoning a kitty cat." +
                "\n\nEnable this option if you are using other mods.",
                "Info",MessageBoxButtons.OK);
        }
    }
}