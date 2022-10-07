using System;
using System.Collections.Generic;
using Carbine;
using Carbine.Graphics;
using Mother4.Battle.Combatants;
using Mother4.Battle.UI;
using Mother4.Data;
using Mother4.Utility;
using SFML.Graphics;
using SFML.System;

namespace Mother4.Battle.Actions
{
	internal class EnemyBashAction : BattleAction
	{
		public bool useCustomText;
		public string customText;
		public EnemyBashAction(ActionParams aparams) : base(aparams)
		{
			this.combatant = (this.sender as EnemyCombatant);
			this.power = (float)aparams.data[0];
			this.messages = new Stack<string>();
			this.statSets = new Stack<Tuple<Combatant, StatSet>>();
			useCustomText = ((aparams.data.Length > 1) ? ((bool)aparams.data[1]) : false);
			customText = ((aparams.data.Length > 2) ? ((string)aparams.data[2]) : "");
			this.state = EnemyBashAction.State.Initialize;
		}

		//this code is so fucked i refuse to look at it
		protected override void UpdateAction()
		{
			base.UpdateAction();
			switch (this.state)
			{
				case EnemyBashAction.State.Initialize:
					{
						Console.WriteLine("BASHMÖDE ({0})", this.combatant.Enemy);
						this.controller.InterfaceController.OnTextboxComplete += this.InteractionComplete;
						this.controller.InterfaceController.FlashEnemy(this.sender as EnemyCombatant, Color.White, ColorBlendMode.Screen, 8, 2);
						this.controller.InterfaceController.PreEnemyAttack.Play();
						int i = 0;
						while (i < this.targets.Length)
						{
							Combatant combatant = this.targets[i];
							if (this.controller.CombatantController.IsIdValid(combatant.ID))
							{
								goto IL_FC;
							}
							Combatant[] factionCombatants = this.controller.CombatantController.GetFactionCombatants(BattleFaction.PlayerTeam);
							Combatant combatant2 = factionCombatants[Engine.Random.Next() % factionCombatants.Length];
							if (Array.IndexOf<Combatant>(this.targets, combatant2) < 0)
							{
								this.targets[i] = combatant2;
								combatant = combatant2;
								goto IL_FC;
							}
						IL_239:
							i++;
							continue;
						IL_FC:
							StatSet item = default(StatSet);
							item.HP = Math.Min(0, -(int)(this.power * (float)this.combatant.Stats.Offense - (float)combatant.Stats.Defense));
							this.statSets.Push(new Tuple<Combatant, StatSet>(combatant, item));
							Console.WriteLine(" Target's HP changed by {0}.", item.HP);
							string text = "";
							switch (combatant.Faction)
							{
								case BattleFaction.PlayerTeam:
									{
										PlayerCombatant playerCombatant = (PlayerCombatant)combatant;
										text = CharacterNames.GetName(playerCombatant.Character);
										break;
									}
								case BattleFaction.EnemyTeam:
									{
										EnemyCombatant enemyCombatant = (EnemyCombatant)combatant;
										text = string.Format("{0}{1}", enemyCombatant.Enemy.PlayerFriendlyName, enemyCombatant.Enemy.PlayerFriendlyName);
										break;
									}
								case BattleFaction.NeutralTeam:
									text = "UNIMPLEMENTED";
									break;
							}

							string item2 = "";

							if (!useCustomText)
							{


                                item2 = string.Format("{0}{1} bashed {2} for {3} hit points of damage!", new object[]
                                {
                                    Capitalizer.Capitalize(this.combatant.Enemy.Article),
                                    this.combatant.Enemy.PlayerFriendlyName,
                                        text,
                                        -item.HP
                                });
							}
							else
							{
								item2 = string.Format("{0}{1} {2} {3} for {4} hit points of damage!", new object[]
								{
									Capitalizer.Capitalize(this.combatant.Enemy.Article),
                                    this.combatant.Enemy.PlayerFriendlyName,
									customText,
									text,
									-item.HP
								});

							}
							this.messages.Push(item2);

							goto IL_239;
						}
						this.state = EnemyBashAction.State.PopMessage;
						return;
					}
				case EnemyBashAction.State.PopMessage:
					{
						Tuple<Combatant, StatSet> tuple = this.statSets.Pop();
						tuple.Item1.AlterStats(tuple.Item2);
						string message = this.messages.Pop();
						this.controller.InterfaceController.ShowMessage(message, false);
						this.controller.InterfaceController.AddDamageNumber(tuple.Item1, tuple.Item2.HP);
						if (tuple.Item1 is PlayerCombatant)
						{
							this.controller.InterfaceController.SetCardSpring(tuple.Item1.ID, BattleCard.SpringMode.Normal, new Vector2f(0f, 8f), new Vector2f(0f, 0.5f), new Vector2f(0f, 0.95f));
						}
						this.state = EnemyBashAction.State.WaitForUI;
						return;
					}
				case EnemyBashAction.State.WaitForUI:
					break;
				case EnemyBashAction.State.Finish:
					this.controller.InterfaceController.OnTextboxComplete -= this.InteractionComplete;
					this.complete = true;
					break;
				default:
					return;
			}
		}

		public void InteractionComplete()
		{
			if (this.messages.Count == 0)
			{
				this.state = EnemyBashAction.State.Finish;
				return;
			}
			this.state = EnemyBashAction.State.PopMessage;
		}

		private const int POWER_INDEX = 0;

		private const int BLINK_DURATION = 8;

		private const int BLINK_COUNT = 2;

		private EnemyBashAction.State state;

		private float power;

		private EnemyCombatant combatant;

		private Stack<string> messages;

		private Stack<Tuple<Combatant, StatSet>> statSets;

		private enum State
		{
			Initialize,
			PopMessage,
			WaitForUI,
			Finish
		}
	}
}
