using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using JPMorrow.Revit.Documents;
using JPMorrow.Revit.ElementCollection;
using JPMorrow.Tools.Diagnostics;

namespace JPMorrow.Revit.ConduitRuns {
	public static class ConduitParamUpdate {
		private static ParameterUpdater _updater;


		public static void RegisterConduitParameterUpdate(ModelInfo info) {
			var id_str = "53051880-2218-4230-8062-141247057676";
			_updater = new ParameterUpdater(info, new Guid(id_str));


			if(!UpdaterRegistry.IsUpdaterRegistered(_updater.GetUpdaterId(), info.DOC))
				UpdaterRegistry.RegisterUpdater(_updater, info.DOC, true);

			var coll = new FilteredElementCollector(info.DOC, info.DOC.ActiveView.Id);
			var conduit = coll.OfCategory(BuiltInCategory.OST_Conduit).First();
			var from = conduit.LookupParameter("From");
			var to = conduit.LookupParameter("To");

			var category = Category.GetCategory(info.DOC, BuiltInCategory.OST_Conduit);
			FilterCategoryRule rule = new FilterCategoryRule(new[] { category.Id });
			ElementParameterFilter filter = new ElementParameterFilter(rule);

			try {
				UpdaterRegistry.AddTrigger(_updater.GetUpdaterId(), filter, Element.GetChangeTypeParameter(from.Id));
				UpdaterRegistry.AddTrigger(_updater.GetUpdaterId(), filter, Element.GetChangeTypeParameter(to.Id));
			}
			catch(Exception ex) {
				debugger.show(err:ex.ToString());
			}

			EnableUpdater();
		}

		private static void DisableUpdater() {
			UpdaterRegistry.DisableUpdater(_updater.GetUpdaterId());
		}

		private static void EnableUpdater() {
			UpdaterRegistry.EnableUpdater(_updater.GetUpdaterId());
		}


		public class ParameterUpdater : IUpdater
		{
			public ModelInfo Info { get; set; }
			UpdaterId _uid;

			public ParameterUpdater(ModelInfo info, Guid guid)
			{
				Info = info;
				_uid = new UpdaterId(info.UIAPP.ActiveAddInId, guid);
			}

			public void Execute(UpdaterData data)
			{
				var doc = data.GetDocument();
				var conduit = doc.GetElement(data.GetModifiedElementIds().First());

                if(conduit == null) return;

				List<ConduitRunInfo> cris = new List<ConduitRunInfo>();
				ConduitRunInfo.ProcessCRIFromConduitId(Info, new[] { conduit.Id }, cris);

				foreach(var cri in cris) {
					var from = conduit.LookupParameter("From").AsString();
					var to = conduit.LookupParameter("To").AsString();

					foreach(var id in cri.ConduitIds.Concat(cri.FittingIds)) {
						var el = doc.GetElement(new ElementId(id));
						el.LookupParameter("From").Set(from);
						el.LookupParameter("To").Set(to);
					}
				}
			}

			public string GetAdditionalInformation()
			{
				return "This is an updater that updates conduit run to/from parameter.";
			}

			public ChangePriority GetChangePriority()
			{
				return ChangePriority.FreeStandingComponents;
			}

			public UpdaterId GetUpdaterId()
			{
				return _uid;
			}

			public string GetUpdaterName()
			{
				return "ConduitRunParameterUpdater";
			}
		}

		public static ToggleConduitParameterUpdater handler_disable_enable_conduit_updater = null;
		public static ExternalEvent exEvent_disable_enable_conduit_updater = null;

		public static void ToggleConduitParameterUpdaterSignUp()
		{
			handler_disable_enable_conduit_updater = new ToggleConduitParameterUpdater();
			exEvent_disable_enable_conduit_updater = ExternalEvent.Create(handler_disable_enable_conduit_updater.Clone() as IExternalEventHandler);
		}

		public static async Task ToggleUpdater(ModelInfo info) {

			ClearHandler();

			handler_disable_enable_conduit_updater.Info = info;

			if(UpdaterRegistry.IsUpdaterEnabled(_updater.GetUpdaterId()))
				handler_disable_enable_conduit_updater.Disable = true;

			exEvent_disable_enable_conduit_updater.Raise();

			while(exEvent_disable_enable_conduit_updater.IsPending) {
				await Task.Delay(200);
			}
		}

		public static void ClearHandler() {
			handler_disable_enable_conduit_updater.Info = null;
			handler_disable_enable_conduit_updater.Disable = false;
		}

		/// <summary>
		/// Revit Event for placing a single hanger
		/// </summary>
		public class ToggleConduitParameterUpdater : IExternalEventHandler, ICloneable
		{
			public ModelInfo Info { get; set; }
			public bool Disable { get; set; } = false;

			public object Clone() {
				return this;
			}

			public void Execute(UIApplication app) {
				//update schedule
				using var tx = new Transaction(Info.DOC, "Disabling/Enabling Updater");

				tx.Start();

				if(Disable)
					DisableUpdater();
				else
					EnableUpdater();

				tx.Commit();
			}

			public string GetName()
			{
				return "Disabling/Enabling Updater";
			}
		}
	}
}