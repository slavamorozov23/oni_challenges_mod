using System;

// Token: 0x02000A41 RID: 2625
public class RoomMonitor : GameStateMachine<RoomMonitor, RoomMonitor.Instance>
{
	// Token: 0x06004C94 RID: 19604 RVA: 0x001BD3F5 File Offset: 0x001BB5F5
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.EventHandler(GameHashes.PathAdvanced, new StateMachine<RoomMonitor, RoomMonitor.Instance, IStateMachineTarget, object>.State.Callback(RoomMonitor.UpdateRoomType));
	}

	// Token: 0x06004C95 RID: 19605 RVA: 0x001BD41C File Offset: 0x001BB61C
	private static void UpdateRoomType(RoomMonitor.Instance smi)
	{
		Room roomOfGameObject = Game.Instance.roomProber.GetRoomOfGameObject(smi.master.gameObject);
		if (roomOfGameObject != smi.currentRoom)
		{
			smi.currentRoom = roomOfGameObject;
			if (roomOfGameObject != null)
			{
				roomOfGameObject.cavity.OnEnter(smi.master.gameObject);
			}
		}
	}

	// Token: 0x02001B28 RID: 6952
	public new class Instance : GameStateMachine<RoomMonitor, RoomMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x0600A8A7 RID: 43175 RVA: 0x003BF834 File Offset: 0x003BDA34
		public Instance(IStateMachineTarget master) : base(master)
		{
		}

		// Token: 0x040083ED RID: 33773
		public Room currentRoom;
	}
}
