using System;
using System.Collections.Generic;
using SQLite;
using System.Data.SQLite;
using System.IO;
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
    /// Interaction logic for ImportByDateRange.xaml
    /// </summary>
    public partial class ImportByDateRange : Window
    {
        public ImportByDateRange()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Кнопка, выполняющая добавление элементов в БД по диапазону дат
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnPickDateRange_Click(object sender, RoutedEventArgs e)
        {
            DateTime start = (DateTime)dpStart.SelectedDate;
            DateTime end = (DateTime)dpEnd.SelectedDate;
            var fileLines = File.ReadAllLines(App.fileSaveLocation).ToList();
            List<DataGridInfo> fromFile = new List<DataGridInfo>();
            try
            {
                foreach (string line in fileLines)
                {
                    string[] s = line.Split('Ω');
                    if (s != null)
                    {
                        DataGridInfo import = new DataGridInfo();
                        import.Importance = s[0].Trim();
                        import.Date = s[1].Trim();
                        import.Body = s[2].Trim();
                        import.Signature = s[3].Trim();
                        import.Location = s[4].Trim();
                        DateTime current = Convert.ToDateTime(import.Date);
                        if (current >= start && current <= end)
                            fromFile.Add(import);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при импорте из файла. Проверьте формат данных в файле. " + ex);
            }

            foreach (DataGridInfo item in fromFile)
            {
                using (SQLite.SQLiteConnection connection = new SQLite.SQLiteConnection(App.dataBasePath))
                {
                    connection.CreateTable<DataGridInfo>();
                    connection.Insert(item);
                }
            }
            Close();
        }
    }
}
