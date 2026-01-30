using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000D1F RID: 3359
public class SparkLayer : LineLayer
{
	// Token: 0x060067EF RID: 26607 RVA: 0x00273BA4 File Offset: 0x00271DA4
	public void SetColor(ColonyDiagnostic.DiagnosticResult result)
	{
		switch (result.opinion)
		{
		case ColonyDiagnostic.DiagnosticResult.Opinion.DuplicantThreatening:
		case ColonyDiagnostic.DiagnosticResult.Opinion.Bad:
			this.SetColor(Constants.NEGATIVE_COLOR);
			return;
		case ColonyDiagnostic.DiagnosticResult.Opinion.Warning:
		case ColonyDiagnostic.DiagnosticResult.Opinion.Concern:
			this.SetColor(Constants.WARNING_COLOR);
			return;
		case ColonyDiagnostic.DiagnosticResult.Opinion.Suggestion:
		case ColonyDiagnostic.DiagnosticResult.Opinion.Tutorial:
		case ColonyDiagnostic.DiagnosticResult.Opinion.Normal:
			this.SetColor(Constants.NEUTRAL_COLOR);
			return;
		case ColonyDiagnostic.DiagnosticResult.Opinion.Good:
			this.SetColor(Constants.POSITIVE_COLOR);
			return;
		default:
			this.SetColor(Constants.NEUTRAL_COLOR);
			return;
		}
	}

	// Token: 0x060067F0 RID: 26608 RVA: 0x00273C1D File Offset: 0x00271E1D
	public void SetColor(Color color)
	{
		this.line_formatting[0].color = color;
	}

	// Token: 0x060067F1 RID: 26609 RVA: 0x00273C34 File Offset: 0x00271E34
	public override GraphedLine NewLine(global::Tuple<float, float>[] points, string ID = "")
	{
		Color positive_COLOR = Constants.POSITIVE_COLOR;
		Color neutral_COLOR = Constants.NEUTRAL_COLOR;
		Color negative_COLOR = Constants.NEGATIVE_COLOR;
		if (this.colorRules.setOwnColor)
		{
			if (points.Length > 2)
			{
				if (this.colorRules.zeroIsBad && points[points.Length - 1].second == 0f)
				{
					this.line_formatting[0].color = negative_COLOR;
				}
				else if (points[points.Length - 1].second > points[points.Length - 2].second)
				{
					this.line_formatting[0].color = (this.colorRules.positiveIsGood ? positive_COLOR : negative_COLOR);
				}
				else if (points[points.Length - 1].second < points[points.Length - 2].second)
				{
					this.line_formatting[0].color = (this.colorRules.positiveIsGood ? negative_COLOR : positive_COLOR);
				}
				else
				{
					this.line_formatting[0].color = neutral_COLOR;
				}
			}
			else
			{
				this.line_formatting[0].color = neutral_COLOR;
			}
		}
		this.ScaleToData(points);
		if (this.subZeroAreaFill != null)
		{
			this.subZeroAreaFill.color = new Color(this.line_formatting[0].color.r, this.line_formatting[0].color.g, this.line_formatting[0].color.b, this.fillAlphaMin);
		}
		return base.NewLine(points, ID);
	}

	// Token: 0x060067F2 RID: 26610 RVA: 0x00273DBA File Offset: 0x00271FBA
	public override void RefreshLine(global::Tuple<float, float>[] points, string ID)
	{
		this.SetColor(points);
		this.ScaleToData(points);
		base.RefreshLine(points, ID);
	}

	// Token: 0x060067F3 RID: 26611 RVA: 0x00273DD4 File Offset: 0x00271FD4
	private void SetColor(global::Tuple<float, float>[] points)
	{
		Color positive_COLOR = Constants.POSITIVE_COLOR;
		Color neutral_COLOR = Constants.NEUTRAL_COLOR;
		Color negative_COLOR = Constants.NEGATIVE_COLOR;
		if (this.colorRules.setOwnColor)
		{
			if (points.Length > 2)
			{
				if (this.colorRules.zeroIsBad && points[points.Length - 1].second == 0f)
				{
					this.line_formatting[0].color = negative_COLOR;
				}
				else if (points[points.Length - 1].second > points[points.Length - 2].second)
				{
					this.line_formatting[0].color = (this.colorRules.positiveIsGood ? positive_COLOR : negative_COLOR);
				}
				else if (points[points.Length - 1].second < points[points.Length - 2].second)
				{
					this.line_formatting[0].color = (this.colorRules.positiveIsGood ? negative_COLOR : positive_COLOR);
				}
				else
				{
					this.line_formatting[0].color = neutral_COLOR;
				}
			}
			else
			{
				this.line_formatting[0].color = neutral_COLOR;
			}
		}
		if (this.subZeroAreaFill != null)
		{
			this.subZeroAreaFill.color = new Color(this.line_formatting[0].color.r, this.line_formatting[0].color.g, this.line_formatting[0].color.b, this.fillAlphaMin);
		}
	}

	// Token: 0x060067F4 RID: 26612 RVA: 0x00273F4C File Offset: 0x0027214C
	private void ScaleToData(global::Tuple<float, float>[] points)
	{
		if (this.scaleWidthToData || this.scaleHeightToData)
		{
			Vector2 vector = base.CalculateMin(points);
			Vector2 vector2 = base.CalculateMax(points);
			if (this.scaleHeightToData)
			{
				base.graph.ClearHorizontalGuides();
				base.graph.axis_y.max_value = vector2.y;
				base.graph.axis_y.min_value = vector.y;
				base.graph.RefreshHorizontalGuides();
			}
			if (this.scaleWidthToData)
			{
				base.graph.ClearVerticalGuides();
				base.graph.axis_x.max_value = vector2.x;
				base.graph.axis_x.min_value = vector.x;
				base.graph.RefreshVerticalGuides();
			}
		}
	}

	// Token: 0x04004756 RID: 18262
	public Image subZeroAreaFill;

	// Token: 0x04004757 RID: 18263
	public SparkLayer.ColorRules colorRules;

	// Token: 0x04004758 RID: 18264
	public bool debugMark;

	// Token: 0x04004759 RID: 18265
	public bool scaleHeightToData = true;

	// Token: 0x0400475A RID: 18266
	public bool scaleWidthToData = true;

	// Token: 0x02001F51 RID: 8017
	[Serializable]
	public struct ColorRules
	{
		// Token: 0x04009237 RID: 37431
		public bool setOwnColor;

		// Token: 0x04009238 RID: 37432
		public bool positiveIsGood;

		// Token: 0x04009239 RID: 37433
		public bool zeroIsBad;
	}
}
