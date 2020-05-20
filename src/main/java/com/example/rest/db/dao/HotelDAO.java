package com.example.rest.db.dao;

import com.example.rest.db.daoModel.Hotel;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

@Repository
public interface HotelDAO extends JpaRepository<Hotel, Integer> {

    Hotel findHotelById(int id);

    Hotel findHotelByHotelName(String hotelName);

    void deleteHotelById(int id);
}
