package com.example.rest.web.endpoints;

import com.example.rest.db.daoModel.Reservation;
import com.example.rest.web.model.ReservationModel;
import com.example.rest.web.service.ReservationService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.MediaType;
import org.springframework.web.bind.annotation.*;

import java.util.List;

@RestController
@RequestMapping("/reservation")
public class ReservationEndpoints {

    @Autowired
    private ReservationService reservationService;

    @RequestMapping(value = "/add", produces = MediaType.APPLICATION_JSON_VALUE, method = RequestMethod.POST)
    public void addReservation(@RequestBody ReservationModel reservationModel) {
        reservationService.addReservation(reservationModel);
    }

    @RequestMapping(value = "/delete/{id}", produces = MediaType.APPLICATION_JSON_VALUE, method = RequestMethod.DELETE)
    public void deleteReservation(int userId, @PathVariable int id) {
        reservationService.deleteReservation(userId, id);
    }

    @RequestMapping(value = "/findById/{id}", produces = MediaType.APPLICATION_JSON_VALUE, method = RequestMethod.GET)
    public Reservation findReservationById(int userId, @PathVariable int id) {
        return reservationService.findReservationById(userId, id);
    }

    @RequestMapping(value = "/findAll", produces = MediaType.APPLICATION_JSON_VALUE, method = RequestMethod.GET)
    public List<Reservation> findReservationById() {
        return reservationService.findAll();
    }

    @RequestMapping(value = "/findByRoomId/{userId}", produces = MediaType.APPLICATION_JSON_VALUE, method = RequestMethod.GET)
    public List<Reservation> findAllReservationByHotelId(@PathVariable int userId) {
        return reservationService.findAllReservationByUserId(userId);
    }
}
