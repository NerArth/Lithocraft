using System;

namespace Lithocraft.API
{
    /// <summary>
    /// Interface defining the interactions provided by the Lithocraft-Metal module.
    /// Implemented by Lithocraft-Metal and consumed by other modules via ILithocraftAPI.
    /// </summary>
    public interface IMetalAPI
    {
        /// <summary>
        /// Retrieves the purity or grade of a processed metal.
        /// This is an example method to demonstrate cross-module logic.
        /// </summary>
        /// <param name="metalCode">The code of the metal</param>
        /// <returns>A float representing purity.</returns>
        float GetMetalPurity(string metalCode);

        // Add more metal-specific API methods here as needed.
    }
}
