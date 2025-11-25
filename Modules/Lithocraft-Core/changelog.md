# Medieval Industry Changelog
(aka Lithocraft)

## v0.7.1
### Overview:
Additions to repair mechanics and agriculture.

### Items:
**Root of scarcity**:
- New crop which fills the niche of a Winter-hardy crop.
- Can be planted all-year round, depending on climate.
- Very high satiation, but low yield.
- Takes an average of 4 months to grow.

**Whetstone (new)**:
- Made with a variety of materials. Crafting requires appropriate ground materials, a binder, wood, and an oil or fat.
- Can be placed in crafting grid with a tool to repair it.
- Has limited uses based on material it's made of, but also stacks up to 16.

**Silage (all types)**:
- Improved recipe clarity, as all recipes have always been shapeless. Balance remains unchanged.

### Tweaks:
- Container textures for gravel blocks.
- Container textures for cobblestone, drystone and cobbleskull  blocks.

### Compatibility:
- Wildcraft fruits and vegetables can now be used to make silage (plant).

## v0.7.0
### Overview:
Reintroducing the grindstone block. Other repair changes coming in another 0.7.x patch.

### Items:
- Grindstone:
    - Reintroduced, mostly as it functioned in v0.3.4.
    - Used to repair tools.
    - Repair amount is based on the grindstone tier.
    - Recipes not fully implemented yet.

## v0.6.2
### Overview:
Very minor update to add some more compatibilities, QOL, and convenience/recycling features.

### Tweaks:
**Recycling**:
- Can now recycle cobblestone, dry stone and ashlar brick blocks into appropriate stone items.
    - **Stone block + pickaxe** in crafting grid.
    - Binding agents like mortar and clay are not returned.
- Can now recycle wooden paths into planks.
    - **Wooden path + saw** in crafting grid.
    - For vanilla paths, will return pine planks.
    - For MR paths, will return appropriate planks.

**Conversion**:
- Aged stone blocks of several types can now be converted into unaged versions.
    - **Aged stone block + hammer + chisel** in crafting grid.

**QOL**:
- Gave vanilla rammed earth blocks a description. Textures are hard to tell apart in the inventory and should help with identifying.

### Fixes:
- Claycutters are now grouped per cutting form in the handbook.

### Compatibility:
- MoreRoads wooden paths can be deconstructed too.

Note: The in-game mod name has been changed simply to "Medieval Industry" due to the internal workings of the different modules, to avoid a tooltip ambiguity issue.

## v0.6.1
### Overview:
Minor update to address some smaller issues.

### Items:
#### Claycutters:
Durability now depends on the shape as well as the material the claycutter is made from.

For each material:
- Shingle cutter durability is unchanged.
- Bowl cutters have about 75% of the original durability.
- Strips cutters have about 200% of the original durability.
- Flat cutters have about 50% of the original durability.

Claycutter ready state is now also indicated more clearly in the item tooltip.

### Handbook:
- Added a missing link in the wiki/secondary features page.

### Fixes:
- Fixed a potential issue with claycutter itemtype file validation.

## v0.6.0
### Overview:
Refactoring of locale and handbook; however, only some additions are listed below. Additional gameplay tweaks.

New claycutter variants, recipes and related items, to allow for claycutting larger/more complex shapes.

### Items:
- New item: Trimmed clay shapes
    - Made by cutting clay with new claycutter variants.
    - Used to craft more complex clay shapes, such as storage vessels.
    - Can also press tool heads and other items into flat clay shapes to make casting shapes.
    - Can be recycled into blue clay. 
      - Only blue clay for now due to some constraints.
#### Claycutters:
- New variants:
    - Flat cutter.
    - Strips cutter.
- Smithing patterns revised, added smithing patterns for new cutter variants.
- Shingles cutters no longer require soldering.

### Handbook:
- General refactoring; reorganised chapters and sections.
    - Individual chapters for each module.
- Claycutters now mention the soldering process in more detail, just as a redundancy.

### Tweaks:
- Crushed alum can now be ground into powder.
    - If Expanded Matter is also installed, its recipe will take precedence instead.

