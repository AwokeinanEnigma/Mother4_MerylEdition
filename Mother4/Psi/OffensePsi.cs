using Mother4.SOMETHING;
using System;

namespace Mother4.Psi
{
	public struct OffensePsi : IPsi
	{
		public OffensePsi(PSIBase baseo) {
			_aux = baseo;
		}
		public OffensePsi(OffensePsi ability)
		{
			Console.WriteLine("hey");
			_aux = ability.aux;
		}


		private PSIBase _aux;
		public PSIBase aux
		{
			get { return _aux; }
			set
			{
				_aux = value;
			}
		}
	}
}