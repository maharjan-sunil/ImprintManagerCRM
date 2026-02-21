using Microsoft.AspNetCore.Mvc;

namespace Demo.DemoMVC.Controllers
{
    public class StudentController : Controller
    {

        /*ControllerBase doesnot have view
         * only used for api since it doesn't need View
         * */

        public StudentController() { }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }
    }
}
