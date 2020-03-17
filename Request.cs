using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace NotePad
{
    class Request
    {
		public string GetRequest(string url)
		{
			try
			{
				string rt;

				WebRequest request = WebRequest.Create(url);

				WebResponse response = request.GetResponse();

				Stream dataStream = response.GetResponseStream();

				StreamReader reader = new StreamReader(dataStream);

				rt = reader.ReadToEnd();

				reader.Close();
				response.Close();
				return rt;
			}
			catch
			{
				Console.WriteLine("haya");
				return "hata";
			}
		}
        public Dictionary<string, float> Parserr(string data)

        {
            Dictionary<string, float> Dict = new Dictionary<string, float>();
            string[] arr = data.Split('{');
            string[] values = arr[2].Split(',');

            foreach (string value in values)
            {
                try
                {
                    string contry;
                    float val;
                    string[] arr2 = value.Split(':');
                    contry = arr2[0].Split('"')[1];
                    val = float.Parse(arr2[1]);
                    Dict.Add(contry, val);
                }
                catch
                {

                }
            }
            return Dict;
        }

    }
}
