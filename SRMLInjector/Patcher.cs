using Mono.Cecil;
using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MethodAttributes = Mono.Cecil.MethodAttributes;

namespace SRMLInjector
{
    public static class Patcher
    {
        static AssemblyDefinition TargetAssembly;
        static string TargetAssemblyPath;
        static MethodDefinition MethodToPatch;
        static MethodDefinition MethodToCall;
        public static void Init(bool uninstall)
        {
            MethodToPatch = Config.PatchMethod.Resolve(Program.MANAGED_PATH,out TargetAssemblyPath);
            MethodToCall = null;
            try
            {
                Config.EntryMethod.Resolve(Program.MANAGED_PATH, out var _);
            }
            catch
            {
                if (!uninstall) throw;
            }
            TargetAssembly = MethodToPatch.Module.Assembly;

        }

        public static void Dispose()
        {
            TargetAssembly.Dispose();
            MethodToCall.Module.Assembly.Dispose();
        }

        public static void Patch()
        {
            GenerateLoadMethod();
            var proc = MethodToPatch.Body.GetILProcessor();
            if (MethodToPatch.Body.Instructions[0].OpCode == OpCodes.Call && MethodToPatch.Body.Instructions[0].Operand is MethodReference refe && refe.Name == "LoadSRModLoader") return;
            var method = GenerateLoadMethod();
            MethodToPatch.DeclaringType.Methods.Add(method);
            proc.InsertBefore(MethodToPatch.Body.Instructions[0], proc.Create(OpCodes.Call, method));
        }

        public static void UnPatch()
        {

        }

       

        public static void Save()
        {
            string intermediate = Path.Combine(Program.MANAGED_PATH, "Intermediate");

            TargetAssembly.Write(intermediate);
            Dispose();
            File.Delete(TargetAssemblyPath);
            File.Move(intermediate,TargetAssemblyPath);
        }

        static MethodDefinition GenerateLoadMethod()
        {
            var method = new MethodDefinition("LoadSRModLoader", MethodAttributes.Public | MethodAttributes.Static, TargetAssembly.MainModule.TypeSystem.Void);
            method.Body.Variables.Add(new VariableDefinition(TargetAssembly.MainModule.ImportReference(typeof(string[]))));
            method.Body.Variables.Add(new VariableDefinition(TargetAssembly.MainModule.TypeSystem.Int32));
            var proc = method.Body.GetILProcessor();
            var maincall = method.Body.GetILProcessor().Create(OpCodes.Call, TargetAssembly.MainModule.ImportReference(MethodToCall));
            if (!TargetAssembly.MainModule.TryGetTypeReference("UnityEngine.Debug", out var reference))
                throw new Exception("Couldn't find UnityEngine.Debug");
            var logref = new MethodReference("Log", TargetAssembly.MainModule.TypeSystem.Void, reference);
            logref.Parameters.Add(new ParameterDefinition(TargetAssembly.MainModule.TypeSystem.Object));
            var onfailwrite = proc.Create(OpCodes.Call, logref);

            if (!TargetAssembly.MainModule.TryGetTypeReference("UnityEngine.Application", out var quitreference))
                throw new Exception("Couldn't find UnityEngine.Application");

            var applicationquit = proc.Create(OpCodes.Call, new MethodReference("Quit", TargetAssembly.MainModule.TypeSystem.Void, quitreference));

            var ret = proc.Create(OpCodes.Ret);
            var leave = proc.Create(OpCodes.Leave, ret);

            var mainret = proc.Create(OpCodes.Ret);

            foreach (var v in GetLoadingInstructions(proc, TargetAssembly.MainModule))
            {
                proc.Append(v);
            }

            proc.Append(maincall);


            proc.InsertAfter(maincall, mainret);
            proc.InsertAfter(mainret, onfailwrite);
            proc.InsertAfter(onfailwrite, applicationquit);
            proc.InsertAfter(applicationquit, leave);
            proc.InsertAfter(leave, ret);

            var handler = new ExceptionHandler(ExceptionHandlerType.Catch)
            {
                TryStart = method.Body.Instructions.First(),
                TryEnd = onfailwrite,
                HandlerStart = onfailwrite,
                HandlerEnd = ret,
                CatchType = TargetAssembly.MainModule.ImportReference(typeof(Exception)),
            };



            method.Body.ExceptionHandlers.Add(handler);

            return method;
        }

        public static IEnumerable<Instruction> GetLoadingInstructions(ILProcessor proc, ModuleDefinition def)
        {
            yield return proc.Create(OpCodes.Ldstr, Program.LIB_PATH);
            yield return proc.Create(OpCodes.Ldstr, "*.dll");
            yield return proc.Create(OpCodes.Ldc_I4_1);

            var method = typeof(Directory).GetMethods()
                .First((x) => x.Name == "GetFiles" && x.GetParameters().Length == 3);
            var imported = def.ImportReference(method);
            yield return proc.Create(OpCodes.Call, imported);


            yield return proc.Create(OpCodes.Stloc_0);
            yield return proc.Create(OpCodes.Ldc_I4_0);
            yield return proc.Create(OpCodes.Stloc_1);
            var seventeen = proc.Create(OpCodes.Ldloc_1);
            yield return proc.Create(OpCodes.Br_S, seventeen);
            var eight = proc.Create(OpCodes.Ldloc_0);
            yield return eight;
            yield return proc.Create(OpCodes.Ldloc_1);
            yield return proc.Create(OpCodes.Ldelem_Ref);
            yield return proc.Create(OpCodes.Call, def.ImportReference(typeof(Assembly).GetMethod("LoadFrom", BindingFlags.Public | BindingFlags.Static, Type.DefaultBinder, new Type[] { typeof(String) }, null)));
            yield return proc.Create(OpCodes.Pop);
            yield return proc.Create(OpCodes.Ldloc_1);
            yield return proc.Create(OpCodes.Ldc_I4_1);
            yield return proc.Create(OpCodes.Add);
            yield return proc.Create(OpCodes.Stloc_1);
            yield return seventeen;
            yield return proc.Create(OpCodes.Ldloc_0);
            yield return proc.Create(OpCodes.Ldlen);
            yield return proc.Create(OpCodes.Conv_I4);
            yield return proc.Create(OpCodes.Blt_S, eight);

        }
    }
}
