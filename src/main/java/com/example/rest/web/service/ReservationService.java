package com.example.rest.web.service;

import com.example.rest.db.daoModel.Reservation;
import com.example.rest.web.model.ReservationModel;

import java.util.List;

public interface ReservationService {

    void addReservation(ReservationModel reservationModel);

    void deleteReservation(int userId, int reservationId);

    Reservation findReservationById(int userId, int reservationId);

    List<Reservation> findAll();

    List<Reservation> findAllReservationByUserId(int userId);
}
