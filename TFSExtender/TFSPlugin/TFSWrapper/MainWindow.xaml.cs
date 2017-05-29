using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using TFSWrapper.DB;
using TFSWrapper.Model;

namespace TFSWrapper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BaselineDBWrapper _baselineDBWrapper;

        private TFSWorkItemManager _tfsWorkItemManager;

        private BaselineManager _baselineManager;

        public MainWindow()
        {
            InitializeComponent();

            txtProjectCollectionUrl.Text = "http://ie3blt2x8src2:8080/tfs/DefaultCollection";

            _baselineDBWrapper = new BaselineDBWrapper();

            _tfsWorkItemManager = new TFSWorkItemManager();

            HandleViewsOnLoad();

            _baselineManager = new BaselineManager(_baselineDBWrapper);

            LoadBaselines();
        }

        private void LoadBaselines()
        {
            cmbBaselines.DataContext = _baselineManager.BaselineInfoViewModel.Baselines;
        }

        private void HandleViewsOnLoad()
        {
            stkViewBaseline.Visibility = Visibility.Visible;
            stkCreateBaseline.Visibility = Visibility.Collapsed;
            stkWorkItems.Visibility = Visibility.Visible;
            stkBaselinedWorkItems.Visibility = Visibility.Collapsed;
        }

        private void Go_OnClick(object sender, RoutedEventArgs e)
        {
            DataContext = _tfsWorkItemManager.GetAllWorkItems();
        }

        private void CreateBaseline_OnClick(object sender, RoutedEventArgs e)
        {
            //Get selected work items

            if (txtBaselineName.Text.Length < 1)
            {
                MessageBox.Show("Please enter Baseline Name", "Baseline", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var workItems = (ObservableCollection<WorkItemInfo>)dataGrid.DataContext;

            var selectedItems = workItems.Where(o => o.Select);
            

            //Call Insert to insert into SQL
            BaselineInfo baselineInfo = new BaselineInfo()
            {
                CreatedBy = "User",
                CreatedDate = DateTime.Now,
                BaselineName = txtBaselineName.Text,
                BaselineId = new Random().Next(1, 10000)
            };


            _baselineDBWrapper.InsertRecords(baselineInfo, selectedItems.ToList());

            MessageBox.Show("Baseline Created", "Baseline", MessageBoxButton.OK, MessageBoxImage.Information);
            _baselineManager.BaselineInfoViewModel.Baselines.Add(baselineInfo);

            foreach (var workItemInfo in _tfsWorkItemManager.WorkItemViewModel.WorkItemInfos)
            {
                workItemInfo.Select = false;
            }

            stkViewBaseline.Visibility = Visibility.Visible;
            stkCreateBaseline.Visibility = Visibility.Collapsed;
            stkWorkItems.Visibility = Visibility.Visible;
            stkBaselinedWorkItems.Visibility = Visibility.Collapsed;
        }

        private void CreateNewBaseline_OnClick(object sender, RoutedEventArgs e)
        {
            stkViewBaseline.Visibility = Visibility.Collapsed;
            stkCreateBaseline.Visibility = Visibility.Visible;
            stkWorkItems.Visibility = Visibility.Visible;
            stkBaselinedWorkItems.Visibility = Visibility.Collapsed;
        }


        private void CmbBaselines_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            stkViewBaseline.Visibility = Visibility.Visible;
            stkCreateBaseline.Visibility = Visibility.Collapsed;
            stkWorkItems.Visibility = Visibility.Collapsed;
            stkBaselinedWorkItems.Visibility = Visibility.Visible;

            var baselineInfo = (BaselineInfo)cmbBaselines.SelectedItem;

            lblBaselineName.Text = baselineInfo.BaselineName;
            lblCreatedBy.Text = baselineInfo.CreatedBy;
            lblCreatedDate.Text = baselineInfo.CreatedDate.ToString();

            var lstWorkItems = _baselineDBWrapper.GetWorkItemsPerBaselineInfos(baselineInfo.BaselineId);

            foreach (var workItem in lstWorkItems)
            {
                var containsElement =
                    _tfsWorkItemManager.WorkItemViewModel.WorkItemInfos.ToList()
                        .Exists(i => i.Id == workItem.Id && i.Revision == workItem.Revision);

                WorkItemInfo item = null;

                if (containsElement == false)
                {
                    var revItem = _tfsWorkItemManager.WorkItemViewModel.WorkItemInfos.First(
                        i => i.Id == workItem.Id);

                    foreach (Revision revItemRevision in revItem.Revisions)
                    {
                        if ((int) revItemRevision.Fields["Rev"].Value == workItem.Revision)
                        {
                            item = new WorkItemInfo()
                            {
                                Revision = workItem.Revision,
                                Id = revItemRevision.WorkItem.Id,
                                CreatedBy = (string) revItemRevision.Fields["Created By"].Value,
                                Desc = (string) revItemRevision.Fields["Description"].Value,
                                State = (string) revItemRevision.Fields["State"].Value,
                                WorkItemTitle = (string) revItemRevision.Fields["Title"].Value,
                                WorkItemType = (string) revItemRevision.Fields["Work Item Type"].Value,
                                AssignedTo = (string) revItemRevision.Fields["Assigned To"].Value
                            };
                        }
                    }
                }
                else
                {
                    item = _tfsWorkItemManager.WorkItemViewModel.WorkItemInfos.First(
                    i => i.Id == workItem.Id && i.Revision == workItem.Revision);
                }

                if (item != null)
                {
                    workItem.WorkItemType = item.WorkItemType;
                    workItem.WorkItemTitle = item.WorkItemTitle;
                    workItem.Desc = item.Desc;
                    workItem.AssignedTo = item.AssignedTo;
                    workItem.CreatedBy = item.CreatedBy;
                    workItem.State = item.State;

                }
            }
            //_tfsWorkItemManager.WorkItemViewModel.WorkItemInfos.First(i=>i.Id ==)

            dgBaselinedWorkItems.DataContext = lstWorkItems;
        }

        private void ViewBaseline_OnClick(object sender, RoutedEventArgs e)
        {
            HandleViewsOnLoad();
        }
    }
}
