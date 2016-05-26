using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebArchitecture.Data;
using WebArchitecture.Entities.Models;

namespace WebArchitecture.Service
{
    public interface IStudentService : IService<Student>
    {
        int StudentCountByStatus(string status);
    }

    public class StudentService : Service<Student>, IStudentService
    {
        private readonly IRepository<Student> _studentRepository;

        public StudentService(IRepository<Student> studentRepository) : base(studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public int StudentCountByStatus(string status)
        {
            throw new NotImplementedException();
        }

    }
}
