using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Reflection;
using BoneLib;
using BoneLib.BoneMenu;
using BoneLib.BoneMenu.Elements;
using BoneLib.Nullables;
using LabFusion;
using LabFusion.Data;
using LabFusion.Network;
using LabFusion.Representation;
using LabFusion.SDK.Modules;
using MelonLoader;
using SLZ.Interaction;
using SLZ.Marrow.Data;
using SLZ.Marrow.Pool;
using SLZ.Marrow.Warehouse;
using SLZ.Props.Weapons;
using SLZ.Rig;
using UnityEngine;
using Module = LabFusion.SDK.Modules.Module;


namespace PowerToolsFusionModule //currently no need for this but will be useful to have in the future
{

    internal partial class Main : MelonMod
    {
        public override void OnInitializeMelon()
        {
            MelonLogger.Msg("Loading module");
            ModuleHandler.LoadModule(System.Reflection.Assembly.GetExecutingAssembly());
        }
    }
    
        public class FusionModuleSender : Module
        {
        }
}
