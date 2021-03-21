using System;
using System.Collections.Generic;
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

namespace DirectorsPortalWPF.Controls
{
    /// <summary>
    /// Interaction logic for GenericTextInput.xaml
    /// </summary>
    public partial class EmailInput : UserControl
    {
        public string GStrInputName { get; set; } = string.Empty;
        public int GIntEmailId { get; set; } = -1;
        public ContactInput GCiContactInputParent { get; set; }

        public EmailInput()
        {
            InitializeComponent();

            DataContext = this;
        }

        private void BtnRemoveEmail_Click(object sender, RoutedEventArgs e)
        {
            if (GIntEmailId != -1 && GCiContactInputParent != null) 
            {
                GCiContactInputParent.GIntEmailsToRemove.Add(GIntEmailId);
            }

            Content = null;
        }
    }
}
