using UnityEngine;
using Zenject;

namespace Adv
{
	public class AdvInstaller : MonoInstaller
	{
		public override void InstallBindings()
		{
			Container
				.Bind<AdvManager>()
				.FromComponentInHierarchy().AsSingle();

			Container
				.Bind<Engine.AdvEngineManager>()
				.FromComponentInHierarchy().AsSingle();
		}
	}
}