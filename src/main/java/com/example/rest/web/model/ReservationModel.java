package com.example.rest.web.model;

import java.util.Date;

public class ReservationModel {

    private int roomId;
    private int userId;
    private Date reservationFrom;
    private Date reservationTo;

    public int getRoomId() {
        return roomId;
    }

    public void setRoomId(int roomId) {
        this.roomId = roomId;
    }

    public int getUserId() {
        return userId;
    }

    public void setUserId(int userId) {
        this.userId = userId;
    }

    public Date getReservationFrom() {
        return reservationFrom;
    }

    public void setReservationFrom(Date reservationFrom) {
        this.reservationFrom = reservationFrom;
    }

    public Date getReservationTo() {
        return reservationTo;
    }

    public void setReservationTo(Date reservationTo) {
        this.reservationTo = reservationTo;
    }
}
