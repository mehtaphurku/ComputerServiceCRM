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
    public class AccountController : Controller
    {
        #region Variables

        private readonly CustomerRepository _customerRepository;
        private readonly UserRepository _userRepository;

        #endregion

        public AccountController()
        {
            IUnitOfWork uni = new UnitOfWork();
            _customerRepository = new CustomerRepository(uni);
            _userRepository = new UserRepository(uni);
        }

        public ActionResult ListCustomer()
        {
            IList<CustomerData> list = _customerRepository.GetAll();
            return View(list);
        }

        public ActionResult CreateCustomer()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateCustomer(CustomerData customerData)
        {
            CustomerData validCustomer = _customerRepository.FirstOrDefault(q => q.Email == customerData.Email);
            if (validCustomer != null)
            {
                ViewBag.Alert = "E-Posta adresine ait kullanıcı bulunmaktadır.";
                return View();
            }
            else
            {
                customerData.CreatedAt = DateTime.Now;
                _customerRepository.InsertObj(customerData);
            }
            return RedirectToAction("ListCustomer");
        }

        public ActionResult EditCustomer(int customerId)
        {
            CustomerData customerData = _customerRepository.FirstOrDefault(q => q.ID == customerId);
            if (customerData == null)
                return RedirectToAction("ListCustomer");
            else
                return View(customerData);
        }

        [HttpPost]
        public ActionResult EditCustomer(CustomerData customerData)
        {
            if (customerData != null)
                _customerRepository.Update(customerData);

            return RedirectToAction("ListCustomer");
        }

        public ActionResult DeleteCustomer(int customerId)
        {
            CustomerData customerData = _customerRepository.FirstOrDefault(q => q.ID == customerId);
            if (customerData != null)
                _customerRepository.Delete(customerData);

            return RedirectToAction("ListCustomer");
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            CustomerData customer = _customerRepository.FirstOrDefault(q => q.Email == email && q.Password == password);
            if (customer != null)
            {
                Session["LoggedCustomerID"] = customer.ID;
                Session["LoggedCustomerFullName"] = customer.FirstName + " " + customer.LastName;
                Response.Cookies.Add(new HttpCookie("LoggedCustomer", customer.ID.ToString()));

                return RedirectToAction("List", "Task");
            }
            else
            {
                UserData user = _userRepository.FirstOrDefault(q => q.Email == email && q.Password == password);
                if (user != null)
                {
                    Session["LoggedUserID"] = user.ID;
                    Session["LoggedUserFullName"] = user.FirstName + " " + user.LastName;
                    Response.Cookies.Add(new HttpCookie("LoggedUser", user.ID.ToString()));
                    return RedirectToAction("List", "UserTask");
                }
                else
                {
                    ViewBag.Alert = "Kullanıcı bilgileri hatalı.";
                }
            }

            return View();
        }

        public ActionResult Logout()
        {
            Session["LoggedCustomerID"] = null;
            Session["LoggedCustomerFullName"] = null;
            Session["LoggedUserID"] = null;
            Session["LoggedUserFullName"] = null;

            return RedirectToAction("Login");
        }

        public ActionResult ListUser()
        {
            IList<UserData> list = _userRepository.GetAll();
            return View(list);
        }

        public ActionResult CreateUser()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateUser(UserData data)
        {
            if (data != null)
            {
                data.CreatedAt=DateTime.Now;
                _userRepository.InsertObj(data);
            }
            return RedirectToAction("ListUser");
        }
    }
}