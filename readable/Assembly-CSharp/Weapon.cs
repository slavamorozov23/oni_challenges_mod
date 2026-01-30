using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000867 RID: 2151
[AddComponentMenu("KMonoBehaviour/scripts/Weapon")]
public class Weapon : KMonoBehaviour
{
	// Token: 0x06003B08 RID: 15112 RVA: 0x001493CC File Offset: 0x001475CC
	public void Configure(float base_damage_min, float base_damage_max, AttackProperties.DamageType attackType = AttackProperties.DamageType.Standard, AttackProperties.TargetType targetType = AttackProperties.TargetType.Single, int maxHits = 1, float aoeRadius = 0f)
	{
		this.properties = new AttackProperties();
		this.properties.base_damage_min = base_damage_min;
		this.properties.base_damage_max = base_damage_max;
		this.properties.maxHits = maxHits;
		this.properties.damageType = attackType;
		this.properties.aoe_radius = aoeRadius;
		this.properties.attacker = this;
	}

	// Token: 0x06003B09 RID: 15113 RVA: 0x0014942E File Offset: 0x0014762E
	public void AddEffect(string effectID = "WasAttacked", float probability = 1f)
	{
		if (this.properties.effects == null)
		{
			this.properties.effects = new List<AttackEffect>();
		}
		this.properties.effects.Add(new AttackEffect(effectID, probability));
	}

	// Token: 0x06003B0A RID: 15114 RVA: 0x00149464 File Offset: 0x00147664
	public int AttackArea(Vector3 centerPoint)
	{
		Vector3 b = Vector3.zero;
		this.alignment = base.GetComponent<FactionAlignment>();
		if (this.alignment == null)
		{
			return 0;
		}
		List<GameObject> list = new List<GameObject>();
		foreach (Health health in Components.Health.Items)
		{
			if (!(health.gameObject == base.gameObject) && !health.IsDefeated())
			{
				FactionAlignment component = health.GetComponent<FactionAlignment>();
				if (!(component == null) && component.IsAlignmentActive() && FactionManager.Instance.GetDisposition(this.alignment.Alignment, component.Alignment) == FactionManager.Disposition.Attack)
				{
					b = health.transform.GetPosition();
					b.z = centerPoint.z;
					if (Vector3.Distance(centerPoint, b) <= this.properties.aoe_radius)
					{
						list.Add(health.gameObject);
					}
				}
			}
		}
		this.AttackTargets(list.ToArray());
		return list.Count;
	}

	// Token: 0x06003B0B RID: 15115 RVA: 0x0014958C File Offset: 0x0014778C
	public void AttackTarget(GameObject target)
	{
		this.AttackTargets(new GameObject[]
		{
			target
		});
	}

	// Token: 0x06003B0C RID: 15116 RVA: 0x0014959E File Offset: 0x0014779E
	public void AttackTargets(GameObject[] targets)
	{
		if (this.properties == null)
		{
			global::Debug.LogWarning(string.Format("Attack properties not configured. {0} cannot attack with weapon.", base.gameObject.name));
			return;
		}
		new Attack(this.properties, targets);
	}

	// Token: 0x06003B0D RID: 15117 RVA: 0x001495D0 File Offset: 0x001477D0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.properties.attacker = this;
	}

	// Token: 0x040023F0 RID: 9200
	[MyCmpReq]
	private FactionAlignment alignment;

	// Token: 0x040023F1 RID: 9201
	public AttackProperties properties;
}
