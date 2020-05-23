package com.example.rest.web.service;

import com.example.rest.db.daoModel.Hotel;

import java.util.List;

public interface HotelService {

    void addHotel(Hotel hotel);

    void updateHotel(int id, Hotel hotel);

    void deleteHotel(int id);

    Hotel findHotelById(int id);

    List<Hotel> findAll();
}
