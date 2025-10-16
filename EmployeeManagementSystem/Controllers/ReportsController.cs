using EmployeeManagementSystem.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace EmployeeManagementSystem.Controllers
{
    public class ReportsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReportsController(ApplicationDbContext context)
        {
            _context = context;
        }
        //Головна сторінка звітів
        public IActionResult Index()
        {
            return View();
        }
        // Контракти, що закінчуються 30 днів
        public IActionResult ContractsEndingSoon()
        {
            var today = DateTime.Now;
            var nextMonth = today.AddMonths(1);

            var model = _context.Employees
                .Include(e => e.Department)
                .Include(e => e.Position)
                .Where(e => e.ContractEnd >= today && e.ContractEnd <= nextMonth)
                .ToList();

            return View(model);
        }

        //збереження в файл
        public IActionResult DownloadContractsEndingSoon()
        {
            var today = DateTime.Now;
            var nextMonth = today.AddMonths(1);

            var data = _context.Employees
                .Include(e => e.Department)
                .Include(e => e.Position)
                .Where(e => e.ContractEnd >= today && e.ContractEnd <= nextMonth)
                .ToList();         

            var lines = new List<string> { "FirstName,LastName,Department,Position,ContractEnd" };
            foreach (var e in data)
            {
                lines.Add($"{e.FirstName},{e.LastName},{e.Department?.Name},{e.Position?.Name},{e.ContractEnd:yyyy-MM-dd}");
            }

            return DownloadTxt(lines, "ContractsEnding30Days.txt");
        }

        //Кількість працівників по відділах
        public IActionResult EmployeesInDepartment()
        {
            var model = _context.Departments
                .Include(d => d.Employees)
                .ThenInclude(e => e.Position)
                .ToList();

            return View(model);
        }

        public IActionResult DownloadEmployeesInDepartment()
        {
            var departments = _context.Departments
                .Include(d => d.Employees)
                .ThenInclude(e => e.Position)
                .ToList();

            var lines = new List<string> { "Department,FirstName,LastName,Position" };
            foreach (var d in departments)
            {
                foreach (var e in d.Employees)
                {
                    lines.Add($"{d.Name},{e.FirstName},{e.LastName},{e.Position?.Name}");
                }
            }

            return DownloadTxt(lines, "EmployeesInDepartment.txt");
        }

        //Середня зарплата по відділах
        public IActionResult AverageSalaryInDepartment()
        {
            var model = _context.Employees
                .Include(e => e.Department)
                .GroupBy(e => e.Department!.Name)
                .Select(g => new
                {
                    DepartmentName = g.Key,
                    AvgSalary = g.Average(e => e.Salary)
                })
                .ToList();

            return View(model);
        }

        public IActionResult DownloadAverageSalaryInDepartment()
        {
            var data = _context.Employees
                .Include(e => e.Department)
                .GroupBy(e => e.Department!.Name)
                .Select(g => new
                {
                    DepartmentName = g.Key,
                    AvgSalary = g.Average(e => e.Salary)
                })
                .ToList();

            var lines = new List<string> { "Department,AverageSalary" };
            foreach (var d in data)
            {
                lines.Add($"{d.DepartmentName},{d.AvgSalary:F2}");
            }

            return DownloadTxt(lines, "AverageSalaryInDepartment.txt");
        }

        // ------------------ Допоміжний метод ------------------
        private FileResult DownloadTxt(List<string> lines, string fileName)
        {
            var content = string.Join(Environment.NewLine, lines);
            return File(Encoding.UTF8.GetBytes(content), "text/plain", fileName);
        }
    }
}