### Fixes:
- Fixed visuals for dropped claycutters.


## v0.5.1
### Overview:
First-release of new iteration of modules. This first version does not include any new content, but it does include a lot of back-end changes to the mod's code and structure, which will allow for easier future updates and content additions.

Accordingly, the changelog is very short, as it only includes the changes made to the structure of the mod.

### Core:
- Removed many incomplete or unused items, blocks and recipes
    - This was done to make the mod easier to maintain
    - Removed items will return in future updates, but for now they are gone to avoid game-breaking and confusion


## v0.3.4

### Core:
- New metals and their relevant items:
    - Alum (elemental aluminium/aluminum)
        - Tier 2 (copper)
    - Alum bronze (copper-alum-nickel alloy)
        - Tier 4 (iron)
    - Silumin (alum-silicon-copper|iron|magnesium alloy)
        - Tier 5 (steel)
        - Note: not yet craftable
    - Orokhalkos (copper-gold-mercury-titanium amalgam steel alloy)
        - Tier 8 (post-titanium)
    - Titan bronze (titanium-alum-copper-beryllium alloy)
        - Tier 6 (post-steel)
    - Mercury amalgam (mercury-copper-gold|silver amalgam)
        - Not suitable as tool metal

- Gem blocks, monoliths and metal grates can now be placed inside mining bags
    
### Crafting:
- New crushed materials:
    - Crushed emerald/beryl

- New chemical residues:
    - Beryllium, used to make titan bronze
    - Mercury paste, used to make mercury amalgam

- Bauxite processing:
    - Added alternative recipes for prepared bauxite grit

- Ammonia:
    - Anywhere that ammonia was used has been replaced with a ChemLib analogue, which is made using a chemistry setup

- Sulfuric compounds:
    - Some instances where sulfur was used in a recipe, have been replaced with ChemLib sulfuric acid, known as oil of vitriol

- Nitric acid:
    - Added new recipe

### Balance:
- Plant-based silage ratios changed; now 5:4, grass:plantmatter, previously 4g:5p

- Biomass soil nutrients slightly nerfed; now 8N 3P 6K, previously 12N 6P 12K

- Adjusted biomass crafting for the several recipes to try and level the field a bit
    - Compost + saltpetre now gives 24 biomass, up from 8
    - Peat + sulfur now gives 12 biomass, up from 4
        - Now requires 2 peat bricks, down from 4
    - Plant silage + soil now gives 12 biomass, down from 16 

### Compat:
- Lithocraft:
    - Made grindstone generally compatible out-of-the-box with some tools from some other mods, like QP's Chisel Tools
        - (other mods can still add their own compatibility with the grindstone by patching the attributes)

- Lavoisier/ChemistryLib:
    - Added as hard dependencies; this means that from now on, Lithocraft *requires* these mods
    - Override some of ChemistryLib textures with Lithocraft's own textures
    - Note: where applicable, existing Lithocraft items will no longer be craftable and will be phased out

### Fixes:
- Made some adjustments to chemical residue models

- Sodash recipes now display properly in the handbook

- Metal grates now always drop the correct default rotation variant

- Metal grate style variants now must be swapped-in-world with a wrench
    - Note: due to an incompatible behaviour with forced orientations, styling variants for metal grates can no longer be cycled with the crafting grid. Hopefully a better solution can be implemented in future

- Fixed some issues with organic matter silage crafting

## v0.3.3

### Core:
- Allow for silage (grass/plant) to be used as animal feed in large troughs, as if they were hay bales
    - For silage (grass), this just preserves the hay feeding functionality
    - For silage (plant), this enables a variety of crops to be used as if they were hay or fruit mash; it is no more efficient but in some cases provides versatility of choice

### Fixes:
- Fixed the grindstone interaction bug that happened when changing selected item while in the middle of an interaction; this also fixes an exploit that allowed repairing of otherwise unrepairable tools
    
- Grindstone: the list of valid items/tools are now set in the grindstone.json; in principle any item with durability can be added to the list; look at the comment inside the json for more information
    - Note: this allows other mods to patch grindstone.json to allow their tools to be repairable

- Adjusted description of silage (grass/plant) blocks

