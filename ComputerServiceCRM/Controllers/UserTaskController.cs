using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Data.Base;
using Data.IBase;
using Data.Models;
using Data.Repository;

namespace ComputerServiceCRM.Controllers
{
    public class UserTaskController : Controller
    {
        #region Variables

        private readonly TaskRepository _taskRepository;

        #endregion

        public UserTaskController()
        {
            IUnitOfWork uni = new UnitOfWork();
            _taskRepository = new TaskRepository(uni);
        }

        public ActionResult List()
        {
            if (Session["LoggedUserID"] == null)
            {
                return RedirectToAction("Logout", "Account");
            }
            else
            {
                IList<TaskData> list = _taskRepository.GetAll();
                return View(list);

            }
        }

        public ActionResult Edit(int taskId)
        {
            TaskData data = _taskRepository.FirstOrDefault(q => q.ID == taskId);
            return View(data);
        }
        [HttpPost]
        public ActionResult Edit(TaskData postData)
        {
            TaskData entity = _taskRepository.FirstOrDefault(q => q.ID == postData.ID);
            if (entity != null)
            {
                entity.CompletingUserID = Convert.ToInt32(Session["LoggedUserID"]);
                entity.CompletingNote = postData.CompletingNote;
                _taskRepository.Update(entity);
            }
           
            return RedirectToAction("List");
        }
    }
}