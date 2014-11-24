using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MFramework.Common.Core.Collections.Extensions;

namespace MFramework.EF.Core.Builder.Rules
{
    /// <summary>
    /// Aggregazione di piu Rules
    /// </summary>
    public class CompositeRule : IBuilderRule
    {
        private List<IBuilderRule> _rules;

        /// <summary>
        /// Inizializza una Composite Rules
        /// </summary>
        public CompositeRule()
        {
            _rules= new List<IBuilderRule>();
        }
        /// <summary>
        /// Inizializza una CompositeRule con un elenco di Rules
        /// </summary>
        /// <param name="e"></param>
        public CompositeRule(IEnumerable<IBuilderRule> e)
        {
            e.PushIn(_rules);
        }

        public CompositeRule Add(IBuilderRule r)
        {
            _rules.Add(r);
            return this;
        }
        public CompositeRule Remove(IBuilderRule r)
        {
            _rules.Remove(r);
            return this;
        }
        /// <summary>
        /// Esegue la configurazione del builder richiamando la configurazione delle singole rule
        /// </summary>
        /// <param name="builder"></param>
        public void Build(DbModelBuilder builder)
        {
            _rules.ForEach( r => r.Build(builder));
        }
    }
}
