using System;
using System.Windows;
using System.Windows.Navigation;
using BarcodeScanner;
using Microsoft.Phone.Controls;
using WPCordovaClassLib.Cordova;
using WPCordovaClassLib.Cordova.Commands;
using WPCordovaClassLib.Cordova.JSON;

namespace Cordova.Extension.Commands
{
	public class BarcodeScanner : BaseCommand
	{
		private PhoneApplicationFrame currentRootVisual;

		public void scan(string options)
		{
			Deployment.Current.Dispatcher.BeginInvoke(() =>
			{
				this.currentRootVisual = Application.Current.RootVisual as PhoneApplicationFrame;
				this.currentRootVisual.Navigated += this.OnFrameNavigated;
				this.currentRootVisual.Navigate(new Uri("/Plugins/com.phonegap.plugins.barcodescanner/Scan.xaml", UriKind.Relative));
			});            
		}

		public void OnScanFailed(string error)
		{
			this.DispatchCommandResult(new PluginResult(PluginResult.Status.ERROR, error));
		}

		public void OnScanSucceeded(BarcodeScannerResult scanResult)
		{
			var resultString = JsonHelper.Serialize(scanResult);
			this.DispatchCommandResult(new PluginResult(PluginResult.Status.OK, resultString));
		}

		private void OnFrameNavigated(object sender, NavigationEventArgs e)
		{
			var scanPage = e.Content as Scan;
			if (scanPage != null)
			{
				scanPage.BarcodeScannerPlugin = this;
			}
		}
	}
}