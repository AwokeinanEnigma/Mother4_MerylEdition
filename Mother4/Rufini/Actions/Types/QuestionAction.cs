using System;
using System.Collections.Generic;
using Carbine.Flags;
using Mother4.Actors.NPCs;
using Mother4.Scripts;
using Mother4.Scripts.Actions;
using Mother4.Scripts.Actions.ParamTypes;
using Rufini.Strings;

namespace Rufini.Actions.Types
{
	internal class QuestionAction : RufiniAction
	{
		public QuestionAction()
		{
			this.paramList = new List<ActionParam>
			{
				new ActionParam
				{
					Name = "text",
					Type = typeof(RufiniString)
				},
				new ActionParam
				{
					Name = "opt1",
					Type = typeof(RufiniString)
				},
				new ActionParam
				{
					Name = "opt2",
					Type = typeof(RufiniString)
				},
				new ActionParam
				{
					Name = "sin",
					Type = typeof(bool)
				},
				new ActionParam
				{
					Name = "sout",
					Type = typeof(bool)
				},
				new ActionParam
				{
					Name = "lbx",
					Type = typeof(bool)
				}
			};
		}

		public override ActionReturnContext Execute(ExecutionContext context)
		{
			this.context = context;
			RufiniString value = base.GetValue<RufiniString>("text");
			string value2 = StringFile.Instance.Get(value.Names).Value;
			RufiniString value3 = base.GetValue<RufiniString>("opt1");
			string value4 = StringFile.Instance.Get(value3.Names).Value;
			RufiniString value5 = base.GetValue<RufiniString>("opt2");
			string value6 = StringFile.Instance.Get(value5.Names).Value;
			bool value7 = base.GetValue<bool>("sin");
			bool value8 = base.GetValue<bool>("sout");
			this.context.QuestionBox.OnSelection += this.ContinueAfterTextbox;
			this.context.QuestionBox.Reset(value2, this.context.Nametag, value4, value6, value7, value8);
			this.context.QuestionBox.Show();
			this.context.Player.MovementLocked = true;
			if (this.context.ActiveNPC != null)
			{
				this.activeNpc = this.context.ActiveNPC;
				this.context.QuestionBox.OnTypewriterComplete += this.StopTalking;
				this.activeNpc.StartTalking();
			}
			return new ActionReturnContext
			{
				Wait = ScriptExecutor.WaitType.Event
			};
		}

		private void StopTalking()
		{
			this.activeNpc.StopTalking();
			this.context.QuestionBox.OnTypewriterComplete -= this.StopTalking;
			this.activeNpc = null;
		}

		private void ContinueAfterTextbox(int selection)
		{
			FlagManager.Instance[2] = (selection == 0);
			this.context.QuestionBox.Hide();
			this.context.QuestionBox.OnSelection -= this.ContinueAfterTextbox;
			this.context.Player.MovementLocked = false;
			this.context.Executor.Continue();
		}

		private ExecutionContext context;

		private NPC activeNpc;
	}
}
