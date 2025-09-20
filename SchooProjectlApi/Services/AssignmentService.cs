using Microsoft.EntityFrameworkCore;
using SchooProjectlApi.Data;
using SchooProjectlApi.Entities;

namespace SchooProjectlApi.Services
{
    public class AssignmentService : IAssignmentService
    {
        private readonly SchoolContext _context;
        public AssignmentService(SchoolContext context) => _context = context;

        // Teacher adds a new assignment
        public async Task<Assignment> CreateAsync(Assignment assignment, int teacherId)
        {
            // Ensure the course belongs to this teacher
            var course = await _context.Courses.FirstOrDefaultAsync(c => c.Id == assignment.CourseId && c.TeacherId == teacherId);
            if (course == null) throw new UnauthorizedAccessException("You cannot add assignment to this course.");

            _context.Assignments.Add(assignment);
            await _context.SaveChangesAsync();
            return assignment;
        }

        // Get assignment by id
        public async Task<Assignment?> GetByIdAsync(int id) =>
            await _context.Assignments
                .Include(a => a.Course)
                .FirstOrDefaultAsync(a => a.Id == id);

        // Student submits assignment
        public async Task<bool> SubmitAsync(int assignmentId, int studentId)
        {
            var assignment = await _context.Assignments.FindAsync(assignmentId);
            if (assignment == null) return false;

            // Check if already submitted
            var existingGrade = await _context.Grades
                .FirstOrDefaultAsync(g => g.AssignmentId == assignmentId && g.StudentId == studentId);

            if (existingGrade != null)
            {
                existingGrade.Submitted = true;
                existingGrade.SubmittedAt = DateTime.UtcNow;
            }
            else
            {
                // Create grade record to track submission
                _context.Grades.Add(new Grade
                {
                    AssignmentId = assignmentId,
                    StudentId = studentId,
                    Submitted = true,
                    SubmittedAt = DateTime.UtcNow
                });
            }

            await _context.SaveChangesAsync();
            return true;
        }

        // Get assignments for a course
        public async Task<IEnumerable<Assignment>> GetAssignmentsByCourseAsync(int courseId) =>
            await _context.Assignments.Where(a => a.CourseId == courseId).ToListAsync();
    }
}
