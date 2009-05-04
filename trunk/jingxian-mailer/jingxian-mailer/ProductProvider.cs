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

using System.Drawing;
using Empinia.Core.Runtime;
using Empinia.UI;
using Empinia.UI.Workbench;
using jingxian.Resources;

namespace jingxian
{
	/// <summary>
	/// Provides the application product configuration used to configure the workbench.
	/// </summary>
	internal class ProductProvider: IProductProvider, ISplashScreenConfigurationProvider
	{
        private DefaultSplashScreenConfiguration m_SplashScreenConfiguration;
        
		public Icon Icon
		{
			get
			{
                return jingxian.Resources.Resources.jingxian;
			}
		}

		public string ApplicationLaunchableId
		{
			get
			{
				return WorkbenchConstants.ApplicationLaunchableId;
			}
		}

		public ISplashScreenConfiguration SplashScreenConfiguration
		{
			get
			{
				if (m_SplashScreenConfiguration== null)
				{
					m_SplashScreenConfiguration = new DefaultSplashScreenConfiguration();
				}
				return m_SplashScreenConfiguration;
			}
		}
	}
}
