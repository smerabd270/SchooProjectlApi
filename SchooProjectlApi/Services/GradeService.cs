using Microsoft.EntityFrameworkCore;
using SchooProjectlApi.Data;
using SchooProjectlApi.Entities;

namespace SchooProjectlApi.Services
{
    public class GradeService : IGradeService
    {
        private readonly SchoolContext _context;
        public GradeService(SchoolContext context) => _context = context;

        public async Task<Grade?> AssignGradeAsync(int assignmentId, int studentId, int score, string? feedback, int teacherId)
        {
            // make sure assignment exists and belongs to this teacher
            var assignment = await _context.Assignments
                .Include(a => a.Course)
                .FirstOrDefaultAsync(a => a.Id == assignmentId);

            if (assignment == null || assignment.Course.TeacherId != teacherId)
                return null; // unauthorized or not found

            // check if grade already exists
            var existing = await _context.Grades
                .FirstOrDefaultAsync(g => g.AssignmentId == assignmentId && g.StudentId == studentId);

            if (existing != null)
            {
                existing.Score = score;
                existing.Feedback = feedback;
                await _context.SaveChangesAsync();
                return existing;
            }

            var grade = new Grade
            {
                AssignmentId = assignmentId,
                StudentId = studentId,
                Score = score,
                Feedback = feedback
            };

            _context.Grades.Add(grade);
            await _context.SaveChangesAsync();
            return grade;
        }

        public async Task<IEnumerable<Grade>> GetGradesByStudentAsync(int studentId) =>
            await _context.Grades
                .Where(g => g.StudentId == studentId)
                .Include(g => g.Assignment)
                .ThenInclude(a => a.Course)
                .ToListAsync();
    }
}
