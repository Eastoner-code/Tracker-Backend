using System;
using System.Globalization;

namespace TrackerApi
{
    public class RecordNotFoundException : Exception
    {
        public RecordNotFoundException() : base()
        {
        }
        public RecordNotFoundException(string message) : base(message)
        {
        }
        public RecordNotFoundException(string message, params object[] args) : base(string.Format(CultureInfo.CurrentCulture, message, args))
        {
        }
    }
}
