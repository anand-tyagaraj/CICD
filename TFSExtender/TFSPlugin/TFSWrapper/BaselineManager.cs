using System.Collections.Generic;
using System.Collections.ObjectModel;
using TFSWrapper.DB;
using TFSWrapper.Model;

namespace TFSWrapper
{
    public class BaselineManager
    {
        private BaselineInfoViewModel _baselineInfoViewModel;

        public BaselineInfoViewModel BaselineInfoViewModel {
            get { return _baselineInfoViewModel; }
        }
        private BaselineDBWrapper _baselineDBWrapper;

        public BaselineManager(BaselineDBWrapper baselineDBWrapper)
        {
            _baselineDBWrapper = baselineDBWrapper;
            _baselineInfoViewModel = new BaselineInfoViewModel();

            _baselineInfoViewModel.Baselines = new ObservableCollection<BaselineInfo>(_baselineDBWrapper.GetAllBaselineInfos());
        }
        
        
    }
}