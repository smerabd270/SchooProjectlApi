using Microsoft.EntityFrameworkCore;
using SchooProjectlApi.Data;
using SchooProjectlApi.Entities;

namespace SchooProjectlApi.Services
{
    public class GradeService : IGradeService
    {
        private readonly SchoolContext _context;
        public GradeService(SchoolContext context) => _context = context;

        public async Task<Grade> AddGradeAsync(Grade grade)
        {
            _context.Grades.Add(grade);
            await _context.SaveChangesAsync();
            return grade;
        }

        public async Task<IEnumerable<Grade>> GetGradesByStudentAsync(int studentId) =>
            await _context.Grades
                .Where(g => g.StudentId == studentId)
                .Include(g => g.Assignment)
                .ToListAsync();
    }
}