- Added missing gravel->sand recipes for andesite and chalk

### Compat:
- CAN Jewelry:
    - Initial compatibility implementation
        - Allow crafting of some gem blocks and some monoliths using appropriate CAN Jewelry gems
        - Not all gems supported and some gems are only supported when GA is also active

## v0.3.2

### Fixes:
- Stop grindstones from repairing hammers and wrenches (was allowed by accident)

- Correction made to stone-framed painting trivia

- Other handbook corrections and tweaks

## v0.3.1

### Core:
- Handbook:
    - Added a mostly-complete chapter on bauxite processing; missing links to items

- The core of the bauxite process is now available
    - Items made from metallic aluminium will be added in a later patch and alloyed aluminium metals will be added when the sintering processes are coded and finalised

- Tweaks: patched in-container textures for:
    - Sand
    - Gravel
    - Loose shingles

- Grindstone is now craftable and usable; refer to 0.3.0 patch notes for repair mechanics details
    - Durability repaired is partly dependent on the tool itself, so that people playing with high durability modifiers are not penalised in terms of time
        - The base repair amount is 0.9% of a tool's maximum durability
    - Made changes to repair amount compared to the original plan in 0.3.0, due to the play-tested repair rates
        - Now repair rates by tier are stone (t1) 1x, ceramic (t2) 2x, gem (t3) 3x

- Claycutters now have a custom code class; this enables the mod to put text on the item letting a player know if it's ready to do claycutting or not

- Sodash can now be used as a weak general-purpose fertiliser

### Crafting:
- The following blocks/items already existed but are now craftable:
    - Grindstone (all tiers); bear in mind, the higher tiers have unfinished placeholder recipes
    - Sodash
    - Saluminate
    - Alum slurry
    - Alum solution
    - Aluox
    - Alumina

- New recipe(s) for making vanilla compost
    - This was added because when sulfur is difficult to find or compost is being difficult to produce, making biomass felt too difficult
    - When applicable and made using copper sulfate, the process takes less time
    - Some of these recipes use copper sulfate in small amounts, giving copper/copper sulfate some extra niche use in the game's farming
        - Note: how authentic this actually is would depend on a lot of things, but generally speaking, it's assumed for the purpose of the mod that we're trying to add acidity and generally promote breakdown of organic matter
    - Some of the alternative recipes are balanced to be MOSTLY in-line with vanilla rot composting but the yield is slightly worse in most cases since the incentive of doing it this way is to prioritise speed over quantity

- New block: Silage
    - Made by sealing hay blocks in a barrel
    - Alternatively made by mixing some raw food ingredients, like vegetables, with dry grass
    - Can be used to make compost, biomass and organic slurry
    - Will rot in two stages
    - In a later version, these will be usable as feed in troughs

- New item: Stone-framed painting
    - 14 initial variants of different sizes: 1x1, 1x2, 2x1, 2x3
    - The paintings can be sold by the furniture trader
    - The paintings may also be crafted

- New chemical residue:
    - Prepared bauxite grit, primarily made by mixing sodash and bauxite sand
    - Used to produce saluminate

- (NYI) New chemical compound:
    - Thermite, made by mixing aluminium dust/bits or other appropriate reactive metals with appropriate metal shavings/powder etc. and with a salt of some kind; it can then be compressed with the powder press

- New recipe for deconstructing stone walls (dry and ashlar) and some paths
    - Drystone walls give back loose stone at a 100% ratio
    - Ashlar walls give back ashlar blocks at a 75% ratio
    - Vanilla paths give back loose stone at a 100% ratio, as granite; this unintentionally allows for any rock to be converted to granite at the cost of dirt
        - Any other paths from More Roads give at least one appropriate material
        - All this is applicable to slabs and stairs too

### Balance:
Rationale: From playtesting, it became obvious that a lot of the crafting processes added by Lithocraft are complex enough that they require some extra amount of time and mental investment, so they ended up feeling like they weren't worthwhile, especially where the process competed or added on to a vanilla process.

