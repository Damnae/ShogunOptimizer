ShogunOptimizer is a program for people that don't want to form an intimate relationship with spreadsheets: It tests all possible artifact and weapon combinations from given options and determines which is the best for your character.

# Usage
ShogunOptimizer requires a json file output from [Genshin Optimizer](https://frzyc.github.io/genshin-optimizer/). Once you have finished importing your artifacts as per Genshin Optimizer instructions go to the `Database` tab and click download on the `Database Download` section. The file must be saved as `godata.json` and it must be placed in `../../Debug/net5.0/` from the program's working directory.
Currently, configurations for character builds are done inside the source code, which means that in order to select which weapons to consider for calculation, account for party resonance or other buffs you will have to modify code in BuildTargets.
