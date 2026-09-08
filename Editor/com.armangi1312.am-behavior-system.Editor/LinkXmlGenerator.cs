using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

namespace AMBehaviorSystem.Editor
{
    public class LinkXmlGenerator : IPreprocessBuildWithReport
    {
        private const string OutputPath = "Assets/link.xml";
        public int callbackOrder => 0;

        public void OnPreprocessBuild(BuildReport report)
        {
            Type[] processorTypes = TypeCache.GetTypesDerivedFrom<Processor>()
                                             .Where(type => !type.IsAbstract)
                                             .ToArray();

            Type[] settingTypes = TypeCache.GetTypesDerivedFrom<ISetting>()
                                           .Where(type => !type.IsAbstract && !type.IsInterface)
                                           .ToArray();

            Type[] contextTypes = TypeCache.GetTypesDerivedFrom<IContext>()
                                           .Where(type => !type.IsAbstract && !type.IsInterface)
                                           .ToArray();

            Type[] allTypes = processorTypes.Concat(settingTypes).Concat(contextTypes).ToArray();

            WriteLinkXml(allTypes);
        }

        private static void WriteLinkXml(Type[] types)
        {
            Dictionary<string, List<Type>> groupedByAssembly = types
                .GroupBy(type => type.Assembly.GetName().Name)
                .ToDictionary(group => group.Key, group => group.ToList());

            StringBuilder builder = new();
            builder.AppendLine("<linker>");

            foreach(KeyValuePair<string, List<Type>> pair in groupedByAssembly)
            {
                builder.AppendLine($"    <assembly fullname=\"{pair.Key}\">");

                foreach(Type type in pair.Value)
                    builder.AppendLine($"        <type fullname=\"{type.FullName}\" preserve=\"all\"/>");

                builder.AppendLine("    </assembly>");
            }

            builder.AppendLine("</linker>");

            File.WriteAllText(OutputPath, builder.ToString());
            AssetDatabase.Refresh();
        }
    }
}
