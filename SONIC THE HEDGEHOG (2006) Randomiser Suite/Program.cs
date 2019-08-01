using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using HedgeLib.Sets;

namespace SONIC_THE_HEDGEHOG__2006__Randomiser_Suite
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainWindow());
        }

        static public void HedgeLibPatch(string filepath, string setName)
        {
            S06SetData sourceSet = new S06SetData();
            S06SetData setTarget = new S06SetData();

            sourceSet.Load(filepath);

            //Dud Object at 0,0,0 to work around HedgeLib often breaking the first object in the set.
            //if (sourceSet.Objects[0].ObjectType != "objectphysics" && sourceSet.Objects[0].Parameters[0].Data.ToString() != "Randomised Indicator")
            //{
                SetObject dubObject = new SetObject();
                dubObject.ObjectID = 0;
                dubObject.Parameters.Add(new SetObjectParam(typeof(string), "Randomised Indicator"));
                dubObject.Parameters.Add(new SetObjectParam(typeof(bool), false));
                List<SetObjectParam> parameters2 = dubObject.Parameters;
                SetObject item2 = new SetObject
                {
                    ObjectType = "objectphysics",
                    ObjectID = dubObject.ObjectID,
                    Parameters = parameters2,
                };
                setTarget.Objects.Add(item2);
            //}

            foreach (SetObject s06Object in sourceSet.Objects)
            {
                SetObject s06ObjectEdited = new SetObject();

                setTarget.Objects.Add(s06Object);
            }


            foreach (SetObject s06Object in setTarget.Objects)
            {
                for (int i = 0; i < s06Object.Parameters.Count; i++)
                {
                    if (s06Object.Parameters[i].DataType.ToString() == "System.UInt32")
                    {
                        if (s06Object.Parameters[i].Data.ToString() != "4294967295")
                        {
                            s06Object.Parameters[i].Data = uint.Parse(s06Object.Parameters[i].Data.ToString()) + 1;
                        }
                    }
                }
            }
            if (!File.Exists(Path.GetDirectoryName(filepath) + "\\" + setName + ".set.s06back"))
            {
                File.Move(filepath, Path.GetDirectoryName(filepath) + "\\" + setName + ".set.s06back");
            }
            setTarget.Save(filepath, true);
        }

        static public void LuaBackup(string filepath, string luaName)
        {
            if (!File.Exists(Path.GetDirectoryName(filepath) + "\\" + luaName + ".lub.s06back"))
            {
                File.Copy(filepath, Path.GetDirectoryName(filepath) + "\\" + luaName + ".lub.s06back");
            }
        }
        static public void CollisionBackup(string filepath, string binName)
        {
            if (!File.Exists(Path.GetDirectoryName(filepath) + "\\" + binName + ".bin.s06back"))
            {
                File.Copy(filepath, Path.GetDirectoryName(filepath) + "\\" + binName + ".bin.s06back");
            }
        }
    }
}
