using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Refit;

namespace Core
{

    [Headers("Accept: application/json")]
    public interface IUserApi
    {
        [Get("/users")]
        Task<List<UserDto>> GetUsers();

        [Get("/users/{id}")]
        Task<UserDto> GetUser(string id);
    }
}
