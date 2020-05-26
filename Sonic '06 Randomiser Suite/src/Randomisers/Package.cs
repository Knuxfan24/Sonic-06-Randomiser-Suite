using System;
using System.Linq;
using HedgeLib.Misc;
using System.Collections.Generic;

namespace Sonic_06_Randomiser_Suite
{
    class Package
    {
        public static void PackageAnimationRandomiser(string package, bool allowForbidden, Random rng)
        {
            List<string> forbiddenAnimations = new List<string>() {
                "silver_style_Root.xnm", "face"
            };

            List<string> availableReferences = new List<string>();
            List<int> usedNumbers = new List<int>();
            int index;

            S06Package pkg = new S06Package();
            pkg.Load(package);

            if (allowForbidden) forbiddenAnimations.Clear();

            // Load all the motion filepaths into avaliable references
            for (int i = 0; i < pkg.Types.Count; i++)
            {
                if (pkg.Types[i].TypeName == "motion")
                {
                    foreach (var entry in pkg.Types[i].Files)
                    {
                        if (!forbiddenAnimations.Any(entry.FilePath.Contains))
                            availableReferences.Add(entry.FilePath);
                    }
                }
            }

            // Select a random value from avaliable references for each motion entry, if the number is already used, pick a new one
            for (int i = 0; i < pkg.Types.Count; i++)
            {
                if (pkg.Types[i].TypeName == "motion")
                {
                    for (int f = 0; f < pkg.Types[i].Files.Count; f++)
                    {
                        if (!forbiddenAnimations.Any(pkg.Types[i].Files[f].FilePath.Contains))
                        {
                            index = rng.Next(availableReferences.Count);

                            if (usedNumbers.Contains(index))
                            {
                                do { index = rng.Next(availableReferences.Count); }
                                while (usedNumbers.Contains(index));
                            }
                            usedNumbers.Add(index);

                            pkg.Types[i].Files[f].FilePath = availableReferences[index];
                        }
                    }
                }
            }

            pkg.Save(package, true);
        }
    }
}
