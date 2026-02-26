using Demo.DemoMVC.IService;
using Demo.DemoMVC.Service;
using Microsoft.AspNetCore.Mvc;

namespace Demo.DemoMVC.Controllers
{
    public class StudentController : Controller
    {

        /*ControllerBase doesnot have view
         * only used for api since it doesn't need View
         * */
        private readonly IStudentService studentService;
        public StudentController(IStudentService _studentService) {
            studentService = _studentService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            studentService.GetAllAsync();
            return View();
        }
    }
}
