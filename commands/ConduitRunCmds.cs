using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Plumbing;
using JPMorrow.Revit.ConduitRuns;
using JPMorrow.Revit.ElementCollection;
using JPMorrow.Revit.Wires;
using JPMorrow.Tools.Diagnostics;

namespace JPMorrow.UI.ViewModels
{
	public partial class ParentViewModel
    {
        public async void DisableEnable(Window window) {
            try {
                Disable_Txt = Disable_Txt.Equals("Disable") ? DisableSwitch[1] : DisableSwitch[0];
                await ConduitParamUpdate.ToggleUpdater(Info);
            }
            catch(Exception ex) {
                debugger.show(err:ex.ToString());
            }
        }

        /*
        public void AddAllRuns(Window window)
        {
            try
            {
                List<ElementId> collected_els = ElementCollector.CollectElements(Info, BuiltInCategory.OST_Conduit, false, "BYPASS").Element_Ids.ToList();

                FilteredElementCollector coll = new FilteredElementCollector(Info.DOC, Info.DOC.ActiveView.Id);
                collected_els.AddRange(coll.OfClass(typeof(FlexPipe)).ToElementIds().ToList());

                if(!collected_els.Any())
                {
                    debugger.show(err: "No conduit in view to process.");
                    return;
                }

                appData.Cris.Clear();
				ConduitRunInfo.ProcessCRIFromConduitId(Info, collected_els, appData.Cris);
                WriteToLog("Runs Processed: " + appData.Cris.Count().ToString());
                RefreshDataGrids(true, true, true, true, true, true, true, true);
            }
            catch(Exception ex)
            {
                debugger.show(err: ex.ToString());
            }
        }

        // add a single distribution run
        public void AddSingeRun(Window window)
        {
            try
            {
                var ids = Info.SEL.GetElementIds();
                ids = ids.ToList().FindAll(x => Info.DOC.GetElement(x).Category.Name.Equals("Conduits"));
                if(!ids.Any())
                {
                    WriteToLog("No conduit is selected");
                }

                ConduitRunInfo.ProcessCRIFromConduitId(Info, new ElementId[] { ids.First() }, appData.Cris);
                WriteToLog("Runs Processed: " + appData.Cris.Count().ToString());
                RefreshDataGrids(true, true, true, true, true, true, true, true);
            }
            catch(Exception ex)
            {
                debugger.show(err: ex.ToString());
            }
        }

        /// <summary>
        /// Remove selected runs from the run table
        /// </summary>
        public void RemoveSelectedRuns(Window window)
        {
            var presenters = Run_Items.Where(x => x.IsSelected).ToList();

            Selected_Run_Items.Clear();
            RaisePropertyChanged("Selected_Run_Items");

            if(!presenters.Any()) return;

            presenters.ForEach(x => Run_Items.Remove(x));
            var runs = presenters.Select(x => x.Value).ToList();
            runs.ForEach(x => appData.Cris.Remove(x));

            // clear wire
            runs.ForEach(x => appData.WireManager.RemoveWires(x.WireIds));

            RefreshDataGrids(false, true, false, false, false, false, true, false);
        }

        /// <summary>
        /// Remove selected runs from the run table
        /// </summary>
        public void SelectRun(Window window)
        {
            var presenters = Run_Items.Where(x => x.IsSelected);

            Selected_Run_Items.Clear();
            presenters.ToList().ForEach(x => Selected_Run_Items.Add(x));
            RaisePropertyChanged("Selected_Run_Items");

            List<ElementId> selected_runs = new List<ElementId>();

            presenters
                .Select(x => x.Value)
                .ToList().ForEach(x => x.WireIds.ToArray()
                .ToList().ForEach(z => selected_runs
                .Add(new ElementId(z))));

            if(!selected_runs.Any()) return;
            Info.SEL.SetElementIds(selected_runs);

            List<Wire> wires = new List<Wire>();
            Run_Items.Where(x => x.IsSelected).ToList().ForEach(x =>
                wires.AddRange(appData.WireManager.GetWires(x.Value.WireIds)));

            Wire_Items.Clear();
            wires.ForEach(x => Wire_Items.Add(new WirePresenter(x, Info)));

            RefreshDataGrids(false, false, false, false, false, false, true, false);
        }
        */
	}
}