using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace hangxiaodian.Model.JsonDataModel
{
    [DataContract]
    class LoginData
    {
        [DataMember]
        public string mNo { get; set; }

        [DataMember]
        public string name { get; set; }

        [DataMember]
        public string department { get; set; }

        [DataMember]
        public string office { get; set; }

        [DataMember]
        public string jsessionid { get; set; }
    }
}
