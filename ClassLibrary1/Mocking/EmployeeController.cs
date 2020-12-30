using System.Data.Entity;

namespace ClassLibrary1.Mocking
{
    public class EmployeeController
    {
        private readonly IEmployeeStorage _storage;

        public EmployeeController(IEmployeeStorage employeeStorage = null)
        {
            _storage = employeeStorage ?? new EmployeeStorage();
        }

        public ActionResult DeleteEmployee(int id)
        {
            // all that responsibility is encapsulated inside our storage
            _storage.DeleteEmployee(id);
            return RedirectToAction("Employees");
        }

        private ActionResult RedirectToAction(string employees)
        {
            return new RedirectResult();
        }
    }

    public class ActionResult
    {
    }

    public class RedirectResult : ActionResult
    {
    }

    public class EmployeeContext
    {
        public DbSet<Employee> Employees { get; set; }

        public void SaveChanges()
        {
        }
    }

    public class Employee
    {
    }
}