using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectorPortalDatabase.Models
{
    public class Categories
    {
        /// <summary>
        /// The Primary Key of the address in the database.
        /// Autoincrements.
        /// </summary>
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// The type category a buisness falls under (Education, Recreation, etc...)
        /// </summary>
        public string Category { get; set; }
        /// <summary>
        /// Updates the text file storing the category list
        /// Adds a new item sent to this method
        /// </summary>
        public static void UpdateCategoryList(string strNewCategory)
        {
            //Replace filename
            string strFilepath = "Buisness_Categories.txt";
            using (StreamWriter swFileOutput = File.AppendText(strFilepath))
            {
                swFileOutput.WriteLine(strNewCategory);
            }
        }
        /// <summary>
        /// Imports all the initial data from the buisness categories file
        /// </summary>
        public static void ImportFile()
        {
            //Replace filename
            //string strFilepath = "Buisness_Categories.txt";
            var strExePath = AppDomain.CurrentDomain.BaseDirectory;
            var dinfPagesFolder = Directory.GetParent(strExePath).Parent.Parent;
            string strFilepath = "Business_Categories.txt";

            using (StreamReader srFileInput = File.OpenText(strFilepath))
            {
                string strImportCategory = "";
                using (var dbContext = new DatabaseContext())
                {
                    while ((strImportCategory = srFileInput.ReadLine()) != null)
                    {
                        Categories category = new Categories()
                        {
                            Category = strImportCategory
                        };
                        dbContext.Categories.Add(category);
                        
                    }
                    dbContext.SaveChanges();
                }
            }
        }

    }
}
