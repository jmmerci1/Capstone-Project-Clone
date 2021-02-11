using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using DirectorPortalDatabase;
using System.IO;
using System.Windows;


/// <summary>
/// 
/// File Name: BackupUtility.cs
/// 
/// Part of Project: DirectorsPortal
/// 
/// Original Author: Kaden M. Thompson
/// 
/// Date Created: 1/25/2021
/// 
/// File Purpose:
///     This file is used for managing backups of the database, allowing the user to dictate wehre to save a backup to
///     or where the user would like to pull a backup from
///     
/// Command Line Parameter List:
///     (NONE)
/// 
/// Environmental Returns: 
///     (NONE)
/// 
/// Sample Invocation:
///     This code is executed when the user browses for a backup location or picks where to backup from
///     
/// Global Variable List:
///     (NONE)
///     
/// Modification History:
///     1/25/2021 - BD: Inital creation
///     
/// </summary>

namespace DirectorsPortalWPF.SettingsUI
{
    /// <summary>
    /// Provides a front end for picking backup locations
    /// </summary>
    class BackupUtility
    {

        /// <summary>
        ///
        /// Allows the user to select a folder on their system to be the path for the backup database file.
        /// 
        /// Original Author: Kaden Thompson
        /// Date Created: 1/27/2021
        /// 
        /// Modification History:
        ///     1/26/2021 - BD: Initial creation
        ///
        /// </summary>
        /// <returns>A string containing the path the user choose to save database to</returns>
        public String ChooseBackupLocation()
        {



            CommonOpenFileDialog commonOpenFileDialog = new CommonOpenFileDialog();
            commonOpenFileDialog.InitialDirectory = "C:\\Users";
            commonOpenFileDialog.IsFolderPicker = true;

            if(commonOpenFileDialog.ShowDialog() == CommonFileDialogResult.Ok)
            { 
                return commonOpenFileDialog.FileName;
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        ///
        /// Allows user to select a database file to restore the system from
        /// 
        /// </summary>
        public void RestoreFromBackup(string strTargetPath)
        {

            string strBackupFilePath;
            string strDestFile = Path.Combine(DatabaseContext.GetFolderPath(), "database.db");

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Database files (*.db)|*.db";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;


            if (strTargetPath.Length > 0)
            {
                openFileDialog.InitialDirectory = strTargetPath;
            }
            else
            {
                openFileDialog.InitialDirectory = "C:\\";
            }


            if (openFileDialog.ShowDialog() == true)
            {
                strBackupFilePath = openFileDialog.FileName;
                File.Copy(strBackupFilePath, strDestFile, true);
                MessageBox.Show("Database restored from backup");
            }
        }

        /// <summary>
        /// 
        /// Creates a copy from the database that is stored in the AppData>roaming folder and places it where the user chooses
        /// 
        /// Database backups include the data and time so the user can tell what backups are from when.
        /// 
        /// finally checks to make sure the file was actually created
        /// 
        /// </summary>
        /// <param name="strTargetPath">
        /// String that contains the path where the database will be backed up to, decided by the user when choosing browse
        /// from the UI
        /// </param>
        public void CreateBackup(string strTargetPath)
        {
            string strSourcePath = DatabaseContext.GetFolderPath();
            string strBackupFileName;
            string strSourcePathAndFile;
            string strDestPathAndFile;

            if (strTargetPath.Length == 0)
            {
                MessageBox.Show("Please choose a path first");
            }
            else
            {
                

                strBackupFileName= "DB_Backup " + ToSafeFileName(DateTime.Now.ToString()) + ".db";

                strSourcePathAndFile = Path.Combine(strSourcePath, "database.db");
                strDestPathAndFile = Path.Combine(strTargetPath, strBackupFileName);

                File.Copy(strSourcePathAndFile, strDestPathAndFile, false);

                if (File.Exists(strDestPathAndFile))
                {
                    MessageBox.Show("File saved successfully");
                }
            }

        }

        /// <summary>
        /// 
        /// Removes colons and forward slashes created by the backup file name when assigning the current data and time to it
        /// 
        /// </summary>
        /// 
        /// <param name="strFileNameToClean">
        /// File name with illegal characters
        /// </param>
        /// 
        /// <returns>
        /// File name with illegal characters removed
        /// </returns>
        public string ToSafeFileName(string strFileNameToClean)
        {
            return strFileNameToClean
                .Replace(":", "_")
                .Replace("/", "_");
        }

    }
    
}
