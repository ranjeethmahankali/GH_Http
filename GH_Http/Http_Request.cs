﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Linq;
using System.Text;

namespace GH_Http
{
    public class Http_Request
    {
        public static string POST(string url, Dictionary<string,string> dict_data)
        {
            NameValueCollection data = getValues(dict_data);

            byte[] result;
            using (WebClient client = new WebClient())
            {
                result = client.UploadValues(url,"POST",data);
            }

            return Encoding.UTF8.GetString(result);
        }

        public static string GET(string url, Dictionary<string,string> data)
        {
            string query = queryString(data);

            string result;
            using (WebClient client = new WebClient())
            {
                result = client.DownloadString(url+"?"+query);
            }

            return result;
        }

        public static string queryString(Dictionary<string, string> data)
        {
            string query = "";
            int i = 0;
            foreach(string key in data.Keys)
            {
                query += key+"="+data[key];
                if(i < data.Keys.Count - 1) { query += "&"; }
                i++;
            }

            return query;
        }

        public static NameValueCollection getValues(Dictionary<string,string> data)
        {
            NameValueCollection final = new NameValueCollection();
            foreach(string key in data.Keys)
            {
                final.Add(key, data[key]);
            }

            return final;
        }
    }
}
