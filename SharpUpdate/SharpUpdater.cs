using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace SharpUpdate
{
	/// <summary>
	/// Provides application update support in C#
	/// </summary>
	public class SharpUpdater
	{
		/// <summary>
		/// Parent form
		/// </summary>
		private Form ParentForm;

		/// <summary>
		/// Parent assembly
		/// </summary>
		private Assembly ParentAssembly;

		/// <summary>
		/// Parent name
		/// </summary>
		private string ParentPath;

		/// <summary>
		/// Holds the program-to-update's info
		/// </summary>
		private SharpUpdateLocalAppInfo[] LocalApplicationInfos;
		private List<SharpUpdateLocalAppInfo[]> LocalApplicationInfosList = new List<SharpUpdateLocalAppInfo[]>();
		/// <summary>
		/// Holds all the jobs defined in update xml
		/// </summary>
		private SharpUpdateXml[] JobsFromXMLL;
		public List<List<int>> ValidJobsList = new List<List<int>>();
		public List<Version> MaxVersionList = new List<Version>();
		public List<Version> CurrentVersionList = new List<Version>();
		public List<SharpUpdateXml[]> ListOfUpdates = new List<SharpUpdateXml[]>();

		/// <summary>
		/// Total number of jobs
		/// </summary>
		private int Num_Jobs = 0;

		/// <summary>
		/// Lists containing all informtion for files update
		/// </summary>
		private List<string> tempFilePaths = new List<string>();
		private List<string> currentPaths = new List<string>();
		private List<string> newPaths = new List<string>();
		private List<string> launchArgss = new List<string>();
		private List<JobType> jobtypes = new List<JobType>();
		public Assembly UpdaterGene;
		private int acceptJobs = 0;
		public static string User = "";
		public List<string> Programs = new List<string>();

		/// <summary>
		/// Thread to find update
		/// </summary>
		private BackgroundWorker BgWorker;

		/// <summary>
		/// Uri of the update xml on the server
		/// </summary>
		private Uri UpdateXmlLocation;
		private Uri CertificateXmlLocation;
		//private readonly Uri UpdateXmlLocation = new Uri("https://raw.githubusercontent.com/henryxrl/SharpUpdate/master/project.xml");
		//private readonly Uri UpdateXmlLocation = new Uri(new FileInfo(@"..\..\..\project.xml").FullName);       // for local testing

		/// <summary>
		/// Creates a new SharpUpdater object
		/// </summary>
		/// <param name="a">Parent ssembly to be attached</param>
		/// <param name="owner">Parent form to be attached</param>
		/// <param name="XMLOnServer">Uri of the update xml on the server</param>
		public SharpUpdater(Assembly a, Form owner, Uri XMLOnServer,string UserName,string PassWord)
		{
			ParentForm = owner;
			ParentAssembly = a;
			ParentPath = a.Location;

			UpdateXmlLocation = XMLOnServer;
			CertificateXmlLocation = new Uri("https://raw.githubusercontent.com/Omararafa/Tool-Deployment-Order/main/Certificates.xml");
			ServicePointManager.Expect100Continue = true;
			ServicePointManager.DefaultConnectionLimit = 9999;
			//ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls;
			ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
			// Request the update.xml
			HttpWebRequest req = (HttpWebRequest)WebRequest.Create(CertificateXmlLocation.AbsoluteUri);
			// Read for response
			HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
			resp.Close();
			var Certificates=CertificateXml.ParseCertificate(CertificateXmlLocation,UserName,PassWord);


			if(Certificates[0][0]== "User Not Found")
            {
				MessageBoxEx.Show("Mail or Password is wrong", "Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
				//Thread.Sleep(2000);
				User = "User Not Found";
				Application.Exit();
			}
            else
            {
				foreach(var it in Certificates[0])
                {
					Programs.Add(it);
                }
				//resp.StatusCode == HttpStatusCode.OK;
				// Set up backgroundworker
				BgWorker = new BackgroundWorker();
				BgWorker.DoWork += new DoWorkEventHandler(BgWorker_DoWork);
				BgWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BgWorker_RunWorkerCompleted);
			}
		}

		/// <summary>
		/// Checks for an update for the files passed.
		/// If there is an update, a dialog asking to download will appear
		/// </summary>
		public void DoUpdate()
		{
            if (BgWorker != null)
            {
				if (!BgWorker.IsBusy)
					BgWorker.RunWorkerAsync();
			}

		}

		/// <summary>
		/// Checks for/parses update.xml on server
		/// </summary>
		private void BgWorker_DoWork(object sender, DoWorkEventArgs e)
		{
			// Check for update on server
			if (!SharpUpdateXml.ExistsOnServer(UpdateXmlLocation))
				e.Cancel = true;
			else // Parse update xml
				e.Result = SharpUpdateXml.ParseSmart(UpdateXmlLocation);
		}

		/// <summary>
		/// After the background worker is done, prompt to update if there is one
		/// </summary>
		private void BgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			// If there is a file on the server
			if (!e.Cancelled)
			{
				
				JobsFromXMLL = (SharpUpdateXml[])e.Result;
				SharpUpdateXml[] JobsFromXMLSmart = SharpUpdateXml.ParseSmart(UpdateXmlLocation);
				SharpUpdateXml[] JobsFromXMLDeployer = SharpUpdateXml.ParseDeployer(UpdateXmlLocation);
				ListOfUpdates.Add(JobsFromXMLSmart);
				ListOfUpdates.Add(JobsFromXMLDeployer);

				// Check if the update is not null and is a newer version than the current application
				if (JobsFromXMLL != null)
				{
					foreach (var JobsFromXML in ListOfUpdates)
                    {
						Console.WriteLine("Number of updates from XML: " + JobsFromXML.Length);

						// create local app info according to update xml
						Num_Jobs = JobsFromXML.Length;
						Version CurrentVersion = new Version(0, 0, 0, 0);
						LocalApplicationInfos = new SharpUpdateLocalAppInfo[Num_Jobs];
						for (int i = 0; i < Num_Jobs; ++i)
						{
							/*if (Path.GetFileName(ParentPath).CompareTo(Path.GetFileName(JobsFromXML[i].FilePath)) == 0)
							{
								LocalApplicationInfos[i] = new SharpUpdateLocalAppInfo(JobsFromXML[i], ParentAssembly, ParentForm);
							}
							else
							{
								LocalApplicationInfos[i] = new SharpUpdateLocalAppInfo(JobsFromXML[i]);
							}*/
							LocalApplicationInfos[i] = new SharpUpdateLocalAppInfo(JobsFromXML[i]);
							LocalApplicationInfos[i].Print();
							CurrentVersion = LocalApplicationInfos[i].Version;
						}

						// validate all update jobs
						List<int> validJobs = new List<int>();
						Version MaxVersion = new Version(0, 0, 0, 0);
						for (int i = 0; i < Num_Jobs; ++i)
						{
							if (JobsFromXML[i].Version > MaxVersion)
							{
								MaxVersion = JobsFromXML[i].Version;

							}
							if (JobsFromXML[i].Tag == JobType.UPDATE)
							{
								if (!JobsFromXML[i].IsNewerThan(LocalApplicationInfos[i].Version))
								{

									continue;
								}
                                else
                                {
									validJobs.Add(i);
								}

							}


							
						}
						ValidJobsList.Add(validJobs);
						MaxVersionList.Add(MaxVersion);
						CurrentVersionList.Add(CurrentVersion);
						LocalApplicationInfosList.Add(LocalApplicationInfos);
					}

					#region Accept Form
					PluginsAcceptForm AccForm = new PluginsAcceptForm(ValidJobsList,ListOfUpdates,MaxVersionList,CurrentVersionList,Programs);
                    if (Programs.Count == 2)
                    {
						AccForm.Height = 190;
					}
                    else
                    {
						AccForm.Height = 190-40;
					}
					
					AccForm.Width = 700;
					double screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
					double screenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
					double windowWidth = AccForm.Width;
					double windowHeight = AccForm.Height;
					AccForm.Left = (screenWidth / 2) - (windowWidth / 2);
					AccForm.Top = (screenHeight / 2) - (windowHeight / 2);
					AccForm.ShowDialog();
					bool Action = AccForm.Action;
					int ProjectChoosen = AccForm.ProjectChoosen;
					#endregion


					// let user choose to accept update jobs
					bool showMsgBox = true;
					int count = 0;
                    if (Action)
                    {
						foreach (int i in ValidJobsList[ProjectChoosen])
						{
							count++;
							showMsgBox = false;

							// Ask to accept the update
							/*if (new SharpUpdateAcceptForm(LocalApplicationInfos[i], JobsFromXML[i], count, validJobs.Count).ShowDialog(LocalApplicationInfos[0].Context) == DialogResult.Yes)
							{
								acceptJobs++;
								DownloadUpdate(JobsFromXML[i], LocalApplicationInfos[i]); // Do the update
							}*/
							acceptJobs++;
							DownloadUpdate(ListOfUpdates[ProjectChoosen][i], LocalApplicationInfosList[ProjectChoosen][i]); // Do the update
						}

						if (showMsgBox)
						{
							MessageBoxEx.Show(ParentForm, "You have the latest versions already!");
						}
						else
						{
							if (acceptJobs > 0)
								InstallUpdate();
						}
					}
                    else
                    {
						Application.Exit();
					}

				}
                else
                {
					MessageBoxEx.Show(ParentForm, "You have the latest versions already!");
					
				}
					
				
			}
			else
				MessageBoxEx.Show(ParentForm, "No update information found!");
		}

		/// <summary>
		/// Download the update
		/// </summary>
		/// <param name="update">The update xml info</param>
		/// <param name="applicationInfo">An SharpUpdateInfo object containing application's info</param>
		private void DownloadUpdate(SharpUpdateXml update, SharpUpdateLocalAppInfo applicationInfo)
		{
			if (update.Tag == JobType.REMOVE)
			{
				tempFilePaths.Add("");
				currentPaths.Add("");
				newPaths.Add(Path.GetFullPath(applicationInfo.ApplicationPath));
				launchArgss.Add(update.LaunchArgs);
				jobtypes.Add(update.Tag);
				return;
			}

			SharpUpdateDownloadForm form = new SharpUpdateDownloadForm(update.Uri, update.MD5, applicationInfo.ApplicationIcon);
			DialogResult result = form.ShowDialog(applicationInfo.Context);
			result = DialogResult.OK;

			if (result == DialogResult.OK)
			{
				string updatepath= Environment.ExpandEnvironmentVariables(update.FilePath);
				List<string> Y = new List<string>();
				string currentPath = "";
				if (!(updatepath == null || updatepath == ""))
				{
					char separator = '\\';
					foreach (var i in updatepath.Split(separator))
					{
						Y.Add(i);
					}
				}
				if (Y[Y.Count - 1] == "Updater.dll")
				{
					currentPath = (update.Tag == JobType.UPDATE) ? updatepath : "";
				}
                else
                {
					currentPath = (update.Tag == JobType.UPDATE) ? applicationInfo.ApplicationAssembly.Location : "";
				}

				List<string> x = new List<string>();
				if (!(currentPath == null || currentPath == ""))
				{
					char separator = '\\';
					foreach (var i in currentPath.Split(separator))
					{
						x.Add(i);
					}
				}
				string newPath= "";
				try
                {
                    if (x[x.Count - 1] == "Updater.dll")
                    {
						string Pathhh = Environment.ExpandEnvironmentVariables(update.FilePath);
						newPath = (update.Tag == JobType.UPDATE) ? Path.GetFullPath(Pathhh) : Path.GetFullPath(applicationInfo.ApplicationPath);
					}
					newPath = (update.Tag == JobType.UPDATE) ? Path.GetFullPath(Path.GetDirectoryName(currentPath).ToString()+"\\"+x[x.Count-1]) : Path.GetFullPath(applicationInfo.ApplicationPath);
				}
                catch
                {
					newPath = (update.Tag == JobType.UPDATE) ? Path.GetFullPath(update.FilePath) : Path.GetFullPath(applicationInfo.ApplicationPath);
				}

				Directory.CreateDirectory(Path.GetDirectoryName(newPath));

				tempFilePaths.Add(form.TempFilePath);
				currentPaths.Add(currentPath);
				newPaths.Add(newPath);
				launchArgss.Add(update.LaunchArgs);
				jobtypes.Add(update.Tag);
			}
			else if (result == DialogResult.Abort)
			{
				MessageBoxEx.Show(ParentForm, "The update download was cancelled.\nThis program has not been modified.", "Update Download Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
			else
			{
				MessageBoxEx.Show(ParentForm, "There was a problem downloading the update.\nPlease try again later.", "Update Download Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
		}

		/// <summary>
		/// Install all the updates
		/// </summary>
		private void InstallUpdate()
		{
			MessageBoxEx.Show("Plugin Updated Successfully", "Success");
			UpdateApplications();
			Application.Exit();
		}

		/// <summary>
		/// Hack to close program, delete original, move the new one to that location
		/// </summary>
		private void UpdateApplications()
		{
			string argument_start = "/C choice /C Y /N /D Y /T 4 & Start \"\" /D \"{0}\" \"{1}\"";
			string argument_update = "/C choice /C Y /N /D Y /T 4 & Del /F /Q \"{0}\" & choice /C Y /N /D Y /T 2 & Move /Y \"{1}\" \"{2}\"";
			string argument_update_start = argument_update + " & Start \"\" /D \"{3}\" \"{4}\" {5}";
			string argument_add = "/C choice /C Y /N /D Y /T 4 & Move /Y \"{0}\" \"{1}\"";
			string argument_remove = "/C choice /C Y /N /D Y /T 4 & Del /F /Q \"{0}\"";
			string argument_complete = "";

			int curAppidx = -1;
			for (int i = 0; i < acceptJobs; ++i)
			{
				string curName = Path.GetFileName(currentPaths[i]);
				/*if (curName.CompareTo("") != 0 && Path.GetFileName(ParentPath).CompareTo(curName) == 0)
				{
					curAppidx = i;
					continue;
				}*/
				
				if (jobtypes[i] == JobType.ADD)
				{
					argument_complete = string.Format(argument_add, tempFilePaths[i], newPaths[i]);
					Console.WriteLine("add: " + argument_complete);
				}
				else if (jobtypes[i] == JobType.UPDATE)
				{
					argument_complete = string.Format(argument_update, currentPaths[i], tempFilePaths[i], newPaths[i]);
					Console.WriteLine("update: " + argument_complete);
				}
				else
				{
					argument_complete = string.Format(argument_remove, newPaths[i]);
					Console.WriteLine("remove: " + argument_complete);
				}

				ProcessStartInfo cmd = new ProcessStartInfo
				{
					Arguments = argument_complete,
					WindowStyle = ProcessWindowStyle.Hidden,
					CreateNoWindow = true,
					FileName = "cmd.exe"
				};
				Process.Start(cmd);
			}

		/*	if (curAppidx > -1)
			{
				argument_complete = string.Format(argument_update_start, currentPaths[curAppidx], tempFilePaths[curAppidx], newPaths[curAppidx], Path.GetDirectoryName(newPaths[curAppidx]), Path.GetFileName(newPaths[curAppidx]), launchArgss[curAppidx]);
				Console.WriteLine("Update and run main app: " + argument_complete);
			}
			else
			{
				argument_complete = string.Format(argument_start, Path.GetDirectoryName(ParentPath), Path.GetFileName(ParentPath));
				Console.WriteLine("Run main app: " + argument_complete);
			}

			ProcessStartInfo cmd_main = new ProcessStartInfo
			{
				Arguments = argument_complete,
				WindowStyle = ProcessWindowStyle.Hidden,
				CreateNoWindow = true,
				FileName = "cmd.exe"
			};
			Process.Start(cmd_main);*/
		}
	}
}