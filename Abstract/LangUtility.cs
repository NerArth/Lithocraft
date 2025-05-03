using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;

namespace Lithocraft.CodeContent
{
    /// <summary>
    /// <para>An internal abstract class for invoking shorthand methods relating to localised strings.</para>
    /// <para>To make use of this class, initiate the class in the local context like this:</para>
    /// <code>LangUtility _langutil = new LangUtility();</code>
    /// <para>Then the class can be used like this:</para>
    /// <code>_langutil.ProvideErrorFeedback("internalerrorcode", "lithocraft:feedback-msg",api);</code>
    /// </summary>
    internal class LangUtility
    {
        /// <summary>
        /// <para>Triggers an in-game error message over the hotbar displaying <paramref name="feedbackmsg"/> with an internal <paramref name="errorcode"/>, requires passing-thru the <paramref name="api"/>.</para>
        /// <para><paramref name="feedbackmsg"/> should be a "lithocraft:feedback-msg" string from the locale file.</para>
        /// </summary>
        /// <param name="errorcode"></param>
        /// <param name="feedbackmsg"></param>
        /// <param name="api"></param>
        internal void ProvideErrorFeedback(string errorcode, string feedbackmsg, ICoreAPI api)
        {
            // since we have to provide an api to the function, we better make sure the api is actually validated first
            if (api is null) return;
            // for providing client-side feedback
            if (api.Side == EnumAppSide.Client)
            {
                (api as ICoreClientAPI).TriggerIngameError(this, errorcode, Lang.Get(feedbackmsg));
            }
        }
    }
}
