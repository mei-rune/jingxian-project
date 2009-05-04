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

using System.Configuration;
using Empinia.Core.Runtime.Preference;
namespace jingxian.ui.Properties
{
    using Empinia.UI.Workbench;

	[SettingsProvider(Empinia.Core.Runtime.Preference.Constants.PreferenceSettingsProviderTypeName)]
    [PreferenceStore("jingxian.ui.preferencestore", jingxian.ui.Constants.BundleId, typeof(Settings))]
	// This class allows you to handle specific events on the settings class:
	//  The SettingChanging event is raised before a setting's value is changed.
	//  The PropertyChanged event is raised after a setting's value is changed.
	//  The SettingsLoaded event is raised after the setting values are loaded.
	//  The SettingsSaving event is raised before the setting values are saved.
	internal sealed partial class Settings
	{

		//public Settings()
		//{
		//  // // To add event handlers for saving and changing settings, uncomment the lines below:
		//  //
		//  // this.SettingChanging += this.SettingChangingEventHandler;
		//  //
		//  // this.SettingsSaving += this.SettingsSavingEventHandler;
		//  //
		//}

		//private void SettingChangingEventHandler(object sender, System.Configuration.SettingChangingEventArgs e)
		//{
		//  // Add code to handle the SettingChangingEvent event here.
		//}

		//private void SettingsSavingEventHandler(object sender, System.ComponentModel.CancelEventArgs e)
		//{
		//  // Add code to handle the SettingsSaving event here.
		//}
	}
}
