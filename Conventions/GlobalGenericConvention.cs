using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MFramework.Common.Specifications;

namespace MFramework.EF.Core.Conventions
{
    /// <summary>
    /// Esclude dal mapping determinate classi 
    /// </summary>
    public class GlobalGenericConvention<T>:GlobalConvention<T>
    {

        #region MemeberVariables

        private ISpecification<Type> _typesSelectionExp;
        private Action<ConventionTypeConfiguration, IEnumerable<T>> _configureAction;
        private Func<Type, IEnumerable<T>> _collectorFunc;
        #endregion
        
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="selection"></param>
        /// <param name="collectorFunc"></param>
        /// <param name="configureAction"></param>
        public GlobalGenericConvention(Specification<Type> selection,Func<Type, IEnumerable<T>> collectorFunc,Action<ConventionTypeConfiguration , IEnumerable<T>> configureAction)
        {
            _typesSelectionExp = selection;
            _configureAction = configureAction;
            _collectorFunc = collectorFunc;
        }

        /// <summary>
        /// Connette in and l'espressione di selezione con quella passata come parametro
        /// </summary>
        /// <param name="selection"></param>
        /// <returns></returns>
        public GlobalGenericConvention<T> And(Specification<Type> selection)
        {
            _typesSelectionExp = _typesSelectionExp.And(selection);
            return this;

        }
        /// <summary>
        /// Combina in Or l'espressione con quella passata come paramero
        /// </summary>
        /// <param name="selection"></param>
        /// <returns></returns>
        public GlobalGenericConvention<T> Or(Specification<Type> selection)
        {
            _typesSelectionExp= _typesSelectionExp.Or(selection);
            return this;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="convention"></param>
        /// <param name="selection"></param>
        /// <returns></returns>
        public static GlobalGenericConvention<T> operator &(GlobalGenericConvention<T> convention, Specification<Type> selection)
        {
            return convention.And(selection);
        }
        /// <summary>
        /// Operatore di cobinazione in And delle espressioni di selezione
        /// </summary>
        /// <param name="selection"></param>
        /// <param name="convention"></param>
        /// <returns></returns>
        public static GlobalGenericConvention<T> operator &(Specification<Type> selection,GlobalGenericConvention<T> convention )
        {
            return convention.And(selection);
        }
        /// <summary>
        /// Expression<Func<Type, bool>> selection
        /// </summary>
        /// <param name="convention">Convenzione</param>
        /// <param name="selection">criterio di selezione </param>
        /// <returns></returns>
        public static GlobalGenericConvention<T> operator |(GlobalGenericConvention<T> convention,Specification<Type> selection)
        {
            return convention.Or(selection);
        }
        /// <summary>
        /// Combina in Or l'espressioni di selezione
        /// </summary>
        /// <param name="selection">Criterio di selezione</param>
        /// <param name="convention">Convention</param>
        /// <returns></returns>
        public static GlobalGenericConvention<T> operator |(Specification<Type> selection,GlobalGenericConvention<T> convention)
        {
            return convention.Or(selection);
        }


        protected override IEnumerable<T> Having(Type t)
        {
            return(_typesSelectionExp.IsSatisfiedBy(t)?null:_collectorFunc(t));
        }

        protected override void Configure(ConventionTypeConfiguration conventionTypeConfiguration, IEnumerable<T> enumerable)
        {
            _configureAction(conventionTypeConfiguration,enumerable);
        }
    }
}
