using System;
using System.Linq;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace WebAppFixed.Models
{
    public class Dot 
    {
        [DataMember]
        public int Id { get; set; }      // Id значения
        [DataMember]
        public double LaboratoryValue { get; set; }
        [DataMember]
        public double SensorValue { get; set; }
        [DataMember]
        public DateTime Time { get; set; } // Дата создания значений

        public Dot(int id, double laboratoryValue, double sensorValue, DateTime time)
        {
            Id = id;
            LaboratoryValue = laboratoryValue;
            SensorValue = sensorValue;
            Time = time;
        }
        public static string Creator(string start, string end)
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
            if (startValue == null)
                startValue = endValue;

            Dot[] dots = new Dot[0];
            if (endValue.Id - startValue.Id > 0)
            {
                int i = 0;
                dots = new Dot[endValue.Id - startValue.Id + 1];                  // Массив объектов из БД

                for (int id = startValue.Id; id <= endValue.Id; id++)
                {
                    var currentValue = dbV.Values.Find(id);
                    // Создаёт объект с параметрами из БД
                    Dot dot = new Dot(currentValue.Id, currentValue.LaboratoryValue, currentValue.SensorValue, currentValue.Time); 
                    dots[i] = dot;
                    i++;
                }
            }

            return JsonConvert.SerializeObject(dots);
        }
    }
}
