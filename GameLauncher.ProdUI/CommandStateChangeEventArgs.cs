using System;

namespace GameLauncher.ProdUI
{
	public class CommandStateChangeEventArgs : EventArgs
	{
		private long command;

		private bool enable;

		public long Command
		{
			get
			{
				return command;
			}
			set
			{
				command = value;
			}
		}

		public bool Enable
		{
			get
			{
				return enable;
			}
			set
			{
				enable = value;
			}
		}

		public CommandStateChangeEventArgs(long command, ref bool enable)
		{
			this.command = command;
			this.enable = enable;
		}
	}
}
