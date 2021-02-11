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
        [Column("id")]
        public int GIntId { get; set; }
        /// <summary>
        /// The type category a buisness falls under (Education, Recreation, etc...)
        /// </summary>
        [Column("category")]
        public string GStrCategory { get; set; }
        /// <summary>
        /// Updates the text file storing the category list
        /// Adds a new item sent to this method
        /// </summary>
        public static void UpdateCategoryList(string NewCategory)
        {
            //Replace filename
            string filepath = "Buisness_Categories.txt";
            using (StreamWriter fileOutput = File.AppendText(filepath))
            {
                fileOutput.WriteLine(NewCategory);
            }
        }
        /// <summary>
        /// Imports all the initial data from the buisness categories file
        /// </summary>
        public static void ImportFile()
        {
            //Replace filename
            string filepath = "Buisness_Categories.txt";
            using (StreamReader fileInput = File.OpenText(filepath))
            {
                string importCategory = "";
                using (var context = new DatabaseContext())
                {
                    while ((importCategory = fileInput.ReadLine()) != null)
                    {
                        Categories c = new Categories()
                        {
                            GStrCategory = importCategory
                        };
                        context.Categories.Add(c);
                        
                    }
                    context.SaveChanges();
                }
            }
        }

    }
}