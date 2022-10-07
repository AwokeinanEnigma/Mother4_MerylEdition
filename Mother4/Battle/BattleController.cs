using System;
using System.Collections.Generic;
using Carbine.Audio;
using Mother4.Battle.Actions;
using Mother4.Battle.Combatants;
using Mother4.Battle.Combos;
using Mother4.Data;

namespace Mother4.Battle
{
	internal class BattleController : IDisposable
	{
		//private CarbineSound runawaySuccessful;

		public BattleInterfaceController InterfaceController
		{
			get
			{
				return this.uiControl;
			}
		}

		public CombatantController CombatantController
		{
			get
			{
				return this.combatantControl;
			}
		}

		public ComboController ComboController
		{
			get
			{
				return this.comboControl;
			}
		}

		public int ActionCount
		{
			get
			{
				return this.actions.Count;
			}
		}

		public int DecisionCount
		{
			get
			{
				return this.decisions.Count;
			}
		}

		public Dictionary<string, object> Data
		{
			get
			{
				return this.data;
			}
		}

		public BattleStatus Status
		{
			get
			{
				return this.status;
			}
		}

		public BattleController(BattleInterfaceController uicontrol, CombatantController combatantControl, ComboController comboControl)
		{
		//	this.runawaySuccessful = AudioManager.Instance.Use(Paths.AUDIO + "runaway.wav", AudioType.Sound);
			this.uiControl = uicontrol;
			this.combatantControl = combatantControl;
			this.comboControl = comboControl;
			this.status = BattleStatus.Ongoing;
			this.actions = new List<BattleAction>();
			this.decisions = new List<DecisionAction>();
			this.data = new Dictionary<string, object>();
		}

		~BattleController()
		{
			this.Dispose(false);
		}

		public void Update()
		{
			this.RemoveDeadEnemies();
			this.actions.Sort();
			if (this.WinConditionsMet() && !this.winHandled)
			{
				this.status = BattleStatus.Won;
				Console.WriteLine("Win conditions met; queue cleared.");
				this.actions.Clear();
				ActionParams aparams = new ActionParams
				{
					actionType = typeof(BattleWinAction),
					controller = this,
					sender = null,
					priority = 2147483646
				};
				this.actions.Add(BattleAction.GetInstance(aparams));
				this.winHandled = true;
			}
			else if (this.uiControl.RunAttempted)
			{
				this.uiControl.RunAttempted = false;
				if (BattleCalculator.RunSuccess(this.combatantControl, this.turnNumber))
				{
					this.status = BattleStatus.Ran;
					Console.WriteLine("Ran away succesfully; queue cleared.");
					this.actions.Clear();
					this.decisions.Clear();
		//			runawaySuccessful.Play();
					ActionParams aparams2 = new ActionParams
					{
						actionType = typeof(MessageAction),
						controller = this,
						sender = null,
						priority = int.MaxValue,
						data = new object[]
						{
							"You tried to run away...[p:60] and did!",
							false
						}
					};
					this.actions.Add(BattleAction.GetInstance(aparams2));
					ActionParams aparams3 = new ActionParams
					{
						actionType = typeof(BattleEscapeAction),
						controller = this,
						sender = null,
						priority = 2147483646
					};
					this.actions.Add(BattleAction.GetInstance(aparams3));
				}
				else
				{
					Console.WriteLine("Run away failed; removing player team actions from queue");
					this.actions.RemoveAll((BattleAction action) => action.Sender.Faction == BattleFaction.PlayerTeam);
					this.decisions.RemoveAll((DecisionAction decision) => decision.Sender.Faction == BattleFaction.PlayerTeam);
					this.decisionCounter = 0;
					ActionParams aparams4 = new ActionParams
					{
						actionType = typeof(MessageAction),
						controller = this,
						sender = null,
						priority = int.MaxValue,
						data = new object[]
						{
							"You tried to run away...[p:60] but couldn't!",
							false
						}
					};
					this.actions.Add(BattleAction.GetInstance(aparams4));
				}
			}
			if (this.focus != null && !this.focus.Complete)
			{
				this.focus.Update();
				return;
			}
			this.HandleGroovy();
			if (this.decisionCounter < this.decisions.Count)
			{
				this.focus = this.decisions[this.decisionCounter];
				this.decisionCounter++;
				return;
			}
			if (this.actions.Count > 0)
			{
				this.uiControl.HideButtonBar();
				this.focus = this.actions[0];
				this.actions.RemoveAt(0);
				return;
			}
			Console.WriteLine("All actions performed; building new queue from DecisionActions.");
			this.BuildNewDecisionQueue();
		}

		private void RemoveDeadEnemies()
		{
			Combatant[] factionCombatants = this.combatantControl.GetFactionCombatants(BattleFaction.EnemyTeam);
			foreach (Combatant combatant in factionCombatants)
			{
				if (combatant.Stats.HP <= 0)
				{
					ActionParams aparams = new ActionParams
					{
						actionType = typeof(EnemyDeathAction),
						controller = this,
						sender = (combatant as EnemyCombatant),
						priority = int.MaxValue
					};
					this.actions.Add(BattleAction.GetInstance(aparams));
				}
			}
		}

