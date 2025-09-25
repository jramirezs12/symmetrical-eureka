using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RulesEngine.Application.InputSources
{
    public interface IInputSource<TEntity>
    {
        Task<List<TEntity>> Create();
    }
}
