using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;

namespace SharpUpdate
{
	public class CertificateXml
	{
		/// <summary>
		/// The update version #
		/// </summary>
		/// 
		public string User { get; }

		public string Mail { get; }

		/// <summary>
		/// The location of the update binary
		/// </summary>
		public string Password { get; }

		/// <summary>
		/// The file path of the binary
		/// for use on local computer
		/// </summary>
		public XmlAttributeCollection Programs { get; }

		/// <summary>
		/// The MD5 of the update's binary
		/// </summary>
		public string Program { get; }



		/// <summary>
		/// Creates a new SharpUpdateXml object
		/// </summary>
		public CertificateXml(string user, string mail, string password, XmlAttributeCollection programs, string program)
		{
			User = user;
			Mail = mail;
			Password = password;
			Programs = programs;
			Program = program;
		}


		public static List<List<string>> ParseCertificate(Uri location,string UserName, string PassWord)
		{
			string user = "";
			string mail = "";
			string password = "";
			XmlAttributeCollection programs;
			string program = "";
			List<List<string>> result = new List<List<string>>();
			try
			{
				// Load the document
				ServicePointManager.ServerCertificateValidationCallback = (s, ce, ch, ssl) => true;
				XmlDocument doc = new XmlDocument();
				doc.Load(location.AbsoluteUri);

				// Gets the appId's node with the update info
				// This allows you to store all program's update nodes in one file
				// XmlNode updateNode = doc.DocumentElement.SelectSingleNode("//update[@appID='" + appID + "']");
				XmlNodeList updateNodes = doc.DocumentElement.SelectNodes("/Certificates/User");
				int count = 1;
				foreach (XmlNode updateNode in updateNodes)
				{
					// If the node doesn't exist, there is no update
					/*if (updateNode == null)
						return null;*/
					List<string> ProgList = new List<string>();


					// Parse data
					mail = updateNode["Mail"].InnerText;
					password = updateNode["Password"].InnerText;
					var programss = updateNode["Programs"].ChildNodes;
					if (mail == UserName && password == PassWord)
                    {
						foreach(XmlNode Pr in programss)
                        {
							ProgList.Add(Pr.InnerText);
                        }
						result.Add(ProgList);
					}
                    else
                    {
                        if (count == updateNodes.Count)
                        {
							List<string> NotFound = new List<string>()
						{
							"User Not Found"
						};
							result.Add(NotFound);
						}

                    }

					count++;
				}

				return result;
			}
			catch (Exception ex)
			{
				string test = ex.Message;
				return result;
			}
		}

	}
}
