using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StackUnderFlow.Data;
using StackUnderFlow.Models;

namespace StackUnderFlow.Controllers
{
    public class AnswerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;


        public AnswerController(ApplicationDbContext context, UserManager<ApplicationUser> um)
        {
            _context = context;
            _userManager = um;
        }

        // GET: Answer
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Answers.Include(a => a.ApplicationUser).Include(a => a.Question);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Answer/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var answer = await _context.Answers
                .Include(a => a.ApplicationUser)
                .Include(a => a.Question)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (answer == null)
            {
                return NotFound();
            }

            return View(answer);
        }

        // GET: Answer/Create
        public IActionResult Create()
        {
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["QuestionId"] = new SelectList(_context.Questions, "Id", "Id");
            return View();
        }

        // POST: Answer/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([FromRoute]int id, [FromForm]string answer)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                var newAnswer = new Answer
                {
                     AnswerText = answer,
                     ApplicationUserId = user.Id,
                     QuestionId = id, 
                };
                _context.Answers.Add(newAnswer);
                Console.WriteLine($"{newAnswer.QuestionId}, {newAnswer.ApplicationUserId}");

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Details), "Questions", new { id = newAnswer.QuestionId });
            }
            return View(answer);
        }

        // GET: Answer/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var answer = await _context.Answers.SingleOrDefaultAsync(m => m.Id == id);
            if (answer == null)
            {
                return NotFound();
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "Id", answer.ApplicationUserId);
            ViewData["QuestionId"] = new SelectList(_context.Questions, "Id", "Id", answer.QuestionId);
            return View(answer);
        }

        // POST: Answer/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AnswerText,TimeAnswered,UpVote,DownVote,IsCorrectAnswer,ApplicationUserId,QuestionId")] Answer answer)
        {
            if (id != answer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(answer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AnswerExists(answer.Id))
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
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "Id", answer.ApplicationUserId);
            ViewData["QuestionId"] = new SelectList(_context.Questions, "Id", "Id", answer.QuestionId);
            return View(answer);
        }

        // GET: Answer/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var answer = await _context.Answers
                .Include(a => a.ApplicationUser)
                .Include(a => a.Question)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (answer == null)
            {
                return NotFound();
            }

            return View(answer);
        }

        // POST: Answer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var answer = await _context.Answers.SingleOrDefaultAsync(m => m.Id == id);
            _context.Answers.Remove(answer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AnswerExists(int id)
        {
            return _context.Answers.Any(e => e.Id == id);
        }
    }
}
