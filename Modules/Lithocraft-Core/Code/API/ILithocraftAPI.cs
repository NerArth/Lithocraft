using System;

namespace Lithocraft.API
{
    /// <summary>
    /// Central registry for Lithocraft module APIs.
    /// Handled by Lithocraft-Core to allow modules to interact safely.
    /// </summary>
    public interface ILithocraftAPI
    {
        /// <summary>
        /// Gets the Chemistry API if the Lithocraft-Chem module is installed and registered.
        /// Returns null otherwise.
        /// </summary>
        IChemistryAPI? ChemAPI { get; }

        /// <summary>
        /// Gets the Metal API if the Lithocraft-Metal module is installed and registered.
        /// Returns null otherwise.
        /// </summary>
        IMetalAPI? MetalAPI { get; }

        /// <summary>
        /// Registers the Chemistry API implementation. Called by Lithocraft-Chem.
        /// </summary>
        void RegisterChemAPI(IChemistryAPI api);

        /// <summary>
        /// Registers the Metal API implementation. Called by Lithocraft-Metal.
        /// </summary>
        void RegisterMetalAPI(IMetalAPI api);
    }
}
