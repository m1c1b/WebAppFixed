using System;

namespace FormForCreatingValues
{
    public class Value
    {
        public int Id { get; set; }            // Id значения
        public double LaboratoryValue { get; set; } 
        public double SensorValue { get; set; }
        public DateTime Time { get; set; }     // Время создания объекта

        public Value(int id, double laboratoryValue, double sensorValue, DateTime time)    
        {
            Id = id;
            LaboratoryValue = laboratoryValue;
            SensorValue = sensorValue;
            Time = time;
        }
    }
}