		private void HandleGroovy()
		{
			Combatant[] factionCombatants = this.combatantControl.GetFactionCombatants(BattleFaction.PlayerTeam);
			Combatant[] factionCombatants2 = this.combatantControl.GetFactionCombatants(BattleFaction.EnemyTeam);
			if (factionCombatants2.Length > 0)
			{
				Combatant[] array = factionCombatants;
				for (int i = 0; i < array.Length; i++)
				{
					Combatant combatant = array[i];
					BattleAction battleAction = this.actions.Find((BattleAction x) => x is GroovyAction && x.Sender == combatant);
					if (combatant.Stats.HP > 0 && combatant.Stats.Meter >= 1f && battleAction == null)
					{
						ActionParams aparams = new ActionParams
						{
							actionType = typeof(GroovyAction),
							controller = this,
							sender = combatant,
							targets = new Combatant[]
							{
								combatant
							},
							priority = 2147483646
						};
						this.actions.Add(BattleAction.GetInstance(aparams));
						this.actions.Sort();
					}
				}
			}
		}

		public void RemoveCombatant(Combatant combatant)
		{
			this.RemoveCombatantActions(combatant);
			this.uiControl.RemoveEnemy(combatant.ID);
			this.combatantControl.Remove(combatant);
		}

		public void RemoveCombatantActions(Combatant combatant)
		{ 
            this.actions.RemoveAll((BattleAction action) => action.Sender == combatant);
        }

		private bool WinConditionsMet()
		{
			int num = 0;
			foreach (Combatant combatant in this.combatantControl.CombatantList)
			{
				if (combatant.Faction == BattleFaction.EnemyTeam)
				{
					num++;
				}
			}
			return num == 0;
		}

		public void SetFinalStatSets()
		{
			Combatant[] factionCombatants = this.combatantControl.GetFactionCombatants(BattleFaction.PlayerTeam);
			foreach (Combatant combatant in factionCombatants)
			{
				PlayerCombatant playerCombatant = combatant as PlayerCombatant;
				StatSet stats = CharacterStats.GetStats(playerCombatant.Character);
				stats.HP = combatant.Stats.HP;
				stats.PP = combatant.Stats.PP;
				CharacterStats.SetStats(playerCombatant.Character, stats);
			}
		}

		private void BuildNewDecisionQueue()
		{
			this.turnNumber++;
			this.decisions.Clear();
			this.decisionCounter = 0;
			for (int i = 0; i < this.combatantControl.CombatantList.Count; i++)
			{
				Combatant combatant = this.combatantControl.CombatantList[i];
				this.decisions.Add(combatant.GetDecisionAction(this, this.combatantControl.CombatantList.Count - i));
			}
		}

		public void AddAction(BattleAction action)
		{
			this.actions.Add(action);
		}

		public void AddActions(BattleAction[] actions)
		{
			for (int i = 0; i < actions.Length; i++)
			{
				this.AddAction(actions[i]);
			}
		}

		public void AddDecisionAction(DecisionAction action)
		{
			this.decisions.Add(action);
		}

		public void AddDecisionActions(DecisionAction[] actions)
		{
			for (int i = 0; i < actions.Length; i++)
			{
				this.AddDecisionAction(actions[i]);
			}
		}

		public bool RevertDecision()
		{
			if (this.decisionCounter > 1)
			{
				DecisionAction decisionAction = this.decisions[this.decisionCounter - 1];
				DecisionAction decisionAction2 = this.decisions[this.decisionCounter - 2];
				if (decisionAction2.Sender.Faction == BattleFaction.PlayerTeam)
				{
					PlayerCombatant playerCombatant = (PlayerCombatant)decisionAction2.Sender;
					PlayerCombatant playerCombatant2 = (PlayerCombatant)decisionAction.Sender;
					foreach (BattleAction battleAction in this.actions.ToArray())
					{
						if (battleAction.Sender == playerCombatant)
						{
							this.actions.Remove(battleAction);
						}
					}
					this.decisionCounter -= 2;
					this.decisions[this.decisionCounter] = playerCombatant.GetDecisionAction(this, this.decisionCounter, true);
					this.decisions[this.decisionCounter + 1] = playerCombatant2.GetDecisionAction(this, this.decisionCounter + 1);
					return true;
				}
			}
			return false;
		}

		public bool CanRevert()
		{
			return this.decisionCounter > 1;
		}

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool disposing)
		{
			if (!this.disposed && !disposing)
			{
				this.uiControl.Dispose();
			}
			this.disposed = true;
			//AudioManager.Instance.Unuse(this.runawaySuccessful);
			
		}

		private bool disposed;

		private BattleStatus status;

		private List<BattleAction> actions;

		private List<DecisionAction> decisions;

		private BattleAction focus;

		private int decisionCounter;

		private bool winHandled;

		private int turnNumber;

		private BattleInterfaceController uiControl;

		private CombatantController combatantControl;

		private ComboController comboControl;

		private Dictionary<string, object> data;
	}
}
