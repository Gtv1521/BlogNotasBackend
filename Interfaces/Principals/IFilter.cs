using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogNotasBackend.Interfaces.Principals
{
    public interface IFilter<T>
    {
        Task<IEnumerable<T>> Filter(string filter, string id); //filtra datos 
    }
}