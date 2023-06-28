using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_API.Data.Interface
{
    public interface ISqlHelperRepository
    {
        public Task<int> ChangesOnData<T>(string spName, T parameters);
    }
}
