using System;
using Klei.AI;
using TUNING;
using UnityEngine;

// Token: 0x02000599 RID: 1433
[AddComponentMenu("KMonoBehaviour/Workable/AttackableBase")]
public class AttackableBase : Workable, IApproachable
{
	// Token: 0x06002027 RID: 8231 RVA: 0x000B9A10 File Offset: 0x000B7C10
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.attributeConverter = Db.Get().AttributeConverters.AttackDamage;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.BARELY_EVER_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Mining.Id;
		this.skillExperienceMultiplier = SKILLS.BARELY_EVER_EXPERIENCE;
		this.SetupScenePartitioner(null);
		base.Subscribe<AttackableBase>(1088554450, AttackableBase.OnCellChangedDelegate);
		GameUtil.SubscribeToTags<AttackableBase>(this, AttackableBase.OnDeadTagAddedDelegate, true);
		base.Subscribe<AttackableBase>(-1506500077, AttackableBase.OnDefeatedDelegate);
		base.Subscribe<AttackableBase>(-1256572400, AttackableBase.SetupScenePartitionerDelegate);
	}

	// Token: 0x06002028 RID: 8232 RVA: 0x000B9AB0 File Offset: 0x000B7CB0
	public float GetDamageMultiplier()
	{
		if (this.attributeConverter != null && base.worker != null)
		{
			AttributeConverterInstance attributeConverter = base.worker.GetAttributeConverter(this.attributeConverter.Id);
			return Mathf.Max(1f + attributeConverter.Evaluate(), 0.1f);
		}
		return 1f;
	}

	// Token: 0x06002029 RID: 8233 RVA: 0x000B9B08 File Offset: 0x000B7D08
	private void SetupScenePartitioner(object data = null)
	{
		Extents extents = new Extents(Grid.PosToXY(base.transform.GetPosition()).x, Grid.PosToXY(base.transform.GetPosition()).y, 1, 1);
		this.scenePartitionerEntry = GameScenePartitioner.Instance.Add(base.gameObject.name, base.GetComponent<FactionAlignment>(), extents, GameScenePartitioner.Instance.attackableEntitiesLayer, null);
	}

	// Token: 0x0600202A RID: 8234 RVA: 0x000B9B75 File Offset: 0x000B7D75
	private void OnDefeated(object data = null)
	{
		GameScenePartitioner.Instance.Free(ref this.scenePartitionerEntry);
	}

	// Token: 0x0600202B RID: 8235 RVA: 0x000B9B87 File Offset: 0x000B7D87
	public override float GetEfficiencyMultiplier(WorkerBase worker)
	{
		return 1f;
	}

	// Token: 0x0600202C RID: 8236 RVA: 0x000B9B90 File Offset: 0x000B7D90
	protected override void OnCleanUp()
	{
		base.Unsubscribe<AttackableBase>(1088554450, AttackableBase.OnCellChangedDelegate, false);
		GameUtil.UnsubscribeToTags<AttackableBase>(this, AttackableBase.OnDeadTagAddedDelegate);
		base.Unsubscribe<AttackableBase>(-1506500077, AttackableBase.OnDefeatedDelegate, false);
		base.Unsubscribe<AttackableBase>(-1256572400, AttackableBase.SetupScenePartitionerDelegate, false);
		GameScenePartitioner.Instance.Free(ref this.scenePartitionerEntry);
		base.OnCleanUp();
	}

	// Token: 0x040012B3 RID: 4787
	private HandleVector<int>.Handle scenePartitionerEntry;

	// Token: 0x040012B4 RID: 4788
	private static readonly EventSystem.IntraObjectHandler<AttackableBase> OnDeadTagAddedDelegate = GameUtil.CreateHasTagHandler<AttackableBase>(GameTags.Dead, delegate(AttackableBase component, object data)
	{
		component.OnDefeated(data);
	});

	// Token: 0x040012B5 RID: 4789
	private static readonly EventSystem.IntraObjectHandler<AttackableBase> OnDefeatedDelegate = new EventSystem.IntraObjectHandler<AttackableBase>(delegate(AttackableBase component, object data)
	{
		component.OnDefeated(data);
	});

	// Token: 0x040012B6 RID: 4790
	private static readonly EventSystem.IntraObjectHandler<AttackableBase> SetupScenePartitionerDelegate = new EventSystem.IntraObjectHandler<AttackableBase>(delegate(AttackableBase component, object data)
	{
		component.SetupScenePartitioner(data);
	});

	// Token: 0x040012B7 RID: 4791
	private static readonly EventSystem.IntraObjectHandler<AttackableBase> OnCellChangedDelegate = new EventSystem.IntraObjectHandler<AttackableBase>(delegate(AttackableBase component, object data)
	{
		GameScenePartitioner.Instance.UpdatePosition(component.scenePartitionerEntry, Grid.PosToCell(component.gameObject));
	});
}
