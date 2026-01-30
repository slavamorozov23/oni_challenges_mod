using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000670 RID: 1648
[AddComponentMenu("KMonoBehaviour/Workable/Unsealable")]
public class Unsealable : Workable
{
	// Token: 0x060027FF RID: 10239 RVA: 0x000E6674 File Offset: 0x000E4874
	private Unsealable()
	{
	}

	// Token: 0x06002800 RID: 10240 RVA: 0x000E667C File Offset: 0x000E487C
	public override CellOffset[] GetOffsets(int cell)
	{
		if (this.facingRight)
		{
			return OffsetGroups.RightOnly;
		}
		return OffsetGroups.LeftOnly;
	}

	// Token: 0x06002801 RID: 10241 RVA: 0x000E6691 File Offset: 0x000E4891
	protected override void OnPrefabInit()
	{
		this.faceTargetWhenWorking = true;
		base.OnPrefabInit();
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_door_poi_kanim")
		};
	}

	// Token: 0x06002802 RID: 10242 RVA: 0x000E66C0 File Offset: 0x000E48C0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.SetWorkTime(3f);
		if (this.unsealed)
		{
			Deconstructable component = base.GetComponent<Deconstructable>();
			if (component != null)
			{
				component.allowDeconstruction = true;
			}
		}
	}

	// Token: 0x06002803 RID: 10243 RVA: 0x000E66FD File Offset: 0x000E48FD
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
	}

	// Token: 0x06002804 RID: 10244 RVA: 0x000E6708 File Offset: 0x000E4908
	protected override void OnCompleteWork(WorkerBase worker)
	{
		this.unsealed = true;
		base.OnCompleteWork(worker);
		Deconstructable component = base.GetComponent<Deconstructable>();
		if (component != null)
		{
			component.allowDeconstruction = true;
			Game.Instance.Trigger(1980521255, base.gameObject);
		}
	}

	// Token: 0x0400177C RID: 6012
	[Serialize]
	public bool facingRight;

	// Token: 0x0400177D RID: 6013
	[Serialize]
	public bool unsealed;
}
