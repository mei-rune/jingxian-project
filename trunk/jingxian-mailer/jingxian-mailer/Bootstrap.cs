#region Copyright and License
/* Licensed to the Empinia Project under one or more contributor
 * license agreements. See the NOTICE file distributed with this work
 * for additional information regarding copyright ownership.
 * The Empinia Project licenses this file to You under the Apache License,
 * Version 2.0 (the "License"); you may not use this file except in 
 * compliance with the License. You may obtain a copy of the License at
 *
 * http://www.empinia.org/licenses/ApacheSoftwareLicense-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. 
 * See the License for the specific language governing permissions and 
 * limitations under the License.
 *
 * The Empinia Project itself is located at http://www.empinia.org.
 * 
 */

#endregion

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Empinia.Core.Runtime;
using Empinia.Core.Runtime.Internal;
using Empinia.UI.Workbench.Internal;

using PlatformRuntime = Empinia.Core.Runtime.Internal.Platform;

namespace jingxian
{
	internal static class Bootstrap
	{
		public const string RepositoryName = "Empinia";  //NON-NLS-1

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		private static int Main(string[] args)
		{
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);


            jingxian.install.DbInsteller installer = new jingxian.install.DbInsteller();
            if (!installer.Instell(true))
                return -1;


			System.Threading.Thread.CurrentThread.Name = "jingxian GUI main";  //NON-NLS-1
			log4net.ILog log = null;

			CommandLineArguments arguments = new CommandLineArguments(args);
			if (!arguments.Succeeded)
			{
				// TODO write out error info
				System.Console.WriteLine(arguments.GetUsage());
				return 1;
			}
			else
			{
				if (RuntimeConfiguration.Instance.EnableLogging
					&& !arguments.IsVerboseDefined)
				{
					Logging.ConfigureLog4net(typeof(Bootstrap));
					log = log4net.LogManager.GetLogger(typeof(Bootstrap));
					log.Debug("Starting...");
				}
			}


            string binDirectory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);


            IBatisNet.DataMapper.Configuration.DomSqlMapBuilder builder = new IBatisNet.DataMapper.Configuration.DomSqlMapBuilder();
            domainModel.SessionFactory.Mapper = builder.Configure(System.IO.Path.Combine(binDirectory, "db.Config"));

            Dictionary<string, object> context = new Dictionary<string, object>();
            context["startPerspective"] = jingxian.ui.Constants.PerspectiveId;
            context["layoutProvider"] = jingxian.ui.LayoutProvider.PageLayoutProviderPointId;

			int exitCode = RuntimeConfiguration.Instance.ShowSplashScreen
											? PlatformLauncher.Launch(new ProductProvider(), context )
											: PlatformRuntime.Launch(new ProductProvider(), arguments, context );

			string msg = string.Format("Exited with code {0}.", exitCode);
			if (log != null)
			{
				if (exitCode == 0)
				{
					log.Debug(msg);
				}
				else
				{
					log.Error(msg);
				}
			}
			else
			{
					System.Console.WriteLine(msg);
			}

			return exitCode;
		}
	}
}
