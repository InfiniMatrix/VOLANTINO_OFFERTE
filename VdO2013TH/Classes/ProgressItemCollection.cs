using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using VdO2013Core;
using VdO2013THCore;

namespace VdO2013TH
{
    public class ProgressItemCollection : IProgressItemCollection
    {
        private String _job;

        private List<IProgressItem> _progressInfos;

        public String JobName { get { return _job; } set { _job = value; } }

        #region ProgressInfos
        public List<IProgressItem> Items { get { return _progressInfos; } }
        public int Count { get { return _progressInfos.Count; } }
        public IProgressItem LastItem { get { return _progressInfos.Count > 0 ? _progressInfos[Count - 1] : null; } }

        public IProgressItem this[int index] { get { return index >= 0 && _progressInfos.Count > index ? _progressInfos[index] : null; } }
        #endregion ProgressInfos

        #region Infos
        public IEnumerable<IProgressItem> Infos
        {
            get
            {
                var infos = from e in _progressInfos where e.Kind == ProgressItemKind.Information select e;
                return infos;
            }
        }
        //public IProgressItem LastInfo { get { return Infos.Count() > 0 ? Infos.Last() : null; } }
        #endregion Infos

        #region Errors
        public IEnumerable<IProgressItem> Errors
        {
            get
            {
                var errors = from e in _progressInfos where e.Kind == ProgressItemKind.Error select e;
                return errors;
            }
        }
        //public IProgressItem LastError { get { return Errors.Count() > 0 ? Errors.Last() : null; } }
        #endregion Errors

        #region Warnings
        public IEnumerable<IProgressItem> Warnings
        {
            get
            {
                var warnings = from e in _progressInfos where e.Kind == ProgressItemKind.Warning select e;
                return warnings;
            }
        }
        //public IProgressItem LastWarning { get { return Warnings.Count() > 0 ? Warnings.Last() : null; } }
        #endregion Warnings

        public Boolean Cancel { get; set; }

        public ProgressItemCollection(String job)
        {
            _job = job;
            _progressInfos = new List<IProgressItem>() { ProgressItem.NewInformation(0.0, "Initialization", "Initialization") };
            Cancel = false;
        }

        public event ProgressItemAddDelegate OnAddInfo;

        public int AddItem(IProgressItem info)
        {
            if (info == null) return -1;
            _progressInfos.Add(info);

            OnAddInfo?.Invoke(new ProgressInfoAddEventArgs(info));

            return _progressInfos.IndexOf(info);
        }
        public int AddItem(ProgressItem info)
        {
            if (info == null) return -1;
            _progressInfos.Add(info);

            OnAddInfo?.Invoke(new ProgressInfoAddEventArgs(info));

            return _progressInfos.IndexOf(info);
        }

        public int AddInfo(Double percentage, String description, String step = null)
        {
            step = !String.IsNullOrEmpty(step) ? step : LastItem.Step;
            return AddItem(ProgressItem.NewInformation(percentage, description, step));
        }
        public int AddInfo(String description, String step = null)
        {
            return AddInfo(Percentage, description, step);
        }

        public int AddWarning(Double percentage, String description, String step = null)
        {
            step = !String.IsNullOrEmpty(step) ? step : LastItem.Step;
            return AddItem(ProgressItem.NewWarning(percentage, description, step));
        }
        public int AddWarning(String description, String step = null)
        {
            return AddWarning(Percentage, description, step);
        }

        public int AddError(Double percentage, Exception error)
        {
            if (error != null)
                return AddItem(ProgressItem.NewError(Percentage, error));
            return -1;
        }
        public int AddError(Exception error)
        {
            return AddError(Percentage, error);
        }
        public int AddError(Double percentage, String message, Exception innerException = null)
        {
            if (!String.IsNullOrEmpty(message))
                return AddError(percentage, new Exception(message, innerException));
            return -1;
        }
        public int AddError(String message, Exception innerException = null)
        {
            return AddError(Percentage, message, innerException);
        }

        public void Reset(Boolean clearInfos = true, Boolean clearErrors = true, Boolean removeJob = false)
        {
            if (removeJob) _job = String.Empty;
            if (clearInfos) _progressInfos.Clear();
            Cancel = false;
        }

        public Double Percentage { get { return _progressInfos.Count >= 0 ? LastItem.Percentage : 0; } }
        public String Description { get { return _progressInfos.Count >= 0 ? LastItem.Description : null; } }
        public String Step { get { return _progressInfos.Count >= 0 ? LastItem.Step : null; } }
        public ProgressItemKind Kind { get { return _progressInfos.Count >= 0 ? LastItem.Kind : ProgressItemKind.None; } }
        public ProgressItemStatus Status { get { return _progressInfos.Count >= 0 ? LastItem.Status : ProgressItemStatus.None; } }
    }
}
