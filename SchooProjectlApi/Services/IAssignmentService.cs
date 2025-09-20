using SchooProjectlApi.Entities;

namespace SchooProjectlApi.Services
{
    public interface IAssignmentService
    {
        Task<Assignment> CreateAsync(Assignment assignment, int teacherId);
        Task<Assignment?> GetByIdAsync(int id);
        Task<bool> SubmitAsync(int assignmentId, int studentId);
        Task<IEnumerable<Assignment>> GetAssignmentsByCourseAsync(int courseId);
    }
}
