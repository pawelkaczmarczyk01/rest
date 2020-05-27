package com.example.rest.web.serviceImpl;

import com.example.rest.db.dao.AssortmentDAO;
import com.example.rest.db.dao.HotelDAO;
import com.example.rest.db.dao.RoomDAO;
import com.example.rest.db.daoModel.Hotel;
import com.example.rest.db.daoModel.Room;
import com.example.rest.web.service.HotelService;
import org.hibernate.service.spi.ServiceException;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.hateoas.CollectionModel;
import org.springframework.hateoas.Link;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.util.List;
import java.util.Optional;
import java.util.stream.Collectors;

import static org.springframework.hateoas.server.mvc.WebMvcLinkBuilder.linkTo;

@Service
@Transactional
public class HotelServiceImpl implements HotelService {

    @Autowired
    private HotelDAO hotelDAO;
    @Autowired
    private RoomDAO roomDAO;
    @Autowired
    private AssortmentDAO assortmentDAO;

    @Override
    public void addHotel(Hotel hotel) {
        Hotel hotelFromDb = hotelDAO.findHotelByHotelName(hotel.getHotelName());
        if (hotelFromDb == null) {
            hotelDAO.save(hotel);
        } else {
            throw new ServiceException("Hotel name must be unique");
        }
    }

    @Override
    public void updateHotel(int id, Hotel hotel) {
        Hotel hotelFromDb = Optional.ofNullable(hotelDAO.findHotelByHotelName(hotel.getHotelName())).orElse(new Hotel());

        if (hotelFromDb.getId() == null) {
            hotelFromDb = hotelDAO.findHotelById(id);
            if (hotelFromDb != null) {
                if (!hotel.getHotelName().isEmpty() && hotel.getHotelName() != null) {
                    hotelFromDb.setHotelName(hotel.getHotelName());
                }
                if (!hotel.getHotelImagePath().isEmpty() && hotel.getHotelImagePath() != null) {
                    hotelFromDb.setHotelImagePath(hotel.getHotelImagePath());
                }
                hotelDAO.save(hotelFromDb);
            } else {
                throw new ServiceException("Hotel with given id is not existed");
            }
        } else {
            throw new ServiceException("Hotel name must be unique");
        }
    }

    @Override
    public void deleteHotel(int id) {
        if (hotelDAO.findHotelById(id) != null) {
            List<Room> roomListFromDb = roomDAO.findAll();
            roomListFromDb = roomListFromDb.stream().filter(hotelId -> hotelId.getHotelId().getId().equals(id)).collect(Collectors.toList());

            for (Room roomFromDb : roomListFromDb) {
                assortmentDAO.deleteAssortmentById(roomFromDb.getAssortmentId().getId());
                roomDAO.deleteRoomById(roomFromDb.getId());
            }

            hotelDAO.deleteHotelById(id);
        } else {
            throw new ServiceException("Hotel with given id is not existed");
        }
    }

    @Override
    public Hotel findHotelById(int id) {
        return hotelDAO.findHotelById(id);
    }

    @Override
    public List<Hotel> findAll() {
        return hotelDAO.findAll();
    }

    @Override
    public CollectionModel<Hotel> findAllHATEOAS() {

        List<Hotel> allHotels = hotelDAO.findAll();

        for (Hotel hotel : allHotels) {
            String hotelId = hotel.getId().toString();
            Link selfLink = linkTo(HotelServiceImpl.class).slash(hotelId).withSelfRel();
            hotel.add(selfLink);
        }

        Link link = linkTo(HotelServiceImpl.class).withSelfRel();
        CollectionModel<Hotel> result = new CollectionModel<>(allHotels, link);

        return result;
    }
}
