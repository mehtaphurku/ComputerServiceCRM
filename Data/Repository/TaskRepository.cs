using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Base;
using Data.IBase;
using Data.Models;

namespace Data.Repository
{
    public class TaskRepository : BaseRepository<TaskData>
    {
        public TaskRepository(IUnitOfWork unit) : base(unit)
        {


        }
    }
}
