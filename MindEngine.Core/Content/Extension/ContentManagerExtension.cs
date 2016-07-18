namespace MindEngine.Core.Content.Extension
{
    using System;
    using System.Threading;
    using Microsoft.Xna.Framework.Content;

    public static class ContentManagerExtension
    {
        public static void LoadAsync<T>(this ContentManager content, string assetName, Action<T> assetAction)
        {
            ThreadPool.QueueUserWorkItem(
                callback =>
                {
                    var asset = content.Load<T>(assetName);

                    assetAction?.BeginInvoke(asset, null, null);
                });
        }
    }
}
