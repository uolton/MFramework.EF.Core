using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;

namespace MFramework.EF.Core.EntityConfigurations
{
    /// <summary>
    /// Configurazione base delle entita 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class EntityModelConfiguration<TEntity> : EntityTypeConfiguration<TEntity>, IEntityModelConfiguration
        where TEntity:class
    
    {
        /// <summary>
        /// Registra la configurazione nel model builder 
        /// </summary>
        
        public bool Register(DbModelBuilder modelbuilder)
        {
            modelbuilder.Configurations.Add(this);
            return true;
        }

        public Type EntityType { get { return typeof (TEntity); } }
    }
}
