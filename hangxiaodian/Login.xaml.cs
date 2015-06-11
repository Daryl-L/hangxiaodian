using hangxiaodian.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkID=390556 上有介绍

namespace hangxiaodian
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Login : Page
    {
        public Login()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 在此页将要在 Frame 中显示时进行调用。
        /// </summary>
        /// <param name="e">描述如何访问此页的事件数据。
        /// 此参数通常用于配置页。</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var statusBar = StatusBar.GetForCurrentView();
            statusBar.ForegroundColor = Color.FromArgb(0, 255, 0, 0);
        }

        private async void button_Click(object sender, RoutedEventArgs e)
        {
            string stuNo = tbxStuNo.Text;
            string stuPwd = tbxStuPwd.Password;
            if (stuNo == "")
            {
                await new MessageDialog("请输入学号").ShowAsync();
                return;
            }
            var param = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("username", stuNo),
                new KeyValuePair<string, string>("pwd", stuPwd)
            };
            var httpRequest = new HttpRequestHelper("http://incareer.hdu.edu.cn/m/p/telnet", Windows.Web.Http.HttpMethod.Post, param);
            string res = string.Empty;
            var statusBar = StatusBar.GetForCurrentView();
            statusBar.ProgressIndicator.ProgressValue = null;
            statusBar.ProgressIndicator.Text = "正在登录";
            await statusBar.ProgressIndicator.ShowAsync();
            try
            {
                res = await httpRequest.Request();
            }
            catch (Exception ex)
            {
                await new MessageDialog("请检查网络连接").ShowAsync();
                return;
            }
            finally
            {
                statusBar.ProgressIndicator.ProgressValue = 0;
                statusBar.ProgressIndicator.Text = "";
                await statusBar.ProgressIndicator.ShowAsync();
            }
        }
    }
}
