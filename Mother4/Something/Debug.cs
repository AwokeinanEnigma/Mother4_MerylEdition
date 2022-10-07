using SunsetRhapsody.Battle;
using SunsetRhapsody.Battle.Combatants;
using SunsetRhapsody.Psi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunsetRhapsody.SOMETHING
{
    public class Debug : AUXBase
    {
        public override int AUCost => 20; //{ get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override TargetingMode TargetMode => TargetingMode.AllEnemies;//; set => throw new NotImplementedException(); }
        public override int[] Symbols => Array.Empty<int>(); //{ get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override string QualifiedName => "1";//{ get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override string Key => "1"; //{ get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        internal override IPsi identifier => new DefensivePsi(); //{ get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        internal override void Act(Combatant[] combatants)
        {
            Console.WriteLine("bro");
        }

        internal override void Animate()
        {
            throw new NotImplementedException();
        }

        internal override void Initialize()
        {
            throw new NotImplementedException();
        }

        internal override void ScaleToLevel(PlayerCombatant combatant)
        {
            throw new NotImplementedException();
        }
    }
}
