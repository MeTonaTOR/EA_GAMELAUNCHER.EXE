using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace GameLauncher.ProdUI
{
	public class SSOLoginScreen : BaseScreen
	{
		private GameLauncherUI parentForm;

		private IContainer components;

		public SSOLoginScreen(GameLauncherUI launcherForm)
			: base(launcherForm)
		{
			parentForm = launcherForm;
			InitializeComponent();
		}

		public override void LoadScreen()
		{
			PerformLogin();
		}

		public void PerformLogin()
		{
			bool flag = false;
			string environmentVariable = Environment.GetEnvironmentVariable("EAGenericAuthToken");
			if (environmentVariable != null)
			{
				parentForm.LoadServerData();
				if (parentForm.SingleSignOnLogin(environmentVariable))
				{
					flag = true;
				}
				else
				{
					MessageBox.Show("Login expired, please re-login to run the game.", "SSO error");
				}
			}
			if (!flag)
			{
				parentForm.ReplaceScreen(ScreenType.Login, new LoginScreen(parentForm));
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
		}
	}
}
