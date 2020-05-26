using bioscoop_app.Model;
using FluentValidation;

namespace bioscoop_app.Validator
{
    public class ScreenTimeValidator : AbstractValidator<ScreenTime>
    {
        public ScreenTimeValidator()
        {
            RuleFor(x => x.movie).NotEmpty().WithMessage("Movie naam ontbreekt!");
            RuleFor(x => x.startTime).NotEmpty().WithMessage("Start time ontbreekt");
            RuleFor(x => x.endTime).NotEmpty().WithMessage("End time ontbreekt");
            RuleFor(x => x.roomName).NotEmpty().WithMessage("Room name ontbreekt");

            
        }

        private bool ValidateRoomType(string roomType)
        {
            return roomType == "auditorium1" || roomType == "auditorium2" || roomType == "auditorium3";
        }
    }
}