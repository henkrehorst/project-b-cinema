using System;
using System.Collections.Generic;
using bioscoop_app.Service;
using Newtonsoft.Json;
using System.Linq;

namespace bioscoop_app.Model
{
	public class ScreenTime : DataType
	{
		public int movie;
		public DateTime startTime;
		public DateTime endTime;
		public string roomName;
		public bool[,] availability;
		public int availableTickets;

		public ScreenTime(int movie, DateTime startTime, DateTime endTime, string roomName)
		{
			this.movie = movie;
			this.startTime = startTime;
			this.endTime = endTime;
			this.roomName = roomName;
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
			this.startTime = startTime;
			this.endTime = endTime;
			this.roomName = roomName;
			this.availability = availability;
			this.availableTickets = availableTickets;
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

