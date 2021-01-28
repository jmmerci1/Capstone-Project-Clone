using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

/// <summary>
/// 
/// File Name: PaymentsPage.xaml.cs
/// 
/// Part of Project: DirectorsPortal
/// 
/// Original Author: Drake D. Herman
/// 
/// Date Created: 1/27/2021
/// 
/// File Purpose:
///     This file defines the logic for the 'Payments' screen in the Directors Portal application.
///     
/// Command Line Parameter List:
///     (NONE)
/// 
/// Environmental Returns: 
///     (NONE)
/// 
/// Sample Invocation:
///     This code is executed when the user navigates to the "Payment Info" screen from the Directors
///     portal main menu. 
///     
/// Global Variable List:
///     (NONE)
///     
/// Modification History:
///     1/27/2021 - DH: Inital creation
///     
/// </summary>
namespace DirectorsPortalWPF.PaymentInfoUI
{
    /// <summary>
    /// Interaction logic for PaymentsPage.xaml
    /// </summary>
    public partial class PaymentsPage : Page
    {
        /// <summary>
        /// Test list for setting up the UI. This variable should not be used in
        /// the final implmentation.
        /// </summary>
        List<Customer> CustomerList = new List<Customer>();

        /// <summary>
        /// Global Variable for tracking the currently selected customer button.
        /// </summary>
        string CurrentCustomer = "";

        /// <summary>
        /// Intializes the Page and content within the Page
        /// </summary>
        public PaymentsPage()
        {
            InitializeComponent();

            /* Generate the customer objects to populate some test payment
             * Expanders. */
            Random randomNum = new Random();
            int numOfCustomers = randomNum.Next(4, 10);

            int customerItr;
            for(customerItr = 0; customerItr < numOfCustomers; customerItr++) 
            {
                Customer customer = new Customer();
                customer.Id = customerItr;
                customer.Name = "Customer " + customerItr;
                customer.Payments = new List<Payment>();

                int numOfPayments = randomNum.Next(1, 10);

                int paymentItr;
                for (paymentItr = 0; paymentItr < numOfPayments; paymentItr++) 
                {
                    Payment payment = new Payment();
                    payment.PaymentDate = new DateTime(2020, 12, paymentItr + 1);
                    payment.Name = "Payment " + paymentItr;
                    payment.PaymentItems = new List<PaymentRowModel>();

                    int numOfItems = randomNum.Next(1, 5);

                    int itemItr;
                    for (itemItr = 0; itemItr < numOfItems; itemItr++) 
                    {
                        PaymentRowModel paymentItem = new PaymentRowModel();
                        paymentItem.Item = "Item " + itemItr;
                        paymentItem.Quantity = itemItr + 1;
                        paymentItem.UnitPrice = 50.00;

                        payment.PaymentItems.Add(paymentItem);
                    }

                    customer.Payments.Add(payment);
                }

                CustomerList.Add(customer);
            }
        }

        /// <summary>
        /// This functions loads the customer name selection button onto the UI.
        /// 
        /// TODO: Currently the customers are pulled from a test cusotmer objects list. A change will most likely
        ///       be needed for the final implemantation but this is most of the way there.
        /// 
        /// Original Author: Drake D. Herman
        /// Date Created: 1/27/2021
        /// 
        /// Modification History:
        ///     1/27/2020 - DH: Intial creation
        /// </summary>
        /// <param name="sender">The UI object that called the function.</param>
        /// <param name="e">Event data asscociated with this event.</param>
        void LoadCustomerNames(object sender, RoutedEventArgs e) 
        {
            foreach(Customer customer in CustomerList) 
            {
                ToggleButton tglBtnCustomer = new ToggleButton();
                tglBtnCustomer.HorizontalContentAlignment = HorizontalAlignment.Stretch;
                tglBtnCustomer.Tag = customer.Id.ToString();
                tglBtnCustomer.Checked += new RoutedEventHandler(CustomerChecked);
                tglBtnCustomer.Unchecked += new RoutedEventHandler(CustomerUnchecked);

                DockPanel dockPnlText = new DockPanel();

                Label lblCustName = new Label();
                lblCustName.Content = customer.Name;
                lblCustName.FontWeight = FontWeights.Bold;
                DockPanel.SetDock(lblCustName, Dock.Left);

                Label lblClickToSelect = new Label();
                lblClickToSelect.Content = "Click to select";
                lblClickToSelect.HorizontalContentAlignment = HorizontalAlignment.Right;

                dockPnlText.Children.Add(lblCustName);
                dockPnlText.Children.Add(lblClickToSelect);

                tglBtnCustomer.Content = dockPnlText;

                spCustomerNames.Children.Add(tglBtnCustomer);
            }
        }

