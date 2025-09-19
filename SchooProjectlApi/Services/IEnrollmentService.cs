using SchooProjectlApi.Entities;

namespace SchooProjectlApi.Services
{
    public interface IEnrollmentService
    {
        Task<Enrollment> EnrollStudentAsync(Enrollment enrollment);
        Task<IEnumerable<Enrollment>> GetEnrollmentsAsync();
    }
}
