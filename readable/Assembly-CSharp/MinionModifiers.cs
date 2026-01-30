using System;
using System.IO;
using Klei.AI;
using KSerialization;
using UnityEngine;

// Token: 0x02000A05 RID: 2565
[SerializationConfig(MemberSerialization.OptIn)]
public class MinionModifiers : Modifiers, ISaveLoadable
{
	// Token: 0x06004B05 RID: 19205 RVA: 0x001B314C File Offset: 0x001B134C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		if (this.addBaseTraits)
		{
			foreach (Klei.AI.Attribute attribute in Db.Get().Attributes.resources)
			{
				if (this.attributes.Get(attribute) == null)
				{
					this.attributes.Add(attribute);
				}
			}
			foreach (Disease disease in Db.Get().Diseases.resources)
			{
				AmountInstance amountInstance = this.AddAmount(disease.amount);
				this.attributes.Add(disease.cureSpeedBase);
				amountInstance.SetValue(0f);
			}
			ChoreConsumer component = base.GetComponent<ChoreConsumer>();
			if (component != null)
			{
				component.AddProvider(GlobalChoreProvider.Instance);
				base.gameObject.AddComponent<QualityOfLifeNeed>();
			}
		}
	}

	// Token: 0x06004B06 RID: 19206 RVA: 0x001B3264 File Offset: 0x001B1464
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (base.GetComponent<ChoreConsumer>() != null)
		{
			base.Subscribe<MinionModifiers>(1623392196, MinionModifiers.OnDeathDelegate);
			base.Subscribe<MinionModifiers>(-1506069671, MinionModifiers.OnAttachFollowCamDelegate);
			base.Subscribe<MinionModifiers>(-485480405, MinionModifiers.OnDetachFollowCamDelegate);
			base.Subscribe<MinionModifiers>(-1988963660, MinionModifiers.OnBeginChoreDelegate);
			AmountInstance amountInstance = this.GetAmounts().Get("Calories");
			if (amountInstance != null)
			{
				AmountInstance amountInstance2 = amountInstance;
				amountInstance2.OnMaxValueReached = (System.Action)Delegate.Combine(amountInstance2.OnMaxValueReached, new System.Action(this.OnMaxCaloriesReached));
			}
			Vector3 position = base.transform.GetPosition();
			position.z = Grid.GetLayerZ(Grid.SceneLayer.Move);
			base.transform.SetPosition(position);
			base.gameObject.layer = LayerMask.NameToLayer("Default");
			this.SetupDependentAttribute(Db.Get().Attributes.CarryAmount, Db.Get().AttributeConverters.CarryAmountFromStrength);
		}
	}

	// Token: 0x06004B07 RID: 19207 RVA: 0x001B3364 File Offset: 0x001B1564
	private AmountInstance AddAmount(Amount amount)
	{
		AmountInstance instance = new AmountInstance(amount, base.gameObject);
		return this.amounts.Add(instance);
	}

	// Token: 0x06004B08 RID: 19208 RVA: 0x001B338C File Offset: 0x001B158C
	private void SetupDependentAttribute(Klei.AI.Attribute targetAttribute, AttributeConverter attributeConverter)
	{
		Klei.AI.Attribute attribute = attributeConverter.attribute;
		AttributeInstance attributeInstance = attribute.Lookup(this);
		AttributeModifier target_modifier = new AttributeModifier(targetAttribute.Id, attributeConverter.Lookup(this).Evaluate(), attribute.Name, false, false, false);
		this.GetAttributes().Add(target_modifier);
		attributeInstance.OnDirty = (System.Action)Delegate.Combine(attributeInstance.OnDirty, new System.Action(delegate()
		{
			target_modifier.SetValue(attributeConverter.Lookup(this).Evaluate());
		}));
	}

	// Token: 0x06004B09 RID: 19209 RVA: 0x001B3420 File Offset: 0x001B1620
	private void OnDeath(object data)
	{
		global::Debug.LogFormat("OnDeath {0} -- {1} has died!", new object[]
		{
			data,
			base.name
		});
		foreach (MinionIdentity minionIdentity in Components.LiveMinionIdentities.Items)
		{
			minionIdentity.GetComponent<Effects>().Add("Mourning", true);
		}
	}

	// Token: 0x06004B0A RID: 19210 RVA: 0x001B34A0 File Offset: 0x001B16A0
	private void OnMaxCaloriesReached()
	{
		base.GetComponent<Effects>().Add("WellFed", true);
	}

	// Token: 0x06004B0B RID: 19211 RVA: 0x001B34B4 File Offset: 0x001B16B4
	private void OnBeginChore(object data)
	{
		Storage component = base.GetComponent<Storage>();
		if (component != null)
		{
			component.DropAll(false, false, default(Vector3), true, null);
		}
	}

	// Token: 0x06004B0C RID: 19212 RVA: 0x001B34E4 File Offset: 0x001B16E4
	public override void OnSerialize(BinaryWriter writer)
	{
		base.OnSerialize(writer);
	}

	// Token: 0x06004B0D RID: 19213 RVA: 0x001B34ED File Offset: 0x001B16ED
	public override void OnDeserialize(IReader reader)
	{
		base.OnDeserialize(reader);
	}

	// Token: 0x06004B0E RID: 19214 RVA: 0x001B34F6 File Offset: 0x001B16F6
	private void OnAttachFollowCam(object data)
	{
		base.GetComponent<Effects>().Add("CenterOfAttention", false);
	}

	// Token: 0x06004B0F RID: 19215 RVA: 0x001B350A File Offset: 0x001B170A
	private void OnDetachFollowCam(object data)
	{
		base.GetComponent<Effects>().Remove("CenterOfAttention");
	}

	// Token: 0x040031B6 RID: 12726
	public bool addBaseTraits = true;

	// Token: 0x040031B7 RID: 12727
	private static readonly EventSystem.IntraObjectHandler<MinionModifiers> OnDeathDelegate = new EventSystem.IntraObjectHandler<MinionModifiers>(delegate(MinionModifiers component, object data)
	{
		component.OnDeath(data);
	});

	// Token: 0x040031B8 RID: 12728
	private static readonly EventSystem.IntraObjectHandler<MinionModifiers> OnAttachFollowCamDelegate = new EventSystem.IntraObjectHandler<MinionModifiers>(delegate(MinionModifiers component, object data)
	{
		component.OnAttachFollowCam(data);
	});

	// Token: 0x040031B9 RID: 12729
	private static readonly EventSystem.IntraObjectHandler<MinionModifiers> OnDetachFollowCamDelegate = new EventSystem.IntraObjectHandler<MinionModifiers>(delegate(MinionModifiers component, object data)
	{
		component.OnDetachFollowCam(data);
	});

	// Token: 0x040031BA RID: 12730
	private static readonly EventSystem.IntraObjectHandler<MinionModifiers> OnBeginChoreDelegate = new EventSystem.IntraObjectHandler<MinionModifiers>(delegate(MinionModifiers component, object data)
	{
		component.OnBeginChore(data);
	});
}
