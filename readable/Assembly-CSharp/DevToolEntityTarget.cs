using System;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02000698 RID: 1688
public abstract class DevToolEntityTarget
{
	// Token: 0x06002990 RID: 10640
	public abstract string GetTag();

	// Token: 0x06002991 RID: 10641
	[return: TupleElementNames(new string[]
	{
		"cornerA",
		"cornerB"
	})]
	public abstract Option<ValueTuple<Vector2, Vector2>> GetScreenRect();

	// Token: 0x06002992 RID: 10642 RVA: 0x000EE6DB File Offset: 0x000EC8DB
	public string GetDebugName()
	{
		return "[" + this.GetTag() + "] " + this.ToString();
	}

	// Token: 0x0200155D RID: 5469
	public class ForUIGameObject : DevToolEntityTarget
	{
		// Token: 0x06009306 RID: 37638 RVA: 0x00374EC5 File Offset: 0x003730C5
		public ForUIGameObject(GameObject gameObject)
		{
			this.gameObject = gameObject;
		}

		// Token: 0x06009307 RID: 37639 RVA: 0x00374ED4 File Offset: 0x003730D4
		[return: TupleElementNames(new string[]
		{
			"cornerA",
			"cornerB"
		})]
		public override Option<ValueTuple<Vector2, Vector2>> GetScreenRect()
		{
			if (this.gameObject.IsNullOrDestroyed())
			{
				return Option.None;
			}
			RectTransform component = this.gameObject.GetComponent<RectTransform>();
			if (component.IsNullOrDestroyed())
			{
				return Option.None;
			}
			Canvas componentInParent = this.gameObject.GetComponentInParent<Canvas>();
			if (component.IsNullOrDestroyed())
			{
				return Option.None;
			}
			if (!componentInParent.worldCamera.IsNullOrDestroyed())
			{
				DevToolEntityTarget.ForUIGameObject.<>c__DisplayClass2_0 CS$<>8__locals1;
				CS$<>8__locals1.camera = componentInParent.worldCamera;
				Vector3[] array = new Vector3[4];
				component.GetWorldCorners(array);
				return new ValueTuple<Vector2, Vector2>(DevToolEntityTarget.ForUIGameObject.<GetScreenRect>g__ScreenPointToScreenPosition|2_0(CS$<>8__locals1.camera.WorldToScreenPoint(array[0]), ref CS$<>8__locals1), DevToolEntityTarget.ForUIGameObject.<GetScreenRect>g__ScreenPointToScreenPosition|2_0(CS$<>8__locals1.camera.WorldToScreenPoint(array[2]), ref CS$<>8__locals1));
			}
			if (componentInParent.renderMode == RenderMode.ScreenSpaceOverlay)
			{
				Vector3[] array2 = new Vector3[4];
				component.GetWorldCorners(array2);
				return new ValueTuple<Vector2, Vector2>(DevToolEntityTarget.ForUIGameObject.<GetScreenRect>g__ScreenPointToScreenPosition|2_1(array2[0]), DevToolEntityTarget.ForUIGameObject.<GetScreenRect>g__ScreenPointToScreenPosition|2_1(array2[2]));
			}
			return Option.None;
		}

		// Token: 0x06009308 RID: 37640 RVA: 0x00374FF7 File Offset: 0x003731F7
		public override string GetTag()
		{
			return "UI";
		}

		// Token: 0x06009309 RID: 37641 RVA: 0x00374FFE File Offset: 0x003731FE
		public override string ToString()
		{
			return DevToolEntity.GetNameFor(this.gameObject);
		}

		// Token: 0x0600930A RID: 37642 RVA: 0x0037500B File Offset: 0x0037320B
		[CompilerGenerated]
		internal static Vector2 <GetScreenRect>g__ScreenPointToScreenPosition|2_0(Vector2 coord, ref DevToolEntityTarget.ForUIGameObject.<>c__DisplayClass2_0 A_1)
		{
			return new Vector2(coord.x, (float)A_1.camera.pixelHeight - coord.y);
		}

		// Token: 0x0600930B RID: 37643 RVA: 0x0037502B File Offset: 0x0037322B
		[CompilerGenerated]
		internal static Vector2 <GetScreenRect>g__ScreenPointToScreenPosition|2_1(Vector2 coord)
		{
			return new Vector2(coord.x, (float)Screen.height - coord.y);
		}

		// Token: 0x04007194 RID: 29076
		public GameObject gameObject;
	}

	// Token: 0x0200155E RID: 5470
	public class ForWorldGameObject : DevToolEntityTarget
	{
		// Token: 0x0600930C RID: 37644 RVA: 0x00375045 File Offset: 0x00373245
		public ForWorldGameObject(GameObject gameObject)
		{
			this.gameObject = gameObject;
		}