- Adjustments:
    - Copper-lime slurry is now craftable in smaller amounts and with very slightly improved input:yield ratio
    - Copper sulfate is now craftable in smaller amounts and with better input:yield ratio
    - Organic slurry is now craftable in smaller amounts and with better input:yield ratio
    - Gem monoliths now have a better input->output ratio; from 3 gems -> 2 monoliths to 3 gems -> 6 monoliths
        - This brings the crafting cost more in-line with gem blocks
    - Rock monoliths now have a better input->output ratio; from 3 rocks -> 4 monoliths to 3 rocks -> 8 monoliths
    - Biomass from peat and sulfur is now craftable in smaller amounts and with better input:yield ratio
    - Biomass from compost and nitre (saltpetre) now has a slightly better output, up from 6 to 8
    - Biomass burn temperature reduced from 700C to 650C; burn time increased from 24s to 31s.

### Fixes:
- Added missing wrench tip in handbook for metal grates

- Added new models/textures for several residual compounds and greek fire

- Allow for different liquid-containing items like bowls and jugs to be used with the mod liquids, where appropriate

- More back-end changes to patching

- Correction made to creative tabs for stylus/pencil

- Updated flavour text on stylus/pencil

- Made a lot of recipes involving liquids less picky about how much liquid is in the barrel (if the result is an item)

### Compat:
- Expanded Matter:
    - Compatibility recipes for creating thermite
- More Roads:
    - Compatibility for deconstructing paths

### Known issues:
- Grindstone has no animations

- Residual compounds are missing variant-specific container-related textures


## v0.3.0

### Note:
- Items marked as (NYI) are present in the mod but are not usable or relevant, yet! 

### Core:
- Handbook: updated the Gameplay Tweaks chapter

- Handbook: small update to the Liquids chapter

- Renamed: Cupric solution -> Copper sulfate solution

- Tweaks: patched in-container textures for:
    - Clay(s)

### Crafting:
- New item: Stone tablet, made with hammer & chisel and stones in crafting grid
    - A stoney alternative to books!
    - Can be written on with any writing implement, i.e. ink, pencil, stylus
    - Can be put onto shelves and display cases like books
    - Two visual varieties based on the type of stone

- New item: Claycutter, made by shaping through smithing and then must be soldered before they're ready to use
    - Clay cutters can be used to *quickly* craft certain raw clay items
        - Note: Initially I wanted a more involved crafting process but the extra complexity would not be worth the development time
    - Clay cutters can be made of different materials with improving number of uses for higher quality materials
        - (NYI) Corundum and diamond cutters can only be made with sintering
    - Following clay "shape" cutters available to begin with:
        - Shingles, bowls

- (NYI) New chemical residues:
    - Sodash
    - Aluox
    - Saluminate
    - Alumina
    - Notes:
        - Some of these names are not real substance names (sodash, aluox, saluminate)
        - Some compromises are made in terms of real-world accuracy of representation for these items

- (NYI) New liquid: Mineral oil, made by distillation from mineral wax
    - Only serves two uses; crafting of grindstone and crafting of powder press
    - Note: The crafting process may be changed in the future

- (NYI) New block: Grindstone, can be used to repair certain tools a limited number of times; the process takes time and does not instantly repair an item
    - Has 3 tiers: stone, ceramic, gem
    - Grindstone itself has no durability and has unlimited uses, but this may change in future!
    - Amount of times a tool can be fully repaired depends on tier
        - Stone (t1) can fully repair durability 2x
        - Ceramic (t2) can fully repair durability 4x and repairs 3 times as fast
        - Gem (t3) can fully repair durability 5x and repairs 10 times as fast

- (NYI) New block: Powder press, used for one of several steps in advanced metal-making processes

- (NYI) New block: Sintering oven, used for one of several steps in advanced metal-making processes

### Balance:
- Buffed kerogen/mineral wax processing:
    - Processing requires significantly less time (and thus less fuel) to process
    - Now only require 1175C to process, down from 1340C, making it possible to process without coke

### Fixes:
- GA rubellite tourmaline-related LC stuff is now actually craftable

- GA rubellite tourmaline-related LC stuff now has correct text

- Some more back-end changes made to patching

- Pencil/stylus was missing from creative menu

