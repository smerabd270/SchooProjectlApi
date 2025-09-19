using Microsoft.EntityFrameworkCore;
using SchooProjectlApi.Data;
using SchooProjectlApi.Entities;

namespace SchooProjectlApi.Services
{
    public class AssignmentService : IAssignmentService
    {
        private readonly SchoolContext _context;
        public AssignmentService(SchoolContext context) => _context = context;

        public async Task<IEnumerable<Assignment>> GetAssignmentsByCourseAsync(int courseId) =>
            await _context.Assignments.Where(a => a.CourseId == courseId).ToListAsync();

        public async Task<Assignment> CreateAssignmentAsync(Assignment assignment)
        {
            _context.Assignments.Add(assignment);
            await _context.SaveChangesAsync();
            return assignment;
        }
    }
}
