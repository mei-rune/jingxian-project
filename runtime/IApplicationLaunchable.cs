
using System;
using System.Collections.Generic;

namespace jingxian.core.runtime
{

	[ExtensionContract(Constants.Points.Applications)]
	public interface IApplicationLaunchable
	{
		int Launch( IApplicationContext context );
	}
}