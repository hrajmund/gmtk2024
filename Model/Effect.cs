using Gmtk2024.Scripts;

namespace Gmtk2024.Model
{
    public abstract class Effect
    {
        public abstract void Apply(GoldenNugget coin);
        public abstract string ShortHumanReadable();
        public abstract string HumanReadable();
        public abstract int TextureIndex();
    }
}
