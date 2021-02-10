using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Globalization;
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
                //FileStream fs = (FileStream)saveFileDialog.OpenFile();
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
        /// Original Author: Kaden Thompson
        /// Date Created: 1/27/2021
        /// 
        /// Modification History:
        ///     1/26/2021 - BD: Initial creation
        ///
        /// </summary>
        public void RestoreFromBackup(string strTargetPath)
        {

            string strFilePath;
            string strFileContent;

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "txt files (*.txt)|.txt";
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
                
            }
        }

        public void CreateBackup(string strTargetPath)
        {
            if (strTargetPath.Length == 0)
            {
                MessageBox.Show("Please choose a path first");
            }
            else
            {
                string strSourcePath = DatabaseContext.GetFolderPath();

                string fileName = "DB_Backup " + ToSafeFileName(DateTime.Now.ToString()) + ".db";

                string strSourceFile = Path.Combine(strSourcePath, "database.db");
                string strDestFile = Path.Combine(strTargetPath, fileName);

                File.Copy(strSourceFile, strDestFile, true);

                if (File.Exists(strSourceFile))
                {
                    MessageBox.Show("File saved successfully");
                }
            }

        }
        public string ToSafeFileName(string s)
        {
            return s
                .Replace(":", "_")
                .Replace("/", "_");
        }

    }
}
