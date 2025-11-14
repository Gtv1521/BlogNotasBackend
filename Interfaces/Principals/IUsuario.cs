using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackEndNotes.Interfaces.Regulars;
using Microsoft.OpenApi.Validations;

namespace BackEndNotes.Interfaces.Principals
{
    public interface IUsuario<D, M> :  IViewOne<M>, IRemove, IUpdate<D>
    {
        Task<IEnumerable<M>> ViewUserEmail(string email);   
    }
}