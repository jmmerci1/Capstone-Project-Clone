using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;


///<summary>
///This file allows the user to select an excel file and stores
///it into a list for future use.
///</summary>

namespace Excel
{

    /// <summary>
    /// This class contains Form1 that was used soley to add a button click event.
    /// </summary>
    public partial class Form1 : Form
    {

        /// <summary>
        /// Form initialization 
        /// </summary>
        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Contains the button click event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            //List using class Members to store Member information.
            List<Members> Members = new List<Members>();

            //String that contains the excel file that the user selects.
            string FilePath = FindFile();

            //Populated members List.
            Members = ReadExcelFile(FilePath);

        

        }

        /// <summary>
        /// ReadExcelFile method that will read the prior selected excel file. 
        /// </summary>
        /// <param name="filePath">String that contains the name and path of the excel sheet that will be read.</param>
        /// <returns>List that contains Members information from excel sheet.</returns>
        private List<Members> ReadExcelFile(string filePath)
        {
            //Temporary List to build the List that is returned.
            List<Members> TempMembers = new List<Members>();

            //Stream that is useed to read the excel file.
            using (var Stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
            {

                //Reader that is used to hold data read from the excel file.
                using (var Reader = ExcelReaderFactory.CreateReader(Stream))
                { 
                    do
                    {
                        while (Reader.Read())
                        {
                            //Add data that was read from the Excel sheet into a list.
                            TempMembers.Add(new Members(Convert.ToString(Reader[0]), Convert.ToString(Reader[1]), Convert.ToString(Reader[2]), 
                                                        Convert.ToString(Reader[3]), Convert.ToString(Reader[4]), Convert.ToString(Reader[5]), 
                                                        Convert.ToString(Reader[6]), Convert.ToString(Reader[7]), Convert.ToString(Reader[8]), 
                                                        Convert.ToString(Reader[9]), Convert.ToString(Reader[10]), Convert.ToString(Reader[11]),
                                                        Convert.ToString(Reader[12]), Convert.ToString(Reader[13]), Convert.ToString(Reader[14]),
                                                        Convert.ToString(Reader[15]), Convert.ToString(Reader[17]), Convert.ToString(Reader[19])));
                       
                        }
                        //Moves to the next sheet.
                    } while (Reader.NextResult());

                   
                }
            }

            //Returns the temporary list for further use.
            return TempMembers;
        }

        /// <summary>
        /// FindFile method that allows the user to select the file they would like to be read into the application.
        /// </summary>
        /// <returns>String that contains the file path.</returns>
        private string FindFile()
        {
            //Strings to hold the file name and its extension.
            string FileName = "";
            string FileExtension = "";

            //File dialog for ease of use when selecting an Excel file.
            OpenFileDialog FileDialog = new OpenFileDialog();

            //File dialog window formating and filtering.
            FileDialog.Title = "Excel File Dialog";
            FileDialog.InitialDirectory = @"c:\";
            FileDialog.Filter = "All files (*.*)|*.*|All files (*.*)|*.*";
            FileDialog.FilterIndex = 2;
            FileDialog.RestoreDirectory = true;
            FileDialog.ShowDialog();

            //FileExtension string that breaks apart the file name and its extention.
            FileExtension = Path.GetExtension(FileDialog.FileName);

            //Checks whether the file is an appropriate file type.
            if (FileExtension.CompareTo(".xls") == 0 || FileExtension.CompareTo(".xlsx") == 0)
            {
                FileName = FileDialog.FileName;
                
            }
            else
            {
                //Messagebox that prompts user to select the correct file type.
                MessageBox.Show("Please selecet a Excel file.");
            }

            //Returns the file name.
            return FileName;
        }
    }
}


/// <summary>
/// Class Members that contains the data fields for members.
/// </summary>
class Members
{
    //Strings that will contain member information.
    private string Established;
    private string Level;
    private string BusinessName;
    private string MailingAddress;
    private string LocationAddress;
    private string CityStateZip;
    private string ContactPerson;
    private string PhoneNumber;
    private string FaxNumber;
    private string EmailAddress;
    private string WebsiteAddress;
    private string DuesPaid;
    private string RaffleTicketReturnedPaid;
    private string Credit;
    private string Terms;
    private string Notes;
    private string FreeWebAd;
    private string Ballot;

    //Default constructor.
    public Members() { }

    //Constructor that initializes Member Class
    public Members(string established, string level, string businessName, string mailingAddress,
                   string locationAddress, string cityStateZip, string contactPerson, string phoneNumber,
                   string faxNumber, string emailAddress, string websiteAddress, string duesPaid, string raffleTicketReturnedPaid, 
                   string credit, string terms, string notes, string freeWebAd, string ballot)
    {
        //Sets variables with constructor information.
        this.Established = established;
        this.Level = level;
        this.BusinessName = businessName;
        this.MailingAddress = mailingAddress;
        this.LocationAddress = locationAddress;
        this.CityStateZip = cityStateZip;
        this.ContactPerson = contactPerson;
        this.PhoneNumber = phoneNumber;
        this.FaxNumber = faxNumber;
        this.EmailAddress = emailAddress;
        this.WebsiteAddress = websiteAddress;
        this.DuesPaid = duesPaid;
        this.RaffleTicketReturnedPaid = raffleTicketReturnedPaid;
        this.Credit = credit;
        this.Terms = terms;
        this.Notes = notes;
        this.FreeWebAd = freeWebAd;
        this.Ballot = ballot;
    }


    /// <summary>
    /// Get method that returns desired data. There are 18 of these.
    /// </summary>
    /// <returns></returns>
    public string getEstablished()
    {
        return this.Established;
    }
    public string getLevel()
    {
        return this.Level;
    }
    public string getBusinessName()
    {
        return this.BusinessName;
    }
    public string getMailingAddress()
    {
        return this.MailingAddress;
    }
    public string getLocationAddress()
    {
        return this.LocationAddress;
    }
    public string getCityStateZip()
    {
        return this.CityStateZip;
    }
    public string getContactPerson()
    {
        return this.ContactPerson;
    }
    public string getPhoneNumber()
    {
        return this.PhoneNumber;
    }
    public string getFaxNumber()
    {
        return this.FaxNumber;
    }
    public string getEmailAddress()
    {
        return this.EmailAddress;
    }
    public string getWebsiteAddress()
    {
        return this.WebsiteAddress;
    }
    public string getDuesPaid()
    {
        return this.DuesPaid;
    }
    public string getRaffleTicketReturnedPaid()
    {
        return this.RaffleTicketReturnedPaid;
    }
    public string getCredit()
    {
        return this.Credit;
    }
    public string getTerms()
    {
        return this.Terms;
    }
    public string getNotes()
    {
        return this.Notes;
    }
    public string getFreeWebAd()
    {
        return this.FreeWebAd;
    }
    public string getBallot()
    {
        return this.Ballot;
    }
}