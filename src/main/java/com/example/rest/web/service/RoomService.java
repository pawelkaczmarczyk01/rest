package com.example.rest.web.service;

import com.example.rest.db.daoModel.Room;
import com.example.rest.web.model.RoomModel;

import java.util.List;

public interface RoomService {

    void addRoom(RoomModel roomModel);

    void updateRoom(int id, RoomModel roomModel);

    void deleteRoom(int id);

    Room findRoomById(int id);

    List<Room> findAll();

    List<Room> findAllRoomsByHotelId(int id);
}
