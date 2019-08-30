using System;

namespace WebAppFixed.Models
{
    public class Value
    {
        public int Id { get; set; }      // Id значения
        public double LaboratoryValue { get; set; }
        public double SensorValue { get; set; }
        public DateTime Time { get; set; } 

        public Value(double laboratoryValue, double sensorValue, DateTime time)
        {
            LaboratoryValue = laboratoryValue;
            SensorValue = sensorValue;
            Time = time;
        }

        public Value()
        {}
    }
}