using System;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using JPMorrow.Revit.Documents;
using JPMorrow.Tools.Diagnostics;
using JPMorrow.UI.Views;
using System.Diagnostics;
using JPMorrow.Revit.ConduitRuns;

namespace MainApp
{
	/// <summary>
	/// Main Execution
	/// </summary>
	[Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
	[Autodesk.Revit.DB.Macros.AddInId("16824613-93A7-4981-B2CF-27AB6E52280A")]
    public partial class ThisApplication : IExternalCommand
    {

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
			string[] dataDirectories = new string[0];

			//set revit model info
			bool debugApp = false;
			ModelInfo revit_info = ModelInfo.StoreDocuments(commandData, dataDirectories, debugApp);
			IntPtr main_rvt_wind = Process.GetCurrentProcess().MainWindowHandle;

			ConduitParamUpdate.RegisterConduitParameterUpdate(revit_info);
			ConduitParamUpdate.ToggleConduitParameterUpdaterSignUp();

			try
			{
				ParentView pv = new ParentView(revit_info, main_rvt_wind);
				pv.Show();
			}
			catch(Exception ex)
			{
				debugger.show(err:ex.ToString());
			}

			return Result.Succeeded;
        }
    }
}