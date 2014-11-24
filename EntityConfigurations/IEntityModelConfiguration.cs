using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFramework.EF.Core.EntityConfigurations
{
    public interface IEntityModelConfiguration
    {
        /// <summary>
        /// Registra l 'EntityTypeConfiguration nel Model
        /// </summary>
        /// <param name="modelbuilder"></param>
        /// <returns></returns>
        bool Register(DbModelBuilder modelbuilder);
        
        /// <summary>
        /// Tipo dell'entita che va a configurare 
        /// </summary>
        Type EntityType { get; }
    }
}
