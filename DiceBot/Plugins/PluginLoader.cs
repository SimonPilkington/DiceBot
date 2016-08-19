using System.Collections.Generic;
using System.IO;

using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;

namespace DiceBot.Plugins
{
	public sealed class PluginLoader
	{
		public static readonly string PluginPath = "./plugins";

		private static readonly PluginLoader instance = new PluginLoader();
		public static PluginLoader Instance => instance;

		[ImportMany(typeof(IDiceBotPlugin), AllowRecomposition = false, RequiredCreationPolicy = CreationPolicy.Shared)]
		private IEnumerable<IDiceBotPlugin> plugins = null;

		public IEnumerable<IDiceBotPlugin> LoadPlugins()
		{
			if (!Directory.Exists(PluginPath))
				return System.Linq.Enumerable.Empty<IDiceBotPlugin>();

			var catalog = new DirectoryCatalog(PluginPath, "*.dll");
			var compositionContainer = new CompositionContainer(catalog);

			compositionContainer.SatisfyImportsOnce(this);

			return plugins;
		}

		private PluginLoader()
		{ }
	}
}
