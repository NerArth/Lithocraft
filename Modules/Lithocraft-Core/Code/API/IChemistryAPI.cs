using System;

namespace Lithocraft.API
{
    /// <summary>
    /// Interface defining the interactions provided by the Lithocraft-Chem module.
    /// Implemented by Lithocraft-Chem and consumed by other modules via ILithocraftAPI.
    /// </summary>
    public interface IChemistryAPI
    {
        /// <summary>
        /// Checks if a specific chemical process is valid.
        /// This is an example method to demonstrate cross-module logic.
        /// </summary>
        /// <param name="processName">Name of the process</param>
        /// <returns>True if valid, false otherwise.</returns>
        bool IsValidChemicalProcess(string processName);

        // Add more chemistry-specific API methods here as needed.
    }
}
