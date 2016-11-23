using Data.Base;
using Data.IBase;
using Data.Models;

namespace Data.Repository
{
    public class UserRepository : BaseRepository<UserData>
    {
        public UserRepository(IUnitOfWork unit) : base(unit)
        {


        }
    }
}
