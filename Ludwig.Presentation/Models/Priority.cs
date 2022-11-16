using System;
using System.Collections.Generic;

namespace Ludwig.Presentation.Models
{
    public class Priority
    {
        public static Priority Highest { get; } = new Priority("Highest", 0);
        public static Priority High { get; } = new Priority("High", 1);
        public static Priority Medium { get; } = new Priority("Medium", 2);
        public static Priority Low { get; } = new Priority("Low", 3);
        public static Priority Lowest { get; } = new Priority("Lowest", 4);

        public static List<Priority> Priorities = new List<Priority>
        {
            Highest,High,Medium,Low,Lowest
        };
        
        private Priority(string name, int value)
        {
            Name = name;
            Value = value;
        }

        public Priority()
        {
            
        }
        
        public string Name { get; set; }
        
        public int Value { get;  set;}


        public static implicit operator string(Priority p)
        {
            return p.Name;
        }
        public static implicit operator int(Priority p)
        {
            return p.Value;
        }

        public override bool Equals(object obj)
        {
            if (obj != null && obj is Priority p)
            {
                return p.Value == this.Value && p.Name?.ToLower() == this.Name?.ToLower();
            }

            return false;
        }

        protected bool Equals(Priority other)
        {
            return other.Value == this.Value && other.Name?.ToLower() == this.Name?.ToLower();
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Value);
        }

        public static bool operator ==(Priority p1, Priority p2)
        {
            if (p1 is null && p2 is null)
            {
                return true;
            }

            if (p1 is null || p2 is null)
            {
                return false;
            }

            return p1.Name?.ToLower() == p2.Name?.ToLower() && p1.Value == p2.Value;
        }

        public static bool operator !=(Priority p1, Priority p2)
        {
            return !(p1 == p2);
        }
    }
}