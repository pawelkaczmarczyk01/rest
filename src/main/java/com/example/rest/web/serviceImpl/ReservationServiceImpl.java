package com.example.rest.web.serviceImpl;

import com.example.rest.db.dao.ReservationDAO;
import com.example.rest.db.dao.RoomDAO;
import com.example.rest.db.daoModel.Reservation;
import com.example.rest.db.daoModel.Room;
import com.example.rest.db.daoModel.User;
import com.example.rest.web.model.ReservationModel;
import com.example.rest.web.service.ReservationService;
import org.hibernate.service.spi.ServiceException;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.util.List;
import java.util.stream.Collectors;

@Service
@Transactional
public class ReservationServiceImpl implements ReservationService {

    @Autowired
    private ReservationDAO reservationDAO;
    @Autowired
    private RoomDAO roomDAO;

    @Override
    public void addReservation(ReservationModel reservationModel) {
        Room room = roomDAO.findRoomById(reservationModel.getRoomId());

        if (room != null) {
            Reservation reservation = new Reservation()
                    .withUserId(new User().withId(reservationModel.getUserId()))
                    .withRoomId(new Room().withId(reservationModel.getRoomId()))
                    .withReservationFrom(reservationModel.getReservationFrom())
                    .withReservationTo(reservationModel.getReservationTo());
            if (reservation.getReservationFrom().getTime() > reservation.getRoomReservationTo().getTime()) {
                throw new ServiceException("Date from can't be lover than date to");
            }
            if (reservation.getReservationFrom().getTime() == reservation.getRoomReservationTo().getTime()) {
                throw new ServiceException("Date from can't be equals date to");
            }

            List<Reservation> reservationList = reservationDAO.findAll();
            reservationList = reservationList.stream().filter(roomId -> roomId.getRoomId().getId().equals(reservationModel.getRoomId())).collect(Collectors.toList());

            for (Reservation reservationFromDb : reservationList) {
                if (reservation.getReservationFrom().getTime() <= reservationFromDb.getReservationFrom().getTime() && reservation.getRoomReservationTo().getTime() <= reservationFromDb.getReservationFrom().getTime()) {
                    System.out.println();
                } else if (reservation.getReservationFrom().getTime() >= reservationFromDb.getRoomReservationTo().getTime() && reservation.getRoomReservationTo().getTime() >= reservationFromDb.getRoomReservationTo().getTime()) {
                    System.out.println();
                } else {
                    throw new ServiceException("This Room is reserved in this time");
                }
            }
            reservationDAO.save(reservation);
        } else {
            throw new ServiceException("This Room is not existed");
        }
    }

    @Override
    public void deleteReservation(int userId, int reservationId) {
        Reservation reservation = reservationDAO.findReservationById(reservationId);
        if (reservation != null) {
            if (reservation.getUserId().getId() == userId) {
                reservationDAO.deleteReservationById(reservationId);
            } else {
                throw new ServiceException("Access denied !!");
            }
        } else {
            throw new ServiceException("ReservationModel with given id is not existed");
        }
    }

    @Override
    public Reservation findReservationById(int userId, int reservationId) {
        Reservation reservation = reservationDAO.findReservationById(reservationId);
        if (reservation.getUserId().getId().equals(userId)) {
            return reservation;
        } else {
            throw new ServiceException("Access denied !!");
        }
    }

    @Override
    public List<Reservation> findAll() {
        return reservationDAO.findAll();
    }

    @Override
    public List<Reservation> findAllReservationByUserId(int userId) {
        return reservationDAO.findAll().stream().filter(reservation -> reservation.getUserId().getId().equals(userId)).collect(Collectors.toList());
    }
}
