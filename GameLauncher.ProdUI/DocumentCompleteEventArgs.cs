using System;

namespace GameLauncher.ProdUI
{
	public class DocumentCompleteEventArgs : EventArgs
	{
		private object ppDisp;

		private object url;

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

		public object Url
		{
			get
			{
				return url;
			}
			set
			{
				url = value;
			}
		}

		public DocumentCompleteEventArgs(object ppDisp, object url)
		{
			this.ppDisp = ppDisp;
			this.url = url;
		}
	}
}
