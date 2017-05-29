using System;
using System.Collections.ObjectModel;

namespace TFSWrapper.Model
{
    public class BaselineInfo : ViewModelBase
    {
        private int _BaselineId;
        public int BaselineId
        {
            get { return _BaselineId; }
            set
            {
                _BaselineId = value;
                OnPropertyChanged(nameof(BaselineId));
            }
        }

        private string _BaselineName;
        public string BaselineName
        {
            get { return _BaselineName; }
            set
            {
                _BaselineName = value;
                OnPropertyChanged(nameof(BaselineName));
            }
        }

        private DateTime _CreatedDate;

        public DateTime CreatedDate {
            get { return _CreatedDate; }
            set
            {
                _CreatedDate = value;
                OnPropertyChanged(nameof(CreatedDate));
            }
        }

        private string _CreatedBy;
        public string CreatedBy {
            get { return _CreatedBy; }
            set
            {
                _CreatedBy = value;
                OnPropertyChanged(nameof(CreatedBy));
            }
        }

    }

    public class BaselineInfoViewModel : ViewModelBase
    {
        private ObservableCollection<BaselineInfo> _Baselines;
        public ObservableCollection<BaselineInfo> Baselines
        {
            get { return _Baselines; }
            set
            {
                _Baselines = value;
                OnPropertyChanged(nameof(Baselines));
            }
        }
    }
}