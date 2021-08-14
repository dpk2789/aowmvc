using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AowCore.Domain.Payroll;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Data;
using ExcelDataReader;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using AowCore.AppWeb.ViewModels;
using AowCore.AppWeb.Helpers;
using AowCore.Application;


namespace AowCore.AppWeb.Areas.MyBooks.Controllers
{
    [Area("MyBooks")]
    public class EmpAttendencesController : Controller
    {
        private readonly IApplicationDbContext _context;
        private IWebHostEnvironment _environment;
        private readonly ICookieHelper _cookieHelper;
        public EmpAttendencesController(IApplicationDbContext context, IWebHostEnvironment environment, ICookieHelper cookieHelper)
        {
            _context = context;
            _environment = environment;
            _cookieHelper = cookieHelper;
        }

        public async Task<IActionResult> Upload()
        {
            var applicationDbContext = _context.EmpAttendences.Include(e => e.EmployeeDetail);
            return View(await applicationDbContext.ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Import()
        {
            var resultForClient = new JsonResultClientSide();
            try
            {
                IFormFile file = Request.Form.Files[0];
                string path = Path.Combine(this._environment.WebRootPath, "Uploads");
                string fileName = file.FileName;
                string fileContentType = file.ContentType;
                var fileLocation = new FileInfo(path);
                FileStream stream = new FileStream(path, FileMode.Create);
                file.CopyTo(stream);

                IExcelDataReader reader = null;
                if (file.FileName.EndsWith(".xls"))
                {
                    reader = ExcelReaderFactory.CreateBinaryReader(stream);
                }
                else if (file.FileName.EndsWith(".xlsx"))
                {
                    reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                }
                else
                {
                    ModelState.AddModelError("File", "This file format is not supported");
                    return View();
                }

                int i = 0;
                string dateTimeColumnName = string.Empty;
                string enNoColumnName = string.Empty;
                DateTime date = DateTime.Now;
                DataSet result = reader.AsDataSet();

                var cmpid = _cookieHelper.Get("cmpCookee");
                if (cmpid == null)
                {
                    return Redirect("/");
                }
                var cmpidG = Guid.Parse(cmpid);
                var salaryLedger = _context.Ledgers.Include(x=>x.LedgerCategory).Where(x => x.Name == "Salary & Wages" && x.LedgerCategory.CompanyId == cmpidG).FirstOrDefault();
                foreach (DataRow row in result.Tables[0].Rows)
                {
                    string rowEnrNo = row.ItemArray[2].ToString();
                    string rowPunchTime = row.ItemArray[6].ToString();
                    EmpAttendence att = new EmpAttendence();
                    var emp = new EmployeeDetail();
                    att.Id = Guid.NewGuid();
                    string empFullName = string.Empty;
                    foreach (DataColumn col in result.Tables[0].Columns)
                    {
                        var value = row[col.ColumnName].ToString();

                        if (i == 0)
                        {

                        }
                        else
                        {
                            DateTime punchDate = Convert.ToDateTime(rowPunchTime);
                            var retriveAtt = _context.EmpAttendences.Where(x => x.EnrollmentNumber == rowEnrNo && x.PunchTime == punchDate).FirstOrDefault();
                            if (retriveAtt == null)
                            {
                                if (col.ColumnName == "Column0")
                                {
                                    if (!string.IsNullOrEmpty(value))
                                    {
                                        att.SrNo = Convert.ToInt16(value);
                                    }
                                }
                                if (col.ColumnName == "Column1")
                                {
                                    att.MachineNumber = value;
                                }
                                if (col.ColumnName == "Column2")
                                {
                                    var retriveEmp = _context.EmployeeDetails.Where(x => x.EnrollmentNumber == value).FirstOrDefault();
                                    Guid empId = Guid.NewGuid();
                                    if (retriveEmp == null)
                                    {
                                        emp.Id = empId;
                                        emp.EnrollmentNumber = value;
                                        emp.LedgerId = salaryLedger.Id;
                                        emp.CreatedDate = DateTime.Now;
                                        att.EmployeeDetailId = empId;
                                        _context.EmployeeDetails.Add(emp);
                                    }
                                    else
                                    {
                                        att.EmployeeDetailId = retriveEmp.Id;
                                    }

                                    att.EnrollmentNumber = value;
                                }
                                if (col.ColumnName == "Column3")
                                {
                                    emp.FullName = value;
                                    att.FullName = value;
                                }
                                if (col.ColumnName == "Column4")
                                {
                                    att.Mode = value;
                                }
                                if (col.ColumnName == "Column5")
                                {

                                }
                                if (col.ColumnName == "Column6")
                                {
                                    att.PunchTime = punchDate;
                                    att.CreatedDate = DateTime.Now;
                                    _context.EmpAttendences.Add(att);
                                    await _context.SaveChangesAsync();
                                }
                            }

                        }
                    }
                    i++;
                }
                reader.Close();
                return Json(new { resultForClient });
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
                ModelState.AddModelError(string.Empty, ex.Message);
                resultForClient.Msg = ex.InnerException.Message;

            }
            return Json(new { resultForClient });

        }

        public async Task<IActionResult> Report()
        {
            ViewData["EmployeeDetails"] = new SelectList(await _context.EmployeeDetails.OrderBy(x => x.FullName).ToListAsync(), "Id", "FullName");
            return View();
        }

        public async Task<ActionResult> DisplaySearchResults(Guid? Id, DateTime? todate, DateTime? fromdate)
        {
            var filesDispatchViewModelList = new List<EmpAttendenceViewModel>();
            IEnumerable<EmpAttendenceViewModel> filesDispatchViewModelEnu;

            if (Id != null)
            {
                var attendences = await _context.EmpAttendences.Where(x => x.EmployeeDetailId == Id).ToListAsync();
                // var uniq = attendences.GroupBy(x => x.PunchTime.Date).Select(row => row).Distinct();
                //var uniq = from x in attendences select x.FullName + ", " + x.SrNo;
                // var data = (from c in attendences select c).AsEnumerable();

                if (todate != null || fromdate != null)
                {
                    var tofromList = attendences.Where(d1 => d1.PunchTime >= todate && d1.PunchTime <= fromdate).
                            OrderByDescending(d => d.PunchTime).ToList();

                    var data = tofromList.AsEnumerable().GroupBy(x => x.PunchTime.Date).Select(g => g.First()).ToList();
                    foreach (var att in data)
                    {
                        TimeSpan inTime = DateTime.Now.TimeOfDay;
                        TimeSpan outTime = DateTime.Now.TimeOfDay;
                        TimeSpan timespan;

                        int x = 0;
                        int hours = 0;
                        int minutes = 0;
                        var empAttendenceViewModel = new EmpAttendenceViewModel();
                        empAttendenceViewModel.FullName = att.FullName;
                        empAttendenceViewModel.SrNo = att.SrNo;
                        empAttendenceViewModel.Date = att.PunchTime;
                        empAttendenceViewModel.EnrollmentNumber = att.EnrollmentNumber;
                        var inOutDates = attendences.Where(x => x.PunchTime.Date == att.PunchTime.Date);
                        foreach (var date in inOutDates.OrderBy(x => x.PunchTime.TimeOfDay))
                        {
                            if (x == 0)
                            {
                                inTime = date.PunchTime.TimeOfDay;
                                empAttendenceViewModel.InTime = inTime;
                            }
                            else
                            {
                                outTime = date.PunchTime.TimeOfDay;
                                empAttendenceViewModel.OutTime = outTime;
                                timespan = outTime - inTime;
                                hours = timespan.Hours;
                                minutes = timespan.Minutes;
                            }
                            x++;
                        }
                        empAttendenceViewModel.WorkMinutes = minutes;
                        empAttendenceViewModel.WorkHourTotal = hours;
                        filesDispatchViewModelList.Add(empAttendenceViewModel);
                    }
                }
                else
                {
                    var data = attendences.AsEnumerable().GroupBy(x => x.PunchTime.Date).Select(g => g.First()).ToList();
                    foreach (var att in data)
                    {
                        TimeSpan inTime = DateTime.Now.TimeOfDay;
                        TimeSpan outTime = DateTime.Now.TimeOfDay;
                        TimeSpan timespan;

                        int x = 0;
                        int hours = 0;
                        int minutes = 0;
                        var empAttendenceViewModel = new EmpAttendenceViewModel();
                        empAttendenceViewModel.FullName = att.FullName;
                        empAttendenceViewModel.SrNo = att.SrNo;
                        empAttendenceViewModel.Date = att.PunchTime;
                        empAttendenceViewModel.EnrollmentNumber = att.EnrollmentNumber;
                        var inOutDates = attendences.Where(x => x.PunchTime.Date == att.PunchTime.Date);
                        foreach (var date in inOutDates.OrderBy(x => x.PunchTime.TimeOfDay))
                        {
                            if (x == 0)
                            {
                                inTime = date.PunchTime.TimeOfDay;
                                empAttendenceViewModel.InTime = inTime;
                            }
                            else
                            {
                                outTime = date.PunchTime.TimeOfDay;
                                empAttendenceViewModel.OutTime = outTime;
                                timespan = outTime - inTime;
                                hours = timespan.Hours;
                                minutes = timespan.Minutes;
                            }
                            x++;
                        }
                        empAttendenceViewModel.WorkMinutes = minutes;
                        empAttendenceViewModel.WorkHourTotal = hours;
                        filesDispatchViewModelList.Add(empAttendenceViewModel);
                    }
                }

                filesDispatchViewModelEnu = filesDispatchViewModelList;
                //  return PartialView("_FilterDaily", violationViewModelEnu);              
                string modelString = await this.RenderViewAsync("_EmpAttendenceReport", filesDispatchViewModelEnu, true);
                return Json(new { success = true, modelString });
            }

            else
            {
                filesDispatchViewModelEnu = filesDispatchViewModelList;
            }
            return Json(new { success = true, filesDispatchViewModelEnu });
        }

        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.EmpAttendences.Include(e => e.EmployeeDetail);
            return View(await applicationDbContext.ToListAsync());
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empAttendence = await _context.EmpAttendences
                .Include(e => e.EmployeeDetail)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (empAttendence == null)
            {
                return NotFound();
            }

            return View(empAttendence);
        }

        // GET: MyBooks/EmpAttendences/Create
        public IActionResult Create()
        {
            ViewData["LedgerId"] = new SelectList(_context.Ledgers, "Id", "Id");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SrNo,MachineNumber,FullName,EnrollmentNumber,Mode,Date,PunchTime,LedgerId,Id")] EmpAttendence empAttendence)
        {
            if (ModelState.IsValid)
            {
                empAttendence.Id = Guid.NewGuid();
                _context.Add(empAttendence);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["LedgerId"] = new SelectList(_context.Ledgers, "Id", "Id", empAttendence.EmployeeDetail);
            return View(empAttendence);
        }

        // GET: MyBooks/EmpAttendences/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empAttendence = await _context.EmpAttendences.FindAsync(id);
            if (empAttendence == null)
            {
                return NotFound();
            }
            ViewData["LedgerId"] = new SelectList(_context.Ledgers, "Id", "Id", empAttendence.EmployeeDetail);
            return View(empAttendence);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("SrNo,MachineNumber,FullName,EnrollmentNumber,Mode,Date,PunchTime,LedgerId,Id")] EmpAttendence empAttendence)
        {
            if (id != empAttendence.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(empAttendence);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmpAttendenceExists(empAttendence.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["LedgerId"] = new SelectList(_context.Ledgers, "Id", "Id", empAttendence.EmployeeDetailId);
            return View(empAttendence);
        }

        // GET: MyBooks/EmpAttendences/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empAttendence = await _context.EmpAttendences
                .Include(e => e.EmployeeDetail)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (empAttendence == null)
            {
                return NotFound();
            }

            return View(empAttendence);
        }

        // POST: MyBooks/EmpAttendences/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var empAttendence = await _context.EmpAttendences.FindAsync(id);
            _context.EmpAttendences.Remove(empAttendence);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmpAttendenceExists(Guid id)
        {
            return _context.EmpAttendences.Any(e => e.Id == id);
        }
    }
}