- Adjusted spacings in changelog to make it more readable on dense patches

### Compat:
- Geology Additions:
    - Stone tablets can be crafted with several GA rock types
    - Claycutter corundum variants are crafted using GA ruby/sapphire (recipe NYI)

- General:
    - Support for general claycutting, meaning that if a mod enables red/yellow clay, they should be usable

## v0.2.3
Fixes:
- Fixed compatibility issue that made Expanded Matter ores impossible to smelt if they were in crushed form

- Some back-end changes made to several patching behaviours


## v0.2.2
Fixes:
- Fixed: mod not loading at all

## v0.2.1
Fixes:
- Fixed: ashlar stone fence snow texture

## v0.2.0
Core:

- Handbook: new chapter for suggesting room functions/layouts, this will likely be an on-going WIP and may also cover some suggested vanilla rooms since there is no specific vanilla guidance for this in-game yet, may be helpful if you're an objective-driven person

- Handbook: new category for the mod, for now includes all the chapters but eventually will also include items if possible

- Handbook: added triva for synthetic fibre pulp

- Tweaks: patched in-container textures for:
    - Peat bricks
    - Charcoal (I think)
    - Lignite
    - Black coal
    - Anthracite
    - Cinnabar
    - Lapis lazuli
    - Alum
    - Sulfur
    - Sylvite
    - Borax
    - All loose stones

- Tweaks: changed peat brick burning time up from 25s to 122s;
    - Note: one person alone digging up an entire peatland simply wouldn't be able to go through all that fuel in a short amount of time and peat typically should burn considerably longer than wood

- Mod icon updated

- Added a specialthanks.md file

Crafting:

- New item: Pencils/Styli, made on the crafting grid using a saw and a chisel together with two planks of any timber and an appropriate writing medium like a chunk or soft metal bit
    - Pencils/Styli can be used to write in books or on signs

- New block: Metal grates, made on the crafting grid using metal plates; 2 metal plates yield 6 grates (so 400 metal units to 6 grates)
    - Metal grates have different variants that can be cycled through with the crafting grid
    - Metal grates can be recycled for about 97.5% of their metal value; they cannot be smelted directly and must first be recovered as metal bits
    - Note: may add casting moulds in a later version
    - Can be rotated with a wrench

- New block: full stone roofing block like that of clay shingles; this is primarily meant to fill in a niche for slate rooves; unlikely to add slabs or stair variants since these shapes can technically just be chiseled

- New block: decorative monolith pillars, made on the crafting grid using 3 of a raw material in a vertical pattern with a hammer and chisel
    - Has three shape variations, these can be cycled with crafting OR with a wrench

- New block: decorative gem blocks, made on the crafting grid using a hammer and chisel with resin, stone ashlar bricks and appropriate gems/chunks, yielding 4 gem blocks

Fixes:

- Fixed: flintstone "ore" blocks can now be processed with a pickaxe in crafting as other ungraded ores can with Lithocraft

- Fixed: made available a previously-bugged recipe for strong salt water, using sea water and halite

- Fixed: Greek fire now stacks properly, sorry if you got all the way into crafting it and had an issue with this!
    - Also improved wording in the trivia

- Fixed: Greek fire now has better GUI/icon but still needs improvements overall and is still a placeholder shape

- Fixed: some minor things caused pointless debug logging (like recipes that would never be valid)

- Fixed: sounds were missing for ultra polished rocks
    
Compat:

- Geology Additions:
    - Monolith compat
    - Decorative gem block compat
    - Loose stone in-container textures

KNOWN ISSUES:

- Metal grates block fluids in certain situations

- Some monolith texture UVs line up a bit funny

- Still no high polish textures for GA but all high polish textures are due to be refactored at some point
    
## v0.1.2
Crafting:

- Kerogen -> Mineral wax no longer requires a container to "smelt", fixing an issue that meant that mineral wax could not be retrieved at all

## v0.1.1

Core:

- Ashlar stone fence issue with missing sounds fixed

Balance:

