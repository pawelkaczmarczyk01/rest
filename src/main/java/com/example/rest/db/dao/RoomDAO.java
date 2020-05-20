package com.example.rest.db.dao;

import com.example.rest.db.daoModel.Room;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

@Repository
public interface RoomDAO extends JpaRepository<Room, Integer> {

    Room findRoomById(int id);

    void deleteRoomById(int id);
}
