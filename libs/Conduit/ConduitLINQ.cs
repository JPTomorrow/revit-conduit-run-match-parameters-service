using Autodesk.Revit.DB;
using System;
using JPMorrow.Revit.Documents;
using JPMorrow.Revit.Tools;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB.Electrical;

public static class ConduitLINQ
{
	private static Exception NaConduit(Element conduit) => new Exception("This is not a piece of conduit: (ElementID) " + conduit.Id.ToString());

	public static double ConduitLength(this Element conduit)
	{
		if(!conduit.Category.Name.Equals("Conduits"))
			throw NaConduit(conduit);

		Curve conduit_curve = (conduit.Location as LocationCurve).Curve;
		return conduit_curve.Length;
	}

	public static Line GetConduitLine(this Conduit conduit, bool reversed = false)
	{
		Curve conduit_curve = (conduit.Location as LocationCurve).Curve;
		var endpoint1 = conduit_curve.GetEndPoint(reversed ? 1 : 0);
		var endpoint2 = conduit_curve.GetEndPoint(reversed ? 0 : 1);
		return Line.CreateBound(endpoint1, endpoint2);
	}
}