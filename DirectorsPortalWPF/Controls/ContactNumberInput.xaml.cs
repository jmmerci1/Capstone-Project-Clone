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
    /// Interaction logic for ContactNumberInput.xaml
    /// </summary>
    public partial class ContactNumberInput : UserControl
    {
        public string GStrInputName { get; set; }
        public Visibility GVisRemovable { get; set; }

        public ContactNumberInput()
        {
            InitializeComponent();

            DataContext = this;
        }

        private void BtnRemoveNumber_Click(object sender, RoutedEventArgs e)
        {
            Content = null;
        }
    }
}
