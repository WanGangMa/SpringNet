using Mono.Cecil;
using System.Linq;

namespace PublicAnonymous
{
    /// <summary>
    /// 將專案下internal成員改為public，主要用於dynamic視圖
    /// 建制事件："$(SolutionDir)PublicAnonymous\bin\Debug\PublicAnonymous.exe" "$(TargetPath)"
    /// </summary>
    class PublicAnonymous
    {
        static void Main(string[] args)
        {
            var asmFile = args[0];
            //Console.WriteLine("Making anonymous types public for '{0}'.", asmFile);

            var asmDef = AssemblyDefinition.ReadAssembly(asmFile, new ReaderParameters
            {
                ReadSymbols = true
            });

            var anonymousTypes = asmDef.Modules
                .SelectMany(m => m.Types)
                .Where(t => t.Name.Contains("<>f__AnonymousType"));

            foreach (var type in anonymousTypes)
            {
                type.IsPublic = true;
            }

            asmDef.Write(asmFile, new WriterParameters
            {
                WriteSymbols = true
            });
        }
    }
}
