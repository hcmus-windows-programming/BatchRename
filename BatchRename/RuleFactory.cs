﻿using Contract;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchRename
{
    public class RuleFactory:INotifyPropertyChanged
    {
        static Dictionary<string, IRule> _prototypes = new Dictionary<string, IRule>();

        public static void Register(IRule prototype)
        {
            _prototypes.Add(prototype.Name, prototype);
        }

        private static RuleFactory? _instance = null;

        public event PropertyChangedEventHandler? PropertyChanged;

        public static RuleFactory Instance()
        {
            if (_instance == null)
            {
                _instance = new RuleFactory();
            }

            return _instance;
        }

        private RuleFactory()
        {
        }
        public IRule? Parse(string data)
        {
            const string Space = " ";

            var tokens = data.Split(
                new string[] { Space }, StringSplitOptions.None
            );
            var keyword = tokens[0];
            IRule? result = null;

            if (_prototypes.ContainsKey(keyword))
            {
                IRule prototype = _prototypes[keyword];
                result = prototype.Parse(data);
            }

            return result;
        }
    }
}
