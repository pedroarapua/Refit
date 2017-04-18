using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fusillade;
using Refit;
using Polly;

namespace Core
{
    [Headers("Accept: application/json")]
    public interface IUserService
    {
        [Get("")]
        Task<List<User>> FindAllOutro();
    }
}
