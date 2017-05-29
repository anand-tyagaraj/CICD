using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.TeamFoundation.WorkItemTracking.Client;

namespace TFSWrapper.Model
{
    public class WorkItemInfo : ViewModelBase
    {
        private bool _Select;

        public bool Select
        {
            get { return _Select; }
            set
            {
                _Select = value;
                OnPropertyChanged(nameof(Select));
            }
        }

        private int _Id;

        public int Id
        {
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

        private RevisionCollection _Revisions;
        public RevisionCollection Revisions
        {
            get { return _Revisions; }
            set
            {
                _Revisions = value;
                OnPropertyChanged(nameof(Revisions));
            }
        }

        private string _workItemType;

        public string WorkItemType
        {
            get { return _workItemType; }
            set
            {
                _workItemType = value;
                OnPropertyChanged(nameof(WorkItemType));
            }
        }

        private string _workItemTitle;

        public string WorkItemTitle
        {
            get { return _workItemTitle; }
            set
            {
                _workItemTitle = value;
                OnPropertyChanged(nameof(WorkItemTitle));
            }
        }

        private string _desc;

        public string Desc
        {
            get { return _desc; }
            set
            {
                _desc = value;
                OnPropertyChanged(nameof(Desc));
            }
        }

        private string _assignedTo;

        public string AssignedTo
        {
            get { return _assignedTo; }
            set
            {
                _assignedTo = value;
                OnPropertyChanged(nameof(AssignedTo));
            }
        }

        private string _createdBy;

        public string CreatedBy
        {
            get { return _createdBy; }
            set
            {
                _createdBy = value;
                OnPropertyChanged(nameof(CreatedBy));
            }
        }

        private string _state;

        public string State
        {
            get { return _state; }
            set
            {
                _state = value;
                OnPropertyChanged(nameof(State));
            }
        }
    }

    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class WorkItemViewModel : ViewModelBase
    {
        ObservableCollection<WorkItemInfo> infos;

        public WorkItemViewModel()
        {
        }

        public ObservableCollection<WorkItemInfo> WorkItemInfos
        {
            get { return infos; }
            set
            {
                infos = value;
                OnPropertyChanged(nameof(WorkItemInfos));
            }
        }
    }
}