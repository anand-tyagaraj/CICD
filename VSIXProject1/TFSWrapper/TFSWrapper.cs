using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.WorkItemTracking.Client;

namespace TFSWrapper
{
    public class TFSWrapper
    {
        public void GetAllWorkItems()
        {
            //_WorkItemViewModel.WorkItemInfos = new ObservableCollection<WorkItemInfo>();
            TfsTeamProjectCollection prjCollection = new TfsTeamProjectCollection(new Uri("http://ie3blt2x8src2:8080/tfs/DefaultCollection"));

            WorkItemStore workItemStore = prjCollection.GetService<WorkItemStore>();

            Query query = new Query(workItemStore, "SELECT * FROM WorkItems WHERE [System.TeamProject] = @project", new Dictionary<string, string>() { { "project", "TFSBaseline" } });

            WorkItemCollection wic = query.RunQuery();





            //foreach (WorkItem item in wic)
            //{
            //    _WorkItemViewModel.WorkItemInfos.Add(new WorkItemInfo()
            //    {
            //        Id = item.Id,
            //        Revision = item.Revision,
            //        WorkItemType = item.Type.ToString(),
            //        Desc = item.Description,
            //        AssignedTo = item.CreatedBy,
            //        State = item.State
            //    });
            //}

            //return _WorkItemViewModel.WorkItemInfos;
        }

    }
}
