using hangxiaodian.Helper;
using hangxiaodian.Model.JsonDataModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Serialization.Json;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
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

        private async void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            tbxStuNo.IsEnabled = tbxStuPwd.IsEnabled = btnLogin.IsEnabled = false;
            string stuNo = tbxStuNo.Text;
            string stuPwd = tbxStuPwd.Password;
            if (stuNo == "")
            {
                await new MessageDialog("请输入学号").ShowAsync();
                tbxStuNo.IsEnabled = tbxStuPwd.IsEnabled = btnLogin.IsEnabled = true;
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
            catch (Exception)
            {
                await new MessageDialog("请检查网络连接").ShowAsync();
                tbxStuNo.IsEnabled = tbxStuPwd.IsEnabled = btnLogin.IsEnabled = true;
                return;
            }
            finally
            {
                statusBar.ProgressIndicator.ProgressValue = 0;
                statusBar.ProgressIndicator.Text = "";
                await statusBar.ProgressIndicator.ShowAsync();
            }
            LoginData resObj = null;
            var jsonObj = new DataContractJsonSerializer(typeof(LoginData));
            try
            {
                resObj = jsonObj.ReadObject(new MemoryStream(Encoding.UTF8.GetBytes(res))) as LoginData;
            }
            catch (Exception)
            {
                await new MessageDialog("数据解析错误，请重新登录").ShowAsync();
                tbxStuNo.IsEnabled = tbxStuPwd.IsEnabled = btnLogin.IsEnabled = true;
                return;
            }
            var localSettings = ApplicationData.Current.LocalSettings;
            ApplicationDataCompositeValue userinfo = new ApplicationDataCompositeValue();
            userinfo["stuNo"] = resObj.mNo;
            userinfo["name"] = resObj.name;
            userinfo["department"] = resObj.department;
            userinfo["office"] = resObj.office;
            userinfo["jsessionid"] = resObj.jsessionid;
            localSettings.Values["userinfo"] = userinfo;
            Frame.Navigate(typeof(MainPage));
        }
    }
}
