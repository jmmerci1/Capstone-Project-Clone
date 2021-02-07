using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

/// <summary>
/// 
/// File Name: TodoPage.xaml.cs
/// 
/// Part of Project: DirectorsPortal
/// 
/// Original Author: Benjamin J. Dore
/// 
/// File Purpose:
///     This file defines the logic for the 'Todo' screen in the Directors Portal application. The
///     To do page displays a list of outstanding tasks that need to be completed by the end user. These
///     actions could include:
///         - Reviewing a new member data change
///         - Performing a DB backup
///         - Reviewing a new member request.
///     
/// </summary>

namespace DirectorsPortalWPF.TodoUI
{
    /// <summary>
    /// Interaction logic for TodoPage.xaml
    /// </summary>
    public partial class TodoPage : Page
    {
        /// <summary>
        /// Initialize the Page and contents within the Page
        /// </summary>
        public TodoPage()
        {
            InitializeComponent();

            btnDone1.Click += (sender, e) => MarkAsDone(sender, e, card1);  // Template code, should not be used in production
            btnDone2.Click += (sender, e) => MarkAsDone(sender, e, card2);  // Template code, should not be used in production

            btnMarkAllDone.Click += (sender, e) =>
            {
                sPanelTodoList.Children.Clear();

                if (sPanelTodoList.Children.Count == 0)
                {
                    sPanelTodoList.Children.Add(ShowNoTodo());      // If there is no Todo items, notify the end user.
                }
            };

            for (int i = 0; i < 4; i++)
            {

                StackPanel sPanelCard = CreateStackPanelCard();
                StackPanel sPanelCardContent = new StackPanel();

                Button btnDone = CreateButton("Done");

                btnDone.Click += (sender, e) => MarkAsDone(sender, e, sPanelCard);

                //
                // Logic could be included here to populate the Todo list with items from DB. For
                // now it's just template code. 
                //
                // TODO: Modify the init code in the TodoPage.cs file to pull new Todos from DB,
                // remove the template code one DB functionality implemented.
                //
                TextBlock txtBoxCardContent = new TextBlock
                {
                    Text = $"A pending task {i + 3}",
                    Margin = new Thickness(5, 5, 5, 0)
                };

                TextBlock txtBoxCardClicker = new TextBlock
                {
                    Text = "Click to View",
                    Margin = new Thickness(5, 0, 5, 5),
                    FontSize = 10
                };

                sPanelCardContent.Children.Add(txtBoxCardContent);
                sPanelCardContent.Children.Add(txtBoxCardClicker);

                sPanelCard.Children.Add(btnDone);
                sPanelCard.Children.Add(sPanelCardContent);

                sPanelTodoList.Children.Add(sPanelCard);
                lblNumberOfTodo.Content = $"{sPanelTodoList.Children.Count} Number of TODO";
            }
        }

        /// <summary>
        /// Intended to show a Todo item that simply states there is no items in the Todo list  
        /// </summary>
        /// <returns>Returns a StackPanel containing the contents to show a Todo item indicating there are no Todo items</returns>
        private StackPanel ShowNoTodo()
        {
            StackPanel sPanelCard = CreateStackPanelCard();
            StackPanel sPanelCardContent = new StackPanel();

            TextBlock txtBoxCardContent = new TextBlock
            {
                Text = "TODO is empty",
                Margin = new Thickness(10, 10, 10, 10)
            };

            sPanelCardContent.Children.Add(txtBoxCardContent);
            sPanelCard.Children.Add(sPanelCardContent);
            lblNumberOfTodo.Content = "0 Number of TODO";

            return sPanelCard;

        }

        /// <summary>
        /// Invokes when a Todo item's 'Done' button is clicked. Essentially marks an item as done.
        /// For the moment this function simply removes the card from the Todo list but can be modified to work
        /// with a DB.
        /// 
        /// TODO: Modify the MarkAsDone method in Todo to use DB
        /// </summary>
        /// <param name="sender">Done button from the selected task</param>
        /// <param name="e">The Click event</param>
        /// <param name="sPanelCard">The Stack Panel card for the Todo item</param>
        private void MarkAsDone(object sender, RoutedEventArgs e, StackPanel sPanelCard)
        {
            sPanelTodoList.Children.Remove(sPanelCard);
            sPanelCard.Children.Clear();

            lblNumberOfTodo.Content = $"{sPanelTodoList.Children.Count} Number of TODO";

            if (sPanelTodoList.Children.Count == 0)
            {
                sPanelTodoList.Children.Add(ShowNoTodo());      // If there is no Todo items, notify the end user.
            }

            GC.Collect();           // Initiate garbage collection so rogue stack panel children isn't floating around in heap.
        }

        /// <summary>
        /// Creates a new Stack Panel to be used on the 'Todo' screen. This Stack Panel represents a 'Card'
        /// That will contain the contents of a single Todo item.
        /// </summary>
        /// <returns>Returns a generated StackPanel object with pre-set formatting</returns>
        private StackPanel CreateStackPanelCard()
        {
            StackPanel sPanelNewStackPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Background = Brushes.White,
                Margin = new Thickness(50, 1, 50, 1)
            };

            return sPanelNewStackPanel;
        }


        /// <summary>
        /// Creates a new Button to  be used on the 'Todo' screen.
        /// </summary>
        /// <param name="strButtonText">Returns a generated Button object with pre-set formatting</param>
        /// <returns></returns>
        private Button CreateButton(string strButtonText)
        {
            Button btnNewButton = new Button()
            {
                Content = strButtonText,

                Margin = new Thickness(5, 5, 5, 5),
                Template = (ControlTemplate)Application.Current.Resources["xtraSmallButtonGrey"],
            };
            return btnNewButton;
        }
    }
}
