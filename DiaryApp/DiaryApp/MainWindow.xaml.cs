using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
    public partial class MainWindow : Window
    {
        List<DataGridInfo> dataInfo;
        public MainWindow()
        {
            InitializeComponent();
            dataInfo = new List<DataGridInfo>();
            ReadDataBase();
        }
        /// <summary>
        /// Кнопка, открывающая окно добавления новой записи
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAddString_Click(object sender, RoutedEventArgs e)
        {
            AddNewNoteWindow addNewNoteWindow = new AddNewNoteWindow();
            addNewNoteWindow.ShowDialog();
            ReadDataBase();
        }
        /// <summary>
        /// Метод для обновления значений из БД
        /// </summary>
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
        /// <summary>
        /// Метод для выполнения поиска записи по любому значению
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tbSearch = sender as TextBox;
            var filteredList = dataInfo.Where(c => c.Body.ToLower().Contains(tbSearch.Text.ToLower()) || c.Date.Contains(tbSearch.Text.ToLower()) 
                                                || c.Importance.ToLower().Contains(tbSearch.Text.ToLower()) || c.Signature.ToLower().Contains(tbSearch.Text.ToLower()) ||
                                                c.Location.ToLower().Contains(tbSearch.Text.ToLower())).ToList();
            lvData.ItemsSource = filteredList;
        }
        /// <summary>
        /// Кнопка для удаления выделенной записи из БД
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <summary>
        /// Кнопка, которая открывает окно редактирования выделенной записи
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnEditString_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataGridInfo selectedNote = (DataGridInfo)lvData.SelectedItem;
                if (selectedNote != null)
                {
                    UpdateDeleteNoteWindow updateDeleteNoteWindow = new UpdateDeleteNoteWindow(selectedNote);
                    updateDeleteNoteWindow.ShowDialog();
                    ReadDataBase();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Выберите значение! " + ex);
            }
        }
        /// <summary>
        /// Логика, позволяющая при выделении элемента в DataGrid выделить тот же самый элемент в ListView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgData_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var item = (sender as DataGrid).SelectedItem;
            if (item != null)
            {
                lvData.SelectedItem = item;
            }
        }
        /// <summary>
        /// Кнопка, выполняющая экспорт данных в текстовый файл SaveFile.txt. Файл будет находится в папке "Мои документы"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnExportToFile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (TextWriter tw = new StreamWriter(App.fileSaveLocation))
                {
                    foreach (var s in dataInfo)
                    {
                        tw.WriteLine(s);
                    }
                }
                MessageBox.Show("Файл экспортирован успешно.");
            }
            catch(Exception ex)
            {
                MessageBox.Show("Во время экспорта произошла ошибка " + ex);
            }
        }
        /// <summary>
        /// Кнопка, добавляющая в БД элементы из ранее сохраненного файла
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAddFromFile_Click(object sender, RoutedEventArgs e)
        {
            //// TODO добавить возможность выбора файла на диске
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
                        fromFile.Add(import);
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Ошибка при импорте из файла. Проверьте формат данных в файле. " + ex);
            }

            foreach (DataGridInfo item in fromFile)
            {
                using (SQLiteConnection connection = new SQLiteConnection(App.dataBasePath))
                {
                    connection.CreateTable<DataGridInfo>();
                    connection.Insert(item);
                }
            }
            ReadDataBase();
        }
        /// <summary>
        /// Кнопка, которая загружает данные из файла, при этом удаляет старые данные из БД
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLoadFromFile_Click(object sender, RoutedEventArgs e)
        {
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
                using (SQLiteConnection connection = new SQLiteConnection(App.dataBasePath))
                {
                    connection.CreateTable<DataGridInfo>();
                    connection.DeleteAll<DataGridInfo>();
                    connection.Insert(item);
                }
            }
            ReadDataBase();
        }
        /// <summary>
        /// Кнопка открывает окно для добавления записей по диапазону дат
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnImportByDateRange_Click(object sender, RoutedEventArgs e)
        {
            ImportByDateRange importByDateRange = new ImportByDateRange();
            importByDateRange.ShowDialog();
            ReadDataBase();
        }
    }
}
