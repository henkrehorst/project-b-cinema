using System;
using System.Collections.Generic;
using bioscoop_app.Service;
using Newtonsoft.Json;
using System.Linq;
using bioscoop_app.Repository;

namespace bioscoop_app.Model
{
	public class ScreenTime : DataType
	{
		public int movie;
		public int duration;
		private DateTime _startTime;
		private DateTime _endTime;
		public DateTime startTime {
			get => _startTime;
			set
			{
				throw new InvalidOperationException("Use ScreenTime.SetTimes method to modify start and end times");
			}
		}
		public DateTime endTime {
			get => _endTime;
			set
			{
				throw new InvalidOperationException();
			}
		}
		public string roomName;
		public bool[,] availability;
		public int availableTickets;

		public ScreenTime(int movie, DateTime startTime, string roomName)
		{
			this.movie = movie;
			this.roomName = roomName;
			int duration = new Repository<Movie>().Data[movie].duration;
			SetTimes(startTime, duration);
			availability = RoomLayoutService.GetInitialAvailability(roomName);
			foreach (bool val in availability)
			{
				availableTickets += val ? 1 : 0;
			}
		}

		[JsonConstructor]
		public ScreenTime(int id, int movie, DateTime startTime, DateTime endTime, string roomName, bool[,] availability, int availableTickets)
		{
			Id = id;
			this.movie = movie;
			_startTime = startTime;
			_endTime = endTime;
			this.roomName = roomName;
			this.availability = availability;
			this.availableTickets = availableTickets;
		}

		/// <summary>
		/// Setter for start and endtimes
		/// </summary>
		/// <param name="startTime">Start time of the ScreenTime</param>
		/// <param name="duration">Duration of the movie</param>
		public void SetTimes(DateTime startTime, int duration)
		{
			_startTime = startTime;
			_endTime = startTime.AddMinutes(duration);
		}

		/// <summary>
		/// Reserves the seat on the ticket.
		/// </summary>
		/// <param name="ticket"></param>
		/// <exception cref="InvalidOperationException">If the seat is not available.</exception>
		public void SetSeatAvailability(Ticket ticket, bool value)
		{
			if (!value && availability[ticket.row, ticket.seatnr] == false) throw new InvalidOperationException("Seat is not available");
			availability[ticket.row, ticket.seatnr] = value;
			availableTickets -= (!value) ? 1 : -1;
		}
	}

}

