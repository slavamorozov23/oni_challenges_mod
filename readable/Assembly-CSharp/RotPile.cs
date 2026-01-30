using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02000B22 RID: 2850
public class RotPile : StateMachineComponent<RotPile.StatesInstance>
{
	// Token: 0x0600534D RID: 21325 RVA: 0x001E58ED File Offset: 0x001E3AED
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x0600534E RID: 21326 RVA: 0x001E58F5 File Offset: 0x001E3AF5
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x0600534F RID: 21327 RVA: 0x001E5908 File Offset: 0x001E3B08
	protected void ConvertToElement()
	{
		PrimaryElement component = base.smi.master.GetComponent<PrimaryElement>();
		float mass = component.Mass;
		float temperature = component.Temperature;
		if (mass <= 0f)
		{
			Util.KDestroyGameObject(base.gameObject);
			return;
		}
		SimHashes hash = SimHashes.ToxicSand;
		GameObject gameObject = ElementLoader.FindElementByHash(hash).substance.SpawnResource(base.smi.master.transform.GetPosition(), mass, temperature, byte.MaxValue, 0, false, false, false);
		PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Resource, ElementLoader.FindElementByHash(hash).name, gameObject.transform, 1.5f, false);
		Util.KDestroyGameObject(base.smi.gameObject);
	}

	// Token: 0x06005350 RID: 21328 RVA: 0x001E59BC File Offset: 0x001E3BBC
	private static string OnRottenTooltip(List<Notification> notifications, object data)
	{
		string text = "";
		foreach (Notification notification in notifications)
		{
			if (notification.tooltipData != null)
			{
				text = text + "\n• " + (string)notification.tooltipData + " ";
			}
		}
		return string.Format(MISC.NOTIFICATIONS.FOODROT.TOOLTIP, text);
	}

	// Token: 0x06005351 RID: 21329 RVA: 0x001E5A40 File Offset: 0x001E3C40
	public void TryClearNotification()
	{
		if (this.notification != null)
		{
			base.gameObject.AddOrGet<Notifier>().Remove(this.notification);
		}
	}

	// Token: 0x06005352 RID: 21330 RVA: 0x001E5A60 File Offset: 0x001E3C60
	public void TryCreateNotification()
	{
		WorldContainer myWorld = base.smi.master.GetMyWorld();
		if (myWorld != null && myWorld.worldInventory.IsReachable(base.smi.master.gameObject.GetComponent<Pickupable>()))
		{
			this.notification = new Notification(MISC.NOTIFICATIONS.FOODROT.NAME, NotificationType.BadMinor, new Func<List<Notification>, object, string>(RotPile.OnRottenTooltip), null, true, 0f, null, null, null, true, false, false);
			this.notification.tooltipData = base.smi.master.gameObject.GetProperName();
			base.gameObject.AddOrGet<Notifier>().Add(this.notification, "");
		}
	}

	// Token: 0x04003865 RID: 14437
	private Notification notification;

	// Token: 0x02001C70 RID: 7280
	public class StatesInstance : GameStateMachine<RotPile.States, RotPile.StatesInstance, RotPile, object>.GameInstance
	{
		// Token: 0x0600ADAE RID: 44462 RVA: 0x003D0ED4 File Offset: 0x003CF0D4
		public StatesInstance(RotPile master) : base(master)
		{
		}

		// Token: 0x04008820 RID: 34848
		public AttributeModifier baseDecomposeRate;
	}

	// Token: 0x02001C71 RID: 7281
	public class States : GameStateMachine<RotPile.States, RotPile.StatesInstance, RotPile>
	{
		// Token: 0x0600ADAF RID: 44463 RVA: 0x003D0EE0 File Offset: 0x003CF0E0
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.decomposing;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.decomposing.Enter(delegate(RotPile.StatesInstance smi)
			{
				smi.master.TryCreateNotification();
			}).Exit(delegate(RotPile.StatesInstance smi)
			{
				smi.master.TryClearNotification();
			}).ParamTransition<float>(this.decompositionAmount, this.convertDestroy, (RotPile.StatesInstance smi, float p) => p >= 600f).Update("Decomposing", delegate(RotPile.StatesInstance smi, float dt)
			{
				this.decompositionAmount.Delta(dt, smi);
			}, UpdateRate.SIM_200ms, false);
			this.convertDestroy.Enter(delegate(RotPile.StatesInstance smi)
			{
				smi.master.ConvertToElement();
			});
		}

		// Token: 0x04008821 RID: 34849
		public GameStateMachine<RotPile.States, RotPile.StatesInstance, RotPile, object>.State decomposing;

		// Token: 0x04008822 RID: 34850
		public GameStateMachine<RotPile.States, RotPile.StatesInstance, RotPile, object>.State convertDestroy;

		// Token: 0x04008823 RID: 34851
		public StateMachine<RotPile.States, RotPile.StatesInstance, RotPile, object>.FloatParameter decompositionAmount;
	}
}
