using System;
using System.IO;
using System.Linq;
using System.Text;

namespace WebAppFixed.Models
{
    public class DownloadFile
    {
        public static void Create(string start, string end, string path)
        {

            ValuesContext dbV = new ValuesContext();
            DateTime startTime, endTime;
            startTime = Convert.ToDateTime(start);
            endTime = Convert.ToDateTime(end);

            Value startValue = dbV.Values.Where(v => v.Time >= startTime).FirstOrDefault(); 
            Value endValue = dbV.Values.Where(v => v.Time >= endTime).FirstOrDefault(); 
            if (endValue == null)
            {
                int id = dbV.Values.Max(v => v.Id);
                endValue = dbV.Values.Find(id);
            }

            Dot[] dots = new Dot[endValue.Id - startValue.Id + 1]; // Массив объектов из БД
            int i = 0;

            for (int id = startValue.Id; id <= endValue.Id; id++)
            {
                var currentValue = dbV.Values.Find(id);
                if (currentValue == null)
                    break;

                Dot dot = new Dot(currentValue.Id, currentValue.LaboratoryValue, currentValue.SensorValue, currentValue.Time); // Создаёт объект с параметрами из БД
                dots[i] = dot;
                i++;
            }

            using (FileStream fstream = new FileStream(path, FileMode.OpenOrCreate))
            {
                for (i = 0; i < dots.Length; i++)
                {
                    byte[] array = Encoding.Default.GetBytes
                        ($@"L_{dots[i].LaboratoryValue} S_{dots[i].SensorValue} T_{dots[i].Time}.{dots[i].Time.Millisecond} L-S:{dots[i].LaboratoryValue - dots[i].SensorValue}" + "\n\n");
                    fstream.Write(array, 0, array.Length);
                }
            }
        }
    }
}
