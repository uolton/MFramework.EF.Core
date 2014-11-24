using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MFramework.Common.Core.Types.Extensions;

namespace MFramework.EF.Core.Builder.Rules
{
    /// <summary>
    /// 
    /// </summary>
    public class BuilderIgnoreRule:IBuilderRule
    {
        private Type _typeToIgnore;
        private BuilderIgnoreRule(Type typeToIgnore)
        {
            _typeToIgnore = typeToIgnore;
        }
        public void Build(DbModelBuilder builder)
        {
            // Faccio il close e la chiamata al metodo generico Ignore passando come parametro il tipo da escludere da mapping 
            builder.CloseAndInvokeGenericMethod(modelBuilder => modelBuilder.Ignore<object>(), new[] {_typeToIgnore});
        }

        public static BuilderIgnoreRule Ignore<T>()
        {
            return Ignore(typeof(T));
        }
        public static BuilderIgnoreRule Ignore(Type t)
        {
            return new BuilderIgnoreRule(t);
        }
    }
}
