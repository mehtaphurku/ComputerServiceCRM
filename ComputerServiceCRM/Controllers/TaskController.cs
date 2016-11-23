using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Data.Base;
using Data.IBase;
using Data.Models;
using Data.Repository;

namespace ComputerServiceCRM.Controllers
{
    public class TaskController : Controller
    {
        #region Variables

        private readonly TaskRepository _taskRepository;

        #endregion

        public TaskController()
        {
            IUnitOfWork uni = new UnitOfWork();
            _taskRepository = new TaskRepository(uni);
        }

        public ActionResult List()
        {
            if (Session["LoggedCustomerID"] != null)
            {
                IList<TaskData> list = _taskRepository.GetAll().Where(q => q.CustomerID == Convert.ToInt32(Session["LoggedCustomerID"])).ToList();
                return View(list);
            }
            else
            {
                return RedirectToAction("Logout", "Account");
            }
        }

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(TaskData data)
        {
            if (Session["LoggedCustomerID"] != null)
            {
                data.CustomerID = Convert.ToInt32(Session["LoggedCustomerID"]);
                data.CreateDate = DateTime.Now;
                _taskRepository.InsertObj(data);
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Logout", "Account");
            }

        }

        public ActionResult Edit(int taskId)
        {
            TaskData data = _taskRepository.FirstOrDefault(q => q.ID == taskId);
            if (data != null)
            {
                return View(data);
            }
            return RedirectToAction("List");
        }

        [HttpPost]
        public ActionResult Edit(TaskData task)
        {
            if (task != null)
               _taskRepository.Update(task);

            return RedirectToAction("List");
        }
        public ActionResult Delete(int taskId)
        {
            TaskData data = _taskRepository.FirstOrDefault(q => q.ID == taskId);
            if (data != null)
                _taskRepository.Delete(data);

            return RedirectToAction("List");
        }
    }
}