using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reto5
{

    public enum DurationUnid
    {
        Day,
        Week,
        Month
    }
    public struct Duration
    {
        private DurationUnid unid;
        private int value;
        public DurationUnid Unid 
        { 
            get
            {
                return unid;
            }
            set
            {
                unid = value;
            }
        }
        public int Value 
        { 
            get
            {
                return this.value;
            }
            set
            {
                this.value = value; 
            }
        }

        public Duration(DurationUnid unid, int value)
        {
            this.unid = unid;
            this.value = value;
        }
        public static Duration Day = new Duration(DurationUnid.Day, 1);

        public DateTime From(DateTime dateTime)
        {
            switch (Unid)
	        {
		        case DurationUnid.Day:
                    return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day + Value);
                case DurationUnid.Week:
                    return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day + Value * 7);
                case DurationUnid.Month:
                    return new DateTime(dateTime.Year, dateTime.Month + Value, dateTime.Day);
                default:
                 return dateTime;
	        }
        }

        public static Duration Week = new Duration(DurationUnid.Week, 1);

        public static Duration Month = new Duration(DurationUnid.Month, 1);

        public static explicit operator Duration(int i)
        {
            return new Duration(DurationUnid.Month, i);
        }
    }
}
