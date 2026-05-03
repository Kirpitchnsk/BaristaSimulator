using System.Collections.Generic;
using System;
using UnityEngine;


namespace Arenar.Options
{
    public class OptionsController : IOptionsController
    {
        Dictionary<Type, IOption> OptionsDictionary;


        public OptionsController()
        {
            InitializeOptionsDictionary();
        }
        
        
        public T GetOption<T>() where T : IOption
        {
            if (!OptionsDictionary.TryGetValue(typeof(T), out IOption option))
            {
                Debug.LogError($"Not found options {typeof(T)}");
                return default;
            }

            return (T)option;
        }

        public void SetOption<T>(T settingsOption) where T : IOption
        {
            if (!OptionsDictionary.ContainsKey(typeof(T)))
            {
                Debug.LogError($"Not found options {typeof(T)}");
                return;
            }

            OptionsDictionary[typeof(T)] = settingsOption;
        }

        private void InitializeOptionsDictionary()
        {
            var types = typeof(IOption).GetAssignableTypes();

            OptionsDictionary = new Dictionary<Type, IOption>();

            foreach (var type in types)
            {
                IOption optionInstance = (IOption) Activator.CreateInstance(type);

                OptionsDictionary.Add(type, optionInstance);
            }
        }
    }
}
