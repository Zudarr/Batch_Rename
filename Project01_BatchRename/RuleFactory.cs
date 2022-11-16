using Contract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Project01_BatchRename
{
    public class RuleFactory
    {
        private static RuleFactory? _instance = null;
        public static RuleFactory Instance()
        {
            if (_instance == null)
            {
                _instance = new RuleFactory();
            }
            return _instance;
        }

        public Dictionary<string, IRule> _prototypes = new Dictionary<string, IRule>();
        private RuleFactory()
        {
            string exeFilder = AppDomain.CurrentDomain.BaseDirectory;
            var dllFiles = new DirectoryInfo(exeFilder).GetFiles("*.dll");

            foreach (var dllFile in dllFiles)
            {
                var assembly = Assembly.LoadFrom(dllFile.FullName);
                var types = assembly.GetTypes(); //Lấy tất cả các class trong file dll

                foreach (var type in types)
                {
                    if (type.IsClass && typeof(IRule).IsAssignableFrom(type))
                    {
                        IRule rule = (IRule)Activator.CreateInstance(type)!;
                        _prototypes.Add(rule.Name, rule);
                    }
                }
            }
        }

        public IRule? Parse(Dictionary<string, string> input)
        {
            var ruleName = input["Name"];
            IRule? result = null;

            if (_prototypes.ContainsKey(ruleName))
            {
                IRule prototype = _prototypes[ruleName];
                result = prototype.Parse(input);
            }

            return result;
        }
    }
}
