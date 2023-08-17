using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Store.Preview.InstallControl;
using Newtonsoft.Json;
using System.Web;
using System.Net.Http.Json;

namespace Edid_GUI
{

    public class APICalls
    {

        private string APIroute = "https://csatnc-eng-dev/LCDEDIDAPI";
        private HttpClient hc;
        private HttpClient huser;
        public APICalls()
        {
            var handler = new HttpClientHandler()
            {
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };
            try
            {
                hc = new HttpClient(handler);
                huser = new HttpClient(handler);
                string key = ConfigurationManager.AppSettings["XApiKey"];
                string keyUser = ConfigurationManager.AppSettings["XAPIUser"];
                //EDIT XAPI Key here
                hc.DefaultRequestHeaders.Add("XApiKey", key);
                huser.DefaultRequestHeaders.Add("XApiKey", keyUser);
            }
            catch
            {
                throw new Exception("Could not establish connection with the API");
            }
        }
        public async Task<string> getEDID(string mbpn)
        {
            try
            {
                var x = await hc.GetStringAsync(APIroute + $@"/API/LCDEDIDAPI/getEDID?mbpn={mbpn}");
                return x;
            }
            catch (Exception ex)
            {
                return "EDID null " + ex.ToString();
            }
        }
        public async Task<string> setEDID(string mbpn, string EDID)
        {
            try
            {
                var data = Uri.EscapeDataString(JsonConvert.SerializeObject(new set_EDID(mbpn, EDID)));
                var x = await hc.PostAsync(APIroute + $@"/API/LCDEDIDAPI/setEDID?data={data}", null);
                return "0";
            }
            catch
            {
                return "1";
            }
        }
        public async Task<User> GetUserInfo(string username, string password)
        {
            try
            {
                UserResponse response = new UserResponse();
                password = HttpUtility.UrlEncode(password);
                var uri = string.Format(ConfigurationManager.AppSettings["UserAPI"] + "/API/User/GetUserInfo?badgeNum={0}&password={1}", username, password);
                response = await huser.GetFromJsonAsync<UserResponse>(uri);
                return response.User;
            }
            catch
            {
                return null;
            }
        }
        private class set_EDID
        {
            public string mbpn, EDID;
            public set_EDID(string mbpn,string EDID)
            {
                this.mbpn = mbpn;
                this.EDID = EDID;
            }
        }
        public class UserResponse
        {
            public string StatusMessage { get; set; }
            public User User { get; set; }
            public UserResponse()
            {
                User = new User();
                StatusMessage = String.Empty;
            }
        }
        public class User
        {
            public string BadgeNum { get; set; }
            public string Password { get; set; }
            public string Role { get; set; }
            public User()
            {
                BadgeNum = String.Empty;
                Password = String.Empty;
                Role = String.Empty;
            }
        }
    }

}