using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ImmersiveQuiz.Data;
using ImmersiveQuiz.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Components;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace ImmersiveQuiz.Controllers
{
    public class LocationsController : Controller
    {
        private readonly LocationContext _locationContext;
        private readonly QuestionContext _questionContext;
        private readonly AnswerContext _answerContext;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public LocationsController(LocationContext context, QuestionContext questionContext, AnswerContext answerContext, IWebHostEnvironment hostEnvironment)
        {
            _locationContext = context;
            _questionContext = questionContext;
            _answerContext = answerContext;
            _webHostEnvironment = hostEnvironment;
        }

        // GET: Locations
        public async Task<IActionResult> Index(string search)
        {
            var locations = from location in _locationContext.Location
                            select location;

            if (!string.IsNullOrEmpty(search))
            {
                locations = locations.Where(location => location.Name.Contains(search));
            }

            return View(await locations.ToListAsync());
        }

        // GET: Locations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var location = await _locationContext.Location
                .FirstOrDefaultAsync(m => m.LocationId == id);
            if (location == null)
            {
                return NotFound();
            }

            return View(location);
        }

        // GET: Locations/Create
        public IActionResult Create(int? id) // Passing in courseId
        {
            if (id == null)
            {
                return NotFound();
            }

            LocationImageViewModel vm = new LocationImageViewModel()
            {
                CourseId = id.Value
            };
            return View(vm);
        }

        // POST: Locations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LocationImageViewModel vm, int id)
        {
            if (ModelState.IsValid)
            {
                Location location = new Location()
                {
                    CourseId = id,
                    Name = vm.Name,
                    ImageGuid = UploadImage(vm.LocationImage),
                    ImageExtension = Path.GetExtension(vm.LocationImage.FileName)
                };
                _locationContext.Add(location);
                await _locationContext.SaveChangesAsync();
                return RedirectToAction("Details", "Courses", new { id = id });
            }
            return View(vm);
        }

        private Guid UploadImage(IFormFile image)
        {
            Guid imageGuid = Guid.NewGuid();
            string filePath = Path.Combine(Path.Combine(_webHostEnvironment.WebRootPath, "images"), imageGuid.ToString()) + Path.GetExtension(image.FileName);
            
            using var fileStream = new FileStream(filePath, FileMode.Create);
            image.CopyTo(fileStream);
            
            return imageGuid;
        }

        // GET: Locations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var location = await _locationContext.Location.FindAsync(id);
            if (location == null)
            {
                return NotFound();
            }

            
            LocationImageViewModel vm = new LocationImageViewModel()
            {
                CourseId = location.CourseId,
                LocationId = location.LocationId,
                Name = location.Name
            };

            return View(vm);
        }

        // POST: Locations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(LocationImageViewModel vm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _locationContext.Update(vm);
                    await _locationContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LocationExists(vm.LocationId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", "Courses", new { id = vm.CourseId });
            }
            return View(vm);
        }

        // GET: Locations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var location = await _locationContext.Location
                .FirstOrDefaultAsync(m => m.LocationId == id);
            if (location == null)
            {
                return NotFound();
            }

            return View(location);
        }

        // POST: Locations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var location = await _locationContext.Location.FindAsync(id);

            var questionsToLocations = _questionContext.Question.Where(q => q.LocationId == location.LocationId);
            foreach (var question in questionsToLocations)
            {
                var answersToQuestion = _answerContext.Answer.Where(ans => ans.QuestionId == question.QuestionId);
                _answerContext.Answer.RemoveRange(answersToQuestion);

                _questionContext.Remove(question);
            }
            _locationContext.Location.Remove(location);

            await _answerContext.SaveChangesAsync();
            await _questionContext.SaveChangesAsync();
            await _locationContext.SaveChangesAsync();

            return RedirectToAction("Details", "Courses", new { id = location.CourseId });
        }

        private bool LocationExists(int id)
        {
            return _locationContext.Location.Any(e => e.LocationId == id);
        }
    }
}
