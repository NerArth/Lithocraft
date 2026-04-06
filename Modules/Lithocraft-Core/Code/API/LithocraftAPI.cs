using System;

namespace Lithocraft.API
{
    /// <summary>
    /// The concrete implementation of the central Lithocraft API registry.
    /// </summary>
    public class LithocraftAPI : ILithocraftAPI
    {
        public IChemistryAPI? ChemAPI { get; private set; }
        public IMetalAPI? MetalAPI { get; private set; }

        public void RegisterChemAPI(IChemistryAPI api)
        {
            if (ChemAPI != null)
            {
                throw new InvalidOperationException("Lithocraft Chemistry API is already registered.");
            }
            ChemAPI = api;
        }

        public void RegisterMetalAPI(IMetalAPI api)
        {
            if (MetalAPI != null)
            {
                throw new InvalidOperationException("Lithocraft Metal API is already registered.");
            }
            MetalAPI = api;
        }
    }
}