		// Token: 0x0600930D RID: 37645 RVA: 0x00375054 File Offset: 0x00373254
		[return: TupleElementNames(new string[]
		{
			"cornerA",
			"cornerB"
		})]
		public override Option<ValueTuple<Vector2, Vector2>> GetScreenRect()
		{
			if (this.gameObject.IsNullOrDestroyed())
			{
				return Option.None;
			}
			DevToolEntityTarget.ForWorldGameObject.<>c__DisplayClass2_0 CS$<>8__locals1;
			CS$<>8__locals1.camera = Camera.main;
			if (CS$<>8__locals1.camera.IsNullOrDestroyed())
			{
				return Option.None;
			}
			KCollider2D component = this.gameObject.GetComponent<KCollider2D>();
			if (component.IsNullOrDestroyed())
			{
				return Option.None;
			}
			return new ValueTuple<Vector2, Vector2>(DevToolEntityTarget.ForWorldGameObject.<GetScreenRect>g__ScreenPointToScreenPosition|2_0(CS$<>8__locals1.camera.WorldToScreenPoint(component.bounds.min), ref CS$<>8__locals1), DevToolEntityTarget.ForWorldGameObject.<GetScreenRect>g__ScreenPointToScreenPosition|2_0(CS$<>8__locals1.camera.WorldToScreenPoint(component.bounds.max), ref CS$<>8__locals1));
		}

		// Token: 0x0600930E RID: 37646 RVA: 0x00375110 File Offset: 0x00373310
		public override string GetTag()
		{
			return "World";
		}

		// Token: 0x0600930F RID: 37647 RVA: 0x00375117 File Offset: 0x00373317
		public override string ToString()
		{
			return DevToolEntity.GetNameFor(this.gameObject);
		}

		// Token: 0x06009310 RID: 37648 RVA: 0x00375124 File Offset: 0x00373324
		[CompilerGenerated]
		internal static Vector2 <GetScreenRect>g__ScreenPointToScreenPosition|2_0(Vector2 coord, ref DevToolEntityTarget.ForWorldGameObject.<>c__DisplayClass2_0 A_1)
		{
			return new Vector2(coord.x, (float)A_1.camera.pixelHeight - coord.y);
		}

		// Token: 0x04007195 RID: 29077
		public GameObject gameObject;
	}

	// Token: 0x0200155F RID: 5471
	public class ForSimCell : DevToolEntityTarget
	{
		// Token: 0x06009311 RID: 37649 RVA: 0x00375144 File Offset: 0x00373344
		public ForSimCell(int cellIndex)
		{
			this.cellIndex = cellIndex;
		}

		// Token: 0x06009312 RID: 37650 RVA: 0x00375154 File Offset: 0x00373354
		[return: TupleElementNames(new string[]
		{
			"cornerA",
			"cornerB"
		})]
		public override Option<ValueTuple<Vector2, Vector2>> GetScreenRect()
		{
			DevToolEntityTarget.ForSimCell.<>c__DisplayClass2_0 CS$<>8__locals1;
			CS$<>8__locals1.camera = Camera.main;
			if (CS$<>8__locals1.camera.IsNullOrDestroyed())
			{
				return Option.None;
			}
			Vector2 a = Grid.CellToPosCCC(this.cellIndex, Grid.SceneLayer.Background);
			Vector2 b = Grid.HalfCellSizeInMeters * Vector2.one;
			Vector2 v = a - b;
			Vector2 v2 = a + b;
			return new ValueTuple<Vector2, Vector2>(DevToolEntityTarget.ForSimCell.<GetScreenRect>g__ScreenPointToScreenPosition|2_0(CS$<>8__locals1.camera.WorldToScreenPoint(v), ref CS$<>8__locals1), DevToolEntityTarget.ForSimCell.<GetScreenRect>g__ScreenPointToScreenPosition|2_0(CS$<>8__locals1.camera.WorldToScreenPoint(v2), ref CS$<>8__locals1));
		}

		// Token: 0x06009313 RID: 37651 RVA: 0x003751F9 File Offset: 0x003733F9
		public override string GetTag()
		{
			return "Sim Cell";
		}

		// Token: 0x06009314 RID: 37652 RVA: 0x00375200 File Offset: 0x00373400
		public override string ToString()
		{
			return this.cellIndex.ToString();
		}

		// Token: 0x06009315 RID: 37653 RVA: 0x0037520D File Offset: 0x0037340D
		[CompilerGenerated]
		internal static Vector2 <GetScreenRect>g__ScreenPointToScreenPosition|2_0(Vector2 coord, ref DevToolEntityTarget.ForSimCell.<>c__DisplayClass2_0 A_1)
		{
			return new Vector2(coord.x, (float)A_1.camera.pixelHeight - coord.y);
		}

		// Token: 0x04007196 RID: 29078
		public int cellIndex;
	}
}
