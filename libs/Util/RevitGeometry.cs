using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace JPMorrow.Revit.Tools
{


	public static class RGeo
	{
		internal static class PrimitiveDirection
		{
			public static XYZ Up { get; } = new XYZ(0, 0, 1);
			public static XYZ Down { get; } = new XYZ(0, 0, -1);
			public static XYZ XLeft { get; } = new XYZ(-1, 0, 0);
			public static XYZ XRight { get; } = new XYZ(1, 0, 0);
			public static XYZ YLeft { get; } = new XYZ(0, -1, 0);
			public static XYZ YRight { get; } = new XYZ(0, 1, 0);
		}

		public static XYZ DerivePointBetween(Line line, double distance = 1)
		{
			return DerivePointBetween(line.GetEndPoint(0), line.GetEndPoint(1), distance);
		}

		/// <summary>
		/// Get a point between two points that is X distance away
		/// </summary>
		/// <param name="start">starting XYZ coordinate</param>
		/// <param name="end">ending XYZ coordinate</param>
		/// <param name="distance">distance of next point</param>
		/// <returns>a point derived in between</returns>
		public static XYZ DerivePointBetween(XYZ start, XYZ end, double distance = 1)
		{
			double fi = Math.Atan2(end.Y - start.Y, end.X - start.X);
			// Your final point
			XYZ xyz = new XYZ(start.X + distance * Math.Cos(fi),
								start.Y + distance * Math.Sin(fi), end.Z);
			return xyz;
		}

		public static Line ExtendLineEndPoint(Line line, double length_to_extend)
		{
			var orig_length = line.Length;
			var ep1 = line.GetEndPoint(0);
			var ep2 = line.GetEndPoint(1);


			var x = ep2.X + (ep2.X - ep1.X) / orig_length * length_to_extend;
			var y = ep2.Y + (ep2.Y - ep1.Y) / orig_length * length_to_extend;

			XYZ new_ep = new XYZ(x, y, ep2.Z);
			return Line.CreateBound(ep1, new_ep);
		}

		public static bool IsLeft(Line line, XYZ chk_pt)
		{
			var a = line.GetEndPoint(0);
			var b = line.GetEndPoint(1);
			return Math.Sign((b.X - a.X) * (chk_pt.Y - a.Y) - (b.Y - a.Y) * (chk_pt.X - a.X)) < 0;
		}

		public static bool IsRight(Line line, XYZ chk_pt)
		{
			var a = line.GetEndPoint(0);
			var b = line.GetEndPoint(1);
			return Math.Sign((b.X - a.X) * (chk_pt.Y - a.Y) - (b.Y - a.Y) * (chk_pt.X - a.X)) > 0;
		}

		public static bool IsPtOnLine(Line line, XYZ chk_pt)
		{
			var a = line.GetEndPoint(0);
			var b = line.GetEndPoint(1);
			return Math.Sign((b.X - a.X) * (chk_pt.Y - a.Y) - (b.Y - a.Y) * (chk_pt.X - a.X)) == 0;
		}

		public static double AngleBetweenLines(Line line1, Line line2)
		{
			var d1 = line1.Direction;
			var d2 = line2.Direction;
			return Math.Acos(d1.Normalize().DotProduct(d2.Normalize()));
		}

		public static bool IsRotationClockwise(XYZ line_dir, XYZ pt1, XYZ pt2)
		{
			if((pt1.X - pt2.X) * line_dir.Y > (pt1.Y - pt2.Y) * line_dir.X)
				return true;
			else
				return false;
		}
	}

	/// <summary>
	/// Geometry based extension methods
	/// </summary>
	public static class RGeo_Ext
	{
		/// <summary>
		/// Get a printable string of an XYZ structure
		/// </summary>
		public static string PrintPt(this XYZ pt)
		{
			return String.Format(
				"({0}, {1}, {2})",
				pt.X, pt.Y, pt.Z
			);
		}
	}
}