using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using VdO2013Core;
using VdO2013THCore;

namespace VdO2013TH
{
    public class ProgressItem : IProgressItem
    {
        #region Statics
        public static ProgressItem NewInformation(Double percentage, String description, String step = null)
        {
            return new ProgressItem(percentage, description, step, ProgressItemKind.Information);
        }
        public static ProgressItem NewWarning(Double percentage, String description, String step = null)
        {
            return new ProgressItem(percentage, description, step, ProgressItemKind.Warning);
        }
        public static ProgressItem NewError(Double percentage, String description, String step = null)
        {
            return new ProgressItem(percentage, description, step, ProgressItemKind.Error);
        }
        public static ProgressItem NewError(Double percentage, Exception error, String step = null)
        {
            return new ProgressItem(percentage, error.ToString(), step, ProgressItemKind.Error) { Error = error };
        }
        #endregion Statics

        private DateTime _timeStamp;
        private Double _percentage;
        private String _step;
        private String _description;
        private Exception _error;
        private ProgressItemKind _kind;
        private ProgressItemStatus _status;

        public ProgressItem(Double percentage = 0, String description = null, String step = null, ProgressItemKind kind = ProgressItemKind.Information)
        {
            _timeStamp = DateTime.Now;
            _kind = kind;
            _status = ProgressItemStatus.New;
            if (!String.IsNullOrEmpty(step)) Step = step;
            if (!String.IsNullOrEmpty(description)) Description = description;
            Percentage = percentage;
        }

        public ProgressItemKind Kind { get { return _kind; } private set { _kind = value; } }
        public ProgressItemStatus Status { get { return _status; } set { _status = value; } }
        public DateTime TimeStamp { get { return _timeStamp; } }
        public String Step
        {
            get { return _step ?? String.Empty; }
            internal set { _step = value; }
        }
        public String Description
        {
            get { return _description ?? String.Empty; }
            internal set { _description = value; }
        }
        public Double Percentage
        {
            get { return _percentage; }
            private set { _percentage = value; }
        }
        public Exception Error
        {
            get { return _error; }
            private set
            {
                _error = value;
                if (_error != null)
                {
                    _kind = ProgressItemKind.Error;
                    _description = _error.ToString();
                }//if (_error != null)

            }
        }
        public Object Tag { get; set; }
        public Boolean IsError { get { return Error != null; } }
        public override String ToString()
        {
            return String.Format("{0}\t{1}\t{2}\t{3}", TimeStamp.ToString("yyyyMMdd hhmmss"), Step, Description, IsError ? "Error" : "Info");
        }
        public void ConsoleWriteLine() { Console.WriteLine(ToString()); }
    }
}
