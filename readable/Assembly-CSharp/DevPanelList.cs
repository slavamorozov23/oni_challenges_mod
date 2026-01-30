using System;
using System.Collections.Generic;

// Token: 0x02000683 RID: 1667
public class DevPanelList
{
	// Token: 0x0600290D RID: 10509 RVA: 0x000EA3B4 File Offset: 0x000E85B4
	public DevPanel AddPanelFor<T>() where T : DevTool, new()
	{
		return this.AddPanelFor(Activator.CreateInstance<T>());
	}

	// Token: 0x0600290E RID: 10510 RVA: 0x000EA3C8 File Offset: 0x000E85C8
	public DevPanel AddPanelFor(DevTool devTool)
	{
		DevPanel devPanel = new DevPanel(devTool, this);
		this.activePanels.Add(devPanel);
		return devPanel;
	}

	// Token: 0x0600290F RID: 10511 RVA: 0x000EA3EC File Offset: 0x000E85EC
	public Option<T> GetDevTool<T>() where T : DevTool
	{
		foreach (DevPanel devPanel in this.activePanels)
		{
			T t = devPanel.GetCurrentDevTool() as T;
			if (t != null)
			{
				return t;
			}
		}
		return Option.None;
	}

	// Token: 0x06002910 RID: 10512 RVA: 0x000EA464 File Offset: 0x000E8664
	public T AddOrGetDevTool<T>() where T : DevTool, new()
	{
		bool flag;
		T t;
		this.GetDevTool<T>().Deconstruct(out flag, out t);
		bool flag2 = flag;
		T t2 = t;
		if (!flag2)
		{
			t2 = Activator.CreateInstance<T>();
			this.AddPanelFor(t2);
		}
		return t2;
	}

	// Token: 0x06002911 RID: 10513 RVA: 0x000EA49C File Offset: 0x000E869C
	public void ClosePanel(DevPanel panel)
	{
		if (this.activePanels.Remove(panel))
		{
			panel.Internal_Uninit();
		}
	}

	// Token: 0x06002912 RID: 10514 RVA: 0x000EA4B4 File Offset: 0x000E86B4
	public void Render()
	{
		if (this.activePanels.Count == 0)
		{
			return;
		}
		using (ListPool<DevPanel, DevPanelList>.PooledList pooledList = ListPool<DevPanel, DevPanelList>.Allocate())
		{
			for (int i = 0; i < this.activePanels.Count; i++)
			{
				DevPanel devPanel = this.activePanels[i];
				devPanel.RenderPanel();
				if (devPanel.isRequestingToClose)
				{
					pooledList.Add(devPanel);
				}
			}
			foreach (DevPanel panel in pooledList)
			{
				this.ClosePanel(panel);
			}
		}
	}

	// Token: 0x06002913 RID: 10515 RVA: 0x000EA568 File Offset: 0x000E8768
	public void Internal_InitPanelId(Type initialDevToolType, out string panelId, out uint idPostfixNumber)
	{
		idPostfixNumber = this.Internal_GetUniqueIdPostfix(initialDevToolType);
		panelId = initialDevToolType.Name + idPostfixNumber.ToString();
	}

	// Token: 0x06002914 RID: 10516 RVA: 0x000EA588 File Offset: 0x000E8788
	public uint Internal_GetUniqueIdPostfix(Type initialDevToolType)
	{
		uint result;
		using (HashSetPool<uint, DevPanelList>.PooledHashSet pooledHashSet = HashSetPool<uint, DevPanelList>.Allocate())
		{
			foreach (DevPanel devPanel in this.activePanels)
			{
				if (!(devPanel.initialDevToolType != initialDevToolType))
				{
					pooledHashSet.Add(devPanel.idPostfixNumber);
				}
			}
			for (uint num = 0U; num < 100U; num += 1U)
			{
				if (!pooledHashSet.Contains(num))
				{
					return num;
				}
			}
			Debug.Assert(false, "Something went wrong, this should only assert if there's over 100 of the same type of debug window");
			uint num2 = this.fallbackUniqueIdPostfixNumber;
			this.fallbackUniqueIdPostfixNumber = num2 + 1U;
			result = num2;
		}
		return result;
	}

	// Token: 0x04001840 RID: 6208
	private List<DevPanel> activePanels = new List<DevPanel>();

	// Token: 0x04001841 RID: 6209
	private uint fallbackUniqueIdPostfixNumber = 300U;
}
