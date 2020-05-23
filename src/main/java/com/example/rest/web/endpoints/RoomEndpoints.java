package com.example.rest.web.endpoints;

import com.example.rest.db.daoModel.Room;
import com.example.rest.web.model.RoomModel;
import com.example.rest.web.service.RoomService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.MediaType;
import org.springframework.web.bind.annotation.*;

import java.util.List;

@RestController
@RequestMapping("/room")
public class RoomEndpoints {

    @Autowired
    private RoomService roomService;

    @RequestMapping(value = "/add", produces = MediaType.APPLICATION_JSON_VALUE, method = RequestMethod.POST)
    public void addRoom(@RequestBody RoomModel roomModel) {
        roomService.addRoom(roomModel);
    }

    @RequestMapping(value = "/update/{id}", produces = MediaType.APPLICATION_JSON_VALUE, method = RequestMethod.PUT)
    public void updateRoom(@PathVariable int id, @RequestBody RoomModel roomModel) {
        roomService.updateRoom(id, roomModel);
    }

    @RequestMapping(value = "/delete/{id}", produces = MediaType.APPLICATION_JSON_VALUE, method = RequestMethod.DELETE)
    public void deleteRoom(@PathVariable int id) {
        roomService.deleteRoom(id);
    }

    @RequestMapping(value = "/findById/{id}", produces = MediaType.APPLICATION_JSON_VALUE, method = RequestMethod.GET)
    public Room findRoomById(@PathVariable int id) {
        return roomService.findRoomById(id);
    }

    @RequestMapping(value = "/findAll", produces = MediaType.APPLICATION_JSON_VALUE, method = RequestMethod.GET)
    public List<Room> findRoomById() {
        return roomService.findAll();
    }

    @RequestMapping(value = "/findByRoomId/{id}", produces = MediaType.APPLICATION_JSON_VALUE, method = RequestMethod.GET)
    public List<Room> findAllRoomsByHotelId(@PathVariable int id) {
        return roomService.findAllRoomsByHotelId(id);
    }
}