        /// <summary>
        /// This method handles a customer button being toggled to the "Check" state.
        /// 
        /// Original Author: Drake D. Herman
        /// Date Created: 1/27/2021
        /// 
        /// Modification History:
        ///     1/27/2020 - DH: Intial creation
        /// </summary>
        /// <param name="sender">The UI object that called the function.</param>
        /// <param name="e">Event data asscociated with this event.</param>
        private void CustomerChecked(object sender, RoutedEventArgs e)
        {
            ToggleButton tglBtnSender = sender as ToggleButton;
            string btnTag = tglBtnSender.Tag.ToString();
            CurrentCustomer = btnTag;

            ResetAllButtons(btnTag);

            PopulateCustomerPaymentsColumn(btnTag);
        }

        /// <summary>
        /// This method handles a customer button being toggled to the "UnCheck" state.
        /// 
        /// Original Author: Drake D. Herman
        /// Date Created: 1/27/2021
        /// 
        /// Modification History:
        ///     1/27/2020 - DH: Intial creation
        /// </summary>
        /// <param name="sender">The UI object that called the function.</param>
        /// <param name="e">Event data asscociated with this event.</param>
        private void CustomerUnchecked(object sender, RoutedEventArgs e)
        {
            ToggleButton tglBtnSender = sender as ToggleButton;
            DockPanel dockPnlContent = tglBtnSender.Content as DockPanel;
            Label lblClick = dockPnlContent.Children.OfType<Label>().LastOrDefault();
            lblClick.Visibility = Visibility.Visible;

            dpSelectedCustomer.Visibility = Visibility.Hidden;

            spCustomerPayments.Children.Clear();
        }

        /// <summary>
        /// This methods saves the newly entered payment and regenerates the payment list.
        /// 
        /// TODO: This method will need to add a payment to the database.
        /// 
        /// Original Author: Drake D. Herman
        /// Date Created: 1/27/2021
        /// 
        /// Modification History:
        ///     1/27/2020 - DH: Intial creation        
        /// </summary>
        /// <param name="sender">The UI object that called the function.</param>
        /// <param name="e">Event data asscociated with this event.</param>
        private void SaveNewPayment(object sender, RoutedEventArgs e)
        {
            btnAddPayment.Visibility = Visibility.Visible;
            spCustomerPayments.Children.Clear();
            PopulateCustomerPaymentsColumn(CurrentCustomer);
        }

        /// <summary>
        /// This methods discards the new payment and regenerates the payment list.
        /// 
        /// Original Author: Drake D. Herman
        /// Date Created: 1/27/2021
        /// 
        /// Modification History:
        ///     1/27/2020 - DH: Intial creation
        /// </summary>
        /// <param name="sender">The UI object that called the function.</param>
        /// <param name="e">Event data asscociated with this event.</param>
        private void CancelNewPayment(object sender, RoutedEventArgs e)
        {
            btnAddPayment.Visibility = Visibility.Visible;
            spCustomerPayments.Children.Clear();
            PopulateCustomerPaymentsColumn(CurrentCustomer);
        }