- Buffed the ratios/output of mineral wax processes that aren't based on shale, as they felt too harsh. Shale remains the best option but the other options should feel more welcoming in case shale is difficult to find
        - Heating time required reduced across the board, making the process less fuel-intensive
        - Kerogen now converts into mineral wax at a 1 to 1 ratio
        - Graphite now converts into kerogen at a 2 to 1 ratio instead of 4 to 1
        - Graphite powder (EM) and coal powder (EM) now convert into kerogen at a 2 to 1 ratio

- Candles made with mineral wax now require 4 min. wax for 2 candles, down from requiring 6 min. wax for 2 candles; with the kerogen changes, this should be a decent buff to candle production from mineral wax

Crafting:

- Cobbleskulls now have a recipe, requires 6 bones and 1 cobblestone; hopefully this should also be compatible with any mod that adds its own cobblestone/cobbleskull variants
    - In addition, any existing cobbleskull block can be placed with a pickaxe in the crafting grid to yield the original cobblestone

## v0.1.0

Core:
- A big background change to some elements; previously chemical solutions were separate item codes and they have now been unified as a unique itemcode: chemicalportion
    - Note: This change was made to make sure that adding Lithocraft-specific liquids in the future is easier to handle and because it has long-term implications, so is best done sooner rather than later. Old stuff will have remaps available but unfortunately these will likely serve no use

- Chemical residue (ammonia): new name/now a liquid

- Chemical residue (organic slurry): new name/now a liquid

- Synpulp no longer has erroneous "fruit" nutrition

- A link to mod flowchart added to the Lithocraft 02 in-game handbook section; the link may rarely be updated, only if/when required. The current online hosting solution isn't the best for the purpose and this will likely change when something better can be used

- Mineral wax now has a new shape/texture

- Several items, like kerogen, now list crafting processes in the handbook or have extra trivia
    
Balance:
- Final results of some recipes/processes and their yields made to be a bit more generous
    - Making flax fibres artificially through the synpulp process now has a much better total cost of 1 cellulose, 0.2 litres of ammonia, 0.2 litres of cupric solution, for every 6 fibres produced;
    
Crafting:
- Pulverized Alum can now be sieved together with Sulfur to produce Potash at a rate of 1 to 1, producing 3 Potash

- New recipe: full blocks of most ungraded ore (like coal, sulfur, sylvite, etc., not metals) can be placed with a pickaxe in crafting grid for a slightly better yield

- New block: Ashlar stone fence: these are made using ashlar blocks (bricks) in the same way you would make a dry-stone wall (fence) and have a generous rate of production

- New item: Chemical residue (biomass): uses a similar recipe to what organic slurry used to have in v0.0.1; this is an intermediate residue meant to make the new organic slurry liquid but also serves as a low-quality all-round fertiliser
    - Biomass also has an alternate recipe making use of peat and sulfur; in a future version, there will hopefully be a different farming use for peat

- New item: Greek fire, no special purpose for now, but serves a complex crafting alternative for a uniquely high temperature long-burn fuel

- New liquid: Strong salt water: made in several different ways but the most efficient way is by mining full blocks of halite and dissolving them with water in a barrel

- Cupric solution is now usable as a mordant as intended

- Organic slurry now made by mixing biomass with water in a barrel

- Changed ammonia production process to make use of the distiller at a ratio of 50% yield; in a future version, an alternate process with better yield might be added
    
Remapping:
- Old ammonia and organic slurry should remap to new liquid forms on existing saves, but converted amounts and forms will likely be useless, so it may be better to discard any of these leftovers
    
Compat:
- Geology Additions: Recipes for turning rocks into loose stones and for making gravel/sand from other full blocks
    
KNOWN ISSUES:
- Greek fire icon/hand is too small/difficult to see

- Can't access Lithocraft liquid entries information from crafting processes that use them

- Can't use anything other than buckets with Lithocraft liquids

- Geology Additions-based high polish rock blocks are missing textures

- At the time of this release, flowchart is accessible, but incomplete

## v0.0.1
- Corrected order of chapters

## First release. v0.0.0 aka alpha

Core:
- Residual compounds

- Synthetic fibre pulp

- New ways of obtaining vanilla items like candles and saltpeter

- New "shiny" rock blocks