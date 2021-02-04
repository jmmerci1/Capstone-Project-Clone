using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace DirectorsPortalWPF.SettingsUI
{
    class BackupUtility
    {
        public BackupUtility()
        {

        }

        public String CreateBackup()
        {
            CommonOpenFileDialog commonOpenFileDialog = new CommonOpenFileDialog();
            commonOpenFileDialog.InitialDirectory = "C:\\Users";
            commonOpenFileDialog.IsFolderPicker = true;

            if(commonOpenFileDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                
                return commonOpenFileDialog.FileName;
                //FileStream fs = (FileStream)saveFileDialog.OpenFile();
            }
            else
            {
                return "";
            }
        }

        public void RestoreFromBackup()
        {
            String strFilePath;
            String strFileContent;

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "txt files (*.txt)|.txt";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;
            openFileDialog.ShowDialog();

            if (openFileDialog.ShowDialog() == true)
            {

            }


        }
    }
}