        /// <summary>
        /// This methods generates the UI for adding a new payment in place of the 
        /// payments list.
        /// 
        /// Original Author: Drake D. Herman
        /// Date Created: 1/27/2021
        /// 
        /// Modification History:
        ///     1/27/2020 - DH: Intial creation
        /// 
        /// </summary>
        /// <param name="sender">The UI object that called the function.</param>
        /// <param name="e">Event data asscociated with this event.</param>
        private void AddNewPayment(object sender, RoutedEventArgs e)
        {
            spCustomerPayments.Children.Clear();

            btnAddPayment.Visibility = Visibility.Hidden;

            Grid gTextFields = new Grid();
            gTextFields.Width = spCustomerPayments.Width;

            /* Define the grid columns. */
            ColumnDefinition colDef1 = new ColumnDefinition();
            colDef1.Width = new GridLength(1, GridUnitType.Star);
            ColumnDefinition colDef2 = new ColumnDefinition();
            colDef2.Width = new GridLength(2, GridUnitType.Star);

            gTextFields.ColumnDefinitions.Add(colDef1);
            gTextFields.ColumnDefinitions.Add(colDef2);

            /* Define the grid rows */
            RowDefinition rowDef1 = new RowDefinition();
            RowDefinition rowDef2 = new RowDefinition();

            gTextFields.RowDefinitions.Add(rowDef1);
            gTextFields.RowDefinitions.Add(rowDef2);

            /* Create the payment title entry. */
            Label lblPaymentName = new Label();
            lblPaymentName.Content = "Payment Name:";
            lblPaymentName.Margin = new Thickness(1, 1, 2, 1);
            Grid.SetRow(lblPaymentName, 0);
            Grid.SetColumn(lblPaymentName, 0);

            TextBox txtPaymentName = new TextBox();
            txtPaymentName.Margin = new Thickness(1, 1, 2, 1);
            Grid.SetRow(txtPaymentName, 0);
            Grid.SetColumn(txtPaymentName, 1);

            /* Create the date entry. */
            Label lblPaymentDate = new Label();
            lblPaymentDate.Content = "Payment Date:";
            lblPaymentDate.Margin = new Thickness(1, 1, 2, 1);
            Grid.SetRow(lblPaymentDate, 1);
            Grid.SetColumn(lblPaymentDate, 0);

            DatePicker datePkrPaymentDate = new DatePicker();
            datePkrPaymentDate.Margin = new Thickness(1, 1, 2, 1);
            Grid.SetRow(datePkrPaymentDate, 1);
            Grid.SetColumn(datePkrPaymentDate, 1);

            gTextFields.Children.Add(lblPaymentName);
            gTextFields.Children.Add(txtPaymentName);
            gTextFields.Children.Add(lblPaymentDate);
            gTextFields.Children.Add(datePkrPaymentDate);

            /* Create the data grid for enterig new payments. */
            List<PaymentRowModel> newPaymentItems = new List<PaymentRowModel>();

            DataGrid dgNewPayment = new DataGrid();
            dgNewPayment.AutoGenerateColumns = false;
            dgNewPayment.Margin = new Thickness(2, 5, 2, 1);
            dgNewPayment.ItemsSource = newPaymentItems;

            DataGridTextColumn colItem = new DataGridTextColumn();
            colItem.Width = new DataGridLength(2, DataGridLengthUnitType.Star);
            colItem.Header = "Item";
            colItem.Binding = new Binding("Item");
            dgNewPayment.Columns.Add(colItem);

            DataGridTextColumn colQuantity = new DataGridTextColumn();
            colQuantity.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            colQuantity.Header = "Quantity";
            colQuantity.Binding = new Binding("Quantity");
            dgNewPayment.Columns.Add(colQuantity);

            DataGridTextColumn colUnitCost = new DataGridTextColumn();
            colUnitCost.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            colUnitCost.Header = "Unit Price";
            colUnitCost.Binding = new Binding("UnitPrice");
            dgNewPayment.Columns.Add(colUnitCost);

            /* Create the save and cancel buttons. */
            StackPanel spSaveAndCancel = new StackPanel();
            spSaveAndCancel.Width = spCustomerPayments.Width;
            spSaveAndCancel.Orientation = Orientation.Horizontal;
            spSaveAndCancel.HorizontalAlignment = HorizontalAlignment.Right;
            spSaveAndCancel.Margin = new Thickness(2, 5, 2, 1);

            Button btnCancel = new Button();
            btnCancel.Tag = 
            btnCancel.Margin = new Thickness(1, 0, 1, 0);
            btnCancel.Padding = new Thickness(2, 0, 2, 0);
            btnCancel.Content = "Cancel";
            btnCancel.Click += CancelNewPayment;

            Button btnSave = new Button();
            btnSave.Margin = new Thickness(1, 0, 1, 0);
            btnSave.Padding = new Thickness(2, 0, 2, 0);
            btnSave.Content = "Add Payment";
            btnSave.Click += SaveNewPayment;

            spSaveAndCancel.Children.Add(btnCancel);
            spSaveAndCancel.Children.Add(btnSave);

            spCustomerPayments.Children.Add(gTextFields);
            spCustomerPayments.Children.Add(dgNewPayment);
            spCustomerPayments.Children.Add(spSaveAndCancel);
        }

