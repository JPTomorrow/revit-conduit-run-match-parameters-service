using System.Windows.Input;
using System.Windows;
using JPMorrow.Revit.Documents;
using JPMorrow.Views.RelayCmd;

namespace JPMorrow.UI.ViewModels
{

	public partial class ParentViewModel : Presenter
    {
        public void Update(string val) => RaisePropertyChanged(val);

        private static ModelInfo Info { get; set; }

        public bool Phase_Nuet_Checked {get; set;} = false;

        public string Header_Txt { get; set; }
        private string dtxt;
        public string Disable_Txt { get => dtxt; set { dtxt = value; RaisePropertyChanged("Disable_Txt"); } }
        public string[] DisableSwitch = new[] { "Disable", "Enable" };

        public ICommand MasterCloseCmd => new RelayCommand<Window>(MasterClose);
        public ICommand DisableEnableCmd => new RelayCommand<Window>(DisableEnable);

        public ParentViewModel(ModelInfo info)
        {
            //revit documents and pre converted elements
            Info = info;

            Header_Txt = "This is a running service that detects when you change a to/from parameter in your conduit and pushes it to all conduit in the run.";
            dtxt = DisableSwitch[0];

            RaisePropertyChanged("Panel_Phase_Items");
        }
    }
}