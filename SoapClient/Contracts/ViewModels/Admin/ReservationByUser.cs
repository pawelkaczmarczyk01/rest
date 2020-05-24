﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.ViewModels.Admin
{
    public class ReservationByUser
    {
        public int ReservationId { get; set; }
        public int RoomId { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string Information { get; set; }
        public string DateFromString
        {
            get
            {
                return DateFrom.ToString("dd.MM.yyyy");
            }
        }
        public string DateToString
        {
            get
            {
                return DateTo.ToString("dd.MM.yyyy");
            }
        }

        public ReservationByUser(
            int reservationId,
            int roomId,
            DateTime dateFrom,
            DateTime dateTo)
        {
            ReservationId = reservationId;
            RoomId = roomId;
            DateFrom = dateFrom;
            DateTo = dateTo;
            Information = "Rezerwacja nr #" + ReservationId + "|" + RoomId;
        }
    }
}
