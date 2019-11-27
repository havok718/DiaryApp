Сделал на WPF с использованием немножко SQLite. Полностью не успел разобраться, поэтому в коде постоянно происходит конвертация из БД в лист и обратно. Код постарался откомментировать максимально понятно. Некоторые условности:

файл БД будет сохраняться в папке "Мои документы" с именем Diary.db
При экспорте данных в файл будет создаваться текстовый файл в папке "Мои документы" с именем SaveFile.txt
Поля в текстовом файле будут разделены символом Ω. Это сделано для того, чтобы я мог считать данные и преобразовать обратно в нужную мне структуру. Если бы получше разобрался с SQLite, то экспорт/импорт сделал бы напрямую оттуда.
При импорте из файла программа будет выдавать сообщение об ошибке, если формат не будет соответствовать строке по-умолчанию. То есть если каких-то полей будет не хватать или знак разделения не Ω. Это опять же по причине того, что не успел разобраться с SQLite до конца.

WPF app with a bit of SQLite. Didn't have time to figure out everything about SQL querries, so there are a lot of list-to-DB conversions.
DB file Diary.db will be saved in "My Documents" folder.
When exporting to file a new file SaveFile.txt will be created in "My Documents" folder.
Ω will be a divider in generated text file. It was done so that i could read the file back and convert it into a correct struct. If i had more time to figure out SQLite, i would have done a direct DB export/import of data.
When importing data from file an error message will pop up if data format is different from defauilt string. For example, you will get an error if some fields are missing or the divider is not Ω. This happens because i didn't have time to figure out SQLite better.
