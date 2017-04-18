using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fusillade;
//using PropertyChanged;

namespace Core
{
    public class UserController
    {
        //private UsersService _userService;

        public UserController()
        {
            // _userService = new UsersService();
        }

        public List<UserDto> Users { get; set; }



        private void LoadAndCacheSpectulativeUsers(List<UserDto> users)
        {
            foreach (var id in users.Select(x => x.Id))
            {
                // _userService.GetUser(Priority.Speculative, id);
            }
        }
    }
}