using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monocle
{
    public static class VirtualContent
    {
        private static List<VirtualAsset> assets;

        private static bool reloading;

        public static int Count
        {
            get
            {
                return VirtualContent.assets.Count;
            }
        }

        static VirtualContent()
        {
            VirtualContent.assets = new List<VirtualAsset>();
        }

        public static void ByName()
        {
            foreach (VirtualAsset asset in VirtualContent.assets)
            {
                Console.WriteLine(string.Concat(new object[] { asset.Name, "[", asset.Width, "x", asset.Height, "]" }));
            }
        }

        public static void BySize()
        {
            Dictionary<int, Dictionary<int, int>> nums = new Dictionary<int, Dictionary<int, int>>();
            foreach (VirtualAsset asset in VirtualContent.assets)
            {
                if (!nums.ContainsKey(asset.Width))
                {
                    nums.Add(asset.Width, new Dictionary<int, int>());
                }
                if (!nums[asset.Width].ContainsKey(asset.Height))
                {
                    nums[asset.Width].Add(asset.Height, 0);
                }
                Dictionary<int, int> item = nums[asset.Width];
                int height = asset.Height;
                item[height] = item[height] + 1;
            }
            foreach (KeyValuePair<int, Dictionary<int, int>> num in nums)
            {
                foreach (KeyValuePair<int, int> value in num.Value)
                {
                    Console.WriteLine(string.Concat(new object[] { num.Key, "x", value.Key, ": ", value.Value }));
                }
            }
        }

        public static VirtualRenderTarget CreateRenderTarget(string name, int width, int height, bool depth = false, bool preserve = true, int multiSampleCount = 0)
        {
            VirtualRenderTarget virtualRenderTarget = new VirtualRenderTarget(name, width, height, multiSampleCount, depth, preserve);
            VirtualContent.assets.Add(virtualRenderTarget);
            return virtualRenderTarget;
        }

        public static VirtualTexture CreateTexture(string path)
        {
            VirtualTexture virtualTexture = new VirtualTexture(path);
            VirtualContent.assets.Add(virtualTexture);
            return virtualTexture;
        }

        public static VirtualTexture CreateTexture(string name, int width, int height, Color color)
        {
            VirtualTexture virtualTexture = new VirtualTexture(name, width, height, color);
            VirtualContent.assets.Add(virtualTexture);
            return virtualTexture;
        }

        internal static void Reload()
        {
            if (VirtualContent.reloading)
            {
                foreach (VirtualAsset asset in VirtualContent.assets)
                {
                    asset.Reload();
                }
            }
            VirtualContent.reloading = false;
        }

        internal static void Remove(VirtualAsset asset)
        {
            VirtualContent.assets.Remove(asset);
        }

        internal static void Unload()
        {
            foreach (VirtualAsset asset in VirtualContent.assets)
            {
                asset.Unload();
            }
            VirtualContent.reloading = true;
        }
    }
}