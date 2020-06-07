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
		public DateTime StartTime {
			get => _startTime;
			set
			{
				_startTime = value;
				_endTime = _startTime.AddMinutes(duration);
			}
		}
		public DateTime EndTime {
			get => _endTime;
			set
			{
				throw new InvalidOperationException();
			}
		}
		public string roomName;
		public bool[,] availability;
		public int availableTickets;

		public ScreenTime(int movie, DateTime startTime, DateTime endTime, string roomName)
		{
			this.movie = movie;
			this.roomName = roomName;
			duration = new Repository<Movie>().Data[movie].duration;
			StartTime = startTime;

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
			duration = new Repository<Movie>().Data[movie].duration;
			this.startTime = startTime;
			this.roomName = roomName;
			this.availability = availability;
			this.availableTickets = availableTickets;
		}

		/// <summary>
		/// Reserves the seat on the ticket.
		/// </summary>
		/// <param name="ticket"></param>
		public void SetSeatAvailability(Ticket ticket, bool value)
		{
			if (!value && availability[ticket.row, ticket.seatnr] == false) throw new InvalidOperationException("Seat is not available");
			availability[ticket.row, ticket.seatnr] = value;
			availableTickets -= (!value) ? 1 : -1;
		}
	}

}

