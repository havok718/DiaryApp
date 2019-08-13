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
    /// Interaction logic for UpdateDeleteNoteWindow.xaml
    /// </summary>
    public partial class UpdateDeleteNoteWindow : Window
    {
        DataGridInfo dataGridInfo;
        public UpdateDeleteNoteWindow(DataGridInfo dataGridInfo)
        {
            InitializeComponent();
            this.dataGridInfo = dataGridInfo;
            tbImportance.Text = dataGridInfo.Importance;
            tbLocation.Text = dataGridInfo.Location;
            tbSignature.Text = dataGridInfo.Signature;
            tbBody.Text = dataGridInfo.Body;
            dpDate.Text = dataGridInfo.Date;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            dataGridInfo.Importance = tbImportance.Text;
            dataGridInfo.Location = tbLocation.Text;
            dataGridInfo.Body = tbBody.Text;
            dataGridInfo.Date = dpDate.Text;
            dataGridInfo.Signature = tbSignature.Text;

            if (dataGridInfo != null)
            {
                using (SQLiteConnection connection = new SQLiteConnection(App.dataBasePath))
                {
                    connection.CreateTable<DataGridInfo>();
                    connection.Update(dataGridInfo);
                }
            }
            Close();
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridInfo != null)
            {
                using (SQLiteConnection connection = new SQLiteConnection(App.dataBasePath))
                {
                    connection.CreateTable<DataGridInfo>();
                    connection.Delete(dataGridInfo);
                }
            }
            Close();
        }
    }
}
