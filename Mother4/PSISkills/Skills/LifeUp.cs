using SFML.System;
using Mother4.Battle;
using Mother4.Battle.Actions;
using Mother4.Battle.Combatants;
using Mother4.Battle.PsiAnimation;
using Mother4.Battle.UI;
using Mother4.Data;
using Mother4.Psi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Carbine.Graphics;

namespace Mother4.SOMETHING
{
    public class LifeUp : PSIBase
    {
        public override int AUCost => 1; //{ get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override TargetingMode TargetMode => TargetingMode.AllPartyMembers;//; set => throw new NotImplementedException(); }
        public override int[] Symbols => new int[2]; //{ get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override string QualifiedName => "PK LifeUp";//{ get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override string Key => "1"; //{ get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        internal override IPsi identifier => new AssistivePsi(); //{ get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public LifeUp()
        {
       //     Console.WriteLine("THE PURPOSE OF MAN IS TO HEAL");
        }



        internal override void Initialize(PlayerCombatant combantant, BattleInterfaceController interfaceController, PlayerPsiAction action, Combatant[] targets)
        {
            string message = string.Format("{0} tried {1} {2}!", CharacterNames.GetName(combantant.Character), QualifiedName, PsiLetters.Get(action.psiLevel));

            action.state = PlayerPsiAction.State.WaitForUI;
            interfaceController.OnTextboxComplete += OnTextboxComplete;
            interfaceController.ShowMessage(message, false);
            interfaceController.PopCard(combantant.ID, 20);

            void OnTextboxComplete()
            {
                interfaceController.OnTextboxComplete -= OnTextboxComplete;
                action.state = PlayerPsiAction.State.Animate;
            }
            Console.WriteLine("initialize");
        }

        internal override void Animate(PlayerCombatant combantant, BattleInterfaceController interfaceController, PlayerPsiAction action, Combatant[] targets)
        {
            IndexedColorGraphic graphic = new IndexedColorGraphic(Paths.GRAPHICS + "psilifeup1.dat", "lifeupalpha", new Vector2f(160, 90), 579600);
            interfaceController.pipeline.Add(graphic);
            graphic.OnAnimationComplete += Graphic_OnAnimationComplete;
            action.state = PlayerPsiAction.State.WaitForUI;

            void Graphic_OnAnimationComplete(AnimatedRenderable renderable)
            {
                interfaceController.pipeline.Remove(renderable);
                graphic.Visible = false;
                graphic.OnAnimationComplete -= Graphic_OnAnimationComplete;
                action.state = PlayerPsiAction.State.DamageNumbers;
            };
        }

        internal override void Act(Combatant[] combatants, PlayerCombatant combantant, BattleInterfaceController interfaceController, PlayerPsiAction action)
        {
            Console.WriteLine("act");
            foreach (Combatant combatant in combatants)
            {

                DamageNumber damageNumber = interfaceController.AddDamageNumber(combatant, 32, Paths.GRAPHICS + "numbersethealing.dat");
                damageNumber.OnComplete += DamageNumber_OnComplete; ;
                StatSet statChange = new StatSet
                {
                    HP = +32
                };
                combatant.AlterStats(statChange);
                if (combatant as EnemyCombatant != null)
                {
                    interfaceController.BlinkEnemy(combatant as EnemyCombatant, 3, 2);
                }
            }
            StatSet statChange2 = new StatSet
            {
                PP = -this.AUCost,
                Meter = 0.026666667f
            };
            combantant.AlterStats(statChange2);
            action.state = PlayerPsiAction.State.WaitForUI;
            void DamageNumber_OnComplete(DamageNumber sender)
            {
                action.state = PlayerPsiAction.State.Finish;
            }
        }

        internal override void ScaleToLevel(PlayerCombatant combatant)
        {
        }

        internal override void ShowUnavaliableMessage(PlayerCombatant combatant, BattleInterfaceController interfaceController)
        {
            interfaceController.ShowMessage("Not enough AU!", false);
        }

        internal override void Finish(Combatant[] combatants, PlayerCombatant combantant, BattleInterfaceController interfaceController, PlayerPsiAction action)
        {
            interfaceController.PopCard(combantant.ID, 0);
            action.Finish();
        }
    }
}
