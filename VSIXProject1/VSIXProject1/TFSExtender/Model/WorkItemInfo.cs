using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VSIXProject1.Annotations;

namespace VSIXProject1.TFSExtender
{
    public class WorkItemInfo : ViewModelBase
    {
        private int _Id;
        public int Id {
            get { return _Id; }
            set
            {
                _Id = value;
                OnPropertyChanged(nameof(Id));
            }
        }

        private int _Revision;
        public int Revision
        {
            get { return _Revision; }
            set
            {
                _Revision = value;
                OnPropertyChanged(nameof(Revision));
            }
        }

        private string _workItemType;
        public string  WorkItemType {
            get { return _workItemType; }
            set
            {
                _workItemType = value; OnPropertyChanged(nameof(WorkItemType));}
        }

        private string _desc;
        public string Desc
        {
            get { return _desc; }
            set
            {
                _desc = value; OnPropertyChanged(nameof(Desc));
            }
        }

        private string _assignedTo;
        public string AssignedTo
        {
            get { return _assignedTo; }
            set
            {
                _assignedTo = value; OnPropertyChanged(nameof(AssignedTo));
            }
        }

        private string _state;
        public string State
        {
            get { return _state; }
            set
            {
                _state = value; OnPropertyChanged(nameof(State));
            }
        }
    }

    public class ViewModelBase :INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class WorkItemViewModel:ViewModelBase
    {
        ObservableCollection<WorkItemInfo> infos;
        ICommand _command;

        public WorkItemViewModel()
        {
            infos = new ObservableCollection<WorkItemInfo>();
            
        }

        public ObservableCollection<WorkItemInfo> WorkItemInfos
        {
            get
            {
                return infos;
            }
            set
            {
                infos = value;
                OnPropertyChanged(nameof(WorkItemInfos));
            }
        }
    }
}
