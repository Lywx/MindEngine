namespace MindEngine.Audio
{
    using System;

    public class MMAudio : IEquatable<MMAudio>
    {
        public MMAudio(MMAudioAsset asset)
        {
            if (asset == null)
            {
                throw new ArgumentNullException(nameof(asset));
            }

            this.Name = asset.Name;
        }

        public string Name { get; }

        #region IEquable

        public bool Equals(MMAudio other)
        {
            return other != null && this.Name == other.Name;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as MMAudio);
        }

        public override int GetHashCode()
        {
            return this.Name?.GetHashCode() ?? 0;
        }

        #endregion
    }
}