using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using TFSWrapper.Model;

namespace TFSWrapper
{
    public class TFSWorkItemManager
    {
        private WorkItemViewModel _WorkItemViewModel;

        public WorkItemViewModel WorkItemViewModel
        {
            get { return _WorkItemViewModel; }
        }

        public TFSWorkItemManager()
        {
            _WorkItemViewModel = new WorkItemViewModel();
            GetAllWorkItems();
        }

        public ObservableCollection<WorkItemInfo> GetAllWorkItems()
        {
            _WorkItemViewModel.WorkItemInfos = new ObservableCollection<WorkItemInfo>();
            TfsTeamProjectCollection prjCollection = new TfsTeamProjectCollection(new Uri("http://ie3blt2x8src2:8080/tfs/DefaultCollection"));

            WorkItemStore workItemStore = prjCollection.GetService<WorkItemStore>();

            Query query = new Query(workItemStore, "SELECT * FROM WorkItems WHERE [System.TeamProject] = @project", new Dictionary<string, string>() { { "project", "TFSBaseline" } });

            WorkItemCollection wic = query.RunQuery();

            foreach (WorkItem item in wic)
            {
                _WorkItemViewModel.WorkItemInfos.Add(new WorkItemInfo()
                {
                    Id = item.Id,
                    Revisions = item.Revisions,
                    Revision = item.Revision,
                    WorkItemType = item.Type.Name,
                    WorkItemTitle = item.Title,
                    Desc = item.Description,
                    CreatedBy = item.CreatedBy,
                    State = item.State,
                    AssignedTo = item["Assigned To"].ToString()
                });
            }

            return _WorkItemViewModel.WorkItemInfos;
        }
    }
}