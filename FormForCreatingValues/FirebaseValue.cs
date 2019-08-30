using System;

namespace FormForCreatingValues
{
    public class FirebaseValue
    {
        public int Id;
        public double Value;
        public DateTime Time;

        public FirebaseValue(int id, double value, DateTime time)
        {
            Id = id;
            Value = value;
            Time = time;
        }
    }
}