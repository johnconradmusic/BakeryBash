using Monocle;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BakeryBash
{
    public static class GameplayBuffers
    {
        public static VirtualRenderTarget Gameplay;
        public static VirtualRenderTarget Level;
        public static VirtualRenderTarget Light;
        public static VirtualRenderTarget Lightning;
        public static VirtualRenderTarget TempA;
        public static VirtualRenderTarget TempB;

        private static List<VirtualRenderTarget> all = new List<VirtualRenderTarget>();

        public static void Create()
        {
            GameplayBuffers.Unload();
            GameplayBuffers.Gameplay = GameplayBuffers.Create(320, 180);
            GameplayBuffers.Level = GameplayBuffers.Create(1920, 1080);
            GameplayBuffers.Light = GameplayBuffers.Create(320, 180);
            GameplayBuffers.Lightning = GameplayBuffers.Create(160, 160);
            GameplayBuffers.TempA = GameplayBuffers.Create(320, 180);
            GameplayBuffers.TempB = GameplayBuffers.Create(320, 180);
        }

        private static VirtualRenderTarget Create(int width, int height)
        {
            VirtualRenderTarget renderTarget = VirtualContent.CreateRenderTarget("gameplay-buffer-" + (object)GameplayBuffers.all.Count, width, height);
            GameplayBuffers.all.Add(renderTarget);
            return renderTarget;
        }

        public static void Unload()
        {
            foreach (VirtualAsset virtualAsset in GameplayBuffers.all)
                virtualAsset.Dispose();
            GameplayBuffers.all.Clear();
        }
    }
}