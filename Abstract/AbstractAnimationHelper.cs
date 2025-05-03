using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintagestory.API.Common;
using Vintagestory.GameContent;

namespace Lithocraft.CodeContent
{
    // not in use
    public class AnimationHelper
    {
        private BlockEntityAnimationUtil animUtil;

        public AnimationHelper(BlockEntityAnimationUtil animUtil)
        {
            this.animUtil = animUtil;
        }

        public void StartAnimation (string animationName, float speed, Vintagestory.API.Common.Func<float,float> easingFunction)
        {
            var metaData = new AnimationMetaData
            {
                Animation = animationName,
                AnimationSpeed = speed,
                EaseInSpeed = easingFunction(0),
                EaseOutSpeed = easingFunction(1),
            };
            animUtil.StartAnimation(metaData);
        }
        public void StopAnimation(string animationName)
        {
            animUtil.StopAnimation(animationName);
        }

        private static Vintagestory.API.Common.Func<float,float> EaseOutQuad = t => t * (2 -t);
        private static Vintagestory.API.Common.Func<float,float> EaseInQuad = t => t < 0.5f ? 2 * t * t : -1 + (4-2 * t) * t;
    }
}
