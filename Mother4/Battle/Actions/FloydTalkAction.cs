using System;
using System.Diagnostics.Contracts;
using System.Text;
using Carbine;
using Carbine.Utility;
using Mother4.Battle.Combatants;
using Mother4.Data;
using Mother4.Utility;

namespace Mother4.Battle.Actions
{
    internal class FloydTalkAction : BattleAction
    {
        public FloydTalkAction(ActionParams aparams) : base(aparams)
        {
            this.combatant = (aparams.sender as PlayerCombatant);
            this.target = (aparams.targets[0] as EnemyCombatant);

            string article = this.target.Enemy.Article;
            string name = this.target.Enemy.PlayerFriendlyName;
            string like = this.target.Enemy.GetStringQualifiedName("thoughts");
            string subjective = this.target.Enemy.GetStringQualifiedName("subjective");

            StringBuilder stringBuilder = new StringBuilder();
            if (like == String.Empty)
            {
                stringBuilder.AppendFormat("@{0} tried chatting up {1}{2}.", CharacterNames.GetName(this.combatant.Character), article, name);
                stringBuilder.Append($"@[p:10].[p:10].[p:30].But... the {name} wouldn't respond!");
                this.message = stringBuilder.ToString();
                this.state = FloydTalkAction.State.Fail;
                //this.controller.Data.AddReplace("topicOfDiscussion", like);
                return;
            }
            stringBuilder.AppendFormat("@{0} tried chatting up {1}{2}.", CharacterNames.GetName(this.combatant.Character), article, name);
            stringBuilder.AppendFormat("@{0} had a lot to say about {1}.", Capitalizer.Capitalize(subjective), like);
            stringBuilder.Append("@[p:10].[p:10].[p:30].They really hit it off!");
            this.controller.Data.AddReplace("topicOfDiscussion", like);
            this.message = stringBuilder.ToString();
            this.state = FloydTalkAction.State.Initialize;
        }

        protected override void UpdateAction()
        {
            base.UpdateAction();
            switch (this.state)
            {
                case FloydTalkAction.State.Initialize:
                    this.controller.InterfaceController.OnTextboxComplete += this.InteractionComplete;
                    this.controller.InterfaceController.ShowMessage(this.message, false);
                    this.controller.InterfaceController.TalkSound.Play();
                    this.controller.InterfaceController.PopCard(this.combatant.ID, 12);
                    if (!this.controller.CombatantController.IsIdValid(this.target.ID))
                    {
                        Combatant[] factionCombatants = this.controller.CombatantController.GetFactionCombatants(BattleFaction.EnemyTeam);
                        this.target = (factionCombatants[Engine.Random.Next() % factionCombatants.Length] as EnemyCombatant);
                    }
                    this.state = FloydTalkAction.State.WaitForUI;
                    return;
                case FloydTalkAction.State.WaitForUI:
                    break;
                case FloydTalkAction.State.Finish:
                    {
                        this.controller.InterfaceController.OnTextboxComplete -= this.InteractionComplete;
                        this.controller.InterfaceController.PopCard(this.combatant.ID, 0);
                        int turnCount = 2 + (int)Math.Round(Math.Abs(GaussianRandom.Next(0.0, 0.6)));
                        this.combatant.SavedTargets = new Combatant[]
                        {
                    this.target
                        };
                        this.combatant.AddStatusEffect(StatusEffect.Talking, int.MaxValue);
                        this.target.SavedTargets = new Combatant[]
                        {
                    this.combatant
                        };
                        // darlton
                        this.target.AddStatusEffect(StatusEffect.Talking, turnCount);
                        this.target.OnStatusEffectChange += this.combatant.HandleStatusChangeFromOther;
                        this.complete = true;
                        break;
                    }
                case State.Fail:
                    {
                        this.controller.InterfaceController.OnTextboxComplete += this.Fail;
                        this.controller.InterfaceController.ShowMessage(this.message, false);
                        this.controller.InterfaceController.TalkSound.Play();
                        this.controller.InterfaceController.PopCard(this.combatant.ID, 12);
                        this.state = FloydTalkAction.State.WaitForUI;
                        break;
                    }
                case FloydTalkAction.State.FailFinish:
                    {
                        this.controller.InterfaceController.OnTextboxComplete -= this.InteractionComplete;
                        this.controller.InterfaceController.PopCard(this.combatant.ID, 0);
                        this.complete = true;
                        break;
                    }
                default:
                    return;
            }
        }
        public void Fail()
        {
            this.state = FloydTalkAction.State.FailFinish;
        }
        public void InteractionComplete()
        {
            this.state = FloydTalkAction.State.Finish;
        }

        private const string TOPIC_OF_DISCUSSION = "topicOfDiscussion";

        private const int CARD_POP_HEIGHT = 12;

        private const bool USE_BUTTON = false;

        private FloydTalkAction.State state;

        private string message;

        private PlayerCombatant combatant;

        private EnemyCombatant target;

        private enum State
        {
            Initialize,
            WaitForUI,
            Finish,
            Fail,
            FailFinish,
        }
    }
}
