using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace DiaryApp
{
    /// <summary>
    /// Описание некоторых глобальных переменных
    /// </summary>
    public partial class App : Application
    {
        static string dataBaseName = "Diary.db";
        public static string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        public static string dataBasePath = System.IO.Path.Combine(folderPath, dataBaseName);
        static string fileName = "SaveFile.txt";
        public static string fileSaveLocation = System.IO.Path.Combine(folderPath, fileName);
    }
}
