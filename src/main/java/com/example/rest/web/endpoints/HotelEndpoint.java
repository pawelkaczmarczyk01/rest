package com.example.rest.web.endpoints;

import com.example.rest.db.daoModel.Hotel;
import com.example.rest.web.service.HotelService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.hateoas.CollectionModel;
import org.springframework.http.MediaType;
import org.springframework.web.bind.annotation.*;

import java.util.List;

@RestController
@RequestMapping("/hotel")
public class HotelEndpoint {

    @Autowired
    private HotelService hotelService;

    @RequestMapping(value = "/add", produces = MediaType.APPLICATION_JSON_VALUE, method = RequestMethod.POST)
    public void addHotel(@RequestBody Hotel hotel) {
        hotelService.addHotel(hotel);
    }

    @RequestMapping(value = "/update/{id}", produces = MediaType.APPLICATION_JSON_VALUE, method = RequestMethod.PUT)
    public void updateHotel(@PathVariable int id, @RequestBody Hotel hotel) {
        hotelService.updateHotel(id, hotel);
    }

    @RequestMapping(value = "/delete/{id}", produces = MediaType.APPLICATION_JSON_VALUE, method = RequestMethod.DELETE)
    public void deleteHotel(@PathVariable int id) {
        hotelService.deleteHotel(id);
    }

    @RequestMapping(value = "/findById/{id}", produces = MediaType.APPLICATION_JSON_VALUE, method = RequestMethod.GET)
    public Hotel findHotelById(@PathVariable int id) {
        return hotelService.findHotelById(id);
    }

    @RequestMapping(value = "/findAll", produces = MediaType.APPLICATION_JSON_VALUE, method = RequestMethod.GET)
    public List<Hotel> findHotelById() {
        return hotelService.findAll();
    }

    @RequestMapping(value = "/findAllHATEOAS", produces = MediaType.APPLICATION_JSON_VALUE, method = RequestMethod.GET)
    public CollectionModel<Hotel> findHotelHATEOAS() {
        return hotelService.findAllHATEOAS();
    }
}
