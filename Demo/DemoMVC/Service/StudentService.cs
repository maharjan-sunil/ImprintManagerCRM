using Demo.DemoMVC.IService;
using Demo.DemoMVC.Model;
using Demo.HubR.Interface;
using System;
using static Demo.DemoMVC.Service.StudentService;

namespace Demo.DemoMVC.Service
{

    public class StudentService: IStudentService
    {
        private readonly INotificationService _notificationService;

        public StudentService(INotificationService notificationService) {
            _notificationService = notificationService;
        }


        //public async Task Handle(OrderCreatedEvent @event)
        //{
        //    await _notificationService
        //        .SendOrderCreatedAsync(@event.UserId, @event.OrderId);
        //}


        public Task<StudentModel> CreateAsync(StudentModel dto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<StudentModel>> GetAllAsync()
        {
            await _notificationService.SendOrderCreatedAsync("you have a new notification");
            return new List<StudentModel>
        {
            new StudentModel { StudentId = 1, StudentName = "John" },
            new StudentModel { StudentId = 2, StudentName = "Jane" }
        };
        }

        public Task<StudentModel?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(int id, StudentModel dto)
        {
            throw new NotImplementedException();
        }

    }
}
