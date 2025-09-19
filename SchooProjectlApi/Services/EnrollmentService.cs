using Microsoft.EntityFrameworkCore;
using SchooProjectlApi.Data;
using SchooProjectlApi.Entities;

namespace SchooProjectlApi.Services
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly SchoolContext _context;
        public EnrollmentService(SchoolContext context) => _context = context;

        public async Task<Enrollment> EnrollStudentAsync(Enrollment enrollment)
        {
            _context.Enrollments.Add(enrollment);
            await _context.SaveChangesAsync();
            return enrollment;
        }

        public async Task<IEnumerable<Enrollment>> GetEnrollmentsAsync() =>
            await _context.Enrollments.Include(e => e.User).Include(e => e.Course).ToListAsync();
    }
}
