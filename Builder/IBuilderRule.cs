using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFramework.EF.Core.Builder
{
    public interface IBuilderRule
    {
        void Build(DbModelBuilder builder);
    }
}
