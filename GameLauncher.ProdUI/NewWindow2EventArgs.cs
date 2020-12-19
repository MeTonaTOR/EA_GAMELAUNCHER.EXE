using System.ComponentModel;

namespace GameLauncher.ProdUI
{
	public class NewWindow2EventArgs : CancelEventArgs
	{
		private object ppDisp;

		public object PPDisp
		{
			get
			{
				return ppDisp;
			}
			set
			{
				ppDisp = value;
			}
		}

		public NewWindow2EventArgs(ref object ppDisp, ref bool cancel)
		{
			this.ppDisp = ppDisp;
			base.Cancel = cancel;
		}
	}
}