        /// <summary>
        /// This method resets all of the customer buttons.
        /// 
        /// This process includes unchecking all button besides the currently selected
        /// button and setting the visibility of the "Click to select" label of the selected
        /// button to hidden.
        /// 
        /// Original Author: Drake D. Herman
        /// Date Created: 1/27/2021
        /// 
        /// Modification History:
        ///     1/27/2020 - DH: Intial creation
        /// 
        /// </summary>
        /// <param name="selectedBtnTag">The tag of the currently selected customer button.</param>
        private void ResetAllButtons(string selectedBtnTag)
        {
            List<ToggleButton> customerBtnsList = spCustomerNames.Children.OfType<ToggleButton>().ToList();

            int customerBtnIterator;
            for(customerBtnIterator = 0; customerBtnIterator < customerBtnsList.Count; customerBtnIterator++) 
            {
                ToggleButton tglBtnCustomer = customerBtnsList[customerBtnIterator];
                string btnTag = tglBtnCustomer.Tag.ToString();

                DockPanel dockPnlContent = tglBtnCustomer.Content as DockPanel;
                Label lblClick = dockPnlContent.Children.OfType<Label>().LastOrDefault();

                if (btnTag.Equals(selectedBtnTag)) 
                {
                    /* We found the currently selected button so continue. */
                    lblClick.Visibility = Visibility.Hidden;
                    continue;
                }

                tglBtnCustomer.IsChecked = false;
                lblClick.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// This method populates the UI with all of the selected customers payments.
        /// 
        /// An expander is created for each customer payment that holds a datagrid containing the inforamtion
        /// about the payments items.
        /// 
        /// Original Author: Drake D. Herman
        /// Date Created: 1/27/2021
        /// 
        /// Modification History:
        ///     1/27/2020 - DH: Intial creation
        /// 
        /// </summary>
        /// <param name="custId">The ID of the currently selected customer.</param>
        private void PopulateCustomerPaymentsColumn(string custId)
        {
            Customer selectedCust = CustomerList[int.Parse(custId)];
            lblSelectedCustomer.Content = selectedCust.Name;
            dpSelectedCustomer.Visibility = Visibility.Visible;

            foreach (Payment payment in selectedCust.Payments) 
            {
                Expander expPayment = new Expander();
                expPayment.HorizontalAlignment = HorizontalAlignment.Left;
                expPayment.ExpandDirection = ExpandDirection.Down;
                expPayment.FontSize = 15;
                expPayment.FontWeight = FontWeights.Bold;
                expPayment.Width = spCustomerPayments.Width;

                StackPanel spPaymentHeader = new StackPanel();

                Label lblPaymentDate = new Label();
                lblPaymentDate.FontSize = 7;
                lblPaymentDate.Padding = new Thickness(10, 0, 0, 0);
                lblPaymentDate.Content = payment.PaymentDate.ToString("MM/dd/yyyy");

                Label lblCustomerName = new Label();
                lblCustomerName.FontSize = 15;
                lblCustomerName.Padding = new Thickness(10, 0, 0, 0);
                lblCustomerName.Content = payment.Name;

                spPaymentHeader.Children.Add(lblPaymentDate);
                spPaymentHeader.Children.Add(lblCustomerName);

                expPayment.Header = spPaymentHeader;

                DataGrid dgPaymentItems = new DataGrid();
                dgPaymentItems.Width = spCustomerPayments.Width;
                dgPaymentItems.IsReadOnly = true;

                DataGridTextColumn colItem = new DataGridTextColumn();
                colItem.Width = new DataGridLength(2, DataGridLengthUnitType.Star);
                colItem.Header = "Item";
                colItem.Binding = new Binding("Item");
                dgPaymentItems.Columns.Add(colItem);

                DataGridTextColumn colQuantity = new DataGridTextColumn();
                colQuantity.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
                colQuantity.Header = "Quantity";
                colQuantity.Binding = new Binding("Quantity");
                dgPaymentItems.Columns.Add(colQuantity);

                DataGridTextColumn colUnitCost = new DataGridTextColumn();
                colUnitCost.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
                colUnitCost.Header = "Unit Price";
                colUnitCost.Binding = new Binding("UnitPrice");
                dgPaymentItems.Columns.Add(colUnitCost);

                foreach (PaymentRowModel paymentItems in payment.PaymentItems) 
                {
                    dgPaymentItems.Items.Add(paymentItems);
                }

                expPayment.Content = dgPaymentItems;

                spCustomerPayments.Children.Add(expPayment);
            }
        }
    }
}

/// <summary>
/// A test class that defines properties that make up an item in a
/// payment.
/// </summary>
public class PaymentRowModel
{
    public string Item { get; set; }
    public int Quantity { get; set; }
    public double UnitPrice { get; set; }
}

/// <summary>
/// A test class that defines the properties of a payment.
/// </summary>
public class Payment
{ 
    public DateTime PaymentDate { get; set; }
    public string Name { get; set; }
    public List<PaymentRowModel> PaymentItems { get; set; }
}

/// <summary>
/// A test class that defines the properties of a customer.
/// </summary>
public class Customer
{ 
    public int Id { get; set; }
    public string Name { get; set; }
    public List<Payment> Payments { get; set; } 
}