using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace HW_20._05._23
{
    [Serializable]
    public class Album
    {
        public string Name { get; set; }
        public string Singer { get; set; }

        private int Year;
        public int Min { get; set; }
        public int Sec { get; set; }
        public string Studio { get; set; }
        public int _Year
        {
            get { return Year; }
            set {
                if (value < 1970 || value > DateTime.Now.Year) 
                { Console.WriteLine("\n\tВ дате выпуска альбома допущена ошибка. Введите дату от 1970 до текущего года");
                    Year= DateTime.Now.Year;
                }
                else
                  Year = value; 
            }
        }

        public Album()
        {
            
        }

        public Album(string _name, string _singer, int _year, int min, int sec, string _studio)
        {
            Name = _name;
            Singer=_singer;
            Year = _year;
            Min = min;
            Sec = sec;
            Studio = _studio;
        }

        public override string ToString()
        {
            return $"\n\tНаименование альбома: {Name}" +
                $"\n\tИсполнитель: {Singer}" +
                $"\n\tГод выпуска: {Year}" +
                $"\n\tДлительность: {Min} : {Sec}" +
                $"\n\tCтудия: {Studio}" ;
        }

        public void SaveToXml(string fileName)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Album));
            using (StreamWriter writer = new StreamWriter(fileName))
            {
                serializer.Serialize(writer, this);
            }
        }

        public static Album LoadFromXml(string fileName)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Album));
            using (StreamReader reader = new StreamReader(fileName))
            {
                return (Album)serializer.Deserialize(reader);
            }
        }
    }
    internal class Program
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        static void Main(string[] args)
        {
            Album album = new Album();
            int choice = 0;
            do
            {
                Console.WriteLine("\n\tВыберите действие:");
                Console.WriteLine("\n\t1 - Ввести данные про новый альбом");
                Console.WriteLine("\n\t2 - Изменить данные в существующем альбоме");
                Console.WriteLine("\n\t3 - Вывести информацию про альбом");
                Console.WriteLine("\n\t4 - Сериализовать альбом и записать в бинарный файл");
                Console.WriteLine("\n\t5 - Десериализовать альбом из бинарного файла");
                Console.WriteLine("\n\t6 - Cохранить данные об альбоме в XML-файл");
                Console.WriteLine("\n\t7 - Десериализовать данные из XML-файла");
                choice =Int32.Parse(Console.ReadLine());
                switch (choice)
                {
                    case 1:
                        Console.Write("\n\tВведите название альбома: ");
                        album.Name = Console.ReadLine();
                        Console.Write("\n\tВвеите имя испольнителя: ");
                        album.Singer = Console.ReadLine();
                        Console.Write("\n\tВведите год выпуска (от 1970 до текущего года): ");
                        album._Year=Int32.Parse (Console.ReadLine());
                        Console.Write("\n\tВведите длительность трэка: \n\tминуты: ");
                        album.Min=int.Parse(Console.ReadLine());
                        Console.Write("\n\tсекунды: ");
                        album.Sec=int.Parse(Console.ReadLine());
                        Console.Write("\n\tВведите студию: ");
                        album.Studio=Console.ReadLine();
                        break;
                    case 2:
                        Console.WriteLine("\n\t\tВыберите, какую графу в альбоме нужно изменить:" +
                            "\n\t\t1 - Название альбома;" +
                            "\n\t\t2 - Имя исполнителя; " +
                            "\n\t\t3 - Год выпуска" +
                            "\n\t\t 4- Длительность трэка" +
                            "\n\t\t 5 - Название студии ");
                        int choice2 = int.Parse(Console.ReadLine());
                        switch (choice2)
                        {
                            case 1:
                                Console.Write("\n\t\tВведите название альбома: ");
                                album.Name = Console.ReadLine();
                                break;
                            case 2:
                                Console.Write("\n\t\tВвеите имя испольнителя: ");
                                album.Singer = Console.ReadLine();
                                break;
                            case 3:
                                Console.Write("\n\t\tВведите год выпуска (от 1970 до текущего года): ");
                                album._Year = Int32.Parse(Console.ReadLine());
                                break;
                            case 4:
                                Console.Write("\n\t\tВведите длительность трэка: \n\tминуты: ");
                                album.Min = int.Parse(Console.ReadLine());
                                Console.Write("\n\t\tсекунды: ");
                                album.Sec = int.Parse(Console.ReadLine());
                                break;
                            case 5:
                                Console.Write("\n\t\tВведите студию: ");
                                album.Studio = Console.ReadLine();
                                break;
                            default:
                                Console.WriteLine("\n\t\tВы ввели неверную операцию, попробуйте снова.");
                                break;
                        }
                        break;
                    case 3:
                        Console.WriteLine(album.ToString());
                        break;
                    case 4:
                        Console.Write("\n\tВведите название файла для сохранения: ");
                        string path = Console.ReadLine() ?? "test";
                        if (path.Contains('.'))
                        {
                            string [] parts=path.Split('.');
                            path = parts[0];
                        }
                        using (FileStream fs = new FileStream(path, FileMode.Create))
                        {
                            BinaryFormatter bf = new BinaryFormatter();
                            bf.Serialize(fs, album);
                        }
                            break;
                    case 5:
                        Console.WriteLine("\n\tВведите путь к файлу для десериализации:");
                        string pathDes= Console.ReadLine();
                        if(File.Exists(pathDes))
                        {
                            using (FileStream fs = new FileStream(pathDes,FileMode.Open))
                            {
                                BinaryFormatter bf = new BinaryFormatter();
                                album=(Album)bf.Deserialize(fs);
                            }
                        }
                        else { Console.WriteLine("\n\tФайл не найден"); }
                        break;
                    case 6:
                        Console.WriteLine("\n\tВведите название файла для сохранения в ХML");
                        string pathXml = Console.ReadLine() ?? "test";
                        if (pathXml.Contains('.'))
                        {
                            string[] parts = pathXml.Split('.');
                            path = parts[0];
                        }
                        album.SaveToXml(pathXml + ".xml");
                        break;
                    case 7:
                        Console.WriteLine("\n\tВведите путь к файлу для десериализации:");
                        string pathDesXml = Console.ReadLine();
                        if (!pathDesXml.Contains(".xml"))
                        {
                            pathDesXml += ".xml";
                        }
                        if (File.Exists(pathDesXml))
                        {
                            album = Album.LoadFromXml(pathDesXml);
                        }
                        else
                        {
                            Console.WriteLine("\n\tФайл не найден");
                        }
                        break;

                }
            }
            while (choice != 0);


        }
    }
}
