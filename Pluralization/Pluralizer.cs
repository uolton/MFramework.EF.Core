using System;
using System.Data.Entity;
using System.Reflection;

namespace MFramework.EF.Core.Pluralization
{
	/// <summary>
	/// Used to pluralize table names
	/// </summary>
    public static class Pluralizer
    {
        private static object _pluralizer;
        private static MethodInfo _pluralizationMethod;

		/// <summary>
		/// Pluralize a word
		/// </summary>
		/// <param name="word"></param>
		/// <returns></returns>
        public static string Pluralize(string word)
        {
            CreatePluralizer();
            return (string)_pluralizationMethod.Invoke(_pluralizer, new object[] {word});
        }

		/// <summary>
		/// Setup Pluralizer for use
		/// </summary>
        public static void CreatePluralizer()
        {
            if (_pluralizer == null)
            {
                Assembly aseembly = typeof(DbContext).Assembly;
                var type =
                    aseembly.GetType(
                        "System.Data.Entity.ModelConfiguration.Design.PluralizationServices.EnglishPluralizationService");
                _pluralizer = Activator.CreateInstance(type, true);
                _pluralizationMethod = _pluralizer.GetType().GetMethod("Pluralize");
            }
        }
    }
}
