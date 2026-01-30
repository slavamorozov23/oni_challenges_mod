using System;
using System.Runtime.Serialization;
using Database;
using KSerialization;
using TUNING;
using UnityEngine;

// Token: 0x020007CA RID: 1994
[AddComponentMenu("KMonoBehaviour/scripts/MonumentPart")]
public class MonumentPart : KMonoBehaviour
{
	// Token: 0x060034C5 RID: 13509 RVA: 0x0012B5AC File Offset: 0x001297AC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Components.MonumentParts.Add(this);
		if (!string.IsNullOrEmpty(this.chosenState))
		{
			this.SetState(this.chosenState);
		}
		this.UpdateMonumentDecor();
	}

	// Token: 0x060034C6 RID: 13510 RVA: 0x0012B5E0 File Offset: 0x001297E0
	[OnDeserialized]
	private void OnDeserializedMethod()
	{
		if (Db.GetMonumentParts().TryGet(this.chosenState) == null)
		{
			string id = "";
			if (this.part == MonumentPartResource.Part.Bottom)
			{
				id = "bottom_" + this.chosenState;
			}
			else if (this.part == MonumentPartResource.Part.Middle)
			{
				id = "mid_" + this.chosenState;
			}
			else if (this.part == MonumentPartResource.Part.Top)
			{
				id = "top_" + this.chosenState;
			}
			if (Db.GetMonumentParts().TryGet(id) != null)
			{
				this.chosenState = id;
			}
		}
	}

	// Token: 0x060034C7 RID: 13511 RVA: 0x0012B66A File Offset: 0x0012986A
	protected override void OnCleanUp()
	{
		Components.MonumentParts.Remove(this);
		this.RemoveMonumentPiece();
		base.OnCleanUp();
	}

	// Token: 0x060034C8 RID: 13512 RVA: 0x0012B684 File Offset: 0x00129884
	public void SetState(string state)
	{
		MonumentPartResource monumentPartResource = Db.GetMonumentParts().Get(state);
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		component.SwapAnims(new KAnimFile[]
		{
			monumentPartResource.AnimFile
		});
		component.Play(monumentPartResource.State, KAnim.PlayMode.Once, 1f, 0f);
		this.chosenState = state;
	}

	// Token: 0x060034C9 RID: 13513 RVA: 0x0012B6DC File Offset: 0x001298DC
	public bool IsMonumentCompleted()
	{
		bool flag = this.GetMonumentPart(MonumentPartResource.Part.Top) != null;
		bool flag2 = this.GetMonumentPart(MonumentPartResource.Part.Middle) != null;
		bool flag3 = this.GetMonumentPart(MonumentPartResource.Part.Bottom) != null;
		return flag && flag3 && flag2;
	}

	// Token: 0x060034CA RID: 13514 RVA: 0x0012B718 File Offset: 0x00129918
	public void UpdateMonumentDecor()
	{
		GameObject monumentPart = this.GetMonumentPart(MonumentPartResource.Part.Middle);
		if (this.IsMonumentCompleted())
		{
			monumentPart.GetComponent<DecorProvider>().SetValues(BUILDINGS.DECOR.BONUS.MONUMENT.COMPLETE);
			foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(base.GetComponent<AttachableBuilding>()))
			{
				if (gameObject != monumentPart)
				{
					gameObject.GetComponent<DecorProvider>().SetValues(BUILDINGS.DECOR.NONE);
				}
			}
		}
	}

	// Token: 0x060034CB RID: 13515 RVA: 0x0012B7A4 File Offset: 0x001299A4
	public void RemoveMonumentPiece()
	{
		if (this.IsMonumentCompleted())
		{
			foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(base.GetComponent<AttachableBuilding>()))
			{
				if (gameObject.GetComponent<MonumentPart>() != this)
				{
					gameObject.GetComponent<DecorProvider>().SetValues(BUILDINGS.DECOR.BONUS.MONUMENT.INCOMPLETE);
				}
			}
		}
	}

	// Token: 0x060034CC RID: 13516 RVA: 0x0012B81C File Offset: 0x00129A1C
	private GameObject GetMonumentPart(MonumentPartResource.Part requestPart)
	{
		foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(base.GetComponent<AttachableBuilding>()))
		{
			MonumentPart component = gameObject.GetComponent<MonumentPart>();
			if (!(component == null) && component.part == requestPart)
			{
				return gameObject;
			}
		}
		return null;
	}

	// Token: 0x04001FEF RID: 8175
	public MonumentPartResource.Part part;

	// Token: 0x04001FF0 RID: 8176
	public string stateUISymbol;

	// Token: 0x04001FF1 RID: 8177
	[Serialize]
	private string chosenState;
}
