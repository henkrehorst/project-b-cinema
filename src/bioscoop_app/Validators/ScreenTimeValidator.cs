using bioscoop_app.Model;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace bioscoop_app.Validators
{
    class ScreenTimeValidator : AbstractValidator<ScreenTime>
    {
        private static readonly DateTime _minTime = new DateTime(2020, 1, 1);
        public ScreenTimeValidator(bool futureTime = true)
        {
            RuleFor(screenTime => screenTime.availability).NotNull()
                .Must(map => map.Rank == 2 && map.Length > 0);
            RuleFor(screenTime => screenTime.availableTickets).GreaterThanOrEqualTo(0);
            RuleFor(screenTime => screenTime.movie).GreaterThan(0);
            RuleFor(screenTime => screenTime.roomName)
                .Must(roomName => Array.IndexOf(new string[] { "auditorium1", "auditorium2", "auditorium3" }, roomName) != -1);
            RuleFor(screenTime => screenTime.startTime).NotNull()
                .Must(time => time.CompareTo(futureTime ? DateTime.Now : _minTime) >= 0);
            RuleFor(screenTime => screenTime)
                .Must(screenTime => screenTime.endTime.CompareTo(screenTime.startTime) > 0);
        }
    }
}
