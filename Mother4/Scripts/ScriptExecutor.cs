using System;
using System.Collections.Generic;
using Mother4.Actors.NPCs;
using Mother4.Scripts.Actions;
using Rufini.Actions.Types;

namespace Mother4.Scripts
{
	internal class ScriptExecutor
	{
		public bool Running
		{
			get
			{
				return this.running;
			}
		}

		public int ProgramCounter
		{
			get
			{
				return this.programCounter;
			}
		}

		public ScriptExecutor(ExecutionContext context)
		{
			this.contextStack = new Stack<ScriptExecutor.ScriptContext>();
			this.context = context;
			this.context.Executor = this;
			this.waitMode = ScriptExecutor.WaitType.None;
			this.pausedInstruction = 0;
			this.context.TextBox.OnTypewriterComplete += this.AfterTextboxTypewriter;
			this.context.QuestionBox.OnTypewriterComplete += this.AfterTextboxTypewriter;
		}

		public void PushScript(Script script)
		{
			if (this.waitMode != ScriptExecutor.WaitType.None)
			{
				throw new InvalidOperationException("Cannot push a new script while waiting for an action to complete.");
			}
			if (this.contextStack.Count < 32)
			{
				if (this.script != null)
				{
					this.contextStack.Push(new ScriptExecutor.ScriptContext(this.context, this.script.Value, this.programCounter));
					this.Reset();
					this.context = new ExecutionContext(this.context);
				}
				this.script = new Script?(script);
				this.actions = this.script.Value.Actions;
				this.programCounter = 0;
				this.pushedScript = true;
				return;
			}
			string message = string.Format("Script Executor stack cannot exceed {0} levels.", 32);
			throw new StackOverflowException(message);
		}

		public void SetCheckedNPC(NPC npc)
		{
			this.context.CheckedNPC = npc;
		}

		private void Reset()
		{
			this.pausedInstruction = 0;
			this.running = false;
			this.script = null;
			this.context.Nametag = null;
			if (this.context.ActiveNPC != null)
			{
				this.context.ActiveNPC.StopTalking();
			}
			this.context.ActiveNPC = null;
			this.context.CheckedNPC = null;
		}

		public void Continue()
		{
			this.waitMode = ScriptExecutor.WaitType.None;
			Console.WriteLine("EX: {0} Continued", this.programCounter);
		}

		public void JumpToElseOrEndIf()
		{
			if (this.script != null)
			{
				Script value = this.script.Value;
				int num = value.Actions.Length;
				int num2 = 0;
				for (int i = this.programCounter; i < num; i++)
				{
					RufiniAction rufiniAction = value.Actions[i];
					if (rufiniAction is IfFlagAction || rufiniAction is IfValueAction || rufiniAction is IfReturnAction)
					{
						num2++;
					}
					else if (rufiniAction is EndIfAction)
					{
						num2--;
						if (num2 == 0)
						{
							this.programCounter = i;
							return;
						}
					}
					else if (rufiniAction is ElseAction)
					{
						if (i == this.programCounter)
						{
							num2++;
						}
						else if (num2 - 1 == 0)
						{
							this.programCounter = i;
							return;
						}
					}
				}
			}
		}

		private void AfterTextboxTypewriter()
		{
			if (this.context.ActiveNPC != null)
			{
				this.context.ActiveNPC.PauseTalking();
			}
		}

		private bool PopScript()
		{
			bool result = true;
			this.Reset();
			if (this.contextStack.Count > 0)
			{
				ScriptExecutor.ScriptContext scriptContext = this.contextStack.Pop();
				this.context = scriptContext.ExecutionContext;
				this.script = new Script?(scriptContext.Script);
				this.actions = this.script.Value.Actions;
				this.pausedInstruction = scriptContext.ProgramCounter + 1;
			}
			else
			{
				Console.WriteLine("EX: End of execution");
				result = false;
			}
			return result;
		}

		public void Execute()
		{
			if (this.script != null)
			{
				this.running = true;
				if (this.waitMode != ScriptExecutor.WaitType.Event)
				{
					bool flag = true;
					while (flag)
					{
						flag = false;
						this.programCounter = 0;
						this.programCounter = this.pausedInstruction;
						while (this.programCounter < this.actions.Length)
						{
							if (this.pushedScript)
							{
								this.pushedScript = false;
								this.programCounter = 0;
							}
							RufiniAction rufiniAction = this.actions[this.programCounter];
							Console.WriteLine("EX: {0} Execute {1}", this.programCounter, rufiniAction.GetType().Name);
							this.waitMode = rufiniAction.Execute(this.context).Wait;
							if (this.waitMode != ScriptExecutor.WaitType.None)
							{
								this.pausedInstruction = this.programCounter + 1;
								Console.WriteLine("EX: {0} Paused {1}", this.programCounter, this.pausedInstruction);
								break;
							}
							this.programCounter++;
						}
						if (this.waitMode == ScriptExecutor.WaitType.None && this.programCounter >= this.actions.Length)
						{
							Console.WriteLine("EX: End of script; popping");
							flag = this.PopScript();
						}
					}
				}
			}
		}

		private const int STACK_MAX_SIZE = 32;

		private Stack<ScriptExecutor.ScriptContext> contextStack;

		private ExecutionContext context;

		private Script? script;

		private RufiniAction[] actions;

		private bool running;

		private ScriptExecutor.WaitType waitMode;

		private int pausedInstruction;

		private int programCounter;

		private bool pushedScript;

		public enum WaitType
		{
			None,
			Frame,
			Event
		}

		private struct ScriptContext
		{
			public ExecutionContext ExecutionContext
			{
				get
				{
					return this.context;
				}
			}

			public Script Script
			{
				get
				{
					return this.script;
				}
			}

			public int ProgramCounter
			{
				get
				{
					return this.programCounter;
				}
			}

			public ScriptContext(ExecutionContext context, Script script, int programCounter)
			{
				this.context = context;
				this.script = script;
				this.programCounter = programCounter;
			}

			private ExecutionContext context;

			private Script script;

			private int programCounter;
		}
	}
}
