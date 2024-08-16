using Gmtk2024.Scripts;

namespace Gmtk2024.Model
{
	public abstract class Effect
	{
		public abstract void Apply(GoldenNugget nugget);
		public abstract string HumanReadable();
	}
}
