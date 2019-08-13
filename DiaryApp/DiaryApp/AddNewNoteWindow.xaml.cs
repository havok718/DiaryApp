using SQLite;
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
using System.Windows.Shapes;

namespace DiaryApp
{
    /// <summary>
    /// Interaction logic for AddNewNoteWindow.xaml
    /// </summary>
    public partial class AddNewNoteWindow : Window
    {
        public AddNewNoteWindow()
        {
            InitializeComponent();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            DataGridInfo dataGridInfo = new DataGridInfo()
            {
                Importance = tbImportance.Text,
                Date = dpDate.Text.ToString(),
                Body = tbBody.Text,
                Signature = tbSignature.Text,
                Location = tbLocation.Text
            };

            

            using (SQLiteConnection connection = new SQLiteConnection(App.dataBasePath))
            {
                connection.CreateTable<DataGridInfo>();
                connection.Insert(dataGridInfo);
            }
                

            Close();
        }
    }
}
