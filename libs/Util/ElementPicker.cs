using System.Collections.Generic;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using JPMorrow.Revit.Documents;

namespace JPMorrow.Revit.RevitPicker
{
	public static class RvtPicker
	{
		public static IEnumerable<XYZ> PickPoints(ModelInfo info, ObjectSnapTypes snap, int itr)
		{
			List<XYZ> ret_pts = new List<XYZ>();
			XYZ pt;

			for(var i = 0; i < itr; i++)
			{
				pt = info.SEL.PickPoint(snap);
				ret_pts.Add(pt);
			}

			return ret_pts;
		}
	}
}