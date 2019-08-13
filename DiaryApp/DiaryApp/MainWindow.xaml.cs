using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
/// Разработать ежедневник.
/// В ежедневнике реализовать возможность 
/// - создания
/// - удаления
/// - реактирования 
/// записей
/// 
/// В отдельной записи должно быть не менее пяти полей
/// 
/// Реализовать возможность 
/// - Загрузки данных из файла
/// - Выгрузки данных в файл
/// - Добавления данных в текущий ежедневник из выбранного файла
/// - Импорт записей по выбранному диапазону дат
/// - Упорядочивания записей ежедневника по выбранному полю
namespace DiaryApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<DataGridInfo> dataInfo;
        public MainWindow()
        {
            InitializeComponent();
            dataInfo = new List<DataGridInfo>();
            ReadDataBase();
        }

        private void BtnAddString_Click(object sender, RoutedEventArgs e)
        {
            AddNewNoteWindow addNewNoteWindow = new AddNewNoteWindow();
            addNewNoteWindow.ShowDialog();
            ReadDataBase();
        }
        void ReadDataBase()
        {
            using (SQLiteConnection conn = new SQLiteConnection(App.dataBasePath))
            {
                conn.CreateTable<DataGridInfo>();
                dataInfo = conn.Table<DataGridInfo>().ToList();
            }

            if (dataInfo != null)
            {
                lvData.ItemsSource = dataInfo;
                dgData.ItemsSource = dataInfo;
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tbSearch = sender as TextBox;
            var filteredList = dataInfo.Where(c => c.Body.ToLower().Contains(tbSearch.Text.ToLower()) || c.Date.Contains(tbSearch.Text.ToLower()) 
                                                || c.Importance.ToLower().Contains(tbSearch.Text.ToLower()) || c.Signature.ToLower().Contains(tbSearch.Text.ToLower()) ||
                                                c.Location.ToLower().Contains(tbSearch.Text.ToLower())).ToList();
            lvData.ItemsSource = filteredList;
        }

        private void BtnDeleteString_Click(object sender, RoutedEventArgs e)
        {
            DataGridInfo selectedNote = (DataGridInfo)lvData.SelectedItem;
            if (selectedNote != null)
            {
                using (SQLiteConnection connection = new SQLiteConnection(App.dataBasePath))
                {
                    connection.CreateTable<DataGridInfo>();
                    connection.Delete(selectedNote);
                }
                ReadDataBase();
            }
        }

        private void BtnEditString_Click(object sender, RoutedEventArgs e)
        {
            DataGridInfo selectedNote = (DataGridInfo)lvData.SelectedItem;

            if (selectedNote != null)
            {
                UpdateDeleteNoteWindow updateDeleteNoteWindow = new UpdateDeleteNoteWindow(selectedNote);
                updateDeleteNoteWindow.ShowDialog();
                ReadDataBase();
            }
        }

        private void DgData_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var item = (sender as DataGrid).SelectedItem;
            if (item != null)
            {
                lvData.SelectedItem = item;
            }
        }
        public DataTable ConvertToDataTable<T>(List<T> data)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
            {
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;
        }
    }
}
