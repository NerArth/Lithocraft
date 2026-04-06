# Lithocraft Mod Architecture and Design Guidelines

## Overview

The Lithocraft project is designed as a modular ecosystem for Vintage Story. It consists of a foundational `Lithocraft-Core` module and several optional expansion modules (e.g., `Lithocraft-Chem`, `Lithocraft-Metal`).

This document outlines the design philosophy and technical architecture to ensure these modules interact seamlessly without causing scope creep, hard dependencies between optional modules, or frustrating gameplay loops.

## Design Philosophy: "Bonus, Not Penalty"

A core tenant of the Lithocraft ecosystem is respecting the player's time.

1. **Standalone Viability**: Every module (alongside `Lithocraft-Core`) must offer a complete and satisfying experience on its own.
2. **Side-by-Side Coexistence & Upgrades**: When a player installs an expansion module that expands upon a core process, the complex process should generally replace or significantly upgrade the simpler core process.
3. **Synergy is a Bonus**: If a player installs multiple expansion modules (e.g., both Chem and Metal), interactions between them should provide *bonuses*—such as higher yields, reduced resource waste, or valuable byproducts. Installing a new module should **never** penalize the player by arbitrarily forcing them to perform tedious extra required steps for a process they previously enjoyed.

## Data-Driven Fallbacks (JSON Assets)

For data, such as crafting recipes, item properties, and block behaviors configured via JSON, Lithocraft relies on **Vintage Story's conditional JSON patching system**.

Instead of writing complex C# code to manage recipe fallbacks, we use data patches that only apply when specific conditions are met.

### The `dependsOn` Array
When an expansion module needs to modify or replace a core asset, or when it needs to interact with another expansion module, it should use a patch file equipped with the `dependsOn` property.

**Example scenario:** The Metal module adds a better smelting recipe *if* the Chem module is also installed.
The patch file in `Lithocraft-Metal/assets/lithocraftmetal/patches/` would look like this:

```json
[
  {
    "file": "lithocraftcore:recipes/grid/smelting_basic.json",
    "op": "replace",
    "path": "/ingredients/0",
    "value": { "type": "item", "code": "lithocraftchem:catalyst" },
    "dependsOn": [{ "modid": "lithocraftchem" }]
  }
]
```

*Note: To assist with generating these conditional patches without tedious manual work, use the provided developer helper script `scripts/New-ConditionalPatch.ps1`.*

## C# Code Architecture (Interfaces & API Hub)

For complex block behaviors, entity logic, and C# interactions, cross-module communication is handled via **Interfaces routed through `Lithocraft-Core`**.

Because `Lithocraft-Core` is the required base for all expansion modules, it serves as the central registry.

### 1. Central API Registry
`Lithocraft-Core` defines a central API manager (e.g., `ILithocraftAPI`). This manager holds references to optional module APIs.

### 2. Module Interfaces
`Lithocraft-Core` defines the abstract interfaces for what expansion modules can do, such as `IChemistryAPI` or `IMetalAPI`.
It **does not** implement them.

### 3. Registration
When `Lithocraft-Chem` loads, it implements `IChemistryAPI` and registers its implementation with `Lithocraft-Core`'s central registry.

### 4. Consumption
If `Lithocraft-Metal` needs to perform a chemical calculation, it asks `Lithocraft-Core` for the `IChemistryAPI`.
- If the Chem module is installed, Core returns the registered instance, and Metal uses it.
- If the Chem module is *not* installed, Core returns `null`, and Metal gracefully falls back to its simpler internal logic without crashing or needing a hard dependency on the Chem module assembly.

This structure guarantees that optional modules can talk to each other safely, exclusively via the `Lithocraft-Core` broker, avoiding messy circular dependencies.